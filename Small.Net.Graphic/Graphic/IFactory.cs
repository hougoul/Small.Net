using System;

namespace Small.Net.Graphic.Graphic
{
    public interface IFactory : IDisposable
    {
        IAdapter GetHardwareAdapter();
    }
}