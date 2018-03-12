using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.ViewModels;
using SERVERAPI.Models;
using SERVERAPI.Models.Impl;
using SERVERAPI.Utility;
using static SERVERAPI.Models.StaticData;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SERVERAPI.Controllers
{
    //[RedirectingAction]
    public class NutrientsController : Controller
    {
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public Models.Impl.StaticData _sd { get; set; }
        public IViewRenderService _viewRenderService { get; set; }
        public AppSettings _settings;

        public NutrientsController(IHostingEnvironment env, UserData ud, Models.Impl.StaticData sd, IOptions<AppSettings> settings)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
            _settings = settings.Value;
        }
        // GET: /<controller>/
        public IActionResult Calculate(string nme)
        {
            FarmDetails fd =_ud.FarmDetails();

            CalculateViewModel cvm = new CalculateViewModel
            {
                fields = new List<Field>()
            };

            cvm.regionFnd = (fd.farmRegion.HasValue) ? true : false;

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
                    cvm.currFld = cvm.fields[0].fieldName;
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

                if (!cvm.itemsPresent)
                {
                    cvm.noData = _sd.GetUserPrompt("nonutrientitems");
                }
            }

            return View(cvm);
        }
        [HttpPost]
        public IActionResult Calculate(CalculateViewModel cvm)
        {

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
            Utility.CalculateNutrients calculateNutrients = new CalculateNutrients(_env, _ud, _sd);
            NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            ManureDetailsViewModel mvm = new ManureDetailsViewModel();

            mvm.fieldName = fldName;
            mvm.title = id == null ? "Add" : "Edit";
            mvm.btnText = id == null ? "Calculate" : "Return";
            mvm.id = id;
            mvm.avail = string.Empty;
            mvm.nh4 = string.Empty;
            mvm.stdN = true;
            mvm.stdAvail = true;
            mvm.url = _sd.GetExternalLink("manureunitexplanation");
            mvm.urlText = _sd.GetUserPrompt("moreinfo");

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
                mvm.selManOption = nm.manureId;
                mvm.rate = nm.rate.ToString();
                mvm.nh4 = nm.nh4Retention.ToString("###");
                mvm.yrN = nm.yrN.ToString();
                mvm.yrP2o5 = nm.yrP2o5.ToString();
                mvm.yrK2o = nm.yrK2o.ToString();
                mvm.ltN = nm.ltN.ToString();
                mvm.ltP2o5 = nm.ltP2o5.ToString();
                mvm.ltK2o = nm.ltK2o.ToString();
                FarmManure man = _ud.GetFarmManure(Convert.ToInt32(nm.manureId));
                mvm.currUnit = man.solid_liquid;
                mvm.rateOptions = _sd.GetUnitsDll(mvm.currUnit).ToList();

                int regionid = _ud.FarmDetails().farmRegion.Value;
                Region region = _sd.GetRegion(regionid);
                nOrganicMineralizations = calculateNutrients.GetNMineralization(Convert.ToInt16(mvm.selManOption), region.locationid);

                mvm.stdN = Convert.ToDecimal(mvm.nh4) != (calculateNutrients.GetAmmoniaRetention(Convert.ToInt16(mvm.selManOption), Convert.ToInt16(mvm.selApplOption)) * 100) ? false : true;
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
            }

            ManureDetailsSetup(ref mvm);

            MaunureStillRequired(ref mvm);

            return PartialView(mvm);
        }
        private void MaunureStillRequired(ref ManureDetailsViewModel mvm)
        {
            //recalc totals for display
            ChemicalBalanceMessage cbm = new ChemicalBalanceMessage(_ud, _sd);
            ChemicalBalances chemicalBalances = new ChemicalBalances();

            chemicalBalances = cbm.GetChemicalBalances(mvm.fieldName);

            List<BalanceMessages> msgs = cbm.DetermineBalanceMessages(mvm.fieldName);

            foreach (var m in msgs)
            {
                switch (m.Chemical)
                {
                    case "AgrN":
                        mvm.totNIcon = (chemicalBalances.balance_AgrN > 0) ? "" : m.Icon;
                        break;
                    case "AgrP2O5":
                        mvm.totPIcon = (chemicalBalances.balance_AgrP2O5 > 0) ? "" : m.Icon;
                        break;
                    case "AgrK2O":
                        mvm.totKIcon = (chemicalBalances.balance_AgrK2O > 0) ? "" : m.Icon;
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
            NutrientManure origManure = new NutrientManure();

            Utility.CalculateNutrients calculateNutrients = new CalculateNutrients(_env, _ud, _sd);
            NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            ManureDetailsSetup(ref mvm);

            try
            {

                if (mvm.buttonPressed == "ResetN")
                {
                    ModelState.Clear();
                    mvm.buttonPressed = "";
                    mvm.btnText = "Calculate";

                    // reset to calculated amount                
                    mvm.nh4 = (calculateNutrients.GetAmmoniaRetention(Convert.ToInt16(mvm.selManOption), Convert.ToInt16(mvm.selApplOption)) * 100).ToString("###");

                    mvm.stdN = true;
                    return View(mvm);
                }

                if (mvm.buttonPressed == "ResetA")
                {
                    ModelState.Clear();
                    mvm.buttonPressed = "";
                    mvm.btnText = "Calculate";

                    // reset to calculated amount
                    int regionid = _ud.FarmDetails().farmRegion.Value;
                    Region region = _sd.GetRegion(regionid);
                    nOrganicMineralizations = calculateNutrients.GetNMineralization(Convert.ToInt16(mvm.selManOption), region.locationid);

                    mvm.avail = (nOrganicMineralizations.OrganicN_FirstYear * 100).ToString("###");

                    mvm.stdAvail = true;
                    return View(mvm);
                }

                if (mvm.buttonPressed == "TypeChange")
                {
                    ModelState.Clear();
                    mvm.buttonPressed = "";
                    mvm.btnText = "Calculate";

                    if (mvm.selManOption != "")
                    {
                        FarmManure man = _ud.GetFarmManure(Convert.ToInt32(mvm.selManOption));
                        if (mvm.currUnit != man.solid_liquid)
                        {
                            mvm.currUnit = man.solid_liquid;
                            mvm.rateOptions = _sd.GetUnitsDll(mvm.currUnit).ToList();
                            mvm.selRateOption = mvm.rateOptions[0].Id.ToString();
                        }

                        // if application is present then recalc N and A
                        int regionid = _ud.FarmDetails().farmRegion.Value;
                        Region region = _sd.GetRegion(regionid);
                        nOrganicMineralizations = calculateNutrients.GetNMineralization(Convert.ToInt16(mvm.selManOption), region.locationid);

                        mvm.avail = (nOrganicMineralizations.OrganicN_FirstYear * 100).ToString("###");

                        if (mvm.selApplOption != "" &&
                            mvm.selApplOption != "select")
                        {
                            // recalc N and A values
                            mvm.nh4 = (calculateNutrients.GetAmmoniaRetention(Convert.ToInt16(mvm.selManOption), Convert.ToInt16(mvm.selApplOption)) * 100).ToString("###");
                        }
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
                        mvm.nh4 = (calculateNutrients.GetAmmoniaRetention(Convert.ToInt16(mvm.selManOption), Convert.ToInt16(mvm.selApplOption)) * 100).ToString("###");
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
                        NutrientInputs nutrientInputs = new NutrientInputs();

                        calculateNutrients.manure = mvm.selManOption;
                        calculateNutrients.applicationSeason = mvm.selApplOption;
                        calculateNutrients.applicationRate = Convert.ToDecimal(mvm.rate);
                        calculateNutrients.applicationRateUnits = mvm.selRateOption;
                        calculateNutrients.ammoniaNRetentionPct = Convert.ToDecimal(mvm.nh4);
                        calculateNutrients.firstYearOrganicNAvailablityPct = Convert.ToDecimal(mvm.avail);

                        calculateNutrients.GetNutrientInputs(nutrientInputs);

                        mvm.yrN = nutrientInputs.N_FirstYear.ToString();
                        mvm.yrP2o5 = nutrientInputs.P2O5_FirstYear.ToString();
                        mvm.yrK2o = nutrientInputs.K2O_FirstYear.ToString();
                        mvm.ltN = nutrientInputs.N_LongTerm.ToString();
                        mvm.ltP2o5 = nutrientInputs.P2O5_LongTerm.ToString();
                        mvm.ltK2o = nutrientInputs.K2O_LongTerm.ToString();

                        mvm.btnText = mvm.id == null ? "Add to Field" : "Update Field";

                        // determine if values on screen are book value or not
                        int regionid = _ud.FarmDetails().farmRegion.Value;
                        Region region = _sd.GetRegion(regionid);
                        nOrganicMineralizations = calculateNutrients.GetNMineralization(Convert.ToInt16(mvm.selManOption), region.locationid);

                        if (Convert.ToDecimal(mvm.nh4) != (calculateNutrients.GetAmmoniaRetention(Convert.ToInt16(mvm.selManOption), Convert.ToInt16(mvm.selApplOption)) * 100))
                        {
                            mvm.stdN = false;
                        }
                        if (Convert.ToDecimal(mvm.avail) != (nOrganicMineralizations.OrganicN_FirstYear * 100))
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
            }

            return PartialView(mvm);
        }
        private int ManureInsert(ManureDetailsViewModel mvm)
        {
            NutrientManure nm = new NutrientManure()
            {
                manureId = mvm.selManOption,
                applicationId = mvm.selApplOption,
                unitId = mvm.selRateOption,
                rate = Convert.ToDecimal(mvm.rate),
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
            nm.manureId = mvm.selManOption;
            nm.applicationId = mvm.selApplOption;
            nm.unitId = mvm.selRateOption;
            nm.rate = Convert.ToDecimal(mvm.rate);
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
            mvm.manOptions = new List<Models.StaticData.SelectListItem>();
            mvm.manOptions = _ud.GetFarmManuresDll().ToList();

            mvm.applOptions = new List<Models.StaticData.SelectListItem>();
            if (mvm.selManOption != null &&
                mvm.selManOption != "select" &&
                mvm.selManOption != "")
            {
                FarmManure fm = _ud.GetFarmManure(Convert.ToInt32(mvm.selManOption));
                mvm.applOptions = _sd.GetApplicationsDll(_sd.GetManure(fm.manureId.ToString()).solid_liquid).ToList();
            }

            mvm.rateOptions = new List<Models.StaticData.SelectListItem>();
            mvm.rateOptions = _sd.GetUnitsDll(mvm.currUnit).ToList();
            mvm.selRateOptionText = "(lb/ac)";

            return;
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

                fvm.currUnit = ft.dry_liquid;
                fvm.selFertOption = ft.custom ? 1 : nf.fertilizerId;
                fvm.applRate = nf.applRate.ToString("#.##");
                fvm.selRateOption = nf.applUnitId.ToString();
                fvm.selMethOption = nf.applMethodId;
                fvm.fertilizerType = ft.dry_liquid;
                fvm.calcN = nf.fertN.ToString();
                fvm.calcP2o5 = nf.fertP2o5.ToString();
                fvm.calcK2o = nf.fertK2o.ToString();
                if (nf.applDate.HasValue)
                {
                    fvm.applDate = nf.applDate.HasValue ? nf.applDate.Value.ToString("MMM-yyyy") : "";
                }
                if (ft.dry_liquid == "liquid")
                {
                    fvm.density = nf.liquidDensity.ToString("#.##");
                    fvm.selDenOption = nf.liquidDensityUnitId;
                    if (!ft.custom)
                    {
                        if (fvm.density != _sd.GetLiquidFertilizerDensity(nf.fertilizerId, nf.liquidDensityUnitId).value.ToString("#.##"))
                        {
                            fvm.stdDensity = false;
                        }
                        else
                        {
                            fvm.stdDensity = true;
                        }
                    }
                }
                if (ft.custom)
                {
                    fvm.valN = nf.customN.Value.ToString("0");
                    fvm.valP2o5 = nf.customP2o5.Value.ToString("0");
                    fvm.valK2o = nf.customK2o.Value.ToString("0");
                    fvm.manEntry = true;
                }
                else
                {
                    Fertilizer ff = _sd.GetFertilizer(nf.fertilizerId.ToString());
                    fvm.valN = ff.nitrogen.ToString("0");
                    fvm.valP2o5 = ff.phosphorous.ToString("0");
                    fvm.valK2o = ff.potassium.ToString("0");
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
            ChemicalBalanceMessage cbm = new ChemicalBalanceMessage(_ud, _sd);
            ChemicalBalances chemicalBalances = new ChemicalBalances();

            chemicalBalances = cbm.GetChemicalBalances(fvm.fieldName);

            List<BalanceMessages> msgs = cbm.DetermineBalanceMessages(fvm.fieldName);

            foreach (var m in msgs)
            {
                switch (m.Chemical)
                {
                    case "AgrN":
                        fvm.totNIcon = (chemicalBalances.balance_AgrN > 0) ? "" : m.Icon;
                        break;
                    case "AgrP2O5":
                        fvm.totPIcon = (chemicalBalances.balance_AgrP2O5 > 0) ? "" : m.Icon;
                        break;
                    case "AgrK2O":
                        fvm.totKIcon = (chemicalBalances.balance_AgrK2O > 0) ? "" : m.Icon;
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

                    fvm.density = _sd.GetLiquidFertilizerDensity(nf.fertilizerId, nf.liquidDensityUnitId).value.ToString("#.##");

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
                        Models.StaticData.FertilizerType typ = _sd.GetFertilizerType(fvm.selTypOption);

                        fvm.density = "";


                        if (fvm.currUnit != typ.dry_liquid)
                        {
                            fvm.currUnit = typ.dry_liquid;
                            fvm.rateOptions = _sd.GetFertilizerUnitsDll(fvm.currUnit).ToList();
                            fvm.selRateOption = fvm.rateOptions[0].Id.ToString();
                            fvm.fertilizerType = typ.dry_liquid;
                        }

                        fvm.manEntry = typ.custom;
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
                        fvm.valN = ft.nitrogen.ToString("0");
                        fvm.valP2o5 = ft.phosphorous.ToString("0");
                        fvm.valK2o = ft.potassium.ToString("0");

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

                        if (ft.dry_liquid == "liquid")
                        {
                            if (!ft.custom)
                            {
                                if (fvm.density != _sd.GetLiquidFertilizerDensity(fvm.selFertOption, fvm.selDenOption).value.ToString("#.##"))
                                {
                                    fvm.stdDensity = false;
                                }
                                else
                                {
                                    fvm.stdDensity = true;
                                }
                            }
                        }

                        CalculateFertilizerNutrients calculateFertilizerNutrients = new CalculateFertilizerNutrients(_ud, _sd);
                        calculateFertilizerNutrients.FertilizerId = fvm.selFertOption;
                        calculateFertilizerNutrients.FertilizerType = fvm.fertilizerType;
                        calculateFertilizerNutrients.ApplicationRate = Convert.ToDecimal(fvm.applRate);
                        calculateFertilizerNutrients.ApplicationRateUnits = Convert.ToInt32(fvm.selRateOption);
                        if (fvm.density != null)
                            calculateFertilizerNutrients.Density = Convert.ToDecimal(fvm.density);
                        calculateFertilizerNutrients.DensityUnits = Convert.ToInt16(fvm.selDenOption);
                        calculateFertilizerNutrients.userN = Convert.ToDecimal(fvm.valN);
                        calculateFertilizerNutrients.userP2o5 = Convert.ToDecimal(fvm.valP2o5);
                        calculateFertilizerNutrients.userK2o = Convert.ToDecimal(fvm.valK2o);
                        calculateFertilizerNutrients.CustomFertilizer = fvm.manEntry;

                        FertilizerNutrients fertilizerNutrients = calculateFertilizerNutrients.GetFertilizerNutrients();

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
            fvm.typOptions = new List<Models.StaticData.SelectListItem>();
            fvm.typOptions = _sd.GetFertilizerTypesDll().ToList();

            fvm.denOptions = new List<Models.StaticData.SelectListItem>();
            fvm.denOptions = _sd.GetDensityUnitsDll().ToList();

            fvm.methOptions = new List<Models.StaticData.SelectListItem>();
            fvm.methOptions = _sd.GetFertilizerMethodsDll().ToList();

            FertilizerDetailSetup_Fertilizer(ref fvm);

            //fvm.rateOptions = new List<Models.StaticData.SelectListItem>();
            fvm.rateOptions = _sd.GetFertilizerUnitsDll(fvm.currUnit).ToList();

            //fvm.rateOptions = _sd.GetUnitsDll(fvm.currUnit).ToList();

            return;
        }

        private void FertilizerDetailSetup_Fertilizer(ref FertilizerDetailsViewModel fvm)
        {
            fvm.fertOptions = new List<Models.StaticData.SelectListItem>();
            if (fvm.selTypOption != null &&
               fvm.selTypOption != "select")
            {
                FertilizerType ft = _sd.GetFertilizerType(fvm.selTypOption);
                if (!ft.custom)
                {
                    fvm.fertOptions = _sd.GetFertilizersDll(ft.dry_liquid).ToList();
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
                fvm.density = _sd.GetLiquidFertilizerDensity(Convert.ToInt32(fvm.selFertOption), Convert.ToInt32(fvm.selDenOption)).value.ToString("#.##");
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

            if (ft.custom)
            {
                fertilizerName = ft.dry_liquid == "dry" ? "Custom (Dry)" : "Custom (Liquid)";
            }
            else
            {
                Fertilizer ff = _sd.GetFertilizer(nf.fertilizerId.ToString());
                fertilizerName = ff.name;
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

        public IActionResult CropDetails(string fldName, int? id)
        {
            CalculateCropRequirementRemoval calculateCropRequirementRemoval = new CalculateCropRequirementRemoval(_ud, _sd);
            CropDetailsViewModel cvm = new CropDetailsViewModel();

            cvm.fieldName = fldName;
            cvm.title = id == null ? "Add" : "Edit";
            cvm.btnText = id == null ? "Calculate" : "Return";
            cvm.id = id;
            cvm.stdCrude = true;
            cvm.stdYield = true;
            cvm.nCredit = "0";
            cvm.nCreditLabel = _sd.GetUserPrompt("ncreditlabel");

            if (id != null)
            {
                FieldCrop cp = _ud.GetFieldCrop(fldName, id.Value);

                cvm.fieldName = fldName;
                cvm.id = id;
                cvm.reqN = cp.reqN.ToString();
                cvm.reqP2o5 = cp.reqP2o5.ToString();
                cvm.reqK2o = cp.reqK2o.ToString();
                cvm.remN = cp.remN.ToString();
                cvm.remP2o5 = cp.remP2o5.ToString();
                cvm.remK2o = cp.remK2o.ToString();
                
                cvm.crude = cp.crudeProtien.ToString();
                cvm.selCropOption = cp.cropId;
                cvm.selPrevOption = cp.prevCropId.ToString();
                cvm.coverCropHarvested = cp.coverCropHarvested;
                cvm.nCredit = cvm.selPrevOption != "0" ? _sd.GetPrevCropType(Convert.ToInt32(cvm.selPrevOption)).nCreditImperial.ToString() : "0";
                //E07US18 
                cvm.showHarvestUnitsDDL = false; 

                if (!cp.yieldHarvestUnit.HasValue)
                {   // retrofit old version of data
                    cp.yieldHarvestUnit = _sd.GetHarvestYieldDefaultUnit();
                    cvm.selHarvestUnits = cp.yieldHarvestUnit.ToString();
                    cp.yieldByHarvestUnit = cp.yield; // retrofit old version of data
                    cvm.yield = cp.yield.ToString();
                    cvm.yieldByHarvestUnit = cp.yieldByHarvestUnit.ToString("#.##");
                }
                else
                {
                    cvm.selHarvestUnits = cp.yieldHarvestUnit.ToString();
                    cvm.yieldByHarvestUnit = cp.yieldByHarvestUnit.ToString("#.##");
                    cvm.yield = cp.yield.ToString("#.##");
                }

                CalculateCropRequirementRemoval ccrr = new CalculateCropRequirementRemoval(_ud, _sd);
                decimal? defaultYield = ccrr.GetDefaultYieldByCropId(Convert.ToInt16(cvm.selCropOption), cp.yieldHarvestUnit != _sd.GetHarvestYieldDefaultUnit());
                cvm.stdYield = true;
                if (defaultYield.HasValue)
                {   // E07US18 
                    if (cvm.yieldByHarvestUnit != defaultYield.Value.ToString("#.##"))
                    {
                        cvm.stdYield = false;
                    }
                }

                if (!string.IsNullOrEmpty(cp.cropOther))
                {
                    cvm.manEntry = true;
                    Yield yld = _sd.GetYield(1);
                    cvm.cropDesc = cp.cropOther;
                    cvm.yieldUnit = "(" + yld.yielddesc + ")";
                    cvm.selTypOption = _settings.OtherCropId;
                }
                else
                {
                    Crop crop = _sd.GetCrop(Convert.ToInt32(cp.cropId));
                    Yield yld = _sd.GetYield(crop.yieldcd);
                    cvm.yieldUnit = "(" + yld.yielddesc + ")";
                    cvm.selTypOption = crop.croptypeid.ToString();
                    //E07US18
                    cvm.showHarvestUnitsDDL = _sd.IsCropGrainsAndOilseeds(Convert.ToInt16(crop.croptypeid));

                    CropType crpTyp = _sd.GetCropType(Convert.ToInt32(cvm.selTypOption));
                    if (crpTyp.modifynitrogen)
                    {
                        cvm.modNitrogen = true;

                        // check for standard
                        CropRequirementRemoval cropRequirementRemoval = new CropRequirementRemoval();

                        calculateCropRequirementRemoval.cropid = Convert.ToInt16(cvm.selCropOption);

                        // E07US18 - need to convert cvm.yield before assigning to caclculateCropRequirement
                        if (cvm.showHarvestUnitsDDL && (cp.yieldHarvestUnit != _sd.GetHarvestYieldDefaultUnit() ) )
                            calculateCropRequirementRemoval.yield  = _sd.ConvertYieldFromBushelToTonsPerAcre(Convert.ToInt16(cvm.selCropOption), Convert.ToDecimal(cvm.yieldByHarvestUnit));
                        else 
                            calculateCropRequirementRemoval.yield = Convert.ToDecimal(cvm.yieldByHarvestUnit);
                        if (string.IsNullOrEmpty(cvm.crude))
                            calculateCropRequirementRemoval.crudeProtien = null;
                        else
                            calculateCropRequirementRemoval.crudeProtien = Convert.ToDecimal(cvm.crude);
                        calculateCropRequirementRemoval.coverCropHarvested = cvm.coverCropHarvested;
                        calculateCropRequirementRemoval.fieldName = cvm.fieldName;
                        if (!string.IsNullOrEmpty(cvm.nCredit))
                            calculateCropRequirementRemoval.nCredit = Convert.ToInt16(cvm.nCredit);

                        cropRequirementRemoval = calculateCropRequirementRemoval.GetCropRequirementRemoval();

                        cvm.stdNAmt = cropRequirementRemoval.N_Requirement.ToString();

                        cvm.stdN = (cvm.reqN == cropRequirementRemoval.N_Requirement.ToString()) ? true : false;
                    }
                }

                CropDetailsSetup(ref cvm);

                if (!cvm.manEntry)
                {
                    if (cvm.showCrude)
                    {
                        if (cvm.crude != calculateCropRequirementRemoval.GetCrudeProtienByCropId(Convert.ToInt16(cvm.selCropOption)).ToString("#.#"))
                        {
                            cvm.stdCrude = false;
                        }
                    }
                }

            }
            else
            {
                CropDetailsReset(ref cvm);

                CropDetailsSetup(ref cvm);
            }


            return PartialView(cvm);
        }
        [HttpPost]
        public IActionResult CropDetails(CropDetailsViewModel cvm)
        {
            CalculateCropRequirementRemoval calculateCropRequirementRemoval = new CalculateCropRequirementRemoval(_ud, _sd);
            CropDetailsSetup(ref cvm);
            try
            {
                if (cvm.buttonPressed == "TypeChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";
                    cvm.crude = "";
                    cvm.cropDesc = "";
                    cvm.coverCropHarvested = null;
                    cvm.nCredit = "0";
                    cvm.stdYield = true;
                    cvm.yield = "";
                    cvm.yieldByHarvestUnit = "";
                    cvm.prevOptions = new List<Models.StaticData.SelectListItem>();

                    if (cvm.selTypOption != "select")
                    {
                        CropType crpTyp = _sd.GetCropType(Convert.ToInt32(cvm.selTypOption));

                        if (crpTyp.customcrop)
                        {
                            cvm.manEntry = true;
                            cvm.reqN = string.Empty;
                            cvm.reqP2o5 = string.Empty;
                            cvm.reqK2o = string.Empty;
                            cvm.remN = string.Empty;
                            cvm.remP2o5 = string.Empty;
                            cvm.remK2o = string.Empty;
                            cvm.showHarvestUnitsDDL = false;
                        }
                        else
                        {
                            if (_sd.IsCropGrainsAndOilseeds(crpTyp.id))
                                cvm.showHarvestUnitsDDL = true;
                            else
                                cvm.showHarvestUnitsDDL = false;
                            cvm.manEntry = false;
                            CropDetailsReset(ref cvm);
                        }
                    }
                    else
                    {
                        cvm.manEntry = false;
                        cvm.showHarvestUnitsDDL = false;
                        CropDetailsReset(ref cvm);
                    }
                    return View(cvm);
                }

                if (cvm.buttonPressed == "PrevChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";
                    if (cvm.selPrevOption != "select" &&
                        cvm.selPrevOption != "")
                    {
                        cvm.nCredit = _sd.GetPrevCropType(Convert.ToInt32(cvm.selPrevOption)).nCreditImperial.ToString();
                    }
                    else
                    {
                        cvm.nCredit = "0";
                    }
                    CropDetailsReset(ref cvm);

                    return View(cvm);
                }

                if (cvm.buttonPressed == "ResetCrude")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";

                    cvm.crude = calculateCropRequirementRemoval.GetCrudeProtienByCropId(Convert.ToInt16(cvm.selCropOption)).ToString("#.#");

                    cvm.stdCrude = true;
                    return View(cvm);
                }

                if (cvm.buttonPressed == "ResetN")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";

                    cvm.reqN = cvm.stdNAmt;

                    cvm.stdN = true;
                    return View(cvm);
                }

                if (cvm.buttonPressed == "ResetYield")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";

                    CalculateCropRequirementRemoval ccrr = new CalculateCropRequirementRemoval(_ud, _sd);
                    decimal? defaultYield;
                    // E07US18 - convert defaultYield to bu/ac if required
                    if (cvm.showHarvestUnitsDDL)
                        defaultYield = ccrr.GetDefaultYieldByCropId(Convert.ToInt16(cvm.selCropOption), cvm.selHarvestUnits != _sd.GetHarvestYieldDefaultUnit().ToString());
                    else
                        defaultYield = ccrr.GetDefaultYieldByCropId(Convert.ToInt16(cvm.selCropOption), false);
                    if (defaultYield.HasValue)
                        cvm.yieldByHarvestUnit = defaultYield.Value.ToString("#.##");

                    cvm.reqN = cvm.stdNAmt;

                    cvm.stdYield = true;
                    return View(cvm);
                }

                if (cvm.buttonPressed == "CropChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";
                    cvm.nCredit = "0";
                    cvm.stdYield = true;

                    PreviousCropSetup(ref cvm);
                    CropDetailsReset(ref cvm);

                    if (cvm.selCropOption != "" &&
                        cvm.selCropOption != "0" &&
                        cvm.selCropOption != "select")
                    {
                        Crop cp = _sd.GetCrop(Convert.ToInt32(cvm.selCropOption));
                        Yield yld = _sd.GetYield(cp.yieldcd);

                        cvm.yieldUnit = "(" + yld.yielddesc + ")";
                        // E07US18
                        if (cvm.showHarvestUnitsDDL)
                        {
                            cvm.harvestUnitsOptions = _sd.GetCropHarvestUnitsDll();
                        }
                        if (cvm.showCrude)
                        {
                            cvm.crude = calculateCropRequirementRemoval.GetCrudeProtienByCropId(Convert.ToInt16(cvm.selCropOption)).ToString("#.#");
                            cvm.stdCrude = true;
                        }

                        CalculateCropRequirementRemoval ccrr = new CalculateCropRequirementRemoval(_ud, _sd);
                        decimal? defaultYield;
                        // E07US18
                        if (cvm.showHarvestUnitsDDL)
                            defaultYield = ccrr.GetDefaultYieldByCropId(Convert.ToInt16(cvm.selCropOption), cvm.selHarvestUnits != _sd.GetHarvestYieldDefaultUnit().ToString());
                        else
                            defaultYield = ccrr.GetDefaultYieldByCropId(Convert.ToInt16(cvm.selCropOption), false); 
                       
                        if (defaultYield.HasValue)
                            cvm.yieldByHarvestUnit = defaultYield.Value.ToString("#.##");

                    }
                    cvm.selPrevOption = string.Empty;

                    return View(cvm);
                }

                if (cvm.buttonPressed == "HarvestUnitChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";

                    if (Convert.ToInt32(cvm.selCropOption) > 0)
                    {
                        Crop crop = _sd.GetCrop(Convert.ToInt32(cvm.selCropOption));

                        if (cvm.selHarvestUnits == _sd.GetHarvestYieldDefaultUnit().ToString())
                        {
                            if (crop.harvestBushelsPerTon.HasValue)
                                cvm.yieldByHarvestUnit = (Convert.ToDecimal(cvm.yieldByHarvestUnit) / Convert.ToDecimal(crop.harvestBushelsPerTon)).ToString("#.##");
                        }
                        else
                        {
                            if (crop.harvestBushelsPerTon.HasValue)
                                cvm.yieldByHarvestUnit = (Convert.ToDecimal(cvm.yieldByHarvestUnit) * Convert.ToDecimal(crop.harvestBushelsPerTon)).ToString("#.##");
                        }
                    }
                    return View(cvm);
                }

                if (ModelState.IsValid)
                {
                    if (cvm.coverCrop)
                    {
                        if (!cvm.coverCropHarvested.HasValue)
                        {
                            ModelState.AddModelError("coverCropHarvested", "Required.");
                            return View(cvm);
                        }
                    }

                    if (!string.IsNullOrEmpty(cvm.crude))
                    {
                        decimal crd;
                        if (decimal.TryParse(cvm.crude, out crd))
                        {
                            if (crd < 0 || crd > 100)
                            {
                                ModelState.AddModelError("crude", "Not a valid percentage.");
                                return View(cvm);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("crude", "Not a valid number.");
                            return View(cvm);
                        }
                    }

                    decimal tmpDec;
                    if (decimal.TryParse(cvm.yieldByHarvestUnit, out tmpDec))
                    {
                        if (tmpDec <= 0 ||
                            tmpDec > 99999)
                        {
                            ModelState.AddModelError("yieldByHarvestUnit", "Not a valid yield.");
                            return View(cvm);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("yieldByHarvestUnit", "Not a valid number.");
                        return View(cvm);
                    }

                    //if((string.IsNullOrEmpty(cvm.selCropOption) ||
                    //    cvm.selCropOption == "select") &&
                    //    cvm.selTypOption != _settings.OtherCropId)  // none
                    //{
                    //    ModelState.AddModelError("selCropOption", "Required.");
                    //    return View(cvm);
                    //}

                    if (cvm.manEntry)
                    {
                        if (string.IsNullOrEmpty(cvm.cropDesc))
                        {
                            ModelState.AddModelError("cropDesc", "Required.");
                            return View(cvm);
                        }

                        if (string.IsNullOrEmpty(cvm.reqN))
                        {
                            ModelState.AddModelError("reqN", "Reqd.");
                            return View(cvm);
                        }
                        else
                        {
                            if (decimal.TryParse(cvm.reqN, out tmpDec))
                            {
                                if (tmpDec < 0 ||
                                    tmpDec > 1000)
                                {
                                    ModelState.AddModelError("reqN", "Invalid.");
                                    return View(cvm);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("reqN", "Invalid.");
                                return View(cvm);
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.reqP2o5))
                        {
                            ModelState.AddModelError("reqP2o5", "Reqd.");
                            return View(cvm);
                        }
                        else
                        {
                            if (decimal.TryParse(cvm.reqP2o5, out tmpDec))
                            {
                                if (tmpDec < 0 ||
                                    tmpDec > 1000)
                                {
                                    ModelState.AddModelError("reqP2o5", "Invalid.");
                                    return View(cvm);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("reqP2o5", "Invalid.");
                                return View(cvm);
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.reqK2o))
                        {
                            ModelState.AddModelError("reqK2o", "Reqd.");
                            return View(cvm);
                        }
                        else
                        {
                            if (decimal.TryParse(cvm.reqK2o, out tmpDec))
                            {
                                if (tmpDec < 0 ||
                                    tmpDec > 1000)
                                {
                                    ModelState.AddModelError("reqK2o", "Invalid.");
                                    return View(cvm);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("reqK2o", "Invalid.");
                                return View(cvm);
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.remN))
                        {
                            ModelState.AddModelError("remN", "Reqd.");
                            return View(cvm);
                        }
                        else
                        {
                            if (decimal.TryParse(cvm.remN, out tmpDec))
                            {
                                if (tmpDec < 0 ||
                                    tmpDec > 1000)
                                {
                                    ModelState.AddModelError("remN", "Invalid.");
                                    return View(cvm);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("remN", "Invalid.");
                                return View(cvm);
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.remP2o5))
                        {
                            ModelState.AddModelError("remP2o5", "Reqd.");
                            return View(cvm);
                        }
                        else
                        {
                            if (decimal.TryParse(cvm.remP2o5, out tmpDec))
                            {
                                if (tmpDec < 0 ||
                                    tmpDec > 1000)
                                {
                                    ModelState.AddModelError("remP2o5", "Invalid.");
                                    return View(cvm);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("remP2o5", "Invalid.");
                                return View(cvm);
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.remK2o))
                        {
                            ModelState.AddModelError("remK2o", "Reqd.");
                            return View(cvm);
                        }
                        else
                        {
                            if (decimal.TryParse(cvm.remK2o, out tmpDec))
                            {
                                if (tmpDec < 0 ||
                                    tmpDec > 1000)
                                {
                                    ModelState.AddModelError("remK2o", "Invalid.");
                                    return View(cvm);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("remK2o", "Invalid.");
                                return View(cvm);
                            }
                        }
                    }

                    if (cvm.modNitrogen)
                    {
                        if (decimal.TryParse(cvm.reqN, out tmpDec))
                        {
                            if (tmpDec < 0 ||
                                tmpDec > 1000)
                            {
                                ModelState.AddModelError("reqN", "Not a valid amount.");
                                return View(cvm);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("reqN", "Not a valid number.");
                            return View(cvm);
                        }
                    }

                    if (cvm.btnText == "Calculate")
                    {
                        ModelState.Clear();
                        if (!cvm.manEntry)
                        {
                            CropRequirementRemoval cropRequirementRemoval = new CropRequirementRemoval();

                            calculateCropRequirementRemoval.cropid = Convert.ToInt16(cvm.selCropOption);
                            // E07US18 - need to convert cvm.yield to tons/acre before passing to calculateCrop
                            if (cvm.showHarvestUnitsDDL && !_sd.IsCropHarvestYieldDefaultUnit(Convert.ToInt16(cvm.selHarvestUnits)))
                            {
                                calculateCropRequirementRemoval.yield = _sd.ConvertYieldFromBushelToTonsPerAcre(Convert.ToInt16(cvm.selCropOption), Convert.ToDecimal(cvm.yieldByHarvestUnit));
                                cvm.yield = calculateCropRequirementRemoval.yield.ToString();
                            }
                            else
                            {
                                calculateCropRequirementRemoval.yield = Convert.ToDecimal(cvm.yieldByHarvestUnit);
                                cvm.yield = calculateCropRequirementRemoval.yield.ToString();
                            }
                            if (cvm.crude == null)
                                calculateCropRequirementRemoval.crudeProtien = null;
                            else
                                calculateCropRequirementRemoval.crudeProtien = Convert.ToDecimal(cvm.crude);
                            calculateCropRequirementRemoval.coverCropHarvested = cvm.coverCropHarvested;
                            calculateCropRequirementRemoval.fieldName = cvm.fieldName;
                            if (!string.IsNullOrEmpty(cvm.nCredit))
                                calculateCropRequirementRemoval.nCredit = Convert.ToInt16(cvm.nCredit);

                            cropRequirementRemoval = calculateCropRequirementRemoval.GetCropRequirementRemoval();

                            if (!cvm.modNitrogen)
                            {
                                cvm.reqN = cropRequirementRemoval.N_Requirement.ToString();
                            }
                            else
                            {
                                if (cvm.reqN != cropRequirementRemoval.N_Requirement.ToString())
                                {
                                    cvm.stdN = false;
                                }
                            }
                            cvm.stdNAmt = cropRequirementRemoval.N_Requirement.ToString();
                            cvm.reqP2o5 = cropRequirementRemoval.P2O5_Requirement.ToString();
                            cvm.reqK2o = cropRequirementRemoval.K2O_Requirement.ToString();
                            cvm.remN = cropRequirementRemoval.N_Removal.ToString();
                            cvm.remP2o5 = cropRequirementRemoval.P2O5_Removal.ToString();
                            cvm.remK2o = cropRequirementRemoval.K2O_Removal.ToString();
                            if (cvm.crude != calculateCropRequirementRemoval.GetCrudeProtienByCropId(Convert.ToInt16(cvm.selCropOption)).ToString("#.#"))
                            {
                                cvm.stdCrude = false;
                            }

                            if (!cvm.modNitrogen)
                            {
                                CropType crpTyp = _sd.GetCropType(Convert.ToInt32(cvm.selTypOption));
                                if (crpTyp.modifynitrogen)
                                {
                                    cvm.modNitrogen = true;
                                    cvm.stdN = true;
                                }
                            }

                            CalculateCropRequirementRemoval ccrr = new CalculateCropRequirementRemoval(_ud, _sd);
                            decimal? defaultYield;
                            if (cvm.showHarvestUnitsDDL)
                                defaultYield = ccrr.GetDefaultYieldByCropId(Convert.ToInt16(cvm.selCropOption), cvm.selHarvestUnits != _sd.GetHarvestYieldDefaultUnit().ToString());
                            else
                                defaultYield = ccrr.GetDefaultYieldByCropId(Convert.ToInt16(cvm.selCropOption), false);
                            cvm.stdYield = true;
                            if (defaultYield.HasValue)
                            {
                                if (cvm.yieldByHarvestUnit != defaultYield.Value.ToString("#.##"))
                                {
                                    cvm.stdYield = false;
                                }
                            }

                        }

                        cvm.btnText = cvm.id == null ? "Add to Field" : "Update Field";
                    }
                    else
                    {
                        if (cvm.id == null)
                        {
                            int prevCrop = 0;
                            if (cvm.selPrevOption != "select")
                                prevCrop = Convert.ToInt32(cvm.selPrevOption);

                            int thisCrop = 0;
                            if (cvm.selCropOption != "select")
                                thisCrop = Convert.ToInt32(cvm.selCropOption);
                            // E07US18 - convert cvm.yield
                            FieldCrop crp = new FieldCrop()
                            {
                                cropId = thisCrop.ToString(),
                                cropOther = cvm.cropDesc,
                                yield = Convert.ToDecimal(cvm.yieldByHarvestUnit),
                                yieldByHarvestUnit =Convert.ToDecimal(cvm.yieldByHarvestUnit),
                                reqN = Convert.ToDecimal(cvm.reqN),
                                reqP2o5 = Convert.ToDecimal(cvm.reqP2o5),
                                reqK2o = Convert.ToDecimal(cvm.reqK2o),
                                remN = Convert.ToDecimal(cvm.remN),
                                remP2o5 = Convert.ToDecimal(cvm.remP2o5),
                                remK2o = Convert.ToDecimal(cvm.remK2o),
                                crudeProtien = string.IsNullOrEmpty(cvm.crude) ? (decimal?)null : Convert.ToDecimal(cvm.crude),
                                prevCropId = prevCrop,
                                coverCropHarvested = cvm.coverCropHarvested,
                                prevYearManureAppl_volCatCd = _sd.GetCropPrevYearManureApplVolCatCd(thisCrop),
                                yieldHarvestUnit = (cvm.showHarvestUnitsDDL) ? Convert.ToInt16(cvm.selHarvestUnits) : _sd.GetHarvestYieldDefaultUnit()
                            };
                            if (cvm.showHarvestUnitsDDL && (cvm.selHarvestUnits != _sd.GetHarvestYieldDefaultUnit().ToString()))
                                crp.yield = _sd.ConvertYieldFromBushelToTonsPerAcre(Convert.ToInt16(crp.cropId), Convert.ToDecimal(cvm.yieldByHarvestUnit));

                            _ud.AddFieldCrop(cvm.fieldName, crp);
                        }
                        else
                        {
                            int prevCrop = 0;
                            if (cvm.selPrevOption != "select")
                                prevCrop = Convert.ToInt32(cvm.selPrevOption);

                            int thisCrop = 0;
                            if (cvm.selCropOption != "select")
                                thisCrop = Convert.ToInt32(cvm.selCropOption);

                            FieldCrop crp = _ud.GetFieldCrop(cvm.fieldName, cvm.id.Value);
                            crp.cropId = thisCrop.ToString();
                            crp.cropOther = cvm.cropDesc;
                            //E07US18 - need to convert cvm.yield to TONS/acre before assigin to crp.yield
                            crp.yieldByHarvestUnit = Convert.ToDecimal(cvm.yieldByHarvestUnit);
                            if (cvm.showHarvestUnitsDDL && (cvm.selHarvestUnits != _sd.GetHarvestYieldDefaultUnit().ToString()))
                                crp.yield = _sd.ConvertYieldFromBushelToTonsPerAcre(Convert.ToInt16(crp.cropId), Convert.ToDecimal(cvm.yieldByHarvestUnit));
                            else
                                crp.yield = crp.yieldByHarvestUnit;

                            crp.reqN = Convert.ToDecimal(cvm.reqN);
                            crp.reqP2o5 = Convert.ToDecimal(cvm.reqP2o5);
                            crp.reqK2o = Convert.ToDecimal(cvm.reqK2o);
                            crp.remN = Convert.ToDecimal(cvm.remN);
                            crp.remP2o5 = Convert.ToDecimal(cvm.remP2o5);
                            crp.remK2o = Convert.ToDecimal(cvm.remK2o);
                            crp.crudeProtien = string.IsNullOrEmpty(cvm.crude) ? (decimal?)null : Convert.ToDecimal(cvm.crude);
                            crp.prevCropId = prevCrop;
                            crp.coverCropHarvested = cvm.coverCropHarvested;
                            crp.prevYearManureAppl_volCatCd = _sd.GetCropPrevYearManureApplVolCatCd(Convert.ToInt32(crp.cropId));
                            crp.yieldHarvestUnit = (cvm.showHarvestUnitsDDL) ? Convert.ToInt16(cvm.selHarvestUnits) : _sd.GetHarvestYieldDefaultUnit();

                            _ud.UpdateFieldCrop(cvm.fieldName, crp);
                        }
                        return Json(ReDisplay("#crop", cvm.fieldName));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error -" + ex.Message);
            }

            return PartialView(cvm);
        }
        private void CropDetailsSetup(ref CropDetailsViewModel cvm)
        {
            cvm.showCrude = false;
            cvm.typOptions = new List<Models.StaticData.SelectListItem>();
            cvm.typOptions = _sd.GetCropTypesDll().ToList();

            cvm.cropOptions = new List<Models.StaticData.SelectListItem>();
            cvm.harvestUnitsOptions = new List<Models.StaticData.SelectListItem>();
            if (!string.IsNullOrEmpty(cvm.selTypOption) &&
                cvm.selTypOption != "select")
            {
                cvm.cropOptions = _sd.GetCropsDll(Convert.ToInt32(cvm.selTypOption)).ToList();
                cvm.harvestUnitsOptions = _sd.GetCropHarvestUnitsDll().ToList();

                if (cvm.selTypOption != "select")
                {
                    CropType crpTyp = _sd.GetCropType(Convert.ToInt32(cvm.selTypOption));
                    cvm.showCrude = crpTyp.crudeproteinrequired;
                    cvm.coverCrop = crpTyp.covercrop;
                    cvm.manEntry = crpTyp.customcrop;
                    if (!crpTyp.customcrop)
                    {
                        cvm.cropOptions.Insert(0, new SelectListItem() { Id = 0, Value = "select" });
                    }
                }
            }

            PreviousCropSetup(ref cvm);

            return;
        }
        private void CropDetailsReset(ref CropDetailsViewModel cvm)
        {
            cvm.reqN = "  0";
            cvm.reqP2o5 = "  0";
            cvm.reqK2o = "  0";
            cvm.remN = "  0";
            cvm.remP2o5 = "  0";
            cvm.remK2o = "  0";

            cvm.modNitrogen = false;
            cvm.stdN = true;

            return;
        }
        private void PreviousCropSetup(ref CropDetailsViewModel cvm)
        {
            cvm.prevOptions = new List<Models.StaticData.SelectListItem>();
            if (!string.IsNullOrEmpty(cvm.selCropOption))
            {
                if (cvm.selCropOption != "select" &&
                    cvm.selCropOption != "0")
                {
                    Crop crp = _sd.GetCrop(Convert.ToInt32(cvm.selCropOption));
                    cvm.prevOptions = _sd.GetPrevCropTypesDll(crp.prevcropcd.ToString()).ToList();
                }
            }
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
            dvm.matType = _ud.GetFarmManure(Convert.ToInt32(nm.manureId)).name;

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
                dvm.cropName = _sd.GetCrop(Convert.ToInt32(crp.cropId)).cropname;

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
            Utility.CalculateNutrients calculateNutrients = new CalculateNutrients(_env, _ud, _sd);
            NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            OtherDetailsViewModel ovm = new OtherDetailsViewModel();

            ovm.fieldName = fldName;
            ovm.title = id == null ? "Add" : "Edit";
            ovm.btnText = id == null ? "Add to Field" : "Update Field";
            ovm.id = id;
            ovm.url = _sd.GetExternalLink("othernutrientexplanation");
            ovm.urlText = _sd.GetUserPrompt("moreinfo");
            ovm.placehldr = _sd.GetUserPrompt("othernutrientplaceholder");

            if (id != null)
            {
                ovm.title = "Edit";

                NutrientOther no = _ud.GetFieldNutrientsOther(fldName, id.Value);
                ovm.source = no.description;
                ovm.yrN = no.yrN.ToString();
                ovm.yrP = no.yrP2o5.ToString();
                ovm.yrK = no.yrK.ToString();
                ovm.ltN = no.ltN.ToString();
                ovm.ltP = no.ltP2o5.ToString();
                ovm.ltK = no.ltK.ToString();
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
                    fld.prevYearManureApplicationNitrogenCredit = nitrogenCredit;
                else // only save non-defaulted value (ie. over-ride)
                    fld.prevYearManureApplicationNitrogenCredit = null;
                _ud.UpdateField(fld);
            }
            catch (Exception e)
            {
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

            SERVERAPI.Utility.ChemicalBalanceMessage calculator = new Utility.ChemicalBalanceMessage(_ud, _sd);
            model.defaultNitrogenCredit = calculator.calcPrevYearManureApplDefault(fldName).ToString();
            model.fldName = fldName;
            Field field = _ud.GetFieldDetails(fldName);
            if (field.prevYearManureApplicationNitrogenCredit != null)
                model.nitrogen = Convert.ToInt32(field.prevYearManureApplicationNitrogenCredit).ToString();
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
                    return PartialView(model);
                }
                if  (nitrogenCredit >= 0)  {
                    if (SaveNitrogenCreditToField(model.fldName, nitrogenCredit, Convert.ToInt32(model.defaultNitrogenCredit)) )
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

            SERVERAPI.Utility.ChemicalBalanceMessage calculator = new Utility.ChemicalBalanceMessage(_ud, _sd);
            model.defaultNitrogenCredit = calculator.calcSoitTestNitrateDefault(fldName).ToString();
            model.fldName = fldName;
            Field field = _ud.GetFieldDetails(fldName);
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

    }
}
