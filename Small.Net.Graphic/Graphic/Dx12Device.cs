using TerraFX.Interop;

namespace Small.Net.Graphic.Graphic
{
    public unsafe class Dx12Device : Dx12Object<ID3D12Device>,  IDevice
    {
        internal Dx12Device(ID3D12Device* device) : base(device)
        {
        }

        public ICommandQueue CreateCommandQueue()
        {
            return new DX12CommandQueue(DirectxHelper.CreateCommandQueue(UnsafePtr.Ptr));
        }
    }
}