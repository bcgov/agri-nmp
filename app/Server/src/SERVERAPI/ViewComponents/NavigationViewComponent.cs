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
    public class Navigation : ViewComponent
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
                var journey = _ud.FarmDetails().UserJourney;
                ndvm.mainMenuOptions = _sd.GetJourney((int)journey)
                    .MainMenus
                    .OrderBy(m => m.SortNumber)
                    .ToList();

                var hasAnimals = _ud.FarmDetails()?.HasAnimals ?? true;
                var importsManureCompost = _ud.FarmDetails()?.ImportsManureCompost ?? true;

                if (currentAction > CoreSiteActions.Home)
                {
                    ndvm.UseInterceptJS = currentAction == CoreSiteActions.Farm;
                    var currentMainMenu =
                        ndvm.mainMenuOptions.SingleOrDefault(m => m.IsCurrentMainMenu(currentAction.ToString()));
                    if (currentMainMenu != null)
                    {
                        currentMainMenu.ElementId = "current";
                        ndvm.subMenuOptions = currentMainMenu.SubMenus.OrderBy(s => s.SortNumber).ToList();

                        var currentSubMenu = ndvm.subMenuOptions.SingleOrDefault(sm =>
                            sm.Action.Equals(currentAction.ToString(), StringComparison.CurrentCultureIgnoreCase));

                        if (currentSubMenu != null)
                        {
                            currentSubMenu.ElementId = "current2";
                        }
                    }
                }
            }

            return Task.FromResult(ndvm);
        }
    }
}