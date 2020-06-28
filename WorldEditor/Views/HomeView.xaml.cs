using Windows.UI.Xaml.Controls;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using WorldEditor.Entities;
using WorldEditor.ViewModels;


namespace WorldEditor.Views
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class HomeView : Page
    {
        public HomeView()
        {
            this.InitializeComponent();
        }

        private async void Grid_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var handle = new UwpWindowHandle(DirectXPanel);
            if (DataContext is HomeViewModel model)
            {
                if (model.Engine.Initialise(handle))
                {
                    model.Engine.Render();
                    return;
                }
            }

            var messageDialog = new MessageDialog("no Direct X engine");
            await messageDialog.ShowAsync();
        }

        private void DirectXPanel_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!(DataContext is HomeViewModel model) || !model.Engine.IsInitialised)
            {
                return;
            }

            model.Engine.Resize((int) e.NewSize.Width, (int) e.NewSize.Height);
            model.Engine.Render();
        }
    }
}