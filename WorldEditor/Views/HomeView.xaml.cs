using Windows.UI.Xaml.Controls;
using Vortice.DXGI;
using Vortice.Direct3D12;
using Vortice.Direct3D;
using System;
using Small.Net.Utilities;
using Vortice.Direct3D12.Debug;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using Small.Net.Graphic.Interop;
using SharpGen.Runtime;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace WorldEditor.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        private const int SwapBufferCount = 2;
        private const Format ViewFormat = Format.R8G8B8A8_UNorm;
        private const bool VertivalSync = true;
        private const bool TearingSupported = false;
        private readonly IDisposableManager _cleaner;
        private ID3D12Device5 _device;
        private ID3D12CommandQueue _queue;
        private IDXGISwapChain4 _swapChain;
        private ID3D12Resource[] _renderTargets = new ID3D12Resource[SwapBufferCount];
        private ID3D12GraphicsCommandList _commandList;
        private ID3D12CommandAllocator[] _commandListAllocators = new ID3D12CommandAllocator[SwapBufferCount];
        private ID3D12DescriptorHeap _descriptorHeap;
        private IDXGIFactory4 _factory;
        private ID3D12Fence _fence;
        private EventWaitHandle _fenceEvent;
        private long _fenceValue;
        private long[] _frameFenceValues = new long[SwapBufferCount];
        private int _currentBackBufferIndex;
        private int _rtvDescriptorSize;
        private bool _dxDebug;


        private long _frameCounter;
        private Stopwatch _watch;

        public HomeView(/*IDisposableManager cleaner*/)
        {
            _cleaner = /*cleaner*/ new CommonDisposableManager();
            _cleaner.ReverseDispose = true;
            this.InitializeComponent();
            _watch = new Stopwatch();
        }

        private void Grid_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
#if DEBUG
            var result = D3D12.D3D12GetDebugInterface<ID3D12Debug>(out var debugInterface);
            result.CheckError();
            if (result.Success)
            {
                debugInterface.EnableDebugLayer();
                _cleaner.Add(debugInterface);
                _dxDebug = true;
            }
