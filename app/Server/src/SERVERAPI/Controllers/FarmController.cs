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
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Controllers
{
    //[RedirectingAction]
    public class FarmController : Controller
    {
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public IAgriConfigurationRepository _sd { get; set; }

        public FarmController(IHostingEnvironment env, UserData ud, IAgriConfigurationRepository sd)
        {
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
                fvm.showSubRegion = true;
                fvm.subRegionOptions = _sd.GetSubRegionsDll(fvm.selRegOption);
                if (fvm.subRegionOptions.Count == 1)
                {
                    fvm.selSubRegOption = fvm.subRegionOptions[0].Id;
                }
                else if (fvm.subRegionOptions.Count > 1)
                {
                    if (fvm.selSubRegOption == null)
                    {
                        ModelState.AddModelError("", "Select a sub region");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var farmData = _ud.FarmDetails();

                fvm.regOptions = _sd.GetRegionsDll().ToList();

                farmData.year = fvm.year;
                farmData.farmName = fvm.farmName;
                farmData.farmRegion = fvm.selRegOption;
                farmData.farmSubRegion = fvm.selSubRegOption;

                _ud.UpdateFarmDetails(farmData);
                HttpContext.Session.SetObject("Farm", _ud.FarmDetails().farmName + " " + _ud.FarmDetails().year);

                fvm.currYear = fvm.year;
                ModelState.Remove("userData");

                return RedirectToAction("ManureGeneratedObtained", "ManureManagement");
            }
            else
            {
                return View(fvm);
            }
        }
    }
}
