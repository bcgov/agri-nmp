using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;

namespace Agri.CalculateService
{
    public class ManureUnitConversionCalculator : IManureUnitConversionCalculator
    {
        private IAgriConfigurationRepository _repository;

        public ManureUnitConversionCalculator(IAgriConfigurationRepository repository)
        {
            _repository = repository;
        }

        public decimal GetCubicYardsVolume(ManureMaterialType manureMaterialType, 
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

                return cubicYardsConverted;
            }

            return 0;
        }
        public decimal GetCubicMetersVolume(ManureMaterialType manureMaterialType,
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

                return cubicMetersConverted;
            }

            return 0;
        }

        public decimal GetUSGallonsVolume(ManureMaterialType manureMaterialType,
            decimal amountToConvert,
            AnnualAmountUnits amountUnit)
        {
            if (manureMaterialType == ManureMaterialType.Liquid)
            {

                var converstionFactor = _repository
                    .GetLiquidMaterialsConversionFactors()
                    .Single(cf => cf.InputUnit == amountUnit);

                var usGallonsCoverted = converstionFactor.USGallonsOutput * amountToConvert;

                return usGallonsCoverted;
            }

            return 0;
        }

        public decimal GetTonsWeight(ManureMaterialType manureMaterialType,
            decimal amountToConvert,
            AnnualAmountUnits amountUnit)
        {
            if (manureMaterialType == ManureMaterialType.Solid)
            {
                var converstionFactor = _repository
                    .GetSolidMaterialsConversionFactors()
                    .Single(cf => cf.InputUnit == amountUnit);

                var tonsConverted = converstionFactor.MetricTonsOutput * amountToConvert;

                return tonsConverted;
            }
            else
            {
                return 0;
            }
        }
    }
}
