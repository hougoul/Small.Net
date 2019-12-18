using System;
using System.Collections.Generic;
using NUnit.Framework;
using Small.Net.Collection;

namespace Small.Net.Test
{
    [TestFixture]
    public class BTreeTest
    {
        private class InternalTest
        {
            public int Id { get; set; }

            public DateTime IndexDate { get; set; }
        }

        private List<InternalTest> _withDuplicateKeys;

        [OneTimeSetUp]
        public void Setup()
        {
            _withDuplicateKeys = new List<InternalTest>();
            for (var i = 0; i < 10000; i++)
            {
                _withDuplicateKeys.Add(new InternalTest()
                {
                    Id = i,
                    IndexDate = DateTime.Today.AddDays(i % 75)
                });
            }
        }

        [Test]
        public void AddDuplicateTest()
        {
            var subject = new BpTree<InternalTest, DateTime>(obj => obj.IndexDate);
            foreach (var test in _withDuplicateKeys)
            {
                subject.Add(test);
            }

            Assert.AreEqual(_withDuplicateKeys.Count, subject.Count);
        }
    }
}