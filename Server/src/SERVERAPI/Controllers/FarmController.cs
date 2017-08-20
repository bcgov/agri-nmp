using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.Models;
using SERVERAPI.ViewModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Controllers
{
    public class FarmController : Controller
    {
        private IHostingEnvironment _env;
        private UserData _ud;

        public FarmController(IHostingEnvironment env, UserData ud)
        {
            _env = env;
            _ud = ud;
        }
        [HttpGet]
        public IActionResult Farm()
        {
            var farmData = _ud.FarmDetails();
            Models.Impl.StaticData sd = new Models.Impl.StaticData();

            FarmViewModel fvm = new FarmViewModel();

            fvm.regOptions = sd.GetRegionsDll(HttpContext).ToList();
            fvm.selRegOption = null;

            fvm.year = farmData.year;
            fvm.currYear = farmData.year;
            fvm.farmName = farmData.farmName;

            if (farmData.soilTests != null)
            {
                fvm.soilTests = farmData.soilTests.Value;
            }
            if (farmData.manure != null)
            {
                fvm.manure = farmData.manure.Value;
            }

            fvm.selRegOption = farmData.farmRegion;

            return View(fvm);
        }
        [HttpPost]
        public IActionResult Farm(FarmViewModel fvm)
        {
            Models.Impl.StaticData sd = new Models.Impl.StaticData();
            var farmData = _ud.FarmDetails();

            fvm.regOptions = sd.GetRegionsDll(HttpContext).ToList();

            farmData.year = fvm.year;
            farmData.farmName = fvm.farmName;
            farmData.farmRegion = fvm.selRegOption;
            farmData.soilTests = (fvm.soilTests == null) ? null : fvm.soilTests;
            farmData.manure = (fvm.manure == null) ? null : fvm.manure;

            _ud.UpdateFarmDetails(farmData);

            fvm.currYear = fvm.year;
            ModelState.Remove("userData");

            return View(fvm);
        }
        [HttpGet]
        public ActionResult FieldDetail(string name, string target, string cntl, string actn)
        {
            FieldDetailViewModel fvm = new FieldDetailViewModel();
            fvm.target = target;
            fvm.actn = actn;
            fvm.cntl = cntl;

            if (!string.IsNullOrEmpty(name))
            {
                Field fld = _ud.GetFieldDetails(name);
                //var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
                //YearData yd = farmData.years.FirstOrDefault(y => y.year == farmData.farmDetails.year);
                //Field fld = yd.fields.FirstOrDefault(y => y.fieldName == name);
                //if (fld != null)
                //{
                fvm.currFieldName = fld.fieldName;
                fvm.fieldName = fld.fieldName;
                fvm.fieldArea = fld.area.ToString();
                fvm.fieldComment = fld.comment;
                fvm.act = "Edit";
                //}
                //else
                //{
                //    fvm.act = "Add";
                //}
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
                    fld = _ud.GetFieldDetails(fvm.fieldName);
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
                    url = Url.Action("RefreshFieldsList", "Farm");
                }
                else
                {
                    url = Url.Action("RefreshList", "Farm", new { cntl = fvm.cntl, actn = fvm.actn });
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
            return ViewComponent("FieldList", new { actn = actn, cntl = cntl });
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

                string url = Url.Action("RefreshFieldsList", "Farm");
                return Json(new { success = true, url = url, target = fvm.target });
            }
            return PartialView("FieldDelete", fvm);
        }
    }
}
