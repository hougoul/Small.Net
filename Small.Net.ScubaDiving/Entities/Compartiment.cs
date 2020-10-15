namespace Small.Net.ScubaDiving.Entities
{
    public struct Compartiment
    {
        public int Id { get; set; }

        public CompartimentType Type { get; set; }

        public double PeriodMin { get; set; }

        public double CoefficientA { get; set; }

        public double CoefficientB { get; set; }

        public double CoefficientK { get; set; }
    }
}