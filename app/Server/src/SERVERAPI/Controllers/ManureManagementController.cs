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

        public IActionResult ManureGeneratedObtainedDetail(int? id, string target)
        {
            CalculateAnimalRequirement calculateAnimalRequirement = new CalculateAnimalRequirement(_ud, _sd);
            ManureGeneratedObtainedDetailViewModel mgovm = new ManureGeneratedObtainedDetailViewModel();
            mgovm.title = id == null ? "Add" : "Edit";
            mgovm.stdWashWater = true;
            mgovm.stdMilkProduction = true;
            mgovm.placehldr = _sd.GetUserPrompt("averageanimalnumberplaceholder");

            if (id != null)
            {
                GeneratedManure gm = _ud.GetGeneratedManure(id.Value);
                mgovm.id = id;
                mgovm.selSubTypeOption = gm.animalSubTypeId.ToString();
                mgovm.averageAnimalNumber = gm.averageAnimalNumber.ToString();
                mgovm.selManureMaterialTypeOption = gm.manureType.ToString();

                AnimalSubType animalSubType = _sd.GetAnimalSubType(Convert.ToInt32(gm.animalSubTypeId.ToString()));
                mgovm.selAnimalTypeOption = animalSubType.animalId.ToString();

                
                if (mgovm.showWashWater)
                {
                    mgovm.washWater = calculateAnimalRequirement
                        .GetWashWaterBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString();
                    mgovm.stdWashWater = true;
                }
                if (mgovm.showMilkProduction)
                {
                    mgovm.milkProduction = calculateAnimalRequirement
                        .GetDefaultMilkProductionBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString();
                    mgovm.stdMilkProduction = true;
                }
                animalTypeDetailsSetup(ref mgovm);
            }
            else
            {
                animalTypeDetailsSetup(ref mgovm);
            }

            return PartialView("ManureGeneratedObtainedDetail", mgovm);
        }

        [HttpPost]
        public IActionResult ManureGeneratedObtainedDetail(ManureGeneratedObtainedDetailViewModel mgovm)
        {
            CalculateAnimalRequirement calculateAnimalRequirement = new CalculateAnimalRequirement(_ud, _sd);

            mgovm.placehldr = _sd.GetUserPrompt("averageanimalnumberplaceholder");
            animalTypeDetailsSetup(ref mgovm);
            try
            {
                if (mgovm.buttonPressed == "SubTypeChange")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";
                    mgovm.washWater = "";
                    mgovm.milkProduction = "";

                    if (mgovm.selAnimalTypeOption != "" &&
                        mgovm.selAnimalTypeOption != "0" &&
                        mgovm.selAnimalTypeOption != "select animal")
                    {
                        if (mgovm.showWashWater)
                        {
                            mgovm.washWater = calculateAnimalRequirement
                                .GetWashWaterBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString();
                            mgovm.stdWashWater = true;
                        }
                        if (mgovm.showMilkProduction)
                        {
                            mgovm.milkProduction = calculateAnimalRequirement
                                .GetDefaultMilkProductionBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString();
                            mgovm.stdMilkProduction = true;
                        }
                    }

                    return View(mgovm);
                }

                if (mgovm.buttonPressed == "ManureMaterialTypeChange")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";

                    return View(mgovm);
                }

                if (mgovm.buttonPressed == "AnimalTypeChange")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";
                    mgovm.washWater = "";
                    mgovm.milkProduction = "";
                    mgovm.showWashWater = false;
                    mgovm.showMilkProduction = false;
                    mgovm.averageAnimalNumber = "";
                    mgovm.manureMaterialTypeOptions = new List<SelectListItem>();

                    return View(mgovm);
                }

                if (mgovm.buttonPressed == "ResetWashWater")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";
                    mgovm.stdWashWater = true;

                    mgovm.washWater = calculateAnimalRequirement.GetWashWaterBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString();
                    return View(mgovm);
                }

                if (mgovm.buttonPressed == "ResetMilkProduction")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";

                    mgovm.stdMilkProduction = true;
                    mgovm.milkProduction = calculateAnimalRequirement.GetDefaultMilkProductionBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString();
                    return View(mgovm);
                }

                if (ModelState.IsValid)
                {
                    if (mgovm.btnText == "Save")
                    {
                        ModelState.Clear();
                        if (mgovm.washWater == null)
                            calculateAnimalRequirement.washWater = null;
                        else
                            calculateAnimalRequirement.washWater = Convert.ToDecimal(mgovm.washWater);

                        if (mgovm.milkProduction == null)
                            calculateAnimalRequirement.milkProduction = null;
                        else
                            calculateAnimalRequirement.milkProduction = Convert.ToDecimal(mgovm.milkProduction);


                        if (mgovm.washWater != calculateAnimalRequirement.GetWashWaterBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString())
                        {
                            mgovm.stdWashWater = false;
                        }
                        if (mgovm.milkProduction != calculateAnimalRequirement.GetDefaultMilkProductionBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString())
                        {
                            mgovm.stdMilkProduction = false;
                        }

                        List<GeneratedManure> generatedManures = _ud.GetGeneratedManures();
                        if (mgovm.id == null)
                        {
                            GeneratedManure gm = new GeneratedManure();
                            gm.animalSubTypeId = Convert.ToInt32(mgovm.selSubTypeOption);
                            gm.subTypeName = _sd.GetAnimalSubTypeName(Convert.ToInt32(mgovm.selSubTypeOption));
                            gm.averageAnimalNumber = Convert.ToInt32(mgovm.averageAnimalNumber);
                            gm.manureType = Convert.ToInt32(mgovm.selManureMaterialTypeOption);
                            gm.manureTypeName = _sd.GetManureTypeName(Convert.ToInt32(mgovm.selManureMaterialTypeOption));
                            if (mgovm.washWater != null)
                            {
                                gm.washWaterGallons = Convert.ToDecimal(mgovm.washWater) * Convert.ToInt32(mgovm.averageAnimalNumber) * 365;
                                gm.washWater = Convert.ToDecimal(mgovm.washWater.ToString());
                            }
                            else
                            {
                                gm.washWaterGallons = 0;
                                gm.washWater = 0;
                            }

                            if (mgovm.milkProduction != null)
                            {
                                gm.milkProduction = Convert.ToDecimal(mgovm.milkProduction.ToString());
                            }
                            else
                            {
                                gm.milkProduction = 0;
                            }

                            AnimalSubType animalSubType = _sd.GetAnimalSubType(Convert.ToInt32(mgovm.selSubTypeOption));

                            // manure material type is liquid
                            if (Convert.ToInt32(mgovm.selManureMaterialTypeOption) == 1)
                            {
                                if (animalSubType.liquidPerGalPerAnimalPerDay.HasValue)
                                    gm.annualAmount = (Convert.ToInt32(mgovm.averageAnimalNumber) * animalSubType.liquidPerGalPerAnimalPerDay * 365).ToString() + " gallons";
                            }
                            // manure material type is solid
                            else if (Convert.ToInt32(mgovm.selManureMaterialTypeOption) == 2)
                            {
                                if (animalSubType.solidPerPoundPerAnimalPerDay.HasValue)
                                    gm.annualAmount = (Convert.ToInt32(mgovm.averageAnimalNumber) * animalSubType.solidPerPoundPerAnimalPerDay * 365).ToString() + " tons";
                            }

                            _ud.AddGeneratedManure(gm);
                        }
                        else
                        {
                            int thisSubType = 0;
                            if (mgovm.selSubTypeOption != "select subtype")
                                thisSubType = Convert.ToInt32(mgovm.selSubTypeOption);

                            int thisManureMaterialType = 0;
                            if (mgovm.selManureMaterialTypeOption != "select type")
                                thisManureMaterialType = Convert.ToInt32(mgovm.selManureMaterialTypeOption);

                            GeneratedManure gm = new GeneratedManure();
                            gm.id = mgovm.id;
                            gm.animalSubTypeId = thisSubType;
                            gm.averageAnimalNumber = Convert.ToInt32(mgovm.averageAnimalNumber);
                            gm.manureType = Convert.ToInt32(thisManureMaterialType);
                            _ud.UpdateGeneratedManure(gm);
                        }

                        string url = Url.Action("RefreshManureManagemetList", "ManureManagement");
                        return Json(new { success = true, url = url, target = mgovm.target });
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error -" + ex.Message);
            }

            return PartialView(mgovm);
        }

        private void animalTypeDetailsSetup(ref ManureGeneratedObtainedDetailViewModel mgovm)
        {
            mgovm.showWashWater = false;
            mgovm.showMilkProduction = false;

            mgovm.animalTypeOptions = new List<Models.StaticData.SelectListItem>();
            mgovm.animalTypeOptions = _sd.GetAnimalTypesDll().ToList();

            mgovm.subTypeOptions = new List<Models.StaticData.SelectListItem>();
            mgovm.manureMaterialTypeOptions = new List<Models.StaticData.SelectListItem>();
            mgovm.manureMaterialTypeOptions = _sd.GetManureMaterialTypesDll().ToList();

            if (!string.IsNullOrEmpty(mgovm.selAnimalTypeOption) &&
                mgovm.selAnimalTypeOption != "select animal")
            {
                mgovm.subTypeOptions = _sd.GetSubtypesDll(Convert.ToInt32(mgovm.selAnimalTypeOption)).ToList();
                mgovm.subTypeOptions.Insert(0, new SelectListItem() { Id = 0, Value = "select subtype" });

                if (!string.IsNullOrEmpty(mgovm.selSubTypeOption) &&
                    mgovm.selSubTypeOption != "select subtype")
                {
                    Animal animalType = _sd.GetAnimal(Convert.ToInt32(mgovm.selAnimalTypeOption));
                    if (_sd.DoesAnimalUseWashWater(Convert.ToInt32(mgovm.selSubTypeOption)))
                    {
                        mgovm.showWashWater = true;
                        mgovm.showMilkProduction = true;
                    }
                }
            }
            return;
        }

        public IActionResult RefreshManureManagemetList()
        {
            return ViewComponent("ManureGeneratedObtained");
        }

        [HttpGet]
        public ActionResult ManureGeneratedObtainedDelete(int id, string target)
        {
            ManureGeneratedObtainedDeleteViewModel dvm = new ManureGeneratedObtainedDeleteViewModel();
            dvm.id = id;

            GeneratedManure gm = _ud.GetGeneratedManure(id);
            dvm.subTypeName = _sd.GetAnimalSubType(Convert.ToInt32(gm.animalSubTypeId)).name;

            dvm.title = "Delete";

            return PartialView("ManureGeneratedObtainedDelete", dvm);
        }

        [HttpPost]
        public ActionResult ManureGeneratedObtainedDelete(ManureGeneratedObtainedDeleteViewModel dvm)
        {
            if (ModelState.IsValid)
            {
                _ud.DeleteGeneratedManure(dvm.id);

                string url = Url.Action("RefreshManureManagemetList", "ManureManagement");
                return Json(new { success = true, url = url, target = dvm.target });
            }
            return PartialView("ManureGeneratedObtainedDelete", dvm);
        }
    }
}
