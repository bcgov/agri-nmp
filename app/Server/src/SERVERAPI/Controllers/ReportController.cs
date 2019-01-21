﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.ViewModels;
using SERVERAPI.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using SERVERAPI.Utility;
using System.IO;
using Agri.Interfaces;
using Agri.Models.Calculate;
using Agri.Models.Farm;
using Agri.Models.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SERVERAPI.Models.Impl;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.NodeServices;
using Agri.LegacyData.Models.Impl;
using Agri.Models;
using Agri.Models.Configuration;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Fertilizer = Agri.Models.Configuration.Fertilizer;
using FertilizerType = Agri.Models.Configuration.FertilizerType;
using FertilizerUnit = Agri.Models.Configuration.FertilizerUnit;
using Manure = Agri.Models.Configuration.Manure;
using Region = Agri.Models.Configuration.Region;
using Unit = Agri.Models.Configuration.Unit;

namespace SERVERAPI.Controllers
{
    public class JSONResponse
    {
        public string type;
        public byte[] data;
    }
    public class PDFRequest
    {
        public string html;
        public string options;
    }

    //[RedirectingAction]
    public class ReportController : Controller
    {
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public IAgriConfigurationRepository _sd { get; set; }
        public IViewRenderService _viewRenderService { get; set; }
        public AppSettings _settings;
        private IManureApplicationCalculator _manureApplicationCalculator;

