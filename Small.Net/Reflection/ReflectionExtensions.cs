using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Small.Net.Reflection
{
    public static class ReflectionExtensions
    {
        private static readonly ConcurrentDictionary<Type, IReflectionObject> GetterSetterCache =
            new ConcurrentDictionary<Type, IReflectionObject>();

        public static IReflectionObject GetObjectReflectionHelper(this Type objType)
        {
            if (GetterSetterCache.TryGetValue(objType, out var reflectionObject)) return reflectionObject;
            reflectionObject =
                (IReflectionObject) Activator.CreateInstance(typeof(ReflectionObject<>).MakeGenericType(objType));

            GetterSetterCache.TryAdd(objType, reflectionObject);
            return reflectionObject;
        }

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once ConvertToConstant.Global
        public static string StringToBoolTrueValue = "Y";

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once ConvertToConstant.Global
        public static char CharToBoolTrueValue = 'Y';

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
                conversionType = Nullable.GetUnderlyingType(conversionType);
            }

            Debug.Assert(conversionType != null);
            var valueType = value.GetType();
            if (valueType == conversionType) return value;
            Func<object> conversion = () => Convert.ChangeType(value, conversionType);
            if (conversionType == typeof(string)) conversion = () => Convert.ToString(value);
            else if (conversionType.IsNumericType() && valueType.IsNumericType())
                conversion = () => ConvertNumber(conversionType, (dynamic) value);
            else if (conversionType == typeof(bool))
                conversion = () =>
                    (valueType == typeof(string)
                        ? (string) value == StringToBoolTrueValue || Convert.ToBoolean(value)
                        : (valueType == typeof(char) ? (char) value == CharToBoolTrueValue : Convert.ToBoolean(value)));
            else if (conversionType.IsEnum)
                conversion = () => Enum.Parse(conversionType, Convert.ToString(value), true);
            try
            {
                return conversion();
            }
            catch (Exception)
            {
                return Convert.ChangeType(value, conversionType);
            }
        }

        private static object ConvertNumber(Type type, byte value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return value;
                case TypeCode.SByte:
                    return (sbyte) value;
                case TypeCode.UInt16:
                    return (ushort) value;
                case TypeCode.UInt32:
                    return (uint) value;
                case TypeCode.UInt64:
                    return (ulong) value;
                case TypeCode.Int16:
                    return (short) value;
                case TypeCode.Int32:
                    return (int) value;
                case TypeCode.Int64:
                    return (long) value;
                case TypeCode.Decimal:
                    return (decimal) value;
                case TypeCode.Double:
                    return (double) value;
                case TypeCode.Single:
                    return (float) value;
                default:
                    throw new InvalidOperationException("Type is not numeric one");
            }
        }

        private static object ConvertNumber(Type type, sbyte value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return (byte) value;
                case TypeCode.SByte:
                    return value;
                case TypeCode.UInt16:
                    return (ushort) value;
                case TypeCode.UInt32:
                    return (uint) value;
                case TypeCode.UInt64:
                    return (ulong) value;
                case TypeCode.Int16:
                    return (short) value;
                case TypeCode.Int32:
                    return (int) value;
                case TypeCode.Int64:
                    return (long) value;
                case TypeCode.Decimal:
                    return (decimal) value;
                case TypeCode.Double:
                    return (double) value;
                case TypeCode.Single:
                    return (float) value;
                default:
                    throw new InvalidOperationException("Type is not numeric one");
            }
        }

        private static object ConvertNumber(Type type, ushort value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return (byte) value;
                case TypeCode.SByte:
                    return (sbyte) value;
                case TypeCode.UInt16:
                    return value;
                case TypeCode.UInt32:
                    return (uint) value;
                case TypeCode.UInt64:
                    return (ulong) value;
                case TypeCode.Int16:
                    return (short) value;
                case TypeCode.Int32:
                    return (int) value;
                case TypeCode.Int64:
                    return (long) value;
                case TypeCode.Decimal:
                    return (decimal) value;
                case TypeCode.Double:
                    return (double) value;
                case TypeCode.Single:
                    return (float) value;
                default:
                    throw new InvalidOperationException("Type is not numeric one");
            }
        }

        private static object ConvertNumber(Type type, uint value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return (byte) value;
                case TypeCode.SByte:
                    return (sbyte) value;
                case TypeCode.UInt16:
                    return (ushort) value;
                case TypeCode.UInt32:
                    return value;
                case TypeCode.UInt64:
                    return (ulong) value;
                case TypeCode.Int16:
                    return (short) value;
                case TypeCode.Int32:
                    return (int) value;
                case TypeCode.Int64:
                    return (long) value;
                case TypeCode.Decimal:
                    return (decimal) value;
                case TypeCode.Double:
                    return (double) value;
                case TypeCode.Single:
                    return (float) value;
                default:
                    throw new InvalidOperationException("Type is not numeric one");
            }
        }

        private static object ConvertNumber(Type type, ulong value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return (byte) value;
                case TypeCode.SByte:
                    return (sbyte) value;
                case TypeCode.UInt16:
                    return (ushort) value;
                case TypeCode.UInt32:
                    return (uint) value;
                case TypeCode.UInt64:
                    return value;
                case TypeCode.Int16:
                    return (short) value;
                case TypeCode.Int32:
                    return (int) value;
                case TypeCode.Int64:
                    return (long) value;
                case TypeCode.Decimal:
                    return (decimal) value;
                case TypeCode.Double:
                    return (double) value;
                case TypeCode.Single:
                    return (float) value;
                default:
                    throw new InvalidOperationException("Type is not numeric one");
            }
        }

        private static object ConvertNumber(Type type, short value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return (byte) value;
                case TypeCode.SByte:
                    return (sbyte) value;
                case TypeCode.UInt16:
                    return (ushort) value;
                case TypeCode.UInt32:
                    return (uint) value;
                case TypeCode.UInt64:
                    return (ulong) value;
                case TypeCode.Int16:
                    return value;
                case TypeCode.Int32:
                    return (int) value;
                case TypeCode.Int64:
                    return (long) value;
                case TypeCode.Decimal:
                    return (decimal) value;
                case TypeCode.Double:
                    return (double) value;
                case TypeCode.Single:
                    return (float) value;
                default:
                    throw new InvalidOperationException("Type is not numeric one");
            }
        }

        private static object ConvertNumber(Type type, int value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return (byte) value;
                case TypeCode.SByte:
                    return (sbyte) value;
                case TypeCode.UInt16:
                    return (ushort) value;
                case TypeCode.UInt32:
                    return (uint) value;
                case TypeCode.UInt64:
                    return (ulong) value;
                case TypeCode.Int16:
                    return (short) value;
                case TypeCode.Int32:
                    return value;
                case TypeCode.Int64:
                    return (long) value;
                case TypeCode.Decimal:
                    return (decimal) value;
                case TypeCode.Double:
                    return (double) value;
                case TypeCode.Single:
                    return (float) value;
                default:
                    throw new InvalidOperationException("Type is not numeric one");
            }
        }

        private static object ConvertNumber(Type type, long value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return (byte) value;
                case TypeCode.SByte:
                    return (sbyte) value;
                case TypeCode.UInt16:
                    return (ushort) value;
                case TypeCode.UInt32:
                    return (uint) value;
                case TypeCode.UInt64:
                    return (ulong) value;
                case TypeCode.Int16:
                    return (short) value;
                case TypeCode.Int32:
                    return (int) value;
                case TypeCode.Int64:
                    return value;
                case TypeCode.Decimal:
                    return (decimal) value;
                case TypeCode.Double:
                    return (double) value;
                case TypeCode.Single:
                    return (float) value;
                default:
                    throw new InvalidOperationException("Type is not numeric one");
            }
        }

        private static object ConvertNumber(Type type, decimal value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return (byte) value;
                case TypeCode.SByte:
                    return (sbyte) value;
                case TypeCode.UInt16:
                    return (ushort) value;
                case TypeCode.UInt32:
                    return (uint) value;
                case TypeCode.UInt64:
                    return (ulong) value;
                case TypeCode.Int16:
                    return (short) value;
                case TypeCode.Int32:
                    return (int) value;
                case TypeCode.Int64:
                    return (long) value;
                case TypeCode.Decimal:
                    return value;
                case TypeCode.Double:
                    return (double) value;
                case TypeCode.Single:
                    return (float) value;
                default:
                    throw new InvalidOperationException("Type is not numeric one");
            }
        }

        private static object ConvertNumber(Type type, double value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return (byte) value;
                case TypeCode.SByte:
                    return (sbyte) value;
                case TypeCode.UInt16:
                    return (ushort) value;
                case TypeCode.UInt32:
                    return (uint) value;
                case TypeCode.UInt64:
                    return (ulong) value;
                case TypeCode.Int16:
                    return (short) value;
                case TypeCode.Int32:
                    return (int) value;
                case TypeCode.Int64:
                    return (long) value;
                case TypeCode.Decimal:
                    return (decimal) value;
                case TypeCode.Double:
                    return value;
                case TypeCode.Single:
                    return (float) value;
                default:
                    throw new InvalidOperationException("Type is not numeric one");
            }
        }

        private static object ConvertNumber(Type type, float value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return (byte) value;
                case TypeCode.SByte:
                    return (sbyte) value;
                case TypeCode.UInt16:
                    return (ushort) value;
                case TypeCode.UInt32:
                    return (uint) value;
                case TypeCode.UInt64:
                    return (ulong) value;
                case TypeCode.Int16:
                    return (short) value;
                case TypeCode.Int32:
                    return (int) value;
                case TypeCode.Int64:
                    return (long) value;
                case TypeCode.Decimal:
                    return (decimal) value;
                case TypeCode.Double:
                    return (double) value;
                case TypeCode.Single:
                    return value;
                default:
                    throw new InvalidOperationException("Type is not numeric one");
            }
        }

        private static readonly Type NullableType = typeof(Nullable<>);

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