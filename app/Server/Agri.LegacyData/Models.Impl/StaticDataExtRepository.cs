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
                    DryMatter = Convert.ToInt32(r["dm"].ToString()),
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
                    Amount = r["amt"].ToString() == "null"
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

        public List<CropSoilTestPotassiumRegion> GetCropSoilTestPotassiumRegions()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["crop_stk_regioncds"]["crop_stk_regioncd"];
            var cds = new List<CropSoilTestPotassiumRegion>();

            foreach (var r in array)
            {
                var cd = new CropSoilTestPotassiumRegion()
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

        public List<CropSoilTestPhosphorousRegion> GetCropSoilTestPhosphorousRegions()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["crop_stp_regioncds"]["crop_stp_regioncd"];
            var cds = new List<CropSoilTestPhosphorousRegion>();

            foreach (var r in array)
            {
                var cd = new CropSoilTestPhosphorousRegion()
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



        public List<SoilTestPotassiumKelownaRange> GetSoilTestPotassiumKelownaRanges()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["stk_kelowna_ranges"]["stk_kelowna_range"];
            var ranges = new List<SoilTestPotassiumKelownaRange>();

            foreach (var r in array)
            {
                var range = new SoilTestPotassiumKelownaRange
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

        public List<SoilTestPhosphorousKelownaRange> GetSoilTestPhosphorousKelownaRanges()
        {
            JArray array = (JArray) rss["agri"]["nmp"]["stp_kelowna_ranges"]["stp_kelowna_range"];
            var ranges = new List<SoilTestPhosphorousKelownaRange>();

            foreach (var r in array)
            {
                var range = new SoilTestPhosphorousKelownaRange
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

        public List<SoilTestPotassiumRecommendation> GetSoilTestPotassiumRecommendations()
        {
            var stkRs = new List<SoilTestPotassiumRecommendation>();
            JArray array = (JArray)rss["agri"]["nmp"]["stk_recommends"]["stk_recommend"];

            foreach (var r in array)
            {
                var stk = new SoilTestPotassiumRecommendation
                {
                    SoilTestPotassiumKelownaRangeId = Convert.ToInt32(r["stk_kelowna_rangeid"].ToString()),
                    SoilTestPotassiumRegionCode = Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString()),
                    PotassiumCropGroupRegionCode = Convert.ToInt32(r["potassium_crop_group_region_cd"].ToString()),
                    K2ORecommendationKilogramPerHectare = Convert.ToInt32(r["k2o_recommend_kgperha"].ToString()),
                };
                stkRs.Add(stk);
            }

            return stkRs;
        }

        public List<SoilTestPhosphorousRecommendation> GetSoilTestPhosphorousRecommendations()
        {
            var stpRs = new List<SoilTestPhosphorousRecommendation>();
            JArray array = (JArray) rss["agri"]["nmp"]["stp_recommends"]["stp_recommend"];

            foreach (var r in array)
            {
                var stk = new SoilTestPhosphorousRecommendation
                {
                    SoilTestPhosphorousKelownaRangeId = Convert.ToInt32(r["stp_kelowna_rangeid"].ToString()),
                    SoilTestPhosphorousRegionCode = Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString()),
                    PhosphorousCropGroupRegionCode = Convert.ToInt32(r["phosphorous_crop_group_region_cd"].ToString()),
                    P2O5RecommendationKilogramPerHectare = Convert.ToInt32(r["p2o5_recommend_kgperha"].ToString()),
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

        public List<NitrogenMineralization> GetNitrogeMineralizations()
        {
            var array = (JArray)rss["agri"]["nmp"]["nmineralizations"]["nmineralization"];
            var nmineralizations = new List<NitrogenMineralization>();

            foreach (var r in array)
            {
                var nmineralization = new NitrogenMineralization()
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString(),
                    LocationId = Convert.ToInt32(r["locationid"].ToString()),
                    FirstYearValue = Convert.ToDecimal(r["firstyearvalue"].ToString()),
                    LongTermValue = Convert.ToDecimal(r["longtermvalue"].ToString()),
                };
                nmineralizations.Add(nmineralization);
            }

            return nmineralizations;
        }

        public List<DryMatter> GetDryMatters()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["dms"]["dm"];
            var dms = new List<DryMatter>();

            foreach (var r in array)
            {
                var dm = new DryMatter()
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
