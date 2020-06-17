using SharpDX.Direct3D;
using SharpDX.Direct3D12;
using SharpDX;
using Windows.UI.Xaml.Controls;
using SharpDX.Mathematics.Interop;
using System.Threading;
using System.Diagnostics;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace WorldEditor.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        private const int SwapBufferCount = 2;
        private Device2 _device;
        private SharpDX.DXGI.SwapChain _swapChain;
        private SharpDX.DXGI.Adapter _adapter;
        private SharpDX.DXGI.Factory2 _factory;
        private CommandQueue _queue;
        private CommandAllocator _commandListAllocator;
        private GraphicsCommandList _commandList;
        private DescriptorHeap _descriptorHeap;
        private Resource _renderTarget;
        private RawRectangle _scissorRectangle;
        private Fence _fence;
        private long _currentFence;
        private AutoResetEvent _eventHandle;
        private RawViewportF _viewPort;
        private int _indexLastSwapBuf;
        private readonly Stopwatch _clock;

        public HomeView()
        {
            this.InitializeComponent();
            _clock = Stopwatch.StartNew();
        }

        private void Grid_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _factory = new SharpDX.DXGI.Factory2();
            _adapter = _factory.Adapters[0];
            using (var defaultDevice = new Device(_adapter, FeatureLevel.Level_12_0))
            {
                _device = defaultDevice.QueryInterface<Device2>();
            }

            var swapChainDescription = new SharpDX.DXGI.SwapChainDescription1()
            {
                // No transparency.
                AlphaMode = SharpDX.DXGI.AlphaMode.Ignore,
                // Double buffer.
                BufferCount = SwapBufferCount,
                // BGRA 32bit pixel format.
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                // Unlike in CoreWindow swap chains, the dimensions must be set.
                Height = (int) (DirectXPanel.RenderSize.Height),
                Width = (int) (DirectXPanel.RenderSize.Width),
                // Default multisampling.
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                // In case the control is resized, stretch the swap chain accordingly.
                Scaling = SharpDX.DXGI.Scaling.Stretch,
                // No support for stereo display.
                Stereo = false,
                // Sequential displaying for double buffering.
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
                // This swapchain is going to be used as the back buffer.
                Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
            };
            // Get the DXGI factory automatically created when initializing the Direct3D device.
            // Create the swap chain and get the highest version available.
            _queue = _device.CreateCommandQueue(CommandListType.Direct);
            using (var factory4 = new SharpDX.DXGI.Factory4())
            {
                SharpDX.DXGI.SwapChain1 swapChain1 =
                    new SharpDX.DXGI.SwapChain1(factory4, _queue, ref swapChainDescription);
                _swapChain = swapChain1.QueryInterface<SharpDX.DXGI.SwapChain2>();
            }

            using (var nativeObject = ComObject.As<SharpDX.DXGI.ISwapChainPanelNative>(DirectXPanel))
            {
                nativeObject.SwapChain = _swapChain;
            }

            _commandListAllocator = _device.CreateCommandAllocator(CommandListType.Direct);
            LoadAssets();
            Render();
        }

        private void Render()
        {
            // record all the commands we need to render the scene into the command list
            PopulateCommandLists();

            // execute the command list
            _queue.ExecuteCommandList(_commandList);

            _swapChain.Present(1, 0);
            _indexLastSwapBuf = (_indexLastSwapBuf + 1) % SwapBufferCount;
            _renderTarget?.Dispose();
            _renderTarget = _swapChain.GetBackBuffer<Resource>(_indexLastSwapBuf);
            _device.CreateRenderTargetView(_renderTarget, null, _descriptorHeap.CPUDescriptorHandleForHeapStart);

            // wait and reset EVERYTHING
            WaitForPrevFrame();
        }

        private void PopulateCommandLists()
        {
            _commandListAllocator.Reset();

            _commandList.Reset(_commandListAllocator, null);

            // setup viewport and scissors
            _commandList.SetViewport(_viewPort);
            _commandList.SetScissorRectangles(_scissorRectangle);

            // Use barrier to notify that we are using the RenderTarget to clear it
            _commandList.ResourceBarrierTransition(_renderTarget, ResourceStates.Present, ResourceStates.RenderTarget);

            // Clear the RenderTarget
            var time = _clock.Elapsed.TotalSeconds;
            _commandList.ClearRenderTargetView(_descriptorHeap.CPUDescriptorHandleForHeapStart,
                new RawColor4((float) System.Math.Sin(time) * 0.25f + 0.5f,
                    (float) System.Math.Sin(time * 0.5f) * 0.4f + 0.6f, 0.4f, 1.0f));

            // Use barrier to notify that we are going to present the RenderTarget
            _commandList.ResourceBarrierTransition(_renderTarget, ResourceStates.RenderTarget, ResourceStates.Present);

            // Execute the command
            _commandList.Close();
        }

        /// <summary>
        /// Setup resources for rendering
        /// </summary>
        private void LoadAssets()
        {
            // Create the descriptor heap for the render target view
            _descriptorHeap = _device.CreateDescriptorHeap(new DescriptorHeapDescription()
            {
                Type = DescriptorHeapType.RenderTargetView,
                DescriptorCount = 1
            });

            // Create the main command list
            _commandList = _device.CreateCommandList(CommandListType.Direct, _commandListAllocator, null);

            // Get the backbuffer and creates the render target view
            _renderTarget = _swapChain.GetBackBuffer<Resource>(0);
            _device.CreateRenderTargetView(_renderTarget, null, _descriptorHeap.CPUDescriptorHandleForHeapStart);

            // Create the viewport
            _viewPort = new RawViewportF()
            {
                X = 0,
                Y = 0,
                Width = (int) DirectXPanel.RenderSize.Width,
                Height = (int) DirectXPanel.RenderSize.Height,
                MinDepth = 0,
                MaxDepth = 3000
            };

            // Create the scissor
            _scissorRectangle = new RawRectangle(0, 0, (int) DirectXPanel.RenderSize.Width,
                (int) DirectXPanel.RenderSize.Height);

            // Create a fence to wait for next frame
            _fence = _device.CreateFence(0, FenceFlags.None);
            _currentFence = 1;

            // Close command list
            _commandList.Close();

            // Create an event handle use for VTBL
            _eventHandle = new AutoResetEvent(false);

            // Wait the command list to complete
            WaitForPrevFrame();
        }

        /// <summary>
        /// Wait the previous command list to finish executing.
        /// </summary>
        private void WaitForPrevFrame()
        {
            // WAITING FOR THE FRAME TO COMPLETE BEFORE CONTINUING IS NOT BEST PRACTICE.
            // This is code implemented as such for simplicity.
            long localFence = _currentFence;
            _queue.Signal(_fence, localFence);
            _currentFence++;

            if (_fence.CompletedValue < localFence)
            {
                _fence.SetEventOnCompletion(localFence, _eventHandle.SafeWaitHandle.DangerousGetHandle());
                _eventHandle.WaitOne();
            }
        }
    }
}