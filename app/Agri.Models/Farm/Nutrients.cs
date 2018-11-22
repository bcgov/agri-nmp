using System.Collections.Generic;

namespace Agri.Models.Farm
{
    public class Nutrients
    {
        public List<NutrientManure> nutrientManures { get; set; }
        public List<NutrientFertilizer> nutrientFertilizers { get; set; }
        public List<NutrientOther> nutrientOthers { get; set; }
    }
}