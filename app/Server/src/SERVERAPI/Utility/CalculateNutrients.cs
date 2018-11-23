using Microsoft.AspNetCore.Hosting;
using SERVERAPI.Models;
using SERVERAPI.Models.Impl;
using System;
using Agri.Interfaces;
using Agri.Models.Calculate;
using Agri.Models.Farm;
//using static SERVERAPI.Models.StaticData;
using Agri.LegacyData.Models.Impl;
using Agri.Models.Configuration;

namespace SERVERAPI.Utility
{
    public class CalculateNutrients
    {
        private IHostingEnvironment _env;
        private UserData _ud;
        private IAgriConfigurationRepository _sd;

        public CalculateNutrients(IHostingEnvironment env, UserData ud, IAgriConfigurationRepository sd)
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
            FarmManure mymanure = _ud.GetFarmManure(Convert.ToInt32(manure));

            ConversionFactor _cf = _sd.GetConversionFactor();

            decimal potassiumAvailabilityFirstYear = _cf.PotassiumAvailabilityFirstYear;
            decimal potassiumAvailabilityLongTerm = _cf.PotassiumAvailabilityLongTerm;
            decimal potassiumKtoK2Oconversion = _cf.PotassiumKtoK2OConversion;
            decimal phosphorousAvailabilityFirstYear = _cf.PhosphorousAvailabilityFirstYear;
            decimal phosphorousAvailabilityLongTerm = _cf.PhosphorousAvailabilityLongTerm;
            decimal phosphorousPtoP2O5Kconversion = _cf.PhosphorousPtoP2O5Conversion;
            decimal lbPerTonConversion = _cf.PoundPerTonConversion;
            decimal tenThousand = 10000;

            // get conversion factor for selected units to lb/ac
            Unit myunit = _sd.GetUnit(applicationRateUnits);
            decimal conversion = myunit.ConversionlbTon;

            // for solid manures specified in cubic yards per ac, convert application rate to tons/ac
            if (myunit.Id == 6 && mymanure.solid_liquid.ToUpper() == "SOLID")
            {
                Manure manure = _sd.GetManure(mymanure.manureId.ToString());
                applicationRate = applicationRate * manure.CubicYardConversion;
            }


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

            nOrganicMineralizations = GetNMineralization(mymanure.id, region.LocationId);
            nOrganicMineralizations.OrganicN_FirstYear = firstYearOrganicNAvailablityPct / 100; // get data from screen

            //decimal ammoniaRetention = GetAmmoniaRetention(mymanure.id, Convert.ToInt32(applicationSeason));
            decimal ammoniaRetention = ammoniaNRetentionPct / 100; // get data from screen

            // N 1st year lb/ton = [NH4-N ppm/10,000 * NH4 retention + NO3-N/10,000 + Organic N %  * 1st yr Mineralization] * 20

            decimal a = decimal.Divide(mymanure.ammonia, tenThousand) * ammoniaRetention;

            decimal b1 = decimal.Multiply(organicN, nOrganicMineralizations.OrganicN_FirstYear);
            //E07US20
            decimal c1 = a + b1 + Convert.ToDecimal(mymanure.nitrate) / tenThousand;
            decimal N_Firstyear = decimal.Multiply(c1, lbPerTonConversion);
            nutrientInputs.N_FirstYear = Convert.ToInt32(applicationRate * N_Firstyear * conversion);

            // same for long term
            decimal b2 = decimal.Multiply(organicN, nOrganicMineralizations.OrganicN_LongTerm);
            //E07US20
            decimal c2 = a + b2 + Convert.ToDecimal(mymanure.nitrate) / tenThousand;
            decimal N_LongTerm = decimal.Multiply(c2, lbPerTonConversion);
            nutrientInputs.N_LongTerm = Convert.ToInt32(applicationRate * N_LongTerm * conversion);

            return nutrientInputs;
        }

        public decimal GetAmmoniaRetention(int manureid, int seasonapplicationid)
        {
            decimal ammoniaRention = 0;

            FarmManure myManure = _ud.GetFarmManure(manureid);
            
            AmmoniaRetention myAmmoniaRetention = _sd.GetAmmoniaRetention(seasonapplicationid, myManure.dmid);

            ammoniaRention = myAmmoniaRetention.Value.HasValue ? myAmmoniaRetention.Value.Value : 0;

            return ammoniaRention;
        }

        public NOrganicMineralizations GetNMineralization(int manureid, int locationid)
        {
            NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();            

            FarmManure myManure = _ud.GetFarmManure(manureid);

            NitrogenMineralization myNMineralization = _sd.GetNMineralization(myManure.nminerizationid, locationid);            

            nOrganicMineralizations.OrganicN_FirstYear = myNMineralization.FirstYearValue; 
            nOrganicMineralizations.OrganicN_LongTerm = myNMineralization.LongTermValue;

            return nOrganicMineralizations;
        }

    }
}
