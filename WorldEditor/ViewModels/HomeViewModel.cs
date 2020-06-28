using System;
using Caliburn.Micro;
using Small.Net.Graphic.Core;

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
    }
}