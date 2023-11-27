using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Agri.CalculateService;
using Agri.Data;
using Agri.Models;
using Agri.Models.Calculate;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Agri.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;

namespace SERVERAPI.Controllers
{
    public class CropsController : Controller
    {
        private readonly ILogger<NutrientsController> _logger;
        private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;
        private readonly AppSettings _settings;
        private readonly ICalculateCropRequirementRemoval _calculateCropRequirementRemoval;
        private readonly bool _showBlueberries;

        public CropsController(ILogger<NutrientsController> logger,
            UserData ud,
            IOptions<AppSettings> settings,
            IAgriConfigurationRepository sd,
            ICalculateCropRequirementRemoval calculateCropRequirementRemoval)
        {
            _logger = logger;
            _ud = ud;
            _sd = sd;
            _settings = settings.Value;
            _calculateCropRequirementRemoval = calculateCropRequirementRemoval;
            _showBlueberries = _ud.FarmDetails().UserJourney == UserJourney.Berries;
        }

        public IActionResult Crops()
        {
            return View();
        }

        public IActionResult CropDetails(string fldName, int? id)
        {
            var cvm = new CropDetailsViewModel
            {
                fieldName = fldName,
                title = id == null ? "Add" : "Edit",
                btnText = id == null ? "Calculate" : "Return",
                id = id,
                stdCrude = true,
                stdYield = true,
                nCredit = "0",
                nCreditLabel = _sd.GetUserPrompt("ncreditlabel"),
                showBlueberries = _showBlueberries
            };

            if (id != null)
            {
                var cp = _ud.GetFieldCrop(fldName, id.Value);

                cvm.fieldName = fldName;
                cvm.id = id;
                cvm.reqN = cp.reqN.ToString("G29");
                cvm.reqP2o5 = cp.reqP2o5.ToString("G29");
                cvm.reqK2o = cp.reqK2o.ToString("G29");
                cvm.remN = cp.remN.ToString("G29");
                cvm.remP2o5 = cp.remP2o5.ToString("G29");
                cvm.remK2o = cp.remK2o.ToString("G29");

                cvm.crude = cp.crudeProtien.ToString().Replace(".0", "");
                cvm.selCropOption = cp.cropId;
                cvm.selPrevOption = cp.prevCropId.ToString();
                cvm.coverCropHarvested = cp.coverCropHarvested;
                cvm.nCredit = cvm.selPrevOption != "0" ? _sd.GetPrevCropType(Convert.ToInt32(cvm.selPrevOption)).NitrogenCreditImperial.ToString() : "0";
                //E07US18
                cvm.showHarvestUnitsDDL = false;

                if (!cp.yieldHarvestUnit.HasValue)
                {   // retrofit old version of data
                    cp.yieldHarvestUnit = _sd.GetHarvestYieldDefaultDisplayUnit();
                    cvm.selHarvestUnits = cp.yieldHarvestUnit.ToString();
                    cp.yieldByHarvestUnit = cp.yield; // retrofit old version of data
                    cvm.yield = cp.yield.ToString();
                    cvm.yieldByHarvestUnit = cp.yieldByHarvestUnit.ToString("#.##");
                }
                else
                {
                    cvm.selHarvestUnits = cp.yieldHarvestUnit.ToString();
                    cvm.yieldByHarvestUnit = cp.yieldByHarvestUnit.ToString("#.##");
                    cvm.yield = cp.yield.ToString("#.##");
                }

                decimal? defaultYield = _calculateCropRequirementRemoval.GetDefaultYieldByCropId(_ud.FarmDetails(), Convert.ToInt16(cvm.selCropOption), cp.yieldHarvestUnit != _sd.GetHarvestYieldDefaultUnit());
                cvm.stdYield = true;
                if (defaultYield.HasValue)
                {   // E07US18
                    if (cvm.yieldByHarvestUnit != defaultYield.Value.ToString("#.##"))
                    {
                        cvm.stdYield = false;
                    }
                }

                if (!string.IsNullOrEmpty(cp.cropOther))
                {
                    cvm.manEntry = true;
                    var yld = _sd.GetYieldById(1);
                    cvm.cropDesc = cp.cropOther;
                    cvm.yieldUnit = "(" + yld.YieldDesc + ")";
                    cvm.selTypOption = _settings.OtherCropId;
                }
                else
                {
                    var crop = _sd.GetCrop(Convert.ToInt32(cp.cropId));
                    var yld = _sd.GetYieldById(crop.YieldCd);
                    cvm.yieldUnit = "(" + yld.YieldDesc + ")";
                    cvm.selTypOption = crop.CropTypeId.ToString();
                    //E07US18
                    cvm.showHarvestUnitsDDL = _sd.IsCropGrainsAndOilseeds(Convert.ToInt16(crop.CropTypeId));

                    CropType crpTyp = _sd.GetCropType(Convert.ToInt32(cvm.selTypOption));
                    if (crpTyp.ModifyNitrogen)
                    {
                        cvm.modNitrogen = true;

                        // check for standard
                        var yield = cvm.showHarvestUnitsDDL && (cp.yieldHarvestUnit != _sd.GetHarvestYieldDefaultUnit()) ?
                                _sd.ConvertYieldFromBushelToTonsPerAcre(Convert.ToInt16(cvm.selCropOption), Convert.ToDecimal(cvm.yieldByHarvestUnit)) :
                                Convert.ToDecimal(cvm.yieldByHarvestUnit);

                        var cropRequirementRemoval = _calculateCropRequirementRemoval
                            .GetCropRequirementRemoval(Convert.ToInt16(cvm.selCropOption),
                            yield,
                            string.IsNullOrEmpty(cvm.crude) ? default(decimal?) : Convert.ToDecimal(cvm.crude),
                            cvm.coverCropHarvested,
                            !string.IsNullOrEmpty(cvm.nCredit) ? Convert.ToInt16(cvm.nCredit) : 0,
                            _ud.FarmDetails().FarmRegion.Value,
                            _ud.GetFieldDetails(fldName));

                        cvm.stdNAmt = cropRequirementRemoval.N_Requirement.ToString();

                        cvm.stdN = (cvm.reqN == cropRequirementRemoval.N_Requirement.ToString()) ? true : false;
                    }
                }

                CropDetailsSetup(ref cvm);
                PopulateYield(ref cvm);

                if( _showBlueberries)
                {

                    cvm.selPlantAgeYears = cvm.plantAgeYears.Where( item => item.Value == cp.plantAgeYears)
                                                            .Select(field => field.Id.ToString()).FirstOrDefault();

                    cvm.selNumberOfPlantsPerAcre = cvm.numberOfPlantsPerAcre.Where(item => item.Value == cp.numberOfPlantsPerAcre.ToString())
                                                                 .Select(field => field.Id.ToString()).FirstOrDefault();

                    cvm.selDistanceBtwnPlantsRows = cvm.distanceBtwnPlantsRows.Where(item => item.Value == cp.distanceBtwnPlantsRows)
                                                                 .Select(field => field.Id.ToString()).FirstOrDefault();

                    cvm.selWillPlantsBePruned = cvm.willPlantsBePruned.Where(item => item.Value == ((cp.willPlantsBePruned ?? false) ? "Yes" : "No"))
                                                                 .Select(field => field.Id.ToString()).FirstOrDefault();

                    cvm.selWhereWillPruningsGo = cvm.whereWillPruningsGo.Where(item => item.Value == cp.whereWillPruningsGo)
                                                                 .Select(field => field.Id.ToString()).FirstOrDefault();

                    cvm.selWillSawdustBeApplied = cvm.willSawdustBeApplied.Where(item => item.Value == ((cp.willSawdustBeApplied ?? false) ? "Yes" : "No"))
                                                                           .Select(field => field.Id.ToString()).FirstOrDefault();

                }

                if (!cvm.manEntry)
                {
                    if (cvm.showCrude)
                    {
                        if (cvm.crude.Replace(".0", "") != _calculateCropRequirementRemoval.GetCrudeProtienByCropId(Convert.ToInt16(cvm.selCropOption)).ToString("#.#"))
                        {
                            cvm.stdCrude = false;
                        }
                    }
                }
            }
            else
            {
                CropDetailsReset(ref cvm);

                CropDetailsSetup(ref cvm);
                PopulateYield(ref cvm);
             
            }

            return PartialView(cvm);
        }

