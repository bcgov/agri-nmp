using System;
using System.Collections.Generic;
using System.Data;
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

        public decimal GetDensity(decimal moistureWholePercent)
        {
            var moisturePercentDecimal = Convert.ToDouble(moistureWholePercent / 100);
            if (moistureWholePercent < 40)
            {
                return .27m;
            }
            else if (moistureWholePercent >= 40 && moistureWholePercent <= 82)
            {
                var result = (7.9386 * Math.Pow(moisturePercentDecimal, 3)) - (16.43 * Math.Pow(moisturePercentDecimal, 2)) +
                             (11.993 * moisturePercentDecimal) - 2.3975;

                return Convert.ToDecimal(result);
            }
            else
            {
                return 0.837m;
            }
        }

        public decimal GetDensityFactoredConversion(decimal moistureWholePercent, string conversionFactor)
        {
            var moisterPercentDecimal = Convert.ToDouble(moistureWholePercent / 100);
            var density = GetDensity(moistureWholePercent);

            var parsedExpression = conversionFactor.Replace("density", density.ToString(), StringComparison.CurrentCultureIgnoreCase);
            var conversion = Convert.ToDecimal(new DataTable().Compute(parsedExpression, null));

            return conversion;
        }

        public decimal GetCubicYardsVolume(ManureMaterialType manureMaterialType,
            decimal moistureWholePercent,
            decimal amountToConvert, 
            AnnualAmountUnits amountUnit)
        {
            if (manureMaterialType == ManureMaterialType.Solid)
            {
                var converstionFactor = _repository
                    .GetSolidMaterialsConversionFactors()
                    .Single(cf => cf.InputUnit == amountUnit);

                var cubicYardsConverted =
                    GetDensityFactoredConversion(moistureWholePercent, converstionFactor.CubicYardsOutput) *
                    amountToConvert;

                return cubicYardsConverted;
            }

            return 0;
        }
        public decimal GetCubicMetersVolume(ManureMaterialType manureMaterialType,
            decimal moistureWholePercent,
            decimal amountToConvert,
            AnnualAmountUnits amountUnit)
        {
            if (manureMaterialType == ManureMaterialType.Solid)
            {
                var converstionFactor = _repository
                    .GetSolidMaterialsConversionFactors()
                    .Single(cf => cf.InputUnit == amountUnit);

                var cubicMetersConverted =
                    GetDensityFactoredConversion(moistureWholePercent, converstionFactor.CubicMetersOutput) *
                    amountToConvert;

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
            decimal moistureWholePercent,
            decimal amountToConvert,
            AnnualAmountUnits amountUnit)
        {
            if (manureMaterialType == ManureMaterialType.Solid)
            {
                var converstionFactor = _repository
                    .GetSolidMaterialsConversionFactors()
                    .Single(cf => cf.InputUnit == amountUnit);

                var tonsConverted =
                    GetDensityFactoredConversion(moistureWholePercent, converstionFactor.MetricTonsOutput) *
                    amountToConvert;

                return tonsConverted;
            }
            else
            {
                return 0;
            }
        }

        public decimal GetSolidsTonsPerAcreApplicationRate(
            decimal moistureWholePercent,
            decimal amountToConvert)
        {
            return 0m;
        }
    }
}
