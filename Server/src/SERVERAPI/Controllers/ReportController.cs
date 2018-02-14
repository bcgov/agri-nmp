using System;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SERVERAPI.Models.Impl;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.NodeServices;
using static SERVERAPI.Models.StaticData;

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
        public Models.Impl.StaticData _sd { get; set; }
        public IViewRenderService _viewRenderService { get; set; }
        public AppSettings _settings;

        public ReportController(IHostingEnvironment env, IViewRenderService viewRenderService, UserData ud, Models.Impl.StaticData sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
            _viewRenderService = viewRenderService;
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

            rvm.downloadMsg = _sd.GetUserPrompt("reportdownload");
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
            Utility.CalculateNutrients calculateNutrients = new CalculateNutrients(_env, _ud, _sd);
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
                rf.fieldArea = f.area.ToString();
                rf.fieldName = f.fieldName;
                rf.fieldComment = f.comment;
                rf.soiltest = new ReportFieldSoilTest();
                rf.crops = new List<ReportFieldCrop>();
                rf.otherNutrients = new List<ReportFieldOtherNutrient>();
                rf.footnotes = new List<ReportFieldFootnote>();
                rf.showNitrogenCredit = false;

                if(f.soilTest != null)
                {
                    rf.soiltest.sampleDate = f.soilTest.sampleDate.ToString("MMM yyyy");
                    rf.soiltest.dispNO3H = f.soilTest.valNO3H.ToString() + " ppm";
                    rf.soiltest.dispP = f.soilTest.ValP.ToString() + " ppm (" + _sd.SoilTestRating("phosphorous", stc.GetConvertedSTP(f.soilTest)) + ")";
                    rf.soiltest.dispK = f.soilTest.valK.ToString() + " ppm (" + _sd.SoilTestRating("potassium", stc.GetConvertedSTK(f.soilTest)) + ")";
                    rf.soiltest.dispPH = f.soilTest.valPH.ToString();
                }

                rf.nutrients = new List<ReportFieldNutrient>();
                if (f.crops != null)
                {
                    foreach (var c in f.crops)
                    {
                        ReportFieldCrop fc = new ReportFieldCrop();

                        fc.cropname = string.IsNullOrEmpty(c.cropOther) ? _sd.GetCrop(Convert.ToInt32(c.cropId)).cropname : c.cropOther;
                        if (c.coverCropHarvested.HasValue)
                        {
                            fc.cropname = c.coverCropHarvested.Value ? fc.cropname + "(harvested)" : fc.cropname;
                        }
                        if (c.prevCropId > 0)
                            fc.previousCrop = _sd.GetPrevCropType(c.prevCropId).name;

                        if (_sd.GetCropType(_sd.GetCrop(Convert.ToInt32(c.cropId)).croptypeid).crudeproteinrequired)
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

                        if (_sd.GetCropType(_sd.GetCrop(Convert.ToInt32(c.cropId)).croptypeid).modifynitrogen)
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
                            string nCredit = c.prevCropId != 0 ? _sd.GetPrevCropType(Convert.ToInt32(c.prevCropId)).nCreditImperial.ToString() : "0";

                            if (!string.IsNullOrEmpty(nCredit))
                                calculateCropRequirementRemoval.nCredit = Convert.ToInt16(nCredit);

                            cropRequirementRemoval = calculateCropRequirementRemoval.GetCropRequirementRemoval();

                            string stdNAmt = cropRequirementRemoval.N_Requirement.ToString();

                            if(c.reqN.ToString() != cropRequirementRemoval.N_Requirement.ToString())
                            {
                                ReportFieldFootnote rff = new ReportFieldFootnote();
                                rff.id = rf.footnotes.Count() + 1;
                                rff.message = "Crop required nitrogen adjusted to " + c.reqN.ToString();
                                fc.footnote = rff.id.ToString();
                                rf.footnotes.Add(rff);
                            }
                        }

                        fc.yield = c.yield;
                        fc.reqN = -c.reqN;
                        fc.reqP = -c.reqP2o5;
                        fc.reqK = -c.reqK2o;
                        fc.remN = -c.remN;
                        fc.remP = -c.remP2o5;
                        fc.remK = -c.remK2o;

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
                        rf.showNitrogenCredit = f.crops.Count() > 0 ? true : false;
                        if (f.prevYearManureApplicationNitrogenCredit == null)
                        {   // special case when user navigates with PRINT report using old version of saved data
                            rf.nitrogenCredit = 0;
                        }
                        else
                            rf.nitrogenCredit = f.prevYearManureApplicationNitrogenCredit;
                        rf.reqN +=  Convert.ToDecimal(rf.nitrogenCredit);
                    }
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
                            rfn.nutrientSeason = _sd.GetApplication(m.applicationId.ToString()).season;
                            rfn.nutrientApplication = _sd.GetApplication(m.applicationId.ToString()).application_method;
                            rfn.nutrientUnit = _sd.GetUnit(m.unitId).name;
                            rfn.reqN = m.yrN;
                            rfn.reqP = m.yrP2o5;
                            rfn.reqK = m.yrK2o;
                            rfn.remN = m.ltN;
                            rfn.remP = m.ltP2o5;
                            rfn.remK = m.ltK2o;
                            rf.nutrients.Add(rfn);

                            rf.reqN = rf.reqN + rfn.reqN;
                            rf.reqP = rf.reqP + rfn.reqP;
                            rf.reqK = rf.reqK + rfn.reqK;
                            rf.remN = rf.remN + rfn.remN;
                            rf.remP = rf.remP + rfn.remP;
                            rf.remK = rf.remK + rfn.remK;

                            int regionid = _ud.FarmDetails().farmRegion.Value;
                            Region region = _sd.GetRegion(regionid);
                            nOrganicMineralizations = calculateNutrients.GetNMineralization(Convert.ToInt32(m.manureId), region.locationid);

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

                            if (ftyp.custom)
                            {
                                fertilizerName = ftyp.dry_liquid == "dry" ? "Custom (Dry) " : "Custom (Liquid) ";
                                fertilizerName = fertilizerName + ft.customN.ToString() + "-" + ft.customP2o5.ToString() + "-" + ft.customK2o.ToString();
                                rfn.reqN = ft.fertN;
                                rfn.reqP = ft.fertP2o5;
                                rfn.reqK = ft.fertK2o;
                                rfn.remN = ft.fertN;
                                rfn.remP = ft.fertP2o5;
                                rfn.remK = ft.fertK2o;
                            }
                            else
                            {
                                Fertilizer ff = _sd.GetFertilizer(ft.fertilizerId.ToString());
                                fertilizerName = ff.name;
                                rfn.reqN = ft.fertN;
                                rfn.reqP = ft.fertP2o5;
                                rfn.reqK = ft.fertK2o;
                                rfn.remN = ft.fertN;
                                rfn.remP = ft.fertP2o5;
                                rfn.remK = ft.fertK2o;
                            }

                            rfn.nutrientName = fertilizerName;
                            rfn.nutrientApplication = ft.applMethodId > 0 ? _sd.GetFertilizerMethod(ft.applMethodId.ToString()).name : "";
                            rfn.nutrientUnit = _sd.GetFertilizerUnit(ft.applUnitId).name;

                            rfn.nutrientAmount = String.Format((ft.applRate) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", (ft.applRate));
                            rf.nutrients.Add(rfn);

                            rf.reqN = rf.reqN + rfn.reqN;
                            rf.reqP = rf.reqP + rfn.reqP;
                            rf.reqK = rf.reqK + rfn.reqK;
                            rf.remN = rf.remN + rfn.remN;
                            rf.remP = rf.remP + rfn.remP;
                            rf.remK = rf.remK + rfn.remK;

                            string footNote = "";

                            if (ftyp.dry_liquid == "liquid")
                            {
                                if (!ftyp.custom)
                                {
                                    if (ft.liquidDensity.ToString("#.##") != _sd.GetLiquidFertilizerDensity(ft.fertilizerId, ft.liquidDensityUnitId).value.ToString("#.##"))
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
                            fon.reqN = o.ltN;
                            fon.reqP = o.ltP2o5;
                            fon.reqK = o.ltK;
                            fon.remN = o.yrN;
                            fon.remP = o.yrP2o5;
                            fon.remK = o.yrK;
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
        public async Task<string> RenderSources()
        {
            ReportSourcesViewModel rvm = new ReportSourcesViewModel();
            rvm.year = _ud.FarmDetails().year;
            rvm.details = new List<ReportSourcesDetail>();

            List<Field> fldList = _ud.GetFields();
            foreach(var f in fldList)
            {
                if (f.nutrients != null)
                {
                    if (f.nutrients.nutrientManures != null)
                    {
                        foreach (var m in f.nutrients.nutrientManures)
                        {
                            FarmManure manure = _ud.GetFarmManure(Convert.ToInt32(m.manureId));
                            ReportSourcesDetail rd = rvm.details.FirstOrDefault(d => d.nutrientName == manure.name);
                            if (rd != null)
                            {
                                rd.nutrientAmount = String.Format((m.rate * f.area) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", (m.rate * f.area));
                            }
                            else
                            {
                                rd = new ReportSourcesDetail();
                                rd.nutrientName = manure.name;
                                rd.nutrientUnit = _sd.GetUnit(m.unitId).name;
                                int index = rd.nutrientUnit.LastIndexOf("/");
                                if (index > 0)
                                    rd.nutrientUnit = rd.nutrientUnit.Substring(0, index);
                                rd.nutrientAmount = String.Format((m.rate * f.area) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", (m.rate * f.area));
                                rvm.details.Add(rd);
                            }
                        }
                    }
                }
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportSources.cshtml", rvm);

            return result;
        }
        public async Task<string> RenderAnalysis()
        {
            ReportAnalysisViewModel rvm = new ReportAnalysisViewModel();
            rvm.nitratePresent = false;

            rvm.details = new List<ReportAnalysisDetail>();

            List<FarmManure> manures = _ud.GetFarmManures();

            foreach(var m in manures)
            {
                ReportAnalysisDetail rd = new ReportAnalysisDetail();

                rd.manureName = m.name;
                rd.moisture = m.moisture.ToString();
                rd.ammonia = m.ammonia.ToString("#0");
                rd.nitrogen = m.nitrogen.ToString("#0.00");
                rd.phosphorous = m.phosphorous.ToString("#0.00");
                rd.potassium = m.potassium.ToString("#0.00");
                rd.nitrate = m.nitrate.HasValue ? m.nitrate.Value.ToString("#0"): "n/a";

                if(m.nitrate.HasValue)
                {
                    rvm.nitratePresent = true;
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
                rf.fieldArea = f.area.ToString();
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
                            rfn.nutrientSeason = _sd.GetApplication(m.applicationId.ToString()).season;
                            rfn.nutrientApplication = _sd.GetApplication(m.applicationId.ToString()).application_method;
                            rfn.nutrientUnit = _sd.GetUnit(m.unitId).name;
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

                            if (ftyp.custom)
                            {
                                fertilizerName = ftyp.dry_liquid == "dry" ? "Custom (Dry) " : "Custom (Liquid) ";
                                fertilizerName = fertilizerName + ft.customN.ToString() + "-" + ft.customP2o5.ToString() + "-" + ft.customK2o.ToString();
                            }
                            else
                            {
                                Fertilizer ff = _sd.GetFertilizer(ft.fertilizerId.ToString());
                                fertilizerName = ff.name;
                            }

                            rfn.nutrientName = fertilizerName;
                            rfn.nutrientAmount = String.Format((ft.applRate) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", ft.applRate);
                            rfn.nutrientSeason = ft.applDate != null ? ft.applDate.Value.ToString("MMM-yyyy") : "";
                            rfn.nutrientApplication = ft.applMethodId > 0 ? _sd.GetFertilizerMethod(ft.applMethodId.ToString()).name : "";
                            rfn.nutrientUnit = _sd.GetFertilizerUnit(ft.applUnitId).name;

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
                        crpName = string.IsNullOrEmpty(c.cropOther) ? _sd.GetCrop(Convert.ToInt32(c.cropId)).cropname : c.cropOther;
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
                    dc.nitrogen = m.soilTest.valNO3H.ToString();
                    dc.phosphorous = m.soilTest.ValP.ToString();
                    dc.potassium = m.soilTest.valK.ToString();
                    dc.pH = m.soilTest.valPH.ToString();
                    dc.phosphorousRange = _sd.SoilTestRating("phosphorous",stc.GetConvertedSTP(m.soilTest));
                    dc.potassiumRange = _sd.SoilTestRating("potassium", stc.GetConvertedSTK(m.soilTest));
                }
                else
                {
                    DefaultSoilTest dt = _sd.GetDefaultSoilTest();
                    SoilTest st = new SoilTest();
                    st.valPH = dt.pH;
                    st.ValP = dt.phosphorous;
                    st.valK = dt.potassium;
                    st.valNO3H = dt.nitrogen;
                    st.ConvertedKelownaP = dt.convertedKelownaP;
                    st.ConvertedKelownaK = dt.convertedKelownaK;

                    dc.sampleDate = "Default Values";
                    dc.nitrogen = dt.nitrogen.ToString();
                    dc.phosphorous = dt.phosphorous.ToString();
                    dc.potassium = dt.potassium.ToString();
                    dc.pH = dt.pH.ToString();
                    dc.phosphorousRange = _sd.SoilTestRating("phosphorous", stc.GetConvertedSTP(st));
                    dc.potassiumRange = _sd.SoilTestRating("potassium", stc.GetConvertedSTK(st));
                }
                dc.fieldCrops = null;

                List<FieldCrop> crps = _ud.GetFieldCrops(m.fieldName);
                if(crps != null)
                {
                    foreach(var c in crps)
                    {
                        crpName = string.IsNullOrEmpty(c.cropOther) ? _sd.GetCrop(Convert.ToInt32(c.cropId)).cropname : c.cropOther;
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
                rf.fieldArea = f.area.ToString();
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
                            rfn.nutrientSeason = _sd.GetApplication(m.applicationId.ToString()).season;
                            rfn.nutrientApplication = _sd.GetApplication(m.applicationId.ToString()).application_method;
                            rfn.nutrientUnit = _sd.GetUnit(m.unitId).name;
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

                            if (ftyp.custom)
                            {
                                fertilizerName = ftyp.dry_liquid == "dry" ? "Custom (Dry) " : "Custom (Liquid) ";
                                fertilizerName = fertilizerName + ft.customN.ToString() + "-" + ft.customP2o5.ToString() + "-" + ft.customK2o.ToString();
                            }
                            else
                            {
                                Fertilizer ff = _sd.GetFertilizer(ft.fertilizerId.ToString());
                                fertilizerName = ff.name;
                            }

                            rfn.nutrientName = fertilizerName;
                            rfn.nutrientAmount = String.Format((ft.applRate) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", (ft.applRate));
                            rfn.nutrientSeason = ft.applDate != null ? ft.applDate.Value.ToString("MMM-yyyy") : "";
                            rfn.nutrientApplication = ft.applMethodId > 0 ? _sd.GetFertilizerMethod(ft.applMethodId.ToString()).name : "";
                            rfn.nutrientUnit = _sd.GetFertilizerUnit(ft.applUnitId).name;

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
                        crpName = string.IsNullOrEmpty(c.cropOther) ? _sd.GetCrop(Convert.ToInt32(c.cropId)).cropname : c.cropOther;
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

            string reportApplication = await RenderApplication();
            string reportSources = await RenderSources();
            string reportFields = await RenderFields();
            string reportAnalysis = await RenderAnalysis();
            string reportSummary = await RenderSummary();

            string report = reportApplication + pageBreak + reportSources + pageBreak + reportFields + pageBreak + reportAnalysis + pageBreak + reportSummary;

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
            options.header.contents = "<div><span style=\"font-size:14px\">Farm Name: " + _ud.FarmDetails().farmName + "<br />" +
                                      "Planning Year: " + _ud.FarmDetails().year + "</span></div><div style=\"float:right; vertical-align:top\">Printed: " + DateTime.Now.ToShortDateString() + "</div>";
            options.footer.height = "15mm";
            options.footer.contents = "<div><span style=\"color: #444;\">Page {{page}}</span>/<span>{{pages}}</span></div><div style=\"float:right\">Version " + _sd.GetStaticDataVersion() + "</div>";

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
        [HttpGet]
        public ActionResult InfoMessage(string type)
        {
            InfoAgriViewModel ivm = new InfoAgriViewModel();

            switch (type)
            {
                case "download":
                    ivm.text = _sd.GetUserPrompt("agriinfomessage");
                    break;
                case "load":
                    ivm.text = _sd.GetUserPrompt("cropinfomessage");
                    break;
            }
            return PartialView("InfoMessage", ivm);
        }
    }
}