using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;

namespace Algorithm.Test
{
    public class KadaneTest
    {
        [Test]
        [TestCase(new long[] {1, 2, 3, -2, 5}, 9)]
        [TestCase(new long[] {-1, -2, -3, -4}, -1)]
        [TestCase(new long[] {-2, 1, -3, 4, -1, 2, 1, -5, 4}, 6)]
        public void Test1(long[] array, int expectedResult)
        {
            Assert.AreEqual(expectedResult, MaxSubArraySum(array));
        }

        private static long MaxSubArraySum(long[] array)
        {
            var maxSum = long.MinValue;
            var currentSum = 0L;
            foreach (var i in array)
            {
                currentSum = Math.Max(i, currentSum + i);
                maxSum = Math.Max(currentSum, maxSum);
            }

            return maxSum;
        }
    }
}