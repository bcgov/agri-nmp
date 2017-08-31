using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.ViewModels;
using SERVERAPI.Models;
using Newtonsoft.Json;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewComponents;
using SERVERAPI.Utility;
using static SERVERAPI.Models.StaticData;

namespace SERVERAPI.Controllers
{
    public class NutrientsController : Controller
    {
        private IHostingEnvironment _env;
        private UserData _ud;
        private Models.Impl.StaticData _sd;

        public NutrientsController(IHostingEnvironment env, UserData ud, Models.Impl.StaticData sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
        }
        // GET: /<controller>/
        public IActionResult Calculate(string id)
        {
            CalculateViewModel cvm = new CalculateViewModel();
            cvm.fields = new List<Field>();

            // not id entered so default to the first one for the farm
            if(id == null)
            {
                List<Field> fldLst = _ud.GetFields();

                if(fldLst.Count() == 0)
                {
                    cvm.fldsFnd = false;
                }
                else
                {
                    cvm.fldsFnd = true;
                    foreach(var f in fldLst)
                    {
                        cvm.fields.Add(f);
                    }
                    cvm.currFld = cvm.fields[0].fieldName;
                }
            }
            else
            {
                cvm.currFld = id;
                List<Field> fldLst = _ud.GetFields();
                cvm.fldsFnd = true;
                foreach (var f in fldLst)
                {
                    cvm.fields.Add(f);
                }
            }

            return View(cvm);
        }
        [HttpPost]
        public IActionResult Calculate(CalculateViewModel cvm)
        {

            return View(cvm);
        }

        public IActionResult ManureDetails(string fldName, int? id)
        {
            ManureDetailsViewModel mvm = new ManureDetailsViewModel();

            mvm.fieldName = fldName;
            mvm.title = id == null ? "Add" : "Edit";
            mvm.btnText = id == null ? "Calculate" : "Return";
            mvm.id = id;
            mvm.avail = "40";
            mvm.nh4 = "40";
            mvm.stdN = true;
            mvm.stdAvail = true;

            //var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            ManureDetailsSetup(ref mvm);

            if(id != null)
            {
                NutrientManure nm = _ud.GetFieldNutrientsManure(fldName, id.Value);

                mvm.avail = nm.nAvail.ToString();
                mvm.selRateOption = nm.unitId;
                mvm.selManOption = nm.manureId;
                mvm.selApplOption = nm.applicationId;
                mvm.rate = nm.rate.ToString();
                mvm.nh4 = nm.nh4Retention.ToString();
                mvm.yrN = nm.yrN.ToString();
                mvm.yrP2o5 = nm.yrP2o5.ToString();
                mvm.yrK2o = nm.yrK2o.ToString();
                mvm.ltN = nm.ltN.ToString();
                mvm.ltP2o5 = nm.ltP2o5.ToString();
                mvm.ltK2o = nm.ltK2o.ToString();
                Models.StaticData.Manure man = _sd.GetManure(nm.manureId);
                mvm.currUnit = man.solid_liquid;
                mvm.rateOptions = _sd.GetUnitsDll(mvm.currUnit).ToList();

                mvm.stdN = Convert.ToDecimal(mvm.nh4) != 40 ? false : true;
                mvm.stdAvail = Convert.ToDecimal(mvm.avail) != 40 ? false : true;

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

            return PartialView(mvm);
        }
        [HttpPost]
        public IActionResult ManureDetails(ManureDetailsViewModel mvm)
        {
            ManureDetailsSetup(ref mvm);

            if (mvm.buttonPressed == "ResetN")
            {
                ModelState.Clear();
                mvm.buttonPressed = "";
                mvm.btnText = "Calculate";
                mvm.nh4 = "40";
                mvm.stdN = true;
                return View(mvm);
            }

            if (mvm.buttonPressed == "ResetA")
            {
                ModelState.Clear();
                mvm.buttonPressed = "";
                mvm.btnText = "Calculate";
                mvm.avail = "40";
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
                    Models.StaticData.Manure man = _sd.GetManure(mvm.selManOption);
                    if(mvm.currUnit != man.solid_liquid)
                    {
                        mvm.currUnit = man.solid_liquid;
                        mvm.rateOptions = _sd.GetUnitsDll(mvm.currUnit).ToList();
                        mvm.selRateOption = mvm.rateOptions[0].Id.ToString();
                    }
                }
                return View(mvm);
            }

            if (ModelState.IsValid)
            {
                if (mvm.btnText == "Calculate")
                {
                    ModelState.Clear();
                    NutrientInputs nutrientInputs = new NutrientInputs();
                    CalculateNutrients calculateNutrients = new Utility.CalculateNutrients(_env, _ud, _sd);                   

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

                    if(Convert.ToDecimal(mvm.nh4) != 40)
                    {
                        mvm.stdN = false;
                    }
                    if (Convert.ToDecimal(mvm.avail) != 40)
                    {
                        mvm.stdAvail = false;
                    }
                }
                else
                {
                    if(mvm.id == null)
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

                        _ud.AddFieldNutrientsManure(mvm.fieldName, nm);
                    }
                    else
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
                    string target = "#manure";
                    string url = Url.Action("RefreshManureList", "Nutrients", new {fieldName = mvm.fieldName });
                    return Json(new { success = true, url = url, target = target });
                }
            }

