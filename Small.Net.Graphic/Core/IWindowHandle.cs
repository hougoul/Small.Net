using Vortice.DXGI;

namespace Small.Net.Graphic.Core
{
    public interface IWindowHandle
    {
        int Width { get; }

        int Height { get; }

        object Handle { get; }

        public void AssignTo(IDXGISwapChain4 swapChain);
    }
}