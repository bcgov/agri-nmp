using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;
using SERVERAPI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SERVERAPI.Controllers
{
    //[RedirectingAction]
    public class FieldsController : Controller
    {
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public Models.Impl.StaticData _sd { get; set; }

        public FieldsController(IHostingEnvironment env, UserData ud, Models.Impl.StaticData sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
        }

        public ActionResult Fields()
        {
            FieldPageViewModel fvm = new FieldPageViewModel();

            FarmDetails fd = _ud.FarmDetails();
            fvm.manure = fd.manure;
            fvm.soilTests = fd.soilTests;

            return View(fvm);
        }

        [HttpGet]
        public ActionResult FieldDetail(string name, string target, string cntl, string actn)
        {
            FieldDetailViewModel fvm = new FieldDetailViewModel();
            fvm.target = target;
            fvm.actn = actn;
            fvm.cntl = cntl;
            fvm.placehldr = _sd.GetUserPrompt("fieldcommentplaceholder");

            if (!string.IsNullOrEmpty(name))
            {
                Field fld = _ud.GetFieldDetails(name);
                fvm.currFieldName = fld.fieldName;
                fvm.fieldName = fld.fieldName;
                fvm.fieldArea = fld.area.ToString();
                fvm.fieldComment = fld.comment;
                fvm.fieldId = fld.id;
                fvm.act = "Edit";
            }
            else
            {
                fvm.act = "Add";
            }
            return PartialView("FieldDetail", fvm);
        }
        [HttpPost]
        public ActionResult FieldDetail(FieldDetailViewModel fvm)
        {
            decimal area = 0;
            string url;

            if (ModelState.IsValid)
            {
                try
                {
                    area = decimal.Parse(fvm.fieldArea);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("fieldArea", "Invalid amount for area.");
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

                fld.fieldName = fvm.fieldName;
                fld.area = Math.Round(area, 1);
                fld.comment = fvm.fieldComment;

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
                    url = Url.Action("RefreshList", "Fields", new { cntl = fvm.cntl, actn = fvm.actn });
                }
                return Json(new { success = true, url = url, target = fvm.target });
            }
            return PartialView("FieldDetail", fvm);
        }
        public IActionResult RefreshFieldsList()
        {
            return ViewComponent("Fields");
        }
        public IActionResult RefreshList(string actn, string cntl)
        {
            //return ViewComponent("FieldList", new { actn = actn, cntl = cntl });
            return RedirectToAction(actn, cntl);
        }
        [HttpGet]
        public ActionResult FieldDelete(string name, string target)
        {
            FieldDeleteViewModel fvm = new FieldDeleteViewModel();
            fvm.target = target;

            Field fld = _ud.GetFieldDetails(name);

            fvm.fieldName = fld.fieldName;
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
    }
}
