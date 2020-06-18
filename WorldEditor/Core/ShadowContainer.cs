using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace WorldEditor.Core
{
    /// <summary>
    /// The ShadowContainer is the main container used internally to keep references to all native COM/C++ callbacks.
    /// It is stored in the property <see cref="ICallbackable.Shadow"/>.
    /// </summary>
    internal class ShadowContainer : Disposable
    {
        private readonly Dictionary<Guid, CppObjectShadow> guidToShadow = new Dictionary<Guid, CppObjectShadow>();

        private static readonly Dictionary<Type, List<Type>> typeToShadowTypes = new Dictionary<Type, List<Type>>();

        private IntPtr guidPtr;
        public IntPtr[] Guids { get; private set; }

        public void Initialize(ICallbackable callbackable)
        {
            callbackable.Shadow = this;

            var type = callbackable.GetType();
            List<Type> slimInterfaces;

            // Cache reflection on COM interface inheritance
            lock (typeToShadowTypes)
            {
                if (!typeToShadowTypes.TryGetValue(type, out slimInterfaces))
                {
#if BEFORE_NET45
                    var interfaces = type.GetTypeInfo().GetInterfaces();
#else
                    var interfaces = type.GetTypeInfo().ImplementedInterfaces;
#endif
                    slimInterfaces = new List<Type>();
                    slimInterfaces.AddRange(interfaces);
                    typeToShadowTypes.Add(type, slimInterfaces);

                    // First pass to identify most detailed interfaces
                    foreach (var item in interfaces)
                    {
                        // Only process interfaces that are using shadow
                        var shadowAttribute = ShadowAttribute.Get(item);
                        if (shadowAttribute == null)
                        {
                            slimInterfaces.Remove(item);
                            continue;
                        }

                        // Keep only final interfaces and not intermediate.
#if BEFORE_NET45
                        var inheritList = item.GetTypeInfo().GetInterfaces();
#else
                        var inheritList = item.GetTypeInfo().ImplementedInterfaces;
#endif
                        foreach (var inheritInterface in inheritList)
                        {
                            slimInterfaces.Remove(inheritInterface);
                        }
                    }
                }
            }

            CppObjectShadow iunknownShadow = null;

            // Second pass to instantiate shadow
            foreach (var item in slimInterfaces)
            {
                // Only process interfaces that are using shadow
                var shadowAttribute = ShadowAttribute.Get(item);

                // Initialize the shadow with the callback
                var shadow = (CppObjectShadow) Activator.CreateInstance(shadowAttribute.Type);
                shadow.Initialize(callbackable);

                // Take the first shadow as the main IUnknown
                if (iunknownShadow == null)
                {
                    iunknownShadow = shadow;
                    // Add IUnknown as a supported interface
                    guidToShadow.Add(ComObjectShadow.IID_IUnknown, iunknownShadow);
                }

                guidToShadow.Add(Utilities.GetGuidFromType(item), shadow);

                // Associate also inherited interface to this shadow

                var inheritList = item.GetTypeInfo().ImplementedInterfaces;

                foreach (var inheritInterface in inheritList)
                {
                    var inheritShadowAttribute = ShadowAttribute.Get(inheritInterface);
                    if (inheritShadowAttribute == null)
                        continue;

                    // Use same shadow as derived
                    guidToShadow.Add(Utilities.GetGuidFromType(inheritInterface), shadow);
                }
            }

            // Precalculate the list of GUID without IUnknown and IInspectable
            // Used for WinRT 
            int countGuids = 0;
            foreach (var guidKey in guidToShadow.Keys)
            {
                if (guidKey != Utilities.GetGuidFromType(typeof(IInspectable)) &&
                    guidKey != Utilities.GetGuidFromType(typeof(IUnknown)))
                    countGuids++;
            }

            guidPtr = Marshal.AllocHGlobal(Utilities.SizeOf<Guid>() * countGuids);
            Guids = new IntPtr[countGuids];
            int i = 0;
            unsafe
            {
                var pGuid = (Guid*) guidPtr;
                foreach (var guidKey in guidToShadow.Keys)
                {
                    if (guidKey == Utilities.GetGuidFromType(typeof(IInspectable)) ||
                        guidKey == Utilities.GetGuidFromType(typeof(IUnknown)))
                        continue;

                    pGuid[i] = guidKey;
                    // Store the pointer
                    Guids[i] = new IntPtr(pGuid + i);
                    i++;
                }
            }
        }

        internal IntPtr Find(Type type)
        {
            return Find(Utilities.GetGuidFromType(type));
        }

        internal IntPtr Find(Guid guidType)
        {
            var shadow = FindShadow(guidType);
            return (shadow == null) ? IntPtr.Zero : shadow.NativePointer;
        }

        internal CppObjectShadow FindShadow(Guid guidType)
        {
            CppObjectShadow shadow;
            guidToShadow.TryGetValue(guidType, out shadow);
            return shadow;
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var comObjectCallbackNative in guidToShadow.Values)
                    comObjectCallbackNative.Dispose();
                guidToShadow.Clear();

                if (guidPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(guidPtr);
                    guidPtr = IntPtr.Zero;
                }
            }
        }
    }
}