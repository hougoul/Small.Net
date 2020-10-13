using System;
using System.Collections.Generic;

namespace Small.Net.ScubaDiving.Factories
{
    public interface IDataFactory<TType, in TInput> where TInput : Enum
    {
        IList<TType> CreateData(TInput type);
    }
}