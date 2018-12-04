using System;
using System.Collections.Generic;
using System.Linq;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Calculate;
using Agri.Models.Farm;
using Agri.Models.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MvcRendering = Microsoft.AspNetCore.Mvc.Rendering;
using SERVERAPI.Models.Impl;
using SERVERAPI.Utility;
using SERVERAPI.ViewModels;
using Agri.Models.Configuration;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SERVERAPI.Controllers
{
    public class ManureManagementController : Controller
    {
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public IAgriConfigurationRepository _sd { get; set; }
        public IViewRenderService _viewRenderService { get; set; }
        public AppSettings _settings;
        //private readonly IMapper _mapper;

        public ManureManagementController(IHostingEnvironment env, IViewRenderService viewRenderService, UserData ud,
            IAgriConfigurationRepository sd
            //,IMapper mapper
            )
        {
            _env = env;
            _ud = ud;
            _sd = sd;
            _viewRenderService = viewRenderService;
            //_mapper = mapper;
        }

        #region Manure Generated Obtained

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
                mgovm.selManureMaterialTypeOption = gm.ManureType;

                List<AnimalSubType> animalSubType = _sd.GetAnimalSubTypes(Convert.ToInt32(gm.animalId.ToString()));
                if (animalSubType.Count > 0)
                    mgovm.selAnimalTypeOption = animalSubType[0].AnimalId.ToString();

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
                            AnimalSubType animalSubTypeDetails = _sd.GetAnimalSubType(Convert.ToInt32(mgovm.selSubTypeOption));


                            GeneratedManure gm = new GeneratedManure();
                            gm.animalId = Convert.ToInt32(mgovm.selAnimalTypeOption);
                            gm.animalName = animal.Name;
                            gm.animalSubTypeId = Convert.ToInt32(mgovm.selSubTypeOption);
                            gm.animalSubTypeName = animalSubTypeDetails.Name;
                            gm.averageAnimalNumber = Convert.ToInt32(mgovm.averageAnimalNumber);
                            gm.ManureType = mgovm.selManureMaterialTypeOption;
                            gm.manureTypeName = EnumHelper<Agri.Models.ManureMaterialType>.GetDisplayValue(mgovm.selManureMaterialTypeOption);
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
                                if (animalSubType.LiquidPerGalPerAnimalPerDay.HasValue)
                                    gm.annualAmount = (Math.Round(Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.LiquidPerGalPerAnimalPerDay) * 365)) + " U.S. gallons";
                            }
                            // manure material type is solid
                            else if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Solid)
                            {
                                if (animalSubType.SolidPerPoundPerAnimalPerDay.HasValue)
                                    gm.annualAmount = (Math.Round(((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.SolidPerPoundPerAnimalPerDay) * 365) / 2000))) + " tons";
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

                            AnimalSubType animalSubType = _sd.GetAnimalSubType(Convert.ToInt32(mgovm.selSubTypeOption));

                            ManureMaterialType thisManureMaterialType = 0;
                            if (mgovm.selManureMaterialTypeOption != 0)
                                thisManureMaterialType = mgovm.selManureMaterialTypeOption;

                            Animal animal = _sd.GetAnimal(Convert.ToInt32(mgovm.selAnimalTypeOption));

                            gm.Id = mgovm.id;
                            gm.animalId = thisAnimalType;
                            gm.animalName = animal.Name;
                            gm.animalSubTypeId = thisSubType;
                            gm.animalSubTypeName = animalSubType.Name;
                            gm.averageAnimalNumber = Convert.ToInt32(mgovm.averageAnimalNumber);
                            gm.ManureType = thisManureMaterialType;
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


                            // manure material type is liquid
                            if (Convert.ToInt32(mgovm.selManureMaterialTypeOption) == 1)
                            {
                                if (animalSubType.LiquidPerGalPerAnimalPerDay.HasValue)
                                    gm.annualAmount = (Math.Round(Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.LiquidPerGalPerAnimalPerDay) * 365)) + " U.S. gallons";
                            }
                            // manure material type is solid
                            else if (Convert.ToInt32(mgovm.selManureMaterialTypeOption) == 2)
                            {

                                if (animalSubType.SolidPerPoundPerAnimalPerDay.HasValue)
                                    gm.annualAmount = (Math.Round(((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.SolidPerPoundPerAnimalPerDay) * 365) / 2000))) + " tons";
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

            mgovm.animalTypeOptions = new List<SelectListItem>();
            mgovm.animalTypeOptions = _sd.GetAnimalTypesDll().ToList();

            mgovm.subTypeOptions = new List<SelectListItem>();

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
            dvm.subTypeName = _sd.GetAnimalSubType(Convert.ToInt32(gm.animalSubTypeId)).Name;

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

        #endregion

        #region Manure Storage

        [HttpGet]
        public IActionResult ManureStorage()
        {
            return View();
        }
        
        public IActionResult ManureStorageDetail(int? id, string mode, int? structureId, string target)
        {
            var msvm = new ManureStorageDetailViewModel();
            var systemTitle = "Storage System Details";

            try
            {
                if (mode == "addSystem")
                {
                    msvm.Title = systemTitle;
                    msvm.DisableSystemFields = false;
                    msvm.ShowStructureFields = true;
                }

                if (id.HasValue)
                {
                    msvm.DisableMaterialTypeForEditMode = true;
                    var savedStorageSystem = _ud.GetStorageSystem(id.Value);
                    msvm.SystemId = savedStorageSystem.Id;
                    msvm.SystemName = savedStorageSystem.Name;
                    msvm.SelectedManureMaterialType = savedStorageSystem.ManureMaterialType;
                    var selectedMaterialsToInclude = savedStorageSystem.MaterialsIncludedInSystem
                        .Where(m => m.Id.HasValue).Select(m => m.Id.Value).ToList();
                    msvm.GeneratedManures = GetFilteredMaterialsListForCurrentView(msvm, selectedMaterialsToInclude);
                    msvm.GetsRunoffFromRoofsOrYards = savedStorageSystem.GetsRunoffFromRoofsOrYards;
                    msvm.RunoffAreaSquareFeet = savedStorageSystem.RunoffAreaSquareFeet;

                    if (structureId.HasValue)
                    {
                        var manureStorageStructure =
                            savedStorageSystem.ManureStorageStructures.Single(mss => mss.Id == structureId);
                        msvm.StorageStructureId = manureStorageStructure.Id;
                        msvm.StorageStructureName = manureStorageStructure.Name;
                        msvm.UncoveredAreaOfStorageStructure = manureStorageStructure.UncoveredAreaSquareFeet;
                        msvm.IsStructureCovered = manureStorageStructure.IsStructureCovered;
                    }

                    msvm.StorageStructureNamePlaceholder = msvm.SelectedManureMaterialType == ManureMaterialType.Liquid ?
                        _sd.GetUserPrompt("storagestructureliquidnameplaceholder") :
                        _sd.GetUserPrompt("storagestructuresolidnameplaceholder");

                    if (mode == "editSystem" && !structureId.HasValue)
                    {
                        msvm.ShowStructureFields = false;
                        msvm.DisableSystemFields = false;
                    }
                    else
                    {

                        msvm.ShowStructureFields = true;
                        msvm.DisableSystemFields = true;
                        systemTitle = "Storage Structure Details";
                    }

                    msvm.Title = systemTitle;
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
                    var defaultSystemName = string.Format(_sd.GetUserPrompt("storagesystemnamedefault"), selectedTypeMsg, systemTypeCountMsg);

                    msdvm.SystemName = defaultSystemName;

                    msdvm.StorageStructureNamePlaceholder = msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid ?
                        _sd.GetUserPrompt("storagestructureliquidnameplaceholder") :
                        _sd.GetUserPrompt("storagestructuresolidnameplaceholder");


                    return View(msdvm);
                }

                if (msdvm.ButtonPressed == "SelectedMaterialsToIncludeChange" || msdvm.ButtonPressed == "SystemNameChange")
                {
                    ModelState.Clear();
                    msdvm.ButtonPressed = "";
                    msdvm.ButtonText = "Save";

                    return View(msdvm);
                }

                if (msdvm.ButtonPressed == "GetsRunoffFromRoofsOrYardsChange")
                {
                    ModelState.Clear();
                    msdvm.ButtonPressed = "";
                    msdvm.ButtonText = "Save";
                    msdvm.RunoffAreaSquareFeet = null;

                    return View(msdvm);
                }

                if (msdvm.ButtonPressed == "IsStructureCoveredChange")
                {
                    ModelState.Clear();
                    msdvm.ButtonPressed = "";
                    msdvm.ButtonText = "Save";

                    if (msdvm.IsStructureCovered)
                    {
                        msdvm.UncoveredAreaOfStorageStructure = null;
                    }
                    return View(msdvm);
                }

                if (msdvm.ButtonText == "Save" || msdvm.SystemId.HasValue)
                {
                    ModelState.Clear();

                    if (!msdvm.DisableSystemFields)
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

                        if (msdvm.GetsRunoffFromRoofsOrYards &&
                            (!msdvm.RunoffAreaSquareFeet.HasValue || msdvm.RunoffAreaSquareFeet <= 0))
                        {
                            ModelState.AddModelError("RunoffAreaSquareFeet", "Required");
                        }

                        var otherSystemNames = _ud.GetStorageSystems()
                            .Where(ss => ss.Id != (msdvm.SystemId ?? 0)).Select(x => x.Name);

                        if (otherSystemNames.Any(sn =>
                            sn.Equals(msdvm.SystemName, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            ModelState.AddModelError("SystemName",
                                $"\"{msdvm.SystemName}\" has already been used, please enter a different system name.");
                        }
                    }

                    if (msdvm.ShowStructureFields)
                    {
                        if (string.IsNullOrWhiteSpace(msdvm.StorageStructureName))
                        {
                            ModelState.AddModelError("StorageStructureName", "Required");
                        }

                        if (!msdvm.IsStructureCovered &&
                            !msdvm.UncoveredAreaOfStorageStructure.HasValue)
                        {
                            ModelState.AddModelError("UncoveredAreaOfStorageStructure", "Required");
                        }

                        if (_ud.GetStorageSystems()
                            .Any(ss =>
                                ss.Id == (msdvm.SystemId ?? 0) &&
                                ss.ManureStorageStructures.Any(s =>
                                    s.Name.Equals(msdvm.StorageStructureName) && s.Id != msdvm.StorageStructureId)))
                        {
                            ModelState.AddModelError("StorageStructureName",
                                $"\"{msdvm.StorageStructureName}\" has already been used, please enter a different structure name.");
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        var includedManure = _ud.GetGeneratedManures().Where(gm =>
                            msdvm.SelectedMaterialsToInclude.Any(includedIds => gm.Id == includedIds)).ToList();
                        includedManure.ForEach(m => { m.AssignedToStoredSystem = true; });

                        ManureStorageSystem manureStorageSystem;

                        if (msdvm.SystemId.HasValue)
                        {
                            manureStorageSystem = _ud.GetStorageSystem(msdvm.SystemId.Value);
                        }
                        else
                        {
                            manureStorageSystem = new ManureStorageSystem();
                        }

                        manureStorageSystem.Name = msdvm.SystemName;
                        manureStorageSystem.ManureMaterialType = msdvm.SelectedManureMaterialType;
                        manureStorageSystem.MaterialsIncludedInSystem = includedManure;
                        manureStorageSystem.GetsRunoffFromRoofsOrYards = msdvm.GetsRunoffFromRoofsOrYards;
                        manureStorageSystem.RunoffAreaSquareFeet = msdvm.RunoffAreaSquareFeet;

                        if (msdvm.ShowStructureFields)
                        {
                            ManureStorageStructure storageStructure;
                            if (msdvm.StorageStructureId.HasValue)
                            {
                                storageStructure =
                                    manureStorageSystem.ManureStorageStructures.Single(mss =>
                                        mss.Id == msdvm.StorageStructureId);
                            }
                            else
                            {
                                storageStructure = new ManureStorageStructure();
                            }

                            storageStructure.Name = msdvm.StorageStructureName;
                            storageStructure.UncoveredAreaSquareFeet = msdvm.UncoveredAreaOfStorageStructure;

                            if (!msdvm.StorageStructureId.HasValue)
                            {
                                manureStorageSystem.AddUpdateManureStorageStructure(storageStructure);
                            }
                        }

                        if (msdvm.SystemId.HasValue)
                        {
                            _ud.UpdateManureStorageSystem(manureStorageSystem);
                        }
                        else
                        {
                            _ud.AddManureStorageSystem(manureStorageSystem);
                            msdvm.SystemId = manureStorageSystem.Id;
                        }

                        _ud.UpdateGenerateManuresAllocationToStorage();

                        var url = Url.Action("RefreshStorageList", "ManureManagement");
                        return Json(new { success = true, url = url, target = msdvm.Target });
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error -" + ex.Message);
            }

            return PartialView(msdvm);
        }

        private List<MvcRendering.SelectListItem> GetFilteredMaterialsListForCurrentView(ManureStorageDetailViewModel msdvm)
        {
            return GetFilteredMaterialsListForCurrentView(msdvm, msdvm.SelectedMaterialsToInclude);
        }

        private List<MvcRendering.SelectListItem> GetFilteredMaterialsListForCurrentView(ManureStorageDetailViewModel msdvm, List<int> selectedMaterials)
        {

            if (msdvm.SelectedManureMaterialType > 0)
            {
                var selectedManuresToInclude = selectedMaterials.ToList();
                //Materials already allocated
                if (msdvm.SystemId.HasValue)
                {
                    selectedManuresToInclude.AddRange(_ud.GetStorageSystems()
                                                                        .Single(ss => ss.Id == msdvm.SystemId).MaterialsIncludedInSystem
                                                                        .Select(m => m.Id.Value).ToList());
                    selectedManuresToInclude = selectedManuresToInclude.GroupBy(s => s).Select(m => m.First()).ToList();
                }

                //Materials accounted in another system
                var materialIdsToExclude = new List<int>();

                foreach (var manureStorageSystem in _ud.GetStorageSystems())
                {
                    var accountedFor =
                        manureStorageSystem.MaterialsIncludedInSystem.Where(m =>
                            selectedManuresToInclude.All(include => include != m.Id)).Select(s => s.Id.Value);
                    materialIdsToExclude.AddRange(accountedFor);
                }

                var generatedManures = _ud.GetGeneratedManures()
                    .Where(g => (
                                            (msdvm.SelectedManureMaterialType == ManureMaterialType.Solid && g.ManureType == ManureMaterialType.Solid)
                                            ||
                                            (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid && (g.ManureType == ManureMaterialType.Liquid || g.ManureType == ManureMaterialType.Solid))
                                        )
                                       && !materialIdsToExclude.Any(exclude => g.Id.HasValue && g.Id.Value == exclude));

                //return new MvcRendering.MultiSelectList(generatedManures, "id", "animalSubTypeName", msdvm.SelectedMaterialsToInclude);
                var manureSelectItems = new List<MvcRendering.SelectListItem>();
                foreach (var generatedManure in generatedManures)
                {
                    manureSelectItems.Add(new MvcRendering.SelectListItem
                    {
                        Value = generatedManure.Id.Value.ToString(),
                        Text = generatedManure.animalSubTypeName,
                        Selected = selectedMaterials.Any(sm => sm == generatedManure.Id.Value)
                    });
                }

                return manureSelectItems;
            }

            return null;
        }

        public IActionResult RefreshStorageList()
        {
            return ViewComponent("ManureStorage");
        }

        [HttpGet]
        public IActionResult ManureStorageDelete(int id, int? structureId, string target)
        {
            var vm = new ManureStorageDeleteViewModel();
            var storageSystem = _ud.GetStorageSystem(id);

            vm.Title = "Delete";
            vm.Target = target;
            vm.StorageSystemName = storageSystem.Name;
            vm.SystemId = storageSystem.Id;

            if (structureId.HasValue)
            {
                var structure =
                    storageSystem.ManureStorageStructures.SingleOrDefault(mss => mss.Id == structureId.Value);
                vm.StorageStructureName = structure.Name;
                vm.StructureId = structure.Id;
            }

            return PartialView("ManureStorageDelete", vm);
        }

        [HttpPost]
        public IActionResult ManureStorageDelete(ManureStorageDeleteViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.StructureId.HasValue)
                {
                    var storageSystem = _ud.GetStorageSystem(vm.SystemId);
                    var structureToDelete = storageSystem.ManureStorageStructures.SingleOrDefault(mss => mss.Id == vm.StructureId);
                    storageSystem.ManureStorageStructures.Remove(structureToDelete);
                    _ud.UpdateManureStorageSystem(storageSystem);
                }
                else
                {
                    _ud.DeleteManureStorageSystem(vm.SystemId);
                    _ud.UpdateGenerateManuresAllocationToStorage();
                }


                string url = Url.Action("RefreshStorageList", "ManureManagement");
                return Json(new { success = true, url = url, target = vm.Target });
            }

            return PartialView("ManureStorageDelete", vm);
        }
        
        #endregion

        #region ManureNutrientAnalysis

        [HttpGet]
        public IActionResult ManureNutrientAnalysis()
        {
            return View();
        }

        public IActionResult CompostDetails(int? id, string target)
        {
            //Utility.CalculateNutrients calculateNutrients = new CalculateNutrients(_env, _ud, _sd);
            //NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            CompostDetailViewModel mvm = new CompostDetailViewModel();

            mvm.act = id == null ? "Add" : "Edit";
            mvm.url = _sd.GetExternalLink("labanalysisexplanation");
            mvm.urlText = _sd.GetUserPrompt("moreinfo");


            if (id != null)
            {
                FarmManure fm = _ud.GetFarmManure(id.Value);

                mvm.selManOption = fm.manureId;

                if (!fm.customized)
                {
                    mvm.bookValue = true;
                    mvm.compost = false;
                    mvm.onlyCustom = false;
                    mvm.showNitrate = false;
                }
                else
                {
                    mvm.bookValue = false;
                    mvm.compost = _sd.IsManureClassCompostType(fm.manure_class);
                    mvm.onlyCustom = (_sd.IsManureClassOtherType(fm.manure_class) || _sd.IsManureClassCompostType(fm.manure_class) || _sd.IsManureClassCompostClassType(fm.manure_class));
                    mvm.showNitrate = (_sd.IsManureClassCompostType(fm.manure_class) || _sd.IsManureClassCompostClassType(fm.manure_class));
                }
                mvm.manureName = fm.name;
                mvm.moisture = fm.moisture;
                mvm.nitrogen = fm.nitrogen.ToString("#0.00");
                mvm.ammonia = fm.ammonia.ToString("#0");
                mvm.phosphorous = fm.phosphorous.ToString("#0.00");
                mvm.potassium = fm.potassium.ToString("#0.00");
                mvm.nitrate = fm.nitrate.HasValue ? fm.nitrate.Value.ToString("#0") : ""; // old version of datafile
            }
            else
            {
                mvm.bookValue = true;
                mvm.manureName = "  ";
                mvm.moisture = "  ";
                mvm.nitrogen = "  ";
                mvm.ammonia = "  ";
                mvm.phosphorous = "  ";
                mvm.potassium = "  ";
                mvm.nitrate = "  ";
                mvm.compost = false;
                mvm.onlyCustom = false;
                mvm.showNitrate = false;
            }

            CompostDetailsSetup(ref mvm);

            return PartialView(mvm);
        }
        private void CompostDetailsSetup(ref CompostDetailViewModel cvm)
        {
            // add storage systems created by user to the list of Material Sources
            var storageSystems = _ud.GetStorageSystems();
            cvm.sourceOfMaterialOptions = new List<SelectListItem>();
            foreach (var storageSystem in storageSystems)
            {
                var li = new SelectListItem()
                { Id = storageSystem.Id, Value = storageSystem.Name };
                cvm.sourceOfMaterialOptions.Add(li);
            }

            cvm.manOptions = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(cvm.selsourceOfMaterialOption) &&
                cvm.selsourceOfMaterialOption != "select")
            {
                var manures = _sd.GetManures();
                var storageSystem = _ud.GetStorageSystem(Convert.ToInt32(cvm.selsourceOfMaterialOption));
                var manuresByMaterialTypes = from manure in manures where manure.SolidLiquid == (storageSystem.ManureMaterialType).ToString() select manure;
                cvm.materialType = storageSystem.ManureMaterialType;
                foreach (var manuresByMaterialType in manuresByMaterialTypes)
                {
                    var li = new SelectListItem()
                    { Id = manuresByMaterialType.Id, Value = manuresByMaterialType.Name };
                    cvm.manOptions.Add(li);
                }
            }

            return;
        }
        [HttpPost]
        public IActionResult CompostDetails(CompostDetailViewModel cvm)
        {
            decimal userNitrogen = 0;
            decimal userAmmonia = 0;
            decimal userPhosphorous = 0;
            decimal userPotassium = 0;
            decimal userMoisture = 0;
            decimal userNitrate = 0;
            Manure man;

            CompostDetailsSetup(ref cvm);

            try
            {
                if (cvm.buttonPressed == "SourceOfMaterialChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";

                    if (cvm.selsourceOfMaterialOption != "" && cvm.selsourceOfMaterialOption != "0" &&
                        cvm.selsourceOfMaterialOption != "select")
                    {
                        var manures = _sd.GetManures();
                        var storageSystem = _ud.GetStorageSystem(Convert.ToInt32(cvm.selsourceOfMaterialOption));
                        var manuresByMaterialTypes = from manure in manures where manure.SolidLiquid == (storageSystem.ManureMaterialType).ToString() select manure;
                        foreach (var manuresByMaterialType in manuresByMaterialTypes)
                        {
                            var li = new SelectListItem()
                                { Id = manuresByMaterialType.Id, Value = manuresByMaterialType.Name };
                            cvm.manOptions.Add(li);
                        }
                    }
                    return View(cvm);
                }

                if (cvm.buttonPressed == "ManureChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";

                    if (cvm.selManOption != 0)
                    {
                        man = _sd.GetManure(cvm.selManOption.ToString());
                        if (_sd.IsManureClassOtherType(man.ManureClass) ||
                           _sd.IsManureClassCompostType(man.ManureClass))
                        {
                            cvm.bookValue = false;
                            cvm.onlyCustom = true;
                            cvm.nitrogen = string.Empty;
                            cvm.moisture = string.Empty;
                            cvm.ammonia = string.Empty;
                            cvm.nitrate = string.Empty;
                            cvm.phosphorous = string.Empty;
                            cvm.potassium = string.Empty;
                            cvm.compost = _sd.IsManureClassCompostType(man.ManureClass);
                            cvm.showNitrate = cvm.compost;
                            cvm.manureName = cvm.compost ? "Custom - " + man.Name + " - " : "Custom - " + man.SolidLiquid + " - ";
                        }
                        else
                        {
                            cvm.showNitrate = _sd.IsManureClassCompostClassType(man.ManureClass);
                            cvm.bookValue = !cvm.showNitrate;
                            cvm.compost = false;
                            if (cvm.showNitrate)
                            {
                                cvm.moistureBook = man.Moisture.ToString();
                                cvm.nitrogenBook = man.Nitrogen.ToString();
                                cvm.ammoniaBook = man.Ammonia.ToString();
                                cvm.nitrateBook = man.Nitrate.ToString();
                                cvm.phosphorousBook = man.Phosphorous.ToString();
                                cvm.potassiumBook = man.Potassium.ToString();
                                cvm.nitrateBook = man.Nitrate.ToString();
                                cvm.manureName = "Custom - " + man.Name + " - ";
                                cvm.onlyCustom = cvm.showNitrate;
                                cvm.bookValue = false;
                            }
                            else
                            {
                                cvm.bookValue = true;
                                cvm.nitrogen = man.Nitrogen.ToString();
                                cvm.moisture = man.Moisture.ToString();
                                cvm.ammonia = man.Ammonia.ToString();
                                cvm.nitrate = man.Nitrate.ToString();
                                cvm.phosphorous = man.Phosphorous.ToString();
                                cvm.potassium = man.Potassium.ToString();
                                cvm.manureName = man.Name;
                                cvm.onlyCustom = false;
                            }
                        }
                    }
                    else
                    {
                        cvm.bookValue = true;
                        cvm.showNitrate = false;
                        cvm.compost = false;
                        cvm.onlyCustom = false;
                        cvm.nitrogen = string.Empty;
                        cvm.moisture = string.Empty;
                        cvm.ammonia = string.Empty;
                        cvm.nitrate = string.Empty;
                        cvm.phosphorous = string.Empty;
                        cvm.potassium = string.Empty;
                        cvm.manureName = string.Empty;
                        cvm.nitrate = String.Empty;
                    }
                    return View(cvm);
                }
                if (cvm.buttonPressed == "TypeChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";

                    if (cvm.selManOption != 0)
                    {
                        man = _sd.GetManure(cvm.selManOption.ToString());
                        cvm.onlyCustom = false;
                        if (cvm.bookValue)
                        {
                            cvm.moisture = cvm.bookValue ? man.Moisture.ToString() : "";
                            cvm.nitrogen = man.Nitrogen.ToString();
                            cvm.ammonia = man.Ammonia.ToString();
                            cvm.nitrate = man.Nitrate.ToString();
                            cvm.phosphorous = man.Phosphorous.ToString();
                            cvm.potassium = man.Potassium.ToString();
                            cvm.manureName = man.Name;
                            cvm.showNitrate = false;
                            cvm.compost = false;

                        }
                        else
                        {
                            cvm.nitrogen = string.Empty;
                            cvm.moisture = string.Empty;
                            cvm.ammonia = string.Empty;
                            cvm.nitrate = string.Empty;
                            cvm.phosphorous = string.Empty;
                            cvm.potassium = string.Empty;
                            cvm.manureName = (!cvm.compost) ? "Custom - " + man.Name + " - " : "Custom - " + man.SolidLiquid + " - ";

                            cvm.moistureBook = man.Moisture.ToString();
                            cvm.nitrogenBook = man.Nitrogen.ToString();
                            cvm.ammoniaBook = man.Ammonia.ToString();
                            cvm.nitrateBook = man.Nitrate.ToString();
                            cvm.phosphorousBook = man.Phosphorous.ToString();
                            cvm.potassiumBook = man.Potassium.ToString();
                            cvm.nitrateBook = man.Nitrate.ToString();
                            // only show  NITRATE when MANURE_CLASS = COMPOST or COMPOSTBOOK
                            cvm.compost = _sd.IsManureClassCompostType(man.ManureClass);
                            cvm.showNitrate = _sd.IsManureClassCompostClassType(man.ManureClass) || _sd.IsManureClassCompostType(man.ManureClass);
                        }
                    }
                    else
                    {
                        cvm.nitrogen = string.Empty;
                        cvm.moisture = string.Empty;
                        cvm.ammonia = string.Empty;
                        cvm.nitrate = string.Empty;
                        cvm.phosphorous = string.Empty;
                        cvm.potassium = string.Empty;
                        cvm.manureName = string.Empty;
                    }
                    return View(cvm);
                }

                if (ModelState.IsValid)
                {
                    man = _sd.GetManure(cvm.selManOption.ToString());

                    if (!cvm.bookValue)
                    {
                        if (string.IsNullOrEmpty(cvm.moisture))
                        {
                            ModelState.AddModelError("moisture", "Required.");
                        }
                        else
                        {
                            if (!Decimal.TryParse(cvm.moisture, out userMoisture))
                            {
                                ModelState.AddModelError("moisture", "Numbers only.");
                            }
                            else
                            {
                                if (userMoisture < 0 || userMoisture > 100)
                                {
                                    ModelState.AddModelError("moisture", "Invalid %.");
                                }
                                else
                                {
                                    if (man.SolidLiquid.ToUpper() == "SOLID" &&
                                       man.ManureClass.ToUpper() == "OTHER")
                                    {
                                        if (userMoisture > 80)
                                        {
                                            ModelState.AddModelError("moisture", "must be \u2264 80%.");
                                        }
                                    }
                                    if (man.SolidLiquid.ToUpper() == "LIQUID" &&
                                       man.ManureClass.ToUpper() == "OTHER")
                                    {
                                        if (userMoisture <= 80)
                                        {
                                            ModelState.AddModelError("moisture", "Must be > 80%.");
                                        }
                                    }
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.nitrogen))
                        {
                            ModelState.AddModelError("nitrogen", "Required.");
                        }
                        else
                        {
                            if (!Decimal.TryParse(cvm.nitrogen, out userNitrogen))
                            {
                                ModelState.AddModelError("nitrogen", "Numbers only.");
                            }
                            else
                            {
                                if (userNitrogen < 0 || userNitrogen > 100)
                                {
                                    ModelState.AddModelError("nitrogen", "Invalid %.");
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.ammonia))
                        {
                            ModelState.AddModelError("ammonia", "Required.");
                        }
                        else
                        {
                            if (!Decimal.TryParse(cvm.ammonia, out userAmmonia))
                            {
                                ModelState.AddModelError("ammonia", "Numbers only.");
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.phosphorous))
                        {
                            ModelState.AddModelError("phosphorous", "Required.");
                        }
                        else
                        {
                            if (!Decimal.TryParse(cvm.phosphorous, out userPhosphorous))
                            {
                                ModelState.AddModelError("phosphorous", "Numbers only.");
                            }
                            else
                            {
                                if (userPhosphorous < 0 || userPhosphorous > 100)
                                {
                                    ModelState.AddModelError("phosphorous", "Invalid %.");
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.potassium))
                        {
                            ModelState.AddModelError("potassium", "Required.");
                        }
                        else
                        {
                            if (!Decimal.TryParse(cvm.potassium, out userPotassium))
                            {
                                ModelState.AddModelError("potassium", "Numbers only.");
                            }
                            else
                            {
                                if (userPotassium < 0 || userPotassium > 100)
                                {
                                    ModelState.AddModelError("potassium", "Invalid %.");
                                }
                            }
                        }
                        if (cvm.showNitrate)
                        {
                            if (string.IsNullOrEmpty(cvm.nitrate))
                            {
                                ModelState.AddModelError("nitrate", "Required.");
                            }
                            else
                            {
                                if (!Decimal.TryParse(cvm.nitrate, out userNitrate))
                                {
                                    ModelState.AddModelError("nitrate", "Numbers only.");
                                }
                            }
                        }
                        else
                            userNitrate = Convert.ToDecimal(cvm.nitrate);

                        if (_sd.GetManureByName(cvm.manureName) != null)
                        {
                            ModelState.AddModelError("manureName", "Description cannot match predefined entries.");
                        }
                    }

                    List<FarmManure> manures = _ud.GetFarmManures();
                    foreach (var m in manures)
                    {
                        if (m.customized &&
                           m.name == cvm.manureName &&
                           m.id != cvm.id)
                        {
                            ModelState.AddModelError("manureName", "Descriptions must be unique.");
                            break;
                        }
                    }


                    if (!ModelState.IsValid)
                        return View(cvm);

                    if (cvm.id == null)
                    {
                        FarmManure fm = new FarmManure();
                        if (cvm.bookValue)
                        {
                            fm.manureId = cvm.selManOption;
                            fm.customized = false;
                        }
                        else
                        {
                            man = _sd.GetManure(cvm.selManOption.ToString());

                            fm.customized = true;
                            fm.manureId = cvm.selManOption;
                            fm.ammonia = Convert.ToInt32(cvm.ammonia);
                            fm.dmid = man.DMId;
                            fm.manure_class = man.ManureClass;
                            fm.moisture = cvm.moisture;
                            fm.name = cvm.manureName;
                            fm.nitrogen = Convert.ToDecimal(cvm.nitrogen);
                            fm.nminerizationid = man.NMineralizationId;
                            fm.phosphorous = Convert.ToDecimal(cvm.phosphorous);
                            fm.potassium = Convert.ToDecimal(cvm.potassium);
                            fm.nitrate = cvm.showNitrate ? Convert.ToDecimal(cvm.nitrate) : (decimal?)null;
                            fm.solid_liquid = man.SolidLiquid;
                        }


                        _ud.AddFarmManure(fm);
                    }
                    else
                    {
                        FarmManure fm = _ud.GetFarmManure(cvm.id.Value);
                        if (cvm.bookValue)
                        {
                            fm = new FarmManure();
                            fm.id = cvm.id.Value;
                            fm.manureId = cvm.selManOption;
                            fm.customized = false;
                        }
                        else
                        {
                            man = _sd.GetManure(cvm.selManOption.ToString());

                            fm.customized = true;
                            fm.manureId = cvm.selManOption;
                            fm.ammonia = Convert.ToInt32(cvm.ammonia);
                            fm.dmid = man.DMId;
                            fm.manure_class = man.ManureClass;
                            fm.moisture = cvm.moisture;
                            fm.name = cvm.manureName;
                            fm.nitrogen = Convert.ToDecimal(cvm.nitrogen);
                            fm.nminerizationid = man.NMineralizationId;
                            fm.phosphorous = Convert.ToDecimal(cvm.phosphorous);
                            fm.potassium = Convert.ToDecimal(cvm.potassium);
                            fm.solid_liquid = man.SolidLiquid;
                            fm.nitrate = cvm.showNitrate ? Convert.ToDecimal(cvm.nitrate) : (decimal?)null;
                        }

                        _ud.UpdateFarmManure(fm);

                        ReCalculateManure(fm.id);
                    }

                    string url = Url.Action("RefreshCompostList", "Manure");
                    return Json(new { success = true, url = url, target = cvm.target });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error.");
            }

            return PartialView(cvm);
        }
        private void ReCalculateManure(int id)
        {
            Utility.CalculateNutrients calculateNutrients = new CalculateNutrients(_env, _ud, _sd);
            NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            List<Field> flds = _ud.GetFields();

            foreach (var fld in flds)
            {
                List<NutrientManure> mans = _ud.GetFieldNutrientsManures(fld.fieldName);

                foreach (var nm in mans)
                {
                    if (id.ToString() == nm.manureId)
                    {
                        int regionid = _ud.FarmDetails().farmRegion.Value;
                        Region region = _sd.GetRegion(regionid);
                        nOrganicMineralizations = calculateNutrients.GetNMineralization(Convert.ToInt16(nm.manureId), region.LocationId);

                        string avail = (nOrganicMineralizations.OrganicN_FirstYear * 100).ToString("###");

                        string nh4 = (calculateNutrients.GetAmmoniaRetention(Convert.ToInt16(nm.manureId), Convert.ToInt16(nm.applicationId)) * 100).ToString("###");

                        NutrientInputs nutrientInputs = new NutrientInputs();

                        calculateNutrients.manure = nm.manureId;
                        calculateNutrients.applicationSeason = nm.applicationId;
                        calculateNutrients.applicationRate = Convert.ToDecimal(nm.rate);
                        calculateNutrients.applicationRateUnits = nm.unitId;
                        calculateNutrients.ammoniaNRetentionPct = Convert.ToDecimal(nh4);
                        calculateNutrients.firstYearOrganicNAvailablityPct = Convert.ToDecimal(avail);

                        calculateNutrients.GetNutrientInputs(nutrientInputs);

                        nm.yrN = nutrientInputs.N_FirstYear;
                        nm.yrP2o5 = nutrientInputs.P2O5_FirstYear;
                        nm.yrK2o = nutrientInputs.K2O_FirstYear;
                        nm.ltN = nutrientInputs.N_LongTerm;
                        nm.ltP2o5 = nutrientInputs.P2O5_LongTerm;
                        nm.ltK2o = nutrientInputs.K2O_LongTerm;

                        _ud.UpdateFieldNutrientsManure(fld.fieldName, nm);
                    }
                }
            }
        }
        public ActionResult CompostDelete(int id, string target)
        {
            CompostDeleteViewModel dvm = new CompostDeleteViewModel();
            bool manureUsed = false;

            dvm.id = id;
            dvm.target = target;

            FarmManure nm = _ud.GetFarmManure(id);

            dvm.manureName = nm.name;

            // determine if the selected manure is currently being used on any of the fields
            List<Field> flds = _ud.GetFields();

            foreach (var fld in flds)
            {
                List<NutrientManure> mans = _ud.GetFieldNutrientsManures(fld.fieldName);

                foreach (var man in mans)
                {
                    if (id.ToString() == man.manureId)
                    {
                        manureUsed = true;
                    }
                }
            }

            if (manureUsed)
            {
                dvm.warning = _sd.GetUserPrompt("manuredeletewarning");
            }

            dvm.act = "Delete";

            return PartialView("CompostDelete", dvm);
        }
        [HttpPost]
        public ActionResult CompostDelete(CompostDeleteViewModel dvm)
        {
            if (ModelState.IsValid)
            {
                // first remove manure from all fields that had it applied
                if (!string.IsNullOrEmpty(dvm.warning))
                {
                    List<Field> flds = _ud.GetFields();

                    foreach (var fld in flds)
                    {
                        List<NutrientManure> mans = _ud.GetFieldNutrientsManures(fld.fieldName);

                        foreach (var man in mans)
                        {
                            if (dvm.id.ToString() == man.manureId)
                            {
                                _ud.DeleteFieldNutrientsManure(fld.fieldName, man.id);
                            }
                        }
                    }
                }

                // delete the actual manure
                _ud.DeleteFarmManure(dvm.id);

                string url = Url.Action("RefreshCompostList", "Manure");
                return Json(new { success = true, url = url, target = dvm.target });
            }
            return PartialView("CompostDelete", dvm);
        }
        public IActionResult RefreshCompostList()
        {
            return ViewComponent("Compost");
        }

        #endregion

        #region ManureImported

        [HttpGet]
        public IActionResult ManureImported()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ManureImportedDetail(int? id, string target)
        {
            var vm = new ManureImportedDetailViewModel();

            vm.Title = "Imported Material Details";
            vm.Target = target;
            vm.SelectedManureType = ManureMaterialType.Solid;
            vm.IsLandAppliedBeforeStorage = true;
            vm.LandAppliedLabelText = _sd.GetUserPrompt("importmaterialislandappliedquestion");

            if (id.HasValue)
            {
                var savedImportedManure = _ud.GetImportedManure(id.Value);
            }

            return PartialView("ManureImportedDetail", vm);
        }

        [HttpPost]
        public IActionResult ManureImportedDetail(ManureImportedDetailViewModel vm)
        {

            if (vm.ButtonPressed == "ManureMaterialTypeChange")
            {
                ModelState.Clear();
                vm.ButtonPressed = "";
                vm.ButtonText = "Save";

                return PartialView("ManureImportedDetail", vm);
            }

            //if (vm.SelectedManureType < ManureMaterialType.Liquid)
            //{
            //    ModelState.AddModelError("SelectedManureType", "Required");
            //}
            if (!vm.AnnualAmount.HasValue || vm.AnnualAmount < 0)
            {
                    ModelState.AddModelError("AnnualAmount", "Enter a numeric value");
            }


            if (ModelState.IsValid)
            {
                var url = Url.Action("RefreshImportList", "ManureManagement");
                return Json(new { success = true, url = url, target = vm.Target });
            }

            return PartialView("ManureImportedDetail", vm);
        }

        public IActionResult RefreshImportList()
        {
            return ViewComponent("ManureImported");
        }

        #endregion
    }
}
