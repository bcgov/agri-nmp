using Agri.Interfaces;
using Agri.Models.Calculate;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using SERVERAPI.Models.Impl;
using System;

namespace SERVERAPI.Utility
{
    public class CalculateCropRequirementRemoval
    { 
    
    private UserData _ud;
    private IAgriConfigurationRepository _sd;

        public CalculateCropRequirementRemoval(UserData ud,IAgriConfigurationRepository sd)
        {            
            _ud = ud;
            _sd = sd;
        }

        public int cropid { get; set; }
        public int previousCropid { get; set; }
        public decimal yield { get; set; }
        public decimal? crudeProtien { get; set; }
        public CropRequirementRemoval cropRequirementRemoval { get; set; }
        public bool? coverCropHarvested { get; set; }   
        public string fieldName { get; set; }
        public int nCredit { get; set; }        

        public CropRequirementRemoval GetCropRequirementRemoval()
        {
            decimal n_removal = 0;

            ConversionFactor _cf = _sd.GetConversionFactor();

            CropRequirementRemoval crr = new CropRequirementRemoval();
            Crop crop = _sd.GetCrop(cropid);

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
            
            if (!crudeProtien.HasValue || (crudeProtien.HasValue && crudeProtien.Value == 0))
            { 
                decimal tmpDec;
                if (decimal.TryParse(crop.CropRemovalFactorNitrogen.ToString(), out tmpDec))
                    n_removal = tmpDec * yield;
                else
                    n_removal = 0;
            }
            else
                n_removal = decimal.Divide(Convert.ToDecimal(crudeProtien), (_cf.NitrogenProteinConversion * _cf.UnitConversion)) * yield;
            
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

            int regionid = _ud.FarmDetails().FarmRegion.Value;
            Region region = _sd.GetRegion(regionid);

            Field fld = _ud.GetFieldDetails(fieldName);

            if(fld.soilTest == null)
            {
                fld.soilTest = new SoilTest();
                DefaultSoilTest dt = _sd.GetDefaultSoilTest();
                fld.soilTest.valNO3H = dt.Nitrogen;
                fld.soilTest.ValP = dt.Phosphorous;
                fld.soilTest.valK = dt.Potassium;
                fld.soilTest.valPH = dt.pH;
                fld.soilTest.ConvertedKelownaK = dt.ConvertedKelownaK;
                fld.soilTest.ConvertedKelownaP = dt.ConvertedKelownaP;
            }

            int _STP = fld.soilTest.ConvertedKelownaP;
            if (_STP == 0)
                _STP = _cf.DefaultSoilTestKelownaPhosphorous;

            int _STK = fld.soilTest.ConvertedKelownaK;
            if (fld.soilTest.ConvertedKelownaK == 0)
                _STK = _cf.DefaultSoilTestKelownaPotassium;

            // p2o5 recommend calculations
            CropSoilTestPhosphorousRegion cropSTPRegionCd  = _sd.GetCropSTPRegionCd(cropid, region.SoilTestPhosphorousRegionCd);
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
            int? potassium_crop_group_region_cd = cropSTKRegionCd.PotassiumCropGroupRegionCode;
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
                    if (cropYield.Amount != null)
                        crr.N_Requirement = Convert.ToInt16(decimal.Divide(yield, Convert.ToDecimal(cropYield.Amount)) * crop.NitrogenRecommendationPoundPerAcre);
                    else
                        crr.N_Requirement = 0;
                    break;
            }

            // if a previous crop has been ploughed dowm account for the N in the field (passed in as a credit)
            crr.N_Requirement = crr.N_Requirement - nCredit;

            // only reduce to 0
            crr.N_Requirement = (crr.N_Requirement < 0) ? 0 : crr.N_Requirement;

            return crr;
        }


        // default for % Crude Protien = crop.cropremovalfactor_N * 0.625 [N to protien conversion] * 0.5 [unit conversion]
        public decimal GetCrudeProtienByCropId(int _cropid)
        {
            ConversionFactor _cf = _sd.GetConversionFactor();

            decimal cp = 0;
            decimal crfN = 0;
            Crop crop = _sd.GetCrop(_cropid);
            decimal.TryParse(crop.CropRemovalFactorNitrogen.ToString(), out crfN);
            cp = crfN * _cf.NitrogenProteinConversion * _cf.UnitConversion;

            return cp;
        }

        public decimal? GetDefaultYieldByCropId(int _cropid, bool useBushelPerAcreUnits)
        {
            decimal? defaultYield = null;
            int _locationid;
            if (_ud.FarmDetails().FarmRegion.HasValue)
            { 
                _locationid = _sd.GetRegion(_ud.FarmDetails().FarmRegion.Value).LocationId;
                CropYield cy = _sd.GetCropYield(_cropid, _locationid);
                if (cy.Amount.HasValue)
                        defaultYield = cy.Amount.Value;
                if (useBushelPerAcreUnits && defaultYield.HasValue)
                {
                    //E07US18 - convert to bushels per acre
                    Crop crop = _sd.GetCrop(_cropid);
                    if  (crop.HarvestBushelsPerTon.HasValue) 
                        defaultYield = defaultYield * crop.HarvestBushelsPerTon;
                }
            }            

            return defaultYield;
        }
    }
}
