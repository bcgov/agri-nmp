using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.LegacyData.Models;
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
            _context.Database.EnsureDeleted();
            _context.Database.Migrate();

            var staticDataRepo = new StaticDataRepository();
            var staticExtRepo = new StaticDataExtRepository();

            if (!_context.AmmoniaRetentions.Any())
            {
                var ammoniaRetentions = staticExtRepo.GetAmmoniaRetentions();
                _context.AmmoniaRetentions.AddRange(ammoniaRetentions);
            }

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

            if (!_context.Browsers.Any())
            {
                var browsers = staticDataRepo.GetAllowableBrowsers();
                _context.Browsers.AddRange(browsers);
            }

            if (!_context.ConversionFactors.Any())
            {
                var cFactor = staticDataRepo.GetConversionFactor();
                _context.ConversionFactors.Add(cFactor);
            }

            if (!_context.Locations.Any())
            {
                var locations = staticExtRepo.GetLocations();
                _context.Locations.AddRange(locations);
            }

            if (!_context.SoilTestMethods.Any())
            {
                var soilTestMethods = staticDataRepo.GetSoilTestMethods();
                _context.SoilTestMethods.AddRange(soilTestMethods);
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

            if (!_context.BCSampleDateForNitrateCredit.Any())
            {
                var bcSampleDateForNitrateCredit = staticExtRepo.GetBCSampleDateForNitrateCredit();
                _context.BCSampleDateForNitrateCredit.AddRange(bcSampleDateForNitrateCredit);
            }

            _context.SaveChanges();
        }
    }
}
