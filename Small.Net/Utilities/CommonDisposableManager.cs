using System;
using System.Collections.Generic;

namespace Small.Net.Utilities
{
    public class CommonDisposableManager : IDisposableManager
    {
        private bool disposedValue;
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        public bool ReverseDispose { get ; set; }

        ~CommonDisposableManager()
        {
            Dispose(false);
        }

        public void Add(IDisposable obj)
        {
            disposables.Add(obj);
        }

        public void Delete(IDisposable obj)
        {
            disposables.Remove(obj);
            obj.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (ReverseDispose) disposables.Reverse();
                disposables.ForEach(o => o.Dispose());
                disposables.Clear();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
