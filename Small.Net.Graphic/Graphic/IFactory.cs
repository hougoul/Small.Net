using System;

namespace Small.Net.Graphic.Graphic
{
    public interface IFactory : IDisposable
    {
        IAdapter GetHardwareAdapter();

        ISwapChain CreateSwapChain1(SwapChainConfig config, IDevice device);
    }
}