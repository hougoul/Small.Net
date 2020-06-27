using Vortice.Direct3D12;

namespace Small.Net.Graphic.D12.Core
{
    internal struct CommandAllocatorEntry
    {
        public long FenceValue;
        public ID3D12CommandAllocator CommandAllocator;
    }
}
