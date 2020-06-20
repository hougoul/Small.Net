using Small.Net.Graphic.Core;
using System;
using TerraFX.Interop;

namespace Small.Net.Graphic.Graphic
{
    public static class DirectxHelper
    {
        internal unsafe static IDXGIFactory4* CreateDXGIFactory2(Guid riid)
        {
            IDXGIFactory4* factory = null;
            Result result = Windows.CreateDXGIFactory2(0u, &riid, (void**) &factory);

            result.CheckError();
            return factory;
        }

        internal unsafe static IDXGIAdapter* GetHardwareAdapter(IDXGIFactory4* factory)
        {
            IDXGIAdapter1* adapter;
            var factory1 = (IDXGIFactory1*) factory;
            // We are looking for Direct 12 Hardware
            var iid = Windows.IID_ID3D12Device;

            for (var adapterIndex = 0u;
                Windows.DXGI_ERROR_NOT_FOUND != factory1->EnumAdapters1(adapterIndex, &adapter);
                ++adapterIndex)
            {
                DXGI_ADAPTER_DESC1 desc;
                adapter->GetDesc1(&desc);

                if ((desc.Flags & (uint) DXGI_ADAPTER_FLAG.DXGI_ADAPTER_FLAG_SOFTWARE) != 0)
                {
                    // Don't select the Basic Render Driver adapter.
                    // If you want a software adapter, pass in "/warp" on the command line.
                    continue;
                }

                if (Windows.D3D12CreateDevice((TerraFX.Interop.IUnknown*) adapter,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_12_1, &iid, null) >= 0)
                {
                    // we find our device
                    break;
                }
            }

            return (IDXGIAdapter*) adapter;
        }

        internal unsafe static ID3D12Device* CreateDevice(IDXGIAdapter* adapter)
        {
            ID3D12Device* _device;

            ID3D12Device** device = &_device;
            var iid = Windows.IID_ID3D12Device;
            Result result = Windows.D3D12CreateDevice((IUnknown*) adapter, D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_0,
                &iid, (void**) device);
            result.CheckError();

            return _device;
        }
    }
}