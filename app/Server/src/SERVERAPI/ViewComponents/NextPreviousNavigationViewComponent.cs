using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class NextPreviousNavigationViewComponent : ViewComponent
    {
        private readonly IAgriConfigurationRepository _sd;
        private readonly UserData _ud;

        public NextPreviousNavigationViewComponent(IAgriConfigurationRepository sd, UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync(CoreSiteActions currentAction)
        {
            return View(await GetNavigation(currentAction));
        }

        private Task<NextPreviousNavigationViewModel> GetNavigation(CoreSiteActions currentAction)
        {
            var journey = _ud.FarmDetails().UserJourney;
            var mainMenuOptions = _sd.GetJourney((int)journey)
                .MainMenus
                .OrderBy(m => m.SortNumber)
                .ToList();

            var currentMainMenuItem = mainMenuOptions
                    .Single(m => m.IsCurrentMainMenu(currentAction) ||
                                    m.SubMenus.Any(sm => sm.IsSubMenuCurrent(currentAction)));

            var currentMenuItem = currentMainMenuItem.SubMenus.Any() ?
                currentMainMenuItem.SubMenus.Single(s => s.IsSubMenuCurrent(currentAction)) as Menu :
                currentMainMenuItem as Menu;

            var mnvm = new NextPreviousNavigationViewModel
            {
                UseJSInterceptMethod = currentMenuItem.UseJavaScriptInterceptMethod,
                PreviousController = EnumHelper<AppControllers>.Parse(currentMenuItem.PreviousController),
                PreviousAction = EnumHelper<CoreSiteActions>.Parse(currentMenuItem.PreviousAction),
                NextController = EnumHelper<AppControllers>.Parse(currentMenuItem.NextController),
                NextAction = EnumHelper<CoreSiteActions>.Parse(currentMenuItem.NextAction)
            };

            if (currentAction == CoreSiteActions.Calculate)
            {
                ProcessCalculateNavigation(currentMenuItem, mnvm);
            }

            mnvm.ViewPreviousUrl = Url.Action(mnvm.ViewPreviousAction,
                mnvm.ViewPreviousController,
                mnvm.PreviousParameters);

            mnvm.ViewNextUrl = Url.Action(mnvm.ViewNextAction,
                mnvm.ViewNextController,
                mnvm.NextParameters);

            return Task.FromResult(mnvm);
        }

        private NextPreviousNavigationViewModel ProcessCalculateNavigation(Menu currentMenuItem, NextPreviousNavigationViewModel mnvm)
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
                f.fieldName.Equals(currentField.ToString(), StringComparison.CurrentCultureIgnoreCase));

            if (currentFieldIndex == 0)
            {
                result.PreviousAction = EnumHelper<CoreSiteActions>.Parse(currentMenuItem.PreviousAction);
                result.PreviousController = EnumHelper<AppControllers>.Parse(currentMenuItem.PreviousController);
            }
            else
            {
                result.PreviousParameters = new { nme = fields[currentFieldIndex - 1].fieldName };
            }

            if (currentFieldIndex + 1 < fields.Count)
            {
                result.NextParameters = new { nme = fields[currentFieldIndex + 1].fieldName };
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