#endif
            DXGI.CreateDXGIFactory2(_dxDebug, out _factory);
            _cleaner.Add(_factory);

            _device = CreateDevice();
            if (_device == null)
            {
                throw new InvalidOperationException("device cannot be null");
            }
            _cleaner.Add(_device);

            // Get the DXGI factory automatically created when initializing the Direct3D device.
            // Create the swap chain and get the highest version available.
            _queue = CreateCommandQueue();
            _cleaner.Add(_queue);

            _swapChain = CreateSwapChain();
            _cleaner.Add(_swapChain);
            _currentBackBufferIndex = _swapChain.GetCurrentBackBufferIndex();

            using (var nativeObject = ComObject.As<ISwapChainPanelNative>(DirectXPanel))
            {
                nativeObject.SwapChain = _swapChain;
            }

            _descriptorHeap = CreateDescriptorHeap(DescriptorHeapType.RenderTargetView, SwapBufferCount);
            _cleaner.Add(_descriptorHeap);
            _rtvDescriptorSize = _device.GetDescriptorHandleIncrementSize(DescriptorHeapType.RenderTargetView);
            UpdateRenderTargetViews();
            for (var i = 0; i < SwapBufferCount; i++)
            {
                _commandListAllocators[0] = CreateCommandAllocator(CommandListType.Direct);
            }
            _commandList = CreateCommandList(_commandListAllocators[_currentBackBufferIndex], CommandListType.Direct);
            _fence = CreateFence();

            _watch.Start();

            Update();
            Render();
        }

        private ID3D12Device5 CreateDevice()
        {
            var adapters = _factory.Adapters1.OrderByDescending(a => (long)a.Description1.DedicatedVideoMemory).ToList();
            for (int i = 0; i < adapters.Count; i++)
            {
                // TODO search for food adapter
                var desc = adapters[i].Description1;
                if ((desc.Flags & AdapterFlags.Software) == AdapterFlags.Software)
                {
                    continue;
                }
                var res = D3D12.D3D12CreateDevice(adapters[i], FeatureLevel.Level_12_1, out var dev);
                if (res.Failure)
                {
                    continue;
                }
                FeatureDataD3D12Options5 opt5 = dev.CheckFeatureSupport<FeatureDataD3D12Options5>(Vortice.Direct3D12.Feature.Options5);
                if (opt5.RaytracingTier != RaytracingTier.Tier1_0)
                {
                    throw new NotSupportedException("Raytracing not supported");
                }
                return dev.QueryInterface<ID3D12Device5>();
            }
            return null;
        }

        private ID3D12CommandQueue CreateCommandQueue()
        {
            var desc = new CommandQueueDescription
            {
                Type = CommandListType.Direct,
                Flags = CommandQueueFlags.None,
                NodeMask = 0
            };
            return _device.CreateCommandQueue(desc);
        }

        private bool CheckTearingSupport()
        {
            // Rather than create the DXGI 1.5 factory interface directly, we create the
            // DXGI 1.4 interface and query for the 1.5 interface. This is to enable the 
            // graphics debugging tools which will not support the 1.5 factory interface 
            // until a future update.
            var factory5 = _factory.QueryInterface<IDXGIFactory5>();
            return factory5.PresentAllowTearing;
        }

        private IDXGISwapChain4 CreateSwapChain()
        {
            SwapChainDescription1 desc = new SwapChainDescription1
            {
                Width = (int)DirectXPanel.RenderSize.Width,
                Height = (int)DirectXPanel.RenderSize.Height,
                Format = ViewFormat,
                Stereo = false,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = SwapBufferCount,
                Scaling = Scaling.Stretch,
                SwapEffect = SwapEffect.FlipDiscard,
                AlphaMode = AlphaMode.Unspecified,
                Flags = CheckTearingSupport() ? SwapChainFlags.AllowTearing : SwapChainFlags.None
            };

            var swapChain = _factory.CreateSwapChainForComposition(_queue, desc);

            return swapChain.QueryInterface<IDXGISwapChain4>();
        }

        private ID3D12DescriptorHeap CreateDescriptorHeap(DescriptorHeapType type, int numDescriptors)
        {
            var desc = new DescriptorHeapDescription()
            {
                DescriptorCount = numDescriptors,
                Type = type
            };
            return _device.CreateDescriptorHeap(desc);
        }

        private void UpdateRenderTargetViews()
        {
            var cpuHandle = _descriptorHeap.GetCPUDescriptorHandleForHeapStart();
            var rtvDescSize = _device.GetDescriptorHandleIncrementSize(DescriptorHeapType.RenderTargetView);
            for (var i = 0; i < SwapBufferCount; i++)
            {
                var backBuffer = _swapChain.GetBuffer<ID3D12Resource>(i);
                _device.CreateRenderTargetView(backBuffer, null, cpuHandle);
                _renderTargets[i] = backBuffer;
                _cleaner.Add(backBuffer);
                cpuHandle.Ptr += rtvDescSize;
            }
        }

        private ID3D12CommandAllocator CreateCommandAllocator(CommandListType type)
        {
            var allocator =  _device.CreateCommandAllocator(type);
            _cleaner.Add(allocator);
            return allocator;
        }

        private ID3D12GraphicsCommandList CreateCommandList(ID3D12CommandAllocator commandAllocator, CommandListType type)
        {
            var list = _device.CreateCommandList(0, type, commandAllocator);
            _cleaner.Add(list);
            list.Close();
            return list;
        }

        private ID3D12Fence CreateFence()
        {
            var fence = _device.CreateFence(0, FenceFlags.None);
            _cleaner.Add(fence);
            _fenceEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
            _cleaner.Add(_fenceEvent);
            return fence;
        }

        private long Signal()
        {
            var fenceSignal = ++_fenceValue;
            _queue.Signal(_fence, fenceSignal);
            return fenceSignal;
        }

        private void WaitForFenceValue(long fenceValue, int waitTimeMs = -1)
        {
            if (_fence.CompletedValue < fenceValue)
            {
                _fence.SetEventOnCompletion(_fenceValue, _fenceEvent);
                _fenceEvent.WaitOne(waitTimeMs);
            }
        }

        private void Flush()
        {
            var value = Signal();
            WaitForFenceValue(value);
        }

        private void Update()
        {
            _frameCounter++;
            if (_watch.ElapsedMilliseconds > 1000)
            {
                _watch.Stop();
                Console.WriteLine($"{_frameCounter / (_watch.ElapsedMilliseconds / 1000)} FPS");
                _frameCounter = 0;
                _watch.Restart();
            }
        }

        private void RessourceBarrier(ID3D12Resource ressource, ResourceStates stateBefore, ResourceStates stateAfter)
        {
            var barrier = new ResourceBarrier(new ResourceTransitionBarrier(ressource, stateBefore, stateAfter));
            _commandList.ResourceBarrier(barrier);
        }

        private void Render()
        {
            var commandAllocator = _commandListAllocators[_currentBackBufferIndex];
            var backBuffer = _renderTargets[_currentBackBufferIndex];
            commandAllocator.Reset();
            _commandList.Reset(commandAllocator);

            // We start rendering
            RessourceBarrier(backBuffer, ResourceStates.Present, ResourceStates.RenderTarget);

            var cpuHandler = _descriptorHeap.GetCPUDescriptorHandleForHeapStart();
            _commandList.ClearRenderTargetView(cpuHandler, Color.DarkCyan);

            // We finish rendering
            RessourceBarrier(backBuffer, ResourceStates.RenderTarget, ResourceStates.Present);
            
            _commandList.Close();
            _queue.ExecuteCommandLists(_commandList);
            var result = _swapChain.Present(VertivalSync ? 1 : 0, TearingSupported && !VertivalSync ? PresentFlags.AllowTearing : PresentFlags.None);
            result.CheckError();
            _frameFenceValues[_currentBackBufferIndex] = Signal();
            _currentBackBufferIndex = _swapChain.GetCurrentBackBufferIndex();
            WaitForFenceValue(_frameFenceValues[_currentBackBufferIndex]);
        }

        private void Page_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Flush();
            _cleaner.Dispose();
        }
    }
}