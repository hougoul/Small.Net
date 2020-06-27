using Small.Net.Graphic.Core;
using Small.Net.Graphic.D12.Core;
using Small.Net.Utilities;
using System;

namespace Small.Net.Graphic.D12
{
    public class Dx12Engine : IEngine
    {
        private readonly IDisposableManager _disposableManager = new CommonDisposableManager();
        private bool disposedValue;


        ~Dx12Engine()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: false);
        }

        public CommandQueueManager CommandQueueManager { get; private set; }

        public virtual bool Initialise(IWindowHandle window)
        {
            // TODO
            throw new NotImplementedException();
        }

        public virtual void Resize(int newWidth, int newHeight)
        {
            // TODO
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // supprimer l'état managé (objets managés)
                    CommandQueueManager?.Dispose();
                }

                // libérer les ressources non managées (objets non managés) et substituer le finaliseur
                _disposableManager.ReverseDispose = true;
                _disposableManager.Dispose();
                //  affecter aux grands champs une valeur null
                CommandQueueManager = null;
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
