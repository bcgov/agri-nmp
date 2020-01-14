using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
using Common;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class NextPreviousNavigationViewComponent : ViewComponent
    {
        private readonly IAgriConfigurationRepository _sd;
        private readonly UserData _ud;
        private readonly List<MainMenu> _mainMenuOptions;

        public NextPreviousNavigationViewComponent(IAgriConfigurationRepository sd, UserData ud)
        {
            _sd = sd;
            _ud = ud;
            var journey = _ud.FarmDetails().UserJourney;
            _mainMenuOptions = _sd.GetJourney((int)journey)
                .MainMenus
                .OrderBy(m => m.SortNumber)
                .ToList();
        }

        public async Task<IViewComponentResult> InvokeAsync(NextPrevNavViewModel currentNextPrevVm)
        {
            if (currentNextPrevVm.CurrentIsAPage)
            {
                return View(await GetNavigation(currentNextPrevVm.CurrentPage));
            }
            return View(await GetNavigation(currentNextPrevVm.CurrentAction));
        }

        private Task<NextPrevNavViewModel> GetNavigation(FeaturePages currentPage)
        {
            var pageRoute = currentPage.GetDescription();
            var currentMainMenuItem = _mainMenuOptions
                    .Single(m => m.IsCurrentMainMenu(pageRoute) ||
                                    m.SubMenus.Any(sm => sm.IsSubMenuCurrent(pageRoute)));

            var currentMenuItem = currentMainMenuItem.SubMenus.Any() ?
                currentMainMenuItem.SubMenus.Single(s => s.IsSubMenuCurrent(pageRoute)) as Menu :
                currentMainMenuItem as Menu;

            var mnvm = PopulateViewModel(currentMenuItem);

            return Task.FromResult(mnvm);
        }

        private Task<NextPrevNavViewModel> GetNavigation(CoreSiteActions currentAction)
        {
            var currentMainMenuItem = _mainMenuOptions
                    .Single(m => m.IsCurrentMainMenu(currentAction) ||
                                    m.SubMenus.Any(sm => sm.IsSubMenuCurrent(currentAction)));

            var currentMenuItem = currentMainMenuItem.SubMenus.Any() ?
                currentMainMenuItem.SubMenus.Single(s => s.IsSubMenuCurrent(currentAction)) as Menu :
                currentMainMenuItem as Menu;

            var mnvm = PopulateViewModel(currentMenuItem);

            return Task.FromResult(mnvm);
        }

        private NextPrevNavViewModel PopulateViewModel(Menu currentMenuItem)
        {
            var viewModel = new NextPrevNavViewModel
            {
                UseJSInterceptMethod = currentMenuItem.UseJavaScriptInterceptMethod,
                PreviousController = EnumHelper<AppControllers>.Exists(currentMenuItem.PreviousController) ?
                    EnumHelper<AppControllers>.Parse(currentMenuItem.PreviousController) : AppControllers.NotUsed,
                PreviousAction = EnumHelper<CoreSiteActions>.Exists(currentMenuItem.PreviousAction) ?
                    EnumHelper<CoreSiteActions>.Parse(currentMenuItem.PreviousAction) : CoreSiteActions.NotUsed,
                NextController = EnumHelper<AppControllers>.Exists(currentMenuItem.NextController) ?
                    EnumHelper<AppControllers>.Parse(currentMenuItem.NextController) : AppControllers.NotUsed,
                NextAction = EnumHelper<CoreSiteActions>.Exists(currentMenuItem.NextAction) ?
                    EnumHelper<CoreSiteActions>.Parse(currentMenuItem.NextAction) : CoreSiteActions.NotUsed,
                PreviousPage = EnumHelper<FeaturePages>.ExistsWithDescription(currentMenuItem.PreviousPage) ?
                    EnumHelper<FeaturePages>.GetValueFromDescription(currentMenuItem.PreviousPage) : FeaturePages.NotUsed,
                NextPage = EnumHelper<FeaturePages>.ExistsWithDescription(currentMenuItem.NextPage) ?
                    EnumHelper<FeaturePages>.GetValueFromDescription(currentMenuItem.NextPage) : FeaturePages.NotUsed
            };

            if (currentMenuItem.Action != null &&
                currentMenuItem.Action.Equals(CoreSiteActions.Calculate.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ProcessCalculateNavigation(currentMenuItem, viewModel);
            }

            viewModel.ViewPreviousUrl = viewModel.PreviousIsAPage ?
                Url.Page(viewModel.PreviousPage.GetDescription()) :
                Url.Action(viewModel.ViewPreviousAction, viewModel.ViewPreviousController, viewModel.PreviousParameters);

            viewModel.ViewNextUrl = viewModel.NextIsAPage ?
                Url.Page(viewModel.NextPage.GetDescription()) :
                Url.Action(viewModel.ViewNextAction, viewModel.ViewNextController, viewModel.NextParameters);

            return viewModel;
        }

        private NextPrevNavViewModel ProcessCalculateNavigation(Menu currentMenuItem, NextPrevNavViewModel mnvm)
        {
            var result = mnvm;
            var fields = _ud.GetFields();
            var currentField = HttpContext.Request.Query["nme"];

            if (fields.Count == 0)
            {
                return result;
            }

            result.NextAction = CoreSiteActions.Calculate;
            result.NextController = AppControllers.Nutrients;
            result.PreviousAction = CoreSiteActions.Calculate;
            result.PreviousController = AppControllers.Nutrients;

            var currentFieldIndex = !currentField.Any() ? 0 :
                fields.FindIndex(f =>
                f.FieldName.Equals(currentField.ToString(), StringComparison.CurrentCultureIgnoreCase));

            if (currentFieldIndex == 0)
            {
                result.PreviousAction = EnumHelper<CoreSiteActions>.Parse(currentMenuItem.PreviousAction);
                result.PreviousController = EnumHelper<AppControllers>.Parse(currentMenuItem.PreviousController);
            }
            else
            {
                result.PreviousParameters = new { nme = fields[currentFieldIndex - 1].FieldName };
            }

            if (currentFieldIndex + 1 < fields.Count)
            {
                result.NextParameters = new { nme = fields[currentFieldIndex + 1].FieldName };
            }
            else
            {
                result.NextAction = EnumHelper<CoreSiteActions>.Parse(currentMenuItem.NextAction);
                result.NextController = EnumHelper<AppControllers>.Parse(currentMenuItem.NextController);
            }

            return result;
        }
    }
}