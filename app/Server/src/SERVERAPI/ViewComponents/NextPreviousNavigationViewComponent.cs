using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;

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
            return View(await GetManureNavigation(currentAction));
        }

        private Task<NextPreviousNavigationViewModel> GetManureNavigation(CoreSiteActions currentAction)
        {
            var journey = _ud.FarmDetails().UserJourney;
            var mainMenuOptions = _sd.GetJourney((int)journey)
                .MainMenus
                .OrderBy(m => m.SortNumber)
                .ToList();

            var hasAnimals = _ud.FarmDetails()?.HasAnimals ?? true;
            var importsManureCompost = _ud.FarmDetails()?.ImportsManureCompost ?? true;
            var mnvm = new NextPreviousNavigationViewModel
            {
                UseJSInterceptMethod = false,
                PreviousController = AppControllers.ManureManagement,
                NextController = AppControllers.ManureManagement
            };

            if (currentAction == CoreSiteActions.Farm)
            {
                mnvm.NextAction = CoreSiteActions.ManureGeneratedObtained;
                if (!hasAnimals)
                {
                    mnvm.NextAction = CoreSiteActions.ManureImported;
                }

                if (!importsManureCompost)
                {
                    mnvm.NextAction = CoreSiteActions.Fields;
                    mnvm.NextController = AppControllers.Fields;
                }
            }

            if (currentAction == CoreSiteActions.ManureGeneratedObtained)
            {
                mnvm.PreviousAction = CoreSiteActions.Farm;
                mnvm.PreviousController = AppControllers.Farm;
                if (importsManureCompost)
                {
                    mnvm.NextAction = CoreSiteActions.ManureImported;
                }
                else if (hasAnimals)
                {
                    mnvm.NextAction = CoreSiteActions.ManureStorage;
                }
                else
                {
                    mnvm.NextAction = CoreSiteActions.Fields;
                    mnvm.NextController = AppControllers.Fields;
                }
            }

            if (currentAction == CoreSiteActions.ManureImported)
            {
                mnvm.PreviousAction = CoreSiteActions.ManureGeneratedObtained;

                if (!hasAnimals)
                {
                    //Skip Previous Generated
                    mnvm.PreviousAction = CoreSiteActions.Farm;
                    mnvm.PreviousController = AppControllers.Farm;
                }
                mnvm.NextAction = CoreSiteActions.ManureStorage;
                if (!hasAnimals && !importsManureCompost)
                {
                    mnvm.NextAction = CoreSiteActions.Fields;
                    mnvm.NextController = AppControllers.Fields;
                }
            }

            if (currentAction == CoreSiteActions.ManureStorage)
            {
                mnvm.PreviousAction = CoreSiteActions.ManureImported;
                if (!importsManureCompost && hasAnimals)
                {
                    mnvm.PreviousAction = CoreSiteActions.ManureGeneratedObtained;
                }
                else
                {
                    mnvm.PreviousAction = CoreSiteActions.Farm;
                    mnvm.PreviousController = AppControllers.Farm;
                }

                if (hasAnimals || importsManureCompost)
                {
                    mnvm.NextAction = CoreSiteActions.ManureNutrientAnalysis;
                }
                else
                {
                    mnvm.NextAction = CoreSiteActions.Fields;
                    mnvm.NextController = AppControllers.Fields;
                }
            }

            if (currentAction == CoreSiteActions.ManureNutrientAnalysis)
            {
                mnvm.PreviousAction = CoreSiteActions.ManureStorage;
                if (!hasAnimals && !importsManureCompost)
                {
                    mnvm.PreviousAction = CoreSiteActions.Farm;
                    mnvm.PreviousController = AppControllers.Farm;
                }

                mnvm.NextAction = CoreSiteActions.Fields;
                mnvm.NextController = AppControllers.Fields;
            }

            if (currentAction == CoreSiteActions.Fields)
            {
                if (hasAnimals || importsManureCompost)
                {
                    mnvm.PreviousAction = CoreSiteActions.ManureNutrientAnalysis;
                }
                else
                {
                    mnvm.PreviousAction = CoreSiteActions.Farm;
                    mnvm.PreviousController = AppControllers.Farm;
                }
                mnvm.NextAction = CoreSiteActions.SoilTest;
                mnvm.NextController = AppControllers.Soil;
            }

            if (currentAction == CoreSiteActions.SoilTest)
            {
                mnvm.UseJSInterceptMethod = true;
                mnvm.PreviousAction = CoreSiteActions.Fields;
                mnvm.PreviousController = AppControllers.Fields;
                mnvm.NextAction = CoreSiteActions.Calculate;
                mnvm.NextController = AppControllers.Nutrients;
            }

            if (currentAction == CoreSiteActions.Calculate)
            {
                ProcessCalculateNavigation(mnvm);
            }

            mnvm.ViewPreviousUrl = Url.Action(mnvm.ViewPreviousAction,
                mnvm.ViewPreviousController,
                mnvm.PreviousParameters);

            mnvm.ViewNextUrl = Url.Action(mnvm.ViewNextAction,
                mnvm.ViewNextController,
                mnvm.NextParameters);

            return Task.FromResult(mnvm);
        }

        private NextPreviousNavigationViewModel ProcessCalculateNavigation(NextPreviousNavigationViewModel mnvm)
        {
            var result = mnvm;
            var fields = _ud.GetFields();
            var currentField = HttpContext.Request.Query["nme"];

            if (fields.Count == 0)
            {
                result.PreviousAction = CoreSiteActions.SoilTest;
                result.PreviousController = AppControllers.Soil;
                result.NextAction = CoreSiteActions.Report;
                result.NextController = AppControllers.Report;
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
                result.PreviousAction = CoreSiteActions.SoilTest;
                result.PreviousController = AppControllers.Soil;
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
                result.NextAction = CoreSiteActions.Report;
                result.NextController = AppControllers.Report;
            }

            return result;
        }
    }
}