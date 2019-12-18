using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<InternalTest> _randomKeys;

        [OneTimeSetUp]
        public void Setup()
        {
            _withDuplicateKeys = new List<InternalTest>();
            for (var i = 0; i < 1000000; i++)
            {
                _withDuplicateKeys.Add(new InternalTest()
                {
                    Id = i,
                    IndexDate = DateTime.Today.AddDays(i % 550)
                });
            }

            _randomKeys = new List<InternalTest>();
            var random = new Random();
            for (var i = 0; i < 1000000; i++)
            {
                _randomKeys.Add(new InternalTest()
                {
                    Id = 1,
                    IndexDate = new DateTime(random.Next(1970, 2060), random.Next(1, 12), random.Next(1, 28))
                });
            }
        }

        [Test]
        public void AddDuplicateTest()
        {
            var subject = new BpTree<InternalTest, DateTime>(obj => obj.IndexDate);
            var watch = Stopwatch.StartNew();
            foreach (var test in _withDuplicateKeys)
            {
                subject.Add(test);
            }

            watch.Stop();
            Assert.AreEqual(_withDuplicateKeys.Count, subject.Count);
            Console.WriteLine(watch.ElapsedMilliseconds);
        }

        [Test]
        public void AddRandomTest()
        {
            var subject = new BpTree<InternalTest, DateTime>(obj => obj.IndexDate);
            var watch = Stopwatch.StartNew();
            foreach (var test in _randomKeys)
            {
                subject.Add(test);
            }

            watch.Stop();
            Assert.AreEqual(_withDuplicateKeys.Count, subject.Count);
            Console.WriteLine(watch.ElapsedMilliseconds);
        }
    }
}