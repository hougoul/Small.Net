using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using Small.Net.Graphic.Core;
using WorldEditor.Entities;

namespace WorldEditor.ViewModels
{
    public class HomeViewModel : Screen
    {
        public HomeViewModel(IEngine engine)
        {
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        ~HomeViewModel()
        {
            Engine?.Dispose();
        }

        public IEngine Engine { get; private set; }

        public async void OnLoaded(SwapChainPanel screenView)
        {
            var handle = new UwpWindowHandle(screenView);
            if (Engine.Initialise(handle))
            {
                Engine.Render();
                return;
            }

            var messageDialog = new MessageDialog("no Direct X engine");
            await messageDialog.ShowAsync();
        }

        public void OnResize(SizeChangedEventArgs e)
        {
            if (!Engine.IsInitialised)
            {
                return;
            }

            Engine.Resize((int) e.NewSize.Width, (int) e.NewSize.Height);
            Engine.Render();
        }
    }
}