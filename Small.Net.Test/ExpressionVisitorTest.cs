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
            var converter = new SampleExpressionVisitor();
            var depth = converter.Compute(Sum1).Result;
            Assert.AreEqual(9, depth);
        }

        [Test]
        public void Sum2Test()
        {
            var converter = new SampleExpressionVisitor();
            var depth = converter.Compute(Sum2).Result;
            Assert.AreEqual(10, depth);
        }

        [Test]
        public void FactorialTest()
        {
            var converter = new SampleExpressionVisitor();
            var depth = converter.Compute(Factorial).Result;
            Assert.AreEqual(17, depth);
        }

        [Test]
        public void PerformanceTest()
        {
            var converter = new SampleExpressionVisitor();
            var watch = Stopwatch.StartNew();
            for (var i = 0; i < 100000; i++)
            {
                var depth = converter.Compute(Factorial).Result;
            }

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
        }
    }
}