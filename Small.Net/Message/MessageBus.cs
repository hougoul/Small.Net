using System;
using System.Collections.Generic;

namespace Small.Net.Message
{
    public sealed class MessageBus<T> : IMessageBus<T>, IUnSubscribeMessageBus<T>
    {
        private readonly HashSet<MessageListener<T>> _listeners = new HashSet<MessageListener<T>>();

        /// <summary>Publishes the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Publish(T message)
        {
            foreach (var listener in _listeners) listener.Invoke(message);
        }

        /// <summary>Subscribes the specified handler.</summary>
        /// <param name="handler">The handler.</param>
        /// <returns>Disposable class to unsubscribe</returns>
        public IDisposable Subscribe(Action<T> handler)
        {
            var listener = new MessageListener<T>(this, handler);
            _listeners.Add(listener);
            return listener;
        }

        void IUnSubscribeMessageBus<T>.UnSubscribe(MessageListener<T> listener)
        {
            if (_listeners.Contains(listener)) _listeners.Remove(listener);
        }

        #region IDisposable Support

        private bool _disposedValue;

        public void Dispose()
        {
            if (_disposedValue)
            {
                return;
            }

            foreach (var listener in _listeners)
            {
                listener.Dispose();
            }

            _disposedValue = true;
        }

        #endregion
    }
}