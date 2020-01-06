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

            mvm.act = id == null ? "Add" : "Edit";
            mvm.url = _sd.GetExternalLink("labanalysisexplanation");
            mvm.urlText = _sd.GetUserPrompt("moreinfo");

            if (id != null)
            {
                FarmManure fm = _ud.GetFarmManure(id.Value);
                mvm.selManOption = fm.ManureId;

                if (!fm.Customized)
                {
                    mvm.bookValue = true;
                    mvm.compost = false;
                    mvm.onlyCustom = false;
                    mvm.showNitrate = false;
                }
                else
                {
                    mvm.bookValue = false;
                    mvm.compost = _sd.IsManureClassCompostType(fm.ManureClass);
                    mvm.onlyCustom = (_sd.IsManureClassOtherType(fm.ManureClass) || _sd.IsManureClassCompostType(fm.ManureClass) || _sd.IsManureClassCompostClassType(fm.ManureClass));
                    mvm.showNitrate = (_sd.IsManureClassCompostType(fm.ManureClass) || _sd.IsManureClassCompostClassType(fm.ManureClass));
                }
                mvm.manureName = fm.Name;
                mvm.moisture = fm.Moisture;
                mvm.nitrogen = fm.Nitrogen.ToString("#0.00");
                mvm.ammonia = fm.Ammonia.ToString("#0");
                mvm.phosphorous = fm.Phosphorous.ToString("#0.00");
                mvm.potassium = fm.Potassium.ToString("#0.00");
                mvm.nitrate = fm.Nitrate.HasValue ? fm.Nitrate.Value.ToString("#0") : ""; // old version of datafile
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
            cvm.manOptions = new List<SelectListItem>();
            cvm.manOptions = _sd.GetManuresDll().ToList();

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
                        if (m.Customized &&
                           m.Name == cvm.manureName &&
                           m.Id != cvm.id)
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
                            fm.ManureId = cvm.selManOption;
                            fm.Customized = false;
                        }
                        else
                        {
                            man = _sd.GetManure(cvm.selManOption.ToString());

                            fm.Customized = true;
                            fm.ManureId = cvm.selManOption;
                            fm.Ammonia = Convert.ToInt32(cvm.ammonia);
                            fm.DMId = man.DryMatterId;
                            fm.ManureClass = man.ManureClass;
                            fm.Moisture = cvm.moisture;
                            fm.Name = cvm.manureName;
                            fm.Nitrogen = Convert.ToDecimal(cvm.nitrogen);
                            fm.NMinerizationId = man.NMineralizationId;
                            fm.Phosphorous = Convert.ToDecimal(cvm.phosphorous);
                            fm.Potassium = Convert.ToDecimal(cvm.potassium);
                            fm.Nitrate = cvm.showNitrate ? Convert.ToDecimal(cvm.nitrate) : (decimal?)null;
                            fm.SolidLiquid = man.SolidLiquid;
                        }

                        _ud.AddFarmManure(fm);
                    }
                    else
                    {
                        FarmManure fm = _ud.GetFarmManure(cvm.id.Value);
                        if (cvm.bookValue)
                        {
                            fm = new FarmManure();
                            fm.Id = cvm.id.Value;
                            fm.ManureId = cvm.selManOption;
                            fm.Customized = false;
                        }
                        else
                        {
                            man = _sd.GetManure(cvm.selManOption.ToString());

                            fm.Customized = true;
                            fm.ManureId = cvm.selManOption;
                            fm.Ammonia = Convert.ToInt32(cvm.ammonia);
                            fm.DMId = man.DryMatterId;
                            fm.ManureClass = man.ManureClass;
                            fm.Moisture = cvm.moisture;
                            fm.Name = cvm.manureName;
                            fm.Nitrogen = Convert.ToDecimal(cvm.nitrogen);
                            fm.NMinerizationId = man.NMineralizationId;
                            fm.Phosphorous = Convert.ToDecimal(cvm.phosphorous);
                            fm.Potassium = Convert.ToDecimal(cvm.potassium);
                            fm.SolidLiquid = man.SolidLiquid;
                            fm.Nitrate = cvm.showNitrate ? Convert.ToDecimal(cvm.nitrate) : (decimal?)null;
                        }

                        _ud.UpdateFarmManure(fm);

                        ReCalculateManure(fm.Id);
                    }

                    string url = Url.Action("RefreshCompostList", "Manure");
                    return Json(new { success = true, url = url, target = cvm.target });
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

            dvm.id = id;
            dvm.target = target;

            FarmManure nm = _ud.GetFarmManure(id);

            dvm.manureName = nm.Name;

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
    }
}