using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Small.Net.Reflection 
{
    public class FastGetterSetter<T> : IGetterSetter
    {
        private Func<object, object> _getMethod;
        private Action<T, object> _setMethod;

        public string Name { get; private set; }
        public Type PropertyType { get; private set;}
        public IEnumerable<object> Attributes { get; private set;}
        public bool HasGetter { get; private set; }
        public bool HasSetter { get; private set; }

        public FastGetterSetter(PropertyInfo propertyInfo) {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));
            CreateGetterAndSetter(propertyInfo);
        }

        public object GetValue(object obj) {
            Debug.Assert(HasGetter);
            return _getMethod(obj);
        }

        public void SetValue(object obj, object value) {
            if (!(obj is T objGoodType)) throw new InvalidOperationException("Cannot use the setter on this object");
             Debug.Assert(HasSetter);
            if (PropertyType != value.GetType()) value = PropertyType.ConvertValue(value);
             _setMethod(objGoodType, value);
        }

        private void CreateGetterAndSetter(PropertyInfo pi) {
            var type = typeof(T);
            Name = pi.Name;
            PropertyType = pi.PropertyType;
            Attributes = pi.GetCustomAttributes(true);
            /* Create linq expression to access getter setter */
            var getter = pi.GetGetMethod();
            /* We support simple getter, we don't support getter with index parameter */
            if (getter != null && getter.GetParameters().Length == 0) {
                var parameter = Expression.Parameter(typeof(object));
                var cast = Expression.Convert(parameter, type);
                var propertyGetter = Expression.Property(cast, pi);
                var castResult = Expression.Convert(propertyGetter, typeof(object));//for boxing

                _getMethod = Expression.Lambda<Func<object, object>>(castResult, parameter).Compile();
                HasGetter = true;
            }
            var setter = pi.GetSetMethod(true);
            /* We support simple setter, we don't support setter with index parameter */
            if (setter == null || setter?.IsPublic != true || setter?.GetParameters().Length > 1) return;
            var instance = Expression.Parameter(pi.DeclaringType ?? throw new InvalidOperationException(), "target");
            var argument = Expression.Parameter(typeof(object), "value");
            var setterCall = Expression.Call(
                instance,
                pi.GetSetMethod(),
                Expression.Convert(argument, pi.PropertyType));

            _setMethod = (Action<T, object>)Expression.Lambda(setterCall, instance, argument).Compile();
            HasSetter = true;
        }
    }
}