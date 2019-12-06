using System;
using System.Linq;
using Agri.Interfaces;
using Agri.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using SERVERAPI.Models.Impl;
using Agri.Models;
using Microsoft.Extensions.Logging;
using Agri.Models.Settings;
using Microsoft.Extensions.Options;
using SERVERAPI.Filters;

namespace SERVERAPI.Controllers
{
    [SessionTimeout]
    public class FarmController : BaseController
    {
        private ILogger<FarmController> _logger;
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public IAgriConfigurationRepository _sd { get; set; }
        private IOptions<AppSettings> _appSettings;

        public FarmController(ILogger<FarmController> logger,
            IHostingEnvironment env,
            UserData ud,
            IAgriConfigurationRepository sd,
            IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _env = env;
            _ud = ud;
            _sd = sd;
            _appSettings = appSettings;
        }

        [HttpGet]
        public IActionResult Farm()
        {
            var farmData = _ud.FarmDetails();
            FarmViewModel fvm = new FarmViewModel();
            fvm.IsLegacyNMPReleaseVersion = !_ud.FarmData().NMPReleaseVersion.HasValue || _ud.FarmData().NMPReleaseVersion.Value < _appSettings.Value.NMPReleaseVersion;

            if (fvm.IsLegacyNMPReleaseVersion)
            {
                fvm.LegacyNMPMessage = _sd.GetUserPrompt("FarmDataBackwardsCompatibility");
            }
            fvm.showSubRegion = false;
            fvm.multipleSubRegion = false;

            fvm.regOptions = _sd.GetRegionsDll().ToList();
            fvm.selRegOption = null;

            fvm.Year = farmData.Year;
            fvm.CurrentYear = farmData.Year;
            fvm.FarmName = farmData.FarmName;

            fvm.selRegOption = farmData.FarmRegion;
            fvm.selSubRegOption = farmData.FarmSubRegion;

            if (fvm.selRegOption.HasValue)
            {
                if (farmData.HasAnimals)
                {
                    fvm = SetSubRegions(fvm);
                }

                if (fvm.IsLegacyNMPReleaseVersion && fvm.selSubRegOption.HasValue && farmData.HasAnimals)
                {
                    _ud.UpdateFarmDetailsSubRegion(fvm.selSubRegOption.Value);
                }
                else
                {
                    _ud.SetLegacyDataToUnsaved();
                }
            }

            fvm.HasAnimals = farmData.HasAnimals;
            fvm.HasDairyCows = farmData.HasDairyCows;
            fvm.HasBeefCows = farmData.HasBeefCows;
            fvm.HasPoultry = farmData.HasPoultry;
            fvm.HasMixedLiveStock = farmData.HasMixedLiveStock;

            if (fvm.HasAnimals)
            {
                fvm.ShowAnimals = true;
            }
            else
            {
                fvm.ShowAnimals = false;
            }

            return View(fvm);
        }

        private FarmViewModel SetSubRegions(FarmViewModel fvm)
        {
            return SetSubRegionsForRegion(fvm) as FarmViewModel;
        }

        private FarmIncompleteViewModel SetSubRegions(FarmIncompleteViewModel fvm)
        {
            return SetSubRegionsForRegion(fvm) as FarmIncompleteViewModel;
        }

        private FarmViewModelBase SetSubRegionsForRegion(FarmViewModelBase fvm)
        {
            if (fvm.selRegOption.HasValue)
            {
                fvm.subRegionOptions = _sd.GetSubRegionsDll(fvm.selRegOption);
                if (fvm.subRegionOptions.Count == 1)
                {
                    fvm.selSubRegOption = fvm.selSubRegOption ?? fvm.subRegionOptions[0].Id;
                    fvm.showSubRegion = false;
                    fvm.multipleSubRegion = false;
                }
                else if (fvm.subRegionOptions.Count > 1)
                {
                    fvm.showSubRegion = true;
                    fvm.multipleSubRegion = true;
                    if (fvm.selSubRegOption == null)
                    {
                        ModelState.AddModelError("", "Select a sub region");
                    }
                }
            }

            return fvm;
        }

