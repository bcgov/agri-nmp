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
    public class SoilController : Controller
    {
        private IHostingEnvironment _env;
        private UserData _ud;
        private Models.Impl.StaticData _sd;

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
            fvm.selMthOption = fd.testingMethod;

            fvm.mthOptions = new List<Models.StaticData.SelectListItem>();
            fvm.mthOptions = _sd.GetSoilTestMethodsDll().ToList();

            List<Field> fl = _ud.GetFields();

            fvm.fldsFnd = (fl.Count() > 0) ? true : false;

            return View(fvm);
        }
        [HttpPost]
        public IActionResult SoilTest(SoilTestViewModel fvm)
        {
            fvm.mthOptions = new List<Models.StaticData.SelectListItem>();
            fvm.mthOptions = _sd.GetSoilTestMethodsDll().ToList();

            if (fvm.buttonPressed == "MethodChange")
            {
                FarmDetails fd = _ud.FarmDetails();
                fd.testingMethod = fvm.selMthOption;
                _ud.UpdateFarmDetails(fd);
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

            return View(tvm);
        }
        [HttpPost]
        public IActionResult SoilTestDetails(SoilTestDetailsViewModel tvm)
        {
            if(ModelState.IsValid)
            {
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

                _ud.UpdateFieldSoilTest(fld);

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
    }
}
