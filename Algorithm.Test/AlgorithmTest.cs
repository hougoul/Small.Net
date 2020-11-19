using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;

namespace Algorithm.Test
{
    public class AlgorithmTest
    {
        [Test]
        [TestCase(new int[] {1, 2, 3, -2, 5}, 9)]
        [TestCase(new int[] {-1, -2, -3, -4}, -1)]
        [TestCase(new int[] {-2, 1, -3, 4, -1, 2, 1, -5, 4}, 6)]
        public void KadaneTest(int[] array, int expectedResult)
        {
            Assert.AreEqual(expectedResult, MaxSubArraySum(array));
        }

        private static long MaxSubArraySum(int[] array)
        {
            var maxSum = int.MinValue;
            var currentSum = 0;
            foreach (var i in array)
            {
                currentSum = Math.Max(i, currentSum + i);
                maxSum = Math.Max(currentSum, maxSum);
            }

            return maxSum;
        }

        [Test]
        [TestCase(new int[] {1, 2, 3, 4, 5, 6}, new int[] {6, 1, 5, 2, 4, 3})]
        [TestCase(new int[] {10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110},
            new int[] {110, 10, 100, 20, 90, 30, 80, 40, 70, 50, 60})]
        public void ArrangeTest(int[] array, int[] expectedResult)
        {
            ReArrangeArray(array);
            Assert.IsTrue(expectedResult.SequenceEqual(array));
        }

        private static void ReArrangeArray(int[] array)
        {
            var lastIndex = array.Length - 1;
            var firstIndex = 0;
            var currentIndex = 0;
            var result = new int[array.Length];
            while (firstIndex <= lastIndex)
            {
                if (currentIndex % 2 == 0)
                {
                    result[currentIndex] = array[lastIndex];
                    lastIndex--;
                }
                else
                {
                    result[currentIndex] = array[firstIndex];
                    firstIndex++;
                }

                currentIndex++;
            }

            for (var i = 0; i < array.Length; i++)
            {
                array[i] = result[i];
            }
        }

        [TestCase("toto", "tata", false)]
        [TestCase("toto", "toot", true)]
        [TestCase("toto", "too", false)]
        public void AnagramTest(string first, string second, bool expectedResult)
        {
            var watch = Stopwatch.StartNew();
            Assert.AreEqual(expectedResult, IsAnagram(first, second));
            watch.Stop();
            Console.WriteLine(watch.ElapsedTicks);
            watch.Restart();
            Assert.AreEqual(expectedResult, IsAnagram2(first, second));
            watch.Stop();
            Console.WriteLine(watch.ElapsedTicks);
        }

        private static bool IsAnagram(string first, string second)
        {
            return first.Length == second.Length && first.OrderBy(c => c).SequenceEqual(second.OrderBy(c => c));
        }

        private static bool IsAnagram2(string first, string second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            var count = new int[26];
            var a = 'a';
            for (var i = 0; i < first.Length; i++)
            {
                var c = first[i];
                count[c - a] += 1;
                c = second[i];
                count[c - a] -= 1;
            }

            return count.All(c => c == 0);
        }
    }
}