        [HttpPost]
        public IActionResult CropDetails(CropDetailsViewModel cvm)
        {
            CropDetailsSetup(ref cvm);
            try
            {
                if (cvm.buttonPressed == "NumberOfPlantsPerAcreChange")
                {
                    ModelState.Clear();
                    cvm.selDistanceBtwnPlantsRows = cvm.selNumberOfPlantsPerAcre;
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";
                    return View(cvm);
                }

                if (cvm.buttonPressed == "DistanceBtwnPlantsRowsChange")
                {
                    ModelState.Clear();
                    cvm.selNumberOfPlantsPerAcre = cvm.selDistanceBtwnPlantsRows;
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";
                    return View(cvm);
                }

                if (cvm.buttonPressed == "TypeChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";
                    cvm.crude = "";
                    cvm.cropDesc = "";
                    cvm.coverCropHarvested = null;
                    cvm.nCredit = "0";
                    cvm.stdYield = true;
                    cvm.yield = "";
                    cvm.yieldByHarvestUnit = "";
                    cvm.prevOptions = new List<SelectListItem>();

                    if (cvm.selTypOption != "select")
                    {
                        CropType crpTyp = _sd.GetCropType(Convert.ToInt32(cvm.selTypOption));

                        if (crpTyp.CustomCrop)
                        {
                            cvm.manEntry = true;
                            cvm.reqN = string.Empty;
                            cvm.reqP2o5 = string.Empty;
                            cvm.reqK2o = string.Empty;
                            cvm.remN = string.Empty;
                            cvm.remP2o5 = string.Empty;
                            cvm.remK2o = string.Empty;
                            cvm.showHarvestUnitsDDL = false;
                            cvm.selCropOption = "66";
                        }
                        else
                        {
                            if (_sd.IsCropGrainsAndOilseeds(crpTyp.Id))
                            {
                                cvm.showHarvestUnitsDDL = true;
                                cvm.selHarvestUnits = _sd.GetHarvestYieldDefaultDisplayUnit().ToString();
                            }
                            else
                                cvm.showHarvestUnitsDDL = false;
                            cvm.manEntry = false;
                            CropDetailsReset(ref cvm);
                        }
                    }
                    else
                    {
                        cvm.manEntry = false;
                        cvm.showHarvestUnitsDDL = false;
                        CropDetailsReset(ref cvm);
                    }
                    PopulateYield(ref cvm);
                    return View(cvm);
                }

                if (cvm.buttonPressed == "PrevChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";
                    if (cvm.selPrevOption != "select" &&
                        cvm.selPrevOption != "")
                    {
                        cvm.nCredit = _sd.GetPrevCropType(Convert.ToInt32(cvm.selPrevOption)).NitrogenCreditImperial.ToString();
                    }
                    else
                    {
                        cvm.nCredit = "0";
                    }
                    CropDetailsReset(ref cvm);

                    return View(cvm);
                }

                if (cvm.buttonPressed == "ResetCrude")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";

                    cvm.crude = _calculateCropRequirementRemoval.GetCrudeProtienByCropId(Convert.ToInt16(cvm.selCropOption)).ToString("#.#");

                    cvm.stdCrude = true;
                    return View(cvm);
                }

                if (cvm.buttonPressed == "ResetN")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";

                    cvm.reqN = cvm.stdNAmt;

                    cvm.stdN = true;
                    return View(cvm);
                }

