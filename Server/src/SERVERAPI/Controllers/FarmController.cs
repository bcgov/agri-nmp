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

namespace SERVERAPI.Controllers
{
    public class FarmController : Controller
    {
        private IHostingEnvironment _env;

        public FarmController(IHostingEnvironment env)
        {
            _env = env;
        }
        [HttpGet]
        public IActionResult Farm()
        {
            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            Models.Impl.StaticData sd = new Models.Impl.StaticData();

            FarmViewModel fvm = new FarmViewModel();
            fvm.regOptions = sd.GetRegionsDll(HttpContext).ToList();
            fvm.selRegOption = null;

            fvm.sendNMP = true;

            fvm.year = farmData.year;
            fvm.currYear = farmData.year;
            fvm.farmName = farmData.farmName;
            if (string.IsNullOrEmpty(farmData.year))
            {
                fvm.year = DateTime.Now.ToString("yyyy");
                if (farmData.years == null)
                {
                    farmData.years = new List<YearData>();
                    farmData.years.Add(new YearData { year = fvm.year });
                }

                farmData.year = fvm.year;
                HttpContext.Session.SetObjectAsJson("FarmData", farmData);
            }
            if (farmData.soilTests != null)
            {
                fvm.soilTests = farmData.soilTests.Value;
            }
            if (farmData.manure != null)
            {
                fvm.manure = farmData.manure.Value;
            }
            fvm.selRegOption = farmData.farmRegion;
            fvm.userData = HttpContext.Session.GetString("FarmData");

            return View(fvm);
        }
        [HttpPost]
        public IActionResult Farm(FarmViewModel fvm)
        {
            Models.Impl.StaticData sd = new Models.Impl.StaticData();
            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            fvm.regOptions = sd.GetRegionsDll(HttpContext).ToList();

            farmData.year = fvm.year;
            farmData.farmName = fvm.farmName;
            farmData.farmRegion = fvm.selRegOption;
            farmData.soilTests = (fvm.soilTests == null) ? null : fvm.soilTests;
            farmData.manure = (fvm.manure == null) ? null : fvm.manure;

            if (farmData.years == null)
            {
                farmData.years = new List<YearData>();
                farmData.years.Add(new YearData { year = fvm.year });
            }
            else
            {
                YearData yd = farmData.years.FirstOrDefault(y => y.year == fvm.year);

                if (yd == null)
                {
                    farmData.years.Add(new YearData { year = fvm.year });
                }
            }

            fvm.currYear = fvm.year;
            HttpContext.Session.SetObjectAsJson("FarmData", farmData);
            fvm.userData = JsonConvert.SerializeObject(farmData);
            HttpContext.Session.SetObjectAsJson("FarmData", farmData);
            fvm.sendNMP = false;
            ModelState.Remove("userData");
            ModelState.Remove("sendNMP");

            return View(fvm);
        }
        [HttpGet]
        public ActionResult FieldDetail(string name)
        {
            FieldDetailViewModel fvm = new FieldDetailViewModel();

            if (!string.IsNullOrEmpty(name))
            {
                var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
                YearData yd = farmData.years.FirstOrDefault(y => y.year == farmData.year);
                Field fld = yd.fields.FirstOrDefault(y => y.fieldName == name);
                if (fld != null)
                {
                    fvm.currFieldName = fld.fieldName;
                    fvm.fieldName = fld.fieldName;
                    fvm.fieldArea = fld.area.ToString();
                    fvm.fieldComment = fld.comment;
                    fvm.act = "Edit";
                }
                else
                {
                    fvm.act = "Add";
                }
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

                var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
                YearData yd = farmData.years.FirstOrDefault(y => y.year == farmData.year);

                if (yd.fields == null)
                {
                    yd.fields = new List<Field>();
                }

                Field fld = yd.fields.FirstOrDefault(y => y.fieldName == fvm.fieldName);

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
                    fld = yd.fields.FirstOrDefault(y => y.fieldName == fvm.currFieldName);
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
                    yd.fields.Add(fld);
                }

                HttpContext.Session.SetObjectAsJson("FarmData", farmData);

                string url = Url.Action("RefreshFieldsList", "Farm");
                return Json(new { success = true, url = url, farmdata = JsonConvert.SerializeObject(farmData) });
            }
            return PartialView("FieldDetail", fvm);
        }
        public IActionResult RefreshFieldsList()
        {
            return ViewComponent("Fields");
        }
        [HttpGet]
        public ActionResult FieldDelete(string name)
        {
            FieldDeleteViewModel fvm = new FieldDeleteViewModel();
            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            if (!string.IsNullOrEmpty(name))
            {
                YearData yd = farmData.years.FirstOrDefault(y => y.year == farmData.year);
                Field fld = yd.fields.FirstOrDefault(y => y.fieldName == name);
                if (fld != null)
                {
                    fvm.fieldName = fld.fieldName;
                    fvm.act = "Delete";
                }
            }

            fvm.userDataField = JsonConvert.SerializeObject(farmData);

            return PartialView("FieldDelete", fvm);
        }
        [HttpPost]
        public ActionResult FieldDelete(FieldDeleteViewModel fvm)
        {
            if (ModelState.IsValid)
            {
                var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
                YearData yd = farmData.years.FirstOrDefault(y => y.year == farmData.year);
                Field fld = yd.fields.FirstOrDefault(y => y.fieldName == fvm.fieldName);

                if (fld == null)
                {
                    ModelState.AddModelError("fieldName", "Field name not found.");
                    return PartialView("FieldDelete", fvm);
                }

                yd.fields.Remove(fld);

                HttpContext.Session.SetObjectAsJson("FarmData", farmData);

                string url = Url.Action("RefreshFieldsList", "Farm");
                return Json(new { success = true, url = url, farmdata = JsonConvert.SerializeObject(farmData) });
            }
            return PartialView("FieldDelete", fvm);
        }
    }
}
