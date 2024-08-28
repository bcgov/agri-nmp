using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Fertilizer: SelectOption
    {
        public Fertilizer()
        {
            LiquidFertilizerDensities = new List<LiquidFertilizerDensity>();
        }
        public string DryLiquid { get; set; }
        public decimal Nitrogen { get; set; }
        public decimal Phosphorous { get; set; }
        public decimal Potassium { get; set; }
        public int SortNum { get; set; }

        public List<LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
    }
}