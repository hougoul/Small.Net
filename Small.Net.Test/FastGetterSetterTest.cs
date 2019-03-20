using NUnit.Framework;
using Small.Net.Reflection;

namespace Small.Net.Test
{
    [TestFixture]
    public class FastGetterSetterTest
    {
        [Test]
        public void IGetterSetterTest()
        {
            var objType = typeof(TestObject);
            var properties = objType.GetPropertiesAccessor();
            var obj = new TestObject();
            properties["Prop1"].SetValue(obj, 10);
            Assert.AreEqual(10, properties["PROP1"].GetValue(obj));

            properties["prOp2"].SetValue(obj, 12L);
            Assert.AreEqual(12L, properties["PROP2"].GetValue(obj));

            properties["prOp2"].SetValue(obj, 100.0m);
            Assert.AreEqual(100L, properties["PROP2"].GetValue(obj));

            properties["prOp3"].SetValue(obj, "Y");
            Assert.AreEqual(true, properties["PROP3"].GetValue(obj));

            properties["prOp3"].SetValue(obj, 'Y');
            Assert.AreEqual(true, properties["PROP3"].GetValue(obj));

            properties["prOp3"].SetValue(obj, 1);
            Assert.AreEqual(true, properties["PROP3"].GetValue(obj));

            properties["prOp3"].SetValue(obj, "True");
            Assert.AreEqual(true, properties["PROP3"].GetValue(obj));
            
            properties["prOp4"].SetValue(obj, "Test2");
            Assert.AreEqual(EnumTest.Test2, properties["PROP4"].GetValue(obj));
            
            properties["prOp4"].SetValue(obj,0);
            Assert.AreEqual(EnumTest.Test1, properties["PROP4"].GetValue(obj));
        }
    }

    public enum EnumTest
    {
        Test1 = 0,
        Test2 = 1
    }

    public class TestObject
    {
        public int Prop1 { get; set; }

        public long? Prop2 { get; set; }

        public bool Prop3 { get; set; }

        public EnumTest Prop4 { get; set; }
    } 
}
