using Small.Net.Reflection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Small.Net.Extensions
{
    public static class DataExtensions
    {
        /// <summary>
        /// Converts to an object of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr">The dr.</param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> ConvertTo<T>(this DbDataReader dr) where T : class, new()
        {
            return await dr.ConvertTo<T>(CancellationToken.None).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<T>> ConvertTo<T>(this DbDataReader dr, CancellationToken token) where T : class, new()
        {
            var list = new List<T>();
            var helper = typeof(T).GetObjectReflectionHelper();
            var columns = Enumerable.Range(0,dr.FieldCount).Select(i => new { Index = i, Name = dr.GetName(i) }).Where(c => helper.HasProperty(c.Name, PropertyType.Setter)).ToList();

            while (await dr.ReadAsync(token).ConfigureAwait(false))
            {
                var obj =(T)helper.CreateInstance();
                foreach (var column in columns) helper.SetValue(column.Name, obj, dr.GetValue(column.Index));
                list.Add(obj);

                if (token.IsCancellationRequested) break;
            }

            return list;
        }

        /// <summary>
        /// Converts all dbdatareader to each IEnumerable&lt;object of type&gt;.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <param name="toTypes">To types.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">toTypes</exception>
        public static async Task<IEnumerable<IEnumerable<object>>> ConvertTo(this DbDataReader dr, params Type[] toTypes)
        {
            return await dr.ConvertTo(CancellationToken.None, toTypes).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<IEnumerable<object>>> ConvertTo(this DbDataReader dr, CancellationToken token, params Type[] toTypes)
        {
            if (toTypes == null) throw new ArgumentNullException(nameof(toTypes));

            var list = new List<List<object>>();
            foreach(var type in toTypes)
            {
                if (token.IsCancellationRequested) break;

                var currentList = new List<object>();
                var helper = type.GetObjectReflectionHelper();
                var columns = Enumerable.Range(0, dr.FieldCount).Select(i => new { Index = i, Name = dr.GetName(i) }).Where(c => helper.HasProperty(c.Name, PropertyType.Setter)).ToList();

                while (await dr.ReadAsync(token).ConfigureAwait(false))
                {
                    var obj = helper.CreateInstance();
                    foreach (var column in columns) helper.SetValue(column.Name, obj, dr.GetValue(column.Index));
                    currentList.Add(obj);
                    if (token.IsCancellationRequested) break;
                }

                list.Add(currentList);
                if (!(await dr.NextResultAsync(token).ConfigureAwait(false))) break;
            }

            return list;
        }
    }
}
