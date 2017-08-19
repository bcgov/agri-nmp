using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.ViewModels;
using SERVERAPI.Models;
using Newtonsoft.Json;

namespace SERVERAPI.Controllers
{
    public class NutrientsController : Controller
    {
        private IHostingEnvironment _env;

        public NutrientsController(IHostingEnvironment env)
        {
            _env = env;
        }
        // GET: /<controller>/
        public IActionResult Calculate(int? id)
        {
            CalculateViewModel cvm = new CalculateViewModel();
            cvm.fields = new List<Field>();

            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            Models.Impl.StaticData sd = new Models.Impl.StaticData();

            // not id entered so default to the first one for the farm
            if(id == null)
            {
                YearData yd = farmData.years.FirstOrDefault(y => y.year == farmData.year);
                if(yd.fields == null)
                {
                    cvm.fldsFnd = false;
                }
                else
                {
                    cvm.fldsFnd = true;
                    foreach(var f in yd.fields)
                    {
                        cvm.fields.Add(f);
                    }
                    cvm.currFld = cvm.fields[0].fieldName;
                }
            }
            else
            {

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

            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            ManureDetailsSetup(ref mvm);

            if(id != null)
            {

            }
            else

            {
                mvm.yrN = "0.0";
                mvm.yrP2o5 = "0.0";
                mvm.yrK2o = "0.0";
                mvm.ltN = "0.0";
                mvm.ltP2o5 = "0.0";
                mvm.ltK2o = "0.0";
            }

            return PartialView(mvm);
        }
        [HttpPost]
        public IActionResult ManureDetails(ManureDetailsViewModel mvm)
        {
            int nextId = 1;
            ManureDetailsSetup(ref mvm);

            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            if(mvm.buttonPressed == "TypeChange")
            {
                ModelState.Clear();
                mvm.buttonPressed = "";
                mvm.btnText = "Calculate";

                if (mvm.selManOption != "")
                {
                    Models.Impl.StaticData sd = new Models.Impl.StaticData();
                    Models.StaticData.Manure man = sd.GetManure(HttpContext, mvm.selManOption);
                    if(mvm.currUnit != man.solid_liquid)
                    {
                        mvm.currUnit = man.solid_liquid;
                        mvm.rateOptions = sd.GetUnitsDll(HttpContext, mvm.currUnit).ToList();
                    }
                }
                return View(mvm);
            }

            if (ModelState.IsValid)
            {
                if (mvm.btnText == "Calculate")
                {
                    ModelState.Clear();
                    mvm.yrN = "1.1";
                    mvm.yrP2o5 = "2.2";
                    mvm.yrK2o = "3.3";
                    mvm.ltN = "4.4";
                    mvm.ltP2o5 = "5.5";
                    mvm.ltK2o = "6.6";

                    mvm.btnText = "Add to Field";
                }
                else
                {
                    if(mvm.id == null)
                    {
                        if (farmData.years != null)
                        {
                            YearData yd = farmData.years.FirstOrDefault(y => y.year == farmData.year);
                            if (yd.fields == null)
                            {
                                yd.fields = new List<Field>();
                            }
                            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == mvm.fieldName);
                            if (fld.nutrients == null)
                            {
                                fld.nutrients = new Nutrients();
                            }
                            List<NutrientManure> fldManures = fld.nutrients.nutrientManures;
                            if (fldManures == null)
                            {
                                fldManures = new List<NutrientManure>();
                                fld.nutrients.nutrientManures = fldManures;
                            }
                            foreach(var f in fldManures)
                            {
                                nextId = nextId <= f.id ? f.id + 1 : nextId;
                            }
                            NutrientManure nm = new NutrientManure()
                            {
                                id = nextId,
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

                            fldManures.Add(nm);
                        }
                    }

                    HttpContext.Session.SetObjectAsJson("FarmData", farmData);

                    string url = Url.Action("RefreshManureList", "Nutrients", new {fieldName = mvm.fieldName });
                    return Json(new { success = true, url = url, farmdata = JsonConvert.SerializeObject(farmData) });
                }
            }

            return PartialView(mvm);
        }
        private void ManureDetailsSetup(ref ManureDetailsViewModel mvm)
        {
            Models.Impl.StaticData sd = new Models.Impl.StaticData();

            mvm.manOptions = new List<StaticData.SelectListItem>();
            mvm.manOptions = sd.GetManuresDll(HttpContext).ToList();

            mvm.applOptions = new List<StaticData.SelectListItem>();
            mvm.applOptions = sd.GetApplicationsDll(HttpContext).ToList();

            mvm.rateOptions = new List<StaticData.SelectListItem>();
            mvm.rateOptions = sd.GetUnitsDll(HttpContext, mvm.currUnit).ToList();

            return;
        }
        public IActionResult RefreshManureList(string fieldName)
        {
            return ViewComponent("CalcManure", new { fldName = fieldName });
        }
    }
}