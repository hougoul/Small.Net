using System;
using TerraFX.Interop;

namespace Small.Net.Graphic.Graphic
{
    public unsafe class Dx12Device : IDevice
    {
        private bool disposedValue;
        private ID3D12Device* _device;

        internal Dx12Device(ID3D12Device* device)
        {
            _device = device;
        }

        ~Dx12Device()
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //  supprimer l'état managé (objets managés)
                }

                // libérer les ressources non managées (objets non managés) et substituer le finaliseur
                _device->Release();
                _device = null;
                // affecter aux grands champs une valeur null
                disposedValue = true;
            }
        }
    }
}