using Agri.Data;
using Agri.Models.Calculate;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using System;
using System.Linq;

namespace Agri.CalculateService
{
    public interface ICalculateCropRequirementRemoval
    {
        CropRequirementRemoval GetCropRequirementRemoval(int cropid, decimal yield, decimal? crudeProtien, bool? coverCropHarvested, int nCredit, int regionId, Field field);
        CropRequirementRemoval GetCropRequirementRemovalBlueberries(Field field, FieldCrop crop, string leafTissueP, string leafTissueK);

        decimal GetCrudeProtienByCropId(int cropid);

        decimal? GetDefaultYieldByCropId(FarmDetails farmDetails, int cropid, bool useBushelPerAcreUnits);
    }

    public class CalculateCropRequirementRemoval : ICalculateCropRequirementRemoval
    {
        private readonly IAgriConfigurationRepository _sd;

        public CalculateCropRequirementRemoval(IAgriConfigurationRepository sd)
        {
            _sd = sd;
        }

        public CropRequirementRemoval GetCropRequirementRemoval(
             int cropid,
            decimal yield,
            decimal? crudeProtien,
            bool? coverCropHarvested,
            int nCredit,
            int regionId,
            Field field)
        {
            ConversionFactor _cf = _sd.GetConversionFactor();

            var crr = new CropRequirementRemoval();
            Crop crop = _sd.GetCrop(cropid);

            decimal n_removal;
            //      For testing we're using Soil Test Kelowna of 65ppm, should actually by 100+ (say 101)
            //
            //      Nutrient removal
            //          P2O5 = crop.cropremovalfactor_P2O5 * yield
            //          K2O = crop.cropremovalfactor_K2O * yield
            //          N = crop.cropremovalfactor_N * yield
            //          if crude_protien exists and is not default
            //                  crop.cropremovalfactor_N = (crude protien / (0.625 * 0.5))
            //                  N = crop.cropremovalfactor_N * yield
            //           else
            //                  N = crop.cropremovalfactor_N * yield
            //
            //      Note for Cover crops (only)
            //          if Cover Crop harvested
            //              don't change numbers
            //          if Cover crop not harvested
            //              set all removal amts to zero

            if (!crudeProtien.HasValue || crudeProtien.HasValue && crudeProtien.Value == 0)
            {
                decimal tmpDec;
                if (decimal.TryParse(crop.CropRemovalFactorNitrogen.ToString(), out tmpDec))
                    n_removal = tmpDec * yield;
                else
                    n_removal = 0;
            }
            else
                n_removal = decimal.Divide(Convert.ToDecimal(crudeProtien), _cf.NitrogenProteinConversion * _cf.UnitConversion) * yield;

            crr.P2O5_Removal = Convert.ToInt32(crop.CropRemovalFactorP2O5 * yield);
            crr.K2O_Removal = Convert.ToInt32(crop.CropRemovalFactorK2O * yield);
            crr.N_Removal = Convert.ToInt32(n_removal);

            if (coverCropHarvested.HasValue && coverCropHarvested.Value == false)
            {
                crr.P2O5_Removal = 0;
                crr.K2O_Removal = 0;
                crr.N_Removal = 0;
            }

            //      Crop Requirement
            //          P205
            //              get region.soil_test_phospherous_region_cd
            //              get phosphorous_crop_group_region_cd  = crop_stp_regioncd(cropid, region.soil_test_phospherous_region_cd)
            //              get stp_kelowna_range.id usign default ST Kelowna (65) between range_low and range_high
            //              get P2O5 recommedation  = stp_recommend(stp_kelowna_range.id, region.soil_test_phospherous_region_cd, phosphorous_crop_group_region_cd)
            //
            //          same as above for K2O
            //
            //          For N
            //          Look up crop.n_recommcd
            //          if 0 or 1
            //              N = crop.n_recomm_lbperac
            //          if 2
            //              same as N removal
            //          if 3
            //              get default yield = cropyield(cropid, locationid)
            //              N = (yield / default yield) * crop.n_recomm_lbperac

            var region = _sd.GetRegion(regionId);

            if (field.SoilTest == null)
            {
                field.SoilTest = new SoilTest();
                var dt = _sd.GetDefaultSoilTest();
                field.SoilTest.valNO3H = dt.Nitrogen;
                field.SoilTest.ValP = dt.Phosphorous;
                field.SoilTest.valK = dt.Potassium;
                field.SoilTest.valPH = dt.pH;
                field.SoilTest.ConvertedKelownaK = dt.ConvertedKelownaK;
                field.SoilTest.ConvertedKelownaP = dt.ConvertedKelownaP;
            }

            int _STP = field.SoilTest.ConvertedKelownaP;
            if (_STP == 0)
                _STP = _cf.DefaultSoilTestKelownaPhosphorous;

            int _STK = field.SoilTest.ConvertedKelownaK;
            if (field.SoilTest.ConvertedKelownaK == 0)
                _STK = _cf.DefaultSoilTestKelownaPotassium;

            // p2o5 recommend calculations
            CropSoilTestPhosphorousRegion cropSTPRegionCd = _sd.GetCropSTPRegionCd(cropid, region.SoilTestPhosphorousRegionCd);
            int? phosphorous_crop_group_region_cd = cropSTPRegionCd.PhosphorousCropGroupRegionCode;
            SoilTestPhosphorousKelownaRange sTPKelownaRange = _sd.GetSTPKelownaRangeByPpm(_STP);
            int stp_kelowna_range_id = sTPKelownaRange.Id;
            if (phosphorous_crop_group_region_cd == null)
                crr.P2O5_Requirement = 0;
            else
            {
                SoilTestPhosphorousRecommendation sTPRecommend = _sd.GetSTPRecommend(stp_kelowna_range_id, region.SoilTestPhosphorousRegionCd, Convert.ToInt16(phosphorous_crop_group_region_cd));
                crr.P2O5_Requirement = Convert.ToInt32(Convert.ToDecimal(sTPRecommend.P2O5RecommendationKilogramPerHectare) * _cf.KilogramPerHectareToPoundPerAcreConversion);
            }

            // k2o recommend calculations
            CropSoilTestPotassiumRegion cropSTKRegionCd = _sd.GetCropSTKRegionCd(cropid, region.SoilTestPotassiumRegionCd);
            int? potassium_crop_group_region_cd = cropSTKRegionCd?.PotassiumCropGroupRegionCode;
            SoilTestPotassiumKelownaRange sTKKelownaRange = _sd.GetSTKKelownaRangeByPpm(_STK);
            int stk_kelowna_range_id = sTKKelownaRange.Id;
            if (potassium_crop_group_region_cd == null)
                crr.K2O_Requirement = 0;
            else
            {
                SoilTestPotassiumRecommendation sTKRecommend = _sd.GetSTKRecommend(stk_kelowna_range_id, region.SoilTestPotassiumRegionCd, Convert.ToInt16(potassium_crop_group_region_cd));
                crr.K2O_Requirement = Convert.ToInt32(Convert.ToDecimal(sTKRecommend.K2ORecommendationKilogramPerHectare) * _cf.KilogramPerHectareToPoundPerAcreConversion);
            }

            // n recommend calculations -note the excel n_recommd are zero based, the static data is 1 based
            switch (crop.NitrogenRecommendationId)
            {
                case 1:
                    crr.N_Requirement = Convert.ToInt16(crop.NitrogenRecommendationPoundPerAcre);
                    break;

                case 2:
                    crr.N_Requirement = Convert.ToInt16(crop.NitrogenRecommendationPoundPerAcre);
                    break;

                case 3:
                    crr.N_Requirement = crr.N_Removal;
                    break;

                case 4:
                    CropYield cropYield = _sd.GetCropYield(cropid, region.LocationId);
                    if (cropYield?.Amount != null)
                        crr.N_Requirement = Convert.ToInt16(decimal.Divide(yield, Convert.ToDecimal(cropYield.Amount)) * crop.NitrogenRecommendationPoundPerAcre);
                    else
                        crr.N_Requirement = 0;
                    break;
            }

            // if a previous crop has been ploughed dowm account for the N in the field (passed in as a credit)
            crr.N_Requirement = crr.N_Requirement - nCredit;

            // only reduce to 0
            crr.N_Requirement = crr.N_Requirement < 0 ? 0 : crr.N_Requirement;

            return crr;
        }

