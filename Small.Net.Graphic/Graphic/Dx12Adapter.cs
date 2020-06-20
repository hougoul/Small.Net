using System;
using TerraFX.Interop;

namespace Small.Net.Graphic.Graphic
{
    public unsafe class Dx12Adapter : IAdapter
    {
        private bool disposedValue;
        private IDXGIAdapter* _adapter;

        internal Dx12Adapter(IDXGIAdapter* adapter)
        {
            _adapter = adapter;
        }

        ~Dx12Adapter()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IDevice CreateDevice()
        {
            var device = DirectxHelper.CreateDevice(_adapter);
            return new Dx12Device(device);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // supprimer l'état managé (objets managés)
                }

                //  libérer les ressources non managées (objets non managés) et substituer le finaliseur
                _adapter->Release();
                _adapter = null;
                //  affecter aux grands champs une valeur null
                disposedValue = true;
            }
        }
    }
}