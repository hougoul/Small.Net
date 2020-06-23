using TerraFX.Interop;

namespace Small.Net.Graphic.Graphic
{
    public unsafe class Dx12SwapChain : Dx12Object<IDXGISwapChain3>, ISwapChain
    {
        internal Dx12SwapChain(IDXGISwapChain3* field) : base(field)
        {
        }

        public void SetToUwpSwapChainPanel(object container)
        {
            throw new System.NotImplementedException();
        }
    }
}

