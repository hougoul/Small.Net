using Small.Net.Graphic.Core;
using Small.Net.Graphic.D12.Core;
using Small.Net.Utilities;
using System;
using System.Drawing;
using System.Linq;
using Vortice.Direct3D;
using Vortice.Direct3D12;
using Vortice.Direct3D12.Debug;
using Vortice.DXGI;

namespace Small.Net.Graphic.D12
{
    public class Dx12Engine : IEngine
    {
        private const int SwapBufferCount = 2;
        private const Format ViewFormat = Format.R8G8B8A8_UNorm;

        private readonly IDisposableManager _disposableManager = new CommonDisposableManager();
        private readonly ID3D12Resource[] _backBuffers = new ID3D12Resource[SwapBufferCount];
        private readonly bool _verticalSync;

        private bool _disposedValue;
        private bool _dxDebug;
        private bool _tearingSupported;
        private IDXGIFactory5 _factory;
        private ID3D12Device5 _device;
        private IDXGISwapChain4 _swapChain;
        private ID3D12DescriptorHeap _descriptorHeap;

        private int _currentBackBufferIndex;
        private int _currentWidth;
        private int _currentHeight;

        public Dx12Engine(bool verticalSync = true)
        {
            _verticalSync = verticalSync;
        }

        ~Dx12Engine()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: false);
        }

        private CommandQueueManager CommandQueueManager { get; set; }

        private ID3D12Resource CurrentBuffer => _backBuffers[_currentBackBufferIndex];

        public bool IsInitialised { get; private set; }

        public virtual bool Initialise(IWindowHandle window)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            _currentWidth = window.Width;
            _currentHeight = window.Height;
            if (!D3D12.IsSupported(FeatureLevel.Level_12_1))
            {
                return false;
            }
#if DEBUG
            var result = D3D12.D3D12GetDebugInterface<ID3D12Debug>(out var debugInterface);
            result.CheckError();
            if (result.Success)
            {
                debugInterface.EnableDebugLayer();
                _disposableManager.Add(debugInterface);
                _dxDebug = true;
            }
