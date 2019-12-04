using Agri.Interfaces;
using Agri.Models.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Agri.Data
{
    public class AgriSeeder
    {
        private AgriConfigurationContext _context;
        private readonly IAgriConfigurationRepository _sd;

        public AgriSeeder(AgriConfigurationContext context, IAgriConfigurationRepository sd)
        {
            _context = context;
            _sd = sd;
        }

        public void Seed()
        {
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

            if (!_context.MainMenus.Any())
            {
                var menus = SeedDataLoader.GetSeedJsonData<List<MainMenu>>(Constants.SeedDataFiles.MainMenus);
                _context.MainMenus.AddRange(menus);
                _context.SaveChanges();
            }

            //if (!_context.ManageVersionUsers.Any())
            //{
            //    var locations = SeedDataLoader.GetSeedJsonData<>(Constants.SeedDataFiles.ManageVersionUsers);
            //    _context.Locations.AddRange(locations);
            //    _context.SaveChanges();
            //}

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

            if (!_context.StaticDataVersions.Any())
            {
                var staticDataVersion = SeedDataLoader.GetSeedJsonData<StaticDataVersion>(Constants.SeedDataFiles.StaticDataVersion);
                _sd.LoadConfigurations(staticDataVersion);
            }
        }
    }
}