        public ReportController(IHostingEnvironment env, IViewRenderService viewRenderService, UserData ud, IAgriConfigurationRepository sd,IManureApplicationCalculator manureApplicationCalculator)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
            _viewRenderService = viewRenderService;
            _manureApplicationCalculator = manureApplicationCalculator;
        }
        [HttpGet]
        public IActionResult Report()
        {
            bool cropFound = true;

            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            ReportViewModel rvm = new ReportViewModel();
            rvm.unsavedData = farmData.unsaved;
            rvm.url = _sd.GetExternalLink("finishpagetarget");

            List<Field> fldLst = _ud.GetFields();

            if (fldLst.Count() == 0)
            {
                cropFound = false;
            }
            else
            {
                foreach (var f in fldLst)
                {
                    if(f.crops == null)
                    {
                        cropFound = false;
                        break;
                    }
                }
            }

            if(!cropFound)
            {
                rvm.noCropsMsg = _sd.GetUserPrompt("nocropmessage");
            }

            rvm.GeneratedManures = _ud.GetGeneratedManures();
            rvm.ImportedManures = _ud.GetImportedManures();

            if (rvm.UnallocatedManures.Count > 0)
            {
                rvm.materialsNotStoredMessage = _sd.GetUserPrompt("materialsNotStoredMessage");
            }

            if (fldLst.Count() > 0)
            {
                // materials remaining
                var yearData = _ud.GetYearData();
                rvm.RemainingManures = new List<AppliedManure>();
                rvm.OverUtilizedManures = new List<AppliedManure>();
                if (yearData.ManureStorageSystems != null)
                {
                    foreach (var storageSystem in yearData.ManureStorageSystems)
                    {
                        var manureStorageSystem = yearData.ManureStorageSystems.SingleOrDefault(mss => mss.Id == storageSystem.Id);
                        var appliedStoredManure = _manureApplicationCalculator.GetAppliedManureFromStorageSystem(yearData, manureStorageSystem);
                        if (appliedStoredManure.WholePercentRemaining >= 10)
                        {
                            rvm.RemainingManures.Add(appliedStoredManure);
                        }
                        else if (appliedStoredManure.WholePercentRemaining == 0 && appliedStoredManure.TotalAnnualManureRemainingToApply < 0 && 
                                 ((appliedStoredManure.TotalAnnualManureRemainingToApply / appliedStoredManure.TotalAnnualManureToApply) * 100 <= -10))
                        {
                            rvm.OverUtilizedManures.Add(appliedStoredManure);
                        }
                    }
                }

                if (yearData.ImportedManures != null)
                {
                    foreach (var importedManure in yearData.ImportedManures)
                    {
                        if (!importedManure.IsMaterialStored)
                        {
                            var farmManure = _ud.GetFarmManureByImportedManure(importedManure);
                            var appliedImportedManure = _manureApplicationCalculator.GetAppliedImportedManure(yearData, farmManure);
                            if (appliedImportedManure.WholePercentRemaining >= 10)
                            {
                                rvm.RemainingManures.Add(appliedImportedManure);
                            }
                            else if (appliedImportedManure.WholePercentRemaining == 0 && appliedImportedManure.TotalAnnualManureRemainingToApply < 0 && ((appliedImportedManure.TotalAnnualManureRemainingToApply / appliedImportedManure.TotalAnnualManureToApply) * 100 <= -10))
                            {
                                rvm.OverUtilizedManures.Add(appliedImportedManure);
                            }
                        }
                    }
                }
                
                if (rvm.RemainingManures.Count() > 0)
                {
                    rvm.MaterialsRemainingMessage = _sd.GetUserPrompt("remainingMaterialsMessage");
                }

                if (rvm.OverUtilizedManures.Count() > 0)
                {
                    rvm.OverUtilizedManuresMessage = _sd.GetUserPrompt("overUtilizedMaterialsMessage");
                }
            }
            

            rvm.downloadMsg = string.Format(_sd.GetUserPrompt("reportdownload"), Url.Action("DownloadMessage", "Home"));
            rvm.loadMsg = _sd.GetUserPrompt("reportload");

            return View(rvm);
        }
        [HttpPost]
        public IActionResult Report(ReportViewModel rvm)
        {

            return View(rvm);
        }
        public async Task<string> RenderHeader()
        {
            ReportViewModel rvm = new ReportViewModel();

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportHeader.cshtml", rvm);

            return result;
        }
        public async Task<string> RenderFields()
        {
            Utility.CalculateNutrients calculateNutrients = new CalculateNutrients(_ud, _sd);
            NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();
            Utility.SoilTestConversions stc = new SoilTestConversions(_ud, _sd);
            CalculateCropRequirementRemoval calculateCropRequirementRemoval = new CalculateCropRequirementRemoval(_ud, _sd);

            ReportFieldsViewModel rvm = new ReportFieldsViewModel();
            rvm.fields = new List<ReportFieldsField>();
            rvm.year = _ud.FarmDetails().year;
            rvm.methodName = string.IsNullOrEmpty(_ud.FarmDetails().testingMethod) ? "not selected" : _sd.GetSoilTestMethod(_ud.FarmDetails().testingMethod);
            rvm.prevHdg = _sd.GetUserPrompt("ncreditlabel");

            List<Field> fldList = _ud.GetFields();
            foreach (var f in fldList)
            {
                ReportFieldsField rf = new ReportFieldsField();
                rf.fieldArea = f.area.ToString("G29");
                rf.fieldName = f.fieldName;
                rf.fieldComment = f.comment;
                rf.soiltest = new ReportFieldSoilTest();
                rf.crops = new List<ReportFieldCrop>();
                rf.otherNutrients = new List<ReportFieldOtherNutrient>();
                rf.footnotes = new List<ReportFieldFootnote>();
                rf.showNitrogenCredit = false;
                rf.showSoilTestNitrogenCredit = false;

                if (f.soilTest != null)
                {
                    rf.soiltest.sampleDate = f.soilTest.sampleDate.ToString("MMM yyyy");
                    rf.soiltest.dispNO3H = f.soilTest.valNO3H.ToString("G29") + " ppm";
                    rf.soiltest.dispP = f.soilTest.ValP.ToString("G29") + " ppm (" + _sd.GetPhosphorusSoilTestRating(stc.GetConvertedSTP(f.soilTest)) + ")";
                    rf.soiltest.dispK = f.soilTest.valK.ToString("G29") + " ppm (" + _sd.GetPotassiumSoilTestRating(stc.GetConvertedSTK(f.soilTest)) + ")";
                    rf.soiltest.dispPH = f.soilTest.valPH.ToString("G29");
                }

                rf.nutrients = new List<ReportFieldNutrient>();
                if (f.crops != null)
                {
                    foreach (var c in f.crops)
                    {
                        ReportFieldCrop fc = new ReportFieldCrop();

                        fc.cropname = string.IsNullOrEmpty(c.cropOther) ? _sd.GetCrop(Convert.ToInt32(c.cropId)).CropName : c.cropOther;
                        if (c.coverCropHarvested.HasValue)
                        {
                            fc.cropname = c.coverCropHarvested.Value ? fc.cropname + "(harvested)" : fc.cropname;
                        }
                        if (c.prevCropId > 0)
                            fc.previousCrop = _sd.GetPrevCropType(c.prevCropId).Name;

                        if (_sd.GetCropType(_sd.GetCrop(Convert.ToInt32(c.cropId)).CropTypeId).CrudeProteinRequired)
                        {
                            if (c.crudeProtien.Value.ToString("#.#") != calculateCropRequirementRemoval.GetCrudeProtienByCropId(Convert.ToInt32(c.cropId)).ToString("#.#"))
                            {
                                ReportFieldFootnote rff = new ReportFieldFootnote();
                                rff.id = rf.footnotes.Count() + 1;
                                rff.message = "Crude protein adjusted to " + c.crudeProtien.Value.ToString("#.#") + "%";
                                fc.footnote = rff.id.ToString();
                                rf.footnotes.Add(rff);
                            }
                        }

                        if (_sd.GetCropType(_sd.GetCrop(Convert.ToInt32(c.cropId)).CropTypeId).ModifyNitrogen)
                        {
                            // check for standard
                            CropRequirementRemoval cropRequirementRemoval = new CropRequirementRemoval();

                            calculateCropRequirementRemoval.cropid = Convert.ToInt16(c.cropId);
                            calculateCropRequirementRemoval.yield = Convert.ToDecimal(c.yield);
                            if (c.crudeProtien == (decimal?)null)
                                calculateCropRequirementRemoval.crudeProtien = null;
                            else
                                calculateCropRequirementRemoval.crudeProtien = Convert.ToDecimal(c.crudeProtien);
                            calculateCropRequirementRemoval.coverCropHarvested = c.coverCropHarvested;
                            calculateCropRequirementRemoval.fieldName = f.fieldName;
                            string nCredit = c.prevCropId != 0 ? _sd.GetPrevCropType(Convert.ToInt32(c.prevCropId)).NitrogenCreditImperial.ToString() : "0";

                            if (!string.IsNullOrEmpty(nCredit))
                                calculateCropRequirementRemoval.nCredit = Convert.ToInt16(nCredit);

                            cropRequirementRemoval = calculateCropRequirementRemoval.GetCropRequirementRemoval();

                            string stdNAmt = cropRequirementRemoval.N_Requirement.ToString();

                            if (c.reqN.ToString("G29") != cropRequirementRemoval.N_Requirement.ToString())
                            {
                                ReportFieldFootnote rff = new ReportFieldFootnote();
                                rff.id = rf.footnotes.Count() + 1;
                                rff.message = "Crop required nitrogen adjusted to " + c.reqN.ToString("G29");
                                fc.footnote = rff.id.ToString();
                                rf.footnotes.Add(rff);
                            }
                        }
                        //E07US18
                        if (c.yieldHarvestUnit.HasValue)
                        {
                            fc.yield = c.yieldByHarvestUnit;
                            fc.yieldInUnit = _sd.GetHarvestYieldUnitName(c.yieldHarvestUnit.ToString());
                        }
                        else
                        {
                            fc.yield = c.yield;  // retrofit old versio data (E07US18)
                            fc.yieldInUnit = _sd.GetHarvestYieldDefaultUnitName(); 
                        }
                        
                        fc.reqN = -Convert.ToDecimal((c.reqN).ToString("G29"));
                        fc.reqP = -Convert.ToDecimal((c.reqP2o5).ToString("G29"));
                        fc.reqK = -Convert.ToDecimal((c.reqK2o).ToString("G29"));
                        fc.remN = -Convert.ToDecimal((c.remN).ToString("G29"));
                        fc.remP = -Convert.ToDecimal((c.remP2o5).ToString("G29"));
                        fc.remK = -Convert.ToDecimal((c.remK2o).ToString("G29"));

                        rf.reqN = rf.reqN + fc.reqN;
                        rf.reqP = rf.reqP + fc.reqP;
                        rf.reqK = rf.reqK + fc.reqK;
                        rf.remN = rf.remN + fc.remN;
                        rf.remP = rf.remP + fc.remP;
                        rf.remK = rf.remK + fc.remK;

                        rf.fieldCrops = rf.fieldCrops + fc.cropname + " ";

                        rf.crops.Add(fc);
                    }
                    if (f.crops.Count() > 0)
                    {
                        rf.showNitrogenCredit = f.prevYearManureApplicationFrequency != null ? true : false;
                        if ( rf.showNitrogenCredit )
                        {
                            if (f.prevYearManureApplicationNitrogenCredit == null)
                            {   // calculate default value.
                                SERVERAPI.Utility.ChemicalBalanceMessage calculator = new Utility.ChemicalBalanceMessage(_ud, _sd);
                                rf.nitrogenCredit = calculator.calcPrevYearManureApplDefault(f.fieldName);
                            }
                            else
                                rf.nitrogenCredit = f.prevYearManureApplicationNitrogenCredit;
                            rf.reqN += Convert.ToDecimal(rf.nitrogenCredit);
                        }
                        if (f.soilTest != null)
                        {
                            rf.showSoilTestNitrogenCredit = _sd.IsNitrateCreditApplicable(_ud.FarmDetails().farmRegion, f.soilTest.sampleDate, Convert.ToInt16(_ud.FarmDetails().year));
                            if (rf.showSoilTestNitrogenCredit)
                            {
                                if (f.SoilTestNitrateOverrideNitrogenCredit == null)
                                {   // calculate default value
                                    SERVERAPI.Utility.ChemicalBalanceMessage calculator = new Utility.ChemicalBalanceMessage(_ud, _sd);
                                    rf.soilTestNitrogenCredit = Math.Round(calculator.calcSoitTestNitrateDefault(f.fieldName));
                                }
                                else
                                    rf.soilTestNitrogenCredit = Math.Round(Convert.ToDecimal(f.SoilTestNitrateOverrideNitrogenCredit));
                                
                                rf.reqN += rf.soilTestNitrogenCredit;
                            }
                        }
                    } // f.crops.Count() > 0
                }
                if (f.nutrients != null)
                {
                    if (f.nutrients.nutrientManures != null)
                    {
                        foreach (var m in f.nutrients.nutrientManures)
                        {
                            FarmManure manure = _ud.GetFarmManure(Convert.ToInt32(m.manureId));
                            ReportFieldNutrient rfn = new ReportFieldNutrient();

                            rfn.nutrientName = manure.name;
                            rfn.nutrientAmount = String.Format((m.rate) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", (m.rate));
                            rfn.nutrientSeason = _sd.GetApplication(m.applicationId.ToString()).Season;
                            rfn.nutrientApplication = _sd.GetApplication(m.applicationId.ToString()).ApplicationMethod;
                            rfn.nutrientUnit = _sd.GetUnit(m.unitId).Name;
                            rfn.reqN = Convert.ToDecimal((m.yrN).ToString("G29"));
                            rfn.reqP = Convert.ToDecimal((m.yrP2o5).ToString("G29"));
                            rfn.reqK = Convert.ToDecimal((m.yrK2o).ToString("G29"));
                            rfn.remN = Convert.ToDecimal((m.ltN).ToString("G29"));
                            rfn.remP = Convert.ToDecimal((m.ltP2o5).ToString("G29"));
                            rfn.remK = Convert.ToDecimal((m.ltK2o).ToString("G29"));
                            rf.nutrients.Add(rfn);

                            rf.reqN = rf.reqN + rfn.reqN;
                            rf.reqP = rf.reqP + rfn.reqP;
                            rf.reqK = rf.reqK + rfn.reqK;
                            rf.remN = rf.remN + rfn.remN;
                            rf.remP = rf.remP + rfn.remP;
                            rf.remK = rf.remK + rfn.remK;

                            int regionid = _ud.FarmDetails().farmRegion.Value;
                            Region region = _sd.GetRegion(regionid);
                            nOrganicMineralizations = calculateNutrients.GetNMineralization(Convert.ToInt32(m.manureId), region.LocationId);

                            string footNote = "";

                            if(m.nAvail != nOrganicMineralizations.OrganicN_FirstYear * 100)
                            {
                                footNote = "1st Yr Organic N Availability adjusted to " + m.nAvail.ToString("###") + "%";
                            }
                            if(m.nh4Retention != (calculateNutrients.GetAmmoniaRetention(Convert.ToInt16(m.manureId), Convert.ToInt16(m.applicationId)) * 100))
                            {
                                footNote = string.IsNullOrEmpty(footNote) ? "" : footNote + ", ";
                                footNote = footNote + "Ammonium-N Retention adjusted to " + m.nh4Retention.ToString("###") + "%";
                            }
                            if(!string.IsNullOrEmpty(footNote))
                            {
                                ReportFieldFootnote rff = new ReportFieldFootnote();
                                rff.id = rf.footnotes.Count() + 1;
                                rff.message = footNote;
                                rfn.footnote = rff.id.ToString();
                                rf.footnotes.Add(rff);
                            }
                        }
                    }
                    if (f.nutrients.nutrientFertilizers != null)
                    {
                        foreach (var ft in f.nutrients.nutrientFertilizers)
                        {
                            string fertilizerName = string.Empty;
                            ReportFieldNutrient rfn = new ReportFieldNutrient();
                            FertilizerType ftyp = _sd.GetFertilizerType(ft.fertilizerTypeId.ToString());

                            if (ftyp.Custom)
                            {
                                fertilizerName = ftyp.DryLiquid == "dry" ? "Custom (Dry) " : "Custom (Liquid) ";
                                fertilizerName = fertilizerName + ft.customN.ToString() + "-" + ft.customP2o5.ToString() + "-" + ft.customK2o.ToString();
                                rfn.reqN = Convert.ToDecimal((ft.fertN).ToString("G29"));
                                rfn.reqP = Convert.ToDecimal((ft.fertP2o5).ToString("G29"));
                                rfn.reqK = Convert.ToDecimal((ft.fertK2o).ToString("G29"));
                                rfn.remN = Convert.ToDecimal((ft.fertN).ToString("G29"));
                                rfn.remP = Convert.ToDecimal((ft.fertP2o5).ToString("G29"));
                                rfn.remK = Convert.ToDecimal((ft.fertK2o).ToString("G29"));
                            }
                            else
                            {
                                Fertilizer ff = _sd.GetFertilizer(ft.fertilizerId.ToString());
                                fertilizerName = ff.Name;
                                rfn.reqN = Convert.ToDecimal((ft.fertN).ToString("G29"));
                                rfn.reqP = Convert.ToDecimal((ft.fertP2o5).ToString("G29"));
                                rfn.reqK = Convert.ToDecimal((ft.fertK2o).ToString("G29"));
                                rfn.remN = Convert.ToDecimal((ft.fertN).ToString("G29"));
                                rfn.remP = Convert.ToDecimal((ft.fertP2o5).ToString("G29"));
                                rfn.remK = Convert.ToDecimal((ft.fertK2o).ToString("G29"));
                            }

                            rfn.nutrientName = fertilizerName;
                            rfn.nutrientApplication = ft.applMethodId > 0 ? _sd.GetFertilizerMethod(ft.applMethodId.ToString()).Name : "";
                            rfn.nutrientUnit = _sd.GetFertilizerUnit(ft.applUnitId).Name;

                            rfn.nutrientAmount = String.Format((ft.applRate) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", (ft.applRate));
                            rf.nutrients.Add(rfn);

                            rf.reqN = rf.reqN + rfn.reqN;
                            rf.reqP = rf.reqP + rfn.reqP;
                            rf.reqK = rf.reqK + rfn.reqK;
                            rf.remN = rf.remN + rfn.remN;
                            rf.remP = rf.remP + rfn.remP;
                            rf.remK = rf.remK + rfn.remK;

                            string footNote = "";

                            if (ftyp.DryLiquid == "liquid")
                            {
                                if (!ftyp.Custom)
                                {
                                    if (ft.liquidDensity.ToString("#.##") != _sd.GetLiquidFertilizerDensity(ft.fertilizerId, ft.liquidDensityUnitId).Value.ToString("#.##"))
                                    {
                                        footNote = "Liquid density adjusted to " + ft.liquidDensity.ToString("#.##");
                                    }
                                    if (!string.IsNullOrEmpty(footNote))
                                    {
                                        ReportFieldFootnote rff = new ReportFieldFootnote();
                                        rff.id = rf.footnotes.Count() + 1;
                                        rff.message = footNote;
                                        rfn.footnote = rff.id.ToString();
                                        rf.footnotes.Add(rff);
                                    }
                                }
                            }
                        }
                    }
                    if (f.nutrients.nutrientOthers != null)
                    {
                        foreach(var o in f.nutrients.nutrientOthers)
                        {
                            ReportFieldOtherNutrient fon = new ReportFieldOtherNutrient();
                            fon.otherName = o.description;
                            fon.reqN = Convert.ToDecimal((o.ltN).ToString("G29")); 
                            fon.reqP = Convert.ToDecimal((o.ltP2o5).ToString("G29"));
                            fon.reqK = Convert.ToDecimal((o.ltK).ToString("G29"));
                            fon.remN = Convert.ToDecimal((o.yrN).ToString("G29"));
                            fon.remP = Convert.ToDecimal((o.yrP2o5).ToString("G29"));
                            fon.remK = Convert.ToDecimal((o.yrK).ToString("G29"));
                            rf.otherNutrients.Add(fon);

                            rf.reqN = rf.reqN + fon.reqN;
                            rf.reqP = rf.reqP + fon.reqP;
                            rf.reqK = rf.reqK + fon.reqK;
                            rf.remN = rf.remN + fon.remN;
                            rf.remP = rf.remP + fon.remP;
                            rf.remK = rf.remK + fon.remK;
                        }
                    }
                }

                if (rf.nutrients.Count() == 0)
                {
                    ReportFieldNutrient rfn = new ReportFieldNutrient();
                    rfn.nutrientName = "None planned";
                    rfn.nutrientAmount = "";
                    rf.nutrients.Add(rfn);
                }
                ChemicalBalanceMessage cbm = new ChemicalBalanceMessage(_ud, _sd);

                var request = HttpContext.Request;
                string scheme = request.Scheme;
                string host = request.Host.ToString();
                string imgLoc = scheme + "://" + host + "/images/{0}.svg"; 

                rf.alertMsgs = cbm.DetermineBalanceMessages(f.fieldName);

                if (rf.alertMsgs.FirstOrDefault(r => r.Chemical == "AgrN") != null)
                {
                    rf.alertN = true;
                    rf.iconAgriN = string.Format(imgLoc, rf.alertMsgs.FirstOrDefault(r => r.Chemical == "AgrN").Icon);
                }
                if (rf.alertMsgs.FirstOrDefault(r => r.Chemical == "AgrP2O5") != null)
                {
                    rf.alertP = true;
                    rf.iconAgriP = string.Format(imgLoc, rf.alertMsgs.FirstOrDefault(r => r.Chemical == "AgrP2O5").Icon);
                }
                if (rf.alertMsgs.FirstOrDefault(r => r.Chemical == "AgrK2O") != null)
                {
                    rf.alertK = true;
                    rf.iconAgriK = string.Format(imgLoc, rf.alertMsgs.FirstOrDefault(r => r.Chemical == "AgrK2O").Icon);
                }
                if (rf.alertMsgs.FirstOrDefault(r => r.Chemical == "CropN") != null)
                {
                    rf.alertN = true;
                    rf.iconCropN = string.Format(imgLoc, rf.alertMsgs.FirstOrDefault(r => r.Chemical == "CropN").Icon);
                }
                if (rf.alertMsgs.FirstOrDefault(r => r.Chemical == "CropP2O5") != null)
                {
                    rf.alertP = true;
                    rf.iconCropP = string.Format(imgLoc, rf.alertMsgs.FirstOrDefault(r => r.Chemical == "CropP2O5").Icon);
                }
                if (rf.alertMsgs.FirstOrDefault(r => r.Chemical == "CropK2O") != null)
                {
                    rf.alertK = true;
                    rf.iconCropK = string.Format(imgLoc, rf.alertMsgs.FirstOrDefault(r => r.Chemical == "CropK2O").Icon);
                }

                //replace icon type with actual icon url for screen processing
                foreach(var i in rf.alertMsgs)
                {
                    i.Icon = string.Format(imgLoc, i.Icon);
                }

                rvm.fields.Add(rf);
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportFields.cshtml", rvm);

            return result;
        }

        public async Task<string> RenderManureCompostInventory()
        {
            ReportManureCompostViewModel rmcvm = new ReportManureCompostViewModel();
            CalculateAnimalRequirement calculateAnimalRequirement = new CalculateAnimalRequirement(_ud, _sd);

            rmcvm.storages = new List<ReportStorage>();
            rmcvm.unstoredManures = new List<ReportManuress>();
            rmcvm.year = _ud.FarmDetails().year;
            decimal rainInMM = 1000;
            decimal conversionForLiquid = 0.024542388m;
            decimal conversionForSolid = 0.000102408m;

            var yd = _ud.GetYearData();
            if (yd.GeneratedManures != null)
            {
                foreach (var g in yd.GeneratedManures)
                {
                    if (g.AssignedToStoredSystem == false)
                    {
                        ReportManuress rm = new ReportManuress();
                        rm.animalManure = g.animalSubTypeName + "," +
                                          g.averageAnimalNumber + " animals";
                        rm.annualAmount =
                            string.Format("{0:#,##0}", g.annualAmount.Split(' ')[0]);
                        if (g.ManureType == ManureMaterialType.Liquid)
                        {
                            rm.units = "US Gallons";
                        }
                        else if (g.ManureType == ManureMaterialType.Solid)
                        {
                            rm.units = "tons";
                        }


                        if (g.washWaterGallonsToString != "0")
                        {
                            rm.milkingCenterWashWater = g.washWaterGallonsToString;
                        }

                        rmcvm.unstoredManures.Add(rm);

                    }
                }
            }

            if (yd.ImportedManures != null)
            {
                foreach (var i in yd.ImportedManures)
                {
                    if (i.AssignedToStoredSystem == false)
                    {
                        ReportManuress rm = new ReportManuress();
                        rm.animalManure = i.MaterialName;
                        if (i.ManureType == ManureMaterialType.Liquid)
                        {
                            rm.annualAmount = string.Format("{0:#,##0}", (Math.Round(i.AnnualAmountUSGallonsVolume)))
                                .ToString();
                            rm.units = "US Gallons";
                        }
                        else if (i.ManureType == ManureMaterialType.Solid)
                        {
                            rm.annualAmount = string.Format("{0:#,##0}", (Math.Round(i.AnnualAmountTonsWeight)))
                                .ToString();
                            rm.units = "tons";
                        }

                        rmcvm.unstoredManures.Add(rm);
                    }
                }
            }

            List<ManureStorageSystem> storageList = _ud.GetStorageSystems();
            foreach (var s in storageList)
            {
                ReportStorage rs = new ReportStorage();
                rs.reportManures = new List<ReportManuress>();
                int? runoffAreaSquareFeet = 0;
                int? areaOfUncoveredLiquidStorage = 0;
                decimal washWaterAdjustedValue = 0;
                decimal annualAmountOfManurePerStorage = 0;

                rs.storageSystemName = s.Name;
                rs.ManureMaterialType = s.ManureMaterialType;
                rs.footnotes = new List<ReportFieldFootnote>();
                if (s.GetsRunoffFromRoofsOrYards)
                {
                    runoffAreaSquareFeet = s.RunoffAreaSquareFeet;
                }

                foreach (var ss in s.ManureStorageStructures)
                {
                    if (!ss.IsStructureCovered)
                        areaOfUncoveredLiquidStorage = ss.UncoveredAreaSquareFeet;
                }

                if (s.ManureMaterialType == ManureMaterialType.Liquid)
                {
                    rs.precipitation = string.Format("{0:#,##0}",
                        ((Convert.ToDecimal(runoffAreaSquareFeet) + Convert.ToDecimal(areaOfUncoveredLiquidStorage)) *
                         rainInMM * conversionForLiquid));
                    rs.units = "US gallons";
                }
                else if (s.ManureMaterialType == ManureMaterialType.Solid)
                {
                    rs.precipitation = string.Format("{0:#,##0}",
                        ((Convert.ToDecimal(runoffAreaSquareFeet) + Convert.ToDecimal(areaOfUncoveredLiquidStorage)) *
                         rainInMM * conversionForSolid));
                    rs.units = "tons";
                }

                if (s.MaterialsIncludedInSystem != null)
                {
                    foreach (var m in s.MaterialsIncludedInSystem)
                    {
                        if (m.ManureId.Contains("Generated"))
                        {
                            var generatedFarmManure = _ud.GetGeneratedManure(m.Id);
                            ReportManuress rm = new ReportManuress();
                            rm.animalManure = generatedFarmManure.animalSubTypeName + "," +
                                              generatedFarmManure.averageAnimalNumber + " animals";
                            rm.annualAmount =
                                string.Format("{0:#,##0}", generatedFarmManure.annualAmount.Split(' ')[0]);
                            if (s.ManureMaterialType == ManureMaterialType.Liquid)
                            {
                                rm.units = "US Gallons";
                            }
                            else if (s.ManureMaterialType == ManureMaterialType.Solid)
                            {
                                rm.units = "tons";
                            }
                            if (s.ManureMaterialType != generatedFarmManure.ManureType &&
                                generatedFarmManure.ManureType == ManureMaterialType.Solid)
                            {
                                // if solid material is added to the liquid system change the calculations to depict that of liquid
                                AnimalSubType animalSubType =
                                    _sd.GetAnimalSubType(Convert.ToInt32(generatedFarmManure.animalSubTypeId));
                                if (animalSubType.SolidPerGalPerAnimalPerDay.HasValue)
                                {
                                    rm.annualAmount =
                                        (Math.Round(Convert.ToInt32(generatedFarmManure.averageAnimalNumber) *
                                                    Convert.ToDecimal(animalSubType.SolidPerGalPerAnimalPerDay) * 365))
                                        .ToString();
                                    rm.units = "US gallons";
                                }
                            }

                            if (generatedFarmManure.washWaterGallonsToString != "0")
                            {
                                rs.milkingCenterWashWater = generatedFarmManure.washWaterGallonsToString;
                                washWaterAdjustedValue = generatedFarmManure.washWater;
                            }

                            if (generatedFarmManure.washWater.ToString("#.##") != calculateAnimalRequirement
                                    .GetWashWaterBySubTypeId(generatedFarmManure.animalSubTypeId).Value
                                    .ToString("#.##"))
                            {
                                ReportFieldFootnote rff = new ReportFieldFootnote();
                                rff.id = rs.footnotes.Count() + 1;
                                rff.message = "Milking Center Wash Water adjusted to " +
                                              washWaterAdjustedValue.ToString("G29") + " US gallons/day/animal";
                                rs.footnote = rff.id.ToString();
                                rs.footnotes.Add(rff);
                            }
                            rs.reportManures.Add(rm);
                        }
                        else if (m.ManureId.Contains("Imported"))
                        {
                            var importedFarmManure = _ud.GetImportedManureByManureId(m.ManureId);
                            if (importedFarmManure.AssignedToStoredSystem)
                            {
                                ReportManuress rm = new ReportManuress();
                                rm.animalManure = importedFarmManure.MaterialName;
                                if (rs.ManureMaterialType == ManureMaterialType.Liquid)
                                {
                                    rm.annualAmount = string.Format("{0:#,##0}", (Math.Round(importedFarmManure.AnnualAmountUSGallonsVolume))).ToString();
                                    rm.units = "US Gallons";
                                }
                                else if (rs.ManureMaterialType == ManureMaterialType.Solid)
                                {
                                    rm.annualAmount = string.Format("{0:#,##0}", (Math.Round(importedFarmManure.AnnualAmountTonsWeight))).ToString();
                                    rm.units = "tons";
                                }
                                rs.reportManures.Add(rm);
                            }
                        }
                    }
                }

                rmcvm.storages.Add(rs);
            }


            foreach (var s in rmcvm.storages)
            {
                decimal annualAmountOfManurePerStorage = 0;
                if (s.precipitation != null)
                {
                    annualAmountOfManurePerStorage = Convert.ToDecimal(s.precipitation);
                }

                if (s.milkingCenterWashWater != null)
                {
                    annualAmountOfManurePerStorage += Convert.ToDecimal(s.milkingCenterWashWater);
                }

                foreach (var m in s.reportManures)
                {
                    if (m.annualAmount != null)
                    {
                        annualAmountOfManurePerStorage += Convert.ToDecimal(m.annualAmount);
                    }
                }

                if (s.annualAmountOfManurePerStorage != "")
                {
                    s.annualAmountOfManurePerStorage = string.Format("{0:#,##0}", (Math.Round(annualAmountOfManurePerStorage))).ToString();
                }
            }
            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportManureCompostInventory.cshtml", rmcvm);

            return result;
        }

        public async Task<string> RenderManureUse()
        {
            ReportManureSummaryViewModel rmsvm = new ReportManureSummaryViewModel();
            rmsvm.manures = new List<ReportManures>();
            rmsvm.footnotes = new List<ReportFieldFootnote>();
            rmsvm.year = _ud.FarmDetails().year;

            var yearData = _ud.GetYearData();

            if (yearData.farmManures != null)
            {
                foreach (var fm in yearData.farmManures)
                {
                    ReportManures rm = new ReportManures();
                    AppliedManure appliedManure = _manureApplicationCalculator.GetAppliedManure(yearData, fm);

                    if (appliedManure != null)
                    {
                        

                        if (fm.stored_imported == NutrientAnalysisTypes.Stored)
                        {
                            rm.MaterialName = "Material in ";
                        }
                        else if (fm.stored_imported == NutrientAnalysisTypes.Imported)
                        {
                            rm.MaterialName = "";
                        }

                        rm.MaterialName += appliedManure.SourceName;

                        // Annual Amount

                        rm.AnnualAmount = string.Format("{0:#,##0}", (Math.Round(appliedManure.TotalAnnualManureToApply))).ToString();
                        if (appliedManure.ManureMaterialType == ManureMaterialType.Liquid)
                        {
                            rm.AnnualAmount += " US Gallons";
                        }
                        else if (appliedManure.ManureMaterialType == ManureMaterialType.Solid)
                        {
                            rm.AnnualAmount += " tons";
                        }

                        // Amount Land Applied
                        rm.LandApplied = string.Format("{0:#,##0}", (Math.Round(appliedManure.TotalApplied))).ToString();
                        if (appliedManure.ManureMaterialType == ManureMaterialType.Liquid)
                        {
                            rm.LandApplied += " US Gallons";
                        }
                        else if (appliedManure.ManureMaterialType == ManureMaterialType.Solid)
                        {
                            rm.LandApplied += " tons";
                        }
                        rm.LandApplied += " (" + appliedManure.WholePercentAppiled + "%)";

                        // Amount Remaining
                        if (appliedManure.WholePercentRemaining < 10)
                        {
                            rm.AmountRemaining = "Insignificant";

                            ReportFieldFootnote rff = new ReportFieldFootnote();
                            rff.id = rmsvm.footnotes.Count() + 1;
                            rff.message = "If the amount remaining is less than 10% of the annual amount, then the amount remaining is insignificant (i.e. within the margin of error of the calculations)";
                            rm.footnote = rff.id.ToString();
                            rmsvm.footnotes.Add(rff);
                        }
                        else
                        {
                            rm.AmountRemaining = string.Format("{0:#,##0}", (Math.Round(appliedManure.TotalAnnualManureRemainingToApply))) + " (" + appliedManure.WholePercentRemaining + "%)";
                        }

                        rmsvm.manures.Add(rm);
                    }
                }
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportManureSummary.cshtml", rmsvm);

            return result;
        }

        public async Task<string> RenderFerilizers()
        {
            ReportSourcesViewModel rvm = new ReportSourcesViewModel();
            List<ReportSourcesDetail> manureRequired = new List<ReportSourcesDetail>();
            List<ReportSourcesDetail> fertilizerRequired = new List<ReportSourcesDetail>();

            rvm.year = _ud.FarmDetails().year;
            rvm.details = new List<ReportSourcesDetail>();


            List<Field> fldList = _ud.GetFields();
            foreach (var f in fldList)
            {
                if (f.nutrients != null)
                {
                    if (f.nutrients.nutrientFertilizers != null)
                        fertilizerRequired = BuildFertilizerRequiredList(fertilizerRequired, f.nutrients.nutrientFertilizers, f.area);
                }
            }

            if (fertilizerRequired.Count > 0)
                rvm.details.AddRange(fertilizerRequired);

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportFertilizers.cshtml", rvm);

            return result;
        }

        public async Task<string> RenderSources()
        {
            ReportSourcesViewModel rvm = new ReportSourcesViewModel();
            List<ReportSourcesDetail> manureRequired = new List<ReportSourcesDetail>();
            List<ReportSourcesDetail> fertilizerRequired = new List<ReportSourcesDetail>();

            rvm.year = _ud.FarmDetails().year;
            rvm.details = new List<ReportSourcesDetail>();


            List<Field> fldList = _ud.GetFields();
            foreach(var f in fldList)
            {
                if (f.nutrients != null)
                {
                    if (f.nutrients.nutrientManures != null)
                        manureRequired = BuildManureRequiredList(manureRequired, f.nutrients.nutrientManures, f.area);
                    if (f.nutrients.nutrientFertilizers != null)
                        fertilizerRequired = BuildFertilizerRequiredList(fertilizerRequired, f.nutrients.nutrientFertilizers, f.area);
                }
            }
            if (manureRequired.Count > 0)
                rvm.details.AddRange(manureRequired);
            if (fertilizerRequired.Count > 0)
                rvm.details.AddRange(fertilizerRequired);

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportSources.cshtml", rvm);

            return result;
        }

        // standard units for reporting
        private decimal ConvertManureToStdRptUnits(FarmManure manure, decimal fieldSize, decimal applicationRate, string unitId)
        {
            decimal result = 0;
            Unit unit = _sd.GetUnit(unitId);

            if ( unit.FarmReqdNutrientsStdUnitsConversion > 0)
                result = unit.FarmReqdNutrientsStdUnitsAreaConversion * fieldSize * applicationRate * unit.FarmReqdNutrientsStdUnitsConversion;
            else
            {
                Manure man = _sd.GetManure(manure.manureId.ToString());
                result = unit.FarmReqdNutrientsStdUnitsAreaConversion * fieldSize * applicationRate * man.CubicYardConversion;
            }
            return result;
        }

        // standard units for reporting
        private decimal ConvertFertilizerToStdRptUnits(decimal fieldSize, decimal applicationRate, int unitId)
        {
            FertilizerUnit unit = _sd.GetFertilizerUnit(unitId);
            return (unit.FarmRequiredNutrientsStdUnitsAreaConversion * fieldSize * applicationRate * unit.FarmRequiredNutrientsStdUnitsConversion);
        }

        private List<ReportSourcesDetail> BuildManureRequiredList(List<ReportSourcesDetail> details, List<NutrientManure> nutrientManures, decimal fieldArea)
        {
            List<ReportSourcesDetail> result = new List<ReportSourcesDetail>();
            decimal nutrientAmount = 0;

            foreach (var m in nutrientManures)
            {
                FarmManure manure = _ud.GetFarmManure(Convert.ToInt32(m.manureId));
                nutrientAmount = ConvertManureToStdRptUnits(manure, fieldArea, m.rate, m.unitId);
                ReportSourcesDetail rd = details.FirstOrDefault(d => d.nutrientName == manure.name);
                if (rd != null)
                {
                    nutrientAmount += Convert.ToDecimal(rd.nutrientAmount);
                    rd.nutrientAmount = String.Format((nutrientAmount) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", nutrientAmount);
                }
                else
                {
                    rd = new ReportSourcesDetail();
                    rd.nutrientName = manure.name;
                    rd.nutrientUnit = _sd.GetManureRptStdUnit(manure.solid_liquid);
                    rd.nutrientAmount = String.Format((nutrientAmount) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", nutrientAmount );
                    details.Add(rd);
                }
            }
            return details;
        }

        private Fertilizer ConvertCustomFertilizerToStdFertilizer(NutrientFertilizer nf)
        {
            Fertilizer fert = new Fertilizer();
            FertilizerType ft = _sd.GetFertilizerType(nf.fertilizerTypeId);
            fert.Id = nf.id;
            if (_sd.IsFertilizerTypeDry(nf.fertilizerTypeId)) {
                fert.Name = "Custom (Dry) " + nf.customN.ToString() + "-" + nf.customP2o5 + "-" + nf.customK2o.ToString();
                fert.DryLiquid = ft.DryLiquid;
            }
            else {
                fert.Name = "Custom (Liquid) " + nf.customN.ToString() + "-" + nf.customP2o5 + "-" + nf.customK2o.ToString();
                fert.DryLiquid = ft.DryLiquid;
            }
            fert.Nitrogen = Convert.ToDecimal(nf.customN);
            fert.Phosphorous = Convert.ToDecimal( nf.customP2o5 );
            fert.Potassium = Convert.ToDecimal(nf.customK2o);

            return fert;
        }

        private List<ReportSourcesDetail> BuildFertilizerRequiredList(List<ReportSourcesDetail> details, List<NutrientFertilizer> nutrientFertilizers, decimal fieldArea)
        {
            decimal nutrientAmount = 0;
            List<ReportSourcesDetail> result = new List<ReportSourcesDetail>();
           Fertilizer fert;

            foreach (var m in nutrientFertilizers)
            {
                if (_sd.IsCustomFertilizer(m.fertilizerTypeId))
                    fert = ConvertCustomFertilizerToStdFertilizer(m);
                else
                    fert = _sd.GetFertilizer(m.fertilizerId.ToString());

                nutrientAmount = ConvertFertilizerToStdRptUnits(fieldArea, m.applRate, m.applUnitId);

                ReportSourcesDetail rd = details.FirstOrDefault(d => d.nutrientName == fert.Name);
                if (rd != null)
                {
                    nutrientAmount += Convert.ToDecimal(rd.nutrientAmount);
                    rd.nutrientAmount = String.Format((nutrientAmount) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", nutrientAmount);
                }
                else
                {
                    rd = new ReportSourcesDetail();
                    rd.nutrientName = fert.Name;
                    rd.nutrientUnit = _sd.GetFertilizerRptStdUnit(fert.DryLiquid);
                    rd.nutrientAmount = String.Format((nutrientAmount) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", nutrientAmount);
                    details.Add(rd);
                }
            }
            return details;

        }

        public async Task<string> RenderAnalysis()
        {
            ReportAnalysisViewModel rvm = new ReportAnalysisViewModel();
            rvm.nitratePresent = false;

            rvm.details = new List<ReportAnalysisDetail>();
            rvm.footnotes = new List<ReportFieldFootnote>();

            List<FarmManure> manures = _ud.GetFarmManures();

            foreach(var m in manures)
            {
                ReportAnalysisDetail rd = new ReportAnalysisDetail();

                rd.sourceOfMaterialName = m.sourceOfMaterialName;
                rd.manureName = m.name;
                rd.moisture = m.moisture.ToString();
                rd.ammonia = m.ammonia.ToString("#0");
                rd.nitrogen = m.nitrogen.ToString("#0.00");
                rd.phosphorous = m.phosphorous.ToString("#0.00");
                rd.potassium = m.potassium.ToString("#0.00");
                rd.nitrate = (m.nitrate.HasValue && m.nitrate >0) ? m.nitrate.Value.ToString("#0"): "n/a";
                rd.isAssignedToStorage = m.IsAssignedToStorage;

                if (m.nitrate.HasValue && m.nitrate > 0)
                {
                    rvm.nitratePresent = true;
                }

                if (m.IsAssignedToStorage == false)
                {
                    ReportFieldFootnote rff = new ReportFieldFootnote();
                    rff.id = rvm.footnotes.Count() + 1;
                    rff.message = rd.sourceOfMaterialName +" includes materials that have not been allocted to a storage";
                    rd.footnote = rff.id.ToString();
                    rvm.footnotes.Add(rff);
                }

                rvm.details.Add(rd);
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportAnalysis.cshtml", rvm);

            return result;
        }
        public async Task<string> RenderApplication()
        {
            string crpName = string.Empty;

            ReportApplicationViewModel rvm = new ReportApplicationViewModel();
            rvm.fields = new List<ReportApplicationField>();
            rvm.year = _ud.FarmDetails().year;

            List<Field> fldList = _ud.GetFields();
            foreach(var f in fldList)
            {
                ReportApplicationField rf = new ReportApplicationField();
                rf.fieldName = f.fieldName;
                rf.fieldArea = f.area.ToString("G29");
                rf.fieldComment = f.comment;
                rf.nutrients = new List<ReportFieldNutrient>();
                if (f.nutrients != null)
                {
                    if (f.nutrients.nutrientManures != null)
                    {
                        foreach (var m in f.nutrients.nutrientManures)
                        {
                            FarmManure manure = _ud.GetFarmManure(Convert.ToInt32(m.manureId));
                            ReportFieldNutrient rfn = new ReportFieldNutrient();

                            rfn.nutrientName = manure.name;
                            rfn.nutrientAmount = String.Format((m.rate) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", m.rate);
                            rfn.nutrientSeason = _sd.GetApplication(m.applicationId.ToString()).Season;
                            rfn.nutrientApplication = _sd.GetApplication(m.applicationId.ToString()).ApplicationMethod;
                            rfn.nutrientUnit = _sd.GetUnit(m.unitId).Name;
                            rf.nutrients.Add(rfn);
                        }
                    }
                    if (f.nutrients.nutrientFertilizers != null)
                    {
                        foreach (var ft in f.nutrients.nutrientFertilizers)
                        {
                            string fertilizerName = string.Empty;
                            ReportFieldNutrient rfn = new ReportFieldNutrient();
                            FertilizerType ftyp = _sd.GetFertilizerType(ft.fertilizerTypeId.ToString());

                            if (ftyp.Custom)
                            {
                                fertilizerName = ftyp.DryLiquid == "dry" ? "Custom (Dry) " : "Custom (Liquid) ";
                                fertilizerName = fertilizerName + ft.customN.ToString() + "-" + ft.customP2o5.ToString() + "-" + ft.customK2o.ToString();
                            }
                            else
                            {
                                Fertilizer ff = _sd.GetFertilizer(ft.fertilizerId.ToString());
                                fertilizerName = ff.Name;
                            }

                            rfn.nutrientName = fertilizerName;
                            rfn.nutrientAmount = String.Format((ft.applRate) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", ft.applRate);
                            rfn.nutrientSeason = ft.applDate != null ? ft.applDate.Value.ToString("MMM-yyyy") : "";
                            rfn.nutrientApplication = ft.applMethodId > 0 ? _sd.GetFertilizerMethod(ft.applMethodId.ToString()).Name : "";
                            rfn.nutrientUnit = _sd.GetFertilizerUnit(ft.applUnitId).Name;

                            rf.nutrients.Add(rfn);
                        }
                    }
                }
                if (rf.nutrients.Count() == 0)
                {
                    ReportFieldNutrient rfn = new ReportFieldNutrient();
                    rfn.nutrientName = "None planned";
                    rfn.nutrientAmount = "";
                    rf.nutrients.Add(rfn);
                }

                if (f.crops != null)
                {
                    foreach(var c in f.crops)
                    {
                        crpName = string.IsNullOrEmpty(c.cropOther) ? _sd.GetCrop(Convert.ToInt32(c.cropId)).CropName : c.cropOther;
                        rf.fieldCrops = string.IsNullOrEmpty(rf.fieldCrops) ? crpName : rf.fieldCrops + "\n" + crpName;
                    }
                }
                rvm.fields.Add(rf);
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportApplication.cshtml", rvm);

            return result;
        }
        public async Task<string> RenderSummary()
        {
            string crpName = string.Empty;
            Utility.SoilTestConversions stc = new SoilTestConversions(_ud, _sd);

            ReportSummaryViewModel rvm = new ReportSummaryViewModel();

            rvm.testMethod = string.IsNullOrEmpty(_ud.FarmDetails().testingMethod) ? "Not Specified" : _sd.GetSoilTestMethod(_ud.FarmDetails().testingMethod);
            rvm.year = _ud.FarmDetails().year;

            FarmDetails fd = _ud.FarmDetails();

            rvm.tests = new List<ReportSummaryTest>();

            List<Field> flds = _ud.GetFields();

            foreach (var m in flds)
            {
                ReportSummaryTest dc = new ReportSummaryTest();
                dc.fieldName = m.fieldName;
                if (m.soilTest != null)
                {
                    dc.sampleDate = m.soilTest.sampleDate.ToString("MMM-yyyy");
                    dc.nitrogen = m.soilTest.valNO3H.ToString("G29");
                    dc.phosphorous = m.soilTest.ValP.ToString("G29");
                    dc.potassium = m.soilTest.valK.ToString("G29");
                    dc.pH = m.soilTest.valPH.ToString("G29");
                    dc.phosphorousRange = _sd.GetPhosphorusSoilTestRating(stc.GetConvertedSTP(m.soilTest));
                    dc.potassiumRange = _sd.GetPotassiumSoilTestRating(stc.GetConvertedSTK(m.soilTest));
                }
                else
                {
                    DefaultSoilTest dt = _sd.GetDefaultSoilTest();
                    SoilTest st = new SoilTest();
                    st.valPH = dt.pH;
                    st.ValP = dt.Phosphorous;
                    st.valK = dt.Potassium;
                    st.valNO3H = dt.Nitrogen;
                    st.ConvertedKelownaP = dt.ConvertedKelownaP;
                    st.ConvertedKelownaK = dt.ConvertedKelownaK;

                    dc.sampleDate = "Default Values";
                    dc.nitrogen = dt.Nitrogen.ToString("G29");
                    dc.phosphorous = dt.Phosphorous.ToString("G29");
                    dc.potassium = dt.Potassium.ToString("G29");
                    dc.pH = dt.pH.ToString("G29");
                    dc.phosphorousRange = _sd.GetPhosphorusSoilTestRating(stc.GetConvertedSTP(st));
                    dc.potassiumRange = _sd.GetPotassiumSoilTestRating(stc.GetConvertedSTK(st));
                }
                dc.fieldCrops = null;

                List<FieldCrop> crps = _ud.GetFieldCrops(m.fieldName);
                if(crps != null)
                {
                    foreach(var c in crps)
                    {
                        crpName = string.IsNullOrEmpty(c.cropOther) ? _sd.GetCrop(Convert.ToInt32(c.cropId)).CropName : c.cropOther;
                        dc.fieldCrops = string.IsNullOrEmpty(dc.fieldCrops) ? crpName : dc.fieldCrops + "\n" + crpName;
                    }
                }
                rvm.tests.Add(dc);
            }


            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportSummary.cshtml", rvm);

            return result;
        }
        public async Task<string> RenderSheets()
        {
            string crpName = string.Empty;
            ReportSheetsViewModel rvm = new ReportSheetsViewModel();
            rvm.year = _ud.FarmDetails().year;

            rvm.fields = new List<ReportSheetsField>();

            List<Field> fldList = _ud.GetFields();
            foreach (var f in fldList)
            {
                ReportSheetsField rf = new ReportSheetsField();
                rf.fieldName = f.fieldName;
                rf.fieldArea = f.area.ToString("G29");
                rf.nutrients = new List<ReportFieldNutrient>();
                if (f.nutrients != null)
                {
                    if (f.nutrients.nutrientManures != null)
                    {
                        foreach (var m in f.nutrients.nutrientManures)
                        {
                            FarmManure manure = _ud.GetFarmManure(Convert.ToInt32(m.manureId));
                            ReportFieldNutrient rfn = new ReportFieldNutrient();

                            rfn.nutrientName = manure.name;
                            rfn.nutrientAmount = String.Format((m.rate) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", (m.rate));
                            rfn.nutrientSeason = _sd.GetApplication(m.applicationId.ToString()).Season;
                            rfn.nutrientApplication = _sd.GetApplication(m.applicationId.ToString()).ApplicationMethod;
                            rfn.nutrientUnit = _sd.GetUnit(m.unitId).Name;
                            rf.nutrients.Add(rfn);
                        }
                    }
                    if (f.nutrients.nutrientFertilizers != null)
                    {
                        foreach (var ft in f.nutrients.nutrientFertilizers)
                        {
                            string fertilizerName = string.Empty;
                            ReportFieldNutrient rfn = new ReportFieldNutrient();
                            FertilizerType ftyp = _sd.GetFertilizerType(ft.fertilizerTypeId.ToString());

                            if (ftyp.Custom)
                            {
                                fertilizerName = ftyp.DryLiquid == "dry" ? "Custom (Dry) " : "Custom (Liquid) ";
                                fertilizerName = fertilizerName + ft.customN.ToString() + "-" + ft.customP2o5.ToString() + "-" + ft.customK2o.ToString();
                            }
                            else
                            {
                                Fertilizer ff = _sd.GetFertilizer(ft.fertilizerId.ToString());
                                fertilizerName = ff.Name;
                            }

                            rfn.nutrientName = fertilizerName;
                            rfn.nutrientAmount = String.Format((ft.applRate) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", (ft.applRate));
                            rfn.nutrientSeason = ft.applDate != null ? ft.applDate.Value.ToString("MMM-yyyy") : "";
                            rfn.nutrientApplication = ft.applMethodId > 0 ? _sd.GetFertilizerMethod(ft.applMethodId.ToString()).Name : "";
                            rfn.nutrientUnit = _sd.GetFertilizerUnit(ft.applUnitId).Name;

                            rf.nutrients.Add(rfn);
                        }
                    }
                }
                if(rf.nutrients.Count() == 0)
                {
                    ReportFieldNutrient rfn = new ReportFieldNutrient();
                    rfn.nutrientName = "None planned";
                    rfn.nutrientAmount = "";
                    rf.nutrients.Add(rfn);
                }
                if (f.crops != null)
                {
                    foreach (var c in f.crops)
                    {
                        crpName = string.IsNullOrEmpty(c.cropOther) ? _sd.GetCrop(Convert.ToInt32(c.cropId)).CropName : c.cropOther;
                        rf.fieldCrops = string.IsNullOrEmpty(rf.fieldCrops) ? crpName : rf.fieldCrops + "\n" + crpName;
                    }
                }
                if (string.IsNullOrEmpty(rf.fieldCrops))
                {
                    rf.fieldCrops = "None recorded";
                }

                rvm.fields.Add(rf);
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportSheets.cshtml", rvm);

            return result;
        }
        public async Task<string> RenderFonts()
        {
            ReportFontsViewModel rvm = new ReportFontsViewModel();

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportFonts.cshtml", rvm);

            return result;
        }
        public async Task<FileContentResult> BuildPDF(INodeServices nodeServices, PDFRequest rawdata)
        {
            JObject options = JObject.Parse(rawdata.options);
            JSONResponse result = null;

            // execute the Node.js component to generate a PDF
            result = await nodeServices.InvokeAsync<JSONResponse>("./pdf.js", rawdata.html, options);

            return new FileContentResult(result.data, "application/pdf");
        }
        public async Task<IActionResult> PrintFields()
        {
            FileContentResult result = null;

            string reportFields = await RenderFields();

            result = await PrintReportAsync(reportFields, true);

            return result;
        }

        public async Task<IActionResult> PrintManure()
        {
            FileContentResult result = null;
            string pageBreak = "<div>&nbsp;&nbsp;&nbsp;&nbsp;<br/><br/><br/><br/><br/><br/><br/><br/><br/></div>";

            string reportManureCompostInventory = await RenderManureCompostInventory();
            string reportManureUse = await RenderManureUse();
            string reportFertilizers = await RenderFerilizers();

            string report = reportManureCompostInventory + pageBreak + reportManureUse + pageBreak + reportFertilizers;

            result = await PrintReportAsync(report, true);

            return result;
        }

        public async Task<IActionResult> PrintSources()
        {
            FileContentResult result = null;

            string reportSources = await RenderSources();

            result = await PrintReportAsync(reportSources, true);

            return result;
        }
        public async Task<IActionResult> PrintSheets()
        {
            FileContentResult result = null;

            string reportSheets = await RenderSheets();

            result = await PrintReportAsync(reportSheets, false);

            return result;
        }
        public async Task<IActionResult> PrintAnalysis()
        {
            FileContentResult result = null;

            string reportAnalysis = await RenderAnalysis();

            result = await PrintReportAsync(reportAnalysis, true);

            return result;
        }
        public async Task<IActionResult> PrintApplication()
        {
            FileContentResult result = null;

            string reportApplication = await RenderApplication();

            result = await PrintReportAsync(reportApplication, true);

            return result;
        }
        public async Task<IActionResult> PrintSummary()
        {
            FileContentResult result = null;

            string reportSummary = await RenderSummary();

            result = await PrintReportAsync(reportSummary, true);

            return result;
        }
        public async Task<IActionResult> PrintFonts()
        {
            FileContentResult result = null;

            string reportFonts = await RenderFonts();

            result = await PrintReportAsync(reportFonts, false);

            return result;
        }
        public async Task<IActionResult> PrintComplete()
        {
            FileContentResult result = null;
            string pageBreak = "<div style=\"page-break-after:always;\">&nbsp;</div>";
            string pageBreakForManure = "<div>&nbsp;&nbsp;&nbsp;&nbsp;<br/><br/><br/><br/><br/><br/><br/><br/><br/></div>";

            string reportApplication = await RenderApplication();
            string reportManureCompostInventory = await RenderManureCompostInventory();
            string reportManureUse = await RenderManureUse();
            string reportFertilizers = await RenderFerilizers();
            string reportFields = await RenderFields();
            string reportAnalysis = await RenderAnalysis();
            string reportSummary = await RenderSummary();

            string report = reportApplication + pageBreak + reportManureCompostInventory + pageBreakForManure + reportManureUse + pageBreakForManure + reportFertilizers + pageBreak + reportFields + pageBreak + reportAnalysis + pageBreak + reportSummary;

            result = await PrintReportAsync(report, true);

            return result;
        }
        public async Task<FileContentResult> PrintReportAsync(string content, bool portrait)
        {
            string reportHeader = string.Empty;

            FileContentResult result = null;
            var pdfHost = Environment.GetEnvironmentVariable("PDF_SERVICE_NAME");

            string targetUrl = pdfHost + "/api/PDF/BuildPDF";

            PDF_Options options = new PDF_Options();
            options.border = new PDF_Border();
            options.header = new PDF_Header();
            options.footer = new PDF_Footer();

            options.type = "pdf";
            options.quality = "75";
            options.format = "letter";
            options.orientation = (portrait) ? "portrait" : "landscape";
            options.fontbase = "/usr/share/fonts/dejavu";
            options.border.top = ".25in";
            options.border.right = ".25in";
            options.border.bottom = ".25in";
            options.border.left = ".25in";
            options.header.height = "20mm";
            options.header.contents = "<div><span style=\"float: left; font-size:14px\">Farm Name: " + _ud.FarmDetails().farmName + "<br />" +
                                      "Planning Year: " + _ud.FarmDetails().year + "</span></div><div style=\"float:right; vertical-align:top; text-align: right\"><span style=\"color: #444;\">Page {{page}}</span>/<span>{{pages}}</span><br />Printed: " + DateTime.Now.ToShortDateString() + "</div>";
            options.footer.height = "15mm";
            options.footer.contents = "<div></div><div style=\"float:right\">Version " + _sd.GetStaticDataVersion() + "</div>";

            // call the microservice
            try
            {
                PDFRequest req = new PDFRequest();

                HttpClient client = new HttpClient();

                reportHeader = await RenderHeader();

                string rawdata = "<!DOCTYPE html>" +
                    "<html>" +
                    reportHeader +
                    "<body>" +
                    //"<div style='display: table; width: 100%'>" +
                    //"<div style='display: table-row-group; width: 100%'>" +
                    content +
                    //"</div>" +
                    //"</div>" +
                    "</body></html>";

                req.html = rawdata;
                req.options = JsonConvert.SerializeObject(options);
                req.options = req.options.Replace("fontbase", "base");

                //FileContentResult res = await BuildPDF(nodeServices, req);

                //return res;

                string payload = JsonConvert.SerializeObject(req);

                var request = new HttpRequestMessage(HttpMethod.Post, targetUrl);
                request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

                request.Headers.Clear();
                // transfer over the request headers.
                foreach (var item in Request.Headers)
                {
                    string key = item.Key;
                    string value = item.Value;
                    request.Headers.Add(key, value);
                }

                Task<HttpResponseMessage> responseTask = client.SendAsync(request);
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;

                ViewBag.StatusCode = response.StatusCode.ToString();

                if (response.StatusCode == HttpStatusCode.OK) // success
                {
                    var bytetask = response.Content.ReadAsByteArrayAsync();
                    bytetask.Wait();

                    result = new FileContentResult(bytetask.Result, "application/pdf");
                }
                else
                {
                    string errorMsg = "Url: " + targetUrl + "\r\n" +
                                      "Result: " + response.ToString();
                    result = new FileContentResult(Encoding.ASCII.GetBytes(errorMsg), "text/plain");
                }
            }
            catch (Exception e)
            {
                string errorMsg = "Exception " + "\r\n" +
                                  "Url: " + targetUrl + "\r\n" +
                                  "Result: " + e.Message + "\r\n" +
                                  "Result: " + e.InnerException.Message;
                result = new FileContentResult(Encoding.ASCII.GetBytes(errorMsg), "text/plain");
            }

            return result;
        }
    }
}