namespace Small.Net.ScubaDiving.Entities
{
    public class Tissue
    {
        public Tissue()
        {
            // Pressure at ocean level
            PpHe = 0.0;
            PpN2 = 0.7452;
        }

        public double PpHe { get; set; }

        public double PpN2 { get; set; }
    }
}