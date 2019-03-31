using NUnit.Framework;
using System;
using System.Collections.Generic;
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
                Prop3 = 10,
                InternalCollection = new List<CopyObject3>(new [] {new CopyObject3(){ Prop1 = 1, Prop2 = 2}, new CopyObject3() { Prop1 = 3, Prop2 = 4}})
            };
            var obj2 = obj1.CopyTo<CopyObject1, CopyObject1>();
            Assert.AreEqual(obj1.Prop2, obj2.Prop2);
            Assert.AreEqual(obj1.Prop3, obj2.Prop3);
            Assert.AreEqual(obj1.InternalCollection.Count, obj2.InternalCollection.Count);
            for (var i = 0; i < obj1.InternalCollection.Count; ++i)
            {
                var intObj1 = obj1.InternalCollection[i];
                var intObj2 = obj2.InternalCollection[i];
                Assert.AreEqual(intObj1.Prop1, intObj2.Prop1);
                Assert.AreEqual(intObj1.Prop2, intObj2.Prop2);
            }
            var obj3 = obj1.CopyTo<CopyObject1, CopyObject2>();
            Assert.AreEqual(10, obj3.Prop1);
            Assert.AreEqual(5, obj3.Prop2);
            Assert.AreEqual(obj1.Prop3, obj3.Prop3);
            Assert.AreEqual(obj1.InternalCollection.Count, obj3.InternalCollection.Count);
            for (var i = 0; i < obj1.InternalCollection.Count; ++i)
            {
                var intObj1 = obj1.InternalCollection[i];
                var intObj2 = obj3.InternalCollection[i];
                Assert.AreEqual(intObj1.Prop1, intObj2.Prop1);
                Assert.AreEqual(intObj1.Prop2, intObj2.Prop2);
            }
        }
    }

    public class CopyObject1 : ICloneable
    {
        public int Prop1 { set => Prop2 = value % 10; }

        public int Prop2 { get; set; }

        public int Prop3 { get; set; }

        public List<CopyObject3> InternalCollection { get; set; }

        public object Clone()
        {
            var value = new CopyObject1()
            {
                Prop2 = Prop2,
                Prop3 = Prop3,
                InternalCollection = new List<CopyObject3>()
            };
            foreach (var obj in InternalCollection)
            {
                value.InternalCollection.Add((CopyObject3)obj.Clone());
            }
            return value;
        }
    }

    public class CopyObject2
    {
        public int Prop1 { get; set; } = 10;

        public int Prop2 { get; } = 5;

        public int Prop3 { get; set; }
        
        public List<CopyObject4> InternalCollection { get; set; }
    }

    public class CopyObject3 : ICloneable
    {
        public int Prop1 { get; set; }

        public int Prop2 { get; set; }
        public object Clone()
        {
            return new CopyObject3()
            {
                Prop1 = Prop1,
                Prop2 = Prop2
            };
        }
    }

    public class CopyObject4
    {
        public int Prop1 { get; set; }

        public int Prop2 { get; set; }
    }

}
