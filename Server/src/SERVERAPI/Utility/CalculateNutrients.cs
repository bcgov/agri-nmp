﻿using System;
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
        public NOrganicMineralizations nOrganicMineralizations { get; set; }

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
            decimal tenThousand = 10000;



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

            // get N values
            // Organic N% = Total N% - NH4-N ppm / 10,000
            decimal organicN = mymanure.nitrogen - (Convert.ToDecimal(mymanure.ammonia) / tenThousand);

            NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            int regionid = _ud.FarmDetails().farmRegion.Value;
            Region region = _sd.GetRegion(regionid);            

            nOrganicMineralizations = GetNMineralization(mymanure.id, region.locationid);
            nOrganicMineralizations.OrganicN_FirstYear = firstYearOrganicNAvailablityPct / 100; // get data from screen

            //decimal ammoniaRetention = GetAmmoniaRetention(mymanure.id, Convert.ToInt32(applicationSeason));
            decimal ammoniaRetention = ammoniaNRetentionPct / 100; // get data from screen

            // N 1st year lb/ton = [NH4-N ppm/10,000 * NH4 retention + NO3-N/10,000 + Organic N %  * 1st yr Mineralization] * 20

            decimal a = decimal.Divide(mymanure.ammonia, tenThousand) * ammoniaRetention;

            decimal b1 = decimal.Multiply(organicN, nOrganicMineralizations.OrganicN_FirstYear);
            decimal c1 = a + 0 + b1;
            decimal N_Firstyear = decimal.Multiply(c1, lbPerTonConversion);
            nutrientInputs.N_FirstYear = Convert.ToInt32(applicationRate * N_Firstyear * conversion);

            // same for long term
            decimal b2 = decimal.Multiply(organicN, nOrganicMineralizations.OrganicN_LongTerm);
            decimal c2 = a + 0 + b2;
            decimal N_LongTerm = decimal.Multiply(c2, lbPerTonConversion);
            nutrientInputs.N_LongTerm = Convert.ToInt32(applicationRate * N_LongTerm * conversion);

            return nutrientInputs;
        }

        public decimal GetAmmoniaRetention(int manureid, int seasonapplicationid)
        {
            decimal ammoniaRention = 0;

            Manure myManure = _sd.GetManure(manureid.ToString());
            
            AmmoniaRetention myAmmoniaRetention = _sd.GetAmmoniaRetention(seasonapplicationid, myManure.dmid);

            ammoniaRention = myAmmoniaRetention.value;

            return ammoniaRention;
        }

        public NOrganicMineralizations GetNMineralization(int manureid, int locationid)
        {
            NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();            

            Manure myManure = _sd.GetManure(manureid.ToString());

            NMineralization myNMineralization = _sd.GetNMineralization(myManure.nminerizationid, locationid);            

            nOrganicMineralizations.OrganicN_FirstYear = myNMineralization.firstyearvalue; 
            nOrganicMineralizations.OrganicN_LongTerm = myNMineralization.longtermvalue;

            return nOrganicMineralizations;
        }

    }
}
