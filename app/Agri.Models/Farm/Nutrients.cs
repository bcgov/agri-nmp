using System.Collections.Generic;

namespace Agri.Models.Farm
{
    public class Nutrients
    {
        public Nutrients()
        {
            nutrientManures = new List<NutrientManure>();
            nutrientFertilizers = new List<NutrientFertilizer>();
            nutrientOthers = new List<NutrientOther>();
        }
        public List<NutrientManure> nutrientManures { get; set; }
        public List<NutrientFertilizer> nutrientFertilizers { get; set; }
        public List<NutrientOther> nutrientOthers { get; set; }
    }
}