        private void UpdateStorageSystemsForSubRegion(int subRegionId)
        {
            var storageSystems = _ud.GetStorageSystems();
            decimal conversionForLiquid = 0.024542388m;
            decimal conversionForSolid = 0.000102408m;

            if (storageSystems.Count() > 0)
            {
                foreach (var s in storageSystems)
                {
                    if (s.AnnualPrecipitation != null)
                    {
                        SubRegion subregion = _sd.GetSubRegion(subRegionId);
                        s.AnnualPrecipitation = subregion.AnnualPrecipitation;
                    }

                    _ud.UpdateManureStorageSystem(s);
                }
            }
        }

        [HttpPost]
        public IActionResult Farm(FarmViewModel fvm)
        {
            fvm.regOptions = _sd.GetRegionsDll().ToList();
            fvm.HasSelectedFarmType = true;

            if (fvm.buttonPressed == "GetsAnimalsChange")
            {
                ModelState.Clear();
                fvm.buttonPressed = "";

                if (fvm.HasAnimals)
                {
                    fvm.ShowAnimals = true;
                    if (fvm.selRegOption.HasValue)
                    {
                        fvm.buttonPressed = "RegionChange";
                        fvm = SetSubRegions(fvm);
                    }
                }
                else
                {
                    fvm.ShowAnimals = false;
                }

                var farmData = _ud.FarmDetails();
                farmData.HasSelectedFarmType = fvm.HasSelectedFarmType;
                farmData.HasAnimals = fvm.HasAnimals;
                farmData.HasDairyCows = fvm.HasDairyCows;
                farmData.HasBeefCows = fvm.HasBeefCows;
                farmData.HasPoultry = fvm.HasPoultry;
                farmData.HasMixedLiveStock = fvm.HasMixedLiveStock;
                _ud.UpdateFarmDetails(farmData);

                return View(fvm);
            }

            if (fvm.buttonPressed == "RegionChange")
            {
                ModelState.Clear();
                fvm.buttonPressed = "";
                if (fvm.ShowAnimals)
                {
                    fvm = SetSubRegions(fvm);
                }

                var farmData = _ud.FarmDetails();
                farmData.FarmRegion = fvm.selRegOption;
                _ud.UpdateFarmDetails(farmData);

                if (fvm.multipleSubRegion && fvm.ShowAnimals)
                {
                    return View(fvm);
                }

                if (fvm.ShowAnimals)
                {
                    fvm.buttonPressed = "SubRegionChange";
                }

                return View(fvm);
            }

            if (fvm.buttonPressed == "SubRegionChange")
            {
                ModelState.Clear();
                fvm.buttonPressed = "";

                if (fvm.multipleSubRegion)
                {
                    fvm = SetSubRegions(fvm);
                }

                UpdateStorageSystemsForSubRegion(fvm.selSubRegOption.Value);

                var farmData = _ud.FarmDetails();
                farmData.FarmSubRegion = fvm.selSubRegOption;
                _ud.UpdateFarmDetails(farmData);

                return View(fvm);
            }

            if (ModelState.IsValid)
            {
                var farmData = _ud.FarmDetails();

                fvm.regOptions = _sd.GetRegionsDll().ToList();

                farmData.Year = fvm.Year;
                farmData.FarmName = fvm.FarmName;
                farmData.FarmRegion = fvm.selRegOption;
                if (fvm.ShowAnimals)
                {
                    fvm.subRegionOptions = _sd.GetSubRegionsDll(fvm.selRegOption);
                    if (fvm.subRegionOptions.Count == 1)
                    {
                        fvm.selSubRegOption = fvm.subRegionOptions[0].Id;
                    }
                    farmData.FarmSubRegion = fvm.selSubRegOption;
                }

                farmData.HasSelectedFarmType = fvm.HasSelectedFarmType;
                farmData.HasAnimals = fvm.HasAnimals;
                farmData.HasDairyCows = fvm.HasDairyCows;
                farmData.HasBeefCows = fvm.HasBeefCows;
                farmData.HasPoultry = fvm.HasPoultry;
                farmData.HasMixedLiveStock = fvm.HasMixedLiveStock;

                _ud.UpdateFarmDetails(farmData);
                HttpContext.Session.SetObject("Farm", _ud.FarmDetails().FarmName + " " + _ud.FarmDetails().Year);

                fvm.CurrentYear = fvm.Year;
                ModelState.Remove("userData");

                //Navigate to next item past Farm
                var journey = _ud.FarmDetails().UserJourney;
                var initialNavigation = _sd.GetJourney((int)journey)
                    .MainMenus
                    .Single(m => m.SortNumber == 2);

                return RedirectToAction(initialNavigation.Action, initialNavigation.Controller);
            }
            else
            {
                if (fvm.ShowAnimals)
                {
                    fvm.subRegionOptions = _sd.GetSubRegionsDll(fvm.selRegOption);
                }
                return View(fvm);
            }
        }

