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
        public static T2 CopyTo<T1, T2>(this T1 obj) where T1 : class where T2 : class, new()
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
            var helper = (IObjectConverter) obj.GetType().GetObjectReflectionHelper().Converter;
            return helper.CreateObjectFrom(obj, toType);
        }
    }
}