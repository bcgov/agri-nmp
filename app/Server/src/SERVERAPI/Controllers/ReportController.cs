using Agri.CalculateService;
using Agri.Data;
using Agri.Models;
using Agri.Models.Calculate;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SERVERAPI.Filters;
using SERVERAPI.Models.Impl;
using SERVERAPI.Utility;
using SERVERAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

    [SessionTimeout]
    public class ReportController : BaseController
    {
        private readonly ILogger<ReportController> _logger;
        private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly ICalculateAnimalRequirement _calculateAnimalRequirement;
        private readonly ICalculateCropRequirementRemoval _calculateCropRequirementRemoval;
        private readonly ICalculateNutrients _calculateNutrients;
        private readonly IChemicalBalanceMessage _chemicalBalanceMessage;
        private readonly IManureApplicationCalculator _manureApplicationCalculator;
        private readonly ISoilTestConverter _soilTestConverter;
        private readonly IFeedAreaCalculator _feedCalculator;

        public ReportController(ILogger<ReportController> logger,
            IViewRenderService viewRenderService,
            UserData ud,
            IAgriConfigurationRepository sd,
            IMapper mapper,
            ICalculateAnimalRequirement calculateAnimalRequirement,
            ICalculateCropRequirementRemoval calculateCropRequirementRemoval,
            ICalculateNutrients calculateNutrients,
            IChemicalBalanceMessage chemicalBalanceMessage,
            IManureApplicationCalculator manureApplicationCalculator,
            ISoilTestConverter soilTestConverter,
            IFeedAreaCalculator feedCalculator
            )
        {
            _logger = logger;
            _ud = ud;
            _sd = sd;
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _calculateAnimalRequirement = calculateAnimalRequirement;
            _calculateCropRequirementRemoval = calculateCropRequirementRemoval;
            _calculateNutrients = calculateNutrients;
            _chemicalBalanceMessage = chemicalBalanceMessage;
            _manureApplicationCalculator = manureApplicationCalculator;
            _soilTestConverter = soilTestConverter;
            _feedCalculator = feedCalculator;
        }

        [HttpGet]
        public IActionResult Report()
        {
            bool cropFound = true;

            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            ReportViewModel rvm = new ReportViewModel();
            rvm.UnsavedData = farmData.unsaved;
            rvm.Url = _sd.GetExternalLink("finishpagetarget");

            List<Field> fldLst = _ud.GetFields();

            if (fldLst.Count() == 0)
            {
                cropFound = false;
            }
            else
            {
                foreach (var f in fldLst)
                {
                    if (f.Crops == null)
                    {
                        cropFound = false;
                        break;
                    }
                }
            }

            if (!cropFound)
            {
                rvm.NoCropsMsg = _sd.GetUserPrompt("nocropmessage");
            }

            rvm.GeneratedManures = _ud.GetGeneratedManures();
            rvm.ImportedManures = _ud.GetImportedManures();

            if (rvm.UnallocatedManures.Count > 0)
            {
                rvm.MaterialsNotStoredMessage = _sd.GetUserPrompt("materialsNotStoredMessage");
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

            rvm.DownloadMsg = string.Format(_sd.GetUserPrompt("reportdownload"), Url.Action("DownloadMessage", "Home"));
            rvm.LoadMsg = _sd.GetUserPrompt("reportload");

            return View(rvm);
        }

        [HttpPost]
        public IActionResult Report(ReportViewModel rvm)
        {
            return View(rvm);
        }

        public async Task<string> RenderHeader()
        {
            var rvm = new ReportViewModel();

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportHeader.cshtml", rvm);

            return result;
        }

        public async Task<string> RenderBody(string rawContent, string header, string footer)
        {
            var rvm = new ReportPrintViewModel
            {
                Body = rawContent,
                RepeatHeader = header,
                RepeatFooter = footer
            };

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportBody.cshtml", rvm);

            return result;
        }

        public async Task<string> RenderFields()
        {
            ReportFieldsViewModel rvm = new ReportFieldsViewModel();
            rvm.fields = new List<ReportFieldsField>();
            rvm.year = _ud.FarmDetails().Year;
            rvm.methodName = string.IsNullOrEmpty(_ud.FarmDetails().TestingMethod) ? "not selected" : _sd.GetSoilTestMethod(_ud.FarmDetails().TestingMethod);
            rvm.prevHdg = _sd.GetUserPrompt("ncreditlabel");

            List<Field> fldList = _ud.GetFields();
            foreach (var f in fldList)
            {
                ReportFieldsField rf = new ReportFieldsField();
                rf.fieldArea = f.Area.ToString("G29");
                rf.fieldName = f.FieldName;
                rf.fieldComment = f.Comment;
                rf.soiltest = new ReportFieldSoilTest();
                rf.crops = new List<ReportFieldCrop>();
                rf.otherNutrients = new List<ReportFieldOtherNutrient>();
                rf.footnotes = new List<ReportFieldFootnote>();
                rf.showNitrogenCredit = false;
                rf.showSoilTestNitrogenCredit = false;

                if (f.SoilTest != null)
                {
                    rf.soiltest.sampleDate = f.SoilTest.sampleDate.ToString("MMM yyyy");
                    rf.soiltest.dispNO3H = f.SoilTest.valNO3H.ToString("G29") + " ppm";
                    rf.soiltest.dispP = f.SoilTest.ValP.ToString("G29") + " ppm (" + _sd.GetPhosphorusSoilTestRating(_soilTestConverter.GetConvertedSTP(_ud.FarmDetails()?.TestingMethod, f.SoilTest)) + ")";
                    rf.soiltest.dispK = f.SoilTest.valK.ToString("G29") + " ppm (" + _sd.GetPotassiumSoilTestRating(_soilTestConverter.GetConvertedSTK(_ud.FarmDetails()?.TestingMethod, f.SoilTest)) + ")";
                    rf.soiltest.dispPH = f.SoilTest.valPH.ToString("G29");
                }

                rf.nutrients = new List<ReportFieldNutrient>();
                if (f.Crops != null)
                {
                    foreach (var c in f.Crops)
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
                            if (c.crudeProtien.Value.ToString("#.#") != _calculateCropRequirementRemoval.GetCrudeProtienByCropId(Convert.ToInt32(c.cropId)).ToString("#.#"))
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
                            var cropRequirementRemoval = _calculateCropRequirementRemoval
                                .GetCropRequirementRemoval(Convert.ToInt16(Convert.ToDecimal(c.yield)),
                                Convert.ToDecimal(c.yield),
                                c.crudeProtien.HasValue ? Convert.ToDecimal(c.crudeProtien) : default(decimal?),
                                c.coverCropHarvested,
                                c.prevCropId != 0 ? _sd.GetPrevCropType(Convert.ToInt32(c.prevCropId)).NitrogenCreditImperial : 0,
                                _ud.FarmDetails().FarmRegion.Value,
                                _ud.GetFieldDetails(f.FieldName));

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
                    if (f.Crops.Count() > 0)
                    {
                        rf.showNitrogenCredit = f.PreviousYearManureApplicationFrequency != null ? true : false;
                        if (rf.showNitrogenCredit)
                        {
                            if (f.PreviousYearManureApplicationNitrogenCredit == null)
                            {   // calculate default value.
                                rf.nitrogenCredit = this._chemicalBalanceMessage.CalcPrevYearManureApplDefault(f);
                            }
                            else
                                rf.nitrogenCredit = f.PreviousYearManureApplicationNitrogenCredit;
                            rf.reqN += Convert.ToDecimal(rf.nitrogenCredit);
                        }
                        if (f.SoilTest != null)
                        {
                            rf.showSoilTestNitrogenCredit = _sd.IsNitrateCreditApplicable(_ud.FarmDetails().FarmRegion, f.SoilTest.sampleDate, Convert.ToInt16(_ud.FarmDetails().Year));
                            if (rf.showSoilTestNitrogenCredit)
                            {
                                if (f.SoilTestNitrateOverrideNitrogenCredit == null)
                                {   // calculate default value
                                    rf.soilTestNitrogenCredit = Math.Round(this._chemicalBalanceMessage.CalcSoitTestNitrateDefault(f));
                                }
                                else
                                    rf.soilTestNitrogenCredit = Math.Round(Convert.ToDecimal(f.SoilTestNitrateOverrideNitrogenCredit));

                                rf.reqN += rf.soilTestNitrogenCredit;
                            }
                        }
                    } // f.crops.Count() > 0
                }
                if (f.Nutrients != null)
                {
                    if (f.Nutrients.nutrientManures != null)
                    {
                        foreach (var m in f.Nutrients.nutrientManures)
                        {
                            FarmManure manure = _ud.GetFarmManure(Convert.ToInt32(m.manureId));
                            ReportFieldNutrient rfn = new ReportFieldNutrient();

                            rfn.nutrientName = manure.Name;
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

                            string footNote = "";

                            int regionid = _ud.FarmDetails().FarmRegion.Value;
                            var region = _sd.GetRegion(regionid);
                            if (region != null)
                            {
                                var nOrganicMineralizations = _calculateNutrients.GetNMineralization(_ud.GetFarmManure(Convert.ToInt32(m.manureId)), region.LocationId);

                                if (m.nAvail != nOrganicMineralizations.OrganicN_FirstYear * 100)
                                {
                                    footNote = "1st Yr Organic N Availability adjusted to " + m.nAvail.ToString("###") + "%";
                                }
                            }

                            if (m.nh4Retention != (_calculateNutrients.GetAmmoniaRetention(_ud.GetFarmManure(Convert.ToInt32(m.manureId)), Convert.ToInt16(m.applicationId)) * 100))
                            {
                                footNote = string.IsNullOrEmpty(footNote) ? "" : footNote + ", ";
                                footNote = footNote + "Ammonium-N Retention adjusted to " + m.nh4Retention.ToString("###") + "%";
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
                    if (f.Nutrients.nutrientFertilizers != null)
                    {
                        foreach (var ft in f.Nutrients.nutrientFertilizers)
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
                    if (f.Nutrients.nutrientOthers != null)
                    {
                        foreach (var o in f.Nutrients.nutrientOthers)
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

                var request = HttpContext.Request;
                string scheme = request.Scheme;
                string host = request.Host.ToString();

                rf.alertMsgs = _chemicalBalanceMessage.DetermineBalanceMessages(f, _ud.FarmDetails().FarmRegion.Value, _ud.FarmDetails().Year);

                if (rf.alertMsgs.FirstOrDefault(r => r.Chemical == "AgrN") != null)
                {
                    rf.alertN = true;
                    rf.iconAgriN = rf.alertMsgs.FirstOrDefault(r => r.Chemical == "AgrN").Icon.Replace(" ", "-");
                }
                if (rf.alertMsgs.FirstOrDefault(r => r.Chemical == "AgrP2O5") != null)
                {
                    rf.alertP = true;
                    rf.iconAgriP = rf.alertMsgs.FirstOrDefault(r => r.Chemical == "AgrP2O5").Icon.Replace(" ", "-");
                }
                if (rf.alertMsgs.FirstOrDefault(r => r.Chemical == "AgrK2O") != null)
                {
                    rf.alertK = true;
                    rf.iconAgriK = rf.alertMsgs.FirstOrDefault(r => r.Chemical == "AgrK2O").Icon.Replace(" ", "-");
                }
                if (rf.alertMsgs.FirstOrDefault(r => r.Chemical == "CropN") != null)
                {
                    rf.alertN = true;
                    rf.iconCropN = rf.alertMsgs.FirstOrDefault(r => r.Chemical == "CropN").Icon.Replace(" ", "-");
                }
                if (rf.alertMsgs.FirstOrDefault(r => r.Chemical == "CropP2O5") != null)
                {
                    rf.alertP = true;
                    rf.iconCropP = rf.alertMsgs.FirstOrDefault(r => r.Chemical == "CropP2O5").Icon.Replace(" ", "-");
                }
                if (rf.alertMsgs.FirstOrDefault(r => r.Chemical == "CropK2O") != null)
                {
                    rf.alertK = true;
                    rf.iconCropK = rf.alertMsgs.FirstOrDefault(r => r.Chemical == "CropK2O").Icon.Replace(" ", "-");
                }

                //replace icon space in name with a dash
                foreach (var i in rf.alertMsgs.Where(m => m.Icon != null))
                {
                    i.Icon = i.Icon.Replace(" ", "-");
                }

                rvm.fields.Add(rf);
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportFields.cshtml", rvm);

            return result;
        }

        public async Task<string> RenderManureCompostInventory()
        {
            ReportManureCompostViewModel rmcvm = new ReportManureCompostViewModel();

            rmcvm.storages = new List<ReportStorage>();
            rmcvm.unstoredManures = new List<ReportManuress>();
            rmcvm.year = _ud.FarmDetails().Year;
            decimal rainInMM;
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
                        rm.animalManure = g.AnimalSubTypeName + "," +
                                          g.AverageAnimalNumber + " animals";
                        rm.annualAmount =
                            string.Format("{0:#,##0}", g.AnnualAmount.Split(' ')[0]);
                        if (g.ManureType == ManureMaterialType.Liquid)
                        {
                            rm.units = "US Gallons";
                        }
                        else if (g.ManureType == ManureMaterialType.Solid)
                        {
                            rm.units = "tons";
                        }

                        if (g.WashWaterGallonsToString != "0")
                        {
                            rm.milkingCenterWashWater = g.WashWaterGallonsToString;
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

                rs.storageSystemName = s.Name;
                rs.ManureMaterialType = s.ManureMaterialType;
                rs.annualAmountOfManurePerStorage = string.Format("{0:#,##0}", (s.AnnualTotalAmountofManureInStorage));
                rs.footnotes = new List<ReportFieldFootnote>();
                if (s.GetsRunoffFromRoofsOrYards)
                {
                    runoffAreaSquareFeet = s.RunoffAreaSquareFeet;
                }

                var farmData = _ud.FarmDetails();
                SubRegion subregion = _sd.GetSubRegion(farmData.FarmSubRegion);
                rainInMM = subregion?.AnnualPrecipitation ?? 0;

                // rainInMM = Convert.ToDecimal(s.AnnualPrecipitation);

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
                            rm.animalManure = generatedFarmManure.AnimalSubTypeName + "," +
                                              generatedFarmManure.AverageAnimalNumber + " animals";
                            rm.annualAmount =
                                string.Format("{0:#,##0}", generatedFarmManure.AnnualAmount.Split(' ')[0]);
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
                                    _sd.GetAnimalSubType(Convert.ToInt32(generatedFarmManure.AnimalSubTypeId));
                                if (animalSubType.SolidPerGalPerAnimalPerDay.HasValue)
                                {
                                    rm.annualAmount =
                                        (Math.Round(Convert.ToInt32(generatedFarmManure.AverageAnimalNumber) *
                                                    Convert.ToDecimal(animalSubType.SolidPerGalPerAnimalPerDay) * 365))
                                        .ToString();
                                    rm.units = "US gallons";
                                }
                            }

                            if (generatedFarmManure.WashWaterGallonsToString != "0")
                            {
                                rs.milkingCenterWashWater = generatedFarmManure.WashWaterGallonsToString;
                                washWaterAdjustedValue = generatedFarmManure.WashWater;
                            }

                            if (generatedFarmManure.WashWater.ToString("#.##") != _calculateAnimalRequirement
                                    .GetWashWaterBySubTypeId(generatedFarmManure.AnimalSubTypeId).Value
                                    .ToString("#.##"))
                            {
                                ReportFieldFootnote rff = new ReportFieldFootnote();
                                rff.id = rs.footnotes.Count() + 1;
                                rff.message = "Milking Center Wash Water adjusted to " +
                                              washWaterAdjustedValue.ToString("G29") + " US gallons/day/animal";
                                rs.footnote = rff.id.ToString();
                                rs.footnotes.Add(rff);
                            }

                            if (generatedFarmManure.MilkProduction.ToString() != "0.0")
                            {
                                var defaultMilkProd =
                                    _calculateAnimalRequirement.GetDefaultMilkProductionBySubTypeId(
                                        Convert.ToInt16(generatedFarmManure.AnimalSubTypeId));
                                var breedManureFactor =
                                    _calculateAnimalRequirement.GetBreedManureFactorByBreedId(
                                        Convert.ToInt32(generatedFarmManure.BreedId));
                                var milkProd = defaultMilkProd * breedManureFactor;

                                if (generatedFarmManure.MilkProduction != milkProd)
                                {
                                    ReportFieldFootnote rff = new ReportFieldFootnote();
                                    rff.id = rs.footnotes.Count() + 1;
                                    rff.message = "Milk Production adjusted to " +
                                                  generatedFarmManure.MilkProduction.ToString("G29") + " lb/day/animal";
                                    rs.footnote = rff.id.ToString();
                                    rs.footnotes.Add(rff);
                                }
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
                        else if (m is SeparatedSolidManure)
                        {
                            var separatedSolid = _ud.GetSeparatedManure(m.Id.Value);
                            if (separatedSolid.AssignedToStoredSystem)
                            {
                                var rm = new ReportManuress();
                                rm.animalManure = separatedSolid.ManagedManureName;

                                rm.annualAmount = string.Format("{0:#,##0}", Math.Round(separatedSolid.AnnualAmountTonsWeight)).ToString();
                                rm.units = "tons";
                                rs.reportManures.Add(rm);
                            }
                        }
                    }
                }

                rmcvm.storages.Add(rs);
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportManureCompostInventory.cshtml", rmcvm);

            return result;
        }

        public async Task<string> RenderManureUse()
        {
            if (_ud.FarmDetails().UserJourney != UserJourney.Dairy || _ud.FarmDetails().UserJourney != UserJourney.Mixed)
            {
                return string.Empty;
            }

            ReportManureSummaryViewModel rmsvm = new ReportManureSummaryViewModel();
            rmsvm.manures = new List<ReportManures>();
            rmsvm.footnotes = new List<ReportFieldFootnote>();
            rmsvm.year = _ud.FarmDetails().Year;

            var yearData = _ud.GetYearData();

            if (yearData.FarmManures != null)
            {
                foreach (var fm in yearData.FarmManures)
                {
                    ReportManures rm = new ReportManures();
                    AppliedManure appliedManure = _manureApplicationCalculator.GetAppliedManure(yearData, fm);

                    if (appliedManure != null)
                    {
                        if (fm.StoredImported == NutrientAnalysisTypes.Stored)
                        {
                            rm.MaterialName = "Material in ";
                        }
                        else if (fm.StoredImported == NutrientAnalysisTypes.Imported)
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
                            rm.AmountRemaining = "None";

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

        public async Task<string> RenderOctoberToMarchStorageVolumes()
        {
            ReportOctoberToMarchStorageSummaryViewModel romssvm = new ReportOctoberToMarchStorageSummaryViewModel();
            romssvm.storages = new List<ReportStorages>();
            romssvm.footnotes = new List<ReportFieldFootnote>();
            romssvm.year = _ud.FarmDetails().Year;

            var request = HttpContext.Request;
            string scheme = request.Scheme;
            string host = request.Host.ToString();
            romssvm.ImageClass = "dollar-warning";

            var yearData = _ud.GetYearData();

            if (yearData.FarmManures != null)
            {
                foreach (var fm in yearData.ManureStorageSystems)
                {
                    if (fm.ManureMaterialType == ManureMaterialType.Liquid)
                    {
                        ReportStorages rs = new ReportStorages();
                        rs.storageName = fm.Name;
                        rs.materialsGeneratedImported = fm.OctoberToMarchManagedManuresText;
                        rs.yardRunoff = string.IsNullOrEmpty(fm.OctoberToMarchRunoffText) ? "0" : fm.OctoberToMarchRunoffText;
                        rs.precipitationIntoStorage = string.IsNullOrEmpty(fm.OctoberToMarchPrecipitationText) ? "0" : fm.OctoberToMarchPrecipitationText;
                        rs.totalStored = (fm.OctoberToMarchManagedManures + Convert.ToDecimal(fm.OctoberToMarchRunoff) + Convert.ToDecimal(fm.OctoberToMarchPrecipitation)).ToString();
                        rs.storageVolume = fm.ManureStorageVolume?.Split(' ')[0];
                        rs.materialsStoredAfterSLSeparaton = string.Format("{0:#,##0}", fm.OctoberToMarchSeparatedLiquidsUSGallons);
                        rs.isThereSolidLiquidSeparation = fm.IsThereSolidLiquidSeparation;

                        if (fm.IsThereSolidLiquidSeparation)
                        {
                            rs.totalStored = string.Format("{0:#,##0}", fm.OctoberToMarchSeparatedLiquidsUSGallons + Convert.ToDecimal(fm.OctoberToMarchPrecipitation));
                        }
                        else
                        {
                            rs.totalStored = string.Format("{0:#,##0}", fm.OctoberToMarchManagedManures + Convert.ToDecimal(fm.OctoberToMarchRunoff) +
                                             Convert.ToDecimal(fm.OctoberToMarchPrecipitation));
                        }

                        if (!string.IsNullOrEmpty(fm.ManureStorageVolume) &&
                            Convert.ToInt32(fm.TotalStored) >
                            Convert.ToInt32(fm.ManureStorageVolume.Split(' ')[0].Replace(",", "")))
                        {
                            rs.isThereDeficitOfStorageVolume = true;

                            ReportFieldFootnote rff = new ReportFieldFootnote();
                            rff.id = romssvm.footnotes.Count() + 1;
                            rff.message = "There may not be enough capacity to contain the manure and water to be stored during the non-growing season of an average year";
                            rs.footNote = rff.id.ToString();
                            romssvm.footnotes.Add(rff);
                        }
                        else
                            rs.isThereDeficitOfStorageVolume = false;
                        romssvm.storages.Add(rs);
                    }
                }
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportOctoberToMarchStorageSummary.cshtml", romssvm);

            return result;
        }

        public async Task<string> RenderCropManureUse()
        {
            if (_ud.FarmDetails().UserJourney != UserJourney.Crops)
            {
                return string.Empty;
            }

            var viewModel = new ReportCropsManureViewModel();

            var yearData = _ud.GetYearData();

            if (yearData.FarmManures != null)
            {
                foreach (var fm in yearData.FarmManures)
                {
                    ReportManures rm = new ReportManures();
                    AppliedManure appliedManure = _manureApplicationCalculator.GetAppliedManure(yearData, fm);

                    if (appliedManure != null)
                    {
                        rm.MaterialName = _sd.GetManure(fm.ManureId).Name;
                        rm.MaterialSource = appliedManure.SourceName;

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
                            rm.AmountRemaining = "None";

                            ReportFieldFootnote rff = new ReportFieldFootnote();
                            rff.id = viewModel.Footnotes.Count() + 1;
                            rff.message = "If the amount remaining is less than 10% of the annual amount, then the amount remaining is insignificant (i.e. within the margin of error of the calculations)";
                            rm.footnote = rff.id.ToString();
                            viewModel.Footnotes.Add(rff);
                        }
                        else
                        {
                            rm.AmountRemaining = string.Format("{0:#,##0}", (Math.Round(appliedManure.TotalAnnualManureRemainingToApply))) + " (" + appliedManure.WholePercentRemaining + "%)";
                        }

                        viewModel.Manures.Add(rm);
                    }
                }
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportCropsManure.cshtml", viewModel);

            return result;
        }

        public async Task<string> RenderBeefManureUse()
        {
            if (_ud.FarmDetails().UserJourney != UserJourney.Ranch)
            {
                return string.Empty;
            }

            var viewModel = new ReportBeefManureCollectedViewModel();

            var yearData = _ud.GetYearData();

            if (yearData.FarmManures != null)
            {
                foreach (var fm in yearData.FarmManures)
                {
                    ReportManures rm = new ReportManures();
                    AppliedManure appliedManure = _manureApplicationCalculator.GetAppliedManure(yearData, fm);

                    if (appliedManure != null)
                    {
                        rm.MaterialName = appliedManure.ManureMaterialName;
                        rm.MaterialSource = appliedManure.SourceName;

                        // Annual Amount

                        rm.AnnualAmount = string.Format("{0:#,##0}", Math.Round(appliedManure.TotalAnnualManureToApply)).ToString();
                        rm.AnnualAmount = $"{rm.AnnualAmount} tons";

                        // Amount Land Applied
                        rm.LandApplied = string.Format("{0:#,##0}", Math.Round(appliedManure.TotalApplied)).ToString();
                        rm.LandApplied = $"{rm.LandApplied} tons";

                        // Amount Remaining
                        if (appliedManure.WholePercentRemaining < 10)
                        {
                            rm.AmountRemaining = "None";

                            ReportFieldFootnote rff = new ReportFieldFootnote();
                            rff.id = viewModel.Footnotes.Count() + 1;
                            rff.message = "If the amount remaining is less than 10% of the annual amount, then the amount remaining is insignificant (i.e. within the margin of error of the calculations)";
                            rm.footnote = rff.id.ToString();
                            viewModel.Footnotes.Add(rff);
                        }
                        else
                        {
                            rm.AmountRemaining = string.Format("{0:#,##0}", Math.Round(appliedManure.TotalAnnualManureRemainingToApply));
                        }

                        viewModel.Manures.Add(rm);
                    }
                }
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportBeefManureCollected.cshtml", viewModel);

            return result;
        }

        public async Task<string> RenderFerilizers()
        {
            ReportSourcesViewModel rvm = new ReportSourcesViewModel();
            List<ReportSourcesDetail> manureRequired = new List<ReportSourcesDetail>();
            List<ReportSourcesDetail> fertilizerRequired = new List<ReportSourcesDetail>();

            rvm.year = _ud.FarmDetails().Year;
            rvm.details = new List<ReportSourcesDetail>();

            List<Field> fldList = _ud.GetFields();
            foreach (var f in fldList)
            {
                if (f.Nutrients != null)
                {
                    if (f.Nutrients.nutrientFertilizers != null)
                        fertilizerRequired = BuildFertilizerRequiredList(fertilizerRequired, f.Nutrients.nutrientFertilizers, f.Area);
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

            rvm.year = _ud.FarmDetails().Year;
            rvm.details = new List<ReportSourcesDetail>();

            List<Field> fldList = _ud.GetFields();
            foreach (var f in fldList)
            {
                if (f.Nutrients != null)
                {
                    if (f.Nutrients.nutrientManures != null)
                        manureRequired = BuildManureRequiredList(manureRequired, f.Nutrients.nutrientManures, f.Area);
                    if (f.Nutrients.nutrientFertilizers != null)
                        fertilizerRequired = BuildFertilizerRequiredList(fertilizerRequired, f.Nutrients.nutrientFertilizers, f.Area);
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

            if (unit.FarmReqdNutrientsStdUnitsConversion > 0)
                result = unit.FarmReqdNutrientsStdUnitsAreaConversion * fieldSize * applicationRate * unit.FarmReqdNutrientsStdUnitsConversion;
            else
            {
                Manure man = _sd.GetManure(manure.ManureId);
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
                ReportSourcesDetail rd = details.FirstOrDefault(d => d.nutrientName == manure.Name);
                if (rd != null)
                {
                    nutrientAmount += Convert.ToDecimal(rd.nutrientAmount);
                    rd.nutrientAmount = String.Format((nutrientAmount) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", nutrientAmount);
                }
                else
                {
                    rd = new ReportSourcesDetail();
                    rd.nutrientName = manure.Name;
                    rd.nutrientUnit = _sd.GetManureRptStdUnit(manure.SolidLiquid);
                    rd.nutrientAmount = String.Format((nutrientAmount) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", nutrientAmount);
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
            if (_sd.IsFertilizerTypeDry(nf.fertilizerTypeId))
            {
                fert.Name = "Custom (Dry) " + nf.customN.ToString() + "-" + nf.customP2o5 + "-" + nf.customK2o.ToString();
                fert.DryLiquid = ft.DryLiquid;
            }
            else
            {
                fert.Name = "Custom (Liquid) " + nf.customN.ToString() + "-" + nf.customP2o5 + "-" + nf.customK2o.ToString();
                fert.DryLiquid = ft.DryLiquid;
            }
            fert.Nitrogen = Convert.ToDecimal(nf.customN);
            fert.Phosphorous = Convert.ToDecimal(nf.customP2o5);
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

            foreach (var m in manures)
            {
                ReportAnalysisDetail rd = new ReportAnalysisDetail();

                rd.sourceOfMaterialName = m.SourceOfMaterialName;
                rd.manureName = m.Name;
                rd.moisture = m.Moisture.ToString();
                rd.ammonia = m.Ammonia.ToString("#0");
                rd.nitrogen = m.Nitrogen.ToString("#0.00");
                rd.phosphorous = m.Phosphorous.ToString("#0.00");
                rd.potassium = m.Potassium.ToString("#0.00");
                rd.nitrate = (m.Nitrate.HasValue && m.Nitrate > 0) ? m.Nitrate.Value.ToString("#0") : "n/a";
                rd.isAssignedToStorage = m.IsAssignedToStorage;

                if (m.Nitrate.HasValue && m.Nitrate > 0)
                {
                    rvm.nitratePresent = true;
                }

                if (m.IsAssignedToStorage == false)
                {
                    ReportFieldFootnote rff = new ReportFieldFootnote();
                    rff.id = rvm.footnotes.Count() + 1;
                    rff.message = rd.sourceOfMaterialName + " includes materials that have not been allocted to a storage";
                    rd.footnote = rff.id.ToString();
                    rvm.footnotes.Add(rff);
                }

                rvm.details.Add(rd);
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportAnalysis.cshtml", rvm);

            return result;
        }

        public async Task<string> RenderSeasonalFeedAreaSummary()
        {
            var fields = _ud.GetFields();
            var viewModel = new ReportSeasonalFeedAreaViewModel
            {
                Fields = _mapper.Map<List<Field>, List<ReportSeasonalFeedAreaViewModel.Field>>(fields)
            };

            var region = _sd.GetRegion(_ud.FarmDetails().FarmRegion.Value);
            var dailyFeedRequirements = _sd.GetDailyFeedRequirement();

            var feedForageTypes = new List<FeedForageType>();
            if (fields.Any(f => f.FeedForageAnalyses.Any()))
            {
                feedForageTypes = _sd.GetFeedForageTypes();
            }
            foreach (var field in fields)
            {
                if (field.FeedForageAnalyses != null && field.FeedForageAnalyses.Any())
                {
                    var mappedField = viewModel.Fields.Single(f => f.Id == field.Id);
                    mappedField.NAgroBalance = _feedCalculator.GetNitrogenAgronomicBalance(field, region);
                    mappedField.P205AgroBalance = _feedCalculator.GetP205AgronomicBalance(field);
                    mappedField.K20AgroBalance = _feedCalculator.GetK20AgronomicBalance(field);
                    mappedField.NCropRemovalValue = _feedCalculator.GetNitrogenCropRemovalValue(field, region);
                    mappedField.P205CropRemovalValue = _feedCalculator.GetP205CropRemovalValue(field);
                    mappedField.K20CropRemovalValue = _feedCalculator.GetK20CropRemovalValue(field);
                    mappedField.MatureAnimalDailyFeedRequirement = dailyFeedRequirements
                        .Single(df => df.Id == mappedField.MatureAnimalDailyFeedRequirementId.GetValueOrDefault(0))
                        ?.Value;
                    mappedField.MatureAnimalDailyFeedRequirementName = dailyFeedRequirements
                        .Single(df => df.Id == mappedField.MatureAnimalDailyFeedRequirementId.GetValueOrDefault(0))
                        ?.Name;
                    mappedField.GrowingAnimalDailyFeedRequirement = dailyFeedRequirements
                        .Single(df => df.Id == mappedField.GrowingAnimalDailyFeedRequirementId.GetValueOrDefault(0))
                        ?.Value;
                    mappedField.GrowingAnimalDailyFeedRequirementName = dailyFeedRequirements
                        .Single(df => df.Id == mappedField.GrowingAnimalDailyFeedRequirementId.GetValueOrDefault(0))
                        ?.Name;

                    foreach (var analytic in mappedField.FeedForageAnalyses)
                    {
                        analytic.FeedForageType = feedForageTypes.Single(f => f.Id == analytic.FeedForageTypeId).Name;
                    }
                }
            }

            var result = await _viewRenderService
                .RenderToStringAsync("~/Views/Report/ReportSeasonalFeedArea.cshtml", viewModel);

            return result;
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Field, ReportSeasonalFeedAreaViewModel.Field>()
                    .ForMember(m => m.FieldComment, opts => opts.MapFrom(s => s.Comment))
                    .ForMember(m => m.FieldArea, opts => opts.MapFrom(s => s.Area));
                CreateMap<FeedForageAnalysis, ReportSeasonalFeedAreaViewModel.FeedForageAnalysis>();
            }
        }

        public async Task<string> RenderTableOfContents(bool hasFertilizers, bool hasSoilTests)
        {
            var vm = new ReportTableOfContentsViewModel();
            var yd = _ud.GetYearData();
            var pageNumber = 2;

            //ReportApplication
            vm.ContentItems.Add(new ContentItem { SectionName = "Application Schedule", PageNumber = pageNumber });
            //ReportManureCompostInventory
            if (yd.GeneratedManures.Any() || yd.ImportedManures.Any() || yd.ManureStorageSystems.Any())
            {
                pageNumber = pageNumber + 1;
                vm.ContentItems.Add(new ContentItem { SectionName = "Manure/Compost Inventory", PageNumber = pageNumber });
            }

            //Dairy Mixed ReportManureSummary
            if ((_ud.FarmDetails().UserJourney == UserJourney.Dairy ||
                    _ud.FarmDetails().UserJourney == UserJourney.Mixed) &&
                yd.FarmManures.Any())
            {
                pageNumber = pageNumber + 1;
                vm.ContentItems.Add(new ContentItem { SectionName = "Manure and Compost Use", PageNumber = pageNumber });
            }

            //ReportOctoberToMarchStorageVolumes
            if (yd.FarmManures.Any() &&
                yd.ManureStorageSystems.Any() &&
                yd.ManureStorageSystems.Any(mss => mss.ManureMaterialType == ManureMaterialType.Liquid))
            {
                pageNumber = pageNumber + 1;
                vm.ContentItems.Add(new ContentItem
                {
                    SectionName = "Liquid Storage Capacity: October to March",
                    PageNumber = pageNumber
                });
            }

            //Report Crop Manures
            if (_ud.FarmDetails().UserJourney == UserJourney.Crops && yd.FarmManures.Any())
            {
                pageNumber = pageNumber + 1;
                vm.ContentItems.Add(new ContentItem { SectionName = "Manure and Compost Use", PageNumber = pageNumber });
            }

            //ReportBeefManure
            if (_ud.FarmDetails().UserJourney == UserJourney.Ranch && yd.FarmManures.Any())
            {
                pageNumber = pageNumber + 1;
                vm.ContentItems.Add(new ContentItem
                {
                    SectionName = "Manure and Compost Use",
                    PageNumber = pageNumber
                });
            }

            //ReportFertilizers
            if (hasFertilizers)
            {
                pageNumber = pageNumber + 1;
                vm.ContentItems.Add(new ContentItem { SectionName = "Fertilizer Required", PageNumber = pageNumber });
            }

            //ReportFields
            var fieldNames = _ud.GetFields().Select(f => $"Field Summary: {f.FieldName}");
            foreach (var fieldName in fieldNames)
            {
                pageNumber = pageNumber + 1;
                vm.ContentItems.Add(new ContentItem { SectionName = fieldName, PageNumber = pageNumber });
            }

            //ReportAnalysis
            if (yd.FarmManures.Any())
            {
                pageNumber = pageNumber + 1;
                vm.ContentItems.Add(new ContentItem
                { SectionName = "Manure and Compost Analysis", PageNumber = pageNumber });
            }

            //Seasonal Feeding Areas
            var feedingAreas = _ud.GetFields()
                .Where(f => f.IsSeasonalFeedingArea && f.FeedForageAnalyses != null && f.FeedForageAnalyses.Any())
                .Select(f => $"{f.FieldName} Feeding Area");
            foreach (var area in feedingAreas)
            {
                pageNumber = pageNumber + 1;
                vm.ContentItems.Add(new ContentItem { SectionName = area, PageNumber = pageNumber });
            }

            //ReportSummary
            if (hasSoilTests)
            {
                pageNumber = pageNumber + 1;
                vm.ContentItems.Add(new ContentItem { SectionName = "Soil Test Results", PageNumber = pageNumber });
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportTableOfContents.cshtml", vm);

            return result;
        }

        public async Task<string> RenderApplication()
        {
            string crpName = string.Empty;

            ReportApplicationViewModel rvm = new ReportApplicationViewModel();
            rvm.fields = new List<ReportApplicationField>();
            rvm.year = _ud.FarmDetails().Year;

            List<Field> fldList = _ud.GetFields();
            foreach (var f in fldList)
            {
                ReportApplicationField rf = new ReportApplicationField();
                rf.fieldName = f.FieldName;
                rf.fieldArea = f.Area.ToString("G29");
                rf.fieldComment = f.Comment;
                rf.nutrients = new List<ReportFieldNutrient>();
                if (f.Nutrients != null)
                {
                    if (f.Nutrients.nutrientManures != null)
                    {
                        foreach (var m in f.Nutrients.nutrientManures)
                        {
                            FarmManure manure = _ud.GetFarmManure(Convert.ToInt32(m.manureId));
                            ReportFieldNutrient rfn = new ReportFieldNutrient();

                            rfn.nutrientName = manure.Name;
                            rfn.nutrientAmount = String.Format((m.rate) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", m.rate);
                            rfn.nutrientSeason = _sd.GetApplication(m.applicationId.ToString()).Season;
                            rfn.nutrientApplication = _sd.GetApplication(m.applicationId.ToString()).ApplicationMethod;
                            rfn.nutrientUnit = _sd.GetUnit(m.unitId).Name;
                            rf.nutrients.Add(rfn);
                        }
                    }
                    if (f.Nutrients.nutrientFertilizers != null)
                    {
                        foreach (var ft in f.Nutrients.nutrientFertilizers)
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

                if (f.Crops != null)
                {
                    foreach (var c in f.Crops)
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
            ReportSummaryViewModel rvm = new ReportSummaryViewModel();

            rvm.testMethod = string.IsNullOrEmpty(_ud.FarmDetails().TestingMethod) ? "Not Specified" : _sd.GetSoilTestMethod(_ud.FarmDetails().TestingMethod);
            rvm.year = _ud.FarmDetails().Year;

            FarmDetails fd = _ud.FarmDetails();

            rvm.tests = new List<ReportSummaryTest>();

            List<Field> flds = _ud.GetFields();

            foreach (var m in flds)
            {
                ReportSummaryTest dc = new ReportSummaryTest();
                dc.fieldName = m.FieldName;
                if (m.SoilTest != null)
                {
                    dc.sampleDate = m.SoilTest.sampleDate.ToString("MMM-yyyy");
                    dc.nitrogen = m.SoilTest.valNO3H.ToString("G29");
                    dc.phosphorous = m.SoilTest.ValP.ToString("G29");
                    dc.potassium = m.SoilTest.valK.ToString("G29");
                    dc.pH = m.SoilTest.valPH.ToString("G29");
                    dc.phosphorousRange = _sd.GetPhosphorusSoilTestRating(_soilTestConverter.GetConvertedSTP(_ud.FarmDetails()?.TestingMethod, m.SoilTest));
                    dc.potassiumRange = _sd.GetPotassiumSoilTestRating(_soilTestConverter.GetConvertedSTK(_ud.FarmDetails()?.TestingMethod, m.SoilTest));
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
                    dc.phosphorousRange = _sd.GetPhosphorusSoilTestRating(_soilTestConverter.GetConvertedSTP(_ud.FarmDetails()?.TestingMethod, st));
                    dc.potassiumRange = _sd.GetPotassiumSoilTestRating(_soilTestConverter.GetConvertedSTK(_ud.FarmDetails()?.TestingMethod, st));
                }
                dc.fieldCrops = null;

                List<FieldCrop> crps = _ud.GetFieldCrops(m.FieldName);
                if (crps != null)
                {
                    foreach (var c in crps)
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
            var rvm = new ReportSheetsViewModel
            {
                year = _ud.FarmDetails().Year,
                fields = new List<ReportSheetsField>()
            };

            List<Field> fldList = _ud.GetFields();
            foreach (var f in fldList)
            {
                var rf = new ReportSheetsField
                {
                    fieldName = f.FieldName,
                    fieldArea = f.Area.ToString("G29"),
                    nutrients = new List<ReportFieldNutrient>()
                };
                if (f.Nutrients != null)
                {
                    if (f.Nutrients.nutrientManures != null)
                    {
                        foreach (var m in f.Nutrients.nutrientManures)
                        {
                            var manure = _ud.GetFarmManure(Convert.ToInt32(m.manureId));
                            var rfn = new ReportFieldNutrient
                            {
                                nutrientName = manure.Name,
                                nutrientAmount = String.Format((m.rate) % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", (m.rate)),
                                nutrientSeason = _sd.GetApplication(m.applicationId.ToString()).Season,
                                nutrientApplication = _sd.GetApplication(m.applicationId.ToString()).ApplicationMethod,
                                nutrientUnit = _sd.GetUnit(m.unitId).Name
                            };
                            rf.nutrients.Add(rfn);
                        }
                    }
                    if (f.Nutrients.nutrientFertilizers != null)
                    {
                        foreach (var ft in f.Nutrients.nutrientFertilizers)
                        {
                            var rfn = new ReportFieldNutrient();
                            FertilizerType ftyp = _sd.GetFertilizerType(ft.fertilizerTypeId.ToString());

                            string fertilizerName;
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
                if (rf.nutrients.Count() == 0)
                {
                    var rfn = new ReportFieldNutrient
                    {
                        nutrientName = "None planned",
                        nutrientAmount = ""
                    };
                    rf.nutrients.Add(rfn);
                }
                if (f.Crops != null)
                {
                    foreach (var c in f.Crops)
                    {
                        string crpName = string.IsNullOrEmpty(c.cropOther) ? _sd.GetCrop(Convert.ToInt32(c.cropId)).CropName : c.cropOther;
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

            var reportManureCompostInventory = string.Empty;
            var reportManureUse = string.Empty;
            var reportFertilizers = string.Empty;

            Parallel.Invoke(
                async () => { reportManureCompostInventory = await RenderManureCompostInventory(); },
                async () => { reportManureUse = await RenderManureUse(); },
                async () => { reportFertilizers = await RenderFerilizers(); }
            );

            string report = reportManureCompostInventory + pageBreak + reportManureUse + pageBreak + reportFertilizers;

            result = await PrintReportAsync(report, true);

            return result;
        }

        public async Task<IActionResult> PrintSources()
        {
            string reportSources = await RenderSources();

            FileContentResult result = await PrintReportAsync(reportSources, true);
            return result;
        }

        public async Task<IActionResult> GenerateRecordKeepingSheets()
        {
            string reportSheets = await RenderSheets();
            _ud.SaveRecordKeepingSheets(reportSheets);

            return Json(new { success = true });
        }

        public async Task<IActionResult> PrintSheets()
        {
            string reportSheets = _ud.GetRecordKeepingSheets();

            FileContentResult result = await PrintReportAsync(reportSheets, false);
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

        [HttpGet]
        public IActionResult GenerateCompleteReport()
        {
            string pageBreak = "<div style=\"page-break-after:always;\">&nbsp;</div>";
            string pageBreakForManure = "<div>&nbsp;&nbsp;&nbsp;&nbsp;<br/><br/><br/><br/><br/><br/><br/><br/><br/></div>";
            bool hasFertilizers = false;
            bool hasSoilTests = false;

            var reportTableOfContents = string.Empty;
            var reportApplication = string.Empty;
            var reportManureCompostInventory = string.Empty;
            var reportManureUse = string.Empty;
            var reportOctoberToMarchStorageVolumes = string.Empty;
            var reportCropManure = string.Empty;
            var reportBeefManureUse = string.Empty;
            var reportFertilizers = string.Empty;
            var reportFields = string.Empty;
            var reportAnalysis = string.Empty;
            var reportSummary = string.Empty;
            var reportFeedingArea = string.Empty;

            Parallel.Invoke(
                //async () => { reportTableOfContents = await RenderTableOfContents(hasFertilizers, hasSoilTests); },
                async () =>
                {
                    //use AgriConfigurationRepostiory and since EF is not threadsafe, they need to
                    //run in sequence and not in parallel
                    reportApplication = await RenderApplication();
                    reportFertilizers = await RenderFerilizers();
                    reportFeedingArea = await RenderSeasonalFeedAreaSummary();
                    reportFields = await RenderFields();
                    reportManureUse = await RenderManureUse();
                    reportCropManure = await RenderCropManureUse();
                    reportBeefManureUse = await RenderBeefManureUse();
                    reportSummary = await RenderSummary();
                    reportAnalysis = await RenderAnalysis();
                },
                async () => { reportOctoberToMarchStorageVolumes = await RenderOctoberToMarchStorageVolumes(); }
            );

            if (reportFertilizers.Contains("div"))
            {
                hasFertilizers = true;
            }
            if (reportSummary.Contains("div"))
            {
                hasSoilTests = true;
            }

            Parallel.Invoke(async () =>
            {
                reportTableOfContents = await RenderTableOfContents(hasFertilizers, hasSoilTests);
            });

            string report = reportTableOfContents;

            //insert disclaimer right after the table of contents
            report += "<br/><br/><div></div><div></div><div style=\"float:left\">" + _sd.GetUserPrompt("disclaimer") + "</div>";

            if (reportApplication.Contains("div"))
            {
                report += pageBreak;
                report += reportApplication;
            }

            if (reportManureCompostInventory.Contains("div"))
            {
                report += pageBreak;
                report += reportManureCompostInventory;
            }

            if (reportManureUse.Contains("div"))
            {
                report += pageBreakForManure;
                report += reportManureUse;
            }

            if (reportBeefManureUse.Contains("div"))
            {
                report += pageBreakForManure;
                report += reportBeefManureUse;
            }

            if (reportOctoberToMarchStorageVolumes.Contains("div"))
            {
                report += pageBreakForManure;
                report += reportOctoberToMarchStorageVolumes;
            }

            if (reportCropManure.Contains("div"))
            {
                report += pageBreakForManure;
                report += reportCropManure;
            }
            if (reportFertilizers.Contains("div"))
            {
                report += pageBreakForManure;
                report += reportFertilizers;
            }

            if (reportFields.Contains("div"))
            {
                report += pageBreak;
                report += reportFields;
            }

            if (reportAnalysis.Contains("div"))
            {
                report += pageBreak;
                report += reportAnalysis;
            }

            if (reportFeedingArea.Contains("div"))
            {
                report += pageBreak;
                report += reportFeedingArea;
            }

            if (reportSummary.Contains("div"))
            {
                report += pageBreak;
                report += reportSummary;
            }

            _ud.SaveCompleteReport(report);
            return Json(new { success = true });

            //return report;
        }

        public async Task<IActionResult> PrintComplete()
        {
            FileContentResult result = null;
            var report = _ud.GetCompleteReport();

            result = await PrintReportAsync(report, true);

            return result;
        }

        public async Task<FileContentResult> PrintReportAsync(string content, bool portrait)
        {
            var pdfHost = Environment.GetEnvironmentVariable("WEASYPRINT_URL");
            string targetUrl = $"{pdfHost}/pdf";
            FileContentResult result;

            // call weasy print
            try
            {
                string reportHeader = await RenderHeader();
                string rawdata = "<!DOCTYPE html>" +
                    "<html>" +
                    reportHeader +
                    "<body>" +
                    content +
                    "</body></html>";

                //string rawdata = "<!DOCTYPE html>" +
                //    @"<html lang=""en"">" +
                //    reportHeader +
                //    @"<body><table>
                //    <thead><tr><th><div class=""header-space"">&nbsp;</div></th><tr></thead>
                //    <tbody><tr><td><div>" + content + @"</div></td></tr></tbody>
                //    <tfoot><tr><td><div class=""footer-space"">&nbsp;</div></td></tr></tfoot>
                //    </table>
                //    </body></html>";
                //@"<body>
                //        <table>
                //            <tr><td><div class=""content"">" + content + @"</div></td></tr>
                //        </table>
                //    </body></html>";

                //@"<body>
                //            <table>
                //                <thead><tr><td><div class=""header-space"">&nbsp;</div></td ><tr></thead>
                //                <tbody><tr><td><div class=""content"">" + content + @"</div></td></tr></tbody>
                //                <tfoot><tr><td><div class=""footer-space"">&nbsp;</div></td></tr></tfoot>
                //            </table>
                //        </body>
                //    </html>";

                //string rawdata = "<!DOCTYPE html>" +
                //    "<html>" +
                //        reportHeader +
                //        "<body>" +
                //    @"<table>
                //            <thead>
                //                <tr><td>
                //                    <div class=""header-space"">&nbsp;</div>
                //                </td ><tr>
                //            </thead>
                //            <tbody>
                //                <tr><td>
                //                    <div class=""content"">" + content + @"</div>
                //                </td></tr>
                //            </tbody>
                //            <tfoot><tr><td>
                //            <div class=""footer-space"">&nbsp;</div>
                //            </td></tr></tfoot>
                //        </table>" +
                //    "</body></html>";

                string payload = rawdata;

                var request = new HttpRequestMessage(HttpMethod.Post, targetUrl)
                {
                    Content = new StringContent(payload, Encoding.UTF8, "text/html")
                };

                request.Headers.Clear();

                var client = new HttpClient();
                var response = await client.SendAsync(request);

                ViewBag.StatusCode = response.StatusCode.ToString();

                if (response.StatusCode == HttpStatusCode.OK) // success
                {
                    var bytetask = response.Content.ReadAsByteArrayAsync();
                    bytetask.Wait();

                    result = new FileContentResult(bytetask.Result, "application/pdf");
                }
                else
                {
                    string errorMsg = "Url: " + targetUrl + "\r\n" + "Result: " + response.ToString();
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

        private string GetHeader(string page, string pages)
        {
            var header =
                "<div><span style=\"float: left; font-size:14px\">Farm Name: {0}<br />Planning Year: {1}</span></div>" +
                "<div style=\"float:right; vertical-align:top; text-align: right\">" +
                "<span style=\"color: #444;\">Page {{page}}</span>/<span>{{pages}}</span><br />Printed: {2}</div>";

            header = string.Format(header, _ud.FarmDetails().FarmName, _ud.FarmDetails().Year, DateTime.Now.ToShortDateString());

            return header;
        }

        private string GetFooter()
        {
            return $"<div></div><div style=\"float:right\">Version {_sd.GetStaticDataVersion()}</div>";
        }
    }
}