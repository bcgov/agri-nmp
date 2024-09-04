using Agri.CalculateService;
using Agri.Data;
using Agri.Models.Calculate;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Agri.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SERVERAPI.Filters;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Agri.Models;

namespace SERVERAPI.Controllers
{
    [SessionTimeout]
    public class NutrientsController : BaseController
    {
        private readonly ILogger<NutrientsController> _logger;
        private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;
        private readonly Fertigation _fg;
        private readonly AppSettings _settings;
        private readonly ICalculateCropRequirementRemoval _calculateCropRequirementRemoval;
        private readonly ICalculateFertilizerNutrients _calculateFertilizerNutrients;
        private readonly ICalculateFertigationNutrients _calculateFertigationNutrients;
        private readonly ICalculateNutrients _calculateNutrients;
        private readonly IChemicalBalanceMessage _chemicalBalanceMessage;
        private readonly IManureApplicationCalculator _manureApplicationCalculator;
        // private readonly FertigationData;

        public NutrientsController(ILogger<NutrientsController> logger,
            UserData ud,
            IOptions<AppSettings> settings,
            IAgriConfigurationRepository sd,
            ICalculateCropRequirementRemoval calculateCropRequirementRemoval,
            ICalculateFertilizerNutrients calculateFertilizerNutrients,
            ICalculateFertigationNutrients calculateFertigationNutrients,
            ICalculateNutrients calculateNutrients,
            IChemicalBalanceMessage chemicalBalanceMessage,
            IManureApplicationCalculator manureApplicationCalculator)
        {
            _logger = logger;
            _ud = ud;
            _sd = sd;
            _fg = GetFertigationData();
            _settings = settings.Value;
            _calculateCropRequirementRemoval = calculateCropRequirementRemoval;
            _calculateFertilizerNutrients = calculateFertilizerNutrients;
            _calculateFertigationNutrients = calculateFertigationNutrients;
            _calculateNutrients = calculateNutrients;
            _chemicalBalanceMessage = chemicalBalanceMessage;
            _manureApplicationCalculator = manureApplicationCalculator;
        }

        // GET: /<controller>/
        public IActionResult Calculate(string nme)
        {
            FarmDetails fd = _ud.FarmDetails();

            CalculateViewModel cvm = new CalculateViewModel
            {
                fields = new List<Field>(),
                AppSettings = _settings
            };

            cvm.regionFnd = (fd.FarmRegion.HasValue) ? true : false;
            cvm.icons = _sd.GetNutrientIcons();

            // no name entered so default to the first one for the farm
            if (nme == null)
            {
                List<Field> fldLst = _ud.GetFields();

                if (fldLst.Count() == 0)
                {
                    cvm.fldsFnd = 0;
                }
                else
                {
                    cvm.fldsFnd = fldLst.Count();
                    foreach (var f in fldLst)
                    {
                        cvm.fields.Add(f);
                    }
                    cvm.currFld = cvm.fields[0].FieldName;
                }
            }
            else
            {
                cvm.currFld = nme;
                List<Field> fldLst = _ud.GetFields();
                cvm.fldsFnd = fldLst.Count();
                foreach (var f in fldLst)
                {
                    cvm.fields.Add(f);
                }
            }

            if (cvm.fldsFnd > 0)
            {
                cvm.itemsPresent = ItemCount(cvm.currFld) > 0 ? true : false;
            }

            return View(cvm);
        }

        [HttpPost]
        public IActionResult Calculate(CalculateViewModel cvm)
        {
            if (!cvm.itemsPresent)
            {
                cvm.icons = _sd.GetNutrientIcons();
            }

            return View(cvm);
        }

        private int ItemCount(string fldname)
        {
            int items = 0;

            List<FieldCrop> crps = _ud.GetFieldCrops(fldname);
            items = items + crps.Count();

            List<NutrientManure> manures = _ud.GetFieldNutrientsManures(fldname);
            items = items + manures.Count();

            List<NutrientFertilizer> fertilizers = _ud.GetFieldNutrientsFertilizers(fldname);
            items = items + fertilizers.Count();

            List<NutrientOther> others = _ud.GetFieldNutrientsOthers(fldname);
            items = items + others.Count();

            return items;
        }

        public IActionResult ManureDetails(string fldName, int? id)
        {
            var managedManuresData = _ud.GetAllManagedManures();
            var farmManuresData = _ud.GetFarmManures();

            var mvm = new ManureDetailsViewModel()
            {
                fieldName = fldName,
                title = id == null ? "Add" : "Edit",
                btnText = id == null ? "Calculate" : "Return",
                id = id,
                avail = string.Empty,
                nh4 = string.Empty,
                stdN = true,
                stdAvail = true,
                url = _sd.GetExternalLink("manureunitexplanation"),
                urlText = _sd.GetUserPrompt("moreinfo"),
                AmmoniumRetentionMsg = _sd.GetUserPrompt("AmmoniumRetensionMessage"),
                AvailablOrganicNitrogranMsg = _sd.GetUserPrompt("AvailableOrganicNitrogenMessage"),
                AvailableNutrientsThisYearMsg = _sd.GetUserPrompt("AvailableNutrientsThisYearMessage"),
                AvailableNutrientsLongTermMsg = _sd.GetUserPrompt("AvaiableNutreintsLongTermMessage"),
                noManureSourceAddedWarningMsg = _sd.GetUserPrompt("NoManureSourceAddedWarning"),
                noNutrientAnalysisWaningMsg = _sd.GetUserPrompt("NoNutrientAnalysisAddedWarning"),
                UserJourney = _ud.FarmDetails().UserJourney
            };

            if (managedManuresData.Count > 0)
            {
                mvm.areThereMaterialSources = true;
            }
            else
            {
                mvm.areThereMaterialSources = false;
            }

            if (farmManuresData.Count > 0)
            {
                mvm.areThereNutrientAnalysis = true;
            }
            else
            {
                mvm.areThereNutrientAnalysis = false;
            }

            mvm.totN = "0";
            mvm.totP2o5 = "0";
            mvm.totK2o = "0";
            mvm.totNIcon = "";
            mvm.totPIcon = "";
            mvm.totKIcon = "";

            if (id != null)
            {
                NutrientManure nm = _ud.GetFieldNutrientsManure(fldName, id.Value);

                mvm.avail = nm.nAvail.ToString("###");
                mvm.selRateOption = nm.unitId;
                mvm.selApplOption = nm.applicationId;
                mvm.SelectedFarmManure = nm.manureId;
                mvm.ApplicationRate = nm.rate.ToString();
                mvm.nh4 = nm.nh4Retention.ToString("###");
                mvm.yrN = nm.yrN.ToString("G29");
                mvm.yrP2o5 = nm.yrP2o5.ToString("G29");
                mvm.yrK2o = nm.yrK2o.ToString("G29");
                mvm.ltN = nm.ltN.ToString("G29");
                mvm.ltP2o5 = nm.ltP2o5.ToString("G29");
                mvm.ltK2o = nm.ltK2o.ToString("G29");
                FarmManure man = _ud.GetFarmManure(Convert.ToInt32(nm.manureId));
                mvm.currUnit = man.SolidLiquid;
                mvm.rateOptions = _sd.GetUnitsDll(mvm.currUnit).ToList();

                int regionid = _ud.FarmDetails().FarmRegion.Value;
                Region region = _sd.GetRegion(regionid);
                var nOrganicMineralizations = _calculateNutrients
                    .GetNMineralization(_ud.GetFarmManure(Convert.ToInt16(mvm.SelectedFarmManure)),
                    region.LocationId);

                mvm.stdN = Convert.ToDecimal(mvm.nh4) !=
                    (_calculateNutrients.GetAmmoniaRetention(_ud.GetFarmManure(Convert.ToInt16(mvm.SelectedFarmManure)),
                    Convert.ToInt16(mvm.selApplOption)) * 100) ? false : true;
                mvm.stdAvail = Convert.ToDecimal(mvm.avail) != (nOrganicMineralizations.OrganicN_FirstYear * 100) ? false : true;
            }
            else

            {
                mvm.yrN = "  0";
                mvm.yrP2o5 = "  0";
                mvm.yrK2o = "  0";
                mvm.ltN = "  0";
                mvm.ltP2o5 = "  0";
                mvm.ltK2o = "  0";

                if (!string.IsNullOrEmpty(_ud?.FarmData()?.LastAppliedFarmManureId))
                {
                    mvm.SelectedFarmManure = _ud?.FarmData().LastAppliedFarmManureId;
                    var man = _ud.GetFarmManure(Convert.ToInt32(mvm.SelectedFarmManure));
                    mvm.currUnit = man.SolidLiquid;
                    mvm.rateOptions = _sd.GetUnitsDll(mvm.currUnit).ToList();
                    mvm.selRateOption = mvm.rateOptions[0].Id.ToString();
                    mvm.avail = GetOrganicNAvailableThisYear(Convert.ToInt16(mvm.SelectedFarmManure)).ToString("###");
                }
            }

            ManureDetailsSetup(ref mvm);

            MaunureStillRequired(ref mvm);

            ManureApplicationRefresh(mvm);

            return PartialView(mvm);
        }

        public IActionResult FertigationDetails(string fldName, int? id)
        {
            var fgvm = new FertigationDetailsViewModel()
            {
              fieldName = fldName,
              title = id == null ? "Add" : "Edit",
              btnText = id == null ? "Add to Field" : "Update Field",
              id = id,
            };

            if (id != null){
                NutrientFertilizer nf = _ud.GetFieldNutrientsFertilizer(fldName, id.Value);// Not sure how this is working for fertigation. might need to change

                FertilizerType ft = _sd.GetFertilizerType(nf.fertilizerTypeId.ToString());

                fgvm.currUnit = ft.DryLiquid;
                fgvm.selFertOption = ft.Custom ? 1 : nf.fertilizerId;
                fgvm.productRate = nf.applRate.ToString("#.##");
                fgvm.selProductRateUnitOption = nf.applUnitId.ToString();
                fgvm.selTypOption = nf.fertilizerTypeId.ToString();
                fgvm.fertilizerType = ft.DryLiquid;
                fgvm.calcN = nf.fertN.ToString();
                fgvm.calcP2o5 = nf.fertP2o5.ToString();
                fgvm.calcK2o = nf.fertK2o.ToString();
                if (nf.applDate.HasValue)
                {
                    fgvm.applDate = nf.applDate.HasValue ? nf.applDate.Value.ToString("MMM-yyyy") : "";
                }
                fgvm.density = nf.liquidDensity.ToString("#.##");
                fgvm.selDensityUnitOption = nf.liquidDensityUnitId;
                if (!ft.Custom)
                {
                    if (fgvm.density != _fg.GetLiquidFertilizerDensity(nf.fertilizerId, nf.liquidDensityUnitId).Value.ToString("#.##"))
                    {
                        fgvm.stdDensity = false;
                    }
                    else
                    {
                        fgvm.stdDensity = true;
                    }
                }
                if (ft.Custom)
                {
                    fgvm.valN = nf.customN.Value.ToString("0");
                    fgvm.valP2o5 = nf.customP2o5.Value.ToString("0");
                    fgvm.valK2o = nf.customK2o.Value.ToString("0");
                    fgvm.manualEntry = true;
                }
                else
                {
                    Fertilizer ff = _fg.Fertilizers.Single(fert => fert.Id == nf.fertilizerId);
                    fgvm.valN = ff.Nitrogen.ToString("0");
                    fgvm.valP2o5 = ff.Phosphorous.ToString("0");
                    fgvm.valK2o = ff.Potassium.ToString("0");
                    fgvm.manualEntry = false;
                }
            }
            else
            {
                FertigationDetail_Reset(ref fgvm);
            }
            FertigationStillRequired(ref fgvm);
            FertigationDetailsSetup(ref fgvm);

            return PartialView(fgvm);
        }

