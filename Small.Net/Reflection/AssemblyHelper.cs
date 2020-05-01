using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Small.Net.Reflection
{
    /// <summary>
    /// Assembly Helper
    /// </summary>
    public class AssemblyHelper : IAssemblyHelper
    {
        public IEnumerable<Type> TypesImplementInterface(Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(type.IsAssignableFrom);
        }
    }
}