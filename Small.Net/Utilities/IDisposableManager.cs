using System;

namespace Small.Net.Utilities
{
    public interface IDisposableManager : IDisposable
    {
        bool ReverseDispose { get; set; }
        /// <summary>
        /// Add a disposable object
        /// </summary>
        /// <param name="obj"></param>
        void Add(IDisposable obj);

        /// <summary>
        /// Remove disposable object and dispose it
        /// </summary>
        /// <param name="obj"></param>
        void Delete(IDisposable obj);
    }
}
