using NUnit.Framework;
using System;
using Small.Net.Extensions;

namespace Small.Net.Test
{
    [TestFixture]
    public class ObjectExtensionsTest
    {
        [Test]
        public void CopyToTest()
        {
            var obj1 = new CopyObject1()
            {
                Prop2 = 5,
                Prop3 = 10
            };
            var obj2 = obj1.CopyTo<CopyObject1, CopyObject1>();
            Assert.AreEqual(obj1.Prop2, obj2.Prop2);
            Assert.AreEqual(obj1.Prop3, obj2.Prop3);
            var obj3 = obj1.CopyTo<CopyObject1, CopyObject2>();
            Assert.AreEqual(10, obj3.Prop1);
            Assert.AreEqual(5, obj3.Prop2);
            Assert.AreEqual(obj1.Prop3, obj3.Prop3);
        }
    }

    public class CopyObject1 : ICloneable
    {
        public int Prop1 { set => Prop2 = value % 10; }

        public int Prop2 { get; set; }

        public int Prop3 { get; set; }

        public object Clone()
        {
            return new CopyObject1()
            {
                Prop2 = Prop2,
                Prop3 = Prop3
            };
        }
    }

    public class CopyObject2
    {
        public int Prop1 { get; set; } = 10;

        public int Prop2 { get; } = 5;

        public int Prop3 { get; set; }
    }

}
