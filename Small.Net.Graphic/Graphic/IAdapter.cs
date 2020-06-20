using System;

namespace Small.Net.Graphic.Graphic
{
    public interface IAdapter : IDisposable
    {
        IDevice CreateDevice();
    }
}