using Vortice.Direct3D12;

namespace Small.Net.Graphic.Core
{
    public interface IShape<in T>
    {
        void Draw(T commandList);
    }
}