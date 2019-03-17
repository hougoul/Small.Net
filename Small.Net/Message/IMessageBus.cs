using System;

namespace Small.Net.Message
{
    public interface IMessageBus<T> : IDisposable
    {
        IDisposable Subscribe(Action<T> handler);
        void Publish(T message);
    }

    internal interface IUnSubscribeMessageBus<T>
    {
        void UnSubscribe(MessageListener<T> listener);
    }
}