using System;
using TerraFX.Interop;

namespace Small.Net.Graphic.Graphic
{
    public unsafe class Dx12Adapter : Dx12Object<IDXGIAdapter>, IAdapter
    {
        internal Dx12Adapter(IDXGIAdapter* field) : base(field)
        {
        }

        public IDevice CreateDevice()
        {
            var device = DirectxHelper.CreateDevice(UnsafePtr.Ptr);
            return new Dx12Device(device);
        }
    }
}