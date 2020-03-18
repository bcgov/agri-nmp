using Agri.Interfaces;
using Agri.LegacyData.Models.Impl;
using Agri.Models.Configuration;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Agri.Data
{
    public class AgriSeeder
    {
        private AgriConfigurationContext _context;
        private readonly IAgriConfigurationRepository _sd;
        private readonly IMapper _mapper;

        public AgriSeeder(AgriConfigurationContext context, IAgriConfigurationRepository sd, IMapper mapper)
        {
            _context = context;
            _sd = sd;
            _mapper = mapper;
        }

        public void Seed()
        {
            //If the database is not present or if migrations are required
            //create the database and/or run the migrations
            //_context.Database.Migrate();

            var staticExtRepo = new StaticDataExtRepository();
            var currentVersion = _sd.GetCurrentStaticDataVersion();
            if (currentVersion.Id == 1 && string.IsNullOrEmpty(currentVersion.CreatedBy))
            {
                currentVersion.Comments = "Initial version migrated from Legacy StaticData.json file";
                currentVersion.CreatedBy = "Agri.Data.AgriSeeder.cs";
            }

            //AmoniaRetention
            if (!_context.AmmoniaRetentions.Any())
            {
                var ammoniaRetentions = staticExtRepo.GetAmmoniaRetentions();
                ammoniaRetentions.Select(r => { r.SetVersion(currentVersion); return r; }).ToList();
                _context.AmmoniaRetentions.AddRange(ammoniaRetentions);
            }

            //Animal
            //AnimalSubType
            if (!_context.Animals.Any())
            {
                var animals = staticExtRepo.GetAnimals();
                var animalSubtypes = staticExtRepo.GetAnimalSubTypes();
                foreach (var animal in animals)
                {
                    animal.SetVersion(currentVersion);
                    var subtypes = animalSubtypes.Where(s => s.AnimalId == animal.Id).ToList();
                    if (subtypes.Any())
                    {
                        subtypes.Select(st =>
                        {
                            st.SetVersion(currentVersion);
                            return st;
                        }).ToList();
                        animal.AnimalSubTypes.AddRange(subtypes);
                    }
                }
                _context.Animals.AddRange(animals);
            }

            //Browser
            if (!_context.Browsers.Any())
            {
                var browsers = staticExtRepo.GetAllowableBrowsers();
                _context.Browsers.AddRange(browsers);
            }

            //ConversionFactor
            if (!_context.ConversionFactors.Any())
            {
                var cFactor = staticExtRepo.GetConversionFactor();
                cFactor.SoilTestPPMToPoundPerAcre = staticExtRepo.GetSoilTestNitratePPMToPoundPerAcreConversionFactor();
                cFactor.SetVersion(currentVersion);
                _context.ConversionFactors.Add(cFactor);
            }

            //Default Soil Test
            if (!_context.DefaultSoilTests.Any())
            {
                var test = staticExtRepo.GetDefaultSoilTest();
                test.DefaultSoilTestMethodId = staticExtRepo.GetDefaultSoilTestMethod();
                test.SetVersion(currentVersion);
                _context.DefaultSoilTests.Add(test);
            }

            //Soil Test Ranges
            if (!_context.PhosphorusSoilTestRanges.Any())
            {
                var ranges = staticExtRepo.GetPhosphorusSoilTestRanges();
                ranges.Select(r => { r.SetVersion(currentVersion); return r; }).ToList();
                _context.PhosphorusSoilTestRanges.AddRange(ranges);
            }

            if (!_context.PotassiumSoilTestRanges.Any())
            {
                var ranges = staticExtRepo.GetPotassiumSoilTestRanges();
                ranges.Select(r => { r.SetVersion(currentVersion); return r; }).ToList();
                _context.PotassiumSoilTestRanges.AddRange(ranges);
            }

            //Location
            if (!_context.Locations.Any())
            {
                var locations = staticExtRepo.GetLocations();
                _context.Locations.AddRange(locations);
            }

            //Regions
            if (!_context.Regions.Any())
            {
                var regions = staticExtRepo.GetRegions();
                regions.Select(r => { r.SetVersion(currentVersion); return r; }).ToList();
                _context.Regions.AddRange(regions);
            }

            //CropTypes
            if (!_context.CropTypes.Any())
            {
                var types = staticExtRepo.GetCropTypes();
                types.Select(t => { t.SetVersion(currentVersion); return t; }).ToList();
                _context.CropTypes.AddRange(types);
            }

            //PrevManureApplicationYear
            if (!_context.PrevManureApplicationYears.Any())
            {
                var years = staticExtRepo.GetPrevManureApplicationInPrevYears();
                years.Select(y => { y.SetVersion(currentVersion); return y; }).ToList();
                _context.PrevManureApplicationYears.AddRange(years);
            }

            //PrevYearManureApplicationlDefaultNitrogen
            if (!_context.PrevYearManureApplicationNitrogenDefaults.Any())
            {
                var nitrogenDefaults = staticExtRepo.GetPrevYearManureNitrogenCreditDefaults();
                nitrogenDefaults.Select(nd =>
                {
                    nd.PreviousYearManureAplicationFrequency = nd.FieldManureApplicationHistory.ToString();
                    nd.SetVersion(currentVersion);
                    return nd;
                }).ToList();
                _context.PrevYearManureApplicationNitrogenDefaults.AddRange(nitrogenDefaults);
            }

            //Crops
            //CropStkRegion
            //CropStpRegion
            //CropYield
            //PrevCropType
            if (!_context.Crops.Any())
            {
                var crops = staticExtRepo.GetCrops();
                var stksRegions = staticExtRepo.GetCropSoilTestPotassiumRegions();
                var stpsRegions = staticExtRepo.GetCropSoilTestPhosphorousRegions();
                var cropYields = staticExtRepo.GetCropYields();
                var prevCropTypes = staticExtRepo.GetPreviousCropTypes();

                foreach (var crop in crops)
                {
                    crop.SetVersion(currentVersion);
                    if (stksRegions.Any(s => s.CropId == crop.Id))
                    {
                        crop.CropSoilTestPotassiumRegions.AddRange(stksRegions.Where(s => s.CropId == crop.Id));
                    }

                    if (stpsRegions.Any(s => s.CropId == crop.Id))
                    {
                        crop.CropSoilTestPhosphorousRegions.AddRange(stpsRegions.Where(s => s.CropId == crop.Id));
                    }

                    if (cropYields.Any(s => s.CropId == crop.Id))
                    {
                        crop.CropYields.AddRange(cropYields.Where(s => s.CropId == crop.Id));
                    }

                    if (prevCropTypes.Any(c => c.PreviousCropCode == crop.PreviousCropCode))
                    {
                        crop.PreviousCropTypes.AddRange(prevCropTypes.Where(c => c.PreviousCropCode == crop.PreviousCropCode));
                    }
                }
                crops.Select(c => { c.SetVersion(currentVersion); return c; }).ToList();
                currentVersion.Crops.AddRange(crops);
                _context.Crops.AddRange(crops);
            }

            //DensityType
            if (!_context.DensityUnits.Any())
            {
                var units = staticExtRepo.GetDensityUnits();
                units.Select(u => { u.SetVersion(currentVersion); return u; }).ToList();
                _context.DensityUnits.AddRange(units);
            }

            //Fertilizer
            if (!_context.Fertilizers.Any())
            {
                var fertilizers = staticExtRepo.GetFertilizers();
                fertilizers.Select(f => { f.SetVersion(currentVersion); return f; }).ToList();
                _context.Fertilizers.AddRange(fertilizers);
            }

            //FertilizerType
            if (!_context.FertilizerTypes.Any())
            {
                var types = staticExtRepo.GetFertilizerTypes();
                types.Select(f => { f.SetVersion(currentVersion); return f; }).ToList();
                _context.FertilizerTypes.AddRange(types);
            }

            //FertilizerUnit
            if (!_context.FertilizerUnits.Any())
            {
                var units = staticExtRepo.GetFertilizerUnits();
                units.Select(f => { f.SetVersion(currentVersion); return f; }).ToList();
                _context.FertilizerUnits.AddRange(units);
            }

            //LiquidFertilizerDensity
            if (!_context.LiquidFertilizerDensities.Any())
            {
                var densities = staticExtRepo.GetLiquidFertilizerDensities();
                densities.Select(f => { f.SetVersion(currentVersion); return f; }).ToList();
                _context.LiquidFertilizerDensities.AddRange(densities);
            }

            //DryMatter
            if (!_context.DryMatters.Any())
            {
                var dms = staticExtRepo.GetDryMatters();
                dms.Select(d => { d.SetVersion(currentVersion); return d; }).ToList();
                _context.DryMatters.AddRange(dms);
            }

            //NMineralization
            if (!_context.NitrogenMineralizations.Any())
            {
                var minerals = staticExtRepo.GetNitrogeMineralizations();
                minerals.Select(m => { m.SetVersion(currentVersion); return m; }).ToList();
                _context.NitrogenMineralizations.AddRange(minerals);
            }

            //Manure
            if (!_context.Manures.Any())
            {
                var manures = staticExtRepo.GetManures();
                manures.Select(m => { m.SetVersion(currentVersion); return m; }).ToList();
                _context.Manures.AddRange(manures);
            }

            ////ManureMaterialTypes
            //if (!_context.ManureMaterialTypes.Any())
            //{
            //    var types = staticDataRepo.GetManureMaterialTypes();
            //    _context.ManureMaterialTypes.AddRange(types);
            //}

            //STKKelownaRange
            //STKRecommendations
            if (!_context.SoilTestPotassiumKelownaRanges.Any())
            {
                var ranges = staticExtRepo.GetSoilTestPotassiumKelownaRanges();
                ranges.Select(r => { r.SetVersion(currentVersion); return r; }).ToList();
                var stks = staticExtRepo.GetSoilTestPotassiumRecommendations();
                stks.Select(s => { s.SetVersion(currentVersion); return s; }).ToList();

                foreach (var stkKelownaRange in ranges)
                {
                    if (stks.Any(s => s.SoilTestPotassiumKelownaRangeId == stkKelownaRange.Id))
                    {
                        stkKelownaRange.SoilTestPotassiumRecommendations.AddRange(stks.Where(s =>
                            s.SoilTestPotassiumKelownaRangeId == stkKelownaRange.Id));
                    }
                }

                _context.SoilTestPotassiumKelownaRanges.AddRange(ranges);
            }

            //STPKelownaRange
            //STPRecommendations
            if (!_context.SoilTestPhosphorousKelownaRanges.Any())
            {
                var ranges = staticExtRepo.GetSoilTestPhosphorousKelownaRanges();
                ranges.Select(r => { r.SetVersion(currentVersion); return r; }).ToList();
                var stps = staticExtRepo.GetSoilTestPhosphorousRecommendations();
                stps.Select(s => { s.SetVersion(currentVersion); return s; }).ToList();

                foreach (var stpKelownaRange in ranges)
                {
                    if (stps.Any(s => s.SoilTestPhosphorousKelownaRangeId == stpKelownaRange.Id))
                    {
                        stpKelownaRange.SoilTestPhosphorousRecommendations.AddRange(stps.Where(s =>
                            s.SoilTestPhosphorousKelownaRangeId == stpKelownaRange.Id));
                    }
                }

                _context.SoilTestPhosphorousKelownaRanges.AddRange(ranges);
            }

            if (!_context.FertilizerMethods.Any())
            {
                var fertilizerMethods = staticExtRepo.GetFertilizerMethods();
                fertilizerMethods.Select(f => { f.SetVersion(currentVersion); return f; }).ToList();
                _context.FertilizerMethods.AddRange(fertilizerMethods);
            }

            if (!_context.UserPrompts.Any())
            {
                var userPrompts = staticExtRepo.GetUserPrompts();
                _context.UserPrompts.AddRange(userPrompts);
            }

            if (!_context.ExternalLinks.Any())
            {
                var externalLinks = staticExtRepo.GetExternalLinks();
                _context.ExternalLinks.AddRange(externalLinks);
            }

            if (!_context.Units.Any())
            {
                var units = staticExtRepo.GetUnits();
                units.Select(u => { u.SetVersion(currentVersion); return u; }).ToList();
                _context.Units.AddRange(units);
            }

            if (!_context.SoilTestMethods.Any())
            {
                var soilTestMethods = staticExtRepo.GetSoilTestMethods();
                soilTestMethods.Select(s => { s.SetVersion(currentVersion); return s; }).ToList();
                _context.SoilTestMethods.AddRange(soilTestMethods);
            }

            if (!_context.SoilTestPhosphorusRanges.Any())
            {
                var getSoilTestPhosphorusRanges = staticExtRepo.GetSoilTestPhosphorusRanges();
                getSoilTestPhosphorusRanges.Select(s => { s.SetVersion(currentVersion); return s; }).ToList();
                _context.SoilTestPhosphorusRanges.AddRange(getSoilTestPhosphorusRanges);
            }

            if (!_context.SoilTestPotassiumRanges.Any())
            {
                var getSoilTestPotassiumRanges = staticExtRepo.GetSoilTestPotassiumRanges();
                getSoilTestPotassiumRanges.Select(s => { s.SetVersion(currentVersion); return s; }).ToList();
                _context.SoilTestPotassiumRanges.AddRange(getSoilTestPotassiumRanges);
            }

            if (!_context.Messages.Any())
            {
                var getMessages = staticExtRepo.GetMessages();
                getMessages.Select(m => { m.SetVersion(currentVersion); return m; }).ToList();
                _context.Messages.AddRange(getMessages);
            }

            if (!_context.SeasonApplications.Any())
            {
                var getSeasonApplications = staticExtRepo.GetSeasonApplications();
                getSeasonApplications.Select(s => { s.SetVersion(currentVersion); return s; }).ToList();
                _context.SeasonApplications.AddRange(getSeasonApplications);
            }

            if (!_context.Yields.Any())
            {
                var getYields = staticExtRepo.GetYields();
                getYields.Select(s => { s.SetVersion(currentVersion); return s; }).ToList();
                _context.Yields.AddRange(getYields);
            }

            if (!_context.NitrogenRecommendations.Any())
            {
                var getNitrogenRecommendations = staticExtRepo.GetNitrogenRecommendations();
                getNitrogenRecommendations.Select(n => { n.SetVersion(currentVersion); return n; }).ToList();
                _context.NitrogenRecommendations.AddRange(getNitrogenRecommendations);
            }

            if (!_context.RptCompletedManureRequiredStdUnits.Any())
            {
                var rptCompletedManureRequiredStdUnits = staticExtRepo.GetRptCompletedManureRequiredStdUnit();
                rptCompletedManureRequiredStdUnits.SetVersion(currentVersion);
                _context.RptCompletedManureRequiredStdUnits.AddRange(rptCompletedManureRequiredStdUnits);
            }

            if (!_context.RptCompletedFertilizerRequiredStdUnits.Any())
            {
                var rptCompletedFertilizerRequiredStdUnits = staticExtRepo.GetRptCompletedFertilizerRequiredStdUnit();
                rptCompletedFertilizerRequiredStdUnits.SetVersion(currentVersion);
                _context.RptCompletedFertilizerRequiredStdUnits.AddRange(rptCompletedFertilizerRequiredStdUnits);
            }

            //HarvestUnit
            if (!_context.HarvestUnits.Any())
            {
                var units = staticExtRepo.GetHarvestUnits();
                units.Select(u => { u.SetVersion(currentVersion); return u; }).ToList();
                _context.HarvestUnits.AddRange(units);
            }

            if (!_context.BCSampleDateForNitrateCredit.Any())
            {
                var bcSampleDateForNitrateCredit = staticExtRepo.GetBCSampleDateForNitrateCredit();
                bcSampleDateForNitrateCredit.SetVersion(currentVersion);
                _context.BCSampleDateForNitrateCredit.AddRange(bcSampleDateForNitrateCredit);
            }

            //Nutrient Icons
            if (!_context.NutrientIcons.Any())
            {
                var icons = staticExtRepo.GetNutrientIcons();
                _context.NutrientIcons.AddRange(icons);
            }

            //if (!_context.MiniApps.Any())
            //{
            //    var miniapps = SeedDataLoader.GetSeedJsonData<List<MiniApp>>(Constants.SeedDataFiles.MiniApps);
            //    _context.MiniApps.AddRange(miniapps);
            //    _context.SaveChanges();
            //}

            //if (!_context.MiniAppLabels.Any())
            //{
            //    var miniapplabels = SeedDataLoader.GetSeedJsonData<List<MiniAppLabel>>(Constants.SeedDataFiles.MiniAppLabels);
            //    _context.MiniAppLabels.AddRange(miniapplabels);
            //    _context.SaveChanges();
            //}

            if (!_context.StaticDataVersions.Any())
            {
                var version = staticExtRepo.GetStaticDataVersion();
                _context.StaticDataVersions.Add(new StaticDataVersion { Version = version });
            }

            if (!_context.NitrateCreditSampleDates.Any())
            {
                var dates = staticExtRepo.GetNitrateCreditSampleDates();
                dates.Select(d => { d.SetVersion(currentVersion); return d; }).ToList();
                _context.NitrateCreditSampleDates.AddRange(dates);
            }

            if (!_context.ManureImportedDefaults.Any())
            {
                var defaultManure = staticExtRepo.GetManureImportedDefault();
                defaultManure.SetVersion(currentVersion);
                _context.Add(defaultManure);
            }

            //MainMenu
            //SubMenu
            if (!_context.MainMenus.Any())
            {
                var mainMenus = staticExtRepo.GetMainMenus();
                var subMenus = staticExtRepo.GetSubMenus();
                foreach (var mainMenu in mainMenus)
                {
                    var subMenu = subMenus.Where(s => s.MainMenuId == mainMenu.Id).ToList();
                    if (subMenu.Any())
                    {
                        mainMenu.SubMenus.AddRange(subMenu);
                    }
                }
                _context.MainMenus.AddRange(mainMenus);
            }

            if (!_context.LiquidMaterialsConversionFactors.Any())
            {
                var conversions = staticExtRepo.GetLiquidMaterialsConversionFactors();
                conversions.Select(c => { c.SetVersion(currentVersion); return c; }).ToList();
                _context.LiquidMaterialsConversionFactors.AddRange(conversions);
            }

            if (!_context.SolidMaterialsConversionFactors.Any())
            {
                var conversions = staticExtRepo.GetSolidMaterialsConversionFactors();
                conversions.Select(c => { c.SetVersion(currentVersion); return c; }).ToList();
                _context.SolidMaterialsConversionFactors.AddRange(conversions);
            }

            if (!_context.LiquidMaterialApplicationUsGallonsPerAcreRateConversions.Any())
            {
                var conversions = staticExtRepo.GetLiquidMaterialApplicationUSGallonsPerAcreRateConversion();
                conversions.Select(c => { c.SetVersion(currentVersion); return c; }).ToList();
                _context.LiquidMaterialApplicationUsGallonsPerAcreRateConversions.AddRange(conversions);
            }

            if (!_context.SolidMaterialApplicationTonPerAcreRateConversions.Any())
            {
                var conversions = staticExtRepo.GetSolidMaterialApplicationTonPerAcreRateConversions();
                conversions.Select(c => { c.SetVersion(currentVersion); return c; }).ToList();
                _context.SolidMaterialApplicationTonPerAcreRateConversions.AddRange(conversions);
            }

            _context.StaticDataVersions.Update(currentVersion);
            _context.SaveChanges();

            AppliedMigrationsSeedData();
        }

        public void AppliedMigrationsSeedData()
        {
            //Updates
            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("1_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("1_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("2_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("2_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("3_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("3_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("4_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("4_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        var updated = _context.UserPrompts.Single(up => up.Id == newUserPrompt.Id);
                        _mapper.Map(newUserPrompt, updated);
                        _context.UserPrompts.Update(updated);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("11_Breed", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<Breed>>("11_Breed");
                var currentVersion = _sd.GetCurrentStaticDataVersion();

                foreach (var newBreed in migrationSeedData.Data)
                {
                    newBreed.SetVersion(currentVersion);
                    if (!_context.Breed.Any(up => up.Id == newBreed.Id))
                    {
                        _context.Breed.Add(newBreed);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.StaticDataVersions.Update(currentVersion);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("14_AnimalSubTypes", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<AnimalSubType>>("14_AnimalSubTypes");
                var currentVersion = _sd.GetCurrentStaticDataVersion();
                foreach (var newSubType in migrationSeedData.Data)
                {
                    if (_context.AnimalSubType.Any(up => up.Id == newSubType.Id && up.StaticDataVersionId == currentVersion.Id))
                    {
                        var updated = _context.AnimalSubType.Single(up => up.Id == newSubType.Id && up.StaticDataVersionId == currentVersion.Id);
                        _mapper.Map(newSubType, updated);
                        _context.AnimalSubType.Update(updated);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("16_LiquidSolidSeparationDefault", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<LiquidSolidSeparationDefault>>("16_LiquidSolidSeparationDefault");
                var currentVersion = _sd.GetCurrentStaticDataVersion();
                foreach (var liquidSolidSeparationDefault in migrationSeedData.Data)
                {
                    liquidSolidSeparationDefault.SetVersion(currentVersion);
                    if (!_context.LiquidSolidSeparationDefaults.Any(up => up.Id == liquidSolidSeparationDefault.Id))
                    {
                        _context.LiquidSolidSeparationDefaults.Add(liquidSolidSeparationDefault);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.StaticDataVersions.Update(currentVersion);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("17_SubRegions", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<SubRegion>>("17_SubRegions");
                var currentVersion = _context.StaticDataVersions.Single(v => v.Id == _sd.GetCurrentStaticDataVersion().Id);
                foreach (var newSubRegion in migrationSeedData.Data)
                {
                    newSubRegion.SetVersion(currentVersion);
                    newSubRegion.Region = _sd.GetRegion(newSubRegion.RegionId);
                    if (!_context.SubRegion.Any(up => up.Id == newSubRegion.Id))
                    {
                        _context.SubRegion.Add(newSubRegion);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.StaticDataVersions.Update(currentVersion);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a =>
                a.JsonFilename.Equals("19_PotassiumSoilTestRanges", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<PotassiumSoilTestRange>>("19_PotassiumSoilTestRanges");
                var currentVersion = _sd.GetCurrentStaticDataVersion();

                foreach (var testRange in migrationSeedData.Data)
                {
                    var currentTestRange = _context.PotassiumSoilTestRanges.Include(s => s.Version).Single(s => s.Id == testRange.Id);
                    currentTestRange.LowerLimit = testRange.LowerLimit;
                    currentTestRange.UpperLimit = testRange.UpperLimit;
                    currentTestRange.Rating = testRange.Rating;
                }

                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.StaticDataVersions.Update(currentVersion);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a =>
                a.JsonFilename.Equals("20_PhosphorusSoilTestRanges", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<PhosphorusSoilTestRange>>("20_PhosphorusSoilTestRanges");
                var currentVersion = _sd.GetCurrentStaticDataVersion();

                foreach (var testRange in migrationSeedData.Data)
                {
                    var currentTestRange = _context.PhosphorusSoilTestRanges.Include(s => s.Version).Single(s => s.Id == testRange.Id);
                    currentTestRange.LowerLimit = testRange.LowerLimit;
                    currentTestRange.UpperLimit = testRange.UpperLimit;
                    currentTestRange.Rating = testRange.Rating;
                }

                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.StaticDataVersions.Update(currentVersion);
                _context.SaveChanges();
            }

            //if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("18_PrevYearManureApplicationNitrogenDefaults", StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    //var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<PreviousYearManureApplicationNitrogenDefault>>("18_PrevYearManureApplicationNitrogenDefaults");
            //    //foreach (var newPrevManureApplicationNitrogenDefault in migrationSeedData.Data)
            //    //{
            //    //    if (_context.PrevYearManureApplicationNitrogenDefaults.Any(up => up.Id == newPrevManureApplicationNitrogenDefault.Id))
            //    //    {
            //    //        var updated = _context.PrevYearManureApplicationNitrogenDefaults.Single(up => up.Id == newPrevManureApplicationNitrogenDefault.Id);
            //    //        _mapper.Map(newPrevManureApplicationNitrogenDefault, updated);
            //    //        _context.PrevYearManureApplicationNitrogenDefaults.Update(updated);
            //    //    }
            //    //}
            //    //_context.AppliedMigrationSeedData.Add(migrationSeedData);
            //    //_context.SaveChanges();

            //    var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<PreviousYearManureApplicationNitrogenDefault>>("18_PrevYearManureApplicationNitrogenDefaults");
            //    foreach (var newPrevManureApplicationNitrogenDefault in migrationSeedData.Data)
            //    {
            //        newPrevManureApplicationNitrogenDefault.Crops = _sd.GetCropsByManureApplicationHistory(newPrevManureApplicationNitrogenDefault.FieldManureApplicationHistory);
            //        newPrevManureApplicationNitrogenDefault.PreviousManureApplicationYear =
            //            _sd.GetPrevManureApplicationInPrevYearsByManureAppHistory(newPrevManureApplicationNitrogenDefault.FieldManureApplicationHistory);
            //        //newPrevManureApplicationNitrogenDefault.StaticDataVersionId = currentVersion.Id;
            //        //newPrevManureApplicationNitrogenDefault.SetVersion(currentVersion);

            //        if (_context.PrevYearManureApplicationNitrogenDefaults.Any(up => up.Id == newPrevManureApplicationNitrogenDefault.Id))
            //        {
            //            var updated = _context.PrevYearManureApplicationNitrogenDefaults.Single(up => up.Id == newPrevManureApplicationNitrogenDefault.Id);
            //            //_mapper.Map(newPrevManureApplicationNitrogenDefault, updated);
            //            updated.Crops = newPrevManureApplicationNitrogenDefault.Crops;
            //            //updated.PreviousManureApplicationYear =
            //            //    newPrevManureApplicationNitrogenDefault.PreviousManureApplicationYear;
            //            //updated.StaticDataVersionId = newPrevManureApplicationNitrogenDefault.StaticDataVersionId;
            //            _context.PrevYearManureApplicationNitrogenDefaults.Update(updated);
            //        }
            //    }
            //    _context.AppliedMigrationSeedData.Add(migrationSeedData);
            //    _context.SaveChanges();
            //}

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("21_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("21_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("22_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("22_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        var updated = _context.UserPrompts.Single(up => up.Id == newUserPrompt.Id);
                        _mapper.Map(newUserPrompt, updated);
                        _context.UserPrompts.Update(updated);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("5_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("5_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("6_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("6_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("24_MainMenu", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<MainMenu>>("24_MainMenu");
                foreach (var menu in migrationSeedData.Data)
                {
                    var mainMenu = _context.MainMenus.Single(up => up.Id == menu.Id);
                    mainMenu.SortNumber = menu.SortNumber;
                    _context.MainMenus.Update(mainMenu);
                }

                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("25_SubMenu", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<SubMenu>>("25_SubMenu");
                foreach (var updatedMenu in migrationSeedData.Data)
                {
                    var menu = _context.MainMenus
                            .Include(m => m.SubMenus)
                            .Single(up => up.Id == updatedMenu.MainMenuId);
                    menu.SubMenus.Single(sb => sb.Id == updatedMenu.Id)
                        .SortNumber = updatedMenu.SortNumber;
                    _context.MainMenus.Update(menu);
                }

                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("8_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("8_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("23_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("23_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("26_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("26_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("27_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("27_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("27_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("27_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("9_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("9_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("10_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("10_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("12_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("12_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("28_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("28_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("98_MiniAppData", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<MiniApp>>("98_MiniAppData");
                foreach (var label in migrationSeedData.Data)
                {
                    if (!_context.MiniApps.Any(up => up.Id == label.Id))
                    {
                        _context.MiniApps.Add(label);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("99_MiniAppLabels", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<MiniAppLabel>>("99_MiniAppLabels");
                foreach (var label in migrationSeedData.Data)
                {
                    if (!_context.MiniAppLabels.Any(up => up.Id == label.Id))
                    {
                        _context.MiniAppLabels.Add(label);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
                _context.SaveChanges();
            }
        }
    }
}