        public CropRequirementRemoval GetCropRequirementRemovalBlueberries(Field fld, FieldCrop crop, string leafTissueP, string leafTissueK)
        {
            var crr = new CropRequirementRemoval();

            var NGmPlantArr = new (string key, decimal value)[]
            {
                        ("1", 6), ("2", 8.5m), ("3", 14), ("4", 23), ("5", 28),
                        ("6", 31),  ("7", 40), ("8", 45), ("9 or more", 63)
            };
            string plantAge = crop.plantAgeYears;
            int plantsPerAcre = crop.numberOfPlantsPerAcre ?? 0;
            decimal tempN = NGmPlantArr.Where(item => item.key == plantAge).Select(element => element.value).FirstOrDefault();
            bool sawdust = crop.willSawdustBeApplied ?? false;
            crr.N_Requirement = Convert.ToInt32(Math.Round((plantsPerAcre * tempN / 1000 / 1.12m + (sawdust ? 25 : 0)), 0));


            int tempReqP2O5 = 0;

            var soilTestP = fld.SoilTest.ValP;
            if (leafTissueP == "< 0.08")
            {
                tempReqP2O5 = (soilTestP < 100) ? 63 : 40;
            }
            else if (leafTissueP == "0.08 - 0.10")
            {
                tempReqP2O5 = (soilTestP < 100) ? 40 : 0;
            }
            else if (leafTissueP == "> 0.10" || true)
            {
                tempReqP2O5 = 0;
            }
            crr.P2O5_Requirement = Convert.ToInt32(tempReqP2O5);


            int tempReqK2O = 0;

            if (leafTissueK == "< 0.2")
            {
                tempReqK2O = 103;
            }
            else if (leafTissueK == "0.2 - 0.4")
            {
                tempReqK2O = 76;
            }
            else if (leafTissueP == "> 0.4" || true)
            {
                tempReqK2O = 0;
            }
            crr.K2O_Requirement = Convert.ToInt32(tempReqK2O);


            decimal tempRemP2O5 = crop.yield;
            bool isPrunedAndRemoved = crop.willPlantsBePruned ?? false && crop.whereWillPruningsGo == "Removed from field";
            tempRemP2O5 = tempRemP2O5 * 0.687m + (isPrunedAndRemoved ? 3.435m : 0);
            crr.P2O5_Removal = Convert.ToInt32(Math.Round(tempRemP2O5, 0));


            decimal tempRemK2O = crop.yield;
            tempRemK2O = tempRemK2O * 3.509m + (isPrunedAndRemoved ? 7.865m : 0);
            crr.K2O_Removal = Convert.ToInt32(Math.Round(tempRemK2O, 0));

            return crr;
        }

