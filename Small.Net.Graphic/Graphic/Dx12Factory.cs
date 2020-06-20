using Small.Net.Graphic.Core;
using System;
using TerraFX.Interop;

namespace Small.Net.Graphic.Graphic
{
    public unsafe class Dx12Factory : IFactory
    {
        private bool disposedValue;
        private IDXGIFactory4* _factory;

        private Dx12Factory(IDXGIFactory4* factory)
        {
            _factory = factory;
        }

        ~Dx12Factory()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: false);
        }

        public static Dx12Factory CreateFactoryDx12()
        {
            var iid = Windows.IID_IDXGIFactory4;
            var factoryOut = DirectxHelper.CreateDXGIFactory2(iid);
            return new Dx12Factory(factoryOut);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // supprimer l'état managé (objets managés)
                }

                // libérer les ressources non managées (objets non managés) et substituer le finaliseur
                _factory->Release();
                _factory = null;
                // affecter aux grands champs une valeur null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IAdapter GetHardwareAdapter()
        {
            var adapter = DirectxHelper.GetHardwareAdapter(_factory);
            return new Dx12Adapter(adapter);
        }
    }
}