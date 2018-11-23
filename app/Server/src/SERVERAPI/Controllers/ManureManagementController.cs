using System;
using System.Collections.Generic;
using System.Linq;
using Agri.Interfaces;
using Agri.Models;
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
                mgovm.selManureMaterialTypeOption = gm.manureType;

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
                            gm.manureType = mgovm.selManureMaterialTypeOption;
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

                            gm.id = mgovm.id;
                            gm.animalId = thisAnimalType;
                            gm.animalName = animal.Name;
                            gm.animalSubTypeId = thisSubType;
                            gm.animalSubTypeName = animalSubType.Name;
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

                msvm.StorageStructureNamePlaceholder = _sd.GetUserPrompt("storagestructurenameplaceholder");

                if (id.HasValue)
                {

                    msvm.DisableMaterialTypeForEditMode = true;
                    var savedStorageSystem = _ud.GetStorageSystem(id.Value);
                    msvm.SystemId = savedStorageSystem.Id;
                    msvm.SystemName = savedStorageSystem.Name;
                    msvm.SelectedManureMaterialType = savedStorageSystem.ManureMaterialType;
                    var selectedMaterialsToInclude = savedStorageSystem.MaterialsIncludedInSystem
                        .Where(m => m.id.HasValue).Select(m => m.id.Value).ToList();
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
                    }

                    var manureType = msvm.SelectedManureMaterialType == ManureMaterialType.Solid
                        ? "Solid"
                        : "Liquid";

                    systemTitle = $"{manureType} {systemTitle}";

                    if (mode == "editSystem" && !structureId.HasValue)
                    {
                        msvm.ShowStructureFields = false;
                        msvm.DisableSystemFields = false;
                    }
                    else
                    {
                        msvm.ShowStructureFields = true;
                        msvm.DisableSystemFields = true;
                        systemTitle = $"Storage Structure - {systemTitle}";
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
                    var placeHolder = string.Format(_sd.GetUserPrompt("storagesystemnameplaceholder"), selectedTypeMsg, systemTypeCountMsg);

                    msdvm.SystemNamePlaceholder = placeHolder;

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

                        if (msdvm.ShowUncoveredAreaOfStorageStructure &&
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
                            msdvm.SelectedMaterialsToInclude.Any(includedIds => gm.id == includedIds)).ToList();
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
                                                                        .Select(m => m.id.Value).ToList());
                    selectedManuresToInclude = selectedManuresToInclude.GroupBy(s => s).Select(m => m.First()).ToList();
                }

                //Materials accounted in another system
                var materialIdsToExclude = new List<int>();

                foreach (var manureStorageSystem in _ud.GetStorageSystems())
                {
                    var accountedFor =
                        manureStorageSystem.MaterialsIncludedInSystem.Where(m =>
                            selectedManuresToInclude.All(include => include != m.id)).Select(s => s.id.Value);
                    materialIdsToExclude.AddRange(accountedFor);
                }

                var generatedManures = _ud.GetGeneratedManures()
                    .Where(g => (
                                            (msdvm.SelectedManureMaterialType == ManureMaterialType.Solid && g.manureType == ManureMaterialType.Solid)
                                            ||
                                            (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid && (g.manureType == ManureMaterialType.Liquid || g.manureType == ManureMaterialType.Solid))
                                        )
                                       && !materialIdsToExclude.Any(exclude => g.id.HasValue && g.id.Value == exclude));

                //return new MvcRendering.MultiSelectList(generatedManures, "id", "animalSubTypeName", msdvm.SelectedMaterialsToInclude);
                var manureSelectItems = new List<MvcRendering.SelectListItem>();
                foreach (var generatedManure in generatedManures)
                {
                    manureSelectItems.Add(new MvcRendering.SelectListItem
                    {
                        Value = generatedManure.id.Value.ToString(),
                        Text = generatedManure.animalSubTypeName,
                        Selected = selectedMaterials.Any(sm => sm == generatedManure.id.Value)
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

        [HttpGet]
        public IActionResult ManureStorageMaterialsRequireAssigning(string target)
        {
            var vm = new ManureStorageMaterialsRequireAssigningViewModel();

            vm.Title = "";
            vm.Target = target;
            vm.UnallocatedGeneratedManures = _ud.GetGeneratedManures().Where(gm => !gm.AssignedToStoredSystem).ToList();

            return PartialView("ManureStorageMaterialsRequireAssigning", vm);
        }

        #endregion

        #region ManureNutrientAnalysis

        [HttpGet]
        public IActionResult ManureNutrientAnalysis()
        {
            return View();
        }

        #endregion

        #region ManureImported

        [HttpGet]
        public IActionResult ManureImported()
        {
            return View();
        }

        #endregion
    }
}
