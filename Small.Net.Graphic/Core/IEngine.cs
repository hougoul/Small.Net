using System;

namespace Small.Net.Graphic.Core
{
    public interface IEngine : IDisposable
    {
        bool Initialise(IWindowHandle window);
        void Resize(int newWidth, int newHeight);
    }
}
