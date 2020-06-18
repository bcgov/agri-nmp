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
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SERVERAPI.Filters;

namespace SERVERAPI.Controllers
{
    [SessionTimeout]
    public class ManureManagementController : BaseController
    {
        private readonly ILogger<ManureManagementController> _logger;
        private readonly IHostingEnvironment _env;
        private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;
        private readonly IManureUnitConversionCalculator _manureUnitConversionCalculator;
        private readonly IManureLiquidSolidSeparationCalculator _manureLiquidSolidSeparationCalculator;
        private readonly IManureAnimalNumberCalculator _manureAnimalNumberCalculator;
        private readonly IManureOctoberToMarchCalculator _manureOctoberToMarchCalculator;
        private readonly IStorageVolumeCalculator _storageVolumeCalculator;
        private readonly IViewRenderService _viewRenderService;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _appSettings;

        public ManureManagementController(ILogger<ManureManagementController> logger,
            IHostingEnvironment env,
            IViewRenderService viewRenderService, UserData ud,
            IAgriConfigurationRepository sd,
            IManureUnitConversionCalculator manureUnitConversionCalculator,
            IManureLiquidSolidSeparationCalculator manureLiquidSolidSeparationCalculator,
            IStorageVolumeCalculator storageVolumeCalculator,
            IManureAnimalNumberCalculator manureAnimalNumberCalculator,
            IManureOctoberToMarchCalculator manureOctoberToMarchCalculator,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _env = env;
            _ud = ud;
            _sd = sd;
            _manureUnitConversionCalculator = manureUnitConversionCalculator;
            _manureLiquidSolidSeparationCalculator = manureLiquidSolidSeparationCalculator;
            _manureAnimalNumberCalculator = manureAnimalNumberCalculator;
            _manureOctoberToMarchCalculator = manureOctoberToMarchCalculator;
            _storageVolumeCalculator = storageVolumeCalculator;
            _viewRenderService = viewRenderService;
            _mapper = mapper;
            _appSettings = appSettings;
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

            mgovm.title = id == null ? "Add" : "Edit";
            mgovm.stdWashWater = true;
            mgovm.stdMilkProduction = true;
            // mgovm.placehldr = _sd.GetUserPrompt("averageanimalnumberplaceholder");

            if (id != null)
            {
                GeneratedManure gm = _ud.GetGeneratedManure(id.Value);
                mgovm.id = id;
                mgovm.selSubTypeOption = gm.animalSubTypeId.ToString();
                mgovm.averageAnimalNumber = gm.averageAnimalNumber.ToString();
                mgovm.selManureMaterialTypeOption = gm.ManureType;
                mgovm.SelWashWaterUnit = gm.washWaterUnits;
                mgovm.showBreedAndGrazingDaysPerYear = gm.showBreedAndGrazingDaysPerYear;

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

                if (mgovm.showBreedAndGrazingDaysPerYear)
                {
                    mgovm.grazingDaysPerYear = gm.grazingDaysPerYear.ToString();
                    mgovm.selBreedOption = gm.BreedId.ToString();
                }

                if (mgovm.showWashWater)
                {
                    mgovm.washWater = gm.washWater.ToString("#.##");
                }
                if (mgovm.showMilkProduction)
                {
                    mgovm.milkProduction = gm.milkProduction.ToString("#.##");
                }

                if (mgovm.SelWashWaterUnit == WashWaterUnits.USGallonsPerDay)
                {
                    if (mgovm.washWater != (Math.Round((calculateAnimalRequirement.GetWashWaterBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)) ?? 0) * Convert.ToInt32(mgovm.averageAnimalNumber))).ToString())
                    {
                        mgovm.stdWashWater = false;
                    }
                }
                else if (mgovm.SelWashWaterUnit == WashWaterUnits.USGallonsPerDayPerAnimal)
                {
                    if (mgovm.washWater != calculateAnimalRequirement.GetWashWaterBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString())
                    {
                        mgovm.stdWashWater = false;
                    }
                }

                if (mgovm.milkProduction != calculateAnimalRequirement.GetDefaultMilkProductionBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString())
                {
                    mgovm.stdMilkProduction = false;
                }
                animalTypeDetailsSetup(ref mgovm);
            }
            else
            {
                setAnimalFromGeneratedManureList(ref mgovm);
                animalTypeDetailsSetup(ref mgovm);
                if (mgovm.showBreedAndGrazingDaysPerYear)
                {
                    mgovm.grazingDaysPerYear = "0";
                }
            }

