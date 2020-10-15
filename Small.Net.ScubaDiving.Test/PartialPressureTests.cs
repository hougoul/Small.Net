using System;
using System.Collections.Generic;
using NUnit.Framework;
using Small.Net.ScubaDiving.Entities;

namespace Small.Net.ScubaDiving.Test
{
    public class PartialPressureTests
    {
        private const double Epsilon = 0.00000001;

        [Test, TestCaseSource(nameof(CreateTestCase))]
        public void NitroxPartialPressure_CalculationEqualsExpectedValue(TestData data)
        {
            Assert.IsTrue(data.Gaz.IsValid());
            var result = data.Gaz.GetPartialPressureFor(data.Pressure);
            Assert.IsTrue(Math.Abs(data.ExpectedPpO2 - result.PpO2) < ScubaConstants.Epsilon);
            Assert.IsTrue(Math.Abs(data.ExpectedPpN2 - result.PpN2) < ScubaConstants.Epsilon);
        }

        private static IEnumerable<TestData> CreateTestCase()
        {
            yield return new TestData() {Gaz = Gaz.Air, Pressure = 1.0, ExpectedPpO2 = 0.21, ExpectedPpN2 = 0.79};
            yield return new TestData() {Gaz = Gaz.Air, Pressure = 1.6, ExpectedPpO2 = 0.336, ExpectedPpN2 = 1.264};
            yield return new TestData() {Gaz = Gaz.Air, Pressure = 2.0, ExpectedPpO2 = 0.42, ExpectedPpN2 = 1.58};
            yield return new TestData() {Gaz = Gaz.Air, Pressure = 3.0, ExpectedPpO2 = 0.63, ExpectedPpN2 = 2.37};
            yield return new TestData() {Gaz = Gaz.Air, Pressure = 4.0, ExpectedPpO2 = 0.84, ExpectedPpN2 = 3.16};
            yield return new TestData() {Gaz = Gaz.Air, Pressure = 5.0, ExpectedPpO2 = 1.05, ExpectedPpN2 = 3.95};
            yield return new TestData() {Gaz = Gaz.Air, Pressure = 6.0, ExpectedPpO2 = 1.26, ExpectedPpN2 = 4.74};
            yield return new TestData() {Gaz = Gaz.Air, Pressure = 7.0, ExpectedPpO2 = 1.47, ExpectedPpN2 = 5.53};
            yield return new TestData() {Gaz = Gaz.Nx30, Pressure = 2.0, ExpectedPpO2 = 0.6, ExpectedPpN2 = 1.4};
            yield return new TestData() {Gaz = Gaz.Nx30, Pressure = 3.0, ExpectedPpO2 = 0.9, ExpectedPpN2 = 2.1};
            yield return new TestData() {Gaz = Gaz.Nx30, Pressure = 4.0, ExpectedPpO2 = 1.2, ExpectedPpN2 = 2.8};
            yield return new TestData() {Gaz = Gaz.Nx30, Pressure = 5.0, ExpectedPpO2 = 1.5, ExpectedPpN2 = 3.5};
            yield return new TestData() {Gaz = Gaz.Nx32, Pressure = 2.0, ExpectedPpO2 = 0.64, ExpectedPpN2 = 1.36};
            yield return new TestData() {Gaz = Gaz.Nx32, Pressure = 3.0, ExpectedPpO2 = 0.96, ExpectedPpN2 = 2.04};
            yield return new TestData() {Gaz = Gaz.Nx32, Pressure = 4.0, ExpectedPpO2 = 1.28, ExpectedPpN2 = 2.72};
            yield return new TestData() {Gaz = Gaz.Nx32, Pressure = 5.0, ExpectedPpO2 = 1.6, ExpectedPpN2 = 3.4};
            yield return new TestData() {Gaz = Gaz.Nx36, Pressure = 2.0, ExpectedPpO2 = 0.72, ExpectedPpN2 = 1.28};
            yield return new TestData() {Gaz = Gaz.Nx36, Pressure = 3.0, ExpectedPpO2 = 1.08, ExpectedPpN2 = 1.92};
            yield return new TestData() {Gaz = Gaz.Nx36, Pressure = 4.0, ExpectedPpO2 = 1.44, ExpectedPpN2 = 2.56};
        }

        public struct TestData
        {
            public Gaz Gaz { get; set; }
            public double Pressure { get; set; }
            public double ExpectedPpO2 { get; set; }
            public double ExpectedPpN2 { get; set; }
        }
    }
}