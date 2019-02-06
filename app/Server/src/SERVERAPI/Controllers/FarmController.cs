using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.Models;
using SERVERAPI.ViewModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SERVERAPI.Models.Impl;
using Agri.Models;
using Microsoft.Extensions.Logging;

namespace SERVERAPI.Controllers
{
    //[RedirectingAction]
    public class FarmController : BaseController
    {
        private ILogger<FarmController> _logger;
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public IAgriConfigurationRepository _sd { get; set; }

        public FarmController(ILogger<FarmController> logger, 
            IHostingEnvironment env, 
            UserData ud, 
            IAgriConfigurationRepository sd)
        {
            _logger = logger;
            _env = env;
            _ud = ud;
            _sd = sd;
        }

        [HttpGet]
        public IActionResult Farm()
        {
            var farmData = _ud.FarmDetails();

            FarmViewModel fvm = new FarmViewModel();
            fvm.showSubRegion = false;
            fvm.multipleSubRegion = false;

            fvm.regOptions = _sd.GetRegionsDll().ToList();
            fvm.selRegOption = null;

            fvm.year = farmData.year;
            fvm.currYear = farmData.year;
            fvm.farmName = farmData.farmName;

            fvm.selRegOption = farmData.farmRegion;
            fvm.selSubRegOption = farmData.farmSubRegion;
            fvm.subRegionOptions = _sd.GetSubRegionsDll(fvm.selRegOption);

            fvm.HasAnimals = farmData.HasAnimals;
            fvm.ImportsManureCompost = farmData.ImportsManureCompost;
            fvm.UsesFertilizer = farmData.UsesFertilizer;

            if (fvm.subRegionOptions.Count > 1)
            {
                fvm.showSubRegion = true;
                fvm.multipleSubRegion = true;
                fvm.selSubRegOption = farmData.farmSubRegion;
                fvm.selRegOption= farmData.farmRegion;
            }
            else
            {
                fvm.showSubRegion = false;
                fvm.multipleSubRegion = false;
                fvm.selRegOption=farmData.farmRegion;
                fvm.subRegionOptions = _sd.GetSubRegionsDll(fvm.selRegOption);
            }

            return View(fvm);
        }

        [HttpPost]
        public IActionResult Farm(FarmViewModel fvm)
        {
            fvm.regOptions = _sd.GetRegionsDll().ToList();

            if (fvm.buttonPressed == "RegionChange")
            {
                ModelState.Clear();
                fvm.buttonPressed = "";
                fvm.showSubRegion = true;
                fvm.subRegionOptions = _sd.GetSubRegionsDll(fvm.selRegOption);
                if (fvm.subRegionOptions.Count == 1)
                {
                    fvm.selSubRegOption = fvm.subRegionOptions[0].Id;
                    fvm.showSubRegion = false;
                    return View(fvm);
                }
                else if (fvm.subRegionOptions.Count > 1)
                {
                    fvm.showSubRegion = true;
                    if (fvm.selSubRegOption == null)
                    {
                        ModelState.AddModelError("", "Select a sub region");
                    }
                }
                return View(fvm);
            }

            if (fvm.buttonPressed == "SubRegionChange")
            {
                ModelState.Clear();
                fvm.buttonPressed = "";
                fvm.showSubRegion = true;
                fvm.subRegionOptions = _sd.GetSubRegionsDll(fvm.selRegOption);
                if (fvm.subRegionOptions.Count == 1)
                {
                    fvm.selSubRegOption = fvm.subRegionOptions[0].Id;
                    fvm.showSubRegion = false;
                }
                else if (fvm.subRegionOptions.Count > 1)
                {
                    fvm.showSubRegion = true;
                    if (fvm.selSubRegOption == null)
                    {
                        ModelState.AddModelError("", "Select a sub region");
                    }
                }

                var storageSystems = _ud.GetStorageSystems();
                decimal conversionForLiquid = 0.024542388m;
                decimal conversionForSolid = 0.000102408m;

                if (storageSystems.Count() > 0)
                {
                    foreach (var s in storageSystems)
                    {
                        if (s.AnnualPrecipitation != null)
                        {
                            SubRegion subregion = _sd.GetSubRegion(fvm.selSubRegOption);
                            s.AnnualPrecipitation = subregion.AnnualPrecipitation;
                            if (s.ManureMaterialType == ManureMaterialType.Liquid)
                            {
                                s.AnnualTotalPrecipitation = Convert.ToDecimal(s.RunoffAreaSquareFeet) +
                                                             Convert.ToDecimal(s.TotalAreaOfUncoveredLiquidStorage) * Convert.ToDecimal(s.AnnualPrecipitation) * conversionForLiquid;
                            }
                            else if (s.ManureMaterialType == ManureMaterialType.Solid)
                            {
                                s.AnnualTotalPrecipitation = Convert.ToDecimal(s.RunoffAreaSquareFeet) +
                                                             Convert.ToDecimal(s.TotalAreaOfUncoveredLiquidStorage) * Convert.ToDecimal(s.AnnualPrecipitation) * conversionForSolid;
                            }
                        }
                        _ud.UpdateManureStorageSystem(s);
                    }

                    var farmData = _ud.FarmDetails();
                    farmData.farmSubRegion = fvm.selSubRegOption;
                    _ud.UpdateFarmDetails(farmData);
                }

                return View(fvm);
            }

            if (ModelState.IsValid)
            {
                var farmData = _ud.FarmDetails();

                fvm.regOptions = _sd.GetRegionsDll().ToList();

                farmData.year = fvm.year;
                farmData.farmName = fvm.farmName;
                farmData.farmRegion = fvm.selRegOption;
                fvm.subRegionOptions = _sd.GetSubRegionsDll(fvm.selRegOption);
                if (fvm.subRegionOptions.Count == 1)
                {
                    fvm.selSubRegOption = fvm.subRegionOptions[0].Id;
                }
                farmData.farmSubRegion = fvm.selSubRegOption;
                farmData.HasAnimals = fvm.HasAnimals;
                farmData.ImportsManureCompost = fvm.ImportsManureCompost;
                farmData.UsesFertilizer = fvm.UsesFertilizer;

                _ud.UpdateFarmDetails(farmData);
                HttpContext.Session.SetObject("Farm", _ud.FarmDetails().farmName + " " + _ud.FarmDetails().year);

                fvm.currYear = fvm.year;
                ModelState.Remove("userData");

                if (farmData.HasAnimals)
                {
                    return RedirectToAction("ManureGeneratedObtained", "ManureManagement");
                }
                else if (farmData.ImportsManureCompost)
                {
                    return RedirectToAction("ManureImported", "ManureManagement");
                }
                else
                {
                    return RedirectToAction("Fields", "Fields");
                }
            }
            else
            {
                fvm.subRegionOptions = _sd.GetSubRegionsDll(fvm.selRegOption);
                return View(fvm);
            }
        }
    }
}
