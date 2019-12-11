using System;
using Agri.Models;
using Agri.Models.Calculate;

namespace Agri.CalculateService
{
    public interface IManureLiquidSolidSeparationCalculator
    {
        SeparatedManure CalculateSeparatedManure(decimal liquidVolumeGallons, int wholePercentLiquidSeparated);
    }

    public class ManureLiquidSolidSeparationCalculator : IManureLiquidSolidSeparationCalculator
    {
        private IManureUnitConversionCalculator _manureUnitConversionCalculator;

        public ManureLiquidSolidSeparationCalculator(IManureUnitConversionCalculator manureUnitConversionCalculator)
        {
            _manureUnitConversionCalculator = manureUnitConversionCalculator;
        }

        public SeparatedManure CalculateSeparatedManure(decimal liquidVolumeToSeparateGallons, int wholePercentLiquidSeparated)
        {
            var solidsSeparatedUSGallons = Convert.ToInt32(liquidVolumeToSeparateGallons * (wholePercentLiquidSeparated / 100M));
            var separatedManure = new SeparatedManure
            {
                LiquidUSGallons = liquidVolumeToSeparateGallons - solidsSeparatedUSGallons
            };

            //var amountToConvert = (100 - wholePercentLiquidSeparated) / 100M * liquidVolumeToSeparateGallons;
            var moistureWholePercent = 70;

            //Divide by Factor of 1 Gallon to Cubic Meter
            var solidsSeperatedCubicMeters = solidsSeparatedUSGallons / _manureUnitConversionCalculator.GetUSGallonsVolume(ManureMaterialType.Liquid,
                                                                    1M,
                                                                    AnnualAmountUnits.CubicMeters);

            separatedManure.SolidTons = Convert.ToInt32(_manureUnitConversionCalculator.GetTonsWeight(ManureMaterialType.Solid,
                                                            moistureWholePercent,
                                                            Convert.ToDecimal(solidsSeperatedCubicMeters),
                                                            AnnualAmountUnits.CubicMeters));

            return separatedManure;
        }
    }
}