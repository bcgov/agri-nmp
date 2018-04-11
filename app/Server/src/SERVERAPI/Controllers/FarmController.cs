using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public Models.Impl.StaticData _sd { get; set; }

        public FarmController(IHostingEnvironment env, UserData ud, Models.Impl.StaticData sd)
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

            fvm.regOptions = _sd.GetRegionsDll().ToList();
            fvm.selRegOption = null;

            fvm.year = farmData.year;
            fvm.currYear = farmData.year;
            fvm.farmName = farmData.farmName;

            fvm.selRegOption = farmData.farmRegion;

            return View(fvm);
        }
        [HttpPost]
        public IActionResult Farm(FarmViewModel fvm)
        {
            fvm.regOptions = _sd.GetRegionsDll().ToList();

            if (ModelState.IsValid)
            {
                var farmData = _ud.FarmDetails();

                fvm.regOptions = _sd.GetRegionsDll().ToList();

                farmData.year = fvm.year;
                farmData.farmName = fvm.farmName;
                farmData.farmRegion = fvm.selRegOption;

                _ud.UpdateFarmDetails(farmData);
                HttpContext.Session.SetObject("Farm", _ud.FarmDetails().farmName + " " + _ud.FarmDetails().year);

                fvm.currYear = fvm.year;
                ModelState.Remove("userData");

                return RedirectToAction("Fields", "Fields");
            }
            else
            {
                return View(fvm);
            }
        }
    }
}
