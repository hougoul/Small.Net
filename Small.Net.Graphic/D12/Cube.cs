using Small.Net.Graphic.Core;
using Vortice.Direct3D12;

namespace Small.Net.Graphic.D12
{
    public class Cube : IShape<ID3D12GraphicsCommandList2>
    {
        private ID3D12Resource _vertexBuffer;
        private VertexBufferView _vertexBufferView;
        private ID3D12Resource _indexBuffer;
        private IndexBufferView _indexBufferView;
        
        public void Draw(ID3D12GraphicsCommandList2 commandList)
        {
            throw new System.NotImplementedException();
        }
    }
}