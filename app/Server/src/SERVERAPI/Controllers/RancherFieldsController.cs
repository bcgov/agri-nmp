using Agri.Data;
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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SERVERAPI.Controllers
{
    [SessionTimeout]
    public class RancherFieldsController : BaseController
    {
        private readonly ILogger<FieldsController> _logger;
        private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;
        private readonly IOptions<AppSettings> _appSettings;

        public RancherFieldsController(ILogger<FieldsController> logger,
            UserData ud,
            IAgriConfigurationRepository sd,
            IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _ud = ud;
            _sd = sd;
            _appSettings = appSettings;
        }

        public ActionResult RancherFields()
        {
            RancherFieldPageViewModel fvm = new RancherFieldPageViewModel();

            FarmDetails fd = _ud.FarmDetails();

            return View(fvm);
        }

        [HttpGet]
        public ActionResult RancherFieldDetail(string name, string target, string cntl, string actn, string currFld)
        {
            RancherFieldDetailViewModel fvm = new RancherFieldDetailViewModel();

            ConversionFactor cf = _sd.GetConversionFactor();

            fvm.SelectPrevYrManureOptions = _sd.GetPrevManureApplicationInPrevYears();

            fvm.Target = target;
            fvm.Actn = actn;
            fvm.Cntl = cntl;
            fvm.CurrFld = currFld;
            fvm.Placehldr = _sd.GetUserPrompt("fieldcommentplaceholder");

            if (!string.IsNullOrEmpty(name))
            {
                RancherField fld = _ud.GetRancherFieldDetails(name);
                fvm.CurrFieldName = fld.FieldName;
                fvm.FieldName = fld.FieldName;
                fvm.FieldArea = fld.Area.ToString("G29");
                fvm.FieldComment = fld.Comment;
                fvm.IsSeasonalFeedingArea = fld.IsSeasonalFeedingArea;
                fvm.FieldId = fld.Id;
                // retrofit old saved NMP files
                if (String.IsNullOrEmpty(fld.PrevYearManureApplicationFrequency))
                    // set to default (no manure applied in the last two years)
                    fvm.SelectPrevYrManureOption = cf.DefaultApplicationOfManureInPrevYears;
                else
                    fvm.SelectPrevYrManureOption = fld.PrevYearManureApplicationFrequency;
                fvm.Act = "Edit";
            }
            else
            {
                fvm.Act = "Add";
            }
            return PartialView("RancherFieldDetail", fvm);
        }

        [HttpPost]
        public ActionResult RancherFieldDetail(RancherFieldDetailViewModel fvm)
        {
            decimal area = 0;
            string url;
            // required to populate o/w validation messages are suppressed
            fvm.SelectPrevYrManureOptions = _sd.GetPrevManureApplicationInPrevYears();

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(fvm.FieldComment))
                {
                    if (fvm.FieldComment.Length > Convert.ToInt32(_appSettings.Value.CommentLength))
                    {
                        ModelState.AddModelError("fieldComment", "Comment length of " + fvm.FieldComment.Length.ToString() + " exceeds maximum of " + _appSettings.Value.CommentLength);
                        return PartialView("RancherFieldDetail", fvm);
                    }
                }
                try
                {
                    area = decimal.Parse(fvm.FieldArea);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("fieldArea", "Invalid amount for area.");
                    _logger.LogError(ex, "RancherFieldDetail Exception");
                    return PartialView("RancherFieldDetail", fvm);
                }
                if (fvm.SelectPrevYrManureOption == "select")
                {
                    ModelState.AddModelError("selPrevYrManureOption", "Manure application in previous years must be selected");
                    return PartialView("RancherFieldDetail", fvm);
                }

                RancherField fld = _ud.GetRancherFieldDetails(fvm.FieldName);

                if (fld != null)
                {
                    if ((fvm.Act == "Edit" & fvm.CurrFieldName != fvm.FieldName) |
                        fvm.Act == "Add")
                    {
                        ModelState.AddModelError("fieldName", "A field already exists with this name.");
                        return PartialView("RancherFieldDetail", fvm);
                    }
                }

                if (fvm.Act == "Add")
                {
                    fld = new RancherField();
                }
                else
                {
                    fld = _ud.GetRancherFieldDetails(fvm.CurrFieldName);
                    if (fld == null)
                    {
                        fld = new RancherField();
                    }
                }

                fld.FieldName = fvm.FieldName;
                fld.Area = Math.Round(area, 1);
                fld.Comment = fvm.FieldComment;
                fld.PrevYearManureApplicationFrequency = fvm.SelectPrevYrManureOption;
                fld.IsSeasonalFeedingArea = fvm.IsSeasonalFeedingArea;
                fld.SeasonalFeedingArea = fld.IsSeasonalFeedingArea ? "Yes" : "No";

                if (fvm.Act == "Add")
                {
                    _ud.AddRancherField(fld);
                }
                else
                {
                    _ud.UpdateRancherField(fld);
                }
                if (fvm.Target == "#rancherFields")
                {
                    url = Url.Action("RefreshRancherFieldsList", "RancherFields");
                }
                else
                {
                    url = Url.Action(fvm.Actn, fvm.Cntl, new { nme = fld.FieldName });
                }
                return Json(new { success = true, url = url, target = fvm.Target });
            }
            return PartialView("RancherFieldDetail", fvm);
        }

        public IActionResult RefreshRancherFieldsList()
        {
            return ViewComponent("RancherFields");
        }

        [HttpGet]
        public ActionResult RancherFieldDelete(string name, string target)
        {
            RancherFieldDeleteViewModel fvm = new RancherFieldDeleteViewModel();
            fvm.Target = target;

            RancherField fld = _ud.GetRancherFieldDetails(name);

            fvm.FieldName = fld.FieldName;
            fvm.Act = "Delete";

            return PartialView("RancherFieldDelete", fvm);
        }

        [HttpPost]
        public ActionResult RancherFieldDelete(RancherFieldDeleteViewModel fvm)
        {
            if (ModelState.IsValid)
            {
                _ud.DeleteRancherField(fvm.FieldName);

                string url = Url.Action("RefreshRancherFieldsList", "RancherFields");
                return Json(new { success = true, url = url, target = fvm.Target });
            }
            return PartialView("RancherFieldDelete", fvm);
        }
    }
}