                if (cvm.buttonPressed == "ResetYield")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";

                    decimal? defaultYield;
                    // E07US18 - convert defaultYield to bu/ac if required
                    if (cvm.showHarvestUnitsDDL)
                        defaultYield = _calculateCropRequirementRemoval.GetDefaultYieldByCropId(_ud.FarmDetails(), Convert.ToInt16(cvm.selCropOption), cvm.selHarvestUnits != _sd.GetHarvestYieldDefaultUnit().ToString());
                    else
                        defaultYield = _calculateCropRequirementRemoval.GetDefaultYieldByCropId(_ud.FarmDetails(), Convert.ToInt16(cvm.selCropOption), false);
                    if (defaultYield.HasValue)
                        cvm.yieldByHarvestUnit = defaultYield.Value.ToString("#.##");

                    cvm.reqN = cvm.stdNAmt;

                    cvm.stdYield = true;
                    return View(cvm);
                }

                if (cvm.buttonPressed == "CropChange")
                {
                    PopulateYield(ref cvm);
                    return View(cvm);
                }

                if (cvm.buttonPressed == "HarvestUnitChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";
                    cvm.btnText = "Calculate";

                    if (Convert.ToInt32(cvm.selCropOption) > 0)
                    {
                        Crop crop = _sd.GetCrop(Convert.ToInt32(cvm.selCropOption));

                        if (cvm.selHarvestUnits == _sd.GetHarvestYieldDefaultUnit().ToString())
                        {
                            if (crop.HarvestBushelsPerTon.HasValue)
                                cvm.yieldByHarvestUnit = (Convert.ToDecimal(cvm.yieldByHarvestUnit) / Convert.ToDecimal(crop.HarvestBushelsPerTon)).ToString("#.##");
                        }
                        else
                        {
                            if (crop.HarvestBushelsPerTon.HasValue)
                                cvm.yieldByHarvestUnit = (Convert.ToDecimal(cvm.yieldByHarvestUnit) * Convert.ToDecimal(crop.HarvestBushelsPerTon)).ToString("#.##");
                        }
                    }
                    return View(cvm);
                }

