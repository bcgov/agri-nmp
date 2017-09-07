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
    public class CalculateNutrients
    {
        private IHostingEnvironment _env;
        private UserData _ud;
        private Models.Impl.StaticData _sd;

        public CalculateNutrients(IHostingEnvironment env, UserData ud, Models.Impl.StaticData sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
        }

        public string manure { get; set; }
        public string applicationSeason { get; set; }
        public decimal applicationRate { get; set; }
        public string applicationRateUnits { get; set; }
        public decimal ammoniaNRetentionPct { get; set; }
        public decimal firstYearOrganicNAvailablityPct { get; set; }
        public NutrientInputs nutrientInputs { get; set; }

        public NutrientInputs GetNutrientInputs(NutrientInputs nutrientInputs)
        {
            Manure mymanure = _sd.GetManure(manure);

            decimal potassiumAvailabilityFirstYear = 1;
            decimal potassiumAvailabilityLongTerm = 1;
            decimal potassiumKtoK2Oconversion = 1.2M;
            decimal phosphorousAvailabilityFirstYear = 0.7M;
            decimal phosphorousAvailabilityLongTerm = 1;            
            decimal phosphorousPtoP2O5Kconversion = 2.29M;
            decimal lbPerTonConversion = 20;


            // get conversion factor for selected units to lb/ac
            Unit myunit = _sd.GetUnit(applicationRateUnits);
            decimal conversion = myunit.conversion_lbton;

            // get potassium first year
            nutrientInputs.K2O_FirstYear = Convert.ToInt32(decimal.Multiply(applicationRate, mymanure.potassium)
                                            * lbPerTonConversion
                                            * potassiumKtoK2Oconversion
                                            * potassiumAvailabilityFirstYear 
                                            * conversion);

            // get potassium long term
            nutrientInputs.K2O_LongTerm = Convert.ToInt32(decimal.Multiply(applicationRate, mymanure.potassium) 
                                            * lbPerTonConversion
                                            * potassiumKtoK2Oconversion
                                            * potassiumAvailabilityLongTerm 
                                            * conversion);

            // get phosphorous first year
            nutrientInputs.P2O5_FirstYear = Convert.ToInt32(decimal.Multiply(applicationRate, mymanure.phosphorous) 
                                            * lbPerTonConversion 
                                            * phosphorousPtoP2O5Kconversion
                                            * phosphorousAvailabilityFirstYear 
                                            * conversion);

            // get phosphorous long term
            nutrientInputs.P2O5_LongTerm = Convert.ToInt32(decimal.Multiply(applicationRate, mymanure.phosphorous) 
                                            * lbPerTonConversion 
                                            * phosphorousPtoP2O5Kconversion
                                            * phosphorousAvailabilityLongTerm 
                                            * conversion);

            nutrientInputs.N_FirstYear = 0;
            nutrientInputs.N_LongTerm = 0;
                        

            return nutrientInputs;
        }
    }
}
