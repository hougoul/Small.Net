using Vortice.DXGI;

namespace Small.Net.Graphic.Core
{
    public interface IWindowHandle
    {
        object Handle { get; }

        internal void AssignTo(IDXGISwapChain4 swapChain);
    }
}
