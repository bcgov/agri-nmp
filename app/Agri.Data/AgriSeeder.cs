using Agri.Models.Configuration;
using Agri.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Agri.Data
{
    public class AgriSeeder
    {
        private readonly AgriConfigurationContext _context;
        private readonly IAgriConfigurationRepository _sd;
        private readonly IOptions<AppSettings> _options;

        public AgriSeeder(AgriConfigurationContext context, IAgriConfigurationRepository sd,
            IOptions<AppSettings> options)
        {
            _context = context;
            _sd = sd;
            _options = options;
        }

        public void Seed()
        {
            var refreshDatabaseEnv = Environment.GetEnvironmentVariable("LOAD_SEED_DATA");
            var refreshDatabase = (!string.IsNullOrEmpty(refreshDatabaseEnv) && refreshDatabaseEnv.ToLower() == "true") ||
                _options.Value.LoadSeedData;

            var loadSeedConfigDataAsNewVersion = false;

            var expectedSeedDataVersionEnv = Environment.GetEnvironmentVariable("EXPECTED_SEED_DATA_VERSION");
            var expectedSeedDataVersion = _options.Value.ExpectedSeedDataVersion;

            if (!string.IsNullOrEmpty(expectedSeedDataVersionEnv) && int.TryParse(expectedSeedDataVersionEnv, out int parsedVersion))
            {
                expectedSeedDataVersion = parsedVersion;
            }

            var currentVersion = _sd.GetCurrentStaticDataVersion().Id;

            if (refreshDatabase && currentVersion < expectedSeedDataVersion)
            {
                _context.Database.ExecuteSqlCommand(@"TRUNCATE TABLE ""Browsers"",
                                                                                    ""ExternalLinks"",
                                                                                    ""SubMenu"",
                                                                                    ""MainMenus"",
                                                                                    ""Journey"",
                                                                                    ""MiniAppLabels"",
                                                                                    ""MiniApps"",
                                                                                    ""NutrientIcons"",
                                                                                    ""UserPrompts"";");

                loadSeedConfigDataAsNewVersion = true;
            }

            if (!_context.Browsers.Any())
            {
                var browsers = SeedDataLoader.GetSeedJsonData<List<Browser>>(Constants.SeedDataFiles.Browsers);
                _context.Browsers.AddRange(browsers);
                _context.SaveChanges();
            }

            if (!_context.ExternalLinks.Any())
            {
                var links = SeedDataLoader.GetSeedJsonData<List<ExternalLink>>(Constants.SeedDataFiles.ExternalLinks);
                _context.ExternalLinks.AddRange(links);
                _context.SaveChanges();
            }

            if (!_context.Locations.Any())
            {
                var locations = SeedDataLoader.GetSeedJsonData<List<Location>>(Constants.SeedDataFiles.Location);
                _context.Locations.AddRange(locations);
                _context.SaveChanges();
            }

            if (!_context.Journeys.Any())
            {
                var journeys = SeedDataLoader.GetSeedJsonData<List<Journey>>(Constants.SeedDataFiles.Journey);
                _context.Journeys.AddRange(journeys);
                _context.SaveChanges();
            }

            //if (!_context.ManageVersionUsers.Any())
            //{
            //    var locations = SeedDataLoader.GetSeedJsonData<>(Constants.SeedDataFiles.ManageVersionUsers);
            //    _context.Locations.AddRange(locations);
            //    _context.SaveChanges();
            //}

            if (!_context.MiniApps.Any())
            {
                var miniapps = SeedDataLoader.GetSeedJsonData<List<MiniApp>>(Constants.SeedDataFiles.MiniApps);
                _context.MiniApps.AddRange(miniapps);
                _context.SaveChanges();
            }

            if (!_context.MiniAppLabels.Any())
            {
                var miniapplabels = SeedDataLoader.GetSeedJsonData<List<MiniAppLabel>>(Constants.SeedDataFiles.MiniAppLabels);
                _context.MiniAppLabels.AddRange(miniapplabels);
                _context.SaveChanges();
            }

            if (!_context.NutrientIcons.Any())
            {
                var icons = SeedDataLoader.GetSeedJsonData<List<NutrientIcon>>(Constants.SeedDataFiles.NutrientIcons);
                _context.NutrientIcons.AddRange(icons);
                _context.SaveChanges();
            }

            if (!_context.UserPrompts.Any())
            {
                var prompts = SeedDataLoader.GetSeedJsonData<List<UserPrompt>>(Constants.SeedDataFiles.UserPrompts);
                _context.UserPrompts.AddRange(prompts);
                _context.SaveChanges();
            }

            var seedStaticDataVersion = SeedDataLoader.GetSeedJsonData<StaticDataVersion>(Constants.SeedDataFiles.StaticDataVersion);

            if (!_context.StaticDataVersions.Any() ||
                seedStaticDataVersion.Id > _sd.GetStaticDataVersionId() ||
                loadSeedConfigDataAsNewVersion)
            {
                _sd.LoadConfigurations(seedStaticDataVersion);
            }
        }
    }
}