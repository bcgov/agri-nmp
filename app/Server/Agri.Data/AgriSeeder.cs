﻿using System;
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

            if (!_context.CropTypes.Any())
            {
                var types = staticDataRepo.GetCropTypes();
                _context.CropTypes.AddRange(types);
            }

            if (!_context.Crops.Any())
            {
                var crops = staticDataRepo.GetCrops();
                var stksRegions = staticExtRepo.GetCropStkRegionCds();
                var stpsRegions = staticExtRepo.GetCropStpRegionCds();
                var cropYields = staticExtRepo.GetCropYields();
                foreach (var crop in crops)
                {
                    if (stksRegions.Any(s => s.CropId == crop.Id))
                    {
                        crop.CropSTKRegionCds.AddRange(stksRegions.Where(s => s.CropId == crop.Id));
                    }

                    if (stpsRegions.Any(s => s.CropId == crop.Id))
                    {
                        crop.CropSTPRegionCds.AddRange(stpsRegions.Where(s => s.CropId == crop.Id));
                    }

                    if (cropYields.Any(s => s.CropId == crop.Id))
                    {
                        crop.CropYields.AddRange(cropYields.Where(s => s.CropId == crop.Id));
                    }
                }

                _context.Crops.AddRange(crops);
            }

            _context.SaveChanges();
        }
    }
}
