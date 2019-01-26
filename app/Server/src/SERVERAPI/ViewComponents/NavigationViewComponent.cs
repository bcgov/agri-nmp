using Agri.Interfaces;
using Agri.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Models;

namespace SERVERAPI.ViewComponents
{
    public class Navigation: ViewComponent
    {
        private IAgriConfigurationRepository _sd;
        private Models.Impl.UserData _ud;

        public Navigation(IAgriConfigurationRepository sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetNavigationAsync());
        }

        private Task<NavigationDetailViewModel> GetNavigationAsync()
        {
            NavigationDetailViewModel ndvm = new NavigationDetailViewModel();
            var hasAnimals = _ud.FarmDetails()?.HasAnimals ?? true;
            var importsManureCompost = _ud.FarmDetails()?.ImportsManureCompost ?? true;

            ndvm.mainMenuOptions = new List<MainMenu>();
            ndvm.mainMenuOptions = _sd.GetMainMenus();

            ndvm.subMenuOptions = new List<SubMenu>();
            ndvm.subMenuOptions = _sd.GetSubMenus();

            var noManureCompost =
                !_ud.GetAllManagedManures().Any(); //Want true to grey out Storage and Nutrient Analysis

            ndvm.mainMenuOptions
                    .Single(s => s.Action.Equals(CoreSiteActions.ManureGeneratedObtained.ToString()))
                    .GreyOutText = !hasAnimals && !importsManureCompost;

            ndvm.subMenuOptions
                .Single(s => s.Action.Equals(CoreSiteActions.ManureGeneratedObtained.ToString()))
                .GreyOutText = !hasAnimals;

            ndvm.subMenuOptions
                .Single(s => s.Action.Equals(CoreSiteActions.ManureImported.ToString()))
                .GreyOutText = !importsManureCompost;

            ndvm.subMenuOptions
                    .Single(s => s.Action.Equals(CoreSiteActions.ManureStorage.ToString()))
                    .GreyOutText = !hasAnimals && !importsManureCompost;

            ndvm.subMenuOptions
                    .Single(s => s.Action.Equals(CoreSiteActions.ManureNutrientAnalysis.ToString()))
                    .GreyOutText = !hasAnimals && !importsManureCompost;

            return Task.FromResult(ndvm);
        }

    }
}
