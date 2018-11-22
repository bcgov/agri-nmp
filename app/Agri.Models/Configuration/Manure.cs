namespace Agri.Models.Configuration
{
    public class Manure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ManureClass { get; set; }
        public string SolidLiquid { get; set; }
        public string Moisture { get; set; }
        public decimal Nitrogen { get; set; }
        public int Ammonia { get; set; }
        public decimal Phosphorous { get; set; }
        public decimal Potassium { get; set; }
        public int DMId { get; set; }
        public int NMineralizationId { get; set; }
        public int SortNum { get; set; }
        public decimal CubicYardConversion { get; set; }
        public decimal Nitrate { get; set; }

        public NitrogenMineralization NMineralization { get; set; }
        public DryMatter Dm { get; set; }
    }
}