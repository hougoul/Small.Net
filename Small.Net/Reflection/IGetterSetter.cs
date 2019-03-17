using System;
using System.Collections.Generic;

namespace Small.Net.Reflection 
{
    public interface IGetterSetter
    {
        string Name { get; }
        Type PropertyType { get; }
        IEnumerable<object> Attributes { get; }
        bool HasGetter {get;}
        bool HasSetter {get;}

        object GetValue(object obj);
        void SetValue(object obj, object value);
    }
}