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

        public List<CropSTKRegion> GetCropStkRegions()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["crop_stk_regioncds"]["crop_stk_regioncd"];
            var cds = new List<CropSTKRegion>();

            foreach (var r in array)
            {
                var cd = new CropSTKRegion()
                {
                    CropId = Convert.ToInt32(r["cropid"].ToString()),
                    SoilTestPotassiumRegionCode = Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString()),
                    PotassiumCropGroupRegionCode =
                        r["potassium_crop_group_region_cd"].ToString() == "null"
                            ? (int?) null
                            : Convert.ToInt32(r["potassium_crop_group_region_cd"].ToString())
                };
                cds.Add(cd);
            }

            return cds;
        }

        public List<CropSTPRegion> GetCropStpRegions()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["crop_stp_regioncds"]["crop_stp_regioncd"];
            var cds = new List<CropSTPRegion>();

            foreach (var r in array)
            {
                var cd = new CropSTPRegion()
                {
                    CropId = Convert.ToInt32(r["cropid"].ToString()),
                    SoilTestPhosphorousRegionCode = Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString()),
                    PhosphorousCropGroupRegionCode = 
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
                    SoilTestPotassiumRegionCode = Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString()),
                    PotassiumCropGroupRegionCode = Convert.ToInt32(r["potassium_crop_group_region_cd"].ToString()),
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
                    SoilTestPhosphorousRegionCode = Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString()),
                    PhosphorousCropGroupRegionCode = Convert.ToInt32(r["phosphorous_crop_group_region_cd"].ToString()),
                    P2O5_Recommend_KgPerHa = Convert.ToInt32(r["p2o5_recommend_kgperha"].ToString()),
                };
                stpRs.Add(stk);
            }

            return stpRs;
        }

        public List<HarvestUnit> GetHarvestUnits()
        {
            var harvestUnits = new List<HarvestUnit>();
            JArray fertTypes = (JArray)rss["agri"]["nmp"]["harvestunits"]["harvestunit"];

            foreach (var r in fertTypes)
            {
                var li = new HarvestUnit()
                {
                    Id = Convert.ToInt16(r["id"].ToString()),
                    Name = r["name"].ToString() 
                };
                harvestUnits.Add(li);
            }

            return harvestUnits;
        }

        public List<LiquidFertilizerDensity> GetLiquidFertilizerDensities()
        {
            var densities = new List<LiquidFertilizerDensity>();
            var array = (JArray)rss["agri"]["nmp"]["liquidfertilizerdensitys"]["liquidfertilizerdensity"];

            foreach (var rec in array)
            {
                var density = new LiquidFertilizerDensity
                {
                    Id = Convert.ToInt32(rec["id"].ToString()),
                    DensityUnitId = Convert.ToInt32(rec["densityunitid"].ToString()),
                    FertilizerId = Convert.ToInt32(rec["fertilizerid"].ToString()),
                    Value = Convert.ToDecimal(rec["value"].ToString())
                };
                densities.Add(density);
            }

            return densities;
        }

        public List<NMineralization> GetNMineralizations()
        {
            var array = (JArray)rss["agri"]["nmp"]["nmineralizations"]["nmineralization"];
            var nmineralizations = new List<NMineralization>();

            foreach (var r in array)
            {
                var nmineralization = new NMineralization()
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString(),
                    Locationid = Convert.ToInt32(r["locationid"].ToString()),
                    FirstYearValue = Convert.ToDecimal(r["firstyearvalue"].ToString()),
                    LongTermValue = Convert.ToDecimal(r["longtermvalue"].ToString()),
                };
                nmineralizations.Add(nmineralization);
            }

            return nmineralizations;
        }

        public List<DM> GetDMs()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["dms"]["dm"];
            var dms = new List<DM>();

            foreach (var r in array)
            {
                var dm = new DM()
                {
                    Id = Convert.ToInt32(r["ID"].ToString()),
                    Name = r["name"].ToString()
                };
                dms.Add(dm);
            };

            return dms;
        }
    }
}
