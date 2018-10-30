using System.Collections.Generic;

namespace Agri.Models.StaticData
{
    public class Fertilizer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DryLiquid { get; set; }
        public decimal Nitrogen { get; set; }
        public decimal Phosphorous { get; set; }
        public decimal Potassium { get; set; }
        public int SortNum { get; set; }

        public List<LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
    }
}