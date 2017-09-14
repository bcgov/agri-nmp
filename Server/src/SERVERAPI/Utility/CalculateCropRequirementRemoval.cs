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
    public class CalculateCropRequirementRemoval
    { 

    private IHostingEnvironment _env;
    private UserData _ud;
    private Models.Impl.StaticData _sd;

        public CalculateCropRequirementRemoval(IHostingEnvironment env, UserData ud, Models.Impl.StaticData sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
        }

        public int cropid { get; set; }
        public int previousCropid { get; set; }
        public decimal yield { get; set; }
        public int? crudeProtien { get; set; }
        public CropRequirementRemoval cropRequirementRemoval { get; set; }
        decimal n_protien_conversion = 0.625M;
        decimal unit_conversion = 0.5M;
        int defaultSoilTestKelownaP = 65;
        int defaultSoilTestKelownaK = 100;
        decimal kgperha_lbperac_conversion = 0.892176122M;

        public CropRequirementRemoval GetCropRequirementRemoval()
        {
            decimal n_removal = 0;
            

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

            if (crudeProtien == null)
            { 
                decimal tmpDec;
                if (decimal.TryParse(crop.cropremovalfactor_N.ToString(), out tmpDec))
                    n_removal = tmpDec * yield;
                else
                    n_removal = 0;
            }
            else
                n_removal = decimal.Divide(Convert.ToDecimal(crudeProtien), (n_protien_conversion * unit_conversion)) * yield;

            crr.P2O5_Removal = Convert.ToInt32(crop.cropremovalfactor_P2O5 * yield);
            crr.K2O_Removal = Convert.ToInt32(crop.cropremovalfactor_K2O * yield);
            crr.N_Removal = Convert.ToInt32(n_removal);

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

            int regionid = _ud.FarmDetails().farmRegion.Value;
            Region region = _sd.GetRegion(regionid);

            // p2o5 recommend calculations
            CropSTPRegionCd cropSTPRegionCd  = _sd.GetCropSTPRegionCd(cropid, region.soil_test_phospherous_region_cd);
            int? phosphorous_crop_group_region_cd = cropSTPRegionCd.phosphorous_crop_group_region_cd;
            STPKelownaRange sTPKelownaRange = _sd.GetSTPKelownaRangeByPpm(defaultSoilTestKelownaP);
            int stp_kelowna_range_id = sTPKelownaRange.id;
            if (phosphorous_crop_group_region_cd == null)
                crr.P2O5_Requirement = 0;
            else
            {
                STPRecommend sTPRecommend = _sd.GetSTPRecommend(stp_kelowna_range_id, region.soil_test_phospherous_region_cd, Convert.ToInt16(phosphorous_crop_group_region_cd));
                crr.P2O5_Requirement = Convert.ToInt32(Convert.ToDecimal(sTPRecommend.p2o5_recommend_kgperha) * kgperha_lbperac_conversion);
            }

            // k2o recommend calculations
            CropSTKRegionCd cropSTKRegionCd = _sd.GetCropSTKRegionCd(cropid, region.soil_test_potassium_region_cd);
            int? potassium_crop_group_region_cd = cropSTKRegionCd.potassium_crop_group_region_cd;
            STKKelownaRange sTKKelownaRange = _sd.GetSTKKelownaRangeByPpm(defaultSoilTestKelownaK);
            int stk_kelowna_range_id = sTKKelownaRange.id;
            if (potassium_crop_group_region_cd == null)
                crr.K2O_Requirement = 0;
            else
            {
                STKRecommend sTKRecommend = _sd.GetSTKRecommend(stk_kelowna_range_id, region.soil_test_potassium_region_cd, Convert.ToInt16(potassium_crop_group_region_cd));
                crr.K2O_Requirement = Convert.ToInt32(Convert.ToDecimal(sTKRecommend.k2o_recommend_kgperha) * kgperha_lbperac_conversion);
            }

            // n recommend calculations -note the excel n_recommd are zero based, the statid data is 1 based
            switch (crop.n_recommcd)
            {
                case 1:
                    crr.N_Requirement = Convert.ToInt16(crop.n_recomm_lbperac);
                    break;
                case 2:
                    crr.N_Requirement = Convert.ToInt16(crop.n_recomm_lbperac);
                    break;
                case 3:
                    crr.N_Requirement = crr.N_Removal;
                    break;
                case 4:
                    CropYield cropYield = _sd.GetCropYield(cropid, region.locationid);
                    if (cropYield.amt != null)
                        crr.N_Requirement = Convert.ToInt16(decimal.Divide(yield, Convert.ToDecimal(cropYield.amt)) * crop.n_recomm_lbperac);
                    else
                        crr.N_Requirement = 0;
                    break;
            }

            return crr;
        }


        // default for % Crude Protien = crop.cropremovalfactor_N * 0.625 [N to protien conversion] * 0.5 [unit conversion]
        public int GetCrudeProtienByCropId(int _cropid)
        {
            int cp = 0;
            decimal crfN = 0;
            Crop crop = _sd.GetCrop(_cropid);
            decimal.TryParse(crop.cropremovalfactor_N.ToString(), out crfN);
            cp = Convert.ToUInt16(crfN * n_protien_conversion * unit_conversion);

            return cp;
        }

    }


}
