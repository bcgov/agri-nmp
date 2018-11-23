using System.Linq;
using Agri.LegacyData.Models.Impl;
using Microsoft.EntityFrameworkCore;

namespace Agri.Data
{
    public class AgriSeeder
    {
        private AgriConfigurationContext _context;

        public AgriSeeder(AgriConfigurationContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            //If the database is not present or if migrations are required
            //create the database and/or run the migrations
            _context.Database.Migrate();

            var staticDataRepo = new StaticDataRepository();
            var staticExtRepo = new StaticDataExtRepository();

            //AmoniaRetention
            if (!_context.AmmoniaRetentions.Any())
            {
                var ammoniaRetentions = staticExtRepo.GetAmmoniaRetentions();
                _context.AmmoniaRetentions.AddRange(ammoniaRetentions);
            }

            //Animal
            //AnimalSubType
            if (!_context.Animals.Any())
            {
                var animals = staticDataRepo.GetAnimals();
                var animalSubtypes = staticDataRepo.GetAnimalSubTypes();
                foreach (var animal in animals)
                {
                    var subtypes = animalSubtypes.Where(s => s.AnimalId == animal.Id).ToList();
                    if (subtypes.Any())
                    {
                        animal.AnimalSubTypes.AddRange(subtypes);
                    }
                }
                _context.Animals.AddRange(animals);
            }

            //Browser
            if (!_context.Browsers.Any())
            {
                var browsers = staticDataRepo.GetAllowableBrowsers();
                _context.Browsers.AddRange(browsers);
            }

            //ConversionFactor
            if (!_context.ConversionFactors.Any())
            {
                var cFactor = staticDataRepo.GetConversionFactor();
                _context.ConversionFactors.Add(cFactor);
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
                var regions = staticDataRepo.GetRegions();
                _context.Regions.AddRange(regions);
            }

            //CropTypes
            if (!_context.CropTypes.Any())
            {
                var types = staticDataRepo.GetCropTypes();
                _context.CropTypes.AddRange(types);
            }

            //PrevManureApplicationYear
            if (!_context.PrevManureApplicationYears.Any())
            {
                var years = staticDataRepo.GetPrevManureApplicationInPrevYears();
                _context.PrevManureApplicationYears.AddRange(years);
            }

            //PrevYearManureApplicationlDefaultNitrogen
            if (!_context.PrevYearManureApplicationNitrogenDefaults.Any())
            {
                var nitrogenDefaults = staticDataRepo.GetPrevYearManureNitrogenCreditDefaults();
                _context.PrevYearManureApplicationNitrogenDefaults.AddRange(nitrogenDefaults);
            }

            //Crops
            //CropStkRegion
            //CropStpRegion
            //CropYield
            //PrevCropType
            if (!_context.Crops.Any())
            {
                var crops = staticDataRepo.GetCrops();
                var stksRegions = staticExtRepo.GetCropSoilTestPotassiumRegions();
                var stpsRegions = staticExtRepo.GetCropSoilTestPhosphorousRegions();
                var cropYields = staticExtRepo.GetCropYields();
                var prevCropTypes = staticDataRepo.GetPreviousCropTypes();

                foreach (var crop in crops)
                {
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
                
                //DensityType
                if (!_context.DensityUnits.Any())
                {
                    var units = staticDataRepo.GetDensityUnits();
                    _context.DensityUnits.AddRange(units);
                }

                //Fertilizer
                if (!_context.Fertilizers.Any())
                {
                    var fertilizers = staticDataRepo.GetFertilizers();
                    _context.Fertilizers.AddRange(fertilizers);
                }

                //FertilizerType
                if (!_context.FertilizerTypes.Any())
                {
                    var types = staticDataRepo.GetFertilizerTypes();
                    _context.FertilizerTypes.AddRange(types);
                }

                //FertilizerUnit
                if (!_context.FertilizerUnits.Any())
                {
                    var units = staticDataRepo.GetFertilizerUnits();
                    _context.FertilizerUnits.AddRange(units);
                }

                //LiquidFertilizerDensity
                if (!_context.LiquidFertilizerDensities.Any())
                {
                    var densities = staticExtRepo.GetLiquidFertilizerDensities();
                    _context.LiquidFertilizerDensities.AddRange(densities);
                }

                //DryMatter
                if (!_context.DryMatters.Any())
                {
                    var dms = staticExtRepo.GetDryMatters();
                    _context.DryMatters.AddRange(dms);
                }

                //NMineralization
                if (!_context.NitrogenMineralizations.Any())
                {
                    var minerals = staticExtRepo.GetNitrogeMineralizations();
                    _context.NitrogenMineralizations.AddRange(minerals);
                }

                //Manure
                if (!_context.Manures.Any())
                {
                    var manures = staticDataRepo.GetManures();
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
                    var stks = staticExtRepo.GetSoilTestPotassiumRecommendations();

                    foreach (var stkKelownaRange in ranges)
                    {
                        if (stks.Any(s => s.SoilTestPotassiumKelownaRangeId == stkKelownaRange.Id))
                        {
                            stkKelownaRange.SoilTestPotassiumRecommendations.AddRange(stks.Where(s => s.SoilTestPotassiumKelownaRangeId == stkKelownaRange.Id));
                        }
                    }

                    _context.SoilTestPotassiumKelownaRanges.AddRange(ranges);
                }

                //STPKelownaRange
                //STPRecommendations
                if (!_context.SoilTestPhosphorousKelownaRanges.Any())
                {
                    var ranges = staticExtRepo.GetSoilTestPhosphorousKelownaRanges();
                    var stps = staticExtRepo.GetSoilTestPhosphorousRecommendations();

                    foreach (var stpKelownaRange in ranges)
                    {
                        if (stps.Any(s => s.SoilTestPhosphorousKelownaRangeId == stpKelownaRange.Id))
                        {
                            stpKelownaRange.SoilTestPhosphorousRecommendations.AddRange(stps.Where(s => s.SoilTestPhosphorousKelownaRangeId == stpKelownaRange.Id));
                        }
                    }
                    _context.SoilTestPhosphorousKelownaRanges.AddRange(ranges);
                }

                _context.Crops.AddRange(crops);
            }

            if (!_context.FertilizerMethods.Any())
            {
                var fertilizerMethods = staticDataRepo.GetFertilizerMethods();
                _context.FertilizerMethods.AddRange(fertilizerMethods);
            }

            if (!_context.UserPrompts.Any())
            {
                var userPrompts = staticExtRepo.GetUserPromts();
                _context.UserPrompts.AddRange(userPrompts);
            }

            if (!_context.ExternalLinks.Any())
            {
                var externalLinks = staticExtRepo.GetExternalLinks();
                _context.ExternalLinks.AddRange(externalLinks);
            }

            if (!_context.Units.Any())
            {
                var units = staticDataRepo.GetUnits();
                _context.Units.AddRange(units);
            }

            if (!_context.SoilTestMethods.Any())
            {
                var soilTestMethods = staticDataRepo.GetSoilTestMethods();
                _context.SoilTestMethods.AddRange(soilTestMethods);
            }

            if (!_context.SoilTestPhosphorusRanges.Any())
            {
                var getSoilTestPhosphorusRanges = staticExtRepo.GetSoilTestPhosphorusRanges();
                _context.SoilTestPhosphorusRanges.AddRange(getSoilTestPhosphorusRanges);
            }

            if (!_context.SoilTestPotassiumRanges.Any())
            {
                var getSoilTestPotassiumRanges = staticExtRepo.GetSoilTestPotassiumRanges();
                _context.SoilTestPotassiumRanges.AddRange(getSoilTestPotassiumRanges);
            }

            if (!_context.Messages.Any())
            {
                var getMessages = staticExtRepo.GetMessages();
                _context.Messages.AddRange(getMessages);
            }

            if (!_context.SeasonApplications.Any())
            {
                var getSeasonApplications = staticExtRepo.GetSeasonApplications();
                _context.SeasonApplications.AddRange(getSeasonApplications);
            }

            if (!_context.Yields.Any())
            {
                var getYields = staticExtRepo.GetYields();
                _context.Yields.AddRange(getYields);
            }

            if (!_context.NitrogenRecommendations.Any())
            {
                var getNitrogenRecommendations = staticExtRepo.GetNitrogenRecommendations();
                _context.NitrogenRecommendations.AddRange(getNitrogenRecommendations);
            }

            if (!_context.RptCompletedManureRequiredStdUnits.Any())
            {
                var rptCompletedManureRequiredStdUnits = staticExtRepo.GetRptCompletedManureRequiredStdUnit();
                _context.RptCompletedManureRequiredStdUnits.AddRange(rptCompletedManureRequiredStdUnits);
            }

            if (!_context.RptCompletedFertilizerRequiredStdUnits.Any())
            {
                var rptCompletedManureRequiredStdUnits = staticExtRepo.GetRptCompletedFertilizerRequiredStdUnit();
                _context.RptCompletedFertilizerRequiredStdUnits.AddRange(rptCompletedManureRequiredStdUnits);
            }

            //HarvestUnit
            if (!_context.HarvestUnits.Any())
            {
                var units = staticExtRepo.GetHarvestUnits();
                _context.HarvestUnits.AddRange(units);
            }

            if (!_context.BCSampleDateForNitrateCredit.Any())
            {
                var bcSampleDateForNitrateCredit = staticExtRepo.GetBCSampleDateForNitrateCredit();
                _context.BCSampleDateForNitrateCredit.AddRange(bcSampleDateForNitrateCredit);
            }

            //Nutrient Icons
            if (!_context.NutrientIcons.Any())
            {
                var icons = staticDataRepo.GetNutrientIcons();
                _context.NutrientIcons.AddRange(icons);
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

            _context.SaveChanges();
        }
    }
}