#endif
            DXGI.CreateDXGIFactory2(_dxDebug, out _factory);
            if (_factory == null)
            {
                return false;
            }

            _disposableManager.Add(_factory);
            _tearingSupported = _factory.PresentAllowTearing;
            _device = CreateDevice();
            if (_device == null)
            {
                return false;
            }

            CommandQueueManager = new CommandQueueManager(_device, CommandListType.Direct);
            _swapChain = CreateSwapChain(window);
            _currentBackBufferIndex = _swapChain.GetCurrentBackBufferIndex();
            _descriptorHeap = CreateDescriptorHeap(DescriptorHeapType.RenderTargetView, SwapBufferCount);
            UpdateRenderTargetViews();

            IsInitialised = true;
            return true;
        }

        public virtual void Resize(int newWidth, int newHeight)
        {
            if (_currentHeight == newHeight && _currentWidth == newWidth)
            {
                return;
            }

            _currentHeight = newHeight;
            _currentWidth = newWidth;
            CommandQueueManager.Flush();
            foreach (var buffer in _backBuffers)
            {
                buffer.Release();
            }

            var description = _swapChain.Description;
            _swapChain.ResizeBuffers(SwapBufferCount, newWidth, newHeight, ViewFormat, description.Flags);
            _currentBackBufferIndex = _swapChain.GetCurrentBackBufferIndex();

            UpdateRenderTargetViews();
        }

        public virtual void Render()
        {
            var backBuffer = CurrentBuffer;
            var commandList = CommandQueueManager.GetCommandList();
            ResourceBarrier(commandList.CommandList, backBuffer, ResourceStates.Present, ResourceStates.RenderTarget);
            var cpuHandler = _descriptorHeap.GetCPUDescriptorHandleForHeapStart();
            commandList.CommandList.ClearRenderTargetView(cpuHandler, Color.DarkCyan);

            // We finish rendering
            ResourceBarrier(commandList.CommandList, backBuffer, ResourceStates.RenderTarget, ResourceStates.Present);
            var fenceValue = CommandQueueManager.ExecuteCommandList(commandList);
            var result = _swapChain.Present(_verticalSync ? 1 : 0,
                _tearingSupported && !_verticalSync ? PresentFlags.AllowTearing : PresentFlags.None);
            result.CheckError();
            CommandQueueManager.WaitForFenceValue(fenceValue);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                CommandQueueManager?.Dispose();
            }

            _disposableManager.ReverseDispose = true;
            _disposableManager.Dispose();

            CommandQueueManager = null;
            _disposedValue = true;
        }


        private ID3D12Device5 CreateDevice()
        {
            // Generally the adapters with more memory is the best one
            var adapters = _factory.Adapters1.OrderByDescending(a => (long) a.Description1.DedicatedVideoMemory)
                .ToList();
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < adapters.Count; i++)
            {
                var desc = adapters[i].Description1;
                if ((desc.Flags & AdapterFlags.Software) == AdapterFlags.Software)
                {
                    continue;
                }

                var res = D3D12.D3D12CreateDevice(adapters[i], FeatureLevel.Level_12_1, out var device);
                if (res.Failure)
                {
                    continue;
                }

                var opt5 = device.CheckFeatureSupport<FeatureDataD3D12Options5>(Vortice.Direct3D12.Feature.Options5);
                if (opt5.RaytracingTier == RaytracingTier.NotSupported)
                {
                    device.Release();
                    continue;
                }

                _disposableManager.Add(device);
                var device5 = device.QueryInterface<ID3D12Device5>();
                _disposableManager.Add(device5);
                return device5;
            }

            return null;
        }

        private IDXGISwapChain4 CreateSwapChain(IWindowHandle window)
        {
            var desc = new SwapChainDescription1
            {
                Width = window.Width,
                Height = window.Height,
                Format = ViewFormat,
                Stereo = false,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = SwapBufferCount,
                Scaling = Scaling.Stretch,
                SwapEffect = SwapEffect.FlipDiscard,
                AlphaMode = AlphaMode.Unspecified,
                Flags = _tearingSupported ? SwapChainFlags.AllowTearing : SwapChainFlags.None
            };

            var swapChain = _factory.CreateSwapChainForComposition(CommandQueueManager.CommandQueue, desc);
            _disposableManager.Add(swapChain);
            var swapChain4 = swapChain.QueryInterface<IDXGISwapChain4>();
            _disposableManager.Add(swapChain4);
            window.AssignTo(swapChain4);
            return swapChain4;
        }

        private ID3D12DescriptorHeap CreateDescriptorHeap(DescriptorHeapType type, int numDescriptors)
        {
            var desc = new DescriptorHeapDescription()
            {
                DescriptorCount = numDescriptors,
                Type = type
            };
            var descriptorHeap = _device.CreateDescriptorHeap(desc);
            _disposableManager.Add(descriptorHeap);
            return descriptorHeap;
        }

        private void UpdateRenderTargetViews()
        {
            var cpuHandle = _descriptorHeap.GetCPUDescriptorHandleForHeapStart();
            var rtvDescSize = _device.GetDescriptorHandleIncrementSize(DescriptorHeapType.RenderTargetView);
            for (var i = 0; i < SwapBufferCount; i++)
            {
                var backBuffer = _swapChain.GetBuffer<ID3D12Resource>(i);
                _device.CreateRenderTargetView(backBuffer, null, cpuHandle);
                _backBuffers[i] = backBuffer;
                _disposableManager.Add(backBuffer);
                cpuHandle.Ptr += rtvDescSize;
            }
        }

        private void ResourceBarrier(ID3D12GraphicsCommandList2 commandList, ID3D12Resource ressource,
            ResourceStates stateBefore, ResourceStates stateAfter)
        {
            var barrier = new ResourceBarrier(new ResourceTransitionBarrier(ressource, stateBefore, stateAfter));
            commandList.ResourceBarrier(barrier);
        }
    }
}