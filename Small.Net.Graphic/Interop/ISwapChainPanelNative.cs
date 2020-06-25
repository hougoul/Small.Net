using SharpGen.Runtime;
using System;
using System.Runtime.InteropServices;
using Vortice.DXGI;

namespace Small.Net.Graphic.Interop
{
    [Guid("F92F19D2-3ADE-45A6-A20C-F6F1EA90554B")]
    public class ISwapChainPanelNative : ComObject
    {
        public ISwapChainPanelNative(IntPtr nativePtr) : base(nativePtr)
        {
        }
        public static explicit operator ISwapChainPanelNative(IntPtr nativePtr) => nativePtr == IntPtr.Zero ? null : new ISwapChainPanelNative(nativePtr);

        public IDXGISwapChain SwapChain
        {
            set => SetSwapChain(value);
        }

        internal unsafe void SetSwapChain(IDXGISwapChain swapChain)
        {
            IntPtr swapChain_ = IntPtr.Zero;
            swapChain_ = ToCallbackPtr<IDXGISwapChain>(swapChain);
            var hr = LocalInterop.CalliStdCallint(_nativePointer, (void*)swapChain_, (*(void***)_nativePointer)[3]);
            if (hr < 0)
            {
                throw new InvalidOperationException($"HRESULT = 0x{hr:X}");
            }
        }
    }
}
