using Small.Net.Reflection;
using System;
using System.Collections;
using System.Linq;

namespace Small.Net.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Copies to.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static T2 CopyTo<T1,T2>(this T1 obj) where T1: class where T2 : class, new()
        {
            var t2Type = typeof(T2);
            return (T2) obj.CopyTo(t2Type);
        }

        /// <summary>
        /// Copy an object to a specific type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="toType"></param>
        /// <returns></returns>
        public static object CopyTo(this object obj, Type toType)
        {
            var t1Type = obj.GetType();
            if (t1Type == toType && obj is ICloneable cloneable) return cloneable.Clone();
            var helper = toType.GetObjectReflectionHelper();
            var getters = t1Type.GetObjectReflectionHelper().GetProperties(PropertyType.Getter);
            var setters = helper.GetProperties(PropertyType.Setter);
            var copy = helper.CreateInstance();
            foreach (var getter in getters)
            {
                if (!setters.ContainsKey(getter.Key)) continue;
                var value = getter.Value.GetValue(obj);
                var propHelper = setters[getter.Key];
                
                if (!(value is IEnumerable enumerable))
                {
                    propHelper.SetValue(copy, value);
                    continue;
                }

                /* We have a collection of object to duplicate */
                var newValue = propHelper.PropertyType.GetObjectReflectionHelper().CreateInstance();
                if (!(newValue is IList collection)) throw new InvalidOperationException($"The destination property {propHelper.Name} is not a collection");
                var collectionType = collection.GetType().GetGenericArguments()?.FirstOrDefault();
                foreach (var objInList in enumerable)
                {
                    var newObjForList = objInList.CopyTo(collectionType ?? objInList.GetType());
                    collection.Add(newObjForList);
                }
                propHelper.SetValue(copy, newValue);
            }
            return copy;
        }
    }
}
