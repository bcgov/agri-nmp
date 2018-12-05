using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;

namespace Agri.CalculateService
{
    public class ManureUnitConversionCalculator
    {
        private IAgriConfigurationRepository _repository;

        public ManureUnitConversionCalculator(IAgriConfigurationRepository repository)
        {
            _repository = repository;
        }

        public string GetVolume(ManureMaterialType manureMaterialType, 
            decimal amountToConvert, 
            AnnualAmountUnits amountUnit)
        {
            if (manureMaterialType == ManureMaterialType.Solid)
            {
                var converstionFactor = _repository
                    .GetSolidMaterialsConversionFactors()
                    .Single(cf => cf.InputUnit == amountUnit);

                var cubicYardsConverted = converstionFactor.CubicYardsOutput * amountToConvert;
                var cubicMetersConverted = converstionFactor.CubicMetersOutput * amountToConvert;

                return $"{cubicYardsConverted} yards³ ({cubicMetersConverted} m³)";
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetWeight(ManureMaterialType manureMaterialType,
            decimal amountToConvert,
            AnnualAmountUnits amountUnit)
        {
            if (manureMaterialType == ManureMaterialType.Solid)
            {
                var converstionFactor = _repository
                    .GetSolidMaterialsConversionFactors()
                    .Single(cf => cf.InputUnit == amountUnit);

                var tonsConverted = converstionFactor.MetricTonsOutput * amountToConvert;

                return $"{tonsConverted} tons";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
