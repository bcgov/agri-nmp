using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Calculate;

namespace Agri.CalculateService
{
    public class ManureLiquidSolidSeparationCalculator : IManureLiquidSolidSeparationCalculator
    {
        private IManureUnitConversionCalculator _manureUnitConversionCalculator;

        public ManureLiquidSolidSeparationCalculator(IManureUnitConversionCalculator manureUnitConversionCalculator)
        {
            _manureUnitConversionCalculator = manureUnitConversionCalculator;
        }

        public SeparatedManure CalculateSeparatedManure(int liquidVolumeGallons, int wholePercentLiquidSeparated)
        {
            var separatedManure = new SeparatedManure
            {
                LiquidUSGallons = Convert.ToInt32(liquidVolumeGallons * (wholePercentLiquidSeparated / 100.0))
            };

            var amountToConvert = (100 - wholePercentLiquidSeparated) / 100M * liquidVolumeGallons;
            var moistureWholePercent = 70;

            //Divide by Factor of 1 Gallon to Cubic Meter
            var solidsSeperatedCubicMeters = amountToConvert/_manureUnitConversionCalculator.GetUSGallonsVolume(ManureMaterialType.Liquid,
                                                                    1M,
                                                                    AnnualAmountUnits.CubicMeters);

            separatedManure.SolidTons = Convert.ToInt32( _manureUnitConversionCalculator.GetTonsWeight(ManureMaterialType.Solid,
                                                            moistureWholePercent, 
                                                            Convert.ToDecimal(solidsSeperatedCubicMeters), 
                                                            AnnualAmountUnits.CubicMeters));


            return separatedManure;
        }
    }
}
