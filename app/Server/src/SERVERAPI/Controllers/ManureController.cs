using Agri.CalculateService;
using Agri.Data;
using Agri.Models.Calculate;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SERVERAPI.Controllers
{
    //[RedirectingAction]
    public class ManureController : Controller
    {
        private readonly ILogger<ManureController> _logger;
        private readonly IHostingEnvironment _env;
        private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;
        private readonly ICalculateNutrients _calculateNutrients;

        public ManureController(ILogger<ManureController> logger,
            IHostingEnvironment env,
            UserData ud,
            IAgriConfigurationRepository sd,
            ICalculateNutrients calculateNutrients)
        {
            _logger = logger;
            _env = env;
            _ud = ud;
            _sd = sd;
            _calculateNutrients = calculateNutrients;
        }

        // GET: /<controller>/
        public IActionResult Manure()
        {
            return View();
        }

        public IActionResult CompostDetails(int? id, string target)
        {
            //Utility.CalculateNutrients calculateNutrients = new CalculateNutrients(_env, _ud, _sd);
            //NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            CompostDetailViewModel mvm = new CompostDetailViewModel();

            mvm.Action = id == null ? "Add" : "Edit";
            mvm.Url = _sd.GetExternalLink("labanalysisexplanation");
            mvm.UrlText = _sd.GetUserPrompt("moreinfo");

            if (id != null)
            {
                FarmManure fm = _ud.GetFarmManure(id.Value);
                mvm.SelectedManureOption = fm.ManureId;

                if (!fm.Customized)
                {
                    mvm.BookValue = true;
                    mvm.Compost = false;
                    mvm.OnlyCustom = false;
                    mvm.ShowNitrate = false;
                }
                else
                {
                    mvm.BookValue = false;
                    mvm.Compost = _sd.IsManureClassCompostType(fm.ManureClass);
                    mvm.OnlyCustom = (_sd.IsManureClassOtherType(fm.ManureClass) || _sd.IsManureClassCompostType(fm.ManureClass) || _sd.IsManureClassCompostClassType(fm.ManureClass));
                    mvm.ShowNitrate = (_sd.IsManureClassCompostType(fm.ManureClass) || _sd.IsManureClassCompostClassType(fm.ManureClass));
                }
                mvm.ManureName = fm.Name;
                mvm.Moisture = fm.Moisture;
                mvm.Nitrogen = fm.Nitrogen.ToString("#0.00");
                mvm.Ammonia = fm.Ammonia.ToString("#0");
                mvm.Phosphorous = fm.Phosphorous.ToString("#0.00");
                mvm.Potassium = fm.Potassium.ToString("#0.00");
                mvm.Nitrate = fm.Nitrate.HasValue ? fm.Nitrate.Value.ToString("#0") : ""; // old version of datafile
            }
            else
            {
                mvm.BookValue = true;
                mvm.ManureName = "  ";
                mvm.Moisture = "  ";
                mvm.Nitrogen = "  ";
                mvm.Ammonia = "  ";
                mvm.Phosphorous = "  ";
                mvm.Potassium = "  ";
                mvm.Nitrate = "  ";
                mvm.Compost = false;
                mvm.OnlyCustom = false;
                mvm.ShowNitrate = false;
            }

            CompostDetailsSetup(ref mvm);

            return PartialView(mvm);
        }

        private void CompostDetailsSetup(ref CompostDetailViewModel cvm)
        {
            cvm.ManureOptions = new List<SelectListItem>();
            cvm.ManureOptions = _sd.GetManuresDll().ToList();

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
                if (cvm.ButtonPressed == "ManureChange")
                {
                    ModelState.Clear();
                    cvm.ButtonPressed = "";

                    if (cvm.SelectedManureOption != 0)
                    {
                        man = _sd.GetManure(cvm.SelectedManureOption);
                        if (_sd.IsManureClassOtherType(man.ManureClass) ||
                           _sd.IsManureClassCompostType(man.ManureClass))
                        {
                            cvm.BookValue = false;
                            cvm.OnlyCustom = true;
                            cvm.Nitrogen = string.Empty;
                            cvm.Moisture = string.Empty;
                            cvm.Ammonia = string.Empty;
                            cvm.Nitrate = string.Empty;
                            cvm.Phosphorous = string.Empty;
                            cvm.Potassium = string.Empty;
                            cvm.Compost = _sd.IsManureClassCompostType(man.ManureClass);
                            cvm.ShowNitrate = cvm.Compost;
                            cvm.ManureName = cvm.Compost ? "Custom - " + man.Name + " - " : "Custom - " + man.SolidLiquid + " - ";
                        }
                        else
                        {
                            cvm.ShowNitrate = _sd.IsManureClassCompostClassType(man.ManureClass);
                            cvm.BookValue = !cvm.ShowNitrate;
                            cvm.Compost = false;
                            if (cvm.ShowNitrate)
                            {
                                cvm.MoistureBook = man.Moisture.ToString();
                                cvm.NitrogenBook = man.Nitrogen.ToString();
                                cvm.AmmoniaBook = man.Ammonia.ToString();
                                cvm.NitrateBook = man.Nitrate.ToString();
                                cvm.PhosphorousBook = man.Phosphorous.ToString();
                                cvm.PotassiumBook = man.Potassium.ToString();
                                cvm.NitrateBook = man.Nitrate.ToString();
                                cvm.ManureName = "Custom - " + man.Name + " - ";
                                cvm.OnlyCustom = cvm.ShowNitrate;
                                cvm.BookValue = false;
                            }
                            else
                            {
                                cvm.BookValue = true;
                                cvm.Nitrogen = man.Nitrogen.ToString();
                                cvm.Moisture = man.Moisture.ToString();
                                cvm.Ammonia = man.Ammonia.ToString();
                                cvm.Nitrate = man.Nitrate.ToString();
                                cvm.Phosphorous = man.Phosphorous.ToString();
                                cvm.Potassium = man.Potassium.ToString();
                                cvm.ManureName = man.Name;
                                cvm.OnlyCustom = false;
                            }
                        }
                    }
                    else
                    {
                        cvm.BookValue = true;
                        cvm.ShowNitrate = false;
                        cvm.Compost = false;
                        cvm.OnlyCustom = false;
                        cvm.Nitrogen = string.Empty;
                        cvm.Moisture = string.Empty;
                        cvm.Ammonia = string.Empty;
                        cvm.Nitrate = string.Empty;
                        cvm.Phosphorous = string.Empty;
                        cvm.Potassium = string.Empty;
                        cvm.ManureName = string.Empty;
                        cvm.Nitrate = String.Empty;
                    }
                    return View(cvm);
                }
                if (cvm.ButtonPressed == "TypeChange")
                {
                    ModelState.Clear();
                    cvm.ButtonPressed = "";

                    if (cvm.SelectedManureOption != 0)
                    {
                        man = _sd.GetManure(cvm.SelectedManureOption);
                        cvm.OnlyCustom = false;
                        if (cvm.BookValue)
                        {
                            cvm.Moisture = cvm.BookValue ? man.Moisture.ToString() : "";
                            cvm.Nitrogen = man.Nitrogen.ToString();
                            cvm.Ammonia = man.Ammonia.ToString();
                            cvm.Nitrate = man.Nitrate.ToString();
                            cvm.Phosphorous = man.Phosphorous.ToString();
                            cvm.Potassium = man.Potassium.ToString();
                            cvm.ManureName = man.Name;
                            cvm.ShowNitrate = false;
                            cvm.Compost = false;
                        }
                        else
                        {
                            cvm.Nitrogen = string.Empty;
                            cvm.Moisture = string.Empty;
                            cvm.Ammonia = string.Empty;
                            cvm.Nitrate = string.Empty;
                            cvm.Phosphorous = string.Empty;
                            cvm.Potassium = string.Empty;
                            cvm.ManureName = (!cvm.Compost) ? "Custom - " + man.Name + " - " : "Custom - " + man.SolidLiquid + " - ";

                            cvm.MoistureBook = man.Moisture.ToString();
                            cvm.NitrogenBook = man.Nitrogen.ToString();
                            cvm.AmmoniaBook = man.Ammonia.ToString();
                            cvm.NitrateBook = man.Nitrate.ToString();
                            cvm.PhosphorousBook = man.Phosphorous.ToString();
                            cvm.PotassiumBook = man.Potassium.ToString();
                            cvm.NitrateBook = man.Nitrate.ToString();
                            // only show  NITRATE when MANURE_CLASS = COMPOST or COMPOSTBOOK
                            cvm.Compost = _sd.IsManureClassCompostType(man.ManureClass);
                            cvm.ShowNitrate = _sd.IsManureClassCompostClassType(man.ManureClass) || _sd.IsManureClassCompostType(man.ManureClass);
                        }
                    }
                    else
                    {
                        cvm.Nitrogen = string.Empty;
                        cvm.Moisture = string.Empty;
                        cvm.Ammonia = string.Empty;
                        cvm.Nitrate = string.Empty;
                        cvm.Phosphorous = string.Empty;
                        cvm.Potassium = string.Empty;
                        cvm.ManureName = string.Empty;
                    }
                    return View(cvm);
                }

                if (ModelState.IsValid)
                {
                    man = _sd.GetManure(cvm.SelectedManureOption);

                    if (!cvm.BookValue)
                    {
                        if (string.IsNullOrEmpty(cvm.Moisture))
                        {
                            ModelState.AddModelError("Moisture", "Required.");
                        }
                        else
                        {
                            if (!Decimal.TryParse(cvm.Moisture, out userMoisture))
                            {
                                ModelState.AddModelError("Moisture", "Numbers only.");
                            }
                            else
                            {
                                if (userMoisture < 0 || userMoisture > 100)
                                {
                                    ModelState.AddModelError("Moisture", "Invalid %.");
                                }
                                else
                                {
                                    if (man.SolidLiquid.ToUpper() == "SOLID" &&
                                       man.ManureClass.ToUpper() == "OTHER")
                                    {
                                        if (userMoisture > 80)
                                        {
                                            ModelState.AddModelError("Moisture", "must be \u2264 80%.");
                                        }
                                    }
                                    if (man.SolidLiquid.ToUpper() == "LIQUID" &&
                                       man.ManureClass.ToUpper() == "OTHER")
                                    {
                                        if (userMoisture <= 80)
                                        {
                                            ModelState.AddModelError("Moisture", "Must be > 80%.");
                                        }
                                    }
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.Nitrogen))
                        {
                            ModelState.AddModelError("Nitrogen", "Required.");
                        }
                        else
                        {
                            if (!Decimal.TryParse(cvm.Nitrogen, out userNitrogen))
                            {
                                ModelState.AddModelError("Nitrogen", "Numbers only.");
                            }
                            else
                            {
                                if (userNitrogen < 0 || userNitrogen > 100)
                                {
                                    ModelState.AddModelError("Nitrogen", "Invalid %.");
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.Ammonia))
                        {
                            ModelState.AddModelError("Ammonia", "Required.");
                        }
                        else
                        {
                            if (!Decimal.TryParse(cvm.Ammonia, out userAmmonia))
                            {
                                ModelState.AddModelError("Ammonia", "Numbers only.");
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.Phosphorous))
                        {
                            ModelState.AddModelError("Phosphorous", "Required.");
                        }
                        else
                        {
                            if (!Decimal.TryParse(cvm.Phosphorous, out userPhosphorous))
                            {
                                ModelState.AddModelError("Phosphorous", "Numbers only.");
                            }
                            else
                            {
                                if (userPhosphorous < 0 || userPhosphorous > 100)
                                {
                                    ModelState.AddModelError("Phosphorous", "Invalid %.");
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.Potassium))
                        {
                            ModelState.AddModelError("Potassium", "Required.");
                        }
                        else
                        {
                            if (!Decimal.TryParse(cvm.Potassium, out userPotassium))
                            {
                                ModelState.AddModelError("Potassium", "Numbers only.");
                            }
                            else
                            {
                                if (userPotassium < 0 || userPotassium > 100)
                                {
                                    ModelState.AddModelError("Potassium", "Invalid %.");
                                }
                            }
                        }
                        if (cvm.ShowNitrate)
                        {
                            if (string.IsNullOrEmpty(cvm.Nitrate))
                            {
                                ModelState.AddModelError("Nitrate", "Required.");
                            }
                            else
                            {
                                if (!Decimal.TryParse(cvm.Nitrate, out userNitrate))
                                {
                                    ModelState.AddModelError("Nitrate", "Numbers only.");
                                }
                            }
                        }
                        else
                            userNitrate = Convert.ToDecimal(cvm.Nitrate);

                        if (_sd.GetManureByName(cvm.ManureName) != null)
                        {
                            ModelState.AddModelError("ManureName", "Description cannot match predefined entries.");
                        }
                    }

                    List<FarmManure> manures = _ud.GetFarmManures();
                    foreach (var m in manures)
                    {
                        if (m.Customized &&
                           m.Name == cvm.ManureName &&
                           m.Id != cvm.Id)
                        {
                            ModelState.AddModelError("ManureName", "Descriptions must be unique.");
                            break;
                        }
                    }

                    if (!ModelState.IsValid)
                        return View(cvm);

                    if (cvm.Id == null)
                    {
                        FarmManure fm = new FarmManure();
                        if (cvm.BookValue)
                        {
                            fm.ManureId = cvm.SelectedManureOption;
                            fm.Customized = false;
                        }
                        else
                        {
                            man = _sd.GetManure(cvm.SelectedManureOption);

                            fm.Customized = true;
                            fm.ManureId = cvm.SelectedManureOption;
                            fm.Ammonia = Convert.ToInt32(cvm.Ammonia);
                            fm.DMId = man.DryMatterId;
                            fm.ManureClass = man.ManureClass;
                            fm.Moisture = cvm.Moisture;
                            fm.Name = cvm.ManureName;
                            fm.Nitrogen = Convert.ToDecimal(cvm.Nitrogen);
                            fm.NMinerizationId = man.NMineralizationId;
                            fm.Phosphorous = Convert.ToDecimal(cvm.Phosphorous);
                            fm.Potassium = Convert.ToDecimal(cvm.Potassium);
                            fm.Nitrate = cvm.ShowNitrate ? Convert.ToDecimal(cvm.Nitrate) : (decimal?)null;
                            fm.SolidLiquid = man.SolidLiquid;
                        }

                        _ud.AddFarmManure(fm);
                    }
                    else
                    {
                        FarmManure fm = _ud.GetFarmManure(cvm.Id.Value);
                        if (cvm.BookValue)
                        {
                            fm = new FarmManure();
                            fm.Id = cvm.Id.Value;
                            fm.ManureId = cvm.SelectedManureOption;
                            fm.Customized = false;
                        }
                        else
                        {
                            man = _sd.GetManure(cvm.SelectedManureOption);

                            fm.Customized = true;
                            fm.ManureId = cvm.SelectedManureOption;
                            fm.Ammonia = Convert.ToInt32(cvm.Ammonia);
                            fm.DMId = man.DryMatterId;
                            fm.ManureClass = man.ManureClass;
                            fm.Moisture = cvm.Moisture;
                            fm.Name = cvm.ManureName;
                            fm.Nitrogen = Convert.ToDecimal(cvm.Nitrogen);
                            fm.NMinerizationId = man.NMineralizationId;
                            fm.Phosphorous = Convert.ToDecimal(cvm.Phosphorous);
                            fm.Potassium = Convert.ToDecimal(cvm.Potassium);
                            fm.SolidLiquid = man.SolidLiquid;
                            fm.Nitrate = cvm.ShowNitrate ? Convert.ToDecimal(cvm.Nitrate) : (decimal?)null;
                        }

                        _ud.UpdateFarmManure(fm);

                        ReCalculateManure(fm.Id);
                    }

                    string url = Url.Action("RefreshCompostList", "Manure");
                    return Json(new { success = true, Url = url, target = cvm.Target });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error.");
                _logger.LogError(ex, "CompostDetail Exception");
            }

            return PartialView(cvm);
        }

        private void ReCalculateManure(int id)
        {
            NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            List<Field> flds = _ud.GetFields();

            foreach (var fld in flds)
            {
                List<NutrientManure> mans = _ud.GetFieldNutrientsManures(fld.fieldName);

                foreach (var nm in mans)
                {
                    if (id.ToString() == nm.manureId)
                    {
                        int regionid = _ud.FarmDetails().FarmRegion.Value;
                        Region region = _sd.GetRegion(regionid);

                        nOrganicMineralizations = _calculateNutrients.GetNMineralization(_ud.GetFarmManure(Convert.ToInt16(nm.manureId)), region.LocationId);

                        string avail = (nOrganicMineralizations.OrganicN_FirstYear * 100).ToString("###");

                        string nh4 = (_calculateNutrients.GetAmmoniaRetention(_ud.GetFarmManure(Convert.ToInt16(nm.manureId)), Convert.ToInt16(nm.applicationId)) * 100).ToString("###");

                        var nutrientInputs = _calculateNutrients.GetNutrientInputs(
                            _ud.GetFarmManure(Convert.ToInt32(nm.manureId)),
                            region,
                            Convert.ToDecimal(nm.rate),
                            nm.unitId,
                            Convert.ToDecimal(nh4),
                            Convert.ToDecimal(avail)
                            );

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

            dvm.Id = id;
            dvm.Target = target;

            FarmManure nm = _ud.GetFarmManure(id);

            dvm.ManureName = nm.Name;

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
                dvm.Warning = _sd.GetUserPrompt("manuredeletewarning");
            }

            dvm.Action = "Delete";

            return PartialView("CompostDelete", dvm);
        }

        [HttpPost]
        public ActionResult CompostDelete(CompostDeleteViewModel dvm)
        {
            if (ModelState.IsValid)
            {
                // first remove manure from all fields that had it applied
                if (!string.IsNullOrEmpty(dvm.Warning))
                {
                    List<Field> flds = _ud.GetFields();

                    foreach (var fld in flds)
                    {
                        List<NutrientManure> mans = _ud.GetFieldNutrientsManures(fld.fieldName);

                        foreach (var man in mans)
                        {
                            if (dvm.Id.ToString() == man.manureId)
                            {
                                _ud.DeleteFieldNutrientsManure(fld.fieldName, man.id);
                            }
                        }
                    }
                }

                // delete the actual manure
                _ud.DeleteFarmManure(dvm.Id);

                string url = Url.Action("RefreshCompostList", "Manure");
                return Json(new { success = true, Url = url, target = dvm.Target });
            }
            return PartialView("CompostDelete", dvm);
        }

        public IActionResult RefreshCompostList()
        {
            return ViewComponent("Compost");
        }
    }
}