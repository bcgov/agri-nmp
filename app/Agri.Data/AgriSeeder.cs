using Agri.Interfaces;
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
        private readonly AgriConfigurationContext _context;
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
            if (_context.StaticDataVersions.Any())
            {
                return;
            }

            var seedData = SeedDataLoader.GetSeedJsonData();
            _sd.LoadConfigurations(seedData);
        }

        public void AppliedMigrationsSeedData()
        {
            ////Updates
            //if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("1_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("1_UserPrompts");
            //    foreach (var newUserPrompt in migrationSeedData.Data)
            //    {
            //        if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
            //        {
            //            _context.UserPrompts.Add(newUserPrompt);
            //        }
            //    }
            //    _context.AppliedMigrationSeedData.Add(migrationSeedData);
            //    _context.SaveChanges();
            //}
        }
    }
}