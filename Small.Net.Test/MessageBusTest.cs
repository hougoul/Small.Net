using NUnit.Framework;
using Small.Net.Message;
using System;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class MessageBusTest
    {
        [Test]
        public void MonoThread()
        {
            var bus = new MessageBus<int>();
            var count = 0;
            var subscribe1 = bus.Subscribe((v) => {
                count++;
                Assert.AreEqual(1, v);
                });
            var subscribe2 = bus.Subscribe((v) => count+=v);
            bus.Publish(1);
            Assert.AreEqual(2, count);
            subscribe1.Dispose();
            bus.Publish(2);
            subscribe2.Dispose();
            bus.Publish(2);
            Assert.AreEqual(4, count);
        }

        [Test]
        public void MultiThread()
        {
            var bus = new MessageBus<int>();
            var count = 0;
            var subscribe1 = bus.Subscribe((v) => {
                Console.WriteLine(v);
                count += v;
            });
            var taskFactory = new TaskFactory();
            var task = taskFactory.StartNew(() =>
            {
                for (var i = 0; i < 10; i++) bus.Publish(i);
            });
            for (var i = 0; i < 10; i++) bus.Publish(1);
            task.Wait();
            Assert.AreEqual(55, count);
        }
    }
}