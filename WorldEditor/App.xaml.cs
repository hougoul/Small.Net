using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using WorldEditor.Views;
using Small.Net.Utilities;
using Small.Net.Graphic.Core;
using Small.Net.Graphic.D12;
using Autofac;
using System.Linq;

namespace WorldEditor
{
    public sealed partial class App
    {
        private readonly ContainerBuilder _builder;
        private IContainer _container;
        private FrameAdapter _rootFrame;

        public App()
        {
            _builder = new ContainerBuilder();
            Initialize();
            InitializeComponent();
        }

        protected override void Configure()
        {
            _builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            _builder.Register(x => _rootFrame).As<INavigationService>().SingleInstance();
            _builder.RegisterType<CommonDisposableManager>().As<IDisposableManager>();
            _builder.RegisterType<Dx12Engine>().As<IEngine>();
            HandleConfigure(_builder);

            _container = _builder.Build();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            _rootFrame = new FrameAdapter(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
                return;
            DisplayRootView<HomeView>();
        }

        protected override object GetInstance(Type service, string key)
        {
            object instance;
            if (string.IsNullOrEmpty(key))
            {
                if (_container.TryResolve(service, out instance))
                    return instance;
            }
            else
            {
                if (_container.TryResolveNamed(key, service, out instance))
                    return instance;
            }

            throw new Exception(string.Format("Could not locate any instances of service {0}.", service.Name));
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectProperties(instance);
        }

        protected override void OnUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private void HandleConfigure(ContainerBuilder builder)
        {
            /*Register all types by default*/
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                .AsSelf()
                .InstancePerDependency();
        }
    }
}