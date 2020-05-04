using System;

namespace Small.Net.Message
{
    internal sealed class MessageListener<T> : IDisposable
    {
        private IUnSubscribeMessageBus<T> _messageBus;
        private Action<T> _action;

        public MessageListener(IUnSubscribeMessageBus<T> messageBus, Action<T> action)
        {
            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Invoke(T message)
        {
            _action(message);
        }

        #region IDisposable Support

        private bool _disposedValue = false;

        private void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                _messageBus.UnSubscribe(this);
            }

            _messageBus = null;
            _action = null;

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}