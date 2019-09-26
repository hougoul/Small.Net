using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Small.Net.Collection;

namespace Small.Net.Test
{
    [TestFixture]
    public class BinaryHeapTest
    {
        [Test]
        public void Min_BHeap_Test()
        {
            var rnd = new Random();
            var initial = Enumerable.Range(0, 51).OrderBy(x => rnd.Next()).ToList();

            var minHeap = new BinaryHeap<int>(SortDirection.Ascending, initial, Comparer<int>.Default);

            for (var i = 51; i <= 99; i++)
            {
                minHeap.Add(i);
            }

            for (var i = 0; i <= 99; i++)
            {
                var min = minHeap.Extract();
                Assert.AreEqual(min, i);
            }

            //IEnumerable tests.
            Assert.AreEqual(minHeap.Count, minHeap.Count());

            var testSeries = Enumerable.Range(1, 49).OrderBy(x => rnd.Next()).ToList();

            foreach (var item in testSeries)
            {
                minHeap.Add(item);
            }

            for (var i = 1; i <= 49; i++)
            {
                var min = minHeap.Extract();
                Assert.AreEqual(min, i);
            }

            //IEnumerable tests.
            Assert.AreEqual(minHeap.Count, minHeap.Count());
        }


        [Test]
        public void Max_BHeap_Test()
        {
            var rnd = new Random();

            var initial = new List<int>(Enumerable.Range(0, 51)
                .OrderBy(x => rnd.Next()));


            var maxHeap = new BinaryHeap<int>(SortDirection.Descending, initial, Comparer<int>.Default);

            for (var i = 51; i <= 99; i++)
            {
                maxHeap.Add(i);
            }

            for (int i = 0; i <= 99; i++)
            {
                var max = maxHeap.Extract();
                Assert.AreEqual(max, 99 - i);
            }

            //IEnumerable tests.
            Assert.AreEqual(maxHeap.Count, maxHeap.Count());

            var testSeries = Enumerable.Range(1, 49)
                .OrderBy(x => rnd.Next()).ToList();

            foreach (var item in testSeries)
            {
                maxHeap.Add(item);
            }

            for (var i = 1; i <= 49; i++)
            {
                var max = maxHeap.Extract();
                Assert.AreEqual(max, 49 - i + 1);
            }

            //IEnumerable tests.
            Assert.AreEqual(maxHeap.Count, maxHeap.Count());
        }
    }
}