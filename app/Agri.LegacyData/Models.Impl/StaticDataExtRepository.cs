using System;
using System.Collections.Generic;
using System.Linq;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Configuration;
using Newtonsoft.Json.Linq;

namespace Agri.LegacyData.Models.Impl
{
    public class StaticDataExtRepository : StaticDataRepository, IAgriConfigurationRepository
    {
        public List<AmmoniaRetention> GetAmmoniaRetentions()
        {
            JArray array = (JArray) rss["agri"]["nmp"]["ammoniaretentions"]["ammoniaretention"];
            var ammoniaRetentions = new List<AmmoniaRetention>();

            foreach (var r in array)
            {
                var ammoniaRetention = new AmmoniaRetention
                {
                    SeasonApplicationId = Convert.ToInt32(r["seasonapplicatonid"].ToString()),
                    DryMatter = Convert.ToInt32(r["dm"].ToString()),
                    Value = r["value"].ToString() == "null"
                        ? (decimal?) null
                        : Convert.ToDecimal(r["value"].ToString())
                };
                ammoniaRetentions.Add(ammoniaRetention);
            }

            return ammoniaRetentions;
        }

        public List<CropYield> GetCropYields()
        {
            var array = (JArray) rss["agri"]["nmp"]["cropyields"]["cropyield"];
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
            JArray array = (JArray) rss["agri"]["nmp"]["locations"]["location"];
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
            JArray array = (JArray) rss["agri"]["nmp"]["crop_stk_regioncds"]["crop_stk_regioncd"];
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
            JArray array = (JArray) rss["agri"]["nmp"]["crop_stp_regioncds"]["crop_stp_regioncd"];
            var cds = new List<CropSoilTestPhosphorousRegion>();

            foreach (var r in array)
            {
                var cd = new CropSoilTestPhosphorousRegion()
                {
                    CropId = Convert.ToInt32(r["cropid"].ToString()),
                    SoilTestPhosphorousRegionCode = Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString()),
                    PhosphorousCropGroupRegionCode =
                        r["phosphorous_crop_group_region_cd"].ToString() == "null"
                            ? (int?) null
                            : Convert.ToInt32(r["phosphorous_crop_group_region_cd"].ToString())
                };
                cds.Add(cd);
            }

            return cds;
        }

        public List<UserPrompt> GetUserPrompts()
        {
            var array = (JArray) rss["agri"]["nmp"]["userprompts"]["userprompt"];
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
            var array = (JArray) rss["agri"]["nmp"]["externallinks"]["externallink"];
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
            var array = (JArray) rss["agri"]["nmp"]["soiltestranges"]["phosphorous"];
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
            var array = (JArray) rss["agri"]["nmp"]["soiltestranges"]["potassium"];
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
            var array = (JArray) rss["agri"]["nmp"]["messages"]["message"];
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
            JArray array = (JArray) rss["agri"]["nmp"]["season-applications"]["season-application"];
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
            JArray array = (JArray) rss["agri"]["nmp"]["yields"]["yield"];
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
            JArray array = (JArray) rss["agri"]["nmp"]["n_recommcds"]["n_recommcd"];
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
                SolidUnitId = Convert.ToInt32(
                    rss["agri"]["nmp"]["RptCompletedFertilizerRequired_StdUnit"]["solid_unit_id"]
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
                CoastalFromDateMonth =
                    rss["agri"]["nmp"]["coastalBCSampleDtForNitrateCredit"]["fromDateMonth"].ToString(),
                CoastalToDateMonth = rss["agri"]["nmp"]["coastalBCSampleDtForNitrateCredit"]["toDateMonth"].ToString(),
                InteriorFromDateMonth =
                    rss["agri"]["nmp"]["interiorBCSampleDtForNitrateCredit"]["fromDateMonth"].ToString(),
                InteriorToDateMonth = rss["agri"]["nmp"]["interiorBCSampleDtForNitrateCredit"]["toDateMonth"].ToString()
            };
            return bcSampleDateForNitrateCredit;
        }




        public List<SoilTestPotassiumKelownaRange> GetSoilTestPotassiumKelownaRanges()
        {
            JArray array = (JArray) rss["agri"]["nmp"]["stk_kelowna_ranges"]["stk_kelowna_range"];
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
            JArray array = (JArray) rss["agri"]["nmp"]["stk_recommends"]["stk_recommend"];

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
            JArray fertTypes = (JArray) rss["agri"]["nmp"]["harvestunits"]["harvestunit"];

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
            var array = (JArray) rss["agri"]["nmp"]["liquidfertilizerdensitys"]["liquidfertilizerdensity"];

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
            var array = (JArray) rss["agri"]["nmp"]["nmineralizations"]["nmineralization"];
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
            JArray array = (JArray) rss["agri"]["nmp"]["dms"]["dm"];
            var dms = new List<DryMatter>();

            foreach (var r in array)
            {
                var dm = new DryMatter()
                {
                    Id = Convert.ToInt32(r["ID"].ToString()),
                    Name = r["name"].ToString()
                };
                dms.Add(dm);
            }

            ;

            return dms;
        }

