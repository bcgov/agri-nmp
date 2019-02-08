using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Fertilizer: ConfigurationBase
    {
        public Fertilizer()
        {
            LiquidFertilizerDensities = new List<LiquidFertilizerDensity>();
        }

        [Key]
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