            return PartialView("ManureGeneratedObtainedDetail", mgovm);
        }

        [HttpPost]
        public IActionResult ManureGeneratedObtainedDetail(ManureGeneratedObtainedDetailViewModel mgovm)
        {
            CalculateAnimalRequirement calculateAnimalRequirement = new CalculateAnimalRequirement(_ud, _sd);

            // mgovm.placehldr = _sd.GetUserPrompt("averageanimalnumberplaceholder");
            mgovm.ExplainWashWaterVolumesDaily = _sd.GetUserPrompt("ExplainWashWaterTypes");

            animalTypeDetailsSetup(ref mgovm);
            try
            {
                if (mgovm.buttonPressed == "BreedChange")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";
                    mgovm.milkProduction = "";
                    if (mgovm.selAnimalTypeOption != "" &&
                        mgovm.selAnimalTypeOption != "0" &&
                        mgovm.selAnimalTypeOption != "select animal")
                    {
                        var defaultMilkProd =
                            calculateAnimalRequirement.GetDefaultMilkProductionBySubTypeId(
                                Convert.ToInt16(mgovm.selSubTypeOption));
                        var breedManureFactor = calculateAnimalRequirement.GetBreedManureFactorByBreedId(Convert.ToInt32(mgovm.selBreedOption));
                        var milkProd = defaultMilkProd * breedManureFactor;

                        mgovm.milkProduction = Math.Round(milkProd ?? 0M, 1).ToString();
                        mgovm.stdMilkProduction = true;
                    }
                    return View(mgovm);
                }

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
                            mgovm.stdWashWater = true;
                        }
                        if (mgovm.showMilkProduction)
                        {
                            mgovm.milkProduction = (calculateAnimalRequirement
                                .GetDefaultMilkProductionBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)) * calculateAnimalRequirement.GetBreedManureFactorByBreedId(Convert.ToInt32(mgovm.selBreedOption))).ToString();
                            mgovm.stdMilkProduction = true;
                        }

                        if (_sd.DoesAnimalUseWashWater(Convert.ToInt32(mgovm.selSubTypeOption)))
                        {
                            mgovm.SelWashWaterUnit = WashWaterUnits.USGallonsPerDayPerAnimal;
                            var washWaterUnits = mgovm.SelWashWaterUnit;
                            if (washWaterUnits == WashWaterUnits.USGallonsPerDay && mgovm.averageAnimalNumber != null)
                            {
                                mgovm.washWater = (Math.Round((Convert.ToInt32(mgovm.averageAnimalNumber) *
                                                               Convert.ToDecimal(
                                                                   calculateAnimalRequirement
                                                                       .GetWashWaterBySubTypeId(
                                                                           Convert.ToInt16(mgovm.selSubTypeOption))
                                                                       .ToString())))).ToString();
                            }
                            else if (washWaterUnits == WashWaterUnits.USGallonsPerDayPerAnimal)
                            {
                                mgovm.washWater = calculateAnimalRequirement
                                    .GetWashWaterBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString();
                            }
                        }
                    }

                    AnimalSubType animalSubType = _sd.GetAnimalSubType(Convert.ToInt32(mgovm.selSubTypeOption));

                    mgovm.liquidPerGalPerAnimalPerDay = animalSubType.LiquidPerGalPerAnimalPerDay.ToString();
                    mgovm.solidPerPoundPerAnimalPerDay = animalSubType.SolidPerPoundPerAnimalPerDay.ToString();

                    if (mgovm.liquidPerGalPerAnimalPerDay != "0.0000" && mgovm.solidPerPoundPerAnimalPerDay == "0.0000")
                    {
                        mgovm.selManureMaterialTypeOption = ManureMaterialType.Liquid;
                        mgovm.stdManureMaterialType = false;
                        mgovm.hasLiquidManureType = true;
                    }
                    else if (mgovm.solidPerPoundPerAnimalPerDay != "0.0000" && mgovm.liquidPerGalPerAnimalPerDay == "0.0000")
                    {
                        mgovm.selManureMaterialTypeOption = ManureMaterialType.Solid;
                        mgovm.stdManureMaterialType = false;
                        mgovm.hasSolidManureType = true;
                    }

                    // Calculate Average Animal Number hint text when the Milking cow is added to the list
                    List<GeneratedManure> generatedManures = _ud.GetGeneratedManures();
                    if (generatedManures.Any(gm => gm.milkProduction.ToString() != "0.0"))
                    {
                        int animalNumber = generatedManures.Single(gm => gm.milkProduction.ToString() != "0.0").averageAnimalNumber;
                        mgovm.placehldr = _manureAnimalNumberCalculator.CalculateAverageAnimalNumber(animalNumber,
                            mgovm.selSubTypeOption);
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
                    mgovm.hasLiquidManureType = false;
                    mgovm.hasSolidManureType = false;

                    if (!string.IsNullOrEmpty(mgovm.selAnimalTypeOption) &&
                        mgovm.selAnimalTypeOption != "select animal")
                    {
                        mgovm.subTypeOptions = _sd.GetSubtypesDll(Convert.ToInt32(mgovm.selAnimalTypeOption)).ToList();
                        if (mgovm.subTypeOptions.Count() > 1)
                        {
                            mgovm.subTypeOptions.Insert(0, new SelectListItem() { Id = 0, Value = "select subtype" });
                            mgovm.selSubTypeOption = "select subtype";
                            mgovm.selManureMaterialTypeOption = 0;
                        }

                        if (mgovm.subTypeOptions.Count() == 1)
                        {
                            mgovm.selSubTypeOption = mgovm.subTypeOptions[0].Id.ToString();

                            AnimalSubType animalSubType = _sd.GetAnimalSubType(Convert.ToInt32(mgovm.selSubTypeOption));

                            mgovm.liquidPerGalPerAnimalPerDay = animalSubType.LiquidPerGalPerAnimalPerDay.ToString();
                            mgovm.solidPerPoundPerAnimalPerDay = animalSubType.SolidPerPoundPerAnimalPerDay.ToString();

                            if (mgovm.liquidPerGalPerAnimalPerDay != "0.0000" && mgovm.solidPerPoundPerAnimalPerDay == "0.0000")
                            {
                                mgovm.selManureMaterialTypeOption = ManureMaterialType.Liquid;
                                mgovm.stdManureMaterialType = false;
                                mgovm.hasLiquidManureType = true;
                            }
                            else if (mgovm.solidPerPoundPerAnimalPerDay != "0.0000" && mgovm.liquidPerGalPerAnimalPerDay == "0.0000")
                            {
                                mgovm.selManureMaterialTypeOption = ManureMaterialType.Solid;
                                mgovm.stdManureMaterialType = false;
                                mgovm.hasSolidManureType = true;
                            }
                        }

                        if (mgovm.showBreedAndGrazingDaysPerYear)
                        {
                            mgovm.grazingDaysPerYear = "0";
                        }
                    }

                    if (!string.IsNullOrEmpty(mgovm.selSubTypeOption) &&
                        mgovm.selSubTypeOption != "select subtype")
                    {
                        AnimalSubType animalSubType = _sd.GetAnimalSubType(Convert.ToInt32(mgovm.selSubTypeOption));

                        mgovm.liquidPerGalPerAnimalPerDay = animalSubType.LiquidPerGalPerAnimalPerDay.ToString();
                        mgovm.solidPerPoundPerAnimalPerDay = animalSubType.SolidPerPoundPerAnimalPerDay.ToString();

                        if (mgovm.liquidPerGalPerAnimalPerDay != "0.0000" && mgovm.solidPerPoundPerAnimalPerDay == "0.0000")
                        {
                            mgovm.selManureMaterialTypeOption = ManureMaterialType.Liquid;
                            mgovm.stdManureMaterialType = false;
                            mgovm.hasLiquidManureType = true;
                        }
                        else if (mgovm.solidPerPoundPerAnimalPerDay != "0.0000" && mgovm.liquidPerGalPerAnimalPerDay == "0.0000")
                        {
                            mgovm.selManureMaterialTypeOption = ManureMaterialType.Solid;
                            mgovm.stdManureMaterialType = false;
                            mgovm.hasSolidManureType = true;
                        }
                    }

                    return View(mgovm);
                }

                if (mgovm.buttonPressed == "WashWaterUnitsChange")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";
                    mgovm.stdWashWater = true;

                    var washWaterUnits = mgovm.SelWashWaterUnit;

                    if (washWaterUnits == WashWaterUnits.USGallonsPerDay && mgovm.averageAnimalNumber != null)
                    {
                        mgovm.washWater = (Math.Round((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(calculateAnimalRequirement
                                               .GetWashWaterBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString())))).ToString();
                    }
                    else if (washWaterUnits == WashWaterUnits.USGallonsPerDayPerAnimal)
                    {
                        mgovm.washWater = calculateAnimalRequirement
                            .GetWashWaterBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString();
                    }

                    return View(mgovm);
                }

                if (mgovm.buttonPressed == "ResetWashWater")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";
                    mgovm.stdWashWater = true;
                    var washWaterUnits = mgovm.SelWashWaterUnit;

                    if (washWaterUnits == WashWaterUnits.USGallonsPerDay && mgovm.averageAnimalNumber != null)
                    {
                        mgovm.washWater = (Math.Round((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(
                                                           calculateAnimalRequirement
                                                               .GetWashWaterBySubTypeId(
                                                                   Convert.ToInt16(mgovm.selSubTypeOption))
                                                               .ToString())))).ToString();
                    }
                    else if (washWaterUnits == WashWaterUnits.USGallonsPerDayPerAnimal)
                    {
                        mgovm.washWater = calculateAnimalRequirement
                            .GetWashWaterBySubTypeId(Convert.ToInt16(mgovm.selSubTypeOption)).ToString();
                    }

                    return View(mgovm);
                }

                if (mgovm.buttonPressed == "ResetMilkProduction")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";

                    mgovm.stdMilkProduction = true;
                    var defaultMilkProd =
                        calculateAnimalRequirement.GetDefaultMilkProductionBySubTypeId(
                            Convert.ToInt16(mgovm.selSubTypeOption));
                    var breedManureFactor = calculateAnimalRequirement.GetBreedManureFactorByBreedId(Convert.ToInt32(mgovm.selBreedOption));
                    var milkProd = defaultMilkProd * breedManureFactor;

                    mgovm.milkProduction = Math.Round(milkProd ?? 0M, 1).ToString();

                    return View(mgovm);
                }

                if (mgovm.showBreedAndGrazingDaysPerYear)
                {
                    if (!(Convert.ToInt32(mgovm.grazingDaysPerYear) >= 0 && Convert.ToInt32(mgovm.grazingDaysPerYear) <= 365))
                    {
                        ModelState.AddModelError("", "Grazing must be a value between 0 and 365.");
                    }
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

                        if (mgovm.showMilkProduction)
                        {
                            var defaultMilkProd =
                                calculateAnimalRequirement.GetDefaultMilkProductionBySubTypeId(
                                    Convert.ToInt16(mgovm.selSubTypeOption));
                            var breedManureFactor = calculateAnimalRequirement.GetBreedManureFactorByBreedId(Convert.ToInt32(mgovm.selBreedOption));
                            var milkProd = defaultMilkProd * breedManureFactor;

                            if (mgovm.milkProduction != Math.Round(milkProd ?? 0M, 1).ToString())
                            {
                                mgovm.stdMilkProduction = false;
                            }
                        }

                        if (!string.IsNullOrEmpty(mgovm.selSubTypeOption) &&
                            mgovm.selSubTypeOption != "select subtype")
                        {
                            AnimalSubType animalSubType = _sd.GetAnimalSubType(Convert.ToInt32(mgovm.selSubTypeOption));

                            mgovm.liquidPerGalPerAnimalPerDay = animalSubType.LiquidPerGalPerAnimalPerDay.ToString();
                            mgovm.solidPerPoundPerAnimalPerDay = animalSubType.SolidPerPoundPerAnimalPerDay.ToString();

                            if (mgovm.liquidPerGalPerAnimalPerDay != "0.0000" && mgovm.solidPerPoundPerAnimalPerDay == "0.0000")
                            {
                                mgovm.selManureMaterialTypeOption = ManureMaterialType.Liquid;
                                mgovm.stdManureMaterialType = false;
                                mgovm.hasLiquidManureType = true;
                            }
                            else if (mgovm.solidPerPoundPerAnimalPerDay != "0.0000" && mgovm.liquidPerGalPerAnimalPerDay == "0.0000")
                            {
                                mgovm.selManureMaterialTypeOption = ManureMaterialType.Solid;
                                mgovm.stdManureMaterialType = false;
                                mgovm.hasSolidManureType = true;
                            }
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
                            gm.solidPerGalPerAnimalPerDay = animalSubTypeDetails.SolidPerGalPerAnimalPerDay;
                            gm.averageAnimalNumber = Convert.ToInt32(mgovm.averageAnimalNumber);
                            gm.ManureType = mgovm.selManureMaterialTypeOption;
                            gm.manureTypeName = EnumHelper<Agri.Models.ManureMaterialType>.GetDisplayValue(mgovm.selManureMaterialTypeOption);
                            gm.washWaterUnits = mgovm.SelWashWaterUnit;
                            gm.showBreedAndGrazingDaysPerYear = mgovm.showBreedAndGrazingDaysPerYear;
                            if (mgovm.showBreedAndGrazingDaysPerYear)
                            {
                                gm.grazingDaysPerYear = Convert.ToInt32(mgovm.grazingDaysPerYear);
                                List<SelectListItem> breedDetails = _sd.GetBreed(Convert.ToInt32(mgovm.selBreedOption));
                                gm.BreedId = breedDetails[0].Id;
                                gm.BreedName = breedDetails[0].Value;
                            }

                            if (mgovm.washWater != null)
                            {
                                gm.washWater = Convert.ToDecimal(mgovm.washWater.ToString());
                            }
                            else
                            {
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

                            // annual manure generation for Dairy Cattle - Dairy cattle has Breed
                            if (mgovm.breedOptions.Count() > 0)
                            {
                                var defaultMilkProd =
                                    calculateAnimalRequirement.GetDefaultMilkProductionBySubTypeId(
                                        Convert.ToInt16(mgovm.selSubTypeOption));
                                var breedManureFactor = calculateAnimalRequirement.GetBreedManureFactorByBreedId(Convert.ToInt32(mgovm.selBreedOption));
                                var milkProd = defaultMilkProd * breedManureFactor;

                                // annual manure generation for non milking cows
                                if (mgovm.milkProduction == null)
                                {
                                    // manure material type is liquid
                                    if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Liquid)
                                    {
                                        if (animalSubType.LiquidPerGalPerAnimalPerDay.HasValue)
                                            gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.LiquidPerGalPerAnimalPerDay) * (365 - Convert.ToInt32(mgovm.grazingDaysPerYear)) * breedManureFactor ?? 0M))) + " U.S. gallons";
                                    }
                                    // manure material type is solid
                                    else if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Solid)
                                    {
                                        if (animalSubType.SolidPerPoundPerAnimalPerDay.HasValue)
                                        {
                                            var grazingCal = Convert.ToDecimal(((365 - Convert.ToInt32(mgovm.grazingDaysPerYear)) / 2000.0).ToString());
                                            gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.SolidPerPoundPerAnimalPerDay) * grazingCal) * breedManureFactor ?? 0M)))) + " tons";
                                        }
                                    }
                                }
                                // annual manure generation for milking cows
                                else
                                {
                                    if (mgovm.milkProduction != Math.Round(milkProd ?? 0M, 1).ToString())
                                    {
                                        // manure material type is liquid
                                        if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Liquid)
                                        {
                                            if (animalSubType.LiquidPerGalPerAnimalPerDay.HasValue)
                                                gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.LiquidPerGalPerAnimalPerDay) * (365 - Convert.ToInt32(mgovm.grazingDaysPerYear)) * ((Convert.ToDecimal(mgovm.milkProduction)) / (milkProd ?? 0M))))) + " U.S. gallons";
                                        }
                                        // manure material type is solid
                                        else if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Solid)
                                        {
                                            if (animalSubType.SolidPerPoundPerAnimalPerDay.HasValue)
                                            {
                                                var grazingCal = Convert.ToDecimal(((365 - Convert.ToInt32(mgovm.grazingDaysPerYear)) / 2000.0).ToString());
                                                gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.SolidPerPoundPerAnimalPerDay) * grazingCal) * ((Convert.ToDecimal(mgovm.milkProduction)) / (milkProd ?? 0M)))))) + " tons";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // manure material type is liquid
                                        if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Liquid)
                                        {
                                            if (animalSubType.LiquidPerGalPerAnimalPerDay.HasValue)
                                                gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.LiquidPerGalPerAnimalPerDay) * (365 - Convert.ToInt32(mgovm.grazingDaysPerYear)) * (1)))) + " U.S. gallons";
                                        }
                                        // manure material type is solid
                                        else if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Solid)
                                        {
                                            if (animalSubType.SolidPerPoundPerAnimalPerDay.HasValue)
                                            {
                                                var grazingCal = Convert.ToDecimal(((365 - Convert.ToInt32(mgovm.grazingDaysPerYear)) / 2000.0).ToString());
                                                gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.SolidPerPoundPerAnimalPerDay) * grazingCal) * (1))))) + " tons";
                                            }
                                        }
                                    }
                                }
                            }
                            // annual manure generation for non dairy animals
                            else
                            {
                                // manure material type is liquid
                                if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Liquid)
                                {
                                    if (animalSubType.LiquidPerGalPerAnimalPerDay.HasValue)
                                        gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.LiquidPerGalPerAnimalPerDay) * 365))) + " U.S. gallons";
                                }
                                // manure material type is solid
                                else if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Solid)
                                {
                                    if (animalSubType.SolidPerPoundPerAnimalPerDay.HasValue)
                                        gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.SolidPerPoundPerAnimalPerDay) * 365) / 2000)))) + " tons";
                                }
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
                            gm.solidPerGalPerAnimalPerDay = animalSubType.SolidPerGalPerAnimalPerDay;
                            gm.washWaterUnits = mgovm.SelWashWaterUnit;
                            gm.showBreedAndGrazingDaysPerYear = mgovm.showBreedAndGrazingDaysPerYear;
                            gm.grazingDaysPerYear = Convert.ToInt32(mgovm.grazingDaysPerYear);
                            if (mgovm.showBreedAndGrazingDaysPerYear)
                            {
                                gm.grazingDaysPerYear = Convert.ToInt32(mgovm.grazingDaysPerYear);
                                List<SelectListItem> breedDetails = _sd.GetBreed(Convert.ToInt32(mgovm.selBreedOption));
                                gm.BreedId = breedDetails[0].Id;
                                gm.BreedName = breedDetails[0].Value;
                            }

                            if (mgovm.washWater != null)
                            {
                                gm.washWater = Convert.ToDecimal(mgovm.washWater);
                            }
                            else
                            {
                                gm.washWater = 0;
                            }

                            if (mgovm.breedOptions.Count() > 0)
                            {
                                var defaultMilkProd =
                                    calculateAnimalRequirement.GetDefaultMilkProductionBySubTypeId(
                                        Convert.ToInt16(mgovm.selSubTypeOption));
                                var breedManureFactor = calculateAnimalRequirement.GetBreedManureFactorByBreedId(Convert.ToInt32(mgovm.selBreedOption));
                                var milkProd = defaultMilkProd * breedManureFactor;

                                // annual manure generation for non milking cows
                                if (mgovm.milkProduction == null)
                                {
                                    // manure material type is liquid
                                    if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Liquid)
                                    {
                                        if (animalSubType.LiquidPerGalPerAnimalPerDay.HasValue)
                                            gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.LiquidPerGalPerAnimalPerDay) * (365 - Convert.ToInt32(mgovm.grazingDaysPerYear)) * breedManureFactor ?? 0M))) + " U.S. gallons";
                                    }
                                    // manure material type is solid
                                    else if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Solid)
                                    {
                                        if (animalSubType.SolidPerPoundPerAnimalPerDay.HasValue)
                                        {
                                            var grazingCal = Convert.ToDecimal(((365 - Convert.ToInt32(mgovm.grazingDaysPerYear)) / 2000.0).ToString());
                                            gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.SolidPerPoundPerAnimalPerDay) * grazingCal) * breedManureFactor ?? 0M)))) + " tons";
                                        }
                                    }
                                }
                                // annual manure generation for milking cows
                                else
                                {
                                    if (mgovm.milkProduction != Math.Round(milkProd ?? 0M, 1).ToString())
                                    {
                                        // manure material type is liquid
                                        if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Liquid)
                                        {
                                            if (animalSubType.LiquidPerGalPerAnimalPerDay.HasValue)
                                                gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.LiquidPerGalPerAnimalPerDay) * (365 - Convert.ToInt32(mgovm.grazingDaysPerYear)) * ((Convert.ToDecimal(mgovm.milkProduction)) / (milkProd ?? 0M))))) + " U.S. gallons";
                                        }
                                        // manure material type is solid
                                        else if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Solid)
                                        {
                                            if (animalSubType.SolidPerPoundPerAnimalPerDay.HasValue)
                                            {
                                                var grazingCal = Convert.ToDecimal(((365 - Convert.ToInt32(mgovm.grazingDaysPerYear)) / 2000.0).ToString());
                                                gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.SolidPerPoundPerAnimalPerDay) * grazingCal) * ((Convert.ToDecimal(mgovm.milkProduction)) / (milkProd ?? 0M)))))) + " tons";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // manure material type is liquid
                                        if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Liquid)
                                        {
                                            if (animalSubType.LiquidPerGalPerAnimalPerDay.HasValue)
                                                gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.LiquidPerGalPerAnimalPerDay) * (365 - Convert.ToInt32(mgovm.grazingDaysPerYear)) * (1)))) + " U.S. gallons";
                                        }
                                        // manure material type is solid
                                        else if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Solid)
                                        {
                                            if (animalSubType.SolidPerPoundPerAnimalPerDay.HasValue)
                                            {
                                                var grazingCal = Convert.ToDecimal(((365 - Convert.ToInt32(mgovm.grazingDaysPerYear)) / 2000.0).ToString());
                                                gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.SolidPerPoundPerAnimalPerDay) * grazingCal) * (1))))) + " tons";
                                            }
                                        }
                                    }
                                }
                            }
                            // annual manure generation for non dairy animals
                            else
                            {
                                // manure material type is liquid
                                if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Liquid)
                                {
                                    if (animalSubType.LiquidPerGalPerAnimalPerDay.HasValue)
                                        gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.LiquidPerGalPerAnimalPerDay) * 365))) + " U.S. gallons";
                                }
                                // manure material type is solid
                                else if (mgovm.selManureMaterialTypeOption == ManureMaterialType.Solid)
                                {
                                    if (animalSubType.SolidPerPoundPerAnimalPerDay.HasValue)
                                        gm.annualAmount = string.Format("{0:#,##0}", (Math.Round(((Convert.ToInt32(mgovm.averageAnimalNumber) * Convert.ToDecimal(animalSubType.SolidPerPoundPerAnimalPerDay) * 365) / 2000)))) + " tons";
                                }
                            }

                            _ud.UpdateGeneratedManures(gm);
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

        private void setAnimalFromGeneratedManureList(ref ManureGeneratedObtainedDetailViewModel mgovm)
        {
            List<GeneratedManure> generatedManures = _ud.GetGeneratedManures();
            if (generatedManures.Count() > 0)
            {
                mgovm.selAnimalTypeOption = generatedManures[generatedManures.Count - 1].animalId.ToString();
            }
        }

        private void animalTypeDetailsSetup(ref ManureGeneratedObtainedDetailViewModel mgovm)
        {
            mgovm.showWashWater = false;
            mgovm.showMilkProduction = false;
            mgovm.showBreedAndGrazingDaysPerYear = false;
            CalculateAnimalRequirement calculateAnimalRequirement = new CalculateAnimalRequirement(_ud, _sd);

            mgovm.animalTypeOptions = new List<SelectListItem>();
            mgovm.animalTypeOptions = _sd.GetAnimalTypesDll().ToList();

            mgovm.subTypeOptions = new List<SelectListItem>();
            mgovm.breedOptions = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(mgovm.selAnimalTypeOption) &&
                mgovm.selAnimalTypeOption != "select animal")
            {
                mgovm.subTypeOptions = _sd.GetSubtypesDll(Convert.ToInt32(mgovm.selAnimalTypeOption)).ToList();
                if (mgovm.subTypeOptions.Count() > 1)
                {
                    mgovm.subTypeOptions.Insert(0, new SelectListItem() { Id = 0, Value = "select subtype" });
                }

                mgovm.breedOptions = _sd.GetBreedsDll(Convert.ToInt32(mgovm.selAnimalTypeOption)).ToList();
                if (mgovm.breedOptions.Count() > 0)
                {
                    mgovm.showBreedAndGrazingDaysPerYear = true;
                }

                if (mgovm.subTypeOptions.Count() == 1)
                {
                    mgovm.selSubTypeOption = mgovm.subTypeOptions[0].Id.ToString();

                    AnimalSubType animalSubType = _sd.GetAnimalSubType(Convert.ToInt32(mgovm.selSubTypeOption));

                    mgovm.liquidPerGalPerAnimalPerDay = animalSubType.LiquidPerGalPerAnimalPerDay.ToString();
                    mgovm.solidPerPoundPerAnimalPerDay = animalSubType.SolidPerPoundPerAnimalPerDay.ToString();

                    if (mgovm.liquidPerGalPerAnimalPerDay != "0.0000" && mgovm.solidPerPoundPerAnimalPerDay == "0.0000")
                    {
                        mgovm.selManureMaterialTypeOption = ManureMaterialType.Liquid;
                        mgovm.stdManureMaterialType = false;
                        mgovm.hasLiquidManureType = true;
                    }
                    else if (mgovm.solidPerPoundPerAnimalPerDay != "0.0000" && mgovm.liquidPerGalPerAnimalPerDay == "0.0000")
                    {
                        mgovm.selManureMaterialTypeOption = ManureMaterialType.Solid;
                        mgovm.stdManureMaterialType = false;
                        mgovm.hasSolidManureType = true;
                    }
                }

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

        #endregion Manure Generated Obtained

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
            msvm.ZeroManagedManuresMessage = _sd.GetUserPrompt("NoMaterialsForStorage");
            msvm.ExplainMaterialsOctoberToMarch = _sd.GetUserPrompt("MaterialsOctoberToMarchMessage");
            msvm.ExplainRunoffOctoberToMarch = _sd.GetUserPrompt("RunoffOctoberToMarchMessage");
            msvm.ExplainStoredOctoberToMarch = _sd.GetUserPrompt("MaterialsStoredOctoberToMarchMessage");
            msvm.ExplainPrecipitationOctoberToMarch = _sd.GetUserPrompt("PrecipitationOctoberToMarch");
            msvm.ExplainStorageDeficiency = _sd.GetUserPrompt("StorageDeficiencyMessage");
            msvm.ExplainPercentageOfLiquidSeparation = _sd.GetUserPrompt("PercentageLiquidVolumeSeparatedMessage");

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
                    var selectedMaterialsToInclude = savedStorageSystem.MaterialsIncludedInSystem.Select(m => m.ManureId).ToList();
                    msvm.ManagedManures = GetFilteredMaterialsListForCurrentView(msvm, selectedMaterialsToInclude);
                    msvm.GetsRunoffFromRoofsOrYards = savedStorageSystem.GetsRunoffFromRoofsOrYards;
                    msvm.RunoffAreaSquareFeet = savedStorageSystem.RunoffAreaSquareFeet;
                    msvm.IsThereSolidLiquidSeparation = savedStorageSystem.IsThereSolidLiquidSeparation;
                    msvm.PercentageOfLiquidVolumeSeparated = savedStorageSystem.PercentageOfLiquidVolumeSeparated;
                    msvm.SeparatedLiquidsUSGallons = savedStorageSystem.SeparatedLiquidsUSGallons;
                    msvm.SeparatedSolidsTons = savedStorageSystem.SeparatedSolidsTons;
                    msvm.volumeOfStorageSystem = savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons);
                    msvm.volumeUSGallonsOfStorageSystem = savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons).Value.ToString("N0") + " U.S. Gallons (" + savedStorageSystem.Name + ")";
                    if (msvm.SelectedManureMaterialType == ManureMaterialType.Liquid)
                    {
                        msvm.OctoberToMarchSeparatedLiquidUSGallons = savedStorageSystem.OctoberToMarchSeparatedLiquidsUSGallons;
                        msvm.OctoberToMarchRunoff = savedStorageSystem.OctoberToMarchRunoff;
                        msvm.OctoberToMarchPrecipitationStorageSystem = savedStorageSystem.OctoberToMarchPrecipitation;
                        msvm.OctoberToMarchManagedManures = savedStorageSystem.OctoberToMarchManagedManures;
                        msvm.TotalStored = savedStorageSystem.TotalStored;
                    }

                    if (structureId.HasValue)
                    {
                        var manureStorageStructure =
                            savedStorageSystem.ManureStorageStructures.Single(mss => mss.Id == structureId);
                        msvm.StorageStructureId = manureStorageStructure.Id;
                        msvm.StorageStructureName = manureStorageStructure.Name;
                        msvm.UncoveredAreaOfStorageStructure = manureStorageStructure.UncoveredAreaSquareFeet;
                        msvm.IsStructureCovered = manureStorageStructure.IsStructureCovered;
                        if (msvm.SelectedManureMaterialType == ManureMaterialType.Liquid)
                        {
                            msvm.SelectedStorageShape = manureStorageStructure.SelectedStorageStructureShape;
                            if (msvm.SelectedStorageShape == StorageShapes.Rectangular)
                            {
                                msvm.RectangularLength = Convert.ToDecimal(Convert.ToDecimal(manureStorageStructure.RectangularLength).ToString("G29"));
                                msvm.RectangularWidth = Convert.ToDecimal(Convert.ToDecimal(manureStorageStructure.RectangularWidth).ToString("G29"));
                                msvm.RectangularHeight = Convert.ToDecimal(Convert.ToDecimal(manureStorageStructure.RectangularHeight).ToString("G29"));
                            }
                            else if (msvm.SelectedStorageShape == StorageShapes.Circular)
                            {
                                msvm.CircularDiameter = Convert.ToDecimal(Convert.ToDecimal(manureStorageStructure.CircularDiameter).ToString("G29"));
                                msvm.CircularHeight = Convert.ToDecimal(Convert.ToDecimal(manureStorageStructure.CircularHeight).ToString("G29"));
                            }
                            else if (msvm.SelectedStorageShape == StorageShapes.SlopedWallRectangular)
                            {
                                msvm.SlopedWallTopLength = Convert.ToDecimal(Convert.ToDecimal(manureStorageStructure.SlopedWallTopLength).ToString("G29"));
                                msvm.SlopedWallTopWidth = Convert.ToDecimal(Convert.ToDecimal(manureStorageStructure.SlopedWallTopWidth).ToString("G29"));
                                msvm.SlopedWallHeight = Convert.ToDecimal(Convert.ToDecimal(manureStorageStructure.SlopedWallHeight).ToString("G29"));
                                msvm.SlopedWallSlopeOfWall = Convert.ToDecimal(Convert.ToDecimal(manureStorageStructure.SlopedWallSlopeOfWall).ToString("G29"));
                            }
                            msvm.surfaceArea = manureStorageStructure.surfaceArea;
                            msvm.volumeUSGallons = manureStorageStructure.volumeUSGallons;
                            msvm.volumeOfStorageStructure = manureStorageStructure.volumeOfStorageStructure;
                        }
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
                        systemTitle = "Storage Details";
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
                msdvm.ExplainMaterialsOctoberToMarch = _sd.GetUserPrompt("MaterialsOctoberToMarchMessage");
                msdvm.ExplainRunoffOctoberToMarch = _sd.GetUserPrompt("RunoffOctoberToMarchMessage");
                msdvm.ExplainStoredOctoberToMarch = _sd.GetUserPrompt("MaterialsStoredOctoberToMarchMessage");
                msdvm.ExplainPrecipitationOctoberToMarch = _sd.GetUserPrompt("PrecipitationOctoberToMarch");
                msdvm.ExplainStorageDeficiency = _sd.GetUserPrompt("StorageDeficiencyMessage");
                msdvm.ExplainPercentageOfLiquidSeparation = _sd.GetUserPrompt("PercentageLiquidVolumeSeparatedMessage");

                msdvm.ManagedManures = GetFilteredMaterialsListForCurrentView(msdvm);
                if (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid)
                {
                    msdvm.OctoberToMarchManagedManures =
                        GetOctoberToMarchMaterialVolumes(msdvm, msdvm.SelectedMaterialsToInclude, true, true);

                    if (!msdvm.IsStructureCovered)
                    {
                        msdvm.TotalStored = 0;
                        if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                        {
                            if (msdvm.IsThereSolidLiquidSeparation)
                            {
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                            }
                            else
                            {
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                            }
                        }
                        if (msdvm.OctoberToMarchPrecipitationStorageSystem != 0)
                            msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchPrecipitationStorageSystem);
                        if (msdvm.OctoberToMarchRunoff != 0)
                            msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                    }
                    else
                    {
                        msdvm.TotalStored = 0;
                        if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                        {
                            if (msdvm.IsThereSolidLiquidSeparation)
                            {
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                            }
                            else
                            {
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                            }
                        }
                        if (msdvm.OctoberToMarchRunoff != 0)
                            msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                    }
                }

                if (msdvm.ManagedManures == null || !msdvm.ManagedManures.Any())
                {
                    ModelState.AddModelError("SelectedMaterialsToInclude", "No materials of this type have been added.  Return to Manure generated or imported pages to add materials to store.");
                }

                var farmData = _ud.FarmDetails();
                if (farmData != null)
                {
                    SubRegion subregion = _sd.GetSubRegion(farmData.farmSubRegion);
                    msdvm.AnnualPrecipitation = subregion?.AnnualPrecipitation;
                }

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

                    if (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid)
                    {
                        msdvm.ManagedManures = GetFilteredMaterialsListForCurrentView(msdvm, msdvm.SelectedMaterialsToInclude, true);
                        msdvm.OctoberToMarchManagedManures =
                            GetOctoberToMarchMaterialVolumes(msdvm, msdvm.SelectedMaterialsToInclude, true, false);

                        if (!msdvm.IsStructureCovered)
                        {
                            msdvm.TotalStored = 0;
                            if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                            {
                                if (msdvm.IsThereSolidLiquidSeparation)
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                                }
                                else
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                                }
                            }
                            if (msdvm.OctoberToMarchPrecipitationStorageSystem != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchPrecipitationStorageSystem);
                            if (msdvm.OctoberToMarchRunoff != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                        }
                        else
                        {
                            msdvm.TotalStored = 0;
                            if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                            {
                                if (msdvm.IsThereSolidLiquidSeparation)
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                                }
                                else
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                                }
                            }
                            if (msdvm.OctoberToMarchPrecipitationStorageSystem != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchPrecipitationStorageSystem);
                            if (msdvm.OctoberToMarchRunoff != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                        }
                    }

                    return View(msdvm);
                }

                if (msdvm.ButtonPressed == "SelectedMaterialsToIncludeChange" || msdvm.ButtonPressed == "SystemNameChange")
                {
                    ModelState.Clear();
                    msdvm.ButtonPressed = "";
                    msdvm.ButtonText = "Save";

                    msdvm = GetSeparatedManure(msdvm);
                    msdvm = StorageShapesCalculations(msdvm);

                    if (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid)
                    {
                        msdvm.OctoberToMarchManagedManures =
                            GetOctoberToMarchMaterialVolumes(msdvm, msdvm.SelectedMaterialsToInclude, true, false);
                        msdvm.OctoberToMarchSeparatedLiquidUSGallons = GetOctoberToMarchSeparatedManure(msdvm, msdvm.SelectedMaterialsToInclude, true, false);

                        if (!msdvm.IsStructureCovered)
                        {
                            msdvm.TotalStored = 0;
                            if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                            {
                                if (msdvm.IsThereSolidLiquidSeparation)
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                                }
                                else
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                                }
                            }
                            if (msdvm.OctoberToMarchPrecipitationStorageSystem != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchPrecipitationStorageSystem);
                            if (msdvm.OctoberToMarchRunoff != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                        }
                        else
                        {
                            msdvm.TotalStored = 0;
                            if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                            {
                                if (msdvm.IsThereSolidLiquidSeparation)
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                                }
                                else
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                                }
                            }
                            if (msdvm.OctoberToMarchRunoff != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                        }
                    }

                    return View(msdvm);
                }

                if (msdvm.ButtonPressed == "GetsRunoffFromRoofsOrYardsChange")
                {
                    ModelState.Clear();
                    msdvm.ButtonPressed = "";
                    msdvm.ButtonText = "Save";

                    if (!msdvm.GetsRunoffFromRoofsOrYards)
                    {
                        msdvm.RunoffAreaSquareFeet = null;
                    }

                    msdvm = GetSeparatedManure(msdvm);
                    msdvm = StorageShapesCalculations(msdvm);
                    if (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid)
                    {
                        msdvm.OctoberToMarchManagedManures =
                            GetOctoberToMarchMaterialVolumes(msdvm, msdvm.SelectedMaterialsToInclude, true, false);
                        msdvm.OctoberToMarchSeparatedLiquidUSGallons = GetOctoberToMarchSeparatedManure(msdvm, msdvm.SelectedMaterialsToInclude, true, false);
                        msdvm.OctoberToMarchRunoff = GetRunoffToStorageOctoberToMarch(msdvm);
                    }

                    return View(msdvm);
                }

                if (msdvm.ButtonPressed == "IsThereSolidLiquidSeparationChange")
                {
                    ModelState.Clear();
                    msdvm.ButtonPressed = string.Empty;
                    msdvm.ButtonText = "Save";

                    if (!msdvm.IsThereSolidLiquidSeparation)
                    {
                        msdvm.ShowSeparatedValueFields = false;
                        msdvm.PercentageOfLiquidVolumeSeparated = 0;
                    }
                    else
                    {
                        msdvm.OctoberToMarchManagedManures =
                            GetOctoberToMarchMaterialVolumes(msdvm, msdvm.SelectedMaterialsToInclude, true, false);
                        msdvm.PercentageOfLiquidVolumeSeparated = _sd.GetLiquidSolidSeparationDefaults().PercentOfLiquidSeparation;
                        msdvm = GetSeparatedManure(msdvm);
                        msdvm.OctoberToMarchSeparatedLiquidUSGallons = GetOctoberToMarchSeparatedManure(msdvm, msdvm.SelectedMaterialsToInclude, true, false);
                    }

                    msdvm = StorageShapesCalculations(msdvm);

                    if (!msdvm.IsStructureCovered)
                    {
                        msdvm.TotalStored = 0;
                        if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                        {
                            if (msdvm.IsThereSolidLiquidSeparation)
                            {
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                            }
                            else
                            {
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                            }
                        }
                        if (msdvm.OctoberToMarchPrecipitationStorageSystem != 0)
                            msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchPrecipitationStorageSystem);
                        if (msdvm.OctoberToMarchRunoff != 0)
                            msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                    }
                    else
                    {
                        msdvm.TotalStored = 0;
                        if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                        {
                            if (msdvm.IsThereSolidLiquidSeparation)
                            {
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                            }
                            else
                            {
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                            }
                        }
                        if (msdvm.OctoberToMarchRunoff != 0)
                            msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                    }

                    return View(msdvm);
                }

                if (msdvm.ButtonPressed == "RefreshSolidLiquidVolumeFields")
                {
                    ModelState.Clear();
                    msdvm.ButtonPressed = "";
                    msdvm.ButtonText = "Save";

                    msdvm = GetSeparatedManure(msdvm);
                    msdvm = StorageShapesCalculations(msdvm);
                    if (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid)
                    {
                        msdvm.OctoberToMarchManagedManures =
                            GetOctoberToMarchMaterialVolumes(msdvm, msdvm.SelectedMaterialsToInclude, true, false);
                        msdvm.OctoberToMarchSeparatedLiquidUSGallons = GetOctoberToMarchSeparatedManure(msdvm, msdvm.SelectedMaterialsToInclude, true, false);
                        msdvm.OctoberToMarchPrecipitation = GetPrecipitationOctoberToMarch(msdvm);
                        msdvm.OctoberToMarchRunoff = GetRunoffToStorageOctoberToMarch(msdvm);

                        if (!msdvm.IsStructureCovered)
                        {
                            msdvm.TotalStored = 0;
                            if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                            {
                                if (msdvm.IsThereSolidLiquidSeparation)
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                                }
                                else
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                                }
                            }
                            if (msdvm.OctoberToMarchPrecipitationStorageSystem != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchPrecipitationStorageSystem);
                            if (msdvm.OctoberToMarchRunoff != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                        }
                        else
                        {
                            msdvm.TotalStored = 0;
                            if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                            {
                                if (msdvm.IsThereSolidLiquidSeparation)
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                                }
                                else
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                                }
                            }
                            if (msdvm.OctoberToMarchRunoff != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                        }
                    }

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

                    msdvm = GetSeparatedManure(msdvm);
                    msdvm = StorageShapesCalculations(msdvm);
                    if (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid)
                    {
                        msdvm.OctoberToMarchManagedManures =
                            GetOctoberToMarchMaterialVolumes(msdvm, msdvm.SelectedMaterialsToInclude, true, false);
                        msdvm.OctoberToMarchSeparatedLiquidUSGallons = GetOctoberToMarchSeparatedManure(msdvm, msdvm.SelectedMaterialsToInclude, true, false);

                        if (!msdvm.IsStructureCovered)
                        {
                            msdvm.TotalStored = 0;
                            if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                            {
                                if (msdvm.IsThereSolidLiquidSeparation)
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                                }
                                else
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                                }
                            }
                            if (msdvm.OctoberToMarchPrecipitationStorageSystem != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchPrecipitationStorageSystem);
                            if (msdvm.OctoberToMarchRunoff != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                        }
                        else
                        {
                            msdvm.TotalStored = 0;
                            if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                            {
                                if (msdvm.IsThereSolidLiquidSeparation)
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                                }
                                else
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                                }
                            }
                            if (msdvm.OctoberToMarchRunoff != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                        }
                    }

                    return View(msdvm);
                }

                if (msdvm.ButtonPressed == "StorageShapeChange")
                {
                    ModelState.Clear();
                    msdvm.ButtonPressed = "";
                    msdvm.ButtonText = "Save";

                    if (msdvm.SystemId != null)
                    {
                        var savedStorageSystem = _ud.GetStorageSystem(msdvm.SystemId ?? 0);
                        msdvm.volumeUSGallonsOfStorageSystem = savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons).Value.ToString("N0") + " U.S. Gallons (" + savedStorageSystem.Name + ")";
                    }
                    if (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid)
                    {
                        msdvm.OctoberToMarchManagedManures =
                            GetOctoberToMarchMaterialVolumes(msdvm, msdvm.SelectedMaterialsToInclude, true, false);
                        msdvm.OctoberToMarchSeparatedLiquidUSGallons = GetOctoberToMarchSeparatedManure(msdvm, msdvm.SelectedMaterialsToInclude, true, false);

                        if (!msdvm.IsStructureCovered)
                        {
                            msdvm.TotalStored = 0;
                            if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                            {
                                if (msdvm.IsThereSolidLiquidSeparation)
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                                }
                                else
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                                }
                            }
                            if (msdvm.OctoberToMarchPrecipitationStorageSystem != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchPrecipitationStorageSystem);
                            if (msdvm.OctoberToMarchRunoff != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                        }
                        else
                        {
                            msdvm.TotalStored = 0;
                            if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                            {
                                if (msdvm.IsThereSolidLiquidSeparation)
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                                }
                                else
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                                }
                            }
                            if (msdvm.OctoberToMarchRunoff != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                        }
                    }

                    return View(msdvm);
                }

                if (msdvm.ButtonPressed == "StorageShapeMeasurementsChange")
                {
                    ModelState.Clear();
                    msdvm.ButtonPressed = "";
                    msdvm.ButtonText = "Save";

                    msdvm = StorageShapesCalculations(msdvm);
                    if (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid)
                    {
                        msdvm.OctoberToMarchManagedManures =
                            GetOctoberToMarchMaterialVolumes(msdvm, msdvm.SelectedMaterialsToInclude, true, false);
                        msdvm.OctoberToMarchSeparatedLiquidUSGallons = GetOctoberToMarchSeparatedManure(msdvm, msdvm.SelectedMaterialsToInclude, true, false);

                        if (!msdvm.IsStructureCovered)
                        {
                            msdvm.TotalStored = 0;
                            if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                            {
                                if (msdvm.IsThereSolidLiquidSeparation)
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                                }
                                else
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                                }
                            }
                            if (msdvm.OctoberToMarchPrecipitationStorageSystem != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchPrecipitationStorageSystem);
                            if (msdvm.OctoberToMarchRunoff != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                        }
                        else
                        {
                            msdvm.TotalStored = 0;
                            if (msdvm.OctoberToMarchManagedManures != 0 || msdvm.OctoberToMarchSeparatedLiquidUSGallons != 0)
                            {
                                if (msdvm.IsThereSolidLiquidSeparation)
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchSeparatedLiquidUSGallons);
                                }
                                else
                                {
                                    msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchManagedManures);
                                }
                            }
                            if (msdvm.OctoberToMarchRunoff != 0)
                                msdvm.TotalStored += Convert.ToDecimal(msdvm.OctoberToMarchRunoff);
                        }
                    }

                    return View(msdvm);
                }

                if (msdvm.ButtonText == "Save" || msdvm.SystemId.HasValue)
                {
                    ModelState.Clear();
                    var existingNames = _ud.GetStorageSystems()
                        .Where(im => !msdvm.SystemId.HasValue || (msdvm.SystemId.HasValue && im.Id != msdvm.SystemId))
                        .Select(im => im.Name).ToList();
                    if (existingNames.Any(n => n.Trim().Equals(msdvm.SystemName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        ModelState.AddModelError("SystemName", "Use a new name");
                    }

                    if (!msdvm.DisableSystemFields)
                    {
                        if (msdvm.SelectedManureMaterialType == 0)
                        {
                            ModelState.AddModelError("SelectedManureMaterialType", "Required");
                        }

                        //Turning off now that Storage can be empty
                        //if (msdvm.ManagedManures != null && msdvm.ManagedManures.Any() &&
                        //    msdvm.SelectedMaterialsToInclude != null && !msdvm.SelectedMaterialsToInclude.Any())
                        //{
                        //    ModelState.AddModelError("SelectedMaterialsToInclude", "Required");
                        //}

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
                            !msdvm.UncoveredAreaOfStorageStructure.HasValue && msdvm.SelectedManureMaterialType == ManureMaterialType.Solid)
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

                    if (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid)
                    {
                        if (msdvm.SelectedStorageShape == StorageShapes.Rectangular)
                        {
                            if (string.IsNullOrWhiteSpace(msdvm.RectangularLength.ToString()))
                            {
                                ModelState.AddModelError("RectangularLength", "Required");
                            }

                            if (string.IsNullOrWhiteSpace(msdvm.RectangularHeight.ToString()))
                            {
                                ModelState.AddModelError("RectangularHeight", "Required");
                            }

                            if (string.IsNullOrWhiteSpace(msdvm.RectangularWidth.ToString()))
                            {
                                ModelState.AddModelError("RectangularWidth", "Required");
                            }
                        }
                        else if (msdvm.SelectedStorageShape == StorageShapes.Circular)
                        {
                            if (string.IsNullOrWhiteSpace(msdvm.CircularDiameter.ToString()))
                            {
                                ModelState.AddModelError("CircularDiameter", "Required");
                            }

                            if (string.IsNullOrWhiteSpace(msdvm.CircularHeight.ToString()))
                            {
                                ModelState.AddModelError("CircularHeight", "Required");
                            }
                        }
                        else if (msdvm.SelectedStorageShape == StorageShapes.SlopedWallRectangular)
                        {
                            if (string.IsNullOrWhiteSpace(msdvm.SlopedWallTopLength.ToString()))
                            {
                                ModelState.AddModelError("SlopedWallTopLength", "Required");
                            }

                            if (string.IsNullOrWhiteSpace(msdvm.SlopedWallTopWidth.ToString()))
                            {
                                ModelState.AddModelError("SlopedWallTopWidth", "Required");
                            }

                            if (string.IsNullOrWhiteSpace(msdvm.SlopedWallHeight.ToString()))
                            {
                                ModelState.AddModelError("SlopedWallHeight", "Required");
                            }

                            if (string.IsNullOrWhiteSpace(msdvm.SlopedWallSlopeOfWall.ToString()))
                            {
                                ModelState.AddModelError("SlopedWallSlopeOfWall", "Required");
                            }
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        if (msdvm.SelectedStorageShape == StorageShapes.Rectangular)
                        {
                            if (msdvm.RectangularLength != null && msdvm.RectangularWidth != null &&
                                msdvm.RectangularHeight != null)
                            {
                                msdvm.surfaceArea = _storageVolumeCalculator.GetSurfaceAreaOfRectangle(
                                    msdvm.RectangularLength,
                                    msdvm.RectangularWidth, msdvm.RectangularHeight);
                                msdvm.volumeUSGallons = _storageVolumeCalculator.GetVolumeUSGallonsOfRectangle(
                                    msdvm.RectangularLength,
                                    msdvm.RectangularWidth, msdvm.RectangularHeight);
                            }
                            msdvm.UncoveredAreaOfStorageStructure = msdvm.surfaceArea;
                            msdvm.volumeUSGallons = msdvm.volumeUSGallons;
                            msdvm.volumeOfStorageStructure =
                                msdvm.volumeUSGallons.Value.ToString("N0") + " U.S. Gallons (" + msdvm.StorageStructureName + ")";

                            if (msdvm.IsStructureCovered)
                            {
                                msdvm.UncoveredAreaOfStorageStructure = null;
                            }
                            else
                            {
                                msdvm = GetSeparatedManure(msdvm);
                            }
                        }
                        else if (msdvm.SelectedStorageShape == StorageShapes.Circular)
                        {
                            if (msdvm.CircularDiameter != null && msdvm.CircularHeight != null)
                            {
                                msdvm.surfaceArea = _storageVolumeCalculator.GetSurfaceAreaOfCircle(msdvm.CircularDiameter);
                                msdvm.volumeUSGallons = _storageVolumeCalculator.GetVolumeUSGallonsOfCircle(msdvm.CircularDiameter, msdvm.CircularHeight);
                            }
                            if (!msdvm.IsStructureCovered)
                                msdvm.UncoveredAreaOfStorageStructure = msdvm.surfaceArea;
                            msdvm.volumeUSGallons = msdvm.volumeUSGallons;
                            msdvm.volumeOfStorageStructure =
                                msdvm.volumeUSGallons.Value.ToString("N0") + " U.S. Gallons (" + msdvm.StorageStructureName + ")";

                            if (msdvm.IsStructureCovered)
                            {
                                msdvm.UncoveredAreaOfStorageStructure = null;
                            }
                            else
                            {
                                msdvm = GetSeparatedManure(msdvm);
                            }
                        }
                        else if (msdvm.SelectedStorageShape == StorageShapes.SlopedWallRectangular)
                        {
                            if (msdvm.SlopedWallTopLength != null && msdvm.SlopedWallTopWidth != null && msdvm.SlopedWallHeight != null && msdvm.SlopedWallSlopeOfWall != null)
                            {
                                msdvm.surfaceArea = _storageVolumeCalculator.GetSurfaceAreaOfSlopedWall(msdvm.SlopedWallTopLength, msdvm.SlopedWallTopWidth);
                                msdvm.volumeUSGallons = _storageVolumeCalculator.GetVolumeUSGallonsOfSlopedWall(msdvm.SlopedWallTopLength, msdvm.SlopedWallTopWidth, msdvm.SlopedWallHeight, msdvm.SlopedWallSlopeOfWall);
                            }
                            if (!msdvm.IsStructureCovered)
                                msdvm.UncoveredAreaOfStorageStructure = msdvm.surfaceArea;
                            msdvm.volumeUSGallons = msdvm.volumeUSGallons;
                            msdvm.volumeOfStorageStructure =
                                msdvm.volumeUSGallons.Value.ToString("N0") + " U.S. Gallons (" + msdvm.StorageStructureName + ")";

                            if (msdvm.IsStructureCovered)
                            {
                                msdvm.UncoveredAreaOfStorageStructure = null;
                            }
                            else
                            {
                                msdvm = GetSeparatedManure(msdvm);
                            }
                        }

                        var manureStorageSystem = PopulateManureStorageSystem(msdvm);
                        manureStorageSystem.ManureStorageVolume = manureStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons).Value.ToString("N0") + " U.S. Gallons (" + manureStorageSystem.Name + ")";
                        manureStorageSystem.TotalStored = msdvm.TotalStored;

                        if (msdvm.SystemId.HasValue)
                        {
                            _ud.UpdateManureStorageSystem(manureStorageSystem);
                        }
                        else
                        {
                            _ud.AddManureStorageSystem(manureStorageSystem);
                            msdvm.SystemId = manureStorageSystem.Id;
                        }

                        _ud.UpdateManagedManuresAllocationToStorage();

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

        private double GetPrecipitationOctoberToMarch(ManureStorageDetailViewModel msdvm)
        {
            var rainfallOctToMar = 0;
            var farmData = _ud.FarmDetails();

            if (farmData != null)
            {
                SubRegion subregion = _sd.GetSubRegion(farmData.farmSubRegion);
                rainfallOctToMar = subregion.AnnualPrecipitationOctToMar;
            }

            var precipitation = Math.Round(Convert.ToDouble(rainfallOctToMar * msdvm.UncoveredAreaOfStorageStructure * 0.0245424));
            return precipitation;
        }

        private double GetRunoffToStorageOctoberToMarch(ManureStorageDetailViewModel msdvm)
        {
            var rainfallOctToMar = 0;
            var farmData = _ud.FarmDetails();
            if (farmData != null)
            {
                SubRegion subregion = _sd.GetSubRegion(farmData.farmSubRegion);
                rainfallOctToMar = subregion.AnnualPrecipitationOctToMar;
            }

            var runoff = Math.Round(Convert.ToDouble(rainfallOctToMar * msdvm.RunoffAreaSquareFeet * 0.0245424));
            return runoff;
        }

        private ManureStorageSystem PopulateManureStorageSystem(ManureStorageDetailViewModel msdvm)
        {
            ManureStorageSystem manureStorageSystem;

            if (msdvm.SystemId.HasValue)
            {
                manureStorageSystem = _ud.GetStorageSystem(msdvm.SystemId.Value);
            }
            else
            {
                manureStorageSystem = new ManureStorageSystem();
            }

            var includedManures = _ud.GetAllManagedManures().Where(gm =>
                            msdvm.SelectedMaterialsToInclude.Any(includedIds => gm.ManureId == includedIds)).ToList();
            includedManures.ForEach(m => { m.AssignedToStoredSystem = true; });

            manureStorageSystem.Name = msdvm.SystemName;
            manureStorageSystem.ManureMaterialType = msdvm.SelectedManureMaterialType;
            manureStorageSystem.GeneratedManuresIncludedInSystem = includedManures.Where(m => m is GeneratedManure).Cast<GeneratedManure>().ToList();
            manureStorageSystem.ImportedManuresIncludedInSystem = includedManures.Where(m => m is ImportedManure).Cast<ImportedManure>().ToList();
            manureStorageSystem.SeparatedSolidManuresIncludedInSystem = includedManures.Where(m => m is SeparatedSolidManure).Cast<SeparatedSolidManure>().ToList();
            manureStorageSystem.GetsRunoffFromRoofsOrYards = msdvm.GetsRunoffFromRoofsOrYards;
            manureStorageSystem.RunoffAreaSquareFeet = msdvm.RunoffAreaSquareFeet;
            manureStorageSystem.IsThereSolidLiquidSeparation = msdvm.IsThereSolidLiquidSeparation;
            manureStorageSystem.PercentageOfLiquidVolumeSeparated = msdvm.PercentageOfLiquidVolumeSeparated;
            manureStorageSystem.SeparatedSolidsTons = msdvm.SeparatedSolidsTons;
            manureStorageSystem.SeparatedLiquidsUSGallons = msdvm.SeparatedLiquidsUSGallons;
            manureStorageSystem.AnnualPrecipitation = msdvm.AnnualPrecipitation;

            if (manureStorageSystem.ManureStorageStructures.Count() > 0)
            {
                manureStorageSystem.ManureStorageVolume = manureStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons).ToString();
                msdvm.volumeOfStorageSystem = manureStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons);
                msdvm.volumeUSGallonsOfStorageSystem = manureStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons).Value.ToString("N0") + " U.S. Gallons (" + manureStorageSystem.Name + ")";
            }
            if (manureStorageSystem.ManureMaterialType == ManureMaterialType.Liquid)
            {
                manureStorageSystem.OctoberToMarchSeparatedLiquidsUSGallons = msdvm.OctoberToMarchSeparatedLiquidUSGallons;
                if (!msdvm.IsStructureCovered)
                {
                    manureStorageSystem.OctoberToMarchRunoff = msdvm.OctoberToMarchRunoff;
                    manureStorageSystem.OctoberToMarchPrecipitation = msdvm.OctoberToMarchPrecipitationStorageSystem;
                }
                manureStorageSystem.OctoberToMarchManagedManures = msdvm.OctoberToMarchManagedManures;
            }

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

                if (manureStorageSystem.ManureMaterialType == ManureMaterialType.Liquid)
                {
                    storageStructure.OctoberToMarchPrecipitation = msdvm.OctoberToMarchPrecipitation;
                    storageStructure.SelectedStorageStructureShape = msdvm.SelectedStorageShape;
                    if (msdvm.SelectedStorageShape == StorageShapes.Rectangular)
                    {
                        storageStructure.RectangularLength = msdvm.RectangularLength;
                        storageStructure.RectangularWidth = msdvm.RectangularWidth;
                        storageStructure.RectangularHeight = msdvm.RectangularHeight;

                        if (msdvm.RectangularLength != null && msdvm.RectangularWidth != null &&
                            msdvm.RectangularHeight != null)
                        {
                            msdvm.surfaceArea = _storageVolumeCalculator.GetSurfaceAreaOfRectangle(
                                msdvm.RectangularLength,
                                msdvm.RectangularWidth, msdvm.RectangularHeight);
                            msdvm.volumeUSGallons = _storageVolumeCalculator.GetVolumeUSGallonsOfRectangle(
                                msdvm.RectangularLength,
                                msdvm.RectangularWidth, msdvm.RectangularHeight);
                        }
                    }
                    else if (msdvm.SelectedStorageShape == StorageShapes.Circular)
                    {
                        storageStructure.CircularDiameter = msdvm.CircularDiameter;
                        storageStructure.CircularHeight = msdvm.CircularHeight;
                        if (msdvm.CircularDiameter != null && msdvm.CircularHeight != null)
                        {
                            msdvm.surfaceArea =
                                _storageVolumeCalculator.GetSurfaceAreaOfCircle(msdvm.CircularDiameter);
                            msdvm.volumeUSGallons =
                                _storageVolumeCalculator.GetVolumeUSGallonsOfCircle(msdvm.CircularDiameter,
                                    msdvm.CircularHeight);

                            if (!msdvm.IsStructureCovered)
                                msdvm.UncoveredAreaOfStorageStructure = msdvm.surfaceArea;
                            msdvm.volumeUSGallons = msdvm.volumeUSGallons;
                            msdvm.volumeOfStorageStructure =
                                msdvm.volumeUSGallons.Value.ToString("N0") + " U.S. Gallons (" +
                                msdvm.StorageStructureName + ")";
                        }
                    }
                    else if (msdvm.SelectedStorageShape == StorageShapes.SlopedWallRectangular)
                    {
                        storageStructure.SlopedWallTopLength = msdvm.SlopedWallTopLength;
                        storageStructure.SlopedWallTopWidth = msdvm.SlopedWallTopWidth;
                        storageStructure.SlopedWallHeight = msdvm.SlopedWallHeight;
                        storageStructure.SlopedWallSlopeOfWall = msdvm.SlopedWallSlopeOfWall;
                        if (msdvm.SlopedWallTopLength != null && msdvm.SlopedWallTopWidth != null &&
                            msdvm.SlopedWallHeight != null && msdvm.SlopedWallSlopeOfWall != null)
                        {
                            msdvm.surfaceArea =
                                _storageVolumeCalculator.GetSurfaceAreaOfSlopedWall(msdvm.SlopedWallTopLength,
                                    msdvm.SlopedWallTopWidth);
                            msdvm.volumeUSGallons =
                                _storageVolumeCalculator.GetVolumeUSGallonsOfSlopedWall(msdvm.SlopedWallTopLength,
                                    msdvm.SlopedWallTopWidth, msdvm.SlopedWallHeight, msdvm.SlopedWallSlopeOfWall);
                        }
                    }
                    if (msdvm.SelectedStorageShape != 0)
                    {
                        if (!msdvm.IsStructureCovered)
                            msdvm.UncoveredAreaOfStorageStructure = msdvm.surfaceArea;
                        storageStructure.surfaceArea = msdvm.surfaceArea;
                        storageStructure.volumeUSGallons = msdvm.volumeUSGallons;
                        if (msdvm.volumeUSGallons.HasValue && !string.IsNullOrEmpty(msdvm.StorageStructureName))
                        {
                            storageStructure.volumeOfStorageStructure =
                                msdvm.volumeUSGallons.Value.ToString("N0") + " U.S. Gallons (" + msdvm.StorageStructureName + ")";
                        }
                    }
                }

                if (msdvm.StorageStructureName != null)
                {
                    storageStructure.Name = msdvm.StorageStructureName;
                }

                if (msdvm.UncoveredAreaOfStorageStructure.HasValue)
                {
                    storageStructure.UncoveredAreaSquareFeet = msdvm.UncoveredAreaOfStorageStructure;
                }
                else
                {
                    storageStructure.UncoveredAreaSquareFeet = null;
                }

                if (!msdvm.StorageStructureId.HasValue)
                {
                    manureStorageSystem.AddUpdateManureStorageStructure(storageStructure);
                }
            }

            return manureStorageSystem;
        }

        private List<MvcRendering.SelectListItem> GetFilteredMaterialsListForCurrentView(ManureStorageDetailViewModel msdvm)
        {
            return GetFilteredMaterialsListForCurrentView(msdvm, msdvm.SelectedMaterialsToInclude);
        }

        private List<MvcRendering.SelectListItem> GetFilteredMaterialsListForCurrentView(ManureStorageDetailViewModel msdvm, List<string> selectedMaterials)
        {
            return GetFilteredMaterialsListForCurrentView(msdvm, selectedMaterials, false);
        }

        private List<MvcRendering.SelectListItem> GetFilteredMaterialsListForCurrentView(ManureStorageDetailViewModel msdvm, List<string> selectedMaterials, bool selectAllLiquidMaterial)
        {
            if (msdvm.SelectedManureMaterialType > 0)
            {
                var selectedManuresToInclude = selectedMaterials.ToList();
                //Materials already allocated
                if (msdvm.SystemId.HasValue)
                {
                    selectedManuresToInclude.AddRange(_ud.GetStorageSystems()
                                                                        .Single(ss => ss.Id == msdvm.SystemId).MaterialsIncludedInSystem
                                                                        .Select(m => m.ManureId).ToList());
                    selectedManuresToInclude = selectedManuresToInclude.GroupBy(s => s).Select(m => m.First()).ToList();
                }

                //Materials accounted in another system
                var materialIdsToExclude = new List<string>();

                foreach (var manureStorageSystem in _ud.GetStorageSystems())
                {
                    var accountedFor =
                        manureStorageSystem.MaterialsIncludedInSystem.Where(m =>
                            selectedManuresToInclude.All(include => include != m.ManureId)).Select(s => s.ManureId);
                    materialIdsToExclude.AddRange(accountedFor);
                }

                var managedManures = _ud.GetAllManagedManures()
                    .Where(g => (g is GeneratedManure || (g is ImportedManure && (g as ImportedManure).IsMaterialStored) || g is SeparatedSolidManure) &&
                                        (
                                            (msdvm.SelectedManureMaterialType == ManureMaterialType.Solid && g.ManureType == ManureMaterialType.Solid)
                                            ||
                                            (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid && (g.ManureType == ManureMaterialType.Liquid || g.ManureType == ManureMaterialType.Solid))
                                        )
                                       && !materialIdsToExclude.Any(exclude => g.Id.HasValue && g.ManureId == exclude));

                var manureSelectItems = new List<MvcRendering.SelectListItem>();

                if (selectAllLiquidMaterial)
                {
                    var unselectedLiquid = managedManures
                        .Where(mm =>
                            mm.ManureType == ManureMaterialType.Liquid && !selectedMaterials.Any(s =>
                                s.Equals(mm.ManureId, StringComparison.CurrentCultureIgnoreCase)))
                        .Select(m => m.ManureId);

                    selectedMaterials.AddRange(unselectedLiquid);
                }

                foreach (var manure in managedManures)
                {
                    var materialsToInclude = "";
                    if (manure.ManureId.Contains("Generated"))
                    {
                        var manureGenerated = _ud.GetGeneratedManure(manure.Id.GetValueOrDefault());
                        materialsToInclude = $"{manureGenerated.animalSubTypeName}({manureGenerated.averageAnimalNumber} animals), {manureGenerated.manureTypeName}";
                    }
                    else if (manure.ManureId.Contains("Imported"))
                    {
                        var manureImported = _ud.GetImportedManure(manure.Id.GetValueOrDefault());
                        materialsToInclude = $"{manureImported.MaterialName} ({manureImported.ManureTypeName})";
                    }
                    else
                    {
                        var manureSeparated = _ud.GetSeparatedManure(manure.Id.GetValueOrDefault());
                        materialsToInclude = manureSeparated.Name;
                    }
                    manureSelectItems.Add(new MvcRendering.SelectListItem
                    {
                        Value = manure.ManureId.ToString(),
                        Text = materialsToInclude,
                        Selected = selectedMaterials.Any(sm => sm == manure.ManureId)
                    });
                }

                return manureSelectItems;
            }

            return null;
        }

        private ManureStorageDetailViewModel StorageShapesCalculations(ManureStorageDetailViewModel msdvm)
        {
            if (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid)
            {
                if (msdvm.SelectedStorageShape == StorageShapes.Rectangular)
                {
                    if (msdvm.RectangularLength != null && msdvm.RectangularWidth != null &&
                        msdvm.RectangularHeight != null)
                    {
                        msdvm.surfaceArea = _storageVolumeCalculator.GetSurfaceAreaOfRectangle(
                            msdvm.RectangularLength,
                            msdvm.RectangularWidth, msdvm.RectangularHeight);
                        msdvm.volumeUSGallons = _storageVolumeCalculator.GetVolumeUSGallonsOfRectangle(
                            msdvm.RectangularLength,
                            msdvm.RectangularWidth, msdvm.RectangularHeight);

                        if (!msdvm.IsStructureCovered)
                            msdvm.UncoveredAreaOfStorageStructure = msdvm.surfaceArea;
                        msdvm.volumeUSGallons = msdvm.volumeUSGallons;
                        msdvm.volumeOfStorageStructure =
                            msdvm.volumeUSGallons.Value.ToString("N0") + " U.S. Gallons (" +
                            msdvm.StorageStructureName + ")";

                        if (msdvm.IsStructureCovered)
                        {
                            msdvm.UncoveredAreaOfStorageStructure = null;
                        }
                        else
                        {
                            msdvm = GetSeparatedManure(msdvm);
                        }

                        if (msdvm.SystemId != null)
                        {
                            var savedStorageSystem = _ud.GetStorageSystem(msdvm.SystemId ?? 0);

                            ManureStorageStructure storageStructure;
                            if (msdvm.StorageStructureId.HasValue)
                            {
                                storageStructure =
                                    savedStorageSystem.ManureStorageStructures.Single(mss =>
                                        mss.Id == msdvm.StorageStructureId);
                                if (storageStructure != null)
                                {
                                    storageStructure.surfaceArea = msdvm.surfaceArea;
                                    storageStructure.volumeUSGallons = msdvm.volumeUSGallons;
                                }
                                msdvm.volumeOfStorageSystem =
                                    savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons);
                                msdvm.volumeUSGallonsOfStorageSystem =
                                    (savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons).Value)
                                    .ToString("N0") + " U.S. Gallons (" + savedStorageSystem.Name + ")";
                            }
                            else
                            {
                                msdvm.volumeOfStorageSystem = savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons) + msdvm.volumeUSGallons.Value;
                                msdvm.volumeUSGallonsOfStorageSystem =
                                    (savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons).Value + msdvm.volumeUSGallons.Value)
                                    .ToString("N0") + " U.S. Gallons (" + savedStorageSystem.Name + ")";
                            }
                        }
                        else
                        {
                            msdvm.volumeOfStorageSystem = msdvm.volumeUSGallons.Value;
                            msdvm.volumeUSGallonsOfStorageSystem =
                                msdvm.volumeUSGallons.Value.ToString("N0") + " U.S. Gallons (" + msdvm.SystemName + ")";
                        }
                    }
                }
                else if (msdvm.SelectedStorageShape == StorageShapes.Circular)
                {
                    if (msdvm.CircularDiameter != null && msdvm.CircularHeight != null)
                    {
                        msdvm.surfaceArea =
                            _storageVolumeCalculator.GetSurfaceAreaOfCircle(msdvm.CircularDiameter);
                        msdvm.volumeUSGallons =
                            _storageVolumeCalculator.GetVolumeUSGallonsOfCircle(msdvm.CircularDiameter,
                                msdvm.CircularHeight);
                        if (!msdvm.IsStructureCovered)
                            msdvm.UncoveredAreaOfStorageStructure = msdvm.surfaceArea;
                        msdvm.volumeUSGallons = msdvm.volumeUSGallons;
                        msdvm.volumeOfStorageStructure =
                            msdvm.volumeUSGallons.Value.ToString("N0") + " U.S. Gallons (" +
                            msdvm.StorageStructureName + ")";

                        if (msdvm.IsStructureCovered)
                        {
                            msdvm.UncoveredAreaOfStorageStructure = null;
                        }
                        else
                        {
                            msdvm = GetSeparatedManure(msdvm);
                        }

                        if (msdvm.SystemId != null)
                        {
                            var savedStorageSystem = _ud.GetStorageSystem(msdvm.SystemId ?? 0);

                            ManureStorageStructure storageStructure;
                            if (msdvm.StorageStructureId.HasValue)
                            {
                                storageStructure =
                                    savedStorageSystem.ManureStorageStructures.Single(mss =>
                                        mss.Id == msdvm.StorageStructureId);
                                if (storageStructure != null)
                                {
                                    storageStructure.surfaceArea = msdvm.surfaceArea;
                                    storageStructure.volumeUSGallons = msdvm.volumeUSGallons;
                                }
                                msdvm.volumeOfStorageSystem =
                                    savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons);
                                msdvm.volumeUSGallonsOfStorageSystem =
                                    (savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons).Value)
                                    .ToString("N0") + " U.S. Gallons (" + savedStorageSystem.Name + ")";
                            }
                            else
                            {
                                msdvm.volumeOfStorageSystem = savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons) + msdvm.volumeUSGallons.Value;
                                msdvm.volumeUSGallonsOfStorageSystem =
                                    (savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons).Value + msdvm.volumeUSGallons.Value)
                                    .ToString("N0") + " U.S. Gallons (" + savedStorageSystem.Name + ")";
                            }
                        }
                        else
                        {
                            msdvm.volumeOfStorageSystem = msdvm.volumeUSGallons.Value;
                            msdvm.volumeUSGallonsOfStorageSystem =
                                msdvm.volumeUSGallons.Value.ToString("N0") + " U.S. Gallons (" + msdvm.SystemName + ")";
                        }
                    }
                }
                else if (msdvm.SelectedStorageShape == StorageShapes.SlopedWallRectangular)
                {
                    if (msdvm.SlopedWallTopLength != null && msdvm.SlopedWallTopWidth != null &&
                        msdvm.SlopedWallHeight != null && msdvm.SlopedWallSlopeOfWall != null)
                    {
                        msdvm.surfaceArea =
                            _storageVolumeCalculator.GetSurfaceAreaOfSlopedWall(msdvm.SlopedWallTopLength,
                                msdvm.SlopedWallTopWidth);
                        msdvm.volumeUSGallons =
                            _storageVolumeCalculator.GetVolumeUSGallonsOfSlopedWall(msdvm.SlopedWallTopLength,
                                msdvm.SlopedWallTopWidth, msdvm.SlopedWallHeight, msdvm.SlopedWallSlopeOfWall);

                        if (!msdvm.IsStructureCovered)
                            msdvm.UncoveredAreaOfStorageStructure = msdvm.surfaceArea;
                        msdvm.volumeUSGallons = msdvm.volumeUSGallons;
                        msdvm.volumeOfStorageStructure =
                            msdvm.volumeUSGallons.Value.ToString("N0") + " U.S. Gallons (" +
                            msdvm.StorageStructureName + ")";

                        if (msdvm.IsStructureCovered)
                        {
                            msdvm.UncoveredAreaOfStorageStructure = null;
                        }
                        else
                        {
                            msdvm = GetSeparatedManure(msdvm);
                        }

                        if (msdvm.SystemId != null)
                        {
                            var savedStorageSystem = _ud.GetStorageSystem(msdvm.SystemId ?? 0);

                            ManureStorageStructure storageStructure;
                            if (msdvm.StorageStructureId.HasValue)
                            {
                                storageStructure =
                                    savedStorageSystem.ManureStorageStructures.Single(mss =>
                                        mss.Id == msdvm.StorageStructureId);
                                if (storageStructure != null)
                                {
                                    storageStructure.surfaceArea = msdvm.surfaceArea;
                                    storageStructure.volumeUSGallons = msdvm.volumeUSGallons;
                                }
                                msdvm.volumeOfStorageSystem =
                                    savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons);
                                msdvm.volumeUSGallonsOfStorageSystem =
                                    (savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons).Value)
                                    .ToString("N0") + " U.S. Gallons (" + savedStorageSystem.Name + ")";
                            }
                            else
                            {
                                msdvm.volumeOfStorageSystem = savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons) + msdvm.volumeUSGallons.Value;
                                msdvm.volumeUSGallonsOfStorageSystem =
                                    (savedStorageSystem.ManureStorageStructures.Sum(ss => ss.volumeUSGallons).Value + msdvm.volumeUSGallons.Value)
                                    .ToString("N0") + " U.S. Gallons (" + savedStorageSystem.Name + ")";
                            }
                        }
                        else
                        {
                            msdvm.volumeOfStorageSystem = msdvm.volumeUSGallons.Value;
                            msdvm.volumeUSGallonsOfStorageSystem =
                                msdvm.volumeUSGallons.Value.ToString("N0") + " U.S. Gallons (" + msdvm.SystemName + ")";
                        }
                    }
                }

                if (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid && !msdvm.IsStructureCovered)
                {
                    var savedStorageSystem = _ud.GetStorageSystem(msdvm.SystemId ?? 0);
                    if (savedStorageSystem != null)
                    {
                        msdvm.OctoberToMarchPrecipitationStorageSystem =
                            savedStorageSystem.ManureStorageStructures.Sum(ss => ss.OctoberToMarchPrecipitation) + GetPrecipitationOctoberToMarch(msdvm);
                    }
                    else
                    {
                        msdvm.OctoberToMarchPrecipitationStorageSystem = GetPrecipitationOctoberToMarch(msdvm);
                    }

                    msdvm.OctoberToMarchPrecipitation = GetPrecipitationOctoberToMarch(msdvm);
                }
            }

            return msdvm;
        }

        private decimal GetOctoberToMarchMaterialVolumes(ManureStorageDetailViewModel msdvm, List<string> selectedMaterials, bool selectAllLiquidMaterial, bool includeAllMaterials)
        {
            if (msdvm.SelectedManureMaterialType > 0)
            {
                var selectedManuresToInclude = selectedMaterials.ToList();
                //Materials already allocated
                if (msdvm.SystemId.HasValue)
                {
                    selectedManuresToInclude.AddRange(_ud.GetStorageSystems()
                                                                        .Single(ss => ss.Id == msdvm.SystemId).MaterialsIncludedInSystem
                                                                        .Select(m => m.ManureId).ToList());
                    selectedManuresToInclude = selectedManuresToInclude.GroupBy(s => s).Select(m => m.First()).ToList();
                }

                //Materials accounted in another system
                var materialIdsToExclude = new List<string>();

                foreach (var manureStorageSystem in _ud.GetStorageSystems())
                {
                    var accountedFor =
                        manureStorageSystem.MaterialsIncludedInSystem.Where(m =>
                            selectedManuresToInclude.All(include => include != m.ManureId)).Select(s => s.ManureId);
                    materialIdsToExclude.AddRange(accountedFor);
                }

                var managedManures = _ud.GetAllManagedManures()
                    .Where(g => (g is GeneratedManure || (g is ImportedManure && (g as ImportedManure).IsMaterialStored) || g is SeparatedSolidManure) &&
                                        (
                                            (msdvm.SelectedManureMaterialType == ManureMaterialType.Solid && g.ManureType == ManureMaterialType.Solid)
                                            ||
                                            (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid && (g.ManureType == ManureMaterialType.Liquid || g.ManureType == ManureMaterialType.Solid))
                                        )
                                       && !materialIdsToExclude.Any(exclude => g.Id.HasValue && g.ManureId == exclude));

                var manureSelectItems = new List<MvcRendering.SelectListItem>();

                if (selectAllLiquidMaterial)
                {
                    var unselectedLiquid = managedManures
                        .Where(mm =>
                            mm.ManureType == ManureMaterialType.Liquid && !selectedMaterials.Any(s =>
                                s.Equals(mm.ManureId, StringComparison.CurrentCultureIgnoreCase)))
                        .Select(m => m.ManureId);

                    selectedMaterials.AddRange(unselectedLiquid);
                }

                var materialVolumes = 0m;
                foreach (var manure in managedManures)
                {
                    if (includeAllMaterials == true)
                    {
                        if (manure.ManureId.Contains("Generated"))
                        {
                            var manureGenerated = _ud.GetGeneratedManure(manure.Id.GetValueOrDefault());
                            materialVolumes += manureGenerated.annualAmountDecimal;
                            if (manureGenerated.washWaterGallons != 0)
                            {
                                materialVolumes += manureGenerated.washWaterGallons;
                            }
                        }
                        else if (manure.ManureId.Contains("Imported"))
                        {
                            var manureImported = _ud.GetImportedManure(manure.Id.GetValueOrDefault());
                            materialVolumes += manureImported.AnnualAmount;
                        }
                    }
                    else
                    {
                        if (selectedManuresToInclude.Contains(manure.ManureId))
                        {
                            if (manure.ManureId.Contains("Generated"))
                            {
                                var manureGenerated = _ud.GetGeneratedManure(manure.Id.GetValueOrDefault());
                                materialVolumes += manureGenerated.annualAmountDecimal;
                                if (manureGenerated.washWaterGallons != 0)
                                {
                                    materialVolumes += manureGenerated.washWaterGallons;
                                }
                            }
                            else if (manure.ManureId.Contains("Imported"))
                            {
                                var manureImported = _ud.GetImportedManure(manure.Id.GetValueOrDefault());
                                materialVolumes += manureImported.AnnualAmount;
                            }
                        }
                    }
                }

                return (materialVolumes / 365) * 182;
            }

            return 0m;
        }

        private ManureStorageDetailViewModel GetSeparatedManure(ManureStorageDetailViewModel msdvm)
        {
            var result = msdvm;

            if (msdvm.IsThereSolidLiquidSeparation)
            {
                var manureStorageSystem = PopulateManureStorageSystem(msdvm);

                //Calculate Separation
                if (manureStorageSystem.AnnualTotalStoredGeneratedManure > 0 || manureStorageSystem.AnnualTotalImportedManure > 0)
                {
                    var separatedManure =
                        _manureLiquidSolidSeparationCalculator.CalculateSeparatedManure(
                            (manureStorageSystem.AnnualTotalStoredGeneratedManure + manureStorageSystem.AnnualTotalImportedManure),
                            manureStorageSystem.PercentageOfLiquidVolumeSeparated);

                    result.SeparatedLiquidsUSGallons = separatedManure.LiquidUSGallons;
                    result.SeparatedSolidsTons = separatedManure.SolidTons;
                }
            }

            return result;
        }

        private decimal GetOctoberToMarchSeparatedManure(ManureStorageDetailViewModel msdvm, List<string> selectedMaterials, bool selectAllLiquidMaterial, bool includeAllManagedManures)
        {
            if (msdvm.SelectedManureMaterialType > 0)
            {
                var selectedManuresToInclude = selectedMaterials.ToList();
                //Materials already allocated
                if (msdvm.SystemId.HasValue)
                {
                    selectedManuresToInclude.AddRange(_ud.GetStorageSystems()
                        .Single(ss => ss.Id == msdvm.SystemId).MaterialsIncludedInSystem
                        .Select(m => m.ManureId).ToList());
                    selectedManuresToInclude = selectedManuresToInclude.GroupBy(s => s).Select(m => m.First()).ToList();
                }

                //Materials accounted in another system
                var materialIdsToExclude = new List<string>();

                foreach (var manureStorageSystem in _ud.GetStorageSystems())
                {
                    var accountedFor =
                        manureStorageSystem.MaterialsIncludedInSystem.Where(m =>
                            selectedManuresToInclude.All(include => include != m.ManureId)).Select(s => s.ManureId);
                    materialIdsToExclude.AddRange(accountedFor);
                }

                var managedManures = _ud.GetAllManagedManures()
                    .Where(g => (g is GeneratedManure ||
                                 (g is ImportedManure && (g as ImportedManure).IsMaterialStored) ||
                                 g is SeparatedSolidManure) &&
                                (
                                    (msdvm.SelectedManureMaterialType == ManureMaterialType.Solid &&
                                     g.ManureType == ManureMaterialType.Solid)
                                    ||
                                    (msdvm.SelectedManureMaterialType == ManureMaterialType.Liquid &&
                                     (g.ManureType == ManureMaterialType.Liquid ||
                                      g.ManureType == ManureMaterialType.Solid))
                                )
                                && !materialIdsToExclude.Any(exclude => g.Id.HasValue && g.ManureId == exclude));

                var manureSelectItems = new List<MvcRendering.SelectListItem>();

                if (selectAllLiquidMaterial)
                {
                    var unselectedLiquid = managedManures
                        .Where(mm =>
                            mm.ManureType == ManureMaterialType.Liquid && !selectedMaterials.Any(s =>
                                s.Equals(mm.ManureId, StringComparison.CurrentCultureIgnoreCase)))
                        .Select(m => m.ManureId);

                    selectedMaterials.AddRange(unselectedLiquid);
                }

                var materialVolumes = 0m;
                var washWater = 0m;
                foreach (var manure in managedManures)
                {
                    if (includeAllManagedManures)
                    {
                        if (manure.ManureId.Contains("Generated"))
                        {
                            var manureGenerated = _ud.GetGeneratedManure(manure.Id.GetValueOrDefault());
                            materialVolumes += manureGenerated.annualAmountDecimal;
                            if (manureGenerated.washWaterGallons != 0)
                            {
                                materialVolumes += manureGenerated.washWaterGallons;
                            }
                        }
                        else if (manure.ManureId.Contains("Imported"))
                        {
                            var manureImported = _ud.GetImportedManure(manure.Id.GetValueOrDefault());
                            materialVolumes += manureImported.AnnualAmount;
                        }
                    }
                    else
                    {
                        if (msdvm.SelectedMaterialsToInclude.Contains(manure.ManureId))
                        {
                            if (manure.ManureId.Contains("Generated"))
                            {
                                var manureGenerated = _ud.GetGeneratedManure(manure.Id.GetValueOrDefault());
                                materialVolumes += manureGenerated.annualAmountDecimal;
                                if (manureGenerated.washWaterGallons != 0)
                                {
                                    materialVolumes += manureGenerated.washWaterGallons;
                                }
                            }
                            else if (manure.ManureId.Contains("Imported"))
                            {
                                var manureImported = _ud.GetImportedManure(manure.Id.GetValueOrDefault());
                                materialVolumes += manureImported.AnnualAmount;
                            }
                        }
                    }
                }

                if (msdvm.IsThereSolidLiquidSeparation && msdvm.PercentageOfLiquidVolumeSeparated != 0)
                {
                    materialVolumes = (1 - 1M / msdvm.PercentageOfLiquidVolumeSeparated) * (materialVolumes / 365) * 182;
                }
                else
                {
                    materialVolumes = (materialVolumes / 365) * 182;
                }

                return materialVolumes;
            }

            return 0;
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
                    _ud.UpdateManagedManuresAllocationToStorage();
                }

                string url = Url.Action("RefreshStorageList", "ManureManagement");
                return Json(new { success = true, url = url, target = vm.Target });
            }

            return PartialView("ManureStorageDelete", vm);
        }

        #endregion Manure Storage

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
            mvm.ExplainNutrientAnalysisMoisture = _sd.GetUserPrompt("NutrientAnalysisMoistureMessage");
            mvm.ExplainNutrientAnalysisNitrogen = _sd.GetUserPrompt("NutrientAnlalysisNitrogenMessage");
            mvm.ExplainNutrientAnlalysisAmmonia = _sd.GetUserPrompt("NutrientAnlalysisAmmoniaMessage");
            mvm.ExplainNutrientAnlalysisPhosphorous = _sd.GetUserPrompt("NutrientAnlalysisPhosphorousMessage");
            mvm.ExplainNutrientAnlalysisPotassium = _sd.GetUserPrompt("NutrientAnlalysisPotassiumMessage");

            mvm.act = id == null ? "Add" : "Edit";
            mvm.url = _sd.GetExternalLink("labanalysisexplanation");
            mvm.urlText = _sd.GetUserPrompt("moreinfo");

            if (id != null)
            {
                FarmManure fm = _ud.GetFarmManure(id.Value);

                if (!_ud.FarmData().NMPReleaseVersion.HasValue ||
                    _ud.FarmData().NMPReleaseVersion.Value != _appSettings.Value.NMPReleaseVersion)
                {
                    mvm.IsLegacyNMPReleaseVersion = true;
                    mvm.LegacyNMPReleaseVersionManureId = fm.manureId;
                }

                mvm.selsourceOfMaterialOption = fm.sourceOfMaterialId;
                mvm.stored_imported = fm.stored_imported;
                mvm.IsAssignedToStorage = fm.IsAssignedToStorage;
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
                mvm.sourceOfMaterialName = "  ";
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
            cvm.sourceOfMaterialOptions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

            foreach (var storageSystem in storageSystems)
            {
                if (storageSystem.MaterialsIncludedInSystem.Count() != 0)
                {
                    var li = new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                    { Text = "StorageSystem" + "," + storageSystem.Id, Value = storageSystem.Name };
                    cvm.sourceOfMaterialOptions.Add(li);
                }
            }

            // add imported materials that are not being stored to the list of Material Sources
            var importedManures = _ud.GetImportedManures();
            var importedManuresNotStored = new List<ImportedManure>();
            foreach (var importedManure in importedManures)
            {
                if (importedManure.AssignedToStoredSystem == false)
                {
                    importedManuresNotStored.Add(importedManure);
                }
            }

            foreach (var imns in importedManuresNotStored)
            {
                var li = new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                { Text = "Imported" + "," + imns.Id, Value = imns.MaterialName };
                cvm.sourceOfMaterialOptions.Add(li);
            }

            cvm.manOptions = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(cvm.selsourceOfMaterialOption) &&
                cvm.selsourceOfMaterialOption != "select")
            {
                var manures = _sd.GetManures();

                if (cvm.selsourceOfMaterialOption.ToString().Split(",")[0].Contains("StorageSystem"))
                {
                    var storageSystem = _ud.GetStorageSystem(Convert.ToInt32(cvm.selsourceOfMaterialOption.ToString().Split(",")[1]));
                    var manuresByMaterialTypes = from manure in manures where manure.SolidLiquid == (storageSystem.ManureMaterialType).ToString() orderby manure.SortNum, manure.Name select manure;
                    cvm.materialType = storageSystem.ManureMaterialType;
                    foreach (var manuresByMaterialType in manuresByMaterialTypes)
                    {
                        var li = new SelectListItem()
                        { Id = manuresByMaterialType.Id, Value = manuresByMaterialType.Name };
                        cvm.manOptions.Add(li);
                        cvm.stored_imported = NutrientAnalysisTypes.Stored;
                    }
                }
                else if (cvm.selsourceOfMaterialOption.ToString().Split(",")[0].Contains("Imported"))
                {
                    var importedManure = _ud.GetImportedManure(Convert.ToInt32(cvm.selsourceOfMaterialOption.ToString().Split(",")[1]));

                    var manuresByMaterialTypes = from manure in manures where manure.SolidLiquid == (importedManure.ManureType).ToString() select manure;
                    cvm.materialType = importedManure.ManureType;
                    foreach (var manuresByMaterialType in manuresByMaterialTypes)
                    {
                        var li = new SelectListItem()
                        { Id = manuresByMaterialType.Id, Value = manuresByMaterialType.Name };
                        cvm.manOptions.Add(li);
                        cvm.stored_imported = NutrientAnalysisTypes.Imported;
                    }
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

            cvm.ExplainNutrientAnalysisMoisture = _sd.GetUserPrompt("NutrientAnalysisMoistureMessage");
            cvm.ExplainNutrientAnalysisNitrogen = _sd.GetUserPrompt("NutrientAnlalysisNitrogenMessage");
            cvm.ExplainNutrientAnlalysisAmmonia = _sd.GetUserPrompt("NutrientAnlalysisAmmoniaMessage");
            cvm.ExplainNutrientAnlalysisPhosphorous = _sd.GetUserPrompt("NutrientAnlalysisPhosphorousMessage");
            cvm.ExplainNutrientAnlalysisPotassium = _sd.GetUserPrompt("NutrientAnlalysisPotassiumMessage");

            CompostDetailsSetup(ref cvm);

            try
            {
                if (cvm.buttonPressed == "SourceOfMaterialChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";

                    if (cvm.LegacyNMPReleaseVersionManureId.HasValue)
                    {
                        cvm.selManOption = cvm.LegacyNMPReleaseVersionManureId.Value;
                    }

                    cvm.manOptions = new List<SelectListItem>();

                    if (cvm.selsourceOfMaterialOption != "" && cvm.selsourceOfMaterialOption != "0" &&
                        cvm.selsourceOfMaterialOption != "select")
                    {
                        var manures = _sd.GetManures();

                        if (cvm.selsourceOfMaterialOption.ToString().Split(",")[0].Contains("StorageSystem"))
                        {
                            var storageSystem = _ud.GetStorageSystem(Convert.ToInt32(cvm.selsourceOfMaterialOption.ToString().Split(",")[1]));
                            cvm.sourceOfMaterialName = storageSystem.Name;
                            var manuresByMaterialTypes = from manure in manures where manure.SolidLiquid == (storageSystem.ManureMaterialType).ToString() orderby manure.SortNum, manure.Name select manure;
                            cvm.materialType = storageSystem.ManureMaterialType;
                            foreach (var manuresByMaterialType in manuresByMaterialTypes)
                            {
                                var li = new SelectListItem()
                                { Id = manuresByMaterialType.Id, Value = manuresByMaterialType.Name };
                                cvm.manOptions.Add(li);
                            }
                        }
                        else if (cvm.selsourceOfMaterialOption.ToString().Split(",")[0].Contains("Imported"))
                        {
                            var importedManure = _ud.GetImportedManure(Convert.ToInt32(cvm.selsourceOfMaterialOption.ToString().Split(",")[1]));
                            cvm.sourceOfMaterialName = importedManure.MaterialName;
                            var manuresByMaterialTypes = from manure in manures where manure.SolidLiquid == (importedManure.ManureType).ToString() orderby manure.SortNum, manure.Name select manure;
                            cvm.materialType = importedManure.ManureType;
                            foreach (var manuresByMaterialType in manuresByMaterialTypes)
                            {
                                var li = new SelectListItem()
                                { Id = manuresByMaterialType.Id, Value = manuresByMaterialType.Name };
                                cvm.manOptions.Add(li);
                            }
                        }
                    }

                    //For Legacy Release 1 NMP Files
                    if (cvm.IsLegacyNMPReleaseVersion)
                    {
                        cvm.buttonPressed = "ManureChange";
                    }
                    else
                    {
                        return View(cvm);
                    }
                }

                if (cvm.buttonPressed == "ManureChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";

                    if (cvm.selsourceOfMaterialOption != "select")
                    {
                        if (cvm.selsourceOfMaterialOption.ToString().Split(",")[0].Contains("StorageSystem"))
                        {
                            var storageSystem = _ud.GetStorageSystem(Convert.ToInt32(cvm.selsourceOfMaterialOption.ToString().Split(",")[1]));
                            cvm.sourceOfMaterialName = storageSystem.Name;
                        }
                        else if (cvm.selsourceOfMaterialOption.ToString().Split(",")[0].Contains("Imported"))
                        {
                            var importedManure = _ud.GetImportedManure(Convert.ToInt32(cvm.selsourceOfMaterialOption.ToString().Split(",")[1]));
                            cvm.sourceOfMaterialName = importedManure.MaterialName;
                        }
                    }

                    if (cvm.selManOption != 0)
                    {
                        man = _sd.GetManure(cvm.selManOption.ToString());
                        if (_sd.IsManureClassOtherType(man.ManureClass) ||
                           _sd.IsManureClassCompostType(man.ManureClass))
                        {
                            cvm.bookValue = false;
                            cvm.onlyCustom = true;
                            if (!cvm.IsLegacyNMPReleaseVersion)
                            {
                                cvm.nitrogen = string.Empty;
                                cvm.moisture = string.Empty;
                                cvm.ammonia = string.Empty;
                                cvm.nitrate = string.Empty;
                                cvm.phosphorous = string.Empty;
                                cvm.potassium = string.Empty;
                            }

                            cvm.compost = _sd.IsManureClassCompostType(man.ManureClass);
                            cvm.showNitrate = cvm.compost;
                            cvm.manureName = cvm.compost ? "Custom - " + man.Name + " - " : "Custom - " + man.SolidLiquid + " - ";
                        }
                        else
                        {
                            cvm.showNitrate = _sd.IsManureClassCompostClassType(man.ManureClass);
                            cvm.bookValue = !cvm.IsLegacyNMPReleaseVersion ? !cvm.showNitrate : cvm.bookValue;
                            cvm.compost = false;
                            if ((cvm.IsLegacyNMPReleaseVersion && !cvm.bookValue) || cvm.showNitrate)
                            {
                                cvm.moistureBook = man.Moisture.ToString();
                                cvm.nitrogenBook = man.Nitrogen.ToString();
                                cvm.ammoniaBook = man.Ammonia.ToString();
                                cvm.nitrateBook = man.Nitrate.ToString();
                                cvm.phosphorousBook = man.Phosphorous.ToString();
                                cvm.potassiumBook = man.Potassium.ToString();
                                cvm.nitrateBook = man.Nitrate.ToString();
                                cvm.manureName = !cvm.IsLegacyNMPReleaseVersion ? "Custom - " + man.Name + " - " : cvm.manureName;
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

                if (cvm.selsourceOfMaterialOption == "select")
                {
                    ModelState.AddModelError("selsourceOfMaterialOption", "Select a Source of Material");
                }

                if (ModelState.IsValid)
                {
                    man = _sd.GetManure(cvm.selManOption.ToString());

                    if (cvm.selsourceOfMaterialOption != "select")
                    {
                        if (cvm.selsourceOfMaterialOption.ToString().Split(",")[0].Contains("StorageSystem"))
                        {
                            var storageSystem = _ud.GetStorageSystem(Convert.ToInt32(cvm.selsourceOfMaterialOption.ToString().Split(",")[1]));
                            cvm.sourceOfMaterialName = storageSystem.Name;
                            cvm.IsAssignedToStorage = true;
                        }
                        else if (cvm.selsourceOfMaterialOption.ToString().Split(",")[0].Contains("Imported"))
                        {
                            var importedManure = _ud.GetImportedManure(Convert.ToInt32(cvm.selsourceOfMaterialOption.ToString().Split(",")[1]));

                            cvm.sourceOfMaterialName = importedManure.MaterialName;
                            cvm.IsAssignedToStorage = false;
                        }
                    }

                    var includedNutrients = _ud.GetFarmManures();
                    //    .Where(gm =>
                    //    msdvm.SelectedMaterialsToInclude.Any(includedIds => gm.ManureId == includedIds)).ToList();
                    //includedManures.ForEach(m => { m.AssignedToStoredSystem = true; });

                    if (cvm.selsourceOfMaterialOption == "select")
                    {
                        ModelState.AddModelError("selsourceOfMaterialOption", "Select a Source of Material");
                    }

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
                            fm.sourceOfMaterialId = cvm.selsourceOfMaterialOption;
                            fm.sourceOfMaterialName = cvm.sourceOfMaterialName;
                            fm.stored_imported = cvm.stored_imported;
                            fm.IsAssignedToStorage = cvm.IsAssignedToStorage;
                            fm.manureId = cvm.selManOption;
                            fm.customized = false;
                        }
                        else
                        {
                            man = _sd.GetManure(cvm.selManOption.ToString());

                            fm.customized = true;
                            fm.sourceOfMaterialId = cvm.selsourceOfMaterialOption;
                            fm.manureId = cvm.selManOption;
                            fm.ammonia = Convert.ToDecimal(cvm.ammonia);
                            fm.dmid = man.DryMatterId;
                            fm.manure_class = man.ManureClass;
                            fm.moisture = cvm.moisture;
                            fm.name = cvm.manureName;
                            fm.sourceOfMaterialName = cvm.sourceOfMaterialName;
                            fm.nitrogen = Convert.ToDecimal(cvm.nitrogen);
                            fm.nminerizationid = man.NMineralizationId;
                            fm.phosphorous = Convert.ToDecimal(cvm.phosphorous);
                            fm.potassium = Convert.ToDecimal(cvm.potassium);
                            fm.nitrate = cvm.showNitrate ? Convert.ToDecimal(cvm.nitrate) : (decimal?)null;
                            fm.solid_liquid = man.SolidLiquid;
                            fm.stored_imported = cvm.stored_imported;
                            fm.IsAssignedToStorage = cvm.IsAssignedToStorage;
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
                            fm.sourceOfMaterialId = cvm.selsourceOfMaterialOption;
                            fm.sourceOfMaterialName = cvm.sourceOfMaterialName;
                            fm.manureId = cvm.selManOption;
                            fm.customized = false;
                            fm.stored_imported = cvm.stored_imported;
                        }
                        else
                        {
                            man = _sd.GetManure(cvm.selManOption.ToString());

                            fm.customized = true;
                            fm.sourceOfMaterialId = cvm.selsourceOfMaterialOption;
                            fm.manureId = cvm.selManOption;
                            fm.ammonia = Convert.ToDecimal(cvm.ammonia);
                            fm.dmid = man.DryMatterId;
                            fm.manure_class = man.ManureClass;
                            fm.moisture = cvm.moisture;
                            fm.name = cvm.manureName;
                            fm.sourceOfMaterialName = cvm.sourceOfMaterialName;
                            fm.nitrogen = Convert.ToDecimal(cvm.nitrogen);
                            fm.nminerizationid = man.NMineralizationId;
                            fm.phosphorous = Convert.ToDecimal(cvm.phosphorous);
                            fm.potassium = Convert.ToDecimal(cvm.potassium);
                            fm.solid_liquid = man.SolidLiquid;
                            fm.nitrate = cvm.showNitrate ? Convert.ToDecimal(cvm.nitrate) : (decimal?)null;
                            fm.stored_imported = cvm.stored_imported;
                            fm.IsAssignedToStorage = cvm.IsAssignedToStorage;
                        }

                        _ud.UpdateFarmManure(fm);

                        _ud.ReCalculateManure(fm.id);
                    }

                    _ud.UpdateManagedImportedManuresAllocationToNutrientAnalysis();
                    _ud.UpdateStorageSystemsAllocationToNutrientAnalysis();

                    string url = Url.Action("RefreshCompostList", "Manure");
                    return Json(new { success = true, url = url, target = cvm.target });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error.");
                _logger.LogError(ex, "CompostDetails Exception");
            }

            return PartialView(cvm);
        }

        //private void ReCalculateManure(int id)
        //{
        //    CalculateNutrients calculateNutrients = new CalculateNutrients(_ud, _sd);
        //    NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

        //    List<Field> flds = _ud.GetFields();

        //    foreach (var fld in flds)
        //    {
        //        List<NutrientManure> mans = _ud.GetFieldNutrientsManures(fld.fieldName);

        //        foreach (var nm in mans)
        //        {
        //            if (id.ToString() == nm.manureId)
        //            {
        //                int regionid = _ud.FarmDetails().farmRegion.Value;
        //                Region region = _sd.GetRegion(regionid);
        //                nOrganicMineralizations = calculateNutrients.GetNMineralization(Convert.ToInt16(nm.manureId), region.LocationId);

        //                string avail = (nOrganicMineralizations.OrganicN_FirstYear * 100).ToString("###");

        //                string nh4 = (calculateNutrients.GetAmmoniaRetention(Convert.ToInt16(nm.manureId), Convert.ToInt16(nm.applicationId)) * 100).ToString("###");

        //                NutrientInputs nutrientInputs = new NutrientInputs();

        //                calculateNutrients.manure = nm.manureId;
        //                calculateNutrients.applicationSeason = nm.applicationId;
        //                calculateNutrients.applicationRate = Convert.ToDecimal(nm.rate);
        //                calculateNutrients.applicationRateUnits = nm.unitId;
        //                calculateNutrients.ammoniaNRetentionPct = Convert.ToDecimal(nh4);
        //                calculateNutrients.firstYearOrganicNAvailablityPct = Convert.ToDecimal(avail);

        //                calculateNutrients.GetNutrientInputs(nutrientInputs);

        //                nm.yrN = nutrientInputs.N_FirstYear;
        //                nm.yrP2o5 = nutrientInputs.P2O5_FirstYear;
        //                nm.yrK2o = nutrientInputs.K2O_FirstYear;
        //                nm.ltN = nutrientInputs.N_LongTerm;
        //                nm.ltP2o5 = nutrientInputs.P2O5_LongTerm;
        //                nm.ltK2o = nutrientInputs.K2O_LongTerm;

        //                _ud.UpdateFieldNutrientsManure(fld.fieldName, nm);
        //            }
        //        }
        //    }
        //}
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

        #endregion ManureNutrientAnalysis

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

            if (id.HasValue)
            {
                var savedImportedManure = _ud.GetImportedManure(id.Value);
                vm = _mapper.Map<ImportedManure, ManureImportedDetailViewModel>(savedImportedManure);
            }
            else
            {
                vm.StandardSolidMoisture = _sd.GetManureImportedDefault().DefaultSolidMoisture;
                vm.Moisture = vm.StandardSolidMoisture;
                vm.IsMaterialStored = true;
                vm.SelectedManureType = ManureMaterialType.Solid;
            }

            vm.Title = "Imported Material Details";
            vm.Target = target;
            vm.IsMaterialStoredLabelText = _sd.GetUserPrompt("ImportMaterialIsMaterialAppliedQuestion");

            return PartialView("ManureImportedDetail", vm);
        }

        [HttpPost]
        public IActionResult ManureImportedDetail(ManureImportedDetailViewModel vm)
        {
            try
            {
                if (vm.ButtonPressed == "MaterialNameChange")
                {
                    ModelState.Clear();
                    vm.ButtonPressed = "";
                    vm.ButtonText = "Save";

                    return PartialView("ManureImportedDetail", vm);
                }

                if (vm.ButtonPressed == "ManureMaterialTypeChange")
                {
                    ModelState.Clear();
                    vm.ButtonPressed = "";
                    vm.ButtonText = "Save";

                    if (vm.SelectedManureType == ManureMaterialType.Liquid)
                    {
                        vm.Moisture = null;
                    }

                    return PartialView("ManureImportedDetail", vm);
                }

                if (vm.ButtonPressed == "IsMaterialStoredChange")
                {
                    ModelState.Clear();
                    vm.ButtonPressed = "";
                    vm.ButtonText = "Save";
                    return PartialView("ManureImportedDetail", vm);
                }

                if (vm.ButtonPressed == "ResetMoisture")
                {
                    ModelState.Clear();
                    vm.ButtonPressed = "";
                    vm.ButtonText = "Save";

                    vm.Moisture = vm.StandardSolidMoisture;
                    return PartialView("ManureImportedDetail", vm);
                }

                var existingNames = _ud.GetImportedManures()
                    .Where(im => !vm.ManureImportId.HasValue || (vm.ManureImportId.HasValue && im.Id != vm.ManureImportId))
                    .Select(im => im.MaterialName).ToList();
                if (existingNames.Any(n => n.Trim().Equals(vm.MaterialName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    ModelState.AddModelError("MaterialName", "Use a new name");
                }

                if (vm.SelectedManureType == ManureMaterialType.Solid &&
                    (!vm.Moisture.HasValue || vm.Moisture.Value <= 0 || vm.Moisture > 100))
                {
                    ModelState.AddModelError("Moisture", "Enter a value between 0 and 100");
                }

                if (!vm.AnnualAmount.HasValue || vm.AnnualAmount < 0)
                {
                    ModelState.AddModelError("AnnualAmount", "Enter a numeric value");
                }

                if (ModelState.IsValid)
                {
                    var importedManure = _mapper.Map<ImportedManure>(vm);

                    if (vm.SelectedManureType == ManureMaterialType.Solid)
                    {
                        importedManure.AnnualAmountCubicMetersVolume =
                            _manureUnitConversionCalculator.GetCubicMetersVolume(importedManure.ManureType,
                                importedManure.Moisture.Value,
                                importedManure.AnnualAmount,
                                importedManure.Units);

                        importedManure.AnnualAmountCubicYardsVolume =
                            _manureUnitConversionCalculator.GetCubicYardsVolume(importedManure.ManureType,
                                importedManure.Moisture.Value,
                                importedManure.AnnualAmount,
                                importedManure.Units);

                        importedManure.AnnualAmountTonsWeight =
                            _manureUnitConversionCalculator.GetTonsWeight(importedManure.ManureType,
                                importedManure.Moisture.Value,
                                importedManure.AnnualAmount,
                                importedManure.Units);
                    }
                    else
                    {
                        importedManure.AnnualAmountUSGallonsVolume =
                            _manureUnitConversionCalculator.GetUSGallonsVolume(importedManure.ManureType,
                                importedManure.AnnualAmount,
                                importedManure.Units);
                    }

                    if (!vm.ManureImportId.HasValue)
                    {
                        _ud.AddImportedManure(importedManure);
                        vm.ManureImportId = importedManure.Id;
                    }
                    else
                    {
                        _ud.UpdateImportedManure(importedManure);
                    }

                    var url = Url.Action("RefreshImportList", "ManureManagement");
                    return Json(new { success = true, url = url, target = vm.Target });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error -" + ex.Message);
            }

            return PartialView("ManureImportedDetail", vm);
        }

        public IActionResult RefreshImportList()
        {
            return ViewComponent("ManureImported");
        }

        [HttpGet]
        public IActionResult ManureImportedDelete(int id, string target)
        {
            var vm = new ManureImportedDeleteViewModel();
            var manure = _ud.GetImportedManure(id);

            vm.Title = "Delete";
            vm.Target = target;
            vm.ImportManureName = manure.ManagedManureName;
            vm.ImportedManureId = id;
            vm.AppliedToAField = false;

            if (_ud.GetYearData().GetFieldsAppliedWithManure(manure).Any())
            {
                vm.AppliedToAField = true;
                vm.DeleteWarningForUnstorableMaterial = _sd.GetUserPrompt("ImportMaterialNotStoredDeleteWarning");
            }

            return PartialView("ManureImportedDelete", vm);
        }

        [HttpPost]
        public IActionResult ManureImportedDelete(ManureImportedDeleteViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _ud.DeleteImportedManure(vm.ImportedManureId);

                string url = Url.Action("RefreshImportList", "ManureManagement");
                return Json(new { success = true, url = url, target = vm.Target });
            }

            return PartialView("ManureImportedDelete", vm);
        }

        #endregion ManureImported
    }
}