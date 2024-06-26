﻿using Agri.CalculateService;
using Agri.Data;
using Agri.Models;
using Agri.Models.Calculate;
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
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;

namespace SERVERAPI.Controllers
{
    [SessionTimeout]
    public class SoilController : BaseController
    {
        private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;
        private readonly IChemicalBalanceMessage _chemicalBalanceMessage;
        private readonly ISoilTestConverter _soilTestConversions;
        private readonly bool _isBerry;
        private readonly ICalculateCropRequirementRemoval _calculateCropRequirementRemoval;

        public SoilController(
            UserData ud,
            IAgriConfigurationRepository sd,
            IChemicalBalanceMessage chemicalBalanceMessage,
            ISoilTestConverter soilTestConversions,
            ICalculateCropRequirementRemoval calculateCropRequirementRemoval)
        {
            _ud = ud;
            _sd = sd;
            _chemicalBalanceMessage = chemicalBalanceMessage;
            _soilTestConversions = soilTestConversions;
            _isBerry  = _ud.FarmDetails().UserJourney == UserJourney.Berries;
            _calculateCropRequirementRemoval = calculateCropRequirementRemoval;
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

            fvm.showLeafTests = _isBerry;
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
                if (!_isBerry)
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
                if (!_isBerry)
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

            tvm.btnText = "Calculate";

            tvm.LeafTestDetailsItems = new List<LeafTestDetailsItem>();

            if (fld.LeafTest != null)
            {
                tvm.btnText = "Return";
                tvm.leafTissueP = fld.LeafTest.leafTissueP.ToString();
                tvm.leafTissueK = fld.LeafTest.leafTissueK.ToString();

                foreach (FieldCrop crop in fld.Crops)
                {
                    tvm.LeafTestDetailsItems.Add(new LeafTestDetailsItem()
                    {
                        Id = crop.id,
                        cropName = _sd.GetCrop(Convert.ToInt32(crop.cropId)).CropName,
                        cropRequirementN = crop.reqN.ToString("N0"),
                        cropRequirementP2O5 = crop.reqP2o5.ToString("N0"),
                        cropRequirementK2O = crop.reqK2o.ToString("N0"),
                        cropRemovalP2O5 = crop.remP2o5.ToString("N0"),
                        cropRemovalK2O5 = crop.remK2o.ToString("N0")
                    });
                }
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
                if (_isBerry)
                {
                    UpdateCropRequirementsBerries(fld);
                }
                else
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
            
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                if (tvm.btnText == "Calculate")
                {

                    if (!ModelState.IsValid)
                    {
                        return View(tvm);
                    }
                    var fld = _ud.GetFieldDetails(tvm.fieldName);
                    CropRequirementRemoval crr = null;
                    tvm.LeafTestDetailsItems = new List<LeafTestDetailsItem>();

                    decimal soilTestValP = 0;
                    decimal soilTestValK = 0;
                    foreach (FieldCrop crop in fld.Crops)
                    {
                        string cropName = _sd.GetCrop(Convert.ToInt32(crop.cropId)).CropName;
                        if (cropName == "Blueberry")
                        {
                            soilTestValP = (fld.SoilTest == null || (fld.SoilTest != null && fld.SoilTest.ValP == 0)) ?
                                _calculateCropRequirementRemoval.defaultBlueberrySoilTestP :
                                fld.SoilTest.ValP;

                            crr = _calculateCropRequirementRemoval.GetCropRequirementRemovalBlueberries(
                                                                                crop.yieldByHarvestUnit,
                                                                                crop.plantAgeYears,
                                                                                crop.numberOfPlantsPerAcre,
                                                                                crop.willSawdustBeApplied,
                                                                                crop.willPlantsBePruned,
                                                                                crop.whereWillPruningsGo,
                                                                                soilTestValP,
                                                                                Convert.ToDecimal(tvm.leafTissueP),
                                                                                Convert.ToDecimal(tvm.leafTissueK));

                        }
                        else if (cropName == "Raspberry")
                        {
                            soilTestValP = (fld.SoilTest == null || (fld.SoilTest != null && fld.SoilTest.ValP == 0)) ?
                                _calculateCropRequirementRemoval.defaultRaspberrySoilTestP :
                                fld.SoilTest.ValP;
                            soilTestValK = (fld.SoilTest == null || (fld.SoilTest != null && fld.SoilTest.valK == 0)) ?
                                _calculateCropRequirementRemoval.defaultRaspberrySoilTestK :
                                fld.SoilTest.valK;

                            crr = _calculateCropRequirementRemoval.GetCropRequirementRemovalRaspberries(
                                                                                   crop.yieldByHarvestUnit,
                                                                                   crop.willSawdustBeApplied,
                                                                                   crop.willPlantsBePruned,
                                                                                   crop.whereWillPruningsGo,
                                                                                   soilTestValP,
                                                                                   soilTestValK,
                                                                                   Convert.ToDecimal(tvm.leafTissueP),
                                                                                    Convert.ToDecimal(tvm.leafTissueK));
                        }
                        if (crr != null)
                        {
                            tvm.LeafTestDetailsItems.Add(new LeafTestDetailsItem()
                            {
                                Id = crop.id,
                                cropName = _sd.GetCrop(Convert.ToInt32(crop.cropId)).CropName,
                                cropRequirementN = crr.N_Requirement.ToString(),
                                cropRequirementP2O5 = crr.P2O5_Requirement.ToString(),
                                cropRequirementK2O = crr.K2O_Requirement.ToString(),
                                cropRemovalP2O5 = crr.P2O5_Removal.ToString(),
                                cropRemovalK2O5 = crr.K2O_Removal.ToString()
                            });
                        }
                    }

                    tvm.btnText = "Add to Field";

                    return View(tvm);
                }
                if (tvm.btnText == "Return")
                {
                    string target = "#test";
                    string url = Url.Action("RefreshTestList", "Soil");
                    return Json(new { success = true, url = url, target = target });
                }
                if (tvm.btnText == "Add to Field")
                {
                    Field fld = _ud.GetFieldDetails(tvm.fieldName);
                    if (fld.LeafTest == null)
                    {
                        fld.LeafTest = new LeafTest();
                    }

                    fld.LeafTest.leafTissueP = Convert.ToDecimal(tvm.leafTissueP);

                    fld.LeafTest.leafTissueK = Convert.ToDecimal(tvm.leafTissueK);

                    _ud.UpdateFieldLeafTest(fld);
                    foreach (FieldCrop crop in fld.Crops)
                    {
                        crop.reqN = tvm.LeafTestDetailsItems.Where(item => item.Id == crop.id)
                                        .Select(crop => Convert.ToDecimal(crop.cropRequirementN)).FirstOrDefault();
                        crop.reqP2o5 = tvm.LeafTestDetailsItems.Where(item => item.Id == crop.id)
                                        .Select(crop => Convert.ToDecimal(crop.cropRequirementP2O5)).FirstOrDefault();
                        crop.reqK2o = tvm.LeafTestDetailsItems.Where(item => item.Id == crop.id)
                                        .Select(crop => Convert.ToDecimal(crop.cropRequirementK2O)).FirstOrDefault();
                        crop.remP2o5 = tvm.LeafTestDetailsItems.Where(item => item.Id == crop.id)
                                        .Select(crop => Convert.ToDecimal(crop.cropRemovalP2O5)).FirstOrDefault();
                        crop.remK2o = tvm.LeafTestDetailsItems.Where(item => item.Id == crop.id)
                                        .Select(crop => Convert.ToDecimal(crop.cropRemovalK2O5)).FirstOrDefault();
                        _ud.UpdateFieldCrop(tvm.fieldName, crop);
                    }
                    string target = "#test";
                    string url = Url.Action("RefreshTestList", "Soil");
                    return Json(new { success = true, url = url, target = target });
                }
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
                UpdateCropRequirementsBerries(fld);

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
                UpdateCropRequirementsBerries(fld);

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

            if (_isBerry)
            {
                msg += "<ul class='text-danger'>";

                if (TestType != null && TestType.Contains("SoilTest"))
                {
                    msg += _sd.GetSoilTestWarningBlueberries();
                }
                if (TestType != null && TestType.Contains("LeafTest"))
                {
                    msg += _sd.GetLeafTestWarningBlueberries();
                }
                msg += "</ul>";
            }
            else
            {
                if (TestType == null || TestType.Contains("SoilTest"))
                {
                    msg += _sd.GetSoilTestWarning();
                }
            }

            mvm.msg = msg;

            return View(mvm);
        }

        [HttpPost]
        public IActionResult MissingTests(MissingTestsViewModel mvm)
        {
            return View(mvm);
        }

        void UpdateCropRequirementsBerries(Field fld)
        {
            var crr = new CropRequirementRemoval();
            foreach (FieldCrop crop in fld.Crops)
            {
                string cropName = _sd.GetCrop(Convert.ToInt32(crop.cropId)).CropName;

                if (cropName == "Blueberry")
                {

                    decimal leafTissueP = (fld.LeafTest == null || (fld.LeafTest != null && fld.LeafTest.leafTissueP == 0 )) ?
                                                             _calculateCropRequirementRemoval.defaultBlueberryLeafTestP :
                                                             fld.LeafTest.leafTissueP;
                    decimal leafTissueK = (fld.LeafTest == null || (fld.LeafTest != null && fld.LeafTest.leafTissueK == 0 )) ?
                                                             _calculateCropRequirementRemoval.defaultBlueberryLeafTestP :
                                                             fld.LeafTest.leafTissueK;
                    decimal soilTestValP = (fld.SoilTest == null || (fld.SoilTest != null && fld.SoilTest.ValP == 0)) ?
                                                             _calculateCropRequirementRemoval.defaultBlueberrySoilTestP :
                                                             fld.SoilTest.ValP;

                    crr = _calculateCropRequirementRemoval.GetCropRequirementRemovalBlueberries(
                                                                            crop.yieldByHarvestUnit,
                                                                            crop.plantAgeYears,
                                                                            crop.numberOfPlantsPerAcre,
                                                                            crop.willSawdustBeApplied,
                                                                            crop.willPlantsBePruned,
                                                                            crop.whereWillPruningsGo,
                                                                            soilTestValP,
                                                                            leafTissueP,
                                                                            leafTissueK);
                }
                else if (cropName == "Raspberry")
                {

                    var soilTestValP = (fld.SoilTest == null || (fld.SoilTest != null && fld.SoilTest.ValP == 0)) ?
                                                             _calculateCropRequirementRemoval.defaultRaspberrySoilTestP :
                                                             fld.SoilTest.ValP;
                    var soilTestValK = (fld.SoilTest == null || (fld.SoilTest != null && fld.SoilTest.valK == 0)) ?
                                                             _calculateCropRequirementRemoval.defaultRaspberrySoilTestK :
                                                             fld.SoilTest.valK;
                    var leafTissueP = (fld.LeafTest == null || (fld.LeafTest != null && fld.LeafTest.leafTissueP == 0)) ?
                                                             _calculateCropRequirementRemoval.defaultRaspberryLeafTestP :
                                                             fld.LeafTest.leafTissueP;
                    var leafTissueK = (fld.LeafTest == null || (fld.LeafTest != null && fld.LeafTest.leafTissueK == 0 )) ?
                                                             _calculateCropRequirementRemoval.defaultRaspberryLeafTestK :
                                                             fld.LeafTest.leafTissueK;

                    crr = _calculateCropRequirementRemoval.GetCropRequirementRemovalRaspberries(
                                                                           crop.yieldByHarvestUnit,
                                                                           crop.willSawdustBeApplied,
                                                                           crop.willPlantsBePruned,
                                                                           crop.whereWillPruningsGo,
                                                                           soilTestValP,
                                                                           soilTestValK,
                                                                           leafTissueP,
                                                                           leafTissueK);
                }

                crop.reqN = crr.N_Requirement;
                crop.reqP2o5 = crr.P2O5_Requirement;
                crop.reqK2o = crr.K2O_Requirement;
                crop.remP2o5 = crr.P2O5_Removal;
                crop.remK2o = crr.K2O_Removal;
                _ud.UpdateFieldCrop(fld.FieldName, crop);

            }
        }
    }
}