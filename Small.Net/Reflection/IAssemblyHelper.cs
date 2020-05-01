using System;
using System.Collections.Generic;

namespace Small.Net.Reflection
{
    public interface IAssemblyHelper
    {
        IEnumerable<Type> TypesImplementInterface(Type type);
    }
}