        private void FertigationDetail_Reset(ref FertigationDetailsViewModel fgvm)
        {
            fgvm.calcN = "0";
            fgvm.calcK2o = "0";
            fgvm.calcP2o5 = "0";

            fgvm.valN = "0";
            fgvm.valP2o5 = "0";
            fgvm.valK2o = "0";

            fgvm.calcTotalN = "0";
            fgvm.calcTotalK2o = "0";
            fgvm.calcTotalP2o5 = "0";

            fgvm.totN = "0";
            fgvm.totP2o5 = "0";
            fgvm.totK2o = "0";

            fgvm.totNIcon = "";
            fgvm.totPIcon = "";
            fgvm.totKIcon = "";

            fgvm.eventsPerSeason = 1;
            fgvm.applDate = DateTime.Now.ToShortDateString();
        }

        private void FertigationStillRequired(ref FertigationDetailsViewModel fgvm)
        {

            var chemicalBalances = _chemicalBalanceMessage
                .GetChemicalBalances(_ud.GetFieldDetails(fgvm.fieldName), _ud.FarmDetails().FarmRegion.Value, _ud.FarmDetails().Year);

            var msgs = _chemicalBalanceMessage.DetermineBalanceMessages(_ud.GetFieldDetails(fgvm.fieldName), _ud.FarmDetails().FarmRegion.Value, _ud.FarmDetails().Year);

            foreach (var m in msgs)
            {
                switch (m.Chemical)
                {
                    case "AgrN":
                        fgvm.totNIcon = (chemicalBalances.balance_AgrN > 0) ? "" : m.Icon;
                        fgvm.totNIconText = m.IconText;
                        break;

                    case "AgrP2O5":
                        fgvm.totPIcon = (chemicalBalances.balance_AgrP2O5 > 0) ? "" : m.Icon;
                        fgvm.totPIconText = m.IconText;
                        break;

                    case "AgrK2O":
                        fgvm.totKIcon = (chemicalBalances.balance_AgrK2O > 0) ? "" : m.Icon;
                        fgvm.totKIconText = m.IconText;
                        break;
                }
            }
            fgvm.totN = (chemicalBalances.balance_AgrN > 0) ? "0" : Math.Abs(chemicalBalances.balance_AgrN).ToString();
            fgvm.totP2o5 = (chemicalBalances.balance_AgrP2O5 > 0) ? "0" : Math.Abs(chemicalBalances.balance_AgrP2O5).ToString();
            fgvm.totK2o = (chemicalBalances.balance_AgrK2O > 0) ? "0" : Math.Abs(chemicalBalances.balance_AgrK2O).ToString();
       
        }

        private List<SelectListItem> GetFertigationFertilizers(string typeId){
            if(typeId != null && typeId != "select"){
                List<Fertilizer> fertilizers = _fg.Fertilizers;
                FertilizerType type = _sd.GetFertilizerType(typeId);
                List<SelectListItem> list = new List<SelectListItem>();
                foreach (var r in fertilizers)
                {
                    if(r.DryLiquid == type.DryLiquid){
                        var li = new SelectListItem()
                        { Id = r.Id, Value = r.Name };
                        list.Add(li);
                    }
                }
                return list;
            }
            return new List<SelectListItem>();
        }

        private Fertilizer GetFertigationFertilizer(int id)
        {
            List<Fertilizer> fertilizers = _fg.Fertilizers;
            return fertilizers.Find(x => x.Id == id);
        }

        private List<SelectListItem> GetOptionsList<T>(List<T> selectOption) where T: SelectOption{
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var option in selectOption)
            {
                var li = new SelectListItem(option);
                list.Add(li);
            }
            return list;
        }

        private void FertigationDetailsSetup(ref FertigationDetailsViewModel fgvm)
        {
            fgvm.typOptions = GetOptionsList(_fg.FertilizerTypes);
            fgvm.fertOptions = GetFertigationFertilizers(fgvm.selTypOption);
            fgvm.productRateUnitOptions = GetOptionsList(_fg.ProductRateUnits);
            fgvm.injectionRateUnitOptions = GetOptionsList(_fg.InjectionRateUnits);
            fgvm.densityUnitOptions = GetOptionsList(_fg.DensityUnits);
            FertigationDetailSetup_Fertilizer(ref fgvm);
        }

        private void FertigationDetailSetup_Fertilizer(ref FertigationDetailsViewModel fgvm)
        {
            fgvm.fertOptions = new List<SelectListItem>();
            if (fgvm.selTypOption != null &&
               fgvm.selTypOption != "select")
            {
                FertilizerType ft = _sd.GetFertilizerType(fgvm.selTypOption);
                fgvm.fertilizerType = ft.DryLiquid;
                if (!ft.Custom)
                {
                    fgvm.fertOptions = GetFertigationFertilizers(ft.Id.ToString());
                }
                else
                {
                    fgvm.fertOptions = new List<SelectListItem>() { new SelectListItem() { Id = 1, Value = "Custom" } };
                    fgvm.selFertOption = 1;
                    fgvm.stdDensity = true;
                }
            }
            else
            {
                fgvm.fertOptions = new List<SelectListItem>();
                fgvm.selFertOption = 0;
            }

            return;
        }

