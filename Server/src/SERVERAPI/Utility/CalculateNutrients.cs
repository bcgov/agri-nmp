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

            decimal potassiumRate = mymanure.potassium;
            decimal phosphorousRate = mymanure.phosphorous;
            string solidLiquid = mymanure.solid_liquid;
            decimal potassiumAvailabilityFirstYear = 1;
            decimal potassiumAvailabilityLongTerm = 1;
            decimal phosphorousAvailabilityFirstYear = 0.7M;
            decimal phosphorousAvailabilityLongTerm = 0.9M;

            // get conversion factor for selected units to lb/ac
            Unit myunit = _sd.GetUnit(applicationRateUnits);
            decimal conversion = myunit.conversion_lbton;

            // get potassium first year
            nutrientInputs.K2O_FirstYear = Convert.ToInt32(decimal.Multiply(applicationRate, mymanure.potassium)
                                            * Convert.ToDecimal(20) * Convert.ToDecimal(1.2)
                                            * potassiumAvailabilityFirstYear * conversion);

            // get potassium long term
            nutrientInputs.K2O_LongTerm = Convert.ToInt32(decimal.Multiply(applicationRate, mymanure.potassium) 
                                            * Convert.ToDecimal(20) * Convert.ToDecimal(1.2) 
                                            * potassiumAvailabilityLongTerm * conversion);


            // get phosphorous first year
            nutrientInputs.P2O5_FirstYear = Convert.ToInt32(decimal.Multiply(applicationRate, mymanure.phosphorous) 
                                            * Convert.ToDecimal(20) * Convert.ToDecimal(1.2) 
                                            * phosphorousAvailabilityFirstYear * conversion);

            // get phosphorous long term
            nutrientInputs.P2O5_LongTerm = Convert.ToInt32(decimal.Multiply(applicationRate, mymanure.phosphorous) 
                                            * Convert.ToDecimal(20) * Convert.ToDecimal(1.2) 
                                            * phosphorousAvailabilityLongTerm * conversion);

            nutrientInputs.N_FirstYear = 0;
            nutrientInputs.N_LongTerm = 0;
                        

            return nutrientInputs;
        }
    }
}
