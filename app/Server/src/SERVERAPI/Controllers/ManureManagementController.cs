using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models;
using SERVERAPI.Models.Impl;
using SERVERAPI.Utility;
using SERVERAPI.ViewModels;
using static SERVERAPI.Models.StaticData;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SERVERAPI.Controllers
{
    public class ManureManagementController : Controller
    {
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public Models.Impl.StaticData _sd { get; set; }
        public IViewRenderService _viewRenderService { get; set; }
        public AppSettings _settings;

        public ManureManagementController(IHostingEnvironment env, IViewRenderService viewRenderService, UserData ud,
            Models.Impl.StaticData sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
            _viewRenderService = viewRenderService;
        }

        [HttpGet]
        public IActionResult ManureGeneratedObtained()
        {
            return View();
        }

        public IActionResult ManureGeneratedObtainedDetail(int? id)
        {
            ManureGeneratedObtainedDetailViewModel mgovm = new ManureGeneratedObtainedDetailViewModel();
            // Animal a= _ud.

            if (id != null)
            {
            }
            else
            {
                animalTypeDetailsSetup(ref mgovm);
                // mgovm.animalTypeOptions = new List<Models.StaticData.SelectListItem>();
                // mgovm.selAnimalTypeOption = _sd.get
                
            }

            //mm.selAnimalTypeOption = mgovm.;
            //mm.selSubtypeOption = "";
            //mm.selManureTypeOption = "";
            return PartialView("ManureGeneratedObtainedDetail", mgovm);
        }

        [HttpPost]
        public IActionResult ManureGeneratedObtainedDetail(ManureGeneratedObtainedDetailViewModel mgovm)
        {
            animalTypeDetailsSetup(ref mgovm);
            //try
            //{
            //    if (mgovm.buttonPressed == "TypeChange")
            //    {
            //        AnimalSubType crpTyp = _sd.GetAnimalSubTypes(Convert.ToInt32(mgovm.selAnimalTypeOption));
            //    }
            //    return View(mgovm);
            //}

            return View(mgovm);

        }

        private void animalTypeDetailsSetup(ref ManureGeneratedObtainedDetailViewModel mgovm)
        {
            mgovm.animalTypeOptions = new List<Models.StaticData.SelectListItem>();
            mgovm.animalTypeOptions = _sd.GetAnimalTypesDll().ToList();

            mgovm.subTypeOptions = new List<Models.StaticData.SelectListItem>();
            mgovm.manureMaterialTypeOptions = new List<Models.StaticData.SelectListItem>();
            mgovm.manureMaterialTypeOptions = _sd.GetManureMaterialTypesDll().ToList();

            if (!string.IsNullOrEmpty(mgovm.selAnimalTypeOption) &&
                mgovm.selAnimalTypeOption != "select animal")
            {
                mgovm.subTypeOptions = _sd.GetSubtypesDll(Convert.ToInt32(mgovm.selAnimalTypeOption)).ToList();
            }


            return;
        }
    }
}
