using System;

namespace Small.Net.Graphic.Core
{
    public interface IEngine : IDisposable
    {
        bool IsInitialised { get; }
        bool Initialise(IWindowHandle window);
        void Resize(int newWidth, int newHeight);

        void Render();
    }
}