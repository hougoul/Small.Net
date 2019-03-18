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

        /// <summary>
        /// Converts the value.
        /// </summary>
        /// <param name="conversionType">Type of the conversion.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static object ConvertValue(this Type conversionType, object value)
        {
            if (conversionType.IsNullableType())
            {
                if (value == null || Convert.IsDBNull(value)) return null;
                conversionType = conversionType.GetEnumUnderlyingType();
            }
            var valueType = value.GetType();
            if (valueType == conversionType) return value;
            Func<object> conversion = () => Convert.ChangeType(value, conversionType);
            if (conversionType == typeof(string)) conversion = () => value.ToString();
            //else if(conversionType.IsNumericType() && valueType.IsNumericType())
            else if (conversionType.IsEnum) conversion = () => (valueType == typeof(string) ? Enum.Parse(conversionType, (string)value, true) : (Enum.IsDefined(conversionType, value) ? Enum.ToObject(conversionType, value) : throw new InvalidCastException($"Cannot cast {value} to {conversionType}")));
            return conversion();
        }

        private static Type NullableType = typeof(Nullable<>);
        /// <summary>
        /// Determines whether [is nullable type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is nullable type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == NullableType;
        }
        /// <summary>
        /// Determines whether [is numeric type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is numeric type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

    }
}
