using System;

namespace Small.Net.ScubaDiving.Entities
{
    public struct Gaz
    {
        public static readonly Gaz Air = new Gaz() {PpO2 = 0.21, PpHe = 0.0, PpN2 = 0.79};
        public static readonly Gaz Nx30 = new Gaz() {PpO2 = 0.3, PpHe = 0.0, PpN2 = 0.7};
        public static readonly Gaz Nx32 = new Gaz() {PpO2 = 0.32, PpHe = 0.0, PpN2 = 0.68};
        public static readonly Gaz Nx36 = new Gaz() {PpO2 = 0.36, PpHe = 0.0, PpN2 = 0.64};
        public static readonly Gaz Nx40 = new Gaz() {PpO2 = 0.4, PpHe = 0.0, PpN2 = 0.6};
        public static readonly Gaz DecoNx50 = new Gaz() {PpO2 = 0.5, PpHe = 0.0, PpN2 = 0.5};
        public static readonly Gaz DecoNx80 = new Gaz() {PpO2 = 0.8, PpHe = 0.0, PpN2 = 0.2};
        public static readonly Gaz DecoO2 = new Gaz() {PpO2 = 1.0, PpHe = 0.0, PpN2 = 0.0};

        public double PpO2 { get; set; }

        public double PpHe { get; set; }

        public double PpN2 { get; set; }

        /// <summary>
        /// Calculate Partial Pressure for this gaz
        /// </summary>
        /// <param name="pressure">outside pressure in bar</param>
        /// <returns></returns>
        public PartialPressure GetPartialPressureFor(double pressure)
        {
            return new PartialPressure()
            {
                PpO2 = pressure * PpO2,
                PpHe = pressure * PpHe,
                PpN2 = pressure * PpN2
            };
        }

        public bool IsValid()
        {
            return Math.Abs(1.0 - (PpHe + PpN2 + PpO2)) < ScubaConstants.Epsilon;
        }
    }
}