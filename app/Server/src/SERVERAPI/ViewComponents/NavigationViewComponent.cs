using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
using Common;
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

                        ndvm.SubMenus = FilterRanchSubMenus(journey, ndvm.SubMenus);
                        ndvm.SubMenus = FilterPoultrySubMenus(journey, ndvm.SubMenus);

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

        private List<SubMenu> FilterRanchSubMenus(UserJourney journey, List<SubMenu> subMenus)
        {
            //Skip Nutrients Menu for Ranch if Nothing to Analyze
            if (journey == UserJourney.Ranch)
            {
                if (subMenus.Any(sm => sm.UsesFeaturePages &&
                        sm.Page.Equals(FeaturePages.RanchNutrientsIndex.GetDescription())) &&
                    !_ud.GetFarmManures().Any() &&
                    !_ud.GetAllManagedManures().Any(mm => !mm.AssignedWithNutrientAnalysis))
                {
                    //Hide Nutrient Analysis Sub Menu
                    subMenus
                        .Remove(subMenus
                        .Single(sm => sm.Page.Equals(FeaturePages.RanchNutrientsIndex.GetDescription())));
                }

                if (subMenus.Any(sm => sm.UsesFeaturePages &&
                         sm.Page.Equals(FeaturePages.RanchFeedingIndex.GetDescription())) &&
                        !_ud.GetFields().Any(x => x.IsSeasonalFeedingArea))
                {
                    //Hide Nutrient Analysis Sub Menu
                    subMenus
                        .Remove(subMenus
                        .Single(sm => sm.UsesFeaturePages &&
                            sm.Page.Equals(FeaturePages.RanchFeedingIndex.GetDescription())));
                }
            }

            return subMenus;
        }

        private List<SubMenu> FilterPoultrySubMenus(UserJourney journey, List<SubMenu> subMenus)
        {
            //Skip Nutrients Menu for Ranch if Nothing to Analyze
            if (journey == UserJourney.Poultry)
            {
                if (subMenus.Any(sm => sm.UsesFeaturePages &&
                        sm.Page.Equals(FeaturePages.PoultryNutrientsIndex.GetDescription())) &&
                    !_ud.GetFarmManures().Any() &&
                    !_ud.GetAllManagedManures().Any(mm => !mm.AssignedWithNutrientAnalysis))
                {
                    //Hide Nutrient Analysis Sub Menu
                    subMenus
                        .Remove(subMenus
                        .Single(sm => sm.Page.Equals(FeaturePages.PoultryNutrientsIndex.GetDescription())));
                }
            }

            return subMenus;
        }
    }
}