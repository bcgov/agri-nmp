using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Agri.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SERVERAPI.Filters;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SERVERAPI.Controllers
{
    [SessionTimeout]
    public class FieldsController : BaseController
    {
        private readonly ILogger<FieldsController> _logger;
        private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;
        private readonly IOptions<AppSettings> _appSettings;
        private UserJourney _journey;

        public FieldsController(ILogger<FieldsController> logger,
            UserData ud,
            IAgriConfigurationRepository sd,
            IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _ud = ud;
            _sd = sd;
            _appSettings = appSettings;
            _journey = _ud.FarmDetails().UserJourney;
        }

        public ActionResult Fields()
        {
            FieldPageViewModel fvm = new FieldPageViewModel();

            FarmDetails fd = _ud.FarmDetails();

            var fields = _ud.GetFields();

            return View(fvm);
        }

        [HttpGet]
        public ActionResult FieldCopy(string currFld)
        {
            FieldCopyViewModel fvm = new FieldCopyViewModel();
            fvm.fldName = currFld;
            fvm.fieldList = new List<FieldListItem>();

            List<Field> fldList = _ud.GetFields();

            foreach (var f in fldList)
            {
                FieldListItem fli = new FieldListItem();
                if (!(f.FieldName == currFld))
                {
                    fli.fieldName = f.FieldName;
                    fli.fieldSelected = false;
                    fvm.fieldList.Add(fli);
                }
            }

            return PartialView("FieldCopy", fvm);
        }

        [HttpPost]
        public ActionResult FieldCopy(FieldCopyViewModel fvm)
        {
            int numSel = 0;

            if (ModelState.IsValid)
            {
                foreach (var fld in fvm.fieldList)
                {
                    if (fld.fieldSelected)
                    {
                        List<FieldCrop> toCrops = _ud.GetFieldCrops(fld.fieldName);
                        foreach (var c in toCrops)
                        {
                            _ud.DeleteFieldCrop(fld.fieldName, c.id);
                        }

                        List<NutrientFertilizer> toFert = _ud.GetFieldNutrientsFertilizers(fld.fieldName);
                        foreach (var f in toFert)
                        {
                            _ud.DeleteFieldNutrientsFertilizer(fld.fieldName, f.id);
                        }

                        List<NutrientManure> toMan = _ud.GetFieldNutrientsManures(fld.fieldName);
                        foreach (var m in toMan)
                        {
                            _ud.DeleteFieldNutrientsManure(fld.fieldName, m.id);
                        }

                        List<NutrientOther> toOther = _ud.GetFieldNutrientsOthers(fld.fieldName);
                        foreach (var o in toOther)
                        {
                            _ud.DeleteFieldNutrientsOther(fld.fieldName, o.id);
                        }
                        List<FieldCrop> fromCrops = _ud.GetFieldCrops(fvm.fldName);
                        foreach (var c in fromCrops)
                        {
                            _ud.AddFieldCrop(fld.fieldName, c);
                        }

                        List<NutrientFertilizer> fromFert = _ud.GetFieldNutrientsFertilizers(fvm.fldName);
                        foreach (var f in fromFert)
                        {
                            _ud.AddFieldNutrientsFertilizer(fld.fieldName, f);
                        }

                        List<NutrientManure> fromMan = _ud.GetFieldNutrientsManures(fvm.fldName);
                        foreach (var m in fromMan)
                        {
                            _ud.AddFieldNutrientsManure(fld.fieldName, m);
                        }

                        List<NutrientOther> fromOther = _ud.GetFieldNutrientsOthers(fvm.fldName);
                        foreach (var o in fromOther)
                        {
                            _ud.AddFieldNutrientsOther(fld.fieldName, o);
                        }

                        numSel++;
                    }
                }

                if (numSel == 0)
                {
                    ModelState.AddModelError("", "No fields selected for copying of information.");
                    return PartialView("FieldCopy", fvm);
                }

                return Json(new { success = true, reload = true });
            }
            return PartialView("FieldCopy", fvm);
        }

        [HttpGet]
        public ActionResult FieldDetail(string name, string target, string cntl, string actn, string currFld)
        {
            FieldDetailViewModel fvm = new FieldDetailViewModel();

            ConversionFactor cf = _sd.GetConversionFactor();

            fvm.selPrevYrManureOptions = _sd.GetPrevManureApplicationInPrevYears();

            fvm.target = target;
            fvm.actn = actn;
            fvm.cntl = cntl;
            fvm.currFld = currFld;
            fvm.placehldr = _sd.GetUserPrompt("fieldcommentplaceholder");
          
            if (!string.IsNullOrEmpty(name))
            {
                Field fld = _ud.GetFieldDetails(name);
                fvm.currFieldName = fld.FieldName;
                fvm.fieldName = fld.FieldName;
                fvm.fieldArea = fld.Area.ToString("G29");
                fvm.fieldComment = fld.Comment;
                fvm.fieldId = fld.Id;
                // retrofit old saved NMP files
                if (String.IsNullOrEmpty(fld.PreviousYearManureApplicationFrequency))
                    // set to default (no manure applied in the last two years)
                    fvm.selPrevYrManureOption = cf.DefaultApplicationOfManureInPrevYears;
                else
                    fvm.selPrevYrManureOption = fld.PreviousYearManureApplicationFrequency;
                fvm.act = "Edit";
            }
            else
            {
                fvm.act = "Add";
            }
            
            if(_journey == Agri.Models.UserJourney.Berries)
            {
                fvm.showPrevYrManureOption = false;
                fvm.selPrevYrManureOption = null;
            }
            else
            {
                fvm.showPrevYrManureOption = true;
            }

            return PartialView("FieldDetail", fvm);
        }

        [HttpPost]
        public ActionResult FieldDetail(FieldDetailViewModel fvm)
        {
            decimal area = 0;
            string url;
            // required to populate o/w validation messages are suppressed
            fvm.selPrevYrManureOptions = _sd.GetPrevManureApplicationInPrevYears();

            if (_journey == Agri.Models.UserJourney.Berries)
            {
                ModelState.Remove("selPrevYrManureOption");
            }

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(fvm.fieldComment))
                {
                    if (fvm.fieldComment.Length > Convert.ToInt32(_appSettings.Value.CommentLength))
                    {
                        ModelState.AddModelError("fieldComment", "Comment length of " + fvm.fieldComment.Length.ToString() + " exceeds maximum of " + _appSettings.Value.CommentLength);
                        return PartialView("FieldDetail", fvm);
                    }
                }
                try
                {
                    area = decimal.Parse(fvm.fieldArea);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("fieldArea", "Invalid amount for area.");
                    _logger.LogError(ex, "FieldDetail Exception");
                    return PartialView("FieldDetail", fvm);
                }
                if (fvm.selPrevYrManureOption == "select")
                {
                    ModelState.AddModelError("selPrevYrManureOption", "Manure application in previous years must be selected");
                    return PartialView("FieldDetail", fvm);
                }

                Field fld = _ud.GetFieldDetails(fvm.fieldName);

                if (fld != null)
                {
                    if ((fvm.act == "Edit" & fvm.currFieldName != fvm.fieldName) |
                        fvm.act == "Add")
                    {
                        ModelState.AddModelError("fieldName", "A field already exists with this name.");
                        return PartialView("FieldDetail", fvm);
                    }
                }

                if (fvm.act == "Add")
                {
                    fld = new Field();
                }
                else
                {
                    fld = _ud.GetFieldDetails(fvm.currFieldName);
                    if (fld == null)
                    {
                        fld = new Field();
                    }
                }

                fld.FieldName = fvm.fieldName;
                fld.Area = Math.Round(area, 1);
                fld.Comment = fvm.fieldComment;
                fld.PreviousYearManureApplicationFrequency = fvm.selPrevYrManureOption;

                if (fvm.act == "Add")
                {
                    _ud.AddField(fld);
                }
                else
                {
                    _ud.UpdateField(fld);
                }
                if (fvm.target == "#fields")
                {
                    url = Url.Action("RefreshFieldsList", "Fields");
                }
                else
                {
                    url = Url.Action(fvm.actn, fvm.cntl, new { nme = fld.FieldName });

                    //url = Url.Action("RefreshList", "Fields", new { cntl = fvm.cntl, actn = fvm.actn, currFld = fvm.currFld });
                }
                return Json(new { success = true, url = url, target = fvm.target });
            }
            return PartialView("FieldDetail", fvm);
        }

        public IActionResult RefreshFieldsList()
        {
            return ViewComponent("Fields");
        }

        public IActionResult RefreshList(string actn, string cntl, string currFld)
        {
            //return ViewComponent("FieldList", new { actn = actn, cntl = cntl });
            return RedirectToAction(actn, cntl, new { nme = currFld });
        }

        [HttpGet]
        public ActionResult FieldDelete(string name, string target)
        {
            FieldDeleteViewModel fvm = new FieldDeleteViewModel();
            fvm.target = target;

            Field fld = _ud.GetFieldDetails(name);

            fvm.fieldName = fld.FieldName;
            fvm.act = "Delete";

            return PartialView("FieldDelete", fvm);
        }

        [HttpPost]
        public ActionResult FieldDelete(FieldDeleteViewModel fvm)
        {
            if (ModelState.IsValid)
            {
                _ud.DeleteField(fvm.fieldName);

                string url = Url.Action("RefreshFieldsList", "Fields");
                return Json(new { success = true, url = url, target = fvm.target });
            }
            return PartialView("FieldDelete", fvm);
        }

        [HttpGet]
        public MissingFieldViewModel MissingField()
        {
            var fieldPresent = _ud.GetFields().Any();
            var journey = _ud.FarmDetails().UserJourney.ToString();

            var result = new MissingFieldViewModel() { journey = journey, fieldPresent = fieldPresent };

            return result;
        }
    }
}