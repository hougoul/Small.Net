using System;

namespace Small.Net.Graphic.Graphic
{
    public interface ISwapChain : IDisposable
    {
        /// <summary>
        /// Use for UWP window only
        /// </summary>
        /// <param name="container"></param>
        void SetToUwpSwapChainPanel(object container);
    }
}
