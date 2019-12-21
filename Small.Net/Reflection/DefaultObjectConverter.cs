using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Small.Net.Extensions;

namespace Small.Net.Reflection
{
    internal class DefaultObjectConverter<TObject> : IObjectConverter<TObject> where TObject : class
    {
        private readonly IDictionary<string, IGetterSetter> _properties;
        private readonly Func<object> _instanceCreator;

        public DefaultObjectConverter(IDictionary<string, IGetterSetter> properties, Func<object> createInstance)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _instanceCreator = createInstance ?? throw new ArgumentNullException(nameof(createInstance));
        }

        public async Task<IList<TObject>> ConvertFromAsync(DbDataReader dr, CancellationToken token)
        {
            var list = new List<TObject>();
            var columns = Enumerable.Range(0, dr.FieldCount).Select(i => new {Index = i, Name = dr.GetName(i)})
                .Where(c => _properties.TryGetValue(c.Name, out var p) && p.CanForceSetter).ToList();

            while (await dr.ReadAsync(token).ConfigureAwait(false))
            {
                var obj = (TObject) _instanceCreator();
                foreach (var column in columns)
                {
                    _properties[column.Name].SetValue(obj, dr.GetValue(column.Index), true);
                }

                list.Add(obj);

                if (token.IsCancellationRequested) break;
            }

            return list;
        }

        public object CreateObjectFrom(object source, Type objectType)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var sourceType = source.GetType();
            if (sourceType != typeof(TObject))
                throw new InvalidOperationException($"your Source is not a {typeof(TObject)}");
            if (sourceType == objectType && source is ICloneable cloneable) return cloneable.Clone();
            var helper = objectType.GetObjectReflectionHelper();
            var getters = _properties.Where(p => p.Value.HasGetter).ToDictionary(p => p.Key, p => p.Value);
            var setters = helper.GetProperties(PropertyType.Setter);
            var copy = helper.CreateInstance();
            foreach (var getter in getters)
            {
                if (!setters.ContainsKey(getter.Key)) continue;
                var value = getter.Value.GetValue(source);
                var propHelper = setters[getter.Key];

                if (!(value is IEnumerable enumerable) || (value is string))
                {
                    propHelper.SetValue(copy, value);
                    continue;
                }

                /* We have a collection of object to duplicate */
                var newValue = propHelper.PropertyType.GetObjectReflectionHelper().CreateInstance();
                if (!(newValue is IList collection))
                    throw new InvalidOperationException(
                        $"The destination property {propHelper.Name} is not a collection");
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

        async Task<IList<object>> IObjectConverter.ConvertFromAsync(DbDataReader dr, CancellationToken token)
        {
            return (await ConvertFromAsync(dr, token)).Cast<object>().ToList();
        }
    }
}