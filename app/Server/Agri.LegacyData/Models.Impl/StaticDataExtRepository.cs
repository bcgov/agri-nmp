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

        public List<UserPrompt> GetUserPromts()
        {
            var array = (JArray)rss["agri"]["nmp"]["userprompts"]["userprompt"];
            var userPrompts = new List<UserPrompt>();

            foreach (var r in array)
            {
                var userPrompt = new UserPrompt()
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = Convert.ToString(r["name"].ToString()),
                    Text = Convert.ToString(r["text"].ToString())
                };

                userPrompts.Add(userPrompt);
            }

            return userPrompts;
        }

        public List<ExternalLink> GetExternalLinks()
        {
            var array = (JArray)rss["agri"]["nmp"]["externallinks"]["externallink"];
            var externalLinks = new List<ExternalLink>();

            foreach (var r in array)
            {
                var externalLink = new ExternalLink()
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = Convert.ToString(r["name"].ToString()),
                    Url = Convert.ToString(r["url"].ToString())
                };

                externalLinks.Add(externalLink);
            }

            return externalLinks;
        }

        public List<SoilTestPhosphorousRange> GetSoilTestPhosphorousRanges()
        {
            var array = (JArray)rss["agri"]["nmp"]["soiltestranges"]["phosphorous"];
            var soilTestPhosphorousRanges = new List<SoilTestPhosphorousRange>();

            foreach (var r in array)
            {
                var soilTestPhosphorousRange = new SoilTestPhosphorousRange()
                {
                    // Id = Convert.ToInt32(r["id"].ToString()),
                    UpperLimit = Convert.ToInt32(r["upperlimit"].ToString()),
                    Rating = Convert.ToString(r["rating"].ToString())
                };

                soilTestPhosphorousRanges.Add(soilTestPhosphorousRange);
            }

            return soilTestPhosphorousRanges;
        }

        public List<SoilTestPotassiumRange> GetSoilTestPotassiumRanges()
        {
            var array = (JArray)rss["agri"]["nmp"]["soiltestranges"]["potassium"];
            var soilTestPotassiumRanges = new List<SoilTestPotassiumRange>();

            foreach (var r in array)
            {
                var soilTestPotassiumRange = new SoilTestPotassiumRange()
                {
                    // Id = Convert.ToInt32(r["id"].ToString()),
                    UpperLimit = Convert.ToInt32(r["upperlimit"].ToString()),
                    Rating = Convert.ToString(r["rating"].ToString())
                };

                soilTestPotassiumRanges.Add(soilTestPotassiumRange);
            }

            return soilTestPotassiumRanges;
        }

        public List<Message> GetMessages()
        {
            var array = (JArray)rss["agri"]["nmp"]["messages"]["message"];
            var messages = new List<Message>();

            foreach (var r in array)
            {
                var message = new Message()
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Text = Convert.ToString(r["text"].ToString()),
                    DisplayMessage = Convert.ToString(r["displayMessage"].ToString()),
                    Icon = Convert.ToString(r["icon"].ToString()),
                    BalanceType = Convert.ToString(r["balanceType"].ToString()),
                    BalanceLow = Convert.ToInt32(r["balance_low"].ToString()),
                    BalanceHigh = Convert.ToInt32(r["balance_high"].ToString()),
                    SoilTestLow = Convert.ToInt32(r["soiltest_low"].ToString()),
                    SoilTestHigh = Convert.ToInt32(r["soiltest_high"].ToString()),
                    Balance1Low = Convert.ToInt32(r["balance1_low"].ToString()),
                    Balance1High = Convert.ToInt32(r["balance1_high"].ToString())
                };

                messages.Add(message);
            }

            return messages;
        }

        public List<SeasonApplication> GetSeasonApplications()
        {
            var applications = new List<SeasonApplication>();
            JArray array = (JArray)rss["agri"]["nmp"]["season-applications"]["season-application"];
            foreach (var r in array)
            {
                var application = new SeasonApplication()
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString(),
                    Season = r["season"].ToString(),
                    ApplicationMethod = r["application_method"].ToString(),
                    DM_lt1 = Convert.ToDecimal(r["dm_lt1"].ToString()),
                    DM_1_5 = Convert.ToDecimal(r["dm_1_5"].ToString()),
                    DM_5_10 = Convert.ToDecimal(r["dm_5_10"].ToString()),
                    DM_gt10 = Convert.ToDecimal(r["dm_gt10"].ToString()),
                    PoultrySolid = r["poultry_solid"].ToString(),
                    Compost = r["season"].ToString(),
                    SortNum = Convert.ToInt32(r["sortNum"].ToString()),
                    ManureType = r["manure_type"].ToString()
                };
                applications.Add(application);
            }

            return applications;
        }

        public List<Yield> GetYields()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["yields"]["yield"];
            var yields = new List<Yield>();

            foreach (var r in array)
            {
                var yield = new Yield
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    YieldDesc = r["yielddesc"].ToString()
                };
                yields.Add(yield);
            }

            return yields;
        }

        //public List<RptCompletedManureRequiredStdUnit> GetRptCompletedManureRequiredStdUnits()
        //{
        //    var array = (JArray) rss["agri"]["nmp"]["RptCompletedManureRequired_StdUnit"];
        //    var rptCompletedManureRequiredStdUnits = new List<RptCompletedManureRequiredStdUnit>();

        //    foreach (var r in array)
        //    {
        //        var rptCompletedManureRequiredStdUnit = new RptCompletedManureRequiredStdUnit()
        //        {
        //            SolidUnitId = Convert.ToInt32(r["solid_unit_id"].ToString()),
        //            LiquidUnitId = Convert.ToInt32(r["liquid_unit_id"].ToString())
        //        };

        //        rptCompletedManureRequiredStdUnits.Add(rptCompletedManureRequiredStdUnit);
        //    }

        //    return rptCompletedManureRequiredStdUnits;
        //}

    }
    }