                if (ModelState.IsValid)
                {
                    if (cvm.coverCrop)
                    {
                        if (!cvm.coverCropHarvested.HasValue)
                        {
                            ModelState.AddModelError("coverCropHarvested", "Required.");
                            return View(cvm);
                        }
                    }

                    if (!string.IsNullOrEmpty(cvm.crude))
                    {
                        decimal crd;
                        if (decimal.TryParse(cvm.crude, out crd))
                        {
                            if (crd < 0 || crd > 100)
                            {
                                ModelState.AddModelError("crude", "Not a valid percentage.");
                                return View(cvm);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("crude", "Not a valid number.");
                            return View(cvm);
                        }
                    }

                    decimal tmpDec;
                    if (decimal.TryParse(cvm.yieldByHarvestUnit, out tmpDec))
                    {
                        if (tmpDec <= 0 ||
                            tmpDec > 99999)
                        {
                            ModelState.AddModelError("yieldByHarvestUnit", "Not a valid yield.");
                            return View(cvm);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("yieldByHarvestUnit", "Not a valid number.");
                        return View(cvm);
                    }

                    //if((string.IsNullOrEmpty(cvm.selCropOption) ||
                    //    cvm.selCropOption == "select") &&
                    //    cvm.selTypOption != _settings.OtherCropId)  // none
                    //{
                    //    ModelState.AddModelError("selCropOption", "Required.");
                    //    return View(cvm);
                    //}

                    if (cvm.manEntry)
                    {
                        if (string.IsNullOrEmpty(cvm.cropDesc))
                        {
                            ModelState.AddModelError("cropDesc", "Required.");
                            return View(cvm);
                        }

                        if (string.IsNullOrEmpty(cvm.reqN))
                        {
                            ModelState.AddModelError("reqN", "Reqd.");
                            return View(cvm);
                        }
                        else
                        {
                            if (decimal.TryParse(cvm.reqN, out tmpDec))
                            {
                                if (tmpDec < 0 ||
                                    tmpDec > 1000)
                                {
                                    ModelState.AddModelError("reqN", "Invalid.");
                                    return View(cvm);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("reqN", "Invalid.");
                                return View(cvm);
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.reqP2o5))
                        {
                            ModelState.AddModelError("reqP2o5", "Reqd.");
                            return View(cvm);
                        }
                        else
                        {
                            if (decimal.TryParse(cvm.reqP2o5, out tmpDec))
                            {
                                if (tmpDec < 0 ||
                                    tmpDec > 1000)
                                {
                                    ModelState.AddModelError("reqP2o5", "Invalid.");
                                    return View(cvm);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("reqP2o5", "Invalid.");
                                return View(cvm);
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.reqK2o))
                        {
                            ModelState.AddModelError("reqK2o", "Reqd.");
                            return View(cvm);
                        }
                        else
                        {
                            if (decimal.TryParse(cvm.reqK2o, out tmpDec))
                            {
                                if (tmpDec < 0 ||
                                    tmpDec > 1000)
                                {
                                    ModelState.AddModelError("reqK2o", "Invalid.");
                                    return View(cvm);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("reqK2o", "Invalid.");
                                return View(cvm);
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.remN))
                        {
                            ModelState.AddModelError("remN", "Reqd.");
                            return View(cvm);
                        }
                        else
                        {
                            if (decimal.TryParse(cvm.remN, out tmpDec))
                            {
                                if (tmpDec < 0 ||
                                    tmpDec > 1000)
                                {
                                    ModelState.AddModelError("remN", "Invalid.");
                                    return View(cvm);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("remN", "Invalid.");
                                return View(cvm);
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.remP2o5))
                        {
                            ModelState.AddModelError("remP2o5", "Reqd.");
                            return View(cvm);
                        }
                        else
                        {
                            if (decimal.TryParse(cvm.remP2o5, out tmpDec))
                            {
                                if (tmpDec < 0 ||
                                    tmpDec > 1000)
                                {
                                    ModelState.AddModelError("remP2o5", "Invalid.");
                                    return View(cvm);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("remP2o5", "Invalid.");
                                return View(cvm);
                            }
                        }
                        if (string.IsNullOrEmpty(cvm.remK2o))
                        {
                            ModelState.AddModelError("remK2o", "Reqd.");
                            return View(cvm);
                        }
                        else
                        {
                            if (decimal.TryParse(cvm.remK2o, out tmpDec))
                            {
                                if (tmpDec < 0 ||
                                    tmpDec > 1000)
                                {
                                    ModelState.AddModelError("remK2o", "Invalid.");
                                    return View(cvm);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("remK2o", "Invalid.");
                                return View(cvm);
                            }
                        }
                    }

                    if (cvm.modNitrogen)
                    {
                        if (decimal.TryParse(cvm.reqN, out tmpDec))
                        {
                            if (tmpDec < 0 ||
                                tmpDec > 1000)
                            {
                                ModelState.AddModelError("reqN", "Not a valid amount.");
                                return View(cvm);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("reqN", "Not a valid number.");
                            return View(cvm);
                        }
                    }

                    if (cvm.btnText == "Calculate")
                    {
                        ModelState.Clear();
                        if (!cvm.manEntry)
                        {
                            if (_showBlueberries)
                            {
                                bool hasValidationErrors = false;

                                if (cvm.selPlantAgeYears == "select")
                                {
                                    ModelState.AddModelError("selPlantAgeYears", "Required field");
                                    hasValidationErrors = true;
                                }
                                if (cvm.selNumberOfPlantsPerAcre == "select")
                                {
                                    ModelState.AddModelError("selNumberOfPlantsPerAcre", "Required field");
                                    hasValidationErrors = true;
                                }
                                if (cvm.selDistanceBtwnPlantsRows == "select")
                                {
                                    ModelState.AddModelError("selDistanceBtwnPlantsRows", "Required field");
                                    hasValidationErrors = true;
                                }
                                if (cvm.selWillPlantsBePruned == "select")
                                {
                                    ModelState.AddModelError("selWillPlantsBePruned", "Required field");
                                    hasValidationErrors = true;
                                }
                                if (cvm.selWhereWillPruningsGo == "select")
                                {
                                    ModelState.AddModelError("selWhereWillPruningsGo", "Required field");
                                    hasValidationErrors = true;
                                }
                                if (cvm.selWillSawdustBeApplied == "select")
                                {
                                    ModelState.AddModelError("selWillSawdustBeApplied", "Required field");
                                    hasValidationErrors = true;
                                }
                                if (hasValidationErrors)
                                {
                                    return View(cvm);
                                }
                            }

                            // E07US18 - need to convert cvm.yield to tons/acre before passing to calculateCrop
                            var yield = cvm.showHarvestUnitsDDL && !_sd.IsCropHarvestYieldDefaultUnit(Convert.ToInt16(cvm.selHarvestUnits)) ?
                                    _sd.ConvertYieldFromBushelToTonsPerAcre(Convert.ToInt16(cvm.selCropOption), Convert.ToDecimal(cvm.yieldByHarvestUnit)) :
                                    Convert.ToDecimal(cvm.yieldByHarvestUnit);

                            cvm.yield = yield.ToString();

                            CropRequirementRemoval cropRequirementRemoval;
                            if (_showBlueberries)
                            {
                                var fld = _ud.GetFieldDetails(cvm.fieldName);
                                string plantAgeYears = cvm.plantAgeYears.Where(item => item.Id.ToString() == cvm.selPlantAgeYears)
                                                                 .Select(field => field.Value).FirstOrDefault();
                                int? numberOfPlantsPerAcre = cvm.numberOfPlantsPerAcre.Where(item => item.Id.ToString() == cvm.selNumberOfPlantsPerAcre)
                                                                 .Select(field => Convert.ToInt16(field.Value)).FirstOrDefault();
                                bool? willPlantsBePruned = cvm.willPlantsBePruned.Where(item => item.Id.ToString() == cvm.selWillPlantsBePruned)
                                                                 .Select(field => field.Value == "Yes").FirstOrDefault();
                                string whereWillPruningsGo = cvm.whereWillPruningsGo.Where(item => item.Id.ToString() == cvm.selWhereWillPruningsGo)
                                                                 .Select(field => field.Value).FirstOrDefault();
                                bool willSawdustBeApplied = cvm.willSawdustBeApplied.Where(item => item.Id.ToString() == cvm.selWillSawdustBeApplied)
                                                                 .Select(field => field.Value == "Yes").FirstOrDefault();


                                if ( fld.SoilTest != null &&
                                    fld.SoilTest.ValP != 0 &&
                                    fld.LeafTest != null &&
                                    !String.IsNullOrEmpty(fld.LeafTest.leafTissueP) &&
                                    !String.IsNullOrEmpty(fld.LeafTest.leafTissueK) 
                                   )
                                {
                                    cropRequirementRemoval = _calculateCropRequirementRemoval
                                                                    .GetCropRequirementRemovalBlueberries(
                                                                            Convert.ToDecimal(cvm.yieldByHarvestUnit),
                                                                            plantAgeYears,
                                                                            numberOfPlantsPerAcre,
                                                                            willSawdustBeApplied,
                                                                            willPlantsBePruned,
                                                                            whereWillPruningsGo,
                                                                            fld.SoilTest.ValP,
                                                                            fld.LeafTest.leafTissueP,
                                                                            fld.LeafTest.leafTissueK);
                                }
                                else
                                {
                                    cropRequirementRemoval = new CropRequirementRemoval
                                    {
                                        N_Requirement = 0,
                                        P2O5_Requirement = 0,
                                        K2O_Requirement = 0,
                                        N_Removal = 0,
                                        P2O5_Removal = 0,
                                        K2O_Removal = 0
                                    };
                                }
                            }
                            else
                            {
                                cropRequirementRemoval = _calculateCropRequirementRemoval
                                    .GetCropRequirementRemoval(Convert.ToInt16(cvm.selCropOption),
                                    yield,
                                    string.IsNullOrEmpty(cvm.crude) ? default(decimal?) : Convert.ToDecimal(cvm.crude),
                                    cvm.coverCropHarvested,
                                    !string.IsNullOrEmpty(cvm.nCredit) ? Convert.ToInt16(cvm.nCredit) : 0,
                                    _ud.FarmDetails().FarmRegion.Value,
                                    _ud.GetFieldDetails(cvm.fieldName)
                                );
                            }

                            if (!cvm.modNitrogen)
                            {
                                cvm.reqN = cropRequirementRemoval.N_Requirement.ToString();
                            }
                            else
                            {
                                if (cvm.reqN != cropRequirementRemoval.N_Requirement.ToString())
                                {
                                    cvm.stdN = false;
                                }
                            }
                            cvm.stdNAmt = cropRequirementRemoval.N_Requirement.ToString();
                            cvm.reqP2o5 = cropRequirementRemoval.P2O5_Requirement.ToString();
                            cvm.reqK2o = cropRequirementRemoval.K2O_Requirement.ToString();
                            cvm.remN = cropRequirementRemoval.N_Removal.ToString();
                            cvm.remP2o5 = cropRequirementRemoval.P2O5_Removal.ToString();
                            cvm.remK2o = cropRequirementRemoval.K2O_Removal.ToString();
                            if (cvm.crude != null)
                            {
                                if (cvm.crude.Replace(".0", "") != _calculateCropRequirementRemoval.GetCrudeProtienByCropId(Convert.ToInt16(cvm.selCropOption)).ToString("#.#"))
                                {
                                    cvm.stdCrude = false;
                                }
                            }

                            if (!cvm.modNitrogen)
                            {
                                CropType crpTyp = _sd.GetCropType(Convert.ToInt32(cvm.selTypOption));
                                if (crpTyp.ModifyNitrogen)
                                {
                                    cvm.modNitrogen = true;
                                    cvm.stdN = true;
                                }
                            }

                            decimal? defaultYield;
                            if (cvm.showHarvestUnitsDDL)
                                defaultYield = _calculateCropRequirementRemoval.GetDefaultYieldByCropId(_ud.FarmDetails(), Convert.ToInt16(cvm.selCropOption), cvm.selHarvestUnits != _sd.GetHarvestYieldDefaultUnit().ToString());
                            else
                                defaultYield = _calculateCropRequirementRemoval.GetDefaultYieldByCropId(_ud.FarmDetails(), Convert.ToInt16(cvm.selCropOption), false);
                            cvm.stdYield = true;
                            if (defaultYield.HasValue)
                            {
                                if (cvm.yieldByHarvestUnit != defaultYield.Value.ToString("#.##"))
                                {
                                    cvm.stdYield = false;
                                }
                            }
                        }

                        cvm.btnText = cvm.id == null ? "Add to Field" : "Update Field";
                    }
                    else
                    {
                        if (cvm.id == null)
                        {
                            int prevCrop = 0;
                            if (cvm.selPrevOption != "select")
                                prevCrop = Convert.ToInt32(cvm.selPrevOption);

                            int thisCrop = 0;
                            if (cvm.selCropOption != "select")
                                thisCrop = Convert.ToInt32(cvm.selCropOption);
                            // E07US18 - convert cvm.yield
                            FieldCrop crp = new FieldCrop()
                            {
                                cropId = thisCrop.ToString(),
                                cropOther = cvm.cropDesc,
                                yield = Convert.ToDecimal(cvm.yieldByHarvestUnit),
                                yieldByHarvestUnit = Convert.ToDecimal(cvm.yieldByHarvestUnit),
                                reqN = Convert.ToDecimal(cvm.reqN),
                                reqP2o5 = Convert.ToDecimal(cvm.reqP2o5),
                                reqK2o = Convert.ToDecimal(cvm.reqK2o),
                                remN = Convert.ToDecimal(cvm.remN),
                                remP2o5 = Convert.ToDecimal(cvm.remP2o5),
                                remK2o = Convert.ToDecimal(cvm.remK2o),
                                crudeProtien = string.IsNullOrEmpty(cvm.crude) ? (decimal?)null : Convert.ToDecimal(cvm.crude),
                                prevCropId = prevCrop,
                                coverCropHarvested = cvm.coverCropHarvested,
                                prevYearManureAppl_volCatCd = _sd.GetCropPrevYearManureApplVolCatCd(thisCrop),
                                yieldHarvestUnit = (cvm.showHarvestUnitsDDL) ? Convert.ToInt16(cvm.selHarvestUnits) : _sd.GetHarvestYieldDefaultUnit(),

                                plantAgeYears = cvm.plantAgeYears.Where(item => item.Id.ToString() == cvm.selPlantAgeYears)
                                                                 .Select(field => field.Value).FirstOrDefault(),
                                numberOfPlantsPerAcre = cvm.numberOfPlantsPerAcre.Where(item => item.Id.ToString() == cvm.selNumberOfPlantsPerAcre)
                                                                 .Select(field => Convert.ToInt16(field.Value)).FirstOrDefault(),
                                distanceBtwnPlantsRows = cvm.distanceBtwnPlantsRows.Where(item => item.Id.ToString() == cvm.selDistanceBtwnPlantsRows)
                                                                 .Select(field => field.Value).FirstOrDefault(),
                                willPlantsBePruned = cvm.willPlantsBePruned.Where(item => item.Id.ToString() == cvm.selWillPlantsBePruned)
                                                                 .Select(field => field.Value == "Yes").FirstOrDefault(),
                                whereWillPruningsGo = cvm.whereWillPruningsGo.Where(item => item.Id.ToString() == cvm.selWhereWillPruningsGo)
                                                                 .Select(field => field.Value).FirstOrDefault(),
                                willSawdustBeApplied = cvm.willSawdustBeApplied.Where(item => item.Id.ToString() == cvm.selWillSawdustBeApplied)
                                                                 .Select(field => field.Value == "Yes").FirstOrDefault()

                        };
                            if (cvm.showHarvestUnitsDDL && (cvm.selHarvestUnits != _sd.GetHarvestYieldDefaultUnit().ToString()))
                                crp.yield = _sd.ConvertYieldFromBushelToTonsPerAcre(Convert.ToInt16(crp.cropId), Convert.ToDecimal(cvm.yieldByHarvestUnit));

                            _ud.AddFieldCrop(cvm.fieldName, crp);

                            return Json(new { success = true, reload = true });
                        }
                        else
                        {
                            int prevCrop = 0;
                            if (cvm.selPrevOption != "select")
                                prevCrop = Convert.ToInt32(cvm.selPrevOption);

                            int thisCrop = 0;
                            if (cvm.selCropOption != "select")
                                thisCrop = Convert.ToInt32(cvm.selCropOption);

                            FieldCrop crp = _ud.GetFieldCrop(cvm.fieldName, cvm.id.Value);
                            crp.cropId = thisCrop.ToString();
                            crp.cropOther = cvm.cropDesc;
                            //E07US18 - need to convert cvm.yield to TONS/acre before assigin to crp.yield
                            crp.yieldByHarvestUnit = Convert.ToDecimal(cvm.yieldByHarvestUnit);
                            if (cvm.showHarvestUnitsDDL && (cvm.selHarvestUnits != _sd.GetHarvestYieldDefaultUnit().ToString()))
                                crp.yield = _sd.ConvertYieldFromBushelToTonsPerAcre(Convert.ToInt16(crp.cropId), Convert.ToDecimal(cvm.yieldByHarvestUnit));
                            else
                                crp.yield = crp.yieldByHarvestUnit;

                            crp.reqN = Convert.ToDecimal(cvm.reqN);
                            crp.reqP2o5 = Convert.ToDecimal(cvm.reqP2o5);
                            crp.reqK2o = Convert.ToDecimal(cvm.reqK2o);
                            crp.remN = Convert.ToDecimal(cvm.remN);
                            crp.remP2o5 = Convert.ToDecimal(cvm.remP2o5);
                            crp.remK2o = Convert.ToDecimal(cvm.remK2o);
                            crp.crudeProtien = string.IsNullOrEmpty(cvm.crude) ? (decimal?)null : Convert.ToDecimal(cvm.crude);
                            crp.prevCropId = prevCrop;
                            crp.coverCropHarvested = cvm.coverCropHarvested;
                            crp.prevYearManureAppl_volCatCd = _sd.GetCropPrevYearManureApplVolCatCd(Convert.ToInt32(crp.cropId));
                            crp.yieldHarvestUnit = (cvm.showHarvestUnitsDDL) ? Convert.ToInt16(cvm.selHarvestUnits) : _sd.GetHarvestYieldDefaultUnit();

                            crp.plantAgeYears = cvm.plantAgeYears.Where(item => item.Id.ToString() == cvm.selPlantAgeYears)
                                                                 .Select(field => field.Value).FirstOrDefault();
                            crp.numberOfPlantsPerAcre = cvm.numberOfPlantsPerAcre.Where(item => item.Id.ToString() == cvm.selNumberOfPlantsPerAcre)
                                                                 .Select(field => Convert.ToInt16(field.Value)).FirstOrDefault();
                            crp.distanceBtwnPlantsRows = cvm.distanceBtwnPlantsRows.Where(item => item.Id.ToString() == cvm.selDistanceBtwnPlantsRows)
                                                                 .Select(field => field.Value).FirstOrDefault();
                            crp.willPlantsBePruned = cvm.willPlantsBePruned.Where(item => item.Id.ToString() == cvm.selWillPlantsBePruned)
                                                                 .Select(field => field.Value == "Yes").FirstOrDefault();
                            crp.whereWillPruningsGo = cvm.whereWillPruningsGo.Where(item => item.Id.ToString() == cvm.selWhereWillPruningsGo)
                                                                 .Select(field => field.Value).FirstOrDefault();
                            crp.willSawdustBeApplied = cvm.willSawdustBeApplied.Where(item => item.Id.ToString() == cvm.selWillSawdustBeApplied)
                                                                 .Select(field => field.Value == "Yes").FirstOrDefault();

                            _ud.UpdateFieldCrop(cvm.fieldName, crp);

                            return Json(new { success = true, reload = true });
                        }
                        //NOTE: NOT SURE IF REQUIRED FOR STANDALONE CROPS
                        //return Json(ReDisplay("#crop", cvm.fieldName));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error -" + ex.Message);
            }

            return PartialView("CropDetails", cvm);
        }

        private void CropDetailsSetup(ref CropDetailsViewModel cvm)
        {
            cvm.showCrude = false;
            cvm.typOptions = new List<SelectListItem>();
            cvm.typOptions = _sd.GetCropTypesDll().ToList();

            cvm.cropOptions = new List<SelectListItem>();
            cvm.harvestUnitsOptions = new List<SelectListItem>();
            if (_showBlueberries)
            {
                cvm.selTypOption = "7";
            }
            if (!string.IsNullOrEmpty(cvm.selTypOption) &&
                cvm.selTypOption != "select")
            {
                cvm.cropOptions = _sd.GetCropsDll(Convert.ToInt32(cvm.selTypOption)).ToList();
                cvm.harvestUnitsOptions = _sd.GetCropHarvestUnitsDll().ToList();

                if (cvm.selTypOption != "select")
                {
                    CropType crpTyp = _sd.GetCropType(Convert.ToInt32(cvm.selTypOption));
                    cvm.showCrude = crpTyp.CrudeProteinRequired;
                    cvm.coverCrop = crpTyp.CoverCrop;
                    cvm.manEntry = crpTyp.CustomCrop;
                    if (!crpTyp.CustomCrop)
                    {
                        cvm.cropOptions.Insert(0, new SelectListItem() { Id = 0, Value = "select" });
                    }
                }
            }

            PreviousCropSetup(ref cvm);
            InitCrops(ref cvm);

            return;
        }

        private void CropDetailsReset(ref CropDetailsViewModel cvm)
        {
            cvm.reqN = "  0";
            cvm.reqP2o5 = "  0";
            cvm.reqK2o = "  0";
            cvm.remN = "  0";
            cvm.remP2o5 = "  0";
            cvm.remK2o = "  0";

            cvm.modNitrogen = false;
            cvm.stdN = true;

            return;
        }

        private void PreviousCropSetup(ref CropDetailsViewModel cvm)
        {
            cvm.prevOptions = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(cvm.selCropOption))
            {
                if (cvm.selCropOption != "select" &&
                    cvm.selCropOption != "0")
                {
                    Crop crp = _sd.GetCrop(Convert.ToInt32(cvm.selCropOption));
                    cvm.prevOptions = _sd.GetPrevCropTypesDll(crp.PreviousCropCode.ToString()).ToList();
                }
            }
        }

        [HttpGet]
        public ActionResult CropDelete(string fldName, int id)
        {
            var dvm = new CropDeleteViewModel
            {
                id = id,
                fldName = fldName
            };

            FieldCrop crp = _ud.GetFieldCrop(fldName, id);
            if (!string.IsNullOrEmpty(crp.cropOther))
                dvm.cropName = crp.cropOther;
            else
                dvm.cropName = _sd.GetCrop(Convert.ToInt32(crp.cropId)).CropName;

            dvm.act = "Delete";

            return PartialView("CropDelete", dvm);
        }

        [HttpPost]
        public ActionResult CropDelete(CropDeleteViewModel dvm)
        {
            if (ModelState.IsValid)
            {
                _ud.DeleteFieldCrop(dvm.fldName, dvm.id);
                return Json(new { success = true, reload = true });
            }
            return PartialView("CropDelete", dvm);
        }

        [HttpGet]
        public MissingCropViewModel MissingCrops()
        {
            var fieldList = _ud.GetFields();
            var journey = _ud.FarmDetails().UserJourney.ToString();

            var result = new MissingCropViewModel() { journey = journey, cropPresent = false };

            foreach (Field field in fieldList)
            {
                if (field.Crops != null && field.Crops.Any())
                {
                    result.cropPresent = true;
                    return result;
                }
            }
            return result;
        }
        public void PopulateYield(ref CropDetailsViewModel cvm)
        {
            if (cvm.selCropOption != null && cvm.selTypOption != null)
            {
                ModelState.Clear();
                cvm.buttonPressed = "";
                cvm.btnText = "Calculate";
                cvm.nCredit = "0";
                cvm.stdYield = true;

                PreviousCropSetup(ref cvm);
                CropDetailsReset(ref cvm);

                if (cvm.selCropOption != "" &&
                    cvm.selCropOption != "0" &&
                    cvm.selCropOption != "select")
                {
                    Crop cp = _sd.GetCrop(Convert.ToInt32(cvm.selCropOption));
                    Yield yld = _sd.GetYieldById(cp.YieldCd);

                    cvm.yieldUnit = "(" + yld.YieldDesc + ")";
                    // E07US18
                    if (cvm.showHarvestUnitsDDL)
                    {
                        cvm.harvestUnitsOptions = _sd.GetCropHarvestUnitsDll();
                        cvm.selHarvestUnits = _sd.GetHarvestYieldDefaultDisplayUnit().ToString();
                    }
                    if (cvm.showCrude)
                    {
                        cvm.crude = _calculateCropRequirementRemoval.GetCrudeProtienByCropId(Convert.ToInt16(cvm.selCropOption)).ToString("#.#");
                        cvm.stdCrude = true;
                    }

                    decimal? defaultYield;
                    // E07US18
                    if (cvm.showHarvestUnitsDDL)
                        defaultYield = _calculateCropRequirementRemoval.GetDefaultYieldByCropId(_ud.FarmDetails(), Convert.ToInt16(cvm.selCropOption), cvm.selHarvestUnits != _sd.GetHarvestYieldDefaultUnit().ToString());
                    else
                        defaultYield = _calculateCropRequirementRemoval.GetDefaultYieldByCropId(_ud.FarmDetails(), Convert.ToInt16(cvm.selCropOption), false);

                    if (defaultYield.HasValue)
                        cvm.yieldByHarvestUnit = defaultYield.Value.ToString("#.##");
                }
                cvm.selPrevOption = string.Empty;
            }
            return;
        }

        private void InitCrops(ref CropDetailsViewModel cvm)
        {
            if (_showBlueberries)
            {
                cvm.selTypOption = "7";
                cvm.typOptions.RemoveAll(item => item.Id != 7);

                cvm.selCropOption = "75";
            }
            else
            {
                cvm.typOptions.RemoveAll(item => item.Id == 7);
                cvm.cropOptions.RemoveAll(item => item.Id == 75);
            }

            cvm.plantAgeYears = _sd.GetPlantAgeYearsDll();
            cvm.numberOfPlantsPerAcre = _sd.GetNumberOfPlantsPerAcreDll();
            cvm.distanceBtwnPlantsRows = _sd.GetDistanceBtwnPlantsRowsDll();
            cvm.willPlantsBePruned = _sd.GetWillPlantsBePrunedDll();
            cvm.whereWillPruningsGo = _sd.GetWhereWillPruningsGoDll();
            cvm.willSawdustBeApplied = _sd.GetWillSawdustBeAppliedDll();

        }
    }
}