using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class Navigation : ViewComponent
    {
        private readonly IAgriConfigurationRepository _sd;
        private readonly Models.Impl.UserData _ud;

        public Navigation(IAgriConfigurationRepository sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync(NavigationDetailViewModel currentNavViewModel)
        {
            return View(await GetNavigationAsync(currentNavViewModel));
        }

        private Task<NavigationDetailViewModel> GetNavigationAsync(NavigationDetailViewModel currentNavViewModel)
        {
            var ndvm = new NavigationDetailViewModel
            {
                MainMenus = new List<MainMenu>(),
                SubMenus = new List<SubMenu>()
            };

            if (_ud.IsActiveSession())
            {
                var journey = _ud.FarmDetails().UserJourney;
                ndvm.MainMenus = _sd.GetJourney((int)journey)
                    .MainMenus
                    .OrderBy(m => m.SortNumber)
                    .ToList();

                var currentAction = CoreSiteActions.NotUsed;
                var currentPage = FeaturePages.NotUsed;

                if (currentNavViewModel.UsesFeaturePages)
                {
                    if (currentNavViewModel.CurrentPage.ToLower().EndsWith("index"))
                    {
                        currentPage = EnumHelper<FeaturePages>.GetValueFromDescription(currentNavViewModel.CurrentPage);
                    }
                    else
                    {
                        var indexPage = currentNavViewModel
                            .CurrentPage.Substring(0, currentNavViewModel.CurrentPage.LastIndexOf('/') + 1);
                        currentPage = EnumHelper<FeaturePages>.GetValueFromDescription($"{indexPage}Index");
                    }
                }
                else
                {
                    currentAction = EnumHelper<CoreSiteActions>.Parse(currentNavViewModel.CurrentAction);
                }

                if (currentNavViewModel.UsesFeaturePages || currentAction > CoreSiteActions.Home)
                {
                    ndvm.UseInterceptJS = currentAction == CoreSiteActions.Farm;
                    var currentMainMenu = ndvm.MainMenus
                        .SingleOrDefault(m => m.IsCurrentMainMenu(currentAction) || m.IsCurrentMainMenu(currentPage));

                    if (currentMainMenu != null)
                    {
                        currentMainMenu.ElementId = "current";
                        ndvm.SubMenus = currentMainMenu.SubMenus.OrderBy(s => s.SortNumber).ToList();

                        var currentSubMenu = ndvm.SubMenus.SingleOrDefault(sm =>
                            sm.IsSubMenuCurrent(currentAction) || sm.IsSubMenuCurrent(currentPage));

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