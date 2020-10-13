using Vortice.Direct3D12;

namespace Small.Net.Graphic.D12.Core
{
    internal struct CommandAllocatorEntry
    {
        public ulong FenceValue;
        public ID3D12CommandAllocator CommandAllocator;
    }
}
