using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class SolubilityUnit : SelectOption
    {
        public SolubilityUnit()
        {
            DryFertilizerSolubilities = new List<DryFertilizerSolubility>();
        }

        public decimal ConvFactor { get; set; }

        public List<DryFertilizerSolubility> DryFertilizerSolubilities { get; set; }
    }
}