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
            var objHelper = typeof(TestObject).GetObjectReflectionHelper();
            var obj = objHelper.CreateInstance();
            objHelper.SetValue("Prop1", obj, 10);
            Assert.AreEqual(10, objHelper.GetValue("PROP1", obj));

            objHelper.SetValue("prOp2", obj, 12L);
            Assert.AreEqual(12L, objHelper.GetValue("PROP2",obj));

            objHelper.SetValue("prOp2",obj, 100.0m);
            Assert.AreEqual(100L, objHelper.GetValue("PROP2",obj));

            objHelper.SetValue("prOp3",obj, "Y");
            Assert.AreEqual(true, objHelper.GetValue("PROP3", obj));

            objHelper.SetValue("prOp3", obj, 'Y');
            Assert.AreEqual(true, objHelper.GetValue("PROP3",obj));

            objHelper["prOp3"].SetValue(obj, 1);
            Assert.AreEqual(true, objHelper["PROP3"].GetValue(obj));

            objHelper["prOp3"].SetValue(obj, "True");
            Assert.AreEqual(true, objHelper["PROP3"].GetValue(obj));

            objHelper["prOp4"].SetValue(obj, "Test2");
            Assert.AreEqual(EnumTest.Test2, objHelper["PROP4"].GetValue(obj));

            objHelper["prOp4"].SetValue(obj,0);
            Assert.AreEqual(EnumTest.Test1, objHelper["PROP4"].GetValue(obj));
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
