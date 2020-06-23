using NUnit.Framework;
using Small.Net.Graphic.Graphic;

namespace Small.Net.Graphic.Test
{
    [TestFixture]
    public class FactoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateFactoryAndAdapterTest()
        {
            var factory = Dx12Factory.CreateFactoryDx12();
            Assert.IsNotNull(factory);
            var adapter = factory.GetHardwareAdapter();
            Assert.IsNotNull(adapter);
            var device = adapter.CreateDevice();
            Assert.IsNotNull(device);
            var commandQueue = device.CreateCommandQueue();
            Assert.IsNotNull(commandQueue);
            commandQueue.Dispose();
            device.Dispose();
            adapter.Dispose();
            factory.Dispose();
        }
    }
}