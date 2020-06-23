using System;

namespace Small.Net.Graphic.Graphic
{
    public unsafe class Dx12Object<T> : IDisposable where T : unmanaged
    {
        private bool disposedValue;
        private readonly ComPtrField<T> _ptrField;

        internal Dx12Object(T* field)
        {
            _ptrField = new ComPtrField<T>(field);
        }

        // substituer le finaliseur uniquement si 'Dispose(bool disposing)' a du code pour libérer les ressources non managées
        ~Dx12Object()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: false);
        }

        public ComPtrField<T> UnsafePtr => _ptrField;

        public void Dispose()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {

                //  libérer les ressources non managées (objets non managés) et substituer le finaliseur
                _ptrField.Dispose();
                //  affecter aux grands champs une valeur null
                disposedValue = true;
            }
        }
    }
}