            // default for % Crude Protien = crop.cropremovalfactor_N * 0.625 [N to protien conversion] * 0.5 [unit conversion]
            public decimal GetCrudeProtienByCropId(int cropid)
        {
            ConversionFactor _cf = _sd.GetConversionFactor();
            Crop crop = _sd.GetCrop(cropid);
            decimal crfN;
            decimal.TryParse(crop.CropRemovalFactorNitrogen.ToString(), out crfN);
            decimal cp = crfN * _cf.NitrogenProteinConversion * _cf.UnitConversion;

            return cp;
        }

        public decimal? GetDefaultYieldByCropId(FarmDetails farmDetails, int _cropid, bool useBushelPerAcreUnits)
        {
            decimal? defaultYield = null;
            int _locationid;
            if (farmDetails.FarmRegion.HasValue)
            {
                _locationid = _sd.GetRegion(farmDetails.FarmRegion.Value).LocationId;
                CropYield cy = _sd.GetCropYield(_cropid, _locationid);
                if (cy != null && cy.Amount.HasValue)
                    defaultYield = cy.Amount.Value;
                if (useBushelPerAcreUnits && defaultYield.HasValue)
                {
                    //E07US18 - convert to bushels per acre
                    Crop crop = _sd.GetCrop(_cropid);
                    if (crop.HarvestBushelsPerTon.HasValue)
                        defaultYield = defaultYield * crop.HarvestBushelsPerTon;
                }
            }

            return defaultYield;
        }
    }
}