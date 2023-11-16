using Agri.CalculateService;
using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Agri.Models.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SERVERAPI.Filters;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SERVERAPI.Controllers
{
    [SessionTimeout]
    public class SoilController : BaseController
    {
        private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;
        private readonly IChemicalBalanceMessage _chemicalBalanceMessage;
        private readonly ISoilTestConverter _soilTestConversions;
        private readonly bool _showBlueberries;

        public SoilController(
            UserData ud,
            IAgriConfigurationRepository sd,
            IChemicalBalanceMessage chemicalBalanceMessage,
            ISoilTestConverter soilTestConversions)
        {
            _ud = ud;
            _sd = sd;
            _chemicalBalanceMessage = chemicalBalanceMessage;
            _soilTestConversions = soilTestConversions;
            _showBlueberries  = _ud.FarmDetails().UserJourney == UserJourney.Berries;

        }

        [HttpGet]
        public IActionResult SoilTest()
        {
            SoilTestViewModel fvm = new SoilTestViewModel();

            FarmDetails fd = _ud.FarmDetails();
            fvm.selTstOption = fd.TestingMethod;

            if (!string.IsNullOrEmpty(fd.TestingMethod))
                fvm.testSelected = true;

            fvm.tstOptions = new List<SelectListItem>();
            fvm.tstOptions = _sd.GetSoilTestMethodsDll().ToList();

            List<Field> fl = _ud.GetFields();

            fvm.fldsFnd = (fl.Count() > 0) ? true : false;

            fvm.warningMsg = _sd.GetUserPrompt("soiltestwarning");

            fvm.showLeafTests = _showBlueberries;
            fvm.selLeafTstOption = fd.LeafTestingMethod;

            if (!string.IsNullOrEmpty(fd.LeafTestingMethod))
                fvm.leafTestSelected = true;

            fvm.leafTstOptions = new List<SelectListItem>();
            fvm.leafTstOptions = _sd.GetLeafTestMethodsDll().ToList();


            return View(fvm);
        }

        [HttpPost]
        public IActionResult SoilTest(SoilTestViewModel fvm)
        {
            fvm.tstOptions = new List<SelectListItem>();
            fvm.tstOptions = _sd.GetSoilTestMethodsDll().ToList();

            fvm.leafTstOptions = new List<SelectListItem>();
            fvm.leafTstOptions = _sd.GetLeafTestMethodsDll().ToList();

            if (fvm.buttonPressed == "MethodChange")
            {
                ModelState.Clear();
                FarmDetails fd = _ud.FarmDetails();
                fd.TestingMethod = fvm.selTstOption == "No soil tests from within past 3 years" ? string.Empty : fvm.selTstOption;
                _ud.UpdateFarmDetails(fd);
                fvm.testSelected = string.IsNullOrEmpty(fd.TestingMethod) ? false : true;
                if(!fvm.testSelected)
                {
                    _ud.ClearSoilTests();
                }
                List<Field> fl = _ud.GetFields();

                //update fields with convert STP and STK
                _ud.UpdateSTPSTK(fl);

                //update the Nutrient calculations with the new/changed soil test data
                if (!_showBlueberries)
                {
                    var updatedFields = _chemicalBalanceMessage.RecalcCropsSoilTestMessagesByFarm(_ud.GetFields(), _ud.FarmDetails().FarmRegion.Value);

                    foreach (var field in updatedFields)
                    {
                        foreach (var crop in field.Crops)
                        {
                            _ud.UpdateFieldCrop(field.FieldName, crop);
                        }
                    }
                }

                RedirectToAction("SoilTest", "Soil");
            }
            if (fvm.buttonPressed == "LeafTestMethodChange")
            {
                ModelState.Clear();
                FarmDetails fd = _ud.FarmDetails();
                fd.LeafTestingMethod = fvm.selLeafTstOption == "No leaf tests from within past 3 years" ? string.Empty : fvm.selLeafTstOption;
                _ud.UpdateFarmDetails(fd);
                fvm.leafTestSelected = string.IsNullOrEmpty(fd.LeafTestingMethod) ? false : true;
                if (!fvm.leafTestSelected)
                {
                    _ud.ClearLeafTests();
                }
                List<Field> fl = _ud.GetFields();

                //update fields with convert STP and STK
                _ud.UpdateSTPSTK(fl);

                //update the Nutrient calculations with the new/changed soil test data
                if (!_showBlueberries)
                {
                    var updatedFields = _chemicalBalanceMessage.RecalcCropsSoilTestMessagesByFarm(_ud.GetFields(), _ud.FarmDetails().FarmRegion.Value);

                    foreach (var field in updatedFields)
                    {
                        foreach (var crop in field.Crops)
                        {
                            _ud.UpdateFieldCrop(field.FieldName, crop);
                        }
                    }
                }
                RedirectToAction("SoilTest", "Soil");
            }

            return View(fvm);
        }

        [HttpGet]
        public IActionResult SoilTestDetails(string fldName)
        {
            SoilTestDetailsViewModel tvm = new SoilTestDetailsViewModel();

            tvm.title = "Update";
            tvm.url = _sd.GetExternalLink("soiltestexplanation");
            tvm.urlText = _sd.GetUserPrompt("moreinfo");
            tvm.SoilTestValuesMsg = _sd.GetUserPrompt("SoilTestValuesMessage");
            tvm.SoilTestNitrogenNitrateMsg = _sd.GetUserPrompt("SoilTestNitrogenNitrateMessage");
            tvm.SoilTestPhosphorousMsg = _sd.GetUserPrompt("SoilTestPhosphorousMessage");
            tvm.SoilTestPotassiumMsg = _sd.GetUserPrompt("SoilTestPotassiumMessage");
            tvm.SoilTestPHMsg = _sd.GetUserPrompt("SoilTestPHMessage");

            Field fld = _ud.GetFieldDetails(fldName);
            tvm.fieldName = fldName;
            if (fld.SoilTest != null)
            {
                tvm.sampleDate = fld.SoilTest.sampleDate.ToString("MMM-yyyy");
                tvm.dispK = fld.SoilTest.valK.ToString("G29");
                tvm.dispNO3H = fld.SoilTest.valNO3H.ToString("G29");
                tvm.dispP = fld.SoilTest.ValP.ToString("G29");
                tvm.dispPH = fld.SoilTest.valPH.ToString("G29");
            }
            return View(tvm);
        }

        [HttpGet]
        public IActionResult LeafTestDetails(string fldName)
        {
            LeafTestDetailsViewModel tvm = new LeafTestDetailsViewModel();
            tvm.title = "Update";

            Field fld = _ud.GetFieldDetails(fldName);
            tvm.fieldName = fldName;

            tvm.leafTestValuesMsg = _sd.GetUserPrompt("leafTestValuesMessage");
            tvm.leafTestLeafTissuePMsg = _sd.GetUserPrompt("leafTestLeafTissuePMessage");
            tvm.leafTestLeafTissueKMsg = _sd.GetUserPrompt("leafTestLeafTissueKMessage");
            tvm.leafTestCropRequirementNMsg = _sd.GetUserPrompt("leafTestCropRequirementNMessage");
            tvm.leafTestCropRequirementP2O5Msg = _sd.GetUserPrompt("leafTestCropRequirementP2O5Message");
            tvm.leafTestCropRequirementK2O5Msg = _sd.GetUserPrompt("leafTestCropRequirementK2O5Message");
            tvm.leafTestCropRemovalP2O5Msg = _sd.GetUserPrompt("leafTestCropRemovalP2O5Message");
            tvm.leafTestCropRemovalK2O5Msg = _sd.GetUserPrompt("leafTestCropRemovalK2O5Message");

            if (fld.LeafTest != null)
            {
                tvm.leafTissueP = fld.LeafTest.leafTissueP.ToString("G29");
                tvm.leafTissueK = fld.LeafTest.leafTissueK.ToString("G29");
                tvm.cropRequirementN = fld.LeafTest.cropRequirementN.ToString("G29");
                tvm.cropRequirementP2O5 = fld.LeafTest.cropRequirementP2O5.ToString("G29");
                tvm.cropRequirementK2O5 = fld.LeafTest.cropRequirementK2O5.ToString("G29");
                tvm.cropRemovalP2O5 = fld.LeafTest.cropRemovalP2O5.ToString("G29");
                tvm.cropRemovalK2O5 = fld.LeafTest.cropRemovalK2O5.ToString("G29");
            }
            return View(tvm);
        }

        [HttpPost]
        public IActionResult SoilTestDetails(SoilTestDetailsViewModel tvm)
        {
            decimal nmbr;

            if (ModelState.IsValid)
            {
                if (!Decimal.TryParse(tvm.dispNO3H, out nmbr))
                {
                    ModelState.AddModelError("dispNO3H", "Numbers only.");
                }
                else
                {
                    if (nmbr < 0)
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

                if (!ModelState.IsValid)
                {
                    return View(tvm);
                }

                Field fld = _ud.GetFieldDetails(tvm.fieldName);
                if (fld.SoilTest == null)
                {
                    fld.SoilTest = new SoilTest();
                }
                fld.SoilTest.sampleDate = Convert.ToDateTime(tvm.sampleDate);
                fld.SoilTest.ValP = Convert.ToDecimal(tvm.dispP);
                fld.SoilTest.valK = Convert.ToDecimal(tvm.dispK);
                fld.SoilTest.valNO3H = Convert.ToDecimal(tvm.dispNO3H);
                fld.SoilTest.valPH = Convert.ToDecimal(tvm.dispPH);
                fld.SoilTest.ConvertedKelownaK = _soilTestConversions.GetConvertedSTK(_ud.FarmDetails()?.TestingMethod, fld.SoilTest);
                fld.SoilTest.ConvertedKelownaP = _soilTestConversions.GetConvertedSTP(_ud.FarmDetails()?.TestingMethod, fld.SoilTest);

                _ud.UpdateFieldSoilTest(fld);

                //update the Nutrient calculations with the new/changed soil test data
                if (!_showBlueberries)
                {
                    var updatedField = _chemicalBalanceMessage.RecalcCropsSoilTestMessagesByField(fld, _ud.FarmDetails().FarmRegion.Value);
                    foreach (var crop in updatedField.Crops)
                    {
                        _ud.UpdateFieldCrop(updatedField.FieldName, crop);
                    }
                }

                string target = "#test";
                string url = Url.Action("RefreshTestList", "Soil");
                return Json(new { success = true, url = url, target = target });
            }
            return View(tvm);
        }

        [HttpPost]
        public IActionResult LeafTestDetails(LeafTestDetailsViewModel tvm)
        {
            decimal nmbr;

            if (ModelState.IsValid)
            {
                if (!Decimal.TryParse(tvm.leafTissueP, out nmbr))
                {
                    ModelState.AddModelError("leafTissueP", "Numbers only.");
                }
                else
                {
                    if (nmbr < 0)
                    {
                        ModelState.AddModelError("leafTissueP", "Invalid.");
                    }
                }
                if (!Decimal.TryParse(tvm.leafTissueK, out nmbr))
                {
                    ModelState.AddModelError("leafTissueK", "Numbers only.");
                }
                else
                {
                    if (nmbr < 0)
                    {
                        ModelState.AddModelError("leafTissueK", "Invalid.");
                    }
                }
                if (!Decimal.TryParse(tvm.cropRequirementN, out nmbr))
                {
                    ModelState.AddModelError("cropRequirementN", "Numbers only.");
                }
                else
                {
                    if (nmbr < 0)
                    {
                        ModelState.AddModelError("cropRequirementN", "Invalid.");
                    }
                }
                if (!Decimal.TryParse(tvm.cropRequirementP2O5, out nmbr))
                {
                    ModelState.AddModelError("cropRequirementP2O5", "Numbers only.");
                }
                else
                {
                    if (nmbr < 0)
                    {
                        ModelState.AddModelError("cropRequirementP2O5", "Invalid.");
                    }
                }
                if (!Decimal.TryParse(tvm.cropRequirementK2O5, out nmbr))
                {
                    ModelState.AddModelError("cropRequirementK2O5", "Numbers only.");
                }
                else
                {
                    if (nmbr < 0)
                    {
                        ModelState.AddModelError("cropRequirementK2O5", "Invalid.");
                    }
                }
                if (!Decimal.TryParse(tvm.cropRemovalP2O5, out nmbr))
                {
                    ModelState.AddModelError("cropRemovalP2O5", "Numbers only.");
                }
                else
                {
                    if (nmbr < 0)
                    {
                        ModelState.AddModelError("cropRemovalP2O5", "Invalid.");
                    }
                }
                if (!Decimal.TryParse(tvm.cropRemovalK2O5, out nmbr))
                {
                    ModelState.AddModelError("cropRemovalK2O5", "Numbers only.");
                }
                else
                {
                    if (nmbr < 0)
                    {
                        ModelState.AddModelError("cropRemovalK2O5", "Invalid.");
                    }
                }

                if (!ModelState.IsValid)
                {
                    return View(tvm);
                }

                Field fld = _ud.GetFieldDetails(tvm.fieldName);
                if (fld.LeafTest == null)
                {
                    fld.LeafTest = new LeafTest();
                }

                fld.LeafTest.leafTissueP = Convert.ToDecimal(tvm.leafTissueP);
                fld.LeafTest.leafTissueK = Convert.ToDecimal(tvm.leafTissueK);
                fld.LeafTest.cropRequirementN = Convert.ToDecimal(tvm.cropRequirementN);
                fld.LeafTest.cropRequirementP2O5 = Convert.ToDecimal(tvm.cropRequirementP2O5);
                fld.LeafTest.cropRequirementK2O5 = Convert.ToDecimal(tvm.cropRequirementK2O5);
                fld.LeafTest.cropRemovalP2O5 = Convert.ToDecimal(tvm.cropRemovalP2O5);
                fld.LeafTest.cropRemovalK2O5 = Convert.ToDecimal(tvm.cropRemovalK2O5);

                _ud.UpdateFieldLeafTest(fld);

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

                fld.SoilTest = null;

                _ud.UpdateFieldSoilTest(fld);

                string target = "#test";
                string url = Url.Action("RefreshTestList", "Soil");
                return Json(new { success = true, url = url, target = target });
            }
            return PartialView("SoilTestErase", dvm);
        }

        [HttpGet]
        public IActionResult LeafTestErase(string fldName)
        {
            LeafTestDeleteViewModel tvm = new LeafTestDeleteViewModel();
            tvm.title = "Erase";

            tvm.fieldName = fldName;

            return View(tvm);
        }

        [HttpPost]
        public ActionResult LeafTestErase(LeafTestDeleteViewModel dvm)
        {
            if (ModelState.IsValid)
            {
                Field fld = _ud.GetFieldDetails(dvm.fieldName);

                fld.LeafTest = null;

                _ud.UpdateFieldLeafTest(fld);

                string target = "#test";
                string url = Url.Action("RefreshTestList", "Soil");
                return Json(new { success = true, url = url, target = target });
            }
            return PartialView("LeafTestErase", dvm);
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

        public IActionResult MissingTests(string target, string TestType)
        {
            MissingTestsViewModel mvm = new MissingTestsViewModel();
            mvm.target = target;
            string msg = "";

            if ( TestType == null || TestType.Contains("SoilTest"))
            {
                msg += _sd.GetSoilTestWarning();
            }

            if (TestType.Contains("LeafTest"))
            {
                msg += _sd.GetLeafTestWarning();
            }
            mvm.msg = msg;

            return View(mvm);
        }

        [HttpPost]
        public IActionResult MissingTests(MissingTestsViewModel mvm)
        {
            return View(mvm);
        }
    }
}