        [HttpGet]
        public object CheckCompleted()
        {
            var regionId = _ud.FarmDetails().FarmRegion;
            var regionsIncomplete = !regionId.HasValue;

            if (regionId.HasValue)
            {
                regionsIncomplete = _sd.GetRegion(regionId.Value) == null;
            }

            if (!regionsIncomplete)
            {
                var subRegionId = _ud.FarmDetails().FarmSubRegion;
                if (subRegionId.HasValue)
                {
                    regionsIncomplete = _sd.GetSubRegion(subRegionId.Value) == null;
                }
                else
                {
                    regionsIncomplete = true;
                }
            }

            var result = new { incomplete = regionsIncomplete.ToString() };
            return result;
        }

        [HttpGet]
        public IActionResult FarmIncomplete(string target)
        {
            var farmData = _ud.FarmDetails();

            var fvm = new FarmIncompleteViewModel
            {
                Message = _sd.GetUserPrompt("RegionRequiredWarning"),
                Target = target
            };

            fvm.selRegOption = farmData.FarmRegion;
            if (farmData.HasAnimals)
            {
                fvm.selSubRegOption = farmData.FarmSubRegion;
                fvm = SetSubRegions(fvm);
                fvm.showAnimals = true;
            }
            else
            {
                fvm.showAnimals = false;
            }
            fvm.regOptions = _sd.GetRegionsDll().ToList();

            return View(fvm);
        }

        [HttpPost]
        public IActionResult FarmIncomplete(FarmIncompleteViewModel fvm)
        {
            fvm.regOptions = _sd.GetRegionsDll().ToList();

            if (fvm.buttonPressed == "RegionChange")
            {
                ModelState.Clear();
                fvm.buttonPressed = "";
                if (fvm.showAnimals)
                {
                    fvm = SetSubRegions(fvm);
                }

                var farmData = _ud.FarmDetails();
                farmData.FarmRegion = fvm.selRegOption;
                _ud.UpdateFarmDetails(farmData);

                if (fvm.multipleSubRegion && fvm.showAnimals)
                {
                    return View(fvm);
                }

                if (fvm.showAnimals)
                {
                    fvm.buttonPressed = "SubRegionChange";
                }
            }

            if (fvm.buttonPressed == "SubRegionChange")
            {
                ModelState.Clear();
                fvm.buttonPressed = "";

                fvm = SetSubRegions(fvm);

                UpdateStorageSystemsForSubRegion(fvm.selSubRegOption.Value);

                var farmData = _ud.FarmDetails();
                farmData.FarmSubRegion = fvm.selSubRegOption;
                _ud.UpdateFarmDetails(farmData);
                return View(fvm);
            }

            if (ModelState.IsValid)
            {
                return Json(new { success = true, url = fvm.Target });
            }

            if (fvm.showAnimals)
            {
                fvm.subRegionOptions = _sd.GetSubRegionsDll(fvm.selRegOption);
            }

            return View(fvm);
        }
    }
}