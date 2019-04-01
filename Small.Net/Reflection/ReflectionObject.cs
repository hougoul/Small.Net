using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Small.Net.Reflection
{
    public class ReflectionObject<T> : IReflectionObject where T : class, new()
    {
        private readonly Dictionary<string, IGetterSetter> _properties = new Dictionary<string, IGetterSetter>(StringComparer.OrdinalIgnoreCase);
        private delegate object FastActivator(params object[] args);
        private readonly FastActivator _activator;


        public ReflectionObject()
        {
            InitialiseGetterSetter();
            _activator = InitialiseActivator();
        }

        public IGetterSetter this[string propertyName]
        {
            get
            {
                if (!_properties.ContainsKey(propertyName)) throw new ArgumentOutOfRangeException(nameof(propertyName));
                return _properties[propertyName];
            }
        }

        public object CreateInstance(params object[] args)
        {
            return _activator(args);
        }
        
        public object GetValue(string propertyName, object obj)
        {
            if (!_properties.ContainsKey(propertyName)) throw new ArgumentOutOfRangeException(nameof(propertyName));
            var getter = _properties[propertyName];
            if (!getter.HasGetter) throw new InvalidOperationException($"Property {propertyName} doesn't have public getter");
            return getter.GetValue(obj);
        }

        public void SetValue(string propertyName, object obj, object value)
        {
            if (!_properties.ContainsKey(propertyName)) throw new ArgumentOutOfRangeException(nameof(propertyName));
            var setter = _properties[propertyName];
            if (!setter.HasGetter) throw new InvalidOperationException($"Property {propertyName} doesn't have public setter");
            setter.SetValue(obj, value);
        }


        public IReadOnlyDictionary<string, IGetterSetter> GetProperties(PropertyType propertyType)
        {
            switch (propertyType)
            {
                case PropertyType.All:
                    return _properties;
                case PropertyType.Getter:
                    return _properties.Where(pair => pair.Value.HasGetter).ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.OrdinalIgnoreCase);
                case PropertyType.Setter:
                    return _properties.Where(pair => pair.Value.HasSetter).ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.OrdinalIgnoreCase);
            }
            throw new InvalidOperationException("Cannot arrive here");
        }

        public bool HasProperty(string propertyName, PropertyType propertyType)
        {
            switch (propertyType)
            {
                case PropertyType.All:
                    return _properties.ContainsKey(propertyName);
                case PropertyType.Getter:
                    return _properties.ContainsKey(propertyName) && _properties[propertyName].HasGetter;
                case PropertyType.Setter:
                    return _properties.ContainsKey(propertyName) && _properties[propertyName].HasSetter;
            }

            return false;
        }

        private void InitialiseGetterSetter()
        {
            var objType = typeof(T);
            var propertiesInfo = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            if (propertiesInfo.Length == 0) return;
            var getterSetterType = typeof(FastGetterSetter<>).MakeGenericType(objType);
            foreach (var propInfo in propertiesInfo)
            {
                var getterSetter = (IGetterSetter)Activator.CreateInstance(getterSetterType, propInfo);
                _properties.Add(propInfo.Name, getterSetter);
            }
        }

        private FastActivator InitialiseActivator()
        {
            var objType = typeof(T);
            var constructorInfo = objType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault();
            Debug.Assert(constructorInfo != null, nameof(constructorInfo) + " != null");
            var ctorParams = constructorInfo.GetParameters();
            var paramExp = Expression.Parameter(typeof(object[]), "args");

            var expArr = new Expression[ctorParams.Length];

            for (var i = 0; i < ctorParams.Length; i++)
            {
                var ctorType = ctorParams[i].ParameterType;
                var argExp = Expression.ArrayIndex(paramExp, Expression.Constant(i));
                expArr[i] = Expression.Convert(argExp, ctorType);
            }

            var newExp = Expression.New(constructorInfo, expArr);
            var lambda = Expression.Lambda<FastActivator>(newExp, paramExp);
            return lambda.Compile();
        }
    }
}
