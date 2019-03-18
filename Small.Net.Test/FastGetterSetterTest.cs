using NUnit.Framework;
using Small.Net.Reflection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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
        }
    }

    public class TestObject
    {
        public int Prop1 { get; set; }

        public long? Prop2 { get; set; }
    } 
}
