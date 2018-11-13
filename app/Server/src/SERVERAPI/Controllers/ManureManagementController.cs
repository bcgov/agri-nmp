using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MvcRendering = Microsoft.AspNetCore.Mvc.Rendering;
using SERVERAPI.Models;
using SERVERAPI.Models.Impl;
using SERVERAPI.Utility;
using SERVERAPI.ViewModels;
using static SERVERAPI.Models.StaticData;
using StaticData = SERVERAPI.Models.StaticData;

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
        private readonly IMapper _mapper;

        public ManureManagementController(IHostingEnvironment env, IViewRenderService viewRenderService, UserData ud,
            Models.Impl.StaticData sd,IMapper mapper)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
            _viewRenderService = viewRenderService;
            _mapper = mapper;
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
            //mgovm.btnText = id == null ? "Calculate" : "Return";
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
                mgovm.selManureMaterialTypeOption = gm.manureType;

                AnimalSubType animalSubType = _sd.GetAnimalSubType(Convert.ToInt32(gm.animalSubTypeId.ToString()));
                mgovm.selAnimalTypeOption = animalSubType.animalId.ToString();

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

                if (mgovm.showWashWater)
                {
                    mgovm.washWater = gm.washWater.ToString("#.##");
                }
                if (mgovm.showMilkProduction)
                {
                    mgovm.milkProduction = gm.milkProduction.ToString("#.##");
                }

                if (mgovm.washWater != calculateAnimalRequirement.GetWashWaterBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString())
                {
                    mgovm.stdWashWater = false;
                }
                if (mgovm.milkProduction != calculateAnimalRequirement.GetDefaultMilkProductionBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString())
                {
                    mgovm.stdMilkProduction = false;
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
                            Animal animal = _sd.GetAnimal(Convert.ToInt32(mgovm.selAnimalTypeOption));

                            GeneratedManure gm = new GeneratedManure();
                            gm.animalId = Convert.ToInt32(mgovm.selAnimalTypeOption);
                            gm.animalName = animal.name;
                            gm.animalSubTypeId = Convert.ToInt32(mgovm.selSubTypeOption);
                            gm.animalSubTypeName = _sd.GetAnimalSubTypeName(Convert.ToInt32(mgovm.selSubTypeOption));
                            gm.averageAnimalNumber = Convert.ToInt32(mgovm.averageAnimalNumber);
                            gm.manureType = mgovm.selManureMaterialTypeOption;
                            gm.manureTypeName = EnumHelper<ManureMaterialType>.GetDisplayValue(mgovm.selManureMaterialTypeOption);
                            if (mgovm.washWater != null)
                            {
                                gm.washWaterGallons = Math.Round(Convert.ToDecimal(mgovm.washWater) * Convert.ToInt32(mgovm.averageAnimalNumber) * 365);
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
                            if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Liquid)
                            {
                                if (animalSubType.liquidPerGalPerAnimalPerDay.HasValue)
                                    gm.annualAmount = (Math.Round(Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.liquidPerGalPerAnimalPerDay) * 365)) + " U.S. gallons";
                            }
                            // manure material type is solid
                            else if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Solid)
                            {
                                if (animalSubType.solidPerPoundPerAnimalPerDay.HasValue)
                                    gm.annualAmount = (Math.Round(((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.solidPerPoundPerAnimalPerDay) * 365) / 2000))) + " tons";
                            }

                            _ud.AddGeneratedManure(gm);
                        }
                        else
                        {
                            GeneratedManure gm = _ud.GetGeneratedManure(mgovm.id.Value);
                            int thisAnimalType = 0;
                            if (mgovm.selAnimalTypeOption != "select animal")
                                thisAnimalType = Convert.ToInt32(mgovm.selAnimalTypeOption);

                            int thisSubType = 0;
                            if (mgovm.selSubTypeOption != "select subtype")
                                thisSubType = Convert.ToInt32(mgovm.selSubTypeOption);

                            ManureMaterialType thisManureMaterialType = 0;
                            if (mgovm.selManureMaterialTypeOption != 0)
                                thisManureMaterialType =mgovm.selManureMaterialTypeOption;

                            Animal animal = _sd.GetAnimal(Convert.ToInt32(mgovm.selAnimalTypeOption));

                            gm.id = mgovm.id;
                            gm.animalId = thisAnimalType;
                            gm.animalName = animal.name;
                            gm.animalSubTypeId = thisSubType;
                            gm.animalSubTypeName = _sd.GetAnimalSubTypeName(thisSubType);
                            gm.averageAnimalNumber = Convert.ToInt32(mgovm.averageAnimalNumber);
                            gm.manureType = thisManureMaterialType;
                            gm.manureTypeName = EnumHelper<ManureMaterialType>.GetDisplayValue(mgovm.selManureMaterialTypeOption);
                            gm.milkProduction = Convert.ToDecimal(mgovm.milkProduction);

                            if (mgovm.washWater != null)
                            {
                                gm.washWaterGallons = Math.Round(Convert.ToDecimal(mgovm.washWater) * Convert.ToInt32(mgovm.averageAnimalNumber) * 365);
                                gm.washWater = Convert.ToDecimal(mgovm.washWater);
                            }
                            else
                            {
                                gm.washWaterGallons = 0;
                                gm.washWater = 0;
                            }

                            AnimalSubType animalSubType = _sd.GetAnimalSubType(Convert.ToInt32(mgovm.selSubTypeOption));

                            // manure material type is liquid
                            if (Convert.ToInt32(mgovm.selManureMaterialTypeOption) == 1)
                            {
                                if (animalSubType.liquidPerGalPerAnimalPerDay.HasValue)
                                    gm.annualAmount = (Math.Round(Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.liquidPerGalPerAnimalPerDay) * 365)) + " U.S. gallons";
                            }
                            // manure material type is solid
                            else if (Convert.ToInt32(mgovm.selManureMaterialTypeOption) == 2)
                            {
                                if (animalSubType.solidPerPoundPerAnimalPerDay.HasValue)
                                    gm.annualAmount = (Math.Round(((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.solidPerPoundPerAnimalPerDay) * 365 ) / 2000))) + " tons";
                            }

                            _ud.UpdateGeneratedManures(gm);
                        }
                        //mgovm.btnText = mgovm.id == null ? "Add to Field" : "Update Field";

                        string url = Url.Action("RefreshManureManagemetList", "ManureManagement");
                        return Json(new { success = true, url = url, target = mgovm.target });


                    }

                    //string url1="";
                    //if (mgovm.target == "#manuregeneratedobtained")
                    //{
                    //    url1 = Url.Action("RefreshManureManagemetList", "ManureManagement");
                    //}
                    //return Json(new { success = true, url = url1, target = mgovm.target });

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

            if (!string.IsNullOrEmpty(mgovm.selAnimalTypeOption) &&
                mgovm.selAnimalTypeOption != "select animal")
            {
                mgovm.subTypeOptions = _sd.GetSubtypesDll(Convert.ToInt32(mgovm.selAnimalTypeOption)).ToList();
                mgovm.subTypeOptions.Insert(0, new StaticData.SelectListItem() { Id = 0, Value = "select subtype" });

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
            dvm.warning = _sd.GetUserPrompt("deletewarningmanuregenerated");

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

        [HttpGet]
        public IActionResult ManureStorage()
        {
            return View();
        }

        public IActionResult ManureStorageDetail(int? id)
        {
            var msvm = new ManureStorageDetailViewModel();
            try
            {
                msvm.Title = !id.HasValue ? "Add" : "Edit";

                if (id.HasValue)
                {
                    msvm.DisableForEditMode = true;
                    var savedStorageSystem = _ud.GetStorageSystem(id.Value);
                    msvm.SystemId = savedStorageSystem.Id;
                    msvm.SystemName = savedStorageSystem.Name;
                    msvm.SelectedManureMaterialType = savedStorageSystem.ManureMaterialType;
                    msvm.SelectedMaterialsToInclude =
                        savedStorageSystem.MaterialsIncludedInSystem.Where(m => m.id.HasValue).Select(m => m.id.Value).ToList();
                    msvm.GeneratedManures = GetFilteredMaterialsListForCurrentView(msvm);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return PartialView("ManureStorageDetail", msvm);
        }



        [HttpPost]
        public IActionResult ManureStorageDetail(ManureStorageDetailViewModel msdvm)
        {
            try
            {
                var currentStorageSystems = _ud.GetStorageSystems();

                msdvm.GeneratedManures = GetFilteredMaterialsListForCurrentView(msdvm);

                if (msdvm.ButtonPressed == "ManureMaterialTypeChange")
                {
                    ModelState.Clear();
                    msdvm.ButtonPressed = "";
                    msdvm.ButtonText = "Save";

                    var selectedTypeMsg = msdvm.SelectedManureMaterialType == ManureMaterialType.Solid
                        ? "Solid"
                        : "Liquid";
                    var systemTypeCount = _ud.GetStorageSystems().Count(ss => ss.ManureMaterialType == msdvm.SelectedManureMaterialType);
                    var systemTypeCountMsg = systemTypeCount > 0 ? (systemTypeCount + 1).ToString() : string.Empty;
                    var placeHolder = string.Format(_sd.GetUserPrompt("storagesystemnameplaceholder"), selectedTypeMsg, systemTypeCountMsg);

                    msdvm.Placeholder = placeHolder;
                }

                if (msdvm.ButtonPressed == "SelectedMaterialsToIncludeChange")
                {
                    ModelState.Clear();
                    msdvm.ButtonPressed = "";
                    msdvm.ButtonText = "Save";
                }

                if (msdvm.ButtonText == "Save" || msdvm.SystemId.HasValue)
                {
                    if (msdvm.SelectedManureMaterialType == 0)
                    {
                        ModelState.AddModelError("SelectedManureMaterialType", "Required");
                    }

                    if (msdvm.SelectedMaterialsToInclude != null && !msdvm.SelectedMaterialsToInclude.Any())
                    {
                        ModelState.AddModelError("SelectedMaterialsToInclude", "Required");
                    }

                    if (string.IsNullOrEmpty(msdvm.SystemName))
                    {
                        ModelState.AddModelError("SystemName", "Required");
                    }

                    var otherSystemNames = _ud.GetStorageSystems()
                        .Where(ss => ss.Id != (msdvm.SystemId ?? 0)).Select(x => x.Name);

                    if (otherSystemNames.Any(sn =>
                        sn.Equals(msdvm.SystemName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        ModelState.AddModelError("SystemName", $"\"{msdvm.SystemName}\" has already been used, please enter a different system name.");
                    }

                    if (ModelState.IsValid)
                    {
                        var includedManure = _ud.GetGeneratedManures().Where(gm =>
                            msdvm.SelectedMaterialsToInclude.Any(includedIds => gm.id == includedIds)).ToList();
                        var newSystem = new ManureStorageSystem
                        {
                            Name = msdvm.SystemName,
                            ManureMaterialType = msdvm.SelectedManureMaterialType,
                            MaterialsIncludedInSystem = includedManure
                        };

                        _ud.AddManureStorageSystem(newSystem);
                        msdvm.SystemId = newSystem.Id;

                        var url = Url.Action("RefreshStorageList", "ManureManagement");
                        return Json(new {success = true, url = url, target = msdvm.Target});
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error -" + ex.Message);
            }

            return PartialView(msdvm);
        }

        private MvcRendering.MultiSelectList GetFilteredMaterialsListForCurrentView(ManureStorageDetailViewModel msdvm)
        {
            if (msdvm.SelectedManureMaterialType > 0)
            {
                //Materials already allocated
                var materialIdsToInclude = msdvm.SelectedMaterialsToInclude ?? new List<int>();

                //Materials accounted in another system
                var materialIdsToExclude = new List<int>();
                foreach (var manureStorageSystem in _ud.GetStorageSystems())
                {
                    var accountedFor =
                        manureStorageSystem.MaterialsIncludedInSystem.Where(m =>
                            materialIdsToInclude.All(include => include != m.id)).Select(s => s.id.Value);
                    materialIdsToExclude.AddRange(accountedFor);
                }     

                var generatedManures = _ud.GetGeneratedManures()
                    .Where(g => g.manureType == msdvm.SelectedManureMaterialType &&
                                !materialIdsToExclude.Any(exclude => g.id.HasValue && g.id.Value == exclude));

                    //return new MvcRendering.MultiSelectList(_ud.GetGeneratedManures()
                    //    .Where(g => g.manureType == msdvm.SelectedManureMaterialType), "id", "animalSubTypeName", msdvm.SelectedMaterialsToInclude);
                return new MvcRendering.MultiSelectList(generatedManures, "id", "animalSubTypeName", msdvm.SelectedMaterialsToInclude);
            }

            return null;
        }

        public IActionResult RefreshStorageList()
        {
            return ViewComponent("ManureStorage");
        }
    }
}
