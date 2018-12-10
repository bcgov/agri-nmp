using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Configuration;

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

        public decimal GetDensityFactoredConversionUsingMoisture(decimal moistureWholePercent, string conversionFactor)
        {
            var moisterPercentDecimal = Convert.ToDouble(moistureWholePercent / 100);
            var density = GetDensity(moistureWholePercent);

            return GetDenisityFactoredConversion(density, conversionFactor);
        }

        public decimal GetDenisityFactoredConversion(decimal density, string conversionFactor)
        {
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
                    GetDensityFactoredConversionUsingMoisture(moistureWholePercent, converstionFactor.CubicYardsOutput) *
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
                    GetDensityFactoredConversionUsingMoisture(moistureWholePercent, converstionFactor.CubicMetersOutput) *
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
                    GetDensityFactoredConversionUsingMoisture(moistureWholePercent, converstionFactor.MetricTonsOutput) *
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
            decimal amountToConvert,
            ApplicationRateUnits applicationRateUnit)
        {
            var conversionFactor = _repository
                .GetSolidMaterialApplicationTonPerAcreRateConversions()
                .Single(cf => cf.ApplicationRateUnit == applicationRateUnit);

            var densityFactoredConversion =
                GetDensityFactoredConversionUsingMoisture(moistureWholePercent, conversionFactor.TonsPerAcreConversion);

            var tonsConverted = densityFactoredConversion * amountToConvert;

            return tonsConverted;
        }

        public decimal GetSolidsTonsPerAcreApplicationRate(
            int manureId, decimal amountToConvert, ApplicationRateUnits applicationRateUnit)
        {
            var density = _repository.GetManure(manureId.ToString()).CubicYardConversion;
            var conversionFactor = _repository
                .GetSolidMaterialApplicationTonPerAcreRateConversions()
                .Single(cf => cf.ApplicationRateUnit == applicationRateUnit);

            var densityFactoredConversion =
                GetDenisityFactoredConversion(density, conversionFactor.TonsPerAcreConversion);

            var tonsConverted = densityFactoredConversion * amountToConvert;

            return tonsConverted;
        }
    }
}
