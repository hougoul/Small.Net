using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using WorldEditor.ViewModels;
using WorldEditor.Views;
using Small.Net.Utilities;
using Windows.UI.Xaml;

namespace WorldEditor
{
    public sealed partial class App
    {
        private WinRTContainer container;

        public App()
        {
            Initialize();
            InitializeComponent();
        }

        protected override void Configure()
        {
            container = new WinRTContainer();

            container.RegisterWinRTServices();
            container.RegisterPerRequest(typeof(IDisposableManager), "cleaner", typeof(CommonDisposableManager));

            container.PerRequest<HomeViewModel>();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
                return;
            DisplayRootView<HomeView>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}