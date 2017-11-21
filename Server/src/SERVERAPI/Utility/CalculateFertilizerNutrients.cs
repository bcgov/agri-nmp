using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models.Impl;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.Models;
using static SERVERAPI.Models.StaticData;

namespace SERVERAPI.Utility
{
    public class CalculateFertilizerNutrients
    {
        private UserData _ud;
        private Models.Impl.StaticData _sd;

        public CalculateFertilizerNutrients(UserData ud, Models.Impl.StaticData sd)
        {
            _ud = ud;
            _sd = sd;
        }

        public FertilizerNutrients fertilizerNutrients { get; set; }
        public int FertilizerId { get; set; }
        public string FertilizerType { get; set; }
        public decimal ApplicationRate { get; set; }
        public int ApplicationRateUnits { get; set; }
        public decimal Density { get; set; }
        public int DensityUnits { get; set; }
        public int userN { get; set; }
        public int userP2o5 { get; set; }
        public int userK2o { get; set; }
        public bool CustomFertilizer { get; set; }

        // This processing detemines the N P K values in lb per acre for the fertilizer select and appricatio rate/density etc.
        public FertilizerNutrients GetFertilizerNutrients()
        {
            FertilizerNutrients fn = new FertilizerNutrients();

            // get the fertilizer N P K % values from fertlizer look up
            //  for dry fertilizers
            //      N (lb/ac) = Application rate converted to lb/ac * N %
            //      P and K same as above
            //  for wet fertilizers
            //      N (lb/ac) = Application rate converted to lb/ac * N% * Density converted to lb / imp gallons
            //

            decimal applicationRateConversion = 0;
            decimal densityInSelectedUnit = 0;
            decimal densityUnitConversion = 0;
            
            Fertilizer _fertilizer = _sd.GetFertilizer(FertilizerId.ToString());            

            if ((CustomFertilizer && FertilizerType == "dry") || (!CustomFertilizer && _fertilizer.dry_liquid == "dry"))
            {
                densityUnitConversion = 1;
                switch (ApplicationRateUnits)
                { case 1: // application rate in lb/ac no conversion required
                        applicationRateConversion = 1;
                        break;
                  case 2: // application rate in kg/ha, convert to lb/ac
                        ConversionFactor _cf = _sd.GetConversionFactor();
                        applicationRateConversion = _cf.kgperha_lbperac_conversion;
                        break;
                  case 7: // application rate in lb/100 ft squared, convert to lb/ac
                        ConversionFactor _cf1 = _sd.GetConversionFactor();
                        applicationRateConversion = _cf1.lbper1000ftsquared_lbperac_conversion;
                        break;
                }
            }
            else //use liquid fertilizer
            {
                FertilizerUnit _fU = _sd.GetFertilizerUnit(ApplicationRateUnits);
                applicationRateConversion = _fU.conv_to_impgalperac;
                if (CustomFertilizer)
                    densityInSelectedUnit = Density;
                else
                {
                    LiquidFertilizerDensity _lfd = _sd.GetLiquidFertilizerDensity(FertilizerId, DensityUnits);
                    densityInSelectedUnit = _lfd.value;
                }
                DensityUnit _du = _sd.GetDensityUnit(DensityUnits);
                densityUnitConversion = _du.convfactor * densityInSelectedUnit;
            }

            fn.fertilizer_N = ApplicationRate * decimal.Divide(userN,100) * applicationRateConversion * densityUnitConversion;
            fn.fertilizer_P2O5 = ApplicationRate * decimal.Divide(userP2o5, 100) * applicationRateConversion * densityUnitConversion;
            fn.fertilizer_K2O = ApplicationRate * decimal.Divide(userK2o, 100) * applicationRateConversion * densityUnitConversion;

            return fn;
        }
    }
}
