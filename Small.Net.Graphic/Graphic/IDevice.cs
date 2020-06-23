using System;

namespace Small.Net.Graphic.Graphic
{
    public interface IDevice : IDisposable
    {
        ICommandQueue CreateCommandQueue();
    }
}