﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Models;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;

namespace SERVERAPI.ViewComponents
{
    public class NextPreviousNavigationViewComponent : ViewComponent
    {
        private UserData _ud;

        public NextPreviousNavigationViewComponent(UserData ud)
        {
            _ud = ud;
        }
        public async Task<IViewComponentResult> InvokeAsync(CoreSiteActions currentAction)
        {
            return View(await GetManureNavigation(currentAction));
        }

        private Task<NextPreviousNavigationViewModel> GetManureNavigation(CoreSiteActions currentAction)
        {
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

            if (currentAction == CoreSiteActions.ManureGeneratedObtained )
            {
                mnvm.PreviousAction = CoreSiteActions.Farm;
                mnvm.PreviousController = AppControllers.Farm;
                if (importsManureCompost)
                {
                    mnvm.NextAction = CoreSiteActions.ManureImported;
                }
                else if(hasAnimals)
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
                mnvm.PreviousAction= CoreSiteActions.ManureImported;
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
                mnvm.PreviousAction = CoreSiteActions.SoilTest;
                mnvm.PreviousController = AppControllers.Soil;
                mnvm.NextAction = CoreSiteActions.Report;
                mnvm.NextController = AppControllers.Report;
            }

            return Task.FromResult(mnvm);
        }
    }
}
