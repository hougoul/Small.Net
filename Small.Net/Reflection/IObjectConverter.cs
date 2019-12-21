using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Small.Net.Reflection
{
    public interface IObjectConverter
    {
        Task<IList<object>> ConvertFromAsync(DbDataReader dr, CancellationToken token);

        object CreateObjectFrom(object source, Type objectType);
    }

    public interface IObjectConverter<TObject> : IObjectConverter where TObject : class
    {
        new Task<IList<TObject>> ConvertFromAsync(DbDataReader dr, CancellationToken token);
    }
}