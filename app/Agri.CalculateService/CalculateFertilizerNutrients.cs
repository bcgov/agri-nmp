﻿using Agri.Data;
using Agri.Models.Calculate;
using Agri.Models.Configuration;

namespace Agri.CalculateService
{
    public interface ICalculateFertilizerNutrients
    {
        FertilizerNutrients GetFertilizerNutrients(int FertilizerId, string fertilizerType, decimal applicationRate, int applicationRateUnits, decimal density, int densityUnits, decimal userN, decimal userP2o5, decimal userK2o, bool customFertilizer);
    }

    public class CalculateFertilizerNutrients : ICalculateFertilizerNutrients
    {
        private readonly IAgriConfigurationRepository _sd;

        public CalculateFertilizerNutrients(IAgriConfigurationRepository sd)
        {
            _sd = sd;
        }

        // This processing detemines the N P K values in lb per acre for the fertilizer select and appricatio rate/density etc.
        public FertilizerNutrients GetFertilizerNutrients(int FertilizerId,
           string fertilizerType,
           decimal applicationRate,
           int applicationRateUnits,
           decimal density,
           int densityUnits,
           decimal userN,
           decimal userP2o5,
           decimal userK2o,
           bool customFertilizer)
        {
            var fn = new FertilizerNutrients();

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

            if (customFertilizer && fertilizerType == "dry" || !customFertilizer && _fertilizer.DryLiquid == "dry")
            {
                densityUnitConversion = 1;
                switch (applicationRateUnits)
                {
                    case 1: // application rate in lb/ac no conversion required
                        applicationRateConversion = 1;
                        break;

                    case 2: // application rate in kg/ha, convert to lb/ac
                        ConversionFactor _cf = _sd.GetConversionFactor();
                        applicationRateConversion = _cf.KilogramPerHectareToPoundPerAcreConversion;
                        break;

                    case 7: // application rate in lb/100 ft squared, convert to lb/ac
                        ConversionFactor _cf1 = _sd.GetConversionFactor();
                        applicationRateConversion = _cf1.PoundPer1000FtSquaredToPoundPerAcreConversion;
                        break;
                }
            }
            else //use liquid fertilizer
            {
                FertilizerUnit _fU = _sd.GetFertilizerUnit(applicationRateUnits);
                applicationRateConversion = _fU.ConversionToImperialGallonsPerAcre;
                if (customFertilizer)
                    densityInSelectedUnit = density;
                else
                {
                    LiquidFertilizerDensity _lfd = _sd.GetLiquidFertilizerDensity(FertilizerId, densityUnits);
                    densityInSelectedUnit = _lfd.Value;
                }
                DensityUnit _du = _sd.GetDensityUnit(densityUnits);
                densityUnitConversion = _du.ConvFactor * densityInSelectedUnit;
            }

            fn.fertilizer_N = applicationRate * decimal.Divide(userN, 100) * applicationRateConversion * densityUnitConversion;
            fn.fertilizer_P2O5 = applicationRate * decimal.Divide(userP2o5, 100) * applicationRateConversion * densityUnitConversion;
            fn.fertilizer_K2O = applicationRate * decimal.Divide(userK2o, 100) * applicationRateConversion * densityUnitConversion;

            return fn;
        }
    }
}