using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq;

namespace Small.Net.Reflection
{
    public static class ReflectionExtensions
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, IGetterSetter>> GetterSetterCache = new ConcurrentDictionary<Type, Dictionary<string, IGetterSetter>>();

        public static IReadOnlyDictionary<string, IGetterSetter> GetPropertiesAccessor(this Type objType)
        {
            if (!GetterSetterCache.TryGetValue(objType, out var properties))
            {
                properties = new Dictionary<string, IGetterSetter>(StringComparer.OrdinalIgnoreCase);
                var propertiesInfo = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                if (propertiesInfo != null)
                {
                    var getterSetterType = typeof(FastGetterSetter<>).MakeGenericType(objType);
                    foreach(var propInfo in propertiesInfo)
                    {
                        var getterSetter = (IGetterSetter)Activator.CreateInstance(getterSetterType, propInfo);
                        properties.Add(propInfo.Name, getterSetter);
                    }
                }
                GetterSetterCache.TryAdd(objType, properties);
            }
            return properties;
        }
    }
}
