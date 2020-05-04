using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Small.Net.Test.Resources;

namespace Small.Net.Test
{
    [TestFixture]
    public class ExpressionVisitorTest
    {
        private static readonly Expression<Func<int, int>> Sum1 = (a) => 1 + a + 3 + 4;
        private static readonly Expression<Func<int, int, int>> Sum2 = (a, b) => (1 + a) + (3 + b);

        private static readonly Expression<Func<int, int>> Factorial = (n) =>
            n == 0 ? 1 : Enumerable.Range(1, n).Aggregate((p, f) => p * f);

        [Test]
        public void Sum1Test()
        {
            var converter = new SampleExpressionConverter();
            var depth = converter.Convert(Sum1);
            Assert.AreEqual(9, depth);
        }

        [Test]
        public void Sum2Test()
        {
            var converter = new SampleExpressionConverter();
            var depth = converter.Convert(Sum2);
            Assert.AreEqual(10, depth);
        }

        [Test]
        public void FactorialTest()
        {
            var converter = new SampleExpressionConverter();
            var depth = converter.Convert(Factorial);
            Assert.AreEqual(15, depth);
        }

        [Test]
        public void PerformanceTest()
        {
            var converter = new SampleExpressionConverter();
            var watch = Stopwatch.StartNew();
            for (var i = 0; i < 100000; i++)
            {
                var depth = converter.Convert(Factorial);
            }

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
        }
    }
}