        [HttpPost]
        public IActionResult FertigationDetails(FertigationDetailsViewModel fgvm)
        {
            decimal nmbrDensity = 0;
            decimal nmbrN = 0;
            decimal nmbrP = 0;
            decimal nmbrK = 0;
            int addedId = 0;
            NutrientFertilizer origFertilizer = new NutrientFertilizer();

            //Utility.CalculateNutrients calculateNutrients = new CalculateNutrients(_env, _ud, _sd);
            //NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            FertigationDetailsSetup(ref fgvm);

            try
            {
                /*
                if (fgvm.buttonPressed == "Calculate")
                {
                    if (!ModelState.IsValid)
                    {
                        FertigationDetailsSetup(ref fgvm);
                        return PartialView(fgvm);
                    }   

                    // Calculation logic will be implemented here by Adam and the team
                    // As of now, clear the model state until that ticket is implemented
                    ModelState.Clear();
                    fgvm.btnText = "Add to Field";
                    FertigationDetailsSetup(ref fgvm); 
                    return PartialView(fgvm);
                }
                */
                if (fgvm.buttonPressed == "ResetDensity")
                {
                    ModelState.Clear();
                    fgvm.buttonPressed = "";
                    fgvm.btnText = "Calculate";

                    NutrientFertilizer nf = _ud.GetFieldNutrientsFertilizer(fgvm.fieldName, fgvm.id.Value);

                    fgvm.density = _fg.GetLiquidFertilizerDensity(nf.fertilizerId, nf.liquidDensityUnitId).Value.ToString("#.##");

                    fgvm.stdDensity = true;
                    return View(fgvm);
                }

                if (fgvm.buttonPressed == "TypeChange")
                {
                    ModelState.Clear();
                    fgvm.buttonPressed = "";
                    fgvm.btnText = "Calculate";

                    FertigationDetailSetup_Fertilizer(ref fgvm);

                    if (fgvm.selTypOption != "" && fgvm.selTypOption != "select")
                    {
                        FertilizerType typ = _sd.GetFertilizerType(fgvm.selTypOption);

                        fgvm.density = "";

                        // if (fgvm.currUnit != typ.DryLiquid)
                        // {
                        //     fgvm.currUnit = typ.DryLiquid;
                        //     //fgvm.productRateUnitOptions = _sd.GetFertilizerUnitsDll(fgvm.currUnit).ToList();
                        //     fgvm.selProductRateUnitOption = fgvm.productRateUnitOptions[0].Id.ToString();
                        //     fgvm.fertilizerType = typ.DryLiquid;
                        // }
                        fgvm.fertilizerType = typ.DryLiquid;
                        fgvm.manualEntry = typ.Custom;
                        if (!fgvm.manualEntry)
                            fgvm.selFertOption = 0;

                        FertigationDetailSetup_DefaultDensity(ref fgvm);
                    }

                    FertigationDetail_Reset(ref fgvm);

                    return PartialView(fgvm);
                }

                if (fgvm.buttonPressed == "FertilizerChange")
                {
                    ModelState.Clear();
                    fgvm.buttonPressed = "";
                    fgvm.btnText = "Calculate";
                    //FertilizerDetail_Reset(ref fvm);

                    if (fgvm.selFertOption != 0 &&
                       !fgvm.manualEntry)
                    {
                        //Removed () around
                        Fertilizer ft = GetFertigationFertilizer(fgvm.selFertOption ?? 0);
                        fgvm.valN = ft.Nitrogen.ToString("0");
                        fgvm.valP2o5 = ft.Phosphorous.ToString("0");
                        fgvm.valK2o = ft.Potassium.ToString("0");

                        FertigationDetailSetup_DefaultDensity(ref fgvm);
                    }

                    return View(fgvm);
                }

                if (fgvm.buttonPressed == "DensityChange")
                {
                    ModelState.Clear();
                    fgvm.buttonPressed = "";
                    fgvm.btnText = "Calculate";

                    if (!fgvm.manualEntry &&
                        fgvm.fertilizerType == "liquid")
                    {
                        FertigationDetailSetup_DefaultDensity(ref fgvm);
                    }
                    return View(fgvm);
                }

                if (ModelState.IsValid)
                {
                    if (fgvm.fertilizerType == "liquid")
                    {
                        if (string.IsNullOrEmpty(fgvm.density))
                        {
                            ModelState.AddModelError("density", "Required");
                            return View(fgvm);
                        }
                        if (!Decimal.TryParse(fgvm.density, out nmbrDensity))
                        {
                            ModelState.AddModelError("density", "Invalid");
                            return View(fgvm);
                        }
                        else
                        {
                            if (nmbrDensity < 0 ||
                                nmbrDensity > 100)
                            {
                                ModelState.AddModelError("density", "Invalid");
                                return View(fgvm);
                            }
                        }
                        if (fgvm.selDensityUnitOption == 0)
                        {
                            ModelState.AddModelError("selDenOption", "Required");
                            return View(fgvm);
                        }
                    }

                    if (fgvm.manualEntry)
                    {
                        if (string.IsNullOrEmpty(fgvm.valN))
                        {
                            ModelState.AddModelError("valN", "Required");
                            return View(fgvm);
                        }
                        if (!Decimal.TryParse(fgvm.valN, out nmbrN))
                        {
                            ModelState.AddModelError("valN", "Invalid");
                            return View(fgvm);
                        }
                        if (nmbrN < 0 || nmbrN > 100)
                        {
                            ModelState.AddModelError("valN", "Invalid");
                            return View(fgvm);
                        }
                        if (string.IsNullOrEmpty(fgvm.valP2o5))
                        {
                            ModelState.AddModelError("valP2o5", "Required");
                            return View(fgvm);
                        }
                        if (!Decimal.TryParse(fgvm.valP2o5, out nmbrP))
                        {
                            ModelState.AddModelError("valP2o5", "Invalid");
                            return View(fgvm);
                        }
                        if (nmbrP < 0 || nmbrP > 100)
                        {
                            ModelState.AddModelError("valP2o5", "Invalid");
                            return View(fgvm);
                        }
                        if (string.IsNullOrEmpty(fgvm.valK2o))
                        {
                            ModelState.AddModelError("valK2o", "Required");
                            return View(fgvm);
                        }
                        if (!Decimal.TryParse(fgvm.valK2o, out nmbrK))
                        {
                            ModelState.AddModelError("valK2o", "Invalid");
                            return View(fgvm);
                        }
                        if (nmbrK < 0 || nmbrK > 100)
                        {
                            ModelState.AddModelError("valK2o", "Invalid");
                            return View(fgvm);
                        }
                    }

                    if (fgvm.buttonPressed == "Calculate" && ModelState.IsValid)
                    {
                       // if (!ModelState.IsValid)
                       // {
                        //    ModelState.Clear();
                        //    FertigationDetailsSetup(ref fgvm);
                       //     return PartialView(fgvm);
                       // }
                        ModelState.Clear();
                        FertilizerType ft = _sd.GetFertilizerType(fgvm.selTypOption.ToString());

                       // if (ft.DryLiquid == "liquid")
                      //  {
                      //      if (!ft.Custom)
                      //      {
                       //         if (fgvm.density != _sd.GetLiquidFertilizerDensity(fgvm.selFertOption ?? 0, fgvm.selDensityUnitOption ?? 0).Value.ToString("#.##"))
                     //           {
                     //               fgvm.stdDensity = false;
                     //           }
                     //           else
                     //           {
                     //               fgvm.stdDensity = true;
                     //           }
                     //       }
                     //   }
                    //
                        var fertilizerNutrients = _calculateFertigationNutrients.GetFertilizerNutrients(fgvm.selFertOption ?? 0,
                                fgvm.fertilizerType,
                                Convert.ToDecimal(fgvm.productRate),
                                Convert.ToInt32(fgvm.selProductRateUnitOption),
                                fgvm.density != null ? Convert.ToDecimal(fgvm.density) : 0,
                                Convert.ToInt16(fgvm.selDensityUnitOption),
                                Convert.ToDecimal(fgvm.valN),
                                Convert.ToDecimal(fgvm.valP2o5),
                                Convert.ToDecimal(fgvm.valK2o),
                                fgvm.manualEntry);

                        fgvm.calcN = Convert.ToInt32(fertilizerNutrients.fertilizer_N).ToString();
                        fgvm.calcP2o5 = Convert.ToInt32(fertilizerNutrients.fertilizer_P2O5).ToString();
                        fgvm.calcK2o = Convert.ToInt32(fertilizerNutrients.fertilizer_K2O).ToString();

                        fgvm.calcTotalN = Convert.ToString(Convert.ToInt32(fertilizerNutrients.fertilizer_N) * Convert.ToInt32(fgvm.eventsPerSeason));
                        fgvm.calcTotalK2o = Convert.ToString(Convert.ToInt32(fertilizerNutrients.fertilizer_P2O5) * Convert.ToInt32(fgvm.eventsPerSeason));
                        fgvm.calcTotalP2o5 = Convert.ToString(Convert.ToInt32(fertilizerNutrients.fertilizer_K2O) * Convert.ToInt32(fgvm.eventsPerSeason));

                        fgvm.btnText = fgvm.id == null ? "Add to Field" : "Update Field";

                        // temporarily update the farm data so as to recalc the Still Required amounts
                        if (fgvm.id == null)
                        {
                            addedId = FertigationInsert(fgvm);
                        }
                        else
                        {
                            origFertilizer = _ud.GetFieldNutrientsFertilizer(fgvm.fieldName, fgvm.id.Value);
                            FertigationUpdate(fgvm);
                        }

                        FertigationStillRequired(ref fgvm);
                        if (fgvm.id == null)
                        {
                            _ud.DeleteFieldNutrientsFertilizer(fgvm.fieldName, addedId);
                        }
                        else
                        {
                            _ud.UpdateFieldNutrientsFertilizer(fgvm.fieldName, origFertilizer);
                        }
                    } /*
                    else
                    {
                        if (fgvm.id == null)
                        {
                            FertigationInsert(fgvm);
                        }
                        else
                        {
                            FertigationUpdate(fgvm);
                        }
                        return Json(ReDisplay("#fertilizer", fgvm.fieldName));
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error.");
                _logger.LogError(ex, "FertilizerDetails Exception");
            }
            fgvm.buttonPressed = "";
            return PartialView(fgvm);
        }

        private void FertigationDetailSetup_DefaultDensity(ref FertigationDetailsViewModel fgvm)
        {
            fgvm.fertOptions = new List<SelectListItem>();
            if (fgvm.selTypOption != null &&
               fgvm.selTypOption != "select")
            {
                FertilizerType ft = _sd.GetFertilizerType(fgvm.selTypOption);
                if (!ft.Custom)
                {
                    fgvm.fertOptions = GetFertigationFertilizers(ft.Id.ToString()).ToList();
                }
                else
                {
                    fgvm.fertOptions = new List<SelectListItem>() { new SelectListItem() { Id = 1, Value = "Custom" } };
                    fgvm.selFertOption = 1;
                    fgvm.stdDensity = true;
                }
            }
            else
            {
                fgvm.fertOptions = new List<SelectListItem>();
                fgvm.selFertOption = 0;
            }


            if (fgvm.selDensityUnitOption == 0 || fgvm.selFertOption == 0)
            {
                fgvm.density = "";
                fgvm.stdDensity = true;
            }
            
            if (!fgvm.manualEntry &&
                fgvm.fertilizerType == "liquid" &&
                fgvm.selFertOption != 0 &&
                fgvm.selDensityUnitOption != 0)
            {
                fgvm.density = _fg.GetLiquidFertilizerDensity(Convert.ToInt32(fgvm.selFertOption), Convert.ToInt32(fgvm.selDensityUnitOption)).Value.ToString("#.##");
                fgvm.stdDensity = true;
            }

        }

        private int FertigationInsert(FertigationDetailsViewModel fgvm)
        {
            NutrientFertilizer nf = new NutrientFertilizer()
            {
                fertilizerTypeId = Convert.ToInt32(fgvm.selTypOption),
                fertilizerId = fgvm.selFertOption ?? 0,
                applUnitId = Convert.ToInt32(fgvm.selProductRateUnitOption),
                applRate = Convert.ToDecimal(fgvm.productRate),
                applDate = string.IsNullOrEmpty(fgvm.applDate) ? (DateTime?)null : Convert.ToDateTime(fgvm.applDate),
                customN = fgvm.manualEntry ? Convert.ToDecimal(fgvm.valN) : (decimal?)null,
                customP2o5 = fgvm.manualEntry ? Convert.ToDecimal(fgvm.valP2o5) : (decimal?)null,
                customK2o = fgvm.manualEntry ? Convert.ToDecimal(fgvm.valK2o) : (decimal?)null,
                fertN = Convert.ToDecimal(fgvm.calcN),
                fertP2o5 = Convert.ToDecimal(fgvm.calcP2o5),
                fertK2o = Convert.ToDecimal(fgvm.calcK2o),
                liquidDensity =  Convert.ToDecimal(fgvm.density),
                liquidDensityUnitId = Convert.ToInt32(fgvm.selDensityUnitOption)
            };

            return _ud.AddFieldNutrientsFertilizer(fgvm.fieldName, nf);
        }

        private void FertigationUpdate(FertigationDetailsViewModel fgvm)
        {
            NutrientFertilizer nf = _ud.GetFieldNutrientsFertilizer(fgvm.fieldName, fgvm.id.Value);
            nf.fertilizerTypeId = Convert.ToInt32(fgvm.selTypOption);
            nf.fertilizerId = fgvm.selFertOption ?? 0;
            nf.applUnitId = Convert.ToInt32(fgvm.selProductRateUnitOption);
            nf.applRate = Convert.ToDecimal(fgvm.productRate);
            nf.applDate = string.IsNullOrEmpty(fgvm.applDate) ? (DateTime?)null : Convert.ToDateTime(fgvm.applDate);
            nf.customN = fgvm.manualEntry ? Convert.ToDecimal(fgvm.valN) : (decimal?)null;
            nf.customP2o5 = fgvm.manualEntry ? Convert.ToDecimal(fgvm.valP2o5) : (decimal?)null;
            nf.customK2o = fgvm.manualEntry ? Convert.ToDecimal(fgvm.valK2o) : (decimal?)null;
            nf.fertN = Convert.ToDecimal(fgvm.calcN);
            nf.fertP2o5 = Convert.ToDecimal(fgvm.calcP2o5);
            nf.fertK2o = Convert.ToDecimal(fgvm.calcK2o);
            nf.liquidDensity = Convert.ToDecimal(fgvm.density);
            nf.liquidDensityUnitId = Convert.ToInt32(fgvm.selDensityUnitOption);

            _ud.UpdateFieldNutrientsFertilizer(fgvm.fieldName, nf);
        }

        public Fertigation GetFertigationData()
        {
            var filePath = "../../../Agri.Data/SeedData/FertigationData.json";
            var jsonData = System.IO.File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Fertigation>(jsonData);
        }

        private void MaunureStillRequired(ref ManureDetailsViewModel mvm)
        {
            //recalc totals for display

            var field = _ud.GetFieldDetails(mvm.fieldName);
            var chemicalBalances = _chemicalBalanceMessage.GetChemicalBalances(field, _ud.FarmDetails().FarmRegion.Value, _ud.FarmDetails().Year);
            var msgs = _chemicalBalanceMessage.DetermineBalanceMessages(field, _ud.FarmDetails().FarmRegion.Value, _ud.FarmDetails().Year);

            foreach (var m in msgs)
            {
                switch (m.Chemical)
                {
                    case "AgrN":
                        mvm.totNIcon = (chemicalBalances.balance_AgrN > 0) ? "" : m.Icon;
                        mvm.totNIconText = m.IconText;
                        break;

                    case "AgrP2O5":
                        mvm.totPIcon = (chemicalBalances.balance_AgrP2O5 > 0) ? "" : m.Icon;
                        mvm.totPIconText = m.IconText;
                        break;

                    case "AgrK2O":
                        mvm.totKIcon = (chemicalBalances.balance_AgrK2O > 0) ? "" : m.Icon;
                        mvm.totKIconText = m.IconText;
                        break;
                }
            }

            mvm.totN = (chemicalBalances.balance_AgrN > 0) ? "0" : Math.Abs(chemicalBalances.balance_AgrN).ToString();
            mvm.totP2o5 = (chemicalBalances.balance_AgrP2O5 > 0) ? "0" : Math.Abs(chemicalBalances.balance_AgrP2O5).ToString();
            mvm.totK2o = (chemicalBalances.balance_AgrK2O > 0) ? "0" : Math.Abs(chemicalBalances.balance_AgrK2O).ToString();
        }

        [HttpPost]
        public IActionResult ManureDetails(ManureDetailsViewModel mvm)
        {
            decimal nmbr;
            int addedId = 0;
            var managedManuresData = _ud.GetAllManagedManures();
            var farmManuresData = _ud.GetFarmManures();

            mvm.noManureSourceAddedWarningMsg = _sd.GetUserPrompt("NoManureSourceAddedWarning");
            mvm.noNutrientAnalysisWaningMsg = _sd.GetUserPrompt("NoNutrientAnalysisAddedWarning");
            if (managedManuresData.Count > 0)
            {
                mvm.areThereMaterialSources = true;
            }
            else
            {
                mvm.areThereMaterialSources = false;
            }

            if (farmManuresData.Count > 0)
            {
                mvm.areThereNutrientAnalysis = true;
            }
            else
            {
                mvm.areThereNutrientAnalysis = false;
            }

            NutrientManure origManure = new NutrientManure();

            ManureDetailsSetup(ref mvm);

            try
            {
                if (mvm.buttonPressed == "ResetN")
                {
                    ModelState.Clear();
                    mvm.buttonPressed = "";
                    mvm.btnText = "Calculate";

                    // reset to calculated amount
                    mvm.nh4 = (_calculateNutrients.GetAmmoniaRetention(_ud.GetFarmManure(Convert.ToInt16(mvm.SelectedFarmManure)), Convert.ToInt16(mvm.selApplOption)) * 100).ToString("###");

                    mvm.stdN = true;
                    return View(mvm);
                }

                if (mvm.buttonPressed == "ResetA")
                {
                    ModelState.Clear();
                    mvm.buttonPressed = "";
                    mvm.btnText = "Calculate";

                    // reset to calculated amount
                    mvm.avail = GetOrganicNAvailableThisYear(Convert.ToInt16(mvm.SelectedFarmManure)).ToString("###");

                    mvm.stdAvail = true;
                    return View(mvm);
                }

                if (mvm.buttonPressed == "TypeChange")
                {
                    ModelState.Clear();
                    mvm.buttonPressed = "";
                    mvm.btnText = "Calculate";

                    if (mvm.SelectedFarmManure != "" &&
                        !mvm.SelectedFarmManure.Equals("selApplOption", StringComparison.CurrentCultureIgnoreCase))
                    {
                        FarmManure man = _ud.GetFarmManure(Convert.ToInt32(mvm.SelectedFarmManure));
                        if (mvm.currUnit != man.SolidLiquid)
                        {
                            mvm.currUnit = man.SolidLiquid;
                            mvm.rateOptions = _sd.GetUnitsDll(mvm.currUnit).ToList();
                            mvm.selRateOption = mvm.rateOptions[0].Id.ToString();
                        }

                        // if application is present then recalc N and A
                        mvm.avail = GetOrganicNAvailableThisYear(Convert.ToInt16(mvm.SelectedFarmManure)).ToString("###");

                        if (mvm.selApplOption != "" &&
                            mvm.selApplOption != "select")
                        {
                            // recalc N and A values
                            mvm.nh4 = (_calculateNutrients.GetAmmoniaRetention(_ud.GetFarmManure(Convert.ToInt16(mvm.SelectedFarmManure)),
                                Convert.ToInt16(mvm.selApplOption)) * 100).ToString("###");
                        }

                        ManureApplicationRefresh(mvm);
                    }
                    else
                    {
                        mvm.avail = string.Empty;
                    }
                    return View(mvm);
                }

                if (mvm.buttonPressed == "ApplChange")
                {
                    ModelState.Clear();
                    mvm.buttonPressed = "";
                    mvm.btnText = "Calculate";

                    if (mvm.selApplOption != "" &&
                        mvm.selApplOption != "select")
                    {
                        // recalc N and A values
                        mvm.nh4 = (_calculateNutrients.GetAmmoniaRetention(_ud.GetFarmManure(Convert.ToInt16(mvm.SelectedFarmManure)), Convert.ToInt16(mvm.selApplOption)) * 100).ToString("###");
                    }
                    else
                    {
                        mvm.nh4 = string.Empty;
                    }
                    return View(mvm);
                }

                if (ModelState.IsValid)
                {
                    if (!Decimal.TryParse(mvm.avail, out nmbr))
                    {
                        ModelState.AddModelError("avail", "Numbers only.");
                    }
                    else
                    {
                        if (nmbr < 0 ||
                           nmbr > 100)
                        {
                            ModelState.AddModelError("avail", "Invalid.");
                        }
                    }
                    if (!Decimal.TryParse(mvm.nh4, out nmbr))
                    {
                        ModelState.AddModelError("nh4", "Numbers only.");
                    }
                    else
                    {
                        if (nmbr < 0 ||
                           nmbr > 100)
                        {
                            ModelState.AddModelError("nh4", "Invalid.");
                        }
                    }
                    if (!ModelState.IsValid)
                    {
                        return View(mvm);
                    }
                    if (mvm.btnText == "Calculate")
                    {
                        ModelState.Clear();

                        var nutrientInputs = _calculateNutrients.GetNutrientInputs(
                            _ud.GetFarmManure(Convert.ToInt32(mvm.SelectedFarmManure)),
                            _sd.GetRegion(_ud.FarmDetails().FarmRegion.Value),
                            Convert.ToDecimal(mvm.ApplicationRate),
                            mvm.selRateOption,
                            Convert.ToDecimal(mvm.nh4),
                            Convert.ToDecimal(mvm.avail));

                        mvm.yrN = nutrientInputs.N_FirstYear.ToString();
                        mvm.yrP2o5 = nutrientInputs.P2O5_FirstYear.ToString();
                        mvm.yrK2o = nutrientInputs.K2O_FirstYear.ToString();
                        mvm.ltN = nutrientInputs.N_LongTerm.ToString();
                        mvm.ltP2o5 = nutrientInputs.P2O5_LongTerm.ToString();
                        mvm.ltK2o = nutrientInputs.K2O_LongTerm.ToString();

                        mvm.btnText = mvm.id == null ? "Add to Field" : "Update Field";

                        // determine if values on screen are book value or not
                        if (Convert.ToDecimal(mvm.nh4) !=
                            (_calculateNutrients.GetAmmoniaRetention(
                                _ud.GetFarmManure(Convert.ToInt16(mvm.SelectedFarmManure)),
                                Convert.ToInt16(mvm.selApplOption)) * 100))
                        {
                            mvm.stdN = false;
                        }
                        if (Convert.ToDecimal(mvm.avail) != GetOrganicNAvailableThisYear(Convert.ToInt16(mvm.SelectedFarmManure)))
                        {
                            mvm.stdAvail = false;
                        }

                        // temporarily update the farm data so as to recalc the Still Required amounts
                        if (mvm.id == null)
                        {
                            addedId = ManureInsert(mvm);
                        }
                        else
                        {
                            origManure = _ud.GetFieldNutrientsManure(mvm.fieldName, mvm.id.Value);
                            ManureUpdate(mvm);
                        }

                        MaunureStillRequired(ref mvm);
                        ManureApplicationRefresh(mvm);

                        if (mvm.id == null)
                        {
                            _ud.DeleteFieldNutrientsManure(mvm.fieldName, addedId);
                        }
                        else
                        {
                            _ud.UpdateFieldNutrientsManure(mvm.fieldName, origManure);
                        }
                    }
                    else
                    {
                        if (mvm.id == null)
                        {
                            ManureInsert(mvm);
                        }
                        else
                        {
                            ManureUpdate(mvm);
                        }
                        return Json(ReDisplay("#manure", mvm.fieldName));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error.");
                _logger.LogError(ex, "ManureDetails Exception");
            }

            return PartialView(mvm);
        }

        private decimal GetOrganicNAvailableThisYear(int farmManureId)
        {
            int regionid = _ud.FarmDetails().FarmRegion.Value;
            Region region = _sd.GetRegion(regionid);
            var nOrganicMineralizations = _calculateNutrients
                .GetNMineralization(_ud.GetFarmManure(Convert.ToInt16(farmManureId)), region.LocationId);
            return nOrganicMineralizations.OrganicN_FirstYear * 100;
        }

        private int ManureInsert(ManureDetailsViewModel mvm)
        {
            NutrientManure nm = new NutrientManure()
            {
                manureId = mvm.SelectedFarmManure,
                applicationId = mvm.selApplOption,
                unitId = mvm.selRateOption,
                rate = Convert.ToDecimal(mvm.ApplicationRate),
                nh4Retention = Convert.ToDecimal(mvm.nh4),
                nAvail = Convert.ToDecimal(mvm.avail),
                yrN = Convert.ToDecimal(mvm.yrN),
                yrP2o5 = Convert.ToDecimal(mvm.yrP2o5),
                yrK2o = Convert.ToDecimal(mvm.yrK2o),
                ltN = Convert.ToDecimal(mvm.ltN),
                ltP2o5 = Convert.ToDecimal(mvm.ltP2o5),
                ltK2o = Convert.ToDecimal(mvm.ltK2o)
            };

            return _ud.AddFieldNutrientsManure(mvm.fieldName, nm);
        }

        private void ManureUpdate(ManureDetailsViewModel mvm)
        {
            NutrientManure nm = _ud.GetFieldNutrientsManure(mvm.fieldName, mvm.id.Value);
            nm.manureId = mvm.SelectedFarmManure;
            nm.applicationId = mvm.selApplOption;
            nm.unitId = mvm.selRateOption;
            nm.rate = Convert.ToDecimal(mvm.ApplicationRate);
            nm.nh4Retention = Convert.ToDecimal(mvm.nh4);
            nm.nAvail = Convert.ToDecimal(mvm.avail);
            nm.yrN = Convert.ToDecimal(mvm.yrN);
            nm.yrP2o5 = Convert.ToDecimal(mvm.yrP2o5);
            nm.yrK2o = Convert.ToDecimal(mvm.yrK2o);
            nm.ltN = Convert.ToDecimal(mvm.ltN);
            nm.ltP2o5 = Convert.ToDecimal(mvm.ltP2o5);
            nm.ltK2o = Convert.ToDecimal(mvm.ltK2o);

            _ud.UpdateFieldNutrientsManure(mvm.fieldName, nm);
        }

        private void ManureDetailsSetup(ref ManureDetailsViewModel mvm)
        {
            mvm.ManureTypeOptions = new List<SelectListItem>();
            mvm.ManureTypeOptions = _ud.GetFarmManuresDll().ToList();

            mvm.applOptions = new List<SelectListItem>();
            if (mvm.SelectedFarmManure != null &&
                mvm.SelectedFarmManure != "select" &&
                mvm.SelectedFarmManure != "")
            {
                FarmManure fm = _ud.GetFarmManure(Convert.ToInt32(mvm.SelectedFarmManure));
                mvm.applOptions = _sd.GetApplicationsDll(_sd.GetManure(fm.ManureId).SolidLiquid).ToList();
            }

            mvm.rateOptions = new List<SelectListItem>();
            mvm.rateOptions = _sd.GetUnitsDll(mvm.currUnit).ToList();
            mvm.selRateOptionText = "(lb/ac)";

            return;
        }

        private void ManureApplicationRefresh(ManureDetailsViewModel mvm)
        {
            if (!string.IsNullOrWhiteSpace(mvm.SelectedFarmManure) &&
                !mvm.SelectedFarmManure.Equals("select", StringComparison.CurrentCultureIgnoreCase))
            {
                var yearData = _ud.GetYearData();
                FarmManure fm = _ud.GetFarmManure(Convert.ToInt32(mvm.SelectedFarmManure));
                var appliedManure = _manureApplicationCalculator.GetAppliedManure(yearData, fm);
                if (appliedManure != null)
                {
                    mvm.MaterialRemainingLabel = appliedManure.SourceName;
                    mvm.MaterialRemainingWholePercent = appliedManure.WholePercentRemaining;
                }
            }
        }

        public IActionResult FertilizerDetails(string fldName, int? id)
        {
            //Utility.CalculateNutrients calculateNutrients = new CalculateNutrients(_env, _ud, _sd);
            //NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            FertilizerDetailsViewModel fvm = new FertilizerDetailsViewModel
            {
                fieldName = fldName,
                title = id == null ? "Add" : "Edit",
                btnText = id == null ? "Add to Field" : "Update Field",
                id = id,
                selMethOption = 0
            };

            fvm.totN = "0";
            fvm.totP2o5 = "0";
            fvm.totK2o = "0";
            fvm.totNIcon = "";
            fvm.totPIcon = "";
            fvm.totKIcon = "";

            if (id != null)
            {
                NutrientFertilizer nf = _ud.GetFieldNutrientsFertilizer(fldName, id.Value);

                fvm.selTypOption = nf.fertilizerTypeId.ToString();
                FertilizerType ft = _sd.GetFertilizerType(nf.fertilizerTypeId.ToString());

                fvm.currUnit = ft.DryLiquid;
                fvm.selFertOption = ft.Custom ? 1 : nf.fertilizerId;
                fvm.applRate = nf.applRate.ToString("#.##");
                fvm.selRateOption = nf.applUnitId.ToString();
                fvm.selMethOption = nf.applMethodId;
                fvm.fertilizerType = ft.DryLiquid;
                fvm.calcN = nf.fertN.ToString();
                fvm.calcP2o5 = nf.fertP2o5.ToString();
                fvm.calcK2o = nf.fertK2o.ToString();
                if (nf.applDate.HasValue)
                {
                    fvm.applDate = nf.applDate.HasValue ? nf.applDate.Value.ToString("MMM-yyyy") : "";
                }
                if (ft.DryLiquid == "liquid")
                {
                    fvm.density = nf.liquidDensity.ToString("#.##");
                    fvm.selDenOption = nf.liquidDensityUnitId;
                    if (!ft.Custom)
                    {
                        if (fvm.density != _sd.GetLiquidFertilizerDensity(nf.fertilizerId, nf.liquidDensityUnitId).Value.ToString("#.##"))
                        {
                            fvm.stdDensity = false;
                        }
                        else
                        {
                            fvm.stdDensity = true;
                        }
                    }
                }
                if (ft.Custom)
                {
                    fvm.valN = nf.customN.Value.ToString("0");
                    fvm.valP2o5 = nf.customP2o5.Value.ToString("0");
                    fvm.valK2o = nf.customK2o.Value.ToString("0");
                    fvm.manEntry = true;
                }
                else
                {
                    Fertilizer ff = _sd.GetFertilizer(nf.fertilizerId.ToString());
                    fvm.valN = ff.Nitrogen.ToString("0");
                    fvm.valP2o5 = ff.Phosphorous.ToString("0");
                    fvm.valK2o = ff.Potassium.ToString("0");
                    fvm.manEntry = false;
                }
            }
            else

            {
                FertilizerDetail_Reset(ref fvm);
            }

            FertilizerStillRequired(ref fvm);

            FertilizerDetailsSetup(ref fvm);

            return PartialView(fvm);
        }

        private void FertilizerStillRequired(ref FertilizerDetailsViewModel fvm)
        {
            //recalc totals for display

            var chemicalBalances = _chemicalBalanceMessage
                .GetChemicalBalances(_ud.GetFieldDetails(fvm.fieldName), _ud.FarmDetails().FarmRegion.Value, _ud.FarmDetails().Year);

            var msgs = _chemicalBalanceMessage.DetermineBalanceMessages(_ud.GetFieldDetails(fvm.fieldName), _ud.FarmDetails().FarmRegion.Value, _ud.FarmDetails().Year);

            foreach (var m in msgs)
            {
                switch (m.Chemical)
                {
                    case "AgrN":
                        fvm.totNIcon = (chemicalBalances.balance_AgrN > 0) ? "" : m.Icon;
                        fvm.totNIconText = m.IconText;
                        break;

                    case "AgrP2O5":
                        fvm.totPIcon = (chemicalBalances.balance_AgrP2O5 > 0) ? "" : m.Icon;
                        fvm.totPIconText = m.IconText;
                        break;

                    case "AgrK2O":
                        fvm.totKIcon = (chemicalBalances.balance_AgrK2O > 0) ? "" : m.Icon;
                        fvm.totKIconText = m.IconText;
                        break;
                }
            }

            fvm.totN = (chemicalBalances.balance_AgrN > 0) ? "0" : Math.Abs(chemicalBalances.balance_AgrN).ToString();
            fvm.totP2o5 = (chemicalBalances.balance_AgrP2O5 > 0) ? "0" : Math.Abs(chemicalBalances.balance_AgrP2O5).ToString();
            fvm.totK2o = (chemicalBalances.balance_AgrK2O > 0) ? "0" : Math.Abs(chemicalBalances.balance_AgrK2O).ToString();
        }

        [HttpPost]
        public IActionResult FertilizerDetails(FertilizerDetailsViewModel fvm)
        {
            decimal nmbrDensity = 0;
            decimal nmbrN = 0;
            decimal nmbrP = 0;
            decimal nmbrK = 0;
            int addedId = 0;
            NutrientFertilizer origFertilizer = new NutrientFertilizer();

            //Utility.CalculateNutrients calculateNutrients = new CalculateNutrients(_env, _ud, _sd);
            //NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            FertilizerDetailsSetup(ref fvm);

            try
            {
                if (fvm.buttonPressed == "ResetDensity")
                {
                    ModelState.Clear();
                    fvm.buttonPressed = "";
                    fvm.btnText = "Calculate";

                    NutrientFertilizer nf = _ud.GetFieldNutrientsFertilizer(fvm.fieldName, fvm.id.Value);

                    fvm.density = _sd.GetLiquidFertilizerDensity(nf.fertilizerId, nf.liquidDensityUnitId).Value.ToString("#.##");

                    fvm.stdDensity = true;
                    return View(fvm);
                }

                if (fvm.buttonPressed == "TypeChange")
                {
                    ModelState.Clear();
                    fvm.buttonPressed = "";
                    fvm.btnText = "Calculate";

                    FertilizerDetailSetup_Fertilizer(ref fvm);

                    if (fvm.selTypOption != "" && fvm.selTypOption != "select")
                    {
                        FertilizerType typ = _sd.GetFertilizerType(fvm.selTypOption);

                        fvm.density = "";

                        if (fvm.currUnit != typ.DryLiquid)
                        {
                            fvm.currUnit = typ.DryLiquid;
                            fvm.rateOptions = _sd.GetFertilizerUnitsDll(fvm.currUnit).ToList();
                            fvm.selRateOption = fvm.rateOptions[0].Id.ToString();
                            fvm.fertilizerType = typ.DryLiquid;
                        }

                        fvm.manEntry = typ.Custom;
                        if (!fvm.manEntry)
                            fvm.selFertOption = 0;

                        FertilizerDetailSetup_DefaultDensity(ref fvm);
                    }

                    FertilizerDetail_Reset(ref fvm);

                    return View(fvm);
                }

                if (fvm.buttonPressed == "FertilizerChange")
                {
                    ModelState.Clear();
                    fvm.buttonPressed = "";
                    fvm.btnText = "Calculate";
                    //FertilizerDetail_Reset(ref fvm);

                    if (fvm.selFertOption != 0 &&
                       !fvm.manEntry)
                    {
                        Fertilizer ft = _sd.GetFertilizer(fvm.selFertOption.ToString());
                        fvm.valN = ft.Nitrogen.ToString("0");
                        fvm.valP2o5 = ft.Phosphorous.ToString("0");
                        fvm.valK2o = ft.Potassium.ToString("0");

                        FertilizerDetailSetup_DefaultDensity(ref fvm);
                    }

                    return View(fvm);
                }

                if (fvm.buttonPressed == "DensityChange")
                {
                    ModelState.Clear();
                    fvm.buttonPressed = "";
                    fvm.btnText = "Calculate";

                    if (!fvm.manEntry &&
                        fvm.fertilizerType == "liquid")
                    {
                        FertilizerDetailSetup_DefaultDensity(ref fvm);
                    }
                    return View(fvm);
                }

                if (ModelState.IsValid)
                {
                    if (fvm.manEntry &&
                    fvm.fertilizerType == "liquid" &&
                    string.IsNullOrEmpty(fvm.density))
                    {
                        ModelState.AddModelError("density", "Required");
                        return View(fvm);
                    }
                    if (fvm.fertilizerType == "liquid")
                    {
                        if (string.IsNullOrEmpty(fvm.density))
                        {
                            ModelState.AddModelError("density", "Required");
                            return View(fvm);
                        }
                        if (!Decimal.TryParse(fvm.density, out nmbrDensity))
                        {
                            ModelState.AddModelError("density", "Invalid");
                            return View(fvm);
                        }
                        else
                        {
                            if (nmbrDensity < 0 ||
                                nmbrDensity > 100)
                            {
                                ModelState.AddModelError("density", "Invalid");
                                return View(fvm);
                            }
                        }
                        if (fvm.selDenOption == 0)
                        {
                            ModelState.AddModelError("selDenOption", "Required");
                            return View(fvm);
                        }
                    }

                    if (fvm.manEntry)
                    {
                        if (string.IsNullOrEmpty(fvm.valN))
                        {
                            ModelState.AddModelError("valN", "Required");
                            return View(fvm);
                        }
                        if (!Decimal.TryParse(fvm.valN, out nmbrN))
                        {
                            ModelState.AddModelError("valN", "Invalid");
                            return View(fvm);
                        }
                        if (nmbrN < 0 || nmbrN > 100)
                        {
                            ModelState.AddModelError("valN", "Invalid");
                            return View(fvm);
                        }
                        if (string.IsNullOrEmpty(fvm.valP2o5))
                        {
                            ModelState.AddModelError("valP2o5", "Required");
                            return View(fvm);
                        }
                        if (!Decimal.TryParse(fvm.valP2o5, out nmbrP))
                        {
                            ModelState.AddModelError("valP2o5", "Invalid");
                            return View(fvm);
                        }
                        if (nmbrP < 0 || nmbrP > 100)
                        {
                            ModelState.AddModelError("valP2o5", "Invalid");
                            return View(fvm);
                        }
                        if (string.IsNullOrEmpty(fvm.valK2o))
                        {
                            ModelState.AddModelError("valK2o", "Required");
                            return View(fvm);
                        }
                        if (!Decimal.TryParse(fvm.valK2o, out nmbrK))
                        {
                            ModelState.AddModelError("valK2o", "Invalid");
                            return View(fvm);
                        }
                        if (nmbrK < 0 || nmbrK > 100)
                        {
                            ModelState.AddModelError("valK2o", "Invalid");
                            return View(fvm);
                        }
                    }

                    if (fvm.btnText == "Calculate")
                    {
                        ModelState.Clear();
                        FertilizerType ft = _sd.GetFertilizerType(fvm.selTypOption.ToString());

                        if (ft.DryLiquid == "liquid")
                        {
                            if (!ft.Custom)
                            {
                                if (fvm.density != _sd.GetLiquidFertilizerDensity(fvm.selFertOption, fvm.selDenOption).Value.ToString("#.##"))
                                {
                                    fvm.stdDensity = false;
                                }
                                else
                                {
                                    fvm.stdDensity = true;
                                }
                            }
                        }

                        var fertilizerNutrients = _calculateFertilizerNutrients.GetFertilizerNutrients(fvm.selFertOption,
                                fvm.fertilizerType,
                                Convert.ToDecimal(fvm.applRate),
                                Convert.ToInt32(fvm.selRateOption),
                                fvm.density != null ? Convert.ToDecimal(fvm.density) : 0,
                                Convert.ToInt16(fvm.selDenOption),
                                Convert.ToDecimal(fvm.valN),
                                Convert.ToDecimal(fvm.valP2o5),
                                Convert.ToDecimal(fvm.valK2o),
                                fvm.manEntry);

                        fvm.calcN = Convert.ToInt32(fertilizerNutrients.fertilizer_N).ToString();
                        fvm.calcP2o5 = Convert.ToInt32(fertilizerNutrients.fertilizer_P2O5).ToString();
                        fvm.calcK2o = Convert.ToInt32(fertilizerNutrients.fertilizer_K2O).ToString();

                        fvm.btnText = fvm.id == null ? "Add to Field" : "Update Field";

                        // temporarily update the farm data so as to recalc the Still Required amounts
                        if (fvm.id == null)
                        {
                            addedId = FertilizerInsert(fvm);
                        }
                        else
                        {
                            origFertilizer = _ud.GetFieldNutrientsFertilizer(fvm.fieldName, fvm.id.Value);
                            FertilizerUpdate(fvm);
                        }

                        FertilizerStillRequired(ref fvm);
                        if (fvm.id == null)
                        {
                            _ud.DeleteFieldNutrientsFertilizer(fvm.fieldName, addedId);
                        }
                        else
                        {
                            _ud.UpdateFieldNutrientsFertilizer(fvm.fieldName, origFertilizer);
                        }
                    }
                    else
                    {
                        if (fvm.id == null)
                        {
                            FertilizerInsert(fvm);
                        }
                        else
                        {
                            FertilizerUpdate(fvm);
                        }
                        return Json(ReDisplay("#fertilizer", fvm.fieldName));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error.");
                _logger.LogError(ex, "FertilizerDetails Exception");
            }

            return PartialView(fvm);
        }

        private int FertilizerInsert(FertilizerDetailsViewModel fvm)
        {
            NutrientFertilizer nf = new NutrientFertilizer()
            {
                fertilizerTypeId = Convert.ToInt32(fvm.selTypOption),
                fertilizerId = fvm.selFertOption,
                applUnitId = Convert.ToInt32(fvm.selRateOption),
                applRate = Convert.ToDecimal(fvm.applRate),
                applDate = string.IsNullOrEmpty(fvm.applDate) ? (DateTime?)null : Convert.ToDateTime(fvm.applDate),
                applMethodId = fvm.selMethOption,
                customN = fvm.manEntry ? Convert.ToDecimal(fvm.valN) : (decimal?)null,
                customP2o5 = fvm.manEntry ? Convert.ToDecimal(fvm.valP2o5) : (decimal?)null,
                customK2o = fvm.manEntry ? Convert.ToDecimal(fvm.valK2o) : (decimal?)null,
                fertN = Convert.ToDecimal(fvm.calcN),
                fertP2o5 = Convert.ToDecimal(fvm.calcP2o5),
                fertK2o = Convert.ToDecimal(fvm.calcK2o),
                liquidDensity = fvm.fertilizerType == "liquid" ? Convert.ToDecimal(fvm.density) : 0,
                liquidDensityUnitId = fvm.fertilizerType == "liquid" ? Convert.ToInt32(fvm.selDenOption) : 0
            };

            return _ud.AddFieldNutrientsFertilizer(fvm.fieldName, nf);
        }

        private void FertilizerUpdate(FertilizerDetailsViewModel fvm)
        {
            NutrientFertilizer nf = _ud.GetFieldNutrientsFertilizer(fvm.fieldName, fvm.id.Value);
            nf.fertilizerTypeId = Convert.ToInt32(fvm.selTypOption);
            nf.fertilizerId = fvm.selFertOption;
            nf.applUnitId = Convert.ToInt32(fvm.selRateOption);
            nf.applRate = Convert.ToDecimal(fvm.applRate);
            nf.applDate = string.IsNullOrEmpty(fvm.applDate) ? (DateTime?)null : Convert.ToDateTime(fvm.applDate);
            nf.applMethodId = fvm.selMethOption;
            nf.customN = fvm.manEntry ? Convert.ToDecimal(fvm.valN) : (decimal?)null;
            nf.customP2o5 = fvm.manEntry ? Convert.ToDecimal(fvm.valP2o5) : (decimal?)null;
            nf.customK2o = fvm.manEntry ? Convert.ToDecimal(fvm.valK2o) : (decimal?)null;
            nf.fertN = Convert.ToDecimal(fvm.calcN);
            nf.fertP2o5 = Convert.ToDecimal(fvm.calcP2o5);
            nf.fertK2o = Convert.ToDecimal(fvm.calcK2o);
            nf.liquidDensity = fvm.fertilizerType == "liquid" ? Convert.ToDecimal(fvm.density) : 0;
            nf.liquidDensityUnitId = fvm.fertilizerType == "liquid" ? Convert.ToInt32(fvm.selDenOption) : 0;

            _ud.UpdateFieldNutrientsFertilizer(fvm.fieldName, nf);
        }

        private void FertilizerDetailsSetup(ref FertilizerDetailsViewModel fvm)
        {
            fvm.typOptions = new List<SelectListItem>();
            fvm.typOptions = _sd.GetFertilizerTypesDll().ToList();

            fvm.denOptions = new List<SelectListItem>();
            fvm.denOptions = _sd.GetDensityUnitsDll().ToList();

            fvm.methOptions = new List<SelectListItem>();
            fvm.methOptions = _sd.GetFertilizerMethodsDll().ToList();

            FertilizerDetailSetup_Fertilizer(ref fvm);

            fvm.rateOptions = _sd.GetFertilizerUnitsDll(fvm.currUnit).ToList();

            return;
        }

        private void FertilizerDetailSetup_Fertilizer(ref FertilizerDetailsViewModel fvm)
        {
            fvm.fertOptions = new List<SelectListItem>();
            if (fvm.selTypOption != null &&
               fvm.selTypOption != "select")
            {
                FertilizerType ft = _sd.GetFertilizerType(fvm.selTypOption);
                if (!ft.Custom)
                {
                    fvm.fertOptions = _sd.GetFertilizersDll(ft.DryLiquid).ToList();
                }
                else
                {
                    fvm.fertOptions = new List<SelectListItem>() { new SelectListItem() { Id = 1, Value = "Custom" } };
                    fvm.selFertOption = 1;
                    fvm.stdDensity = true;
                }
            }
            else
            {
                fvm.fertOptions = new List<SelectListItem>();
                fvm.selFertOption = 0;
            }

            return;
        }

        private void FertilizerDetailSetup_DefaultDensity(ref FertilizerDetailsViewModel fvm)
        {
            if (fvm.selDenOption == 0)
            {
                fvm.density = "";
                fvm.stdDensity = true;
                return;
            }

            if (fvm.selFertOption == 0)
            {
                fvm.density = "";
                fvm.stdDensity = true;
            }

            if (!fvm.manEntry &&
                fvm.fertilizerType == "liquid" &&
                fvm.selFertOption != 0 &&
                fvm.selDenOption != 0)
            {
                fvm.density = _sd.GetLiquidFertilizerDensity(Convert.ToInt32(fvm.selFertOption), Convert.ToInt32(fvm.selDenOption)).Value.ToString("#.##");
                fvm.stdDensity = true;
            }

            return;
        }

        private void FertilizerDetail_Reset(ref FertilizerDetailsViewModel fvm)
        {
            fvm.calcN = "0";
            fvm.calcK2o = "0";
            fvm.calcP2o5 = "0";
            fvm.valN = "0";
            fvm.valP2o5 = "0";
            fvm.valK2o = "0";

            return;
        }

        [HttpGet]
        public ActionResult FertilizerDelete(string fldName, int id)
        {
            string fertilizerName = string.Empty;

            FertilizerDeleteViewModel fvm = new FertilizerDeleteViewModel();
            fvm.id = id;
            fvm.fldName = fldName;

            NutrientFertilizer nf = _ud.GetFieldNutrientsFertilizer(fldName, id);
            FertilizerType ft = _sd.GetFertilizerType(nf.fertilizerTypeId.ToString());

            if (ft.Custom)
            {
                fertilizerName = ft.DryLiquid == "dry" ? "Custom (Dry)" : "Custom (Liquid)";
            }
            else
            {
                Fertilizer ff = _sd.GetFertilizer(nf.fertilizerId.ToString());
                fertilizerName = ff.Name;
            }

            fvm.fertilizerName = fertilizerName;

            fvm.act = "Delete";

            return PartialView("FertilizerDelete", fvm);
        }

        [HttpPost]
        public ActionResult FertilizerDelete(FertilizerDeleteViewModel dvm)
        {
            if (ModelState.IsValid)
            {
                _ud.DeleteFieldNutrientsFertilizer(dvm.fldName, dvm.id);

                return Json(ReDisplay("#fertilizer", dvm.fldName));
            }
            return PartialView("FertilizerDelete", dvm);
        }

        public IActionResult RefreshManureList(string fieldName)
        {
            return ViewComponent("CalcManure", new { fldName = fieldName });
        }

        public IActionResult RefreshFertilizerList(string fieldName)
        {
            return ViewComponent("CalcFertilizer", new { fldName = fieldName });
        }

        public IActionResult RefreshFieldList(string fieldName)
        {
            return RedirectToAction("Calculate", "Nutrients", new { nme = fieldName });
            //return ViewComponent("FieldList");
        }

        public IActionResult RefreshSummary(string fieldName)
        {
            return ViewComponent("CalcSummary", new { fldName = fieldName });
        }

        public IActionResult RefreshHeading(string fieldName)
        {
            return ViewComponent("CalcHeading", new { fldName = fieldName });
        }

        public IActionResult RefreshMessages(string fieldName)
        {
            return ViewComponent("CalcMessages", new { fldName = fieldName });
        }

        [HttpGet]
        public ActionResult ManureDelete(string fldName, int id)
        {
            ManureDeleteViewModel dvm = new ManureDeleteViewModel();
            dvm.id = id;
            dvm.fldName = fldName;

            NutrientManure nm = _ud.GetFieldNutrientsManure(fldName, id);
            dvm.matType = _ud.GetFarmManure(Convert.ToInt32(nm.manureId)).Name;

            dvm.act = "Delete";

            return PartialView("ManureDelete", dvm);
        }

        [HttpPost]
        public ActionResult ManureDelete(ManureDeleteViewModel dvm)
        {
            if (ModelState.IsValid)
            {
                _ud.DeleteFieldNutrientsManure(dvm.fldName, dvm.id);

                return Json(ReDisplay("#manure", dvm.fldName));
            }
            return PartialView("ManureDelete", dvm);
        }

        public IActionResult RefreshCropList(string fieldName)
        {
            return ViewComponent("CalcCrops", new { fldName = fieldName });
        }

        public IActionResult RefreshOtherList(string fieldName)
        {
            return ViewComponent("CalcOther", new { fldName = fieldName });
        }

        [HttpGet]
        public ActionResult CropDelete(string fldName, int id)
        {
            CropDeleteViewModel dvm = new CropDeleteViewModel();
            dvm.id = id;
            dvm.fldName = fldName;

            FieldCrop crp = _ud.GetFieldCrop(fldName, id);
            if (!string.IsNullOrEmpty(crp.cropOther))
                dvm.cropName = crp.cropOther;
            else
                dvm.cropName = _sd.GetCrop(Convert.ToInt32(crp.cropId)).CropName;

            dvm.act = "Delete";

            return PartialView("CropDelete", dvm);
        }

        [HttpPost]
        public ActionResult CropDelete(CropDeleteViewModel dvm)
        {
            if (ModelState.IsValid)
            {
                _ud.DeleteFieldCrop(dvm.fldName, dvm.id);

                return Json(ReDisplay("#crop", dvm.fldName));
            }
            return PartialView("CropDelete", dvm);
        }

        public IActionResult OtherDetails(string fldName, int? id)
        {
            var ovm = new OtherDetailsViewModel();

            ovm.fieldName = fldName;
            ovm.title = id == null ? "Add" : "Edit";
            ovm.btnText = id == null ? "Add to Field" : "Update Field";
            ovm.id = id;
            ovm.url = _sd.GetExternalLink("othernutrientexplanation");
            ovm.urlText = _sd.GetUserPrompt("moreinfo");
            ovm.placehldr = _sd.GetUserPrompt("othernutrientplaceholder");
            ovm.ExplainCalculateOtherNutrientSource = _sd.GetUserPrompt("CalculateOtherNutrientSourceMessage");
            ovm.ExplainCalculateOtherNutrientAvailableThisYear = _sd.GetUserPrompt("CalculateOtherNutrientAvailableThisYearMessage");
            ovm.ExplainCalculateOtherNutrientAvailbleLongTerm = _sd.GetUserPrompt("CalculateOtherNutrientAvailbleLongTermMessage");

            if (id != null)
            {
                ovm.title = "Edit";

                NutrientOther no = _ud.GetFieldNutrientsOther(fldName, id.Value);
                ovm.source = no.description;
                ovm.yrN = no.yrN.ToString("G29");
                ovm.yrP = no.yrP2o5.ToString("G29");
                ovm.yrK = no.yrK.ToString("G29");
                ovm.ltN = no.ltN.ToString("G29");
                ovm.ltP = no.ltP2o5.ToString("G29");
                ovm.ltK = no.ltK.ToString("G29");
            }
            else

            {
                ovm.title = "Add";
            }

            return PartialView(ovm);
        }

        [HttpPost]
        public IActionResult OtherDetails(OtherDetailsViewModel ovm)
        {
            if (ModelState.IsValid)
            {
                decimal tmp = 0;

                if (!(string.IsNullOrEmpty(ovm.ltN)))
                {
                    if (decimal.TryParse(ovm.ltN, out tmp))
                    {
                        if (tmp < 0 ||
                            tmp > 1000)
                        {
                            ModelState.AddModelError("ltN", "Invalid.");
                            return View(ovm);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ltN", "Not a valid number.");
                        return View(ovm);
                    }
                }
                else
                {
                    ovm.ltN = "0";
                }

                if (!(string.IsNullOrEmpty(ovm.ltP)))
                {
                    if (decimal.TryParse(ovm.ltP, out tmp))
                    {
                        if (tmp < 0 ||
                            tmp > 1000)
                        {
                            ModelState.AddModelError("ltP", "Invalid.");
                            return View(ovm);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ltP", "Not a valid number.");
                        return View(ovm);
                    }
                }
                else
                {
                    ovm.ltP = "0";
                }

                if (!(string.IsNullOrEmpty(ovm.ltK)))
                {
                    if (decimal.TryParse(ovm.ltK, out tmp))

                    {
                        if (tmp < 0 ||
                            tmp > 1000)
                        {
                            ModelState.AddModelError("ltK", "Invalid.");
                            return View(ovm);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ltK", "Not a valid number.");
                        return View(ovm);
                    }
                }
                else
                {
                    ovm.ltK = "0";
                }
                if (!(string.IsNullOrEmpty(ovm.yrN)))
                {
                    if (decimal.TryParse(ovm.yrN, out tmp))
                    {
                        if (tmp < 0 ||
                            tmp > 1000)
                        {
                            ModelState.AddModelError("yrN", "Invalid.");
                            return View(ovm);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("yrN", "Not a valid number.");
                        return View(ovm);
                    }
                }
                else
                {
                    ovm.yrN = "0";
                }

                if (!(string.IsNullOrEmpty(ovm.yrP)))
                {
                    if (decimal.TryParse(ovm.yrP, out tmp))
                    {
                        if (tmp < 0 ||
                            tmp > 1000)
                        {
                            ModelState.AddModelError("yrP", "Invalid.");
                            return View(ovm);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("yrP", "Not a valid number.");
                        return View(ovm);
                    }
                }
                else
                {
                    ovm.yrP = "0";
                }

                if (!(string.IsNullOrEmpty(ovm.yrK)))
                {
                    if (decimal.TryParse(ovm.yrK, out tmp))
                    {
                        if (tmp < 0 ||
                            tmp > 1000)
                        {
                            ModelState.AddModelError("yrK", "Invalid.");
                            return View(ovm);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("yrK", "Not a valid number.");
                        return View(ovm);
                    }
                }
                else
                {
                    ovm.yrK = "0";
                }
                if (ovm.id == null)
                {
                    NutrientOther no = new NutrientOther()
                    {
                        description = ovm.source,
                        ltN = Convert.ToDecimal(ovm.ltN),
                        ltP2o5 = Convert.ToDecimal(ovm.ltP),
                        ltK = Convert.ToDecimal(ovm.ltK),
                        yrN = Convert.ToDecimal(ovm.yrN),
                        yrP2o5 = Convert.ToDecimal(ovm.yrP),
                        yrK = Convert.ToDecimal(ovm.yrK)
                    };

                    _ud.AddFieldNutrientsOther(ovm.fieldName, no);
                }
                else
                {
                    NutrientOther no = _ud.GetFieldNutrientsOther(ovm.fieldName, ovm.id.Value);
                    no.description = ovm.source;
                    no.ltN = Convert.ToDecimal(ovm.ltN);
                    no.ltP2o5 = Convert.ToDecimal(ovm.ltP);
                    no.ltK = Convert.ToDecimal(ovm.ltK);
                    no.yrN = Convert.ToDecimal(ovm.yrN);
                    no.yrP2o5 = Convert.ToDecimal(ovm.yrP);
                    no.yrK = Convert.ToDecimal(ovm.yrK);

                    _ud.UpdateFieldNutrientsOther(ovm.fieldName, no);
                }

                return Json(ReDisplay("#other", ovm.fieldName));
            }
            else
            {
                ModelState.Remove("yrN");
                ModelState.Remove("yrP");
                ModelState.Remove("yrK");
                ModelState.Remove("ltN");
                ModelState.Remove("ltP");
                ModelState.Remove("ltK");
                ovm.yrN = string.IsNullOrEmpty(ovm.yrN) ? "0" : ovm.yrN;
                ovm.yrP = string.IsNullOrEmpty(ovm.yrP) ? "0" : ovm.yrP;
                ovm.yrK = string.IsNullOrEmpty(ovm.yrK) ? "0" : ovm.yrK;
                ovm.ltN = string.IsNullOrEmpty(ovm.ltN) ? "0" : ovm.ltN;
                ovm.ltP = string.IsNullOrEmpty(ovm.ltP) ? "0" : ovm.ltP;
                ovm.ltK = string.IsNullOrEmpty(ovm.ltK) ? "0" : ovm.ltK;
            }
            return View(ovm);
        }

        [HttpGet]
        public ActionResult OtherDelete(string fldName, int id)
        {
            OtherDeleteViewModel ovm = new OtherDeleteViewModel();
            ovm.id = id;
            ovm.fldName = fldName;

            NutrientOther no = _ud.GetFieldNutrientsOther(fldName, id);
            ovm.source = no.description;

            ovm.act = "Delete";

            return PartialView("OtherDelete", ovm);
        }

        [HttpPost]
        public ActionResult OtherDelete(OtherDeleteViewModel ovm)
        {
            if (ModelState.IsValid)
            {
                _ud.DeleteFieldNutrientsOther(ovm.fldName, ovm.id);

                return Json(ReDisplay("#other", ovm.fldName));
            }
            return PartialView("OtherDelete", ovm);
        }

        public object ReDisplay(string target, string fldName)
        {
            string refresher = "";
            bool reload = false;
            // refactor - impact of adding Nitrogen credit?
            if (ItemCount(fldName) < 2)
            {
                reload = true;
            }

            string urlFields = string.Empty;

            switch (target)
            {
                case "#crop":
                    refresher = "RefreshCropList";
                    reload = true;
                    break;

                case "#manure":
                    refresher = "RefreshManureList";
                    break;

                case "#other":
                    refresher = "RefreshOtherList";
                    break;

                case "#fertilizer":
                    refresher = "RefreshFertilizerList";
                    break;

                case "#prevYearManure":
                    refresher = "RefreshNitrogenCredit";
                    break;

                case "#soilTestNitrogenCredit":
                    refresher = "RefreshSoilTestNitrogenCredit";
                    break;
            }
            string url = Url.Action(refresher, "Nutrients", new { fieldName = fldName });
            string urlSumm = Url.Action("RefreshSummary", "Nutrients", new { fieldName = fldName });
            string urlHead = Url.Action("RefreshHeading", "Nutrients", new { fieldName = fldName });
            string urlMsg = Url.Action("RefreshMessages", "Nutrients", new { fieldName = fldName });
            string urlPrevManureNitrogenCredit = Url.Action("RefreshNitrogenCredit", "Nutrients", new { fieldName = fldName });

            var result = new { success = true, url = url, target = target, urlSumm = urlSumm, urlHead = urlHead, urlMsg = urlMsg, reload = reload, urlPrevManureNitrogenCredit = urlPrevManureNitrogenCredit };
            return result;
        }

        [HttpGet]
        public ActionResult InfoMessage(string type)
        {
            InfoAgriViewModel ivm = new InfoAgriViewModel();

            switch (type)
            {
                case "agri":
                    ivm.text = _sd.GetUserPrompt("agriinfomessage");
                    break;

                case "crop":
                    ivm.text = _sd.GetUserPrompt("cropinfomessage");
                    break;

                case "download":
                    ivm.text = _sd.GetUserPrompt("downloadinfomessage");
                    break;

                case "load":
                    ivm.text = _sd.GetUserPrompt("cropinfomessage");
                    break;

                case "soiltest":
                    ivm.text = _sd.GetUserPrompt("soiltest");
                    break;

                case "leaftest":
                    ivm.text = _sd.GetUserPrompt("leaftest");
                    break;
            }
            return PartialView("InfoMessage", ivm);
        }

        public IActionResult SaveWarning(string target)
        {
            SaveWarningViewModel svm = new SaveWarningViewModel();
            svm.target = target;
            svm.msg = _sd.GetUserPrompt("finishwithoutsaving");

            return PartialView("SaveWarning", svm);
        }

        [HttpGet]
        public object CheckUnsaved()
        {
            var result = new { unsaved = _ud.FarmData().unsaved.ToString() };
            return result;
        }

        public IActionResult RefreshNitrogenCredit(string fieldName)
        {
            return ViewComponent("CalcPrevYearManure", new { fldName = fieldName });
        }

        private bool SaveNitrogenCreditToField(string fldName, int nitrogenCredit, int nitrogenCreditDefault)
        {
            try
            {
                Field fld;
                fld = _ud.GetFieldDetails(fldName);
                if (nitrogenCredit != nitrogenCreditDefault)
                    fld.PreviousYearManureApplicationNitrogenCredit = nitrogenCredit;
                else // only save non-defaulted value (ie. over-ride)
                    fld.PreviousYearManureApplicationNitrogenCredit = null;
                _ud.UpdateField(fld);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "SaveNitrogenCreditToField Exception");
                return false;
            }
            return true;
        }

        [HttpGet]
        public IActionResult PrevYearManureApplicationDetails(string fldName)
        {
            PrevYearManureApplicationViewModel model = new PrevYearManureApplicationViewModel();

            model.url = _sd.GetExternalLink("prevmanureexplanation");
            model.urlText = _sd.GetUserPrompt("moreinfo");
            model.ExplainNitrogenFromPrevManure = _sd.GetUserPrompt("CalculateNutrientsPrevManureMessage");
            var field = _ud.GetFieldDetails(fldName);
            model.defaultNitrogenCredit = _chemicalBalanceMessage.CalcPrevYearManureApplDefault(field).ToString();
            model.fldName = fldName;
            if (field.PreviousYearManureApplicationNitrogenCredit != null)
                model.nitrogen = Convert.ToInt32(field.PreviousYearManureApplicationNitrogenCredit).ToString();
            else
                model.nitrogen = model.defaultNitrogenCredit;

            return PartialView(model);
        }

        [HttpPost]
        public IActionResult PrevYearManureApplicationDetails(PrevYearManureApplicationViewModel model)
        {
            int nitrogenCredit = 0;

            if (ModelState.IsValid)
            {
                try
                {
                    nitrogenCredit = Convert.ToInt32(model.nitrogen);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Nitrogen", "An invalid Nitrogen Credit value was entered. Nitrogen credit must be an integer with a value which is greater than or equal to zero");
                    _logger.LogError(ex, "nitrogenCredit = Convert.ToInt32(model.nitrogen) failed");
                    return PartialView(model);
                }
                if (nitrogenCredit >= 0)
                {
                    if (SaveNitrogenCreditToField(model.fldName, nitrogenCredit, Convert.ToInt32(model.defaultNitrogenCredit)))
                    {
                        return Json(ReDisplay("#prevYearManure", model.fldName));
                    }
                    else
                        ModelState.AddModelError("Nitrogen", "Unable to save changes made to the Nitrogen credit.");
                }
                else
                    ModelState.AddModelError("Nitrogen", "Nitrogen Credit cannot be a negative value. It must be greater than or equal to zero");
            } // ...modelstate
            return PartialView(model);
        }

        private bool SaveSoilTestNitrogenCreditToField(string fldName, decimal nitrogenCredit, decimal nitrogenCreditDefault)
        {
            try
            {
                Field fld;
                fld = _ud.GetFieldDetails(fldName);
                if (nitrogenCredit != nitrogenCreditDefault)
                    fld.SoilTestNitrateOverrideNitrogenCredit = nitrogenCredit;
                else // only save non-defaulted value (ie. over-ride)
                    fld.SoilTestNitrateOverrideNitrogenCredit = null;
                _ud.UpdateField(fld);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "SaveSoilTestNitrogenCreditToField Exception");
                return false;
            }
            return true;
        }

        public IActionResult RefreshSoilTestNitrogenCredit(string fieldName)
        {
            return ViewComponent("SoilTestNitrateOverride", new { fldName = fieldName });
        }

        [HttpGet]
        public IActionResult SoilTestNitrateOverrideDetails(string fldName)
        {
            SoilTestNitrateOverrideViewModel model = new SoilTestNitrateOverrideViewModel();

            model.url = _sd.GetExternalLink("soilnitrateexplanation");
            model.urlText = _sd.GetUserPrompt("moreinfo");
            model.ExplainCalculateNutrientsNitrogenNitrate = _sd.GetUserPrompt("CalculateNutrientsNitrogenNitrateMessage");
            var field = _ud.GetFieldDetails(fldName);
            model.defaultNitrogenCredit = _chemicalBalanceMessage.CalcSoitTestNitrateDefault(field).ToString();
            model.fldName = fldName;
            if (field.SoilTestNitrateOverrideNitrogenCredit != null)
                model.nitrogen = field.SoilTestNitrateOverrideNitrogenCredit.ToString();
            else
                model.nitrogen = model.defaultNitrogenCredit;

            return PartialView(model);
        }

        [HttpPost]
        public IActionResult SoilTestNitrateOverrideDetails(SoilTestNitrateOverrideViewModel model)
        {
            decimal nitrogenCredit = 0;

            if (ModelState.IsValid)
            {
                try
                {
                    nitrogenCredit = Convert.ToDecimal(model.nitrogen);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Nitrogen", "An invalid value was entered. Nitrogen credit must be greater than or equal to zero");
                    _logger.LogError(ex, "nitrogenCredit = Convert.ToDecimal(model.nitrogen) failed");
                    return PartialView(model);
                }
                if (nitrogenCredit >= 0)
                {
                    if (SaveSoilTestNitrogenCreditToField(model.fldName, nitrogenCredit, Convert.ToDecimal(model.defaultNitrogenCredit)))
                    {
                        return Json(ReDisplay("#soilTestNitrogenCredit", model.fldName));
                    }
                    else
                        ModelState.AddModelError("Nitrogen", "Unable to save changes made to the Nitrogen credit.");
                }
                else
                    ModelState.AddModelError("Nitrogen", "An invalid value was entered.  Nitrogen Credit must be greater than or equal to zero");
            } // ...modelstate
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult FeedAreaEditNote()
        {
            var model = new FeedAreaEditNoteViewModel
            {
                Message = _sd.GetUserPrompt("FeedAreaCalculation-Ranch")
            };

            return PartialView(model);
        }
    }
}
