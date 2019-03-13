using System;
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

        public async Task<IViewComponentResult> InvokeAsync(CoreSiteActions currentAction)
        {
            return View(await GetNavigationAsync(currentAction));
        }

        private Task<NavigationDetailViewModel> GetNavigationAsync(CoreSiteActions currentAction)
        {
            var ndvm = new NavigationDetailViewModel();
            ndvm.mainMenuOptions = new List<MainMenu>();
            ndvm.subMenuOptions = new List<SubMenu>();

            if (_ud.IsActiveSession())
            {
                ndvm.mainMenuOptions = _sd.GetMainMenus();

                var hasAnimals = _ud.FarmDetails()?.HasAnimals ?? true;
                var importsManureCompost = _ud.FarmDetails()?.ImportsManureCompost ?? true;

                var greyOutClass = "top-level-nav-greyedout";

                var noManureCompost = !_ud.GetAllManagedManures().Any(); //Want true to grey out Storage and Nutrient Analysis

                ndvm.mainMenuOptions
                        .Single(s => s.Action.Equals(CoreSiteActions.ManureGeneratedObtained.ToString()))
                        .GreyOutClass = !hasAnimals && !importsManureCompost ? greyOutClass : string.Empty;

                if (currentAction > CoreSiteActions.Home)
                {
                    ndvm.UseInterceptJS = currentAction == CoreSiteActions.Farm;
                    var currentMainMenu =
                        ndvm.mainMenuOptions.SingleOrDefault(m => m.IsCurrentMainMenu(currentAction.ToString()));
                    if (currentMainMenu != null)
                    {
                        currentMainMenu.ElementId = "current";
                        ndvm.subMenuOptions = currentMainMenu.SubMenus;

                        var currentSubMenu = ndvm.subMenuOptions.SingleOrDefault(sm =>
                            sm.Action.Equals(currentAction.ToString(), StringComparison.CurrentCultureIgnoreCase));

                        if (currentSubMenu != null)
                        {
                            currentSubMenu.ElementId = "current2";
                        }

                        if (currentMainMenu.Controller.Equals(AppControllers.ManureManagement.ToString()))
                        {

                            if (currentMainMenu.Controller == AppControllers.ManureManagement.ToString())
                            {
                                greyOutClass = "second-level-nav-greyedout";
                                ndvm.subMenuOptions
                                    .Single(s => s.Action.Equals(CoreSiteActions.ManureGeneratedObtained.ToString()))
                                    .GreyOutClass = !hasAnimals ? greyOutClass : string.Empty;

                                ndvm.subMenuOptions
                                    .Single(s => s.Action.Equals(CoreSiteActions.ManureImported.ToString()))
                                    .GreyOutClass = !importsManureCompost ? greyOutClass : string.Empty;

                                ndvm.subMenuOptions
                                        .Single(s => s.Action.Equals(CoreSiteActions.ManureStorage.ToString()))
                                        .GreyOutClass = !hasAnimals && !importsManureCompost
                                        ? greyOutClass
                                        : string.Empty;

                                ndvm.subMenuOptions
                                        .Single(s => s.Action.Equals(CoreSiteActions.ManureNutrientAnalysis.ToString()))
                                        .GreyOutClass = !hasAnimals && !importsManureCompost
                                        ? greyOutClass
                                        : string.Empty;
                            }
                        }
                    }
                }
            }

            return Task.FromResult(ndvm);
        }

    }
}
