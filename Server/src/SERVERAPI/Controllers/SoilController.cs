using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.Models;
using SERVERAPI.ViewModels;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Controllers
{
    //[RedirectingAction]
    public class SoilController : Controller
    {
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public Models.Impl.StaticData _sd { get; set; }
        public AppSettings _settings;

        public SoilController(IHostingEnvironment env, UserData ud, Models.Impl.StaticData sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
        }
        [HttpGet]
        public IActionResult SoilTest()
        {
            SoilTestViewModel fvm = new SoilTestViewModel();

            FarmDetails fd = _ud.FarmDetails();
            fvm.selTstOption = fd.testingMethod;

            if (!string.IsNullOrEmpty(fd.testingMethod))
                fvm.testSelected = true;

            fvm.tstOptions = new List<Models.StaticData.SelectListItem>();
            fvm.tstOptions = _sd.GetSoilTestMethodsDll().ToList();

            List<Field> fl = _ud.GetFields();

            fvm.fldsFnd = (fl.Count() > 0) ? true : false;

            fvm.url = _sd.GetExternalLink("soiltestexplanation");
            fvm.urlText = _sd.GetUserPrompt("moreinfo");
            fvm.warningMsg = _sd.GetUserPrompt("soiltestwarning");

            return View(fvm);
        }
        [HttpPost]
        public IActionResult SoilTest(SoilTestViewModel fvm)
        {
            fvm.tstOptions = new List<Models.StaticData.SelectListItem>();
            fvm.tstOptions = _sd.GetSoilTestMethodsDll().ToList();

            if (fvm.buttonPressed == "MethodChange")
            {
                ModelState.Clear();
                FarmDetails fd = _ud.FarmDetails();
                fd.testingMethod = fvm.selTstOption == "select" ? string.Empty : fvm.selTstOption;
                _ud.UpdateFarmDetails(fd);
                fvm.testSelected = string.IsNullOrEmpty(fd.testingMethod) ? false : true;
                Utility.SoilTestConversions soilTestConversions = new Utility.SoilTestConversions(_ud, _sd);
                List<Field> fl = _ud.GetFields();
                
                //update fields with convert STP and STK
                soilTestConversions.UpdateSTPSTK(fl);
                
                //update the Nutrient calculations with the new/changed soil test data
                Utility.ChemicalBalanceMessage cbm = new Utility.ChemicalBalanceMessage(_ud, _sd);
                cbm.RecalcCropsSoilTestMessagesByFarm();

                RedirectToAction("SoilTest", "Soil");
            }
            return View(fvm);
        }
        [HttpGet]
        public IActionResult SoilTestDetails(string fldName)
        {
            SoilTestDetailsViewModel tvm = new SoilTestDetailsViewModel();
            tvm.title = "Update";

            Field fld = _ud.GetFieldDetails(fldName);
            tvm.fieldName = fldName;
            if (fld.soilTest != null)
            {                
                tvm.sampleDate = fld.soilTest.sampleDate.ToString("MMM-yyyy");
                tvm.dispK = fld.soilTest.valK.ToString();
                tvm.dispNO3H = fld.soilTest.valNO3H.ToString();
                tvm.dispP = fld.soilTest.ValP.ToString();
                tvm.dispPH = fld.soilTest.valPH.ToString();
            }

            tvm.url = _sd.GetExternalLink("soiltestvaluesexplanation");
            tvm.urlText = _sd.GetUserPrompt("moreinfo");

            return View(tvm);
        }
        [HttpPost]
        public IActionResult SoilTestDetails(SoilTestDetailsViewModel tvm)
        {
            decimal nmbr;

            if(ModelState.IsValid)
            {
                if (!Decimal.TryParse(tvm.dispNO3H, out nmbr))
                {
                    ModelState.AddModelError("dispNO3H", "Numbers only.");
                }
                else
                {
                    if(nmbr < 0)
                    {
                        ModelState.AddModelError("dispNO3H", "Invalid.");
                    }
                }
                if (!Decimal.TryParse(tvm.dispP, out nmbr))
                {
                    ModelState.AddModelError("dispP", "Numbers only.");
                }
                else
                {
                    if (nmbr < 0)
                    {
                        ModelState.AddModelError("dispP", "Invalid.");
                    }
                }
                if (!Decimal.TryParse(tvm.dispK, out nmbr))
                {
                    ModelState.AddModelError("dispK", "Numbers only.");
                }
                else
                {
                    if (nmbr < 0)
                    {
                        ModelState.AddModelError("dispK", "Invalid.");
                    }
                }
                if (!Decimal.TryParse(tvm.dispPH, out nmbr))
                {
                    ModelState.AddModelError("dispPH", "Numbers only.");
                }
                else
                {
                    if (nmbr < 0 ||
                        nmbr > 14)
                    {
                        ModelState.AddModelError("dispPH", "Invalid.");
                    }
                }
                if(!ModelState.IsValid)
                {
                    return View(tvm);
                }

                Utility.SoilTestConversions soilTestConversions = new Utility.SoilTestConversions(_ud, _sd);
                Field fld = _ud.GetFieldDetails(tvm.fieldName);
                if(fld.soilTest == null)
                {
                    fld.soilTest = new Models.SoilTest();
                }
                fld.soilTest.sampleDate = Convert.ToDateTime(tvm.sampleDate);
                fld.soilTest.ValP = Convert.ToDecimal(tvm.dispP);
                fld.soilTest.valK = Convert.ToDecimal(tvm.dispK);
                fld.soilTest.valNO3H = Convert.ToDecimal(tvm.dispNO3H);
                fld.soilTest.valPH = Convert.ToDecimal(tvm.dispPH);
                fld.soilTest.ConvertedKelownaK = soilTestConversions.GetConvertedSTK(fld.soilTest);
                fld.soilTest.ConvertedKelownaP = soilTestConversions.GetConvertedSTP(fld.soilTest);

                _ud.UpdateFieldSoilTest(fld);

                //update the Nutrient calculations with the new/changed soil test data
                Utility.ChemicalBalanceMessage cbm = new Utility.ChemicalBalanceMessage(_ud, _sd);
                cbm.RecalcCropsSoilTestMessagesByField(tvm.fieldName);

                string target = "#test";
                string url = Url.Action("RefreshTestList", "Soil");
                return Json(new { success = true, url = url, target = target });

            }
            return View(tvm);
        }
        [HttpGet]
        public IActionResult SoilTestErase(string fldName)
        {
            SoilTestDeleteViewModel tvm = new SoilTestDeleteViewModel();
            tvm.title = "Erase";

            tvm.fieldName = fldName;

            return View(tvm);
        }
        [HttpPost]
        public ActionResult SoilTestErase(SoilTestDeleteViewModel dvm)
        {
            if (ModelState.IsValid)
            {
                Field fld = _ud.GetFieldDetails(dvm.fieldName);

                fld.soilTest = null;

                _ud.UpdateFieldSoilTest(fld);

                string target = "#test";
                string url = Url.Action("RefreshTestList", "Soil");
                return Json(new { success = true, url = url, target = target });
            }
            return PartialView("SoilTestErase", dvm);
        }
        public IActionResult RefreshTestList()
        {
            return ViewComponent("SoilTests");
        }
        [HttpGet]
        public IActionResult MissingMethod()
        {
            return View();
        }
        public IActionResult MissingTests(string target)
        {
            MissingTestsViewModel mvm = new MissingTestsViewModel();
            mvm.target = target;
            mvm.msg = _sd.GetSoilTestWarning();

            return View(mvm);
        }
        [HttpPost]
        public IActionResult MissingTests(MissingTestsViewModel mvm)
        {

            return View(mvm);
        }
    }
}
