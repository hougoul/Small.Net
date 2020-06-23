using Small.Net.Graphic.Core;
using TerraFX.Interop;

namespace Small.Net.Graphic.Graphic
{
    public unsafe class Dx12Factory : Dx12Object<IDXGIFactory4>, IFactory
    {
        private Dx12Factory(IDXGIFactory4* factory) : base(factory)
        {
        }

        public static Dx12Factory CreateFactoryDx12()
        {
            var iid = Windows.IID_IDXGIFactory4;
            var factoryOut = DirectxHelper.CreateDXGIFactory2(iid);
            return new Dx12Factory(factoryOut);
        }

        public ISwapChain CreateSwapChain1(SwapChainConfig config, IDevice device)
        {
            IDXGISwapChain3* swapChain3;
            var swapChain = DirectxHelper.CreateSwapChain(config, UnsafePtr.Ptr, ((Dx12Device)device).UnsafePtr.Ptr);
            var iid = Windows.IID_IDXGISwapChain3;
            Result result = swapChain->QueryInterface(&iid, (void**)&swapChain3);
            result.CheckError();
            return new Dx12SwapChain(swapChain3);
        }

        public IAdapter GetHardwareAdapter()
        {
            var adapter = DirectxHelper.GetHardwareAdapter(UnsafePtr.Ptr);
            return new Dx12Adapter(adapter);
        }
    }
}