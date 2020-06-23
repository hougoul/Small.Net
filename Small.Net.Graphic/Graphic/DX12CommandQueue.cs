using System;
using TerraFX.Interop;

namespace Small.Net.Graphic.Graphic
{
    public unsafe class DX12CommandQueue : Dx12Object<ID3D12CommandQueue>, ICommandQueue
    {
        internal DX12CommandQueue(ID3D12CommandQueue* queue) : base(queue)
        {
        }
    }
}