        public List<MainMenu> GetMainMenus()
        {
            var mainMenus = new List<MainMenu>();

            JArray array = (JArray) rss["agri"]["nmp"]["mainMenus"]["mainMenu"];
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

            JArray array = (JArray) rss["agri"]["nmp"]["animalSubTypes"]["animalSubType"];
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
                    {Id = r.Id, Value = r.Name};
                mainMenuOptions.Add(li);
            }

            return mainMenuOptions;
        }

        public List<SubMenu> GetSubMenus()
        {
            var subMenus = new List<SubMenu>();

            JArray array = (JArray) rss["agri"]["nmp"]["subMenus"]["subMenu"];
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
                    {Id = r.Id, Value = r.Name};
                subMenuoptions.Add(li);
            }

            return subMenuoptions;
        }


        public List<NitrateCreditSampleDate> GetNitrateCreditSampleDates()
        {
            var samples = new List<NitrateCreditSampleDate>();

            samples.Add(new NitrateCreditSampleDate
            {
                Location = "CoastalBC",
                FromDateMonth = (string) rss["agri"]["nmp"]["coastalBCSampleDtForNitrateCredit"][
                    "fromDateMonth"],
                ToDateMonth = (string) rss["agri"]["nmp"]["coastalBCSampleDtForNitrateCredit"]["toDateMonth"]
            });

            samples.Add(new NitrateCreditSampleDate
            {
                Location = "InteriorBC",
                FromDateMonth = (string) rss["agri"]["nmp"]["interiorBCSampleDtForNitrateCredit"][
                    "fromDateMonth"],
                ToDateMonth = (string) rss["agri"]["nmp"]["interiorBCSampleDtForNitrateCredit"]["toDateMonth"]
            });

            return samples;
        }

        private List<SoilTestRange> GetSoilTestRanges(string chemical)
        {
            var ranges = new List<SoilTestRange>();

            JArray array = (JArray) rss["agri"]["nmp"]["soiltestranges"][chemical];

            foreach (var r in array)
            {
                SoilTestRange range = new SoilTestRange();
                range.UpperLimit = Convert.ToInt32(r["upperlimit"].ToString());
                range.Rating = r["rating"].ToString();
                ranges.Add(range);
            }

            return ranges;
        }

        public string GetPotassiumSoilTestRating(decimal value)
        {
            return SoilTestRating("potassium", value);
        }

        public string GetPhosphorusSoilTestRating(decimal value)
        {
            return SoilTestRating("phosphorous", value);
        }

        public List<PotassiumSoilTestRange> GetPotassiumSoilTestRanges()
        {
            var ranges = GetSoilTestRanges("potassium");

            return ranges
                .Select(r => new PotassiumSoilTestRange {Rating = r.Rating, UpperLimit = r.UpperLimit})
                .ToList();
        }

        public List<PhosphorusSoilTestRange> GetPhosphorusSoilTestRanges()
        {
            var ranges = GetSoilTestRanges("phosphorous");

            return ranges
                .Select(r => new PhosphorusSoilTestRange {Rating = r.Rating, UpperLimit = r.UpperLimit})
                .ToList();
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

        public List<SolidMaterialsConversionFactor> GetSolidMaterialsConversionFactors()
        {
            var conversionFactors = new List<SolidMaterialsConversionFactor>();

            var array =
                (JArray) rss["agri"]["nmp"]["SolidMaterialsConversionFactors"]["SolidMaterialsConversionFactor"];
            foreach (var record in array)
            {
                var conversionFactor = new SolidMaterialsConversionFactor()
                {
                    Id = Convert.ToInt32(record["Id"].ToString()),
                    InputUnit = (AnnualAmountUnits) Convert.ToInt32(record["InputUnit"].ToString()),
                    InputUnitName = record["InputUnitName"].ToString(),
                    CubicYardsOutput = record["CubicYardsOutput"].ToString(),
                    CubicMetersOutput = record["CubicMetersOutput"].ToString(),
                    MetricTonsOutput = record["MetricTonsOutput"].ToString()
                };
                conversionFactors.Add(conversionFactor);
            }

            return conversionFactors;
        }

        public List<LiquidMaterialsConversionFactor> GetLiquidMaterialsConversionFactors()
        {
            var conversionFactors = new List<LiquidMaterialsConversionFactor>();

            var array =
                (JArray) rss["agri"]["nmp"]["LiquidMaterialsConversionFactors"]["LiquidMaterialsConversionFactor"];
            foreach (var record in array)
            {
                var conversionFactor = new LiquidMaterialsConversionFactor()
                {
                    Id = Convert.ToInt32(record["Id"].ToString()),
                    InputUnit = (AnnualAmountUnits) Convert.ToInt32(record["InputUnit"].ToString()),
                    InputUnitName = record["InputUnitName"].ToString(),
                    USGallonsOutput = Convert.ToDecimal(record["USGallonsOutput"].ToString())
                };
                conversionFactors.Add(conversionFactor);
            }

            return conversionFactors;
        }

        public List<SolidMaterialApplicationTonPerAcreRateConversion>
            GetSolidMaterialApplicationTonPerAcreRateConversions()
        {
            var conversionFactors = new List<SolidMaterialApplicationTonPerAcreRateConversion>();
            var array = (JArray) rss["agri"]["nmp"]["SolidMaterialApplicationTonPerAcreRateConversions"][
                "SolidMaterialApplicationTonPerAcreRateConversion"];

            foreach (var record in array)
            {
                var conversionFactor = new SolidMaterialApplicationTonPerAcreRateConversion
                {
                    Id = Convert.ToInt32(record["Id"].ToString()),
                    ApplicationRateUnit =
                        (ApplicationRateUnits) Convert.ToInt32(record["ApplicationRateUnit"].ToString()),
                    ApplicationRateUnitName = record["ApplicationRateUnitName"].ToString(),
                    TonsPerAcreConversion = record["TonsPerAcreConversion"].ToString(),
                };
                conversionFactors.Add(conversionFactor);
            }

            return conversionFactors;
        }

        public List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion>
            GetLiquidMaterialApplicationUSGallonsPerAcreRateConversion()
        {
            var conversionFactors = new List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion>();
            var array = (JArray) rss["agri"]["nmp"]["LiquidMaterialApplicationUSGallonsPerAcreRateConversions"][
                "LiquidMaterialApplicationUSGallonsPerAcreRateConversion"];

            foreach (var record in array)
            {
                var conversionFactor = new LiquidMaterialApplicationUSGallonsPerAcreRateConversion
                {
                    Id = Convert.ToInt32(record["Id"].ToString()),
                    ApplicationRateUnit =
                        (ApplicationRateUnits) Convert.ToInt32(record["ApplicationRateUnit"].ToString()),
                    ApplicationRateUnitName = record["ApplicationRateUnitName"].ToString(),
                    USGallonsPerAcreConversion = Convert.ToDecimal(record["USGallonsPerAcreConversion"].ToString()),
                };
                conversionFactors.Add(conversionFactor);
            }

            return conversionFactors;
        }

        public List<Breed> GetBreeds()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["breeds"]["breed"];
            var breeds = new List<Breed>();

            foreach (var r in array)
            {
                var breed = new Breed
                {
                    Id = Convert.ToInt32(r["Id"].ToString()),
                    BreedName = r["Breed"].ToString(),
                    BreedManureFactor = Convert.ToDecimal(r["BreedManureFactor"].ToString())
                };
                breeds.Add(breed);
            }

            return breeds;
        }
        public List<SelectListItem> GetBreedsDll(int animalType)
        {
            var animalBreeds = GetBreeds();

            animalBreeds = animalBreeds.OrderBy(n => n.Id).ToList();

            List<SelectListItem> breedOptions = new List<SelectListItem>();

            foreach (var r in animalBreeds)
            {
                if (r.AnimalId == animalType)
                {
                    var li = new SelectListItem()
                        { Id = r.Id, Value = r.BreedName };
                    breedOptions.Add(li);
                }
            }

            return breedOptions;
        }
        public decimal GetBreedManureFactorByBreedId(int breedId)
        {
            Breed breed = new Breed();

            JArray breeds = (JArray)rss["agri"]["nmp"]["breeds"]["breed"];

            foreach (var r in breeds)
            {
                if (r["Id"].ToString() == breedId.ToString())
                {
                    breed.BreedManureFactor = Convert.ToDecimal(r["BreedManureFactor"].ToString());
                }
            }

            return breed.BreedManureFactor;
        }

        public List<SelectListItem> GetBreed(int breedId)
        {
            var animalBreeds = GetBreeds();

            animalBreeds = animalBreeds.OrderBy(n => n.Id).ToList();

            List<SelectListItem> breedOptions = new List<SelectListItem>();

            foreach (var r in animalBreeds)
            {
                if (r.Id == breedId)
                {
                    var li = new SelectListItem()
                        { Id = r.Id, Value = r.BreedName };
                    breedOptions.Add(li);
                }
            }

            return breedOptions;
        }


        public LiquidSolidSeparationDefault GetLiquidSolidSeparationDefaults()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetSubRegionsDll(int? regionId)
        {
            var subRegOptions = new List<SelectListItem>();
            return subRegOptions;
        }
        public SubRegion GetSubRegion(int? subRegionId)
        {
            var subRegion = new SubRegion();
            return subRegion;
        }

        public List<SubRegion> GetSubRegions()
        {
            var listSubRegion = new List<SubRegion>();
            return listSubRegion;
        }

        public List<Crop> GetCropsByManureApplicationHistory(int manureAppHistory)
        {
            throw new NotImplementedException();
        }

        public PreviousManureApplicationYear GetPrevManureApplicationInPrevYearsByManureAppHistory(int manureAppHistory)
        {
            throw new NotImplementedException();
        }

        public MainMenu GetMainMenu(CoreSiteActions action)
        {
            throw new NotImplementedException();
        }
    }
}
