using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace WorldEditor.Core.Diagnostics
{
    /// <summary>
    /// Track all allocated objects.
    /// </summary>
    public static class ObjectTracker
    {
        private static Dictionary<IntPtr, List<ObjectReference>> processGlobalObjectReferences;

        [ThreadStatic] private static Dictionary<IntPtr, List<ObjectReference>> threadStaticObjectReferences;

        /// <summary>
        /// Occurs when a ComObject is tracked.
        /// </summary>
        public static event EventHandler<ComObjectEventArgs> Tracked;

        /// <summary>
        /// Occurs when a ComObject is untracked.
        /// </summary>
        public static event EventHandler<ComObjectEventArgs> UnTracked;

        /// <summary>
        /// Function which provides stack trace for object tracking.
        /// </summary>
        public static Func<string> StackTraceProvider = GetStackTrace;

        private static Dictionary<IntPtr, List<ObjectReference>> ObjectReferences
        {
            get
            {
                Dictionary<IntPtr, List<ObjectReference>> objectReferences;

                if (Configuration.UseThreadStaticObjectTracking)
                {
                    if (threadStaticObjectReferences == null)
                        threadStaticObjectReferences =
                            new Dictionary<IntPtr, List<ObjectReference>>(EqualityComparer.DefaultIntPtr);

                    objectReferences = threadStaticObjectReferences;
                }
                else
                {
                    if (processGlobalObjectReferences == null)
                        processGlobalObjectReferences =
                            new Dictionary<IntPtr, List<ObjectReference>>(EqualityComparer.DefaultIntPtr);

                    objectReferences = processGlobalObjectReferences;
                }

                return objectReferences;
            }
        }

        /// <summary>
        /// Gets default stack trace.
        /// </summary>
        public static string GetStackTrace()
        {
#if WINDOWS_UWP
            var stacktrace = "Stacktrace is not available on this platform";

            // This code is a workaround to be able to get a full stacktrace on Windows Store App. 
            // This is an unsafe code, that should not run on production. Only at dev time!
            // Make sure we are on a 32bit process
            if (IntPtr.Size == 4)
            {
                // Get an access to a restricted method
                try
                {
                    var stackTraceGetMethod = typeof(Environment).GetRuntimeProperty("StackTrace").GetMethod;
                    try
                    {
                        // First try to get the stacktrace
                        stacktrace = (string) stackTraceGetMethod.Invoke(null, null);
                    }
                    catch (Exception)
                    {
                        // If we have an exception, it means that the access to the method is not possible
                        // so we are going to patch the field RuntimeMethodInfo.m_invocationFlags that should contain
                        // 0x41 (initialized + security), and replace it by 0x1 (initialized)
                        // and then callback again the method
                        unsafe
                        {
                            // unsafe code, the RuntimeMethodInfo could be relocated (is it a real managed GC object?), 
                            // but we don't have much choice
                            var addr = *(int**) Interop.Fixed(ref stackTraceGetMethod);
                            // offset to RuntimeMethodInfo.m_invocationFlags
                            addr += 13;
                            // Check if we have the expecting value
                            if (*addr == 0x41)
                            {
                                // if yes, change it to 0x1
                                *addr = 0x1;
                                try
                                {
                                    // And try to callit again a second time
                                    // if it succeeds, first Invoke() should run on next call
                                    stacktrace = (string) stackTraceGetMethod.Invoke(null, null);
                                }
                                catch (Exception)
                                {
                                    // if it is still failing, we can't do anything
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // can't do anything
                }
            }

            return stacktrace;
#else
            // Another WTF: To get a stacktrace, we don't have other ways than throwing an exception on PCL. 
            try
            {
                throw new GetStackTraceException();
            }
            catch (GetStackTraceException ex)
            {
                return ex.StackTrace;
            }
#endif
        }

        /// <summary>
        /// Tracks the specified COM object.
        /// </summary>
        /// <param name="comObject">The COM object.</param>
        public static void Track(ComObject comObject)
        {
            if (comObject == null || comObject.NativePointer == IntPtr.Zero)
                return;
            lock (ObjectReferences)
            {
                // Object is already tracked
                if (!ObjectReferences.TryGetValue(comObject.NativePointer, out List<ObjectReference> referenceList))
                {
                    referenceList = new List<ObjectReference>();
                    ObjectReferences.Add(comObject.NativePointer, referenceList);
                }

                referenceList.Add(new ObjectReference(DateTime.Now, comObject,
                    StackTraceProvider != null ? StackTraceProvider() : String.Empty));

                // Fire Tracked event.
                OnTracked(comObject);
            }
        }

        /// <summary>
        /// Finds a list of object reference from a specified COM object pointer.
        /// </summary>
        /// <param name="comObjectPtr">The COM object pointer.</param>
        /// <returns>A list of object reference</returns>
        public static List<ObjectReference> Find(IntPtr comObjectPtr)
        {
            lock (ObjectReferences)
            {
                // Object is already tracked
                if (ObjectReferences.TryGetValue(comObjectPtr, out var referenceList))
                    return new List<ObjectReference>(referenceList);
            }

            return new List<ObjectReference>();
        }

        /// <summary>
        /// Finds the object reference for a specific COM object.
        /// </summary>
        /// <param name="comObject">The COM object.</param>
        /// <returns>An object reference</returns>
        public static ObjectReference Find(ComObject comObject)
        {
            lock (ObjectReferences)
            {
                // Object is already tracked
                if (ObjectReferences.TryGetValue(comObject.NativePointer, out var referenceList))
                {
                    foreach (var objectReference in referenceList)
                    {
                        if (ReferenceEquals(objectReference.Object.Target, comObject))
                            return objectReference;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Untracks the specified COM object.
        /// </summary>
        /// <param name="comObject">The COM object.</param>
        public static void UnTrack(ComObject comObject)
        {
            if (comObject == null || comObject.NativePointer == IntPtr.Zero)
                return;

            lock (ObjectReferences)
            {
                // Object is already tracked
                if (ObjectReferences.TryGetValue(comObject.NativePointer, out List<ObjectReference> referenceList))
                {
                    for (int i = referenceList.Count - 1; i >= 0; i--)
                    {
                        var objectReference = referenceList[i];
                        if (ReferenceEquals(objectReference.Object.Target, comObject))
                            referenceList.RemoveAt(i);
                        else if (!objectReference.IsAlive)
                            referenceList.RemoveAt(i);
                    }

                    // Remove empty list
                    if (referenceList.Count == 0)
                        ObjectReferences.Remove(comObject.NativePointer);

                    // Fire UnTracked event
                    OnUnTracked(comObject);
                }
            }
        }

        /// <summary>
        /// Reports all COM object that are active and not yet disposed.
        /// </summary>
        public static List<ObjectReference> FindActiveObjects()
        {
            var activeObjects = new List<ObjectReference>();
            lock (ObjectReferences)
            {
                foreach (var referenceList in ObjectReferences.Values)
                {
                    foreach (var objectReference in referenceList)
                    {
                        if (objectReference.IsAlive)
                            activeObjects.Add(objectReference);
                    }
                }
            }

            return activeObjects;
        }

        /// <summary>
        /// Reports all COM object that are active and not yet disposed.
        /// </summary>
        public static string ReportActiveObjects()
        {
            var text = new StringBuilder();
            int count = 0;
            var countPerType = new Dictionary<string, int>();

            foreach (var findActiveObject in FindActiveObjects())
            {
                var findActiveObjectStr = findActiveObject.ToString();
                if (!string.IsNullOrEmpty(findActiveObjectStr))
                {
                    text.AppendFormat("[{0}]: {1}", count, findActiveObjectStr);

                    var target = findActiveObject.Object.Target;
                    if (target != null)
                    {
                        var targetType = target.GetType().Name;
                        if (!countPerType.TryGetValue(targetType, out int typeCount))
                        {
                            countPerType[targetType] = 0;
                        }
                        else
                            countPerType[targetType] = typeCount + 1;
                    }
                }

                count++;
            }

            var keys = new List<string>(countPerType.Keys);
            keys.Sort();

            text.AppendLine();
            text.AppendLine("Count per Type:");
            foreach (var key in keys)
            {
                text.AppendFormat("{0} : {1}", key, countPerType[key]);
                text.AppendLine();
            }

            return text.ToString();
        }

        private static void OnTracked(ComObject obj)
        {
            Tracked?.Invoke(null, new ComObjectEventArgs(obj));
        }

        private static void OnUnTracked(ComObject obj)
        {
            UnTracked?.Invoke(null, new ComObjectEventArgs(obj));
        }

        private class GetStackTraceException : Exception
        {
        }
    }
}