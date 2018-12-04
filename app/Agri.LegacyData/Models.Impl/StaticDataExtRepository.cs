using System;
using System.Collections.Generic;
using System.Linq;
using Agri.Interfaces;
using Agri.Models.Configuration;
using Newtonsoft.Json.Linq;

namespace Agri.LegacyData.Models.Impl
{
    public class StaticDataExtRepository : StaticDataRepository, IAgriConfigurationRepository
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

        public List<SoilTestPhosphorusRange> GetSoilTestPhosphorusRanges()
        {
            var array = (JArray)rss["agri"]["nmp"]["soiltestranges"]["phosphorous"];
            var soilTestPhosphorusRanges = new List<SoilTestPhosphorusRange>();

            foreach (var r in array)
            {
                var soilTestPhosphorusRange = new SoilTestPhosphorusRange()
                {
                    // Id = Convert.ToInt32(r["id"].ToString()),
                    UpperLimit = Convert.ToInt32(r["upperlimit"].ToString()),
                    Rating = Convert.ToString(r["rating"].ToString())
                };

                soilTestPhosphorusRanges.Add(soilTestPhosphorusRange);
            }

            return soilTestPhosphorusRanges;
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
                    DryMatterLessThan1Percent = Convert.ToDecimal(r["dm_lt1"].ToString()),
                    DryMatter1To5Percent = Convert.ToDecimal(r["dm_1_5"].ToString()),
                    DryMatter5To10Percent = Convert.ToDecimal(r["dm_5_10"].ToString()),
                    DryMatterGreaterThan10Percent = Convert.ToDecimal(r["dm_gt10"].ToString()),
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

        public List<NitrogenRecommendation> GetNitrogenRecommendations()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["n_recommcds"]["n_recommcd"];
            var nitrogenRecommendations = new List<NitrogenRecommendation>();

            foreach (var r in array)
            {
                var nitrogenRecommendation = new NitrogenRecommendation
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    RecommendationDesc = r["recommdesc"].ToString()
                };
                nitrogenRecommendations.Add(nitrogenRecommendation);
            }

            return nitrogenRecommendations;
        }

        public RptCompletedManureRequiredStdUnit GetRptCompletedManureRequiredStdUnit()
        {
            var rptCompletedManureRequiredStdUnit = new RptCompletedManureRequiredStdUnit()
            {
                SolidUnitId = Convert.ToInt32(rss["agri"]["nmp"]["RptCompletedManureRequired_StdUnit"]["solid_unit_id"]
                    .ToString()),
                LiquidUnitId =
                    Convert.ToInt32(rss["agri"]["nmp"]["RptCompletedManureRequired_StdUnit"]["liquid_unit_id"]
                        .ToString())
            };
            return rptCompletedManureRequiredStdUnit;
        }

        public RptCompletedFertilizerRequiredStdUnit GetRptCompletedFertilizerRequiredStdUnit()
        {
            var rptCompletedFertilizerRequiredStdUnit = new RptCompletedFertilizerRequiredStdUnit()
            {
                SolidUnitId = Convert.ToInt32(rss["agri"]["nmp"]["RptCompletedFertilizerRequired_StdUnit"]["solid_unit_id"]
                    .ToString()),
                LiquidUnitId =
                    Convert.ToInt32(rss["agri"]["nmp"]["RptCompletedFertilizerRequired_StdUnit"]["liquid_unit_id"]
                        .ToString())
            };
            return rptCompletedFertilizerRequiredStdUnit;
        }

        public BCSampleDateForNitrateCredit GetBCSampleDateForNitrateCredit()
        {
            var bcSampleDateForNitrateCredit = new BCSampleDateForNitrateCredit()
            {
                CoastalFromDateMonth = rss["agri"]["nmp"]["coastalBCSampleDtForNitrateCredit"]["fromDateMonth"].ToString(),
                CoastalToDateMonth =  rss["agri"]["nmp"]["coastalBCSampleDtForNitrateCredit"]["toDateMonth"].ToString(),
                InteriorFromDateMonth = rss["agri"]["nmp"]["interiorBCSampleDtForNitrateCredit"]["fromDateMonth"].ToString(),
                InteriorToDateMonth = rss["agri"]["nmp"]["interiorBCSampleDtForNitrateCredit"]["toDateMonth"].ToString()
            };
            return bcSampleDateForNitrateCredit;
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

        public List<MainMenu> GetMainMenus()
        {
            var mainMenus = new List<MainMenu>();

            JArray array = (JArray)rss["agri"]["nmp"]["mainMenus"]["mainMenu"];
            foreach (var record in array)
            {
                var mainMenu = new MainMenu
                {
                    Id = Convert.ToInt32(record["id"].ToString()),
                    Name = record["name"].ToString(),
                    Controller = record["controller"].ToString(),
                    Action = record["action"].ToString()
                };
                mainMenus.Add(mainMenu);
            }

            return mainMenus;
        }

        public List<SubMenu> GetSubMenus(int mainMenuId)
        {
            var subMenus = new List<SubMenu>();

            JArray array = (JArray)rss["agri"]["nmp"]["animalSubTypes"]["animalSubType"];
            foreach (var record in array)
            {
                if (Convert.ToUInt32(record["mainMenuId"].ToString()) == mainMenuId)
                {
                    var subMenu = new SubMenu
                    {
                        Id = Convert.ToInt32(record["id"]),
                        Name = record["name"].ToString(),
                        MainMenuId = Convert.ToInt32(record["mainMenuId"])
                    };
                    subMenus.Add(subMenu);
                }
            }

            return subMenus;
        }

        public List<SelectListItem> GetMainMenusDll()
        {
            var mainMenus = GetMainMenus();

            List<SelectListItem> mainMenuOptions = new List<SelectListItem>();

            foreach (var r in mainMenus)
            {
                var li = new SelectListItem()
                    { Id = r.Id, Value = r.Name };
                mainMenuOptions.Add(li);
            }

            return mainMenuOptions;
        }

        public List<SubMenu> GetSubMenus()
        {
            var subMenus = new List<SubMenu>();

            JArray array = (JArray)rss["agri"]["nmp"]["subMenus"]["subMenu"];
            foreach (var record in array)
            {
                var subMenu = new SubMenu()
                {
                    Id = Convert.ToInt32(record["id"].ToString()),
                    Name = record["name"].ToString(),
                    Controller = record["controller"].ToString(),
                    Action = record["action"].ToString(),
                    MainMenuId = Convert.ToInt32(record["mainMenuId"].ToString())
                };
                subMenus.Add(subMenu);
            }

            return subMenus;
        }

        public List<SelectListItem> GetSubmenusDll()
        {
            var subMenus = GetSubMenus();

            subMenus = subMenus.OrderBy(n => n.Name).ToList();

            List<SelectListItem> subMenuoptions = new List<SelectListItem>();

            foreach (var r in subMenus)
            {
                //if (r.MainMenuId == mainMenu)
                //{
                //    var li = new SelectListItem()
                //        { Id = r.Id, Value = r.Name };
                //    subMenuoptions.Add(li);
                //}
                var li = new SelectListItem()
                { Id = r.Id, Value = r.Name };
                subMenuoptions.Add(li);
            }

            return subMenuoptions;
        }

        public ManureImportedDefault GetManureImportedDefault()
        {
            var defaultMoistureRaw = rss["agri"]["nmp"]["ManureImportedDefaults"]["defaultSolidMoisture"];

            var importedDefault = new ManureImportedDefault
            {
                DefaultSolidMoisture = Convert.ToInt32(defaultMoistureRaw)
            };

            return importedDefault;
        }
    }
    }