            return PartialView(mvm);
        }
        private void ManureDetailsSetup(ref ManureDetailsViewModel mvm)
        {
            mvm.manOptions = new List<Models.StaticData.SelectListItem>();
            mvm.manOptions = _sd.GetManuresDll().ToList();

            mvm.applOptions = new List<Models.StaticData.SelectListItem>();
            mvm.applOptions = _sd.GetApplicationsDll().ToList();

            mvm.rateOptions = new List<Models.StaticData.SelectListItem>();
            mvm.rateOptions = _sd.GetUnitsDll(mvm.currUnit).ToList();
            mvm.selRateOptionText = "(lb/ac)";

            return;
        }

        public IActionResult CropDetails(string fldName, int? id)
        {
            CropDetailsViewModel cvm = new CropDetailsViewModel();

            cvm.fieldName = fldName;
            cvm.title = id == null ? "Add" : "Edit";
            cvm.id = id;

            //var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            CropDetailsSetup(ref cvm);

            if (id != null)
            {
                //Crop cp = _ud.GetFieldNutrientsManure(fldName, id.Value);

                //mvm.avail = nm.nAvail.ToString();
                //mvm.selRateOption = nm.unitId;
                //mvm.selManOption = nm.manureId;
                //mvm.selApplOption = nm.applicationId;
                //mvm.rate = nm.rate.ToString();
                //mvm.nh4 = nm.nh4Retention.ToString();
                //mvm.yrN = nm.yrN.ToString();
                //mvm.yrP2o5 = nm.yrP2o5.ToString();
                //mvm.yrK2o = nm.yrK2o.ToString();
                //mvm.ltN = nm.ltN.ToString();
                //mvm.ltP2o5 = nm.ltP2o5.ToString();
                //mvm.ltK2o = nm.ltK2o.ToString();
                //Models.StaticData.Manure man = _sd.GetManure(nm.manureId);
                //mvm.currUnit = man.solid_liquid;
                //mvm.rateOptions = _sd.GetUnitsDll(mvm.currUnit).ToList();

                //mvm.stdN = Convert.ToDecimal(mvm.nh4) != 40 ? false : true;
                //mvm.stdAvail = Convert.ToDecimal(mvm.avail) != 40 ? false : true;

            }
            else

            {
                cvm.reqN = "  0";
                cvm.reqP2o5 = "  0";
                cvm.reqK2o = "  0";
                cvm.remN = "  0";
                cvm.remP2o5 = "  0";
                cvm.remK2o = "  0";
            }

            return PartialView(cvm);
        }
        [HttpPost]
        public IActionResult CropDetails(CropDetailsViewModel cvm)
        {
            CropDetailsSetup(ref cvm);

            if (cvm.buttonPressed == "TypeChange")
            {
                ModelState.Clear();
                cvm.buttonPressed = "";
                cvm.btnText = "Calculate";

                if (cvm.selTypOption != "")
                {
                    //Models.StaticData.Manure man = _sd.GetCrops(cvm.selTypOption);
                    cvm.showCrude = (cvm.selTypOption == "1") ? true : false;
                }
                return View(cvm);
            }

            if (ModelState.IsValid)
            {
                if (cvm.btnText == "Calculate")
                {
                    ModelState.Clear();
                    cvm.reqN = "111";
                    cvm.reqP2o5 = "222";
                    cvm.reqK2o = "333";
                    cvm.remN = "444";
                    cvm.remP2o5 = "555";
                    cvm.remK2o = "666";

                    cvm.btnText = cvm.id == null ? "Add to Field" : "Update Field";
                }
                else
                {
                    if (cvm.id == null)
                    {

                        Crop crp = new Crop()
                        {
                            cropId = cvm.selCropOption,
                            yield = Convert.ToDecimal(cvm.yield),
                            reqN = Convert.ToDecimal(cvm.reqN),
                            reqP2o5 = Convert.ToDecimal(cvm.reqP2o5),
                            reqK2o = Convert.ToDecimal(cvm.reqK2o),
                            remN = Convert.ToDecimal(cvm.remN),
                            remP2o5 = Convert.ToDecimal(cvm.remP2o5),
                            remK2o = Convert.ToDecimal(cvm.remK2o)
                        };

                        _ud.AddFieldCrop(cvm.fieldName, crp);
                    }
                    else
                    {
                        Crop crp = _ud.GetFieldCrop(cvm.fieldName, cvm.id.Value);
                        crp.cropId = cvm.selCropOption;
                        crp.yield = Convert.ToDecimal(cvm.yield);
                        crp.reqN = Convert.ToDecimal(cvm.reqN);
                        crp.reqP2o5 = Convert.ToDecimal(cvm.reqP2o5);
                        crp.reqK2o = Convert.ToDecimal(cvm.reqK2o);
                        crp.remN = Convert.ToDecimal(cvm.remN);
                        crp.remP2o5 = Convert.ToDecimal(cvm.remP2o5);
                        crp.remK2o = Convert.ToDecimal(cvm.remK2o);

                        _ud.UpdateFieldCrop(cvm.fieldName, crp);
                    }
                    string target = "#crop";
                    string url = Url.Action("RefreshCropList", "Nutrients", new { fieldName = cvm.fieldName });
                    return Json(new { success = true, url = url, target = target });
                }
            }

            return PartialView(cvm);
        }
        private void CropDetailsSetup(ref CropDetailsViewModel cvm)
        {
            cvm.typOptions = new List<Models.StaticData.SelectListItem>();
            cvm.typOptions = _sd.GetCropTypesDll().ToList();

            cvm.cropOptions = new List<Models.StaticData.SelectListItem>();
            //cvm.cropOptions = _sd.GetApplicationsDll().ToList();

            cvm.prevOptions = new List<Models.StaticData.SelectListItem>();
            //cvm.cropOptions = _sd.GetApplicationsDll().ToList();

            return;
        }
        public IActionResult RefreshManureList(string fieldName)
        {
            return ViewComponent("CalcManure", new { fldName = fieldName });
        }
        public IActionResult RefreshFieldList(string fieldName)
        {
            return RedirectToAction("Calculate", "Nutrients");
            //return ViewComponent("FieldList");
        }
        [HttpGet]
        public ActionResult ManureDelete(string fldName, int id)
        {
            ManureDeleteViewModel dvm = new ManureDeleteViewModel();
            dvm.id = id;
            dvm.fldName = fldName;

            NutrientManure nm = _ud.GetFieldNutrientsManure(fldName, id);
            dvm.matType = _sd.GetManure(nm.manureId).name;

            dvm.act = "Delete";

            return PartialView("ManureDelete", dvm);
        }
        [HttpPost]
        public ActionResult ManureDelete(ManureDeleteViewModel dvm)
        {
            if (ModelState.IsValid)
            {
                _ud.DeleteFieldNutrientsManure(dvm.fldName, dvm.id);

                string target = "#manure";
                string url = Url.Action("RefreshManureList", "Nutrients", new { fieldName = dvm.fldName });
                return Json(new { success = true, url = url, target = target });
            }
            return PartialView("ManureDelete", dvm);
        }
        public IActionResult RefreshCropList(string fieldName)
        {
            return ViewComponent("CalcCrop", new { fldName = fieldName });
        }
        //[HttpGet]
        //public ActionResult CropDelete(string fldName, int id)
        //{
        //    CropDeleteViewModel dvm = new CropDeleteViewModel();
        //    dvm.id = id;
        //    dvm.fldName = fldName;

        //    Crop crp = _ud.GetFieldCrop(fldName, id);
        //    dvm.cropName = _sd.GetCrop(crp.cropId).name;

        //    dvm.act = "Delete";

        //    return PartialView("CropDelete", dvm);
        //}
        //[HttpPost]
        //public ActionResult ManureDelete(ManureDeleteViewModel dvm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _ud.DeleteFieldNutrientsManure(dvm.fldName, dvm.id);

        //        string target = "#manure";
        //        string url = Url.Action("RefreshManureList", "Nutrients", new { fieldName = dvm.fldName });
        //        return Json(new { success = true, url = url, target = target });
        //    }
        //    return PartialView("ManureDelete", dvm);
        //}
    }
}