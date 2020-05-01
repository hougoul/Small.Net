using Small.Net.Reflection;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
        public static async Task<IList<T>> ConvertTo<T>(this DbDataReader dr) where T : class, new()
        {
            return await dr.ConvertTo<T>(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Convert a DataReader to a list of object
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="token"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<IList<T>> ConvertTo<T>(this DbDataReader dr, CancellationToken token)
            where T : class, new()
        {
            var helper = (IObjectConverter<T>) typeof(T).GetObjectReflectionHelper().Converter;

            return await helper.ConvertFromAsync(dr, token);
        }

        /// <summary>
        /// Converts all dbdatareader to each IEnumerable&lt;object of type&gt;.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <param name="toTypes">To types.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">toTypes</exception>
        public static async Task<IEnumerable<IEnumerable<object>>> ConvertTo(this DbDataReader dr,
            params Type[] toTypes)
        {
            return await dr.ConvertTo(CancellationToken.None, toTypes).ConfigureAwait(false);
        }

        /// <summary>
        /// Convert multi Datareader to different class
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="token"></param>
        /// <param name="toTypes"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<IList<IList<object>>> ConvertTo(this DbDataReader dr, CancellationToken token,
            params Type[] toTypes)
        {
            if (toTypes == null) throw new ArgumentNullException(nameof(toTypes));

            var list = new List<IList<object>>();
            foreach (var type in toTypes)
            {
                if (token.IsCancellationRequested) break;

                var helper = type.GetObjectReflectionHelper().Converter;

                list.Add(await helper.ConvertFromAsync(dr, token));
                if (!(await dr.NextResultAsync(token).ConfigureAwait(false))) break;
            }

            return list;
        }
    }
}