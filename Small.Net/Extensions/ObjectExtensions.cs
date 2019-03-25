using Small.Net.Reflection;
using System;
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
            var t1Type = typeof(T1);
            var t2Type = typeof(T2);
            if (t1Type == t2Type && obj is ICloneable cloneable) return (T2)cloneable.Clone();
            var helper = t2Type.GetObjectReflectionHelper();
            var _getters = t1Type.GetObjectReflectionHelper().GetProperties(PropertyType.Getter);
            var _setters = helper.GetProperties(PropertyType.Setter);
            var copy =(T2)helper.CreateInstance();
            foreach (var getter in _getters)
            {
                if (_setters.ContainsKey(getter.Key))
                    _setters[getter.Key].SetValue(copy, getter.Value.GetValue(obj));
            }
            return copy;
        }
    }
}
