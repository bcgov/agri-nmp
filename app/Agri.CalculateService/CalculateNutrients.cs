using System;
using Agri.Data;
using Agri.Models.Calculate;
using Agri.Models.Farm;
using Agri.Models.Configuration;

namespace Agri.CalculateService
{
    public interface ICalculateNutrients
    {
        decimal GetAmmoniaRetention(FarmManure farmManure, int seasonapplicationid);

        NOrganicMineralizations GetNMineralization(FarmManure farmManure, int locationid);

        NutrientInputs GetNutrientInputs(FarmManure farmManure, Region region, decimal applicationRate, string applicationRateUnits, decimal ammoniaNRetentionPct, decimal firstYearOrganicNAvailablityPct);
    }

    public class CalculateNutrients : ICalculateNutrients
    {
        private readonly IAgriConfigurationRepository _sd;

        public CalculateNutrients(IAgriConfigurationRepository sd)
        {
            _sd = sd;
        }

        public NutrientInputs GetNutrientInputs(
            FarmManure farmManure,
            Region region,
            decimal applicationRate,
            string applicationRateUnits,
            decimal ammoniaNRetentionPct,
            decimal firstYearOrganicNAvailablityPct)
        {
            var nutrientInputs = new NutrientInputs();
            var _cf = _sd.GetConversionFactor();

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
            if (myunit.Id == 6 && farmManure.SolidLiquid.ToUpper() == "SOLID")
            {
                Manure manure = _sd.GetManure(farmManure.ManureId);
                applicationRate = applicationRate * manure.CubicYardConversion;
            }

            // get potassium first year
            nutrientInputs.K2O_FirstYear = Convert.ToInt32(decimal.Multiply(applicationRate, farmManure.Potassium)
                                            * lbPerTonConversion
                                            * potassiumKtoK2Oconversion
                                            * potassiumAvailabilityFirstYear
                                            * conversion);

            // get potassium long term
            nutrientInputs.K2O_LongTerm = Convert.ToInt32(decimal.Multiply(applicationRate, farmManure.Potassium)
                                            * lbPerTonConversion
                                            * potassiumKtoK2Oconversion
                                            * potassiumAvailabilityLongTerm
                                            * conversion);

            // get phosphorous first year
            nutrientInputs.P2O5_FirstYear = Convert.ToInt32(decimal.Multiply(applicationRate, farmManure.Phosphorous)
                                            * lbPerTonConversion
                                            * phosphorousPtoP2O5Kconversion
                                            * phosphorousAvailabilityFirstYear
                                            * conversion);

            // get phosphorous long term
            nutrientInputs.P2O5_LongTerm = Convert.ToInt32(decimal.Multiply(applicationRate, farmManure.Phosphorous)
                                            * lbPerTonConversion
                                            * phosphorousPtoP2O5Kconversion
                                            * phosphorousAvailabilityLongTerm
                                            * conversion);

            // get N values
            // Organic N% = Total N% - NH4-N ppm / 10,000
            decimal organicN = farmManure.Nitrogen - Convert.ToDecimal(farmManure.Ammonia) / tenThousand;

            NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            nOrganicMineralizations = GetNMineralization(farmManure, region.LocationId);
            nOrganicMineralizations.OrganicN_FirstYear = firstYearOrganicNAvailablityPct / 100; // get data from screen

            //decimal ammoniaRetention = GetAmmoniaRetention(mymanure.id, Convert.ToInt32(applicationSeason));
            decimal ammoniaRetention = ammoniaNRetentionPct / 100; // get data from screen

            // N 1st year lb/ton = [NH4-N ppm/10,000 * NH4 retention + NO3-N/10,000 + Organic N %  * 1st yr Mineralization] * 20

            decimal a = decimal.Divide(farmManure.Ammonia, tenThousand) * ammoniaRetention;

            decimal b1 = decimal.Multiply(organicN, nOrganicMineralizations.OrganicN_FirstYear);
            //E07US20
            decimal c1 = a + b1 + Convert.ToDecimal(farmManure.Nitrate) / tenThousand;
            decimal N_Firstyear = decimal.Multiply(c1, lbPerTonConversion);
            nutrientInputs.N_FirstYear = Convert.ToInt32(applicationRate * N_Firstyear * conversion);

            // same for long term
            decimal b2 = decimal.Multiply(organicN, nOrganicMineralizations.OrganicN_LongTerm);
            //E07US20
            decimal c2 = a + b2 + Convert.ToDecimal(farmManure.Nitrate) / tenThousand;
            decimal N_LongTerm = decimal.Multiply(c2, lbPerTonConversion);
            nutrientInputs.N_LongTerm = Convert.ToInt32(applicationRate * N_LongTerm * conversion);

            return nutrientInputs;
        }

        public decimal GetAmmoniaRetention(FarmManure farmManure, int seasonapplicationid)
        {
            AmmoniaRetention myAmmoniaRetention = _sd.GetAmmoniaRetention(seasonapplicationid, farmManure.DMId);

            var ammoniaRention = myAmmoniaRetention.Value ?? 0;

            return ammoniaRention;
        }

        public NOrganicMineralizations GetNMineralization(FarmManure farmManure, int locationid)
        {
            NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            NitrogenMineralization myNMineralization = _sd.GetNMineralization(farmManure.NMinerizationId, locationid);

            nOrganicMineralizations.OrganicN_FirstYear = myNMineralization.FirstYearValue;
            nOrganicMineralizations.OrganicN_LongTerm = myNMineralization.LongTermValue;

            return nOrganicMineralizations;
        }
    }
}