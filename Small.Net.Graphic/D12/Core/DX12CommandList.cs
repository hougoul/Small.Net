using Vortice.Direct3D12;

namespace Small.Net.Graphic.D12.Core
{
    public struct DX12CommandList<T> where T : ID3D12CommandList
    {
        public T CommandList;
        public ID3D12CommandAllocator CommandAllocator;
    }
}
