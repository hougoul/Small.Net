using System;
using Windows.UI.Xaml.Controls;
using SharpGen.Runtime;
using Small.Net.Graphic.Core;
using Small.Net.Graphic.Interop;
using Vortice.DXGI;

namespace WorldEditor.Entities
{
    public class UwpWindowHandle : IWindowHandle
    {
        private readonly SwapChainPanel _panel;

        public UwpWindowHandle(SwapChainPanel panel)
        {
            _panel = panel ?? throw new ArgumentNullException(nameof(panel));
        }

        public int Width => (int) _panel.RenderSize.Width;
        public int Height => (int) _panel.RenderSize.Height;

        public object Handle => _panel;

        public void AssignTo(IDXGISwapChain4 swapChain)
        {
            using var nativeObject = ComObject.As<ISwapChainPanelNative>(_panel);
            nativeObject.SwapChain = swapChain;
        }
    }
}