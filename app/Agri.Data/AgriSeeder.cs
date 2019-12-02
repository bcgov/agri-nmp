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

            var seedData = SeedDataLoader.GetSeedJsonData();
            _sd.LoadConfigurations(seedData);
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
        }
    }
}