using System;
using System.Collections.Generic;
using Agri.Models.StaticData;
using Newtonsoft.Json.Linq;

namespace Agri.LegacyData.Models.Impl
{
    public class StaticDataExtRepository : StaticDataRepository
    {
        public List<AmmoniaRetention> GetAmmoniaRetentions()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["ammoniaretentions"]["ammoniaretention"];
            var ammoniaRetentions = new List<AmmoniaRetention>(); 

            foreach (var r in array)
            {
                var ammoniaRetention = new AmmoniaRetention
                {
                    SeasonApplicationId = Convert.ToInt32(r["seasonapplicatonid"].ToString()),
                    DM = Convert.ToInt32(r["dm"].ToString()),
                    Value = r["value"].ToString() == "null"
                            ? (decimal?)null
                            : Convert.ToDecimal(r["value"].ToString())
                };
                ammoniaRetentions.Add(ammoniaRetention);
            }

            return ammoniaRetentions;
        }

        public List<CropYield> GetCropYields()
        {
            var array = (JArray)rss["agri"]["nmp"]["cropyields"]["cropyield"];
            var cropYields = new List<CropYield>();

            foreach (var r in array)
            {
                var cropYield = new CropYield()
                {
                    CropId = Convert.ToInt32(r["cropid"].ToString()),
                    LocationId = Convert.ToInt32(r["locationid"].ToString()),
                    Amt = r["amt"].ToString() == "null"
                        ? (decimal?) null
                        : Convert.ToDecimal(r["amt"].ToString())
                };

                cropYields.Add(cropYield);
            }

            return cropYields;
        }

        public List<Location> GetLocations()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["locations"]["location"];
            var locations = new List<Location>();

            foreach (var r in array)
            {
                var location = new Location()
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString()
                };
                locations.Add(location);
            }

            return locations;
        }

        public List<CropSTKRegionCd> GetCropStkRegionCds()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["crop_stk_regioncds"]["crop_stk_regioncd"];
            var cds = new List<CropSTKRegionCd>();

            foreach (var r in array)
            {
                var cd = new CropSTKRegionCd()
                {
                    CropId = Convert.ToInt32(r["cropid"].ToString()),
                    SoilTestPotassiumRegionCd = Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString()),
                    PotassiumCropGroupRegionCd =
                        r["potassium_crop_group_region_cd"].ToString() == "null"
                            ? (int?) null
                            : Convert.ToInt32(r["potassium_crop_group_region_cd"].ToString())
                };
                cds.Add(cd);
            }

            return cds;
        }

        public List<CropSTPRegionCd> GetCropStpRegionCds()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["crop_stp_regioncds"]["crop_stp_regioncd"];
            var cds = new List<CropSTPRegionCd>();

            foreach (var r in array)
            {
                var cd = new CropSTPRegionCd()
                {
                    CropId = Convert.ToInt32(r["cropid"].ToString()),
                    SoilTestPhosphorousRegionCd = Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString()),
                    PhosphorousCropGroupRegionCd = 
                        r["phosphorous_crop_group_region_cd"].ToString() == "null"
                            ? (int?)null
                            : Convert.ToInt32(r["phosphorous_crop_group_region_cd"].ToString())
                };
                cds.Add(cd);
            }

            return cds;
        }



        public List<STKKelownaRange> GetSTKKelownaRanges()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["stk_kelowna_ranges"]["stk_kelowna_range"];
            var ranges = new List<STKKelownaRange>();

            foreach (var r in array)
            {
                var range = new STKKelownaRange
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Range = r["range"].ToString(),
                    RangeLow = Convert.ToInt32(r["range_low"].ToString()),
                    RangeHigh = Convert.ToInt32(r["range_high"].ToString()),
            };

                ranges.Add(range);
            }
        
            return ranges;
        }

        public List<STPKelownaRange> GetSTPKelownaRanges()
        {
            JArray array = (JArray) rss["agri"]["nmp"]["stp_kelowna_ranges"]["stp_kelowna_range"];
            var ranges = new List<STPKelownaRange>();

            foreach (var r in array)
            {
                var range = new STPKelownaRange
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Range = r["range"].ToString(),
                    RangeLow = Convert.ToInt32(r["range_low"].ToString()),
                    RangeHigh = Convert.ToInt32(r["range_high"].ToString()),
                };

                ranges.Add(range);
            }

            return ranges;
        }

        public List<STKRecommend> GetSTKRecommendations()
        {
            var stkRs = new List<STKRecommend>();
            JArray array = (JArray)rss["agri"]["nmp"]["stk_recommends"]["stk_recommend"];

            foreach (var r in array)
            {
                var stk = new STKRecommend
                {
                    STKKelownaRangeId = Convert.ToInt32(r["stk_kelowna_rangeid"].ToString()),
                    SoilTestPotassiumRegionCd = Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString()),
                    PotassiumCropGroupRegionCd = Convert.ToInt32(r["potassium_crop_group_region_cd"].ToString()),
                    K2O_Recommend_kgPeHa = Convert.ToInt32(r["k2o_recommend_kgperha"].ToString()),
                };
                stkRs.Add(stk);
            }

            return stkRs;
        }

        public List<STPRecommend> GetSTPRecommendations()
        {
            var stpRs = new List<STPRecommend>();
            JArray array = (JArray) rss["agri"]["nmp"]["stp_recommends"]["stp_recommend"];

            foreach (var r in array)
            {
                var stk = new STPRecommend
                {
                    STPKelownaRangeId = Convert.ToInt32(r["stp_kelowna_rangeid"].ToString()),
                    SoilTestPhosphorousRegionCd = Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString()),
                    PhosphorousCropGroupRegionCd = Convert.ToInt32(r["phosphorous_crop_group_region_cd"].ToString()),
                    P2O5_Recommend_KgPerHa = Convert.ToInt32(r["p2o5_recommend_kgperha"].ToString()),
                };
                stpRs.Add(stk);
            }

            return stpRs;
        }
    }
}
