using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Small.Net.Reflection
{
    internal static class AssemblyHelper
    {
        private static readonly List<Type> _converters;

        static AssemblyHelper()
        {
            _converters = new List<Type>(GetTypesWithIObjectConverter());
        }

        private static IEnumerable<Type> GetTypesWithIObjectConverter()
        {
            var type = typeof(IObjectConverter);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass);
        }

        internal static IObjectConverter CreateIObjectConverter<TObject>(IDictionary<string, IGetterSetter> properties,
            Func<object> createInstance) where TObject : class
        {
            var type = typeof(IObjectConverter<TObject>);
            var orderType = typeof(DefaultObjectConverter<TObject>);
            var goodType = _converters.FirstOrDefault(t => type.IsAssignableFrom(t));
            if (goodType != null)
            {
                return (IObjectConverter) Activator.CreateInstance(goodType);
            }

            return new DefaultObjectConverter<TObject>(properties, createInstance);
        }
    }
}