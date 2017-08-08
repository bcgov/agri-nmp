using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using SERVERAPI.Models;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

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
    }
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
    public class HomeController : Controller
    {
        private IHostingEnvironment _env;

        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Launch(string id)
        {
            ViewBag.Title = "NMP";
            //LoadStatic();
            LaunchViewModel lvm = new LaunchViewModel();
            lvm.userData = null;

            if (id == "false")
            {
                lvm.canContinue = false;
            }
            else
            {
                lvm.canContinue = true;
            }

            HttpContext.Session.SetObjectAsJson("FarmData", lvm.userData);

            return View(lvm);

        }
        [HttpPost]
        public IActionResult Launch(LaunchViewModel lvm, string type)
        {
            if(type == "Continue")
            {
                // save the local storage user data in it's stringified form
                HttpContext.Session.SetString("FarmData", lvm.userData);
                var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            }
            else
            {
                FarmData userData = new FarmData();
                HttpContext.Session.SetObjectAsJson("FarmData", userData);
                var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            }
            return RedirectToAction("Farm");
        }
        [HttpGet]
        public async Task<IActionResult> Report([FromServices] INodeServices nodeServices)
        {
            FileContentResult result = null;
            //JSONResponse result = null;
            var pdfHost = Environment.GetEnvironmentVariable("PDF_SERVICE_NAME");

            //string pdfHost = Configuration["PDF_SERVICE_NAME"];

            string targetUrl = pdfHost + "/api/PDF/GetPDF";

            // call the microservice
            try
            {
                PDFRequest req = new PDFRequest();


                HttpClient client = new HttpClient();

                string rawdata = "<!DOCTYPE html><html><head><meta charset='utf-8' /><title></title></head><body><div style='width: 100%; background-color:lightgreen'>Section 1</div><br><div style='page -break-after:always; '></div><div style='width: 100%; background-color:lightgreen'>Section 2</div></body></html>";

                req.html = rawdata;

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
                if (response.StatusCode == HttpStatusCode.OK) // success
                {
                    var bytetask = response.Content.ReadAsByteArrayAsync();
                    bytetask.Wait();

                    result = new FileContentResult(bytetask.Result, "application/pdf");
                }
            }
            catch (Exception e)
            {
                result = null;
            }

            //JSONResponse result = null;

            //var options = new { format = "letter", orientation = "landscape" };

            //var opts = new
            //{
            //    orientation = "landscape",

            //};

            //string rawdata = "<!DOCTYPE html><html><head><meta charset='utf-8' /><title></title></head><body><div style='width: 100%; background-color:lightgreen'>Section 1</div><br><div style='page -break-after:always; '></div><div style='width: 100%; background-color:lightgreen'>Section 2</div></body></html>";

            //// execute the Node.js component
            //result = await nodeServices.InvokeAsync<JSONResponse>("./PDF.js", rawdata, options);

            return new FileContentResult(result.FileContents, "application/pdf");
        }
        [HttpGet]
        public IActionResult Print()
        {
            var userData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            FileContentResult result = null;
            //JSONResponse result = null;
            var pdfHost = Environment.GetEnvironmentVariable("PDF_SERVICE_NAME");

            //string pdfHost = Configuration["PDF_SERVICE_NAME"];

            string targetUrl = pdfHost + "/api/PDF/GetPDF";

            ViewBag.Service = targetUrl;

            // call the microservice
            try
            {
                PDFRequest req = new PDFRequest();


                HttpClient client = new HttpClient();

                string rawdata = "<!DOCTYPE html><html><head><meta charset='utf-8' /><title></title></head><body><div style='width: 100%; background-color:lightgreen'>Section 1</div><br><div style='page -break-after:always; '></div><div style='width: 100%; background-color:lightgreen'>Section 2</div></body></html>";

                req.html = rawdata;

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
            }
            catch (Exception e)
            {
                result = null;
            }

            return View();
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
            fvm.farmName = farmData.farmName;
            if(farmData.soilTests != null)
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
                if(fld != null)
                {
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
            int area = 0;

            if(ModelState.IsValid)
            {
                try
                {
                    area = Int32.Parse(fvm.fieldArea);
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

                if(fvm.act == "Add")
                {
                    if (fld != null)
                    {
                        ModelState.AddModelError("fieldName", "A field already exists with this name.");
                        return PartialView("FieldDetail", fvm);
                    }
                    else
                    {
                        fld = new Field();
                    }
                }

                fld.fieldName = fvm.fieldName;
                fld.area = area;
                fld.comment = fvm.fieldComment;

                if (fvm.act == "Add")
                {
                    yd.fields.Add(fld);
                }

                HttpContext.Session.SetObjectAsJson("FarmData", farmData);

                string url = Url.Action("RefreshFieldsList", "Home");
                return Json(new { success = true, url = url , farmdata = JsonConvert.SerializeObject(farmData) });
            }
            return PartialView("FieldDetail",fvm);
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

                string url = Url.Action("RefreshFieldsList", "Home");
                return Json(new { success = true, url = url, farmdata = JsonConvert.SerializeObject(farmData) });
            }
            return PartialView("FieldDelete", fvm);
        }
        private static async Task<JSONResponse> BuildReport(INodeServices nodeServices)
        {
            JSONResponse result = null;
            var options = new { format = "letter", orientation = "landscape" };

            var opts = new
            {
                orientation = "landscape",

            };

            string rawdata = "<!DOCTYPE html><html><head><meta charset='utf-8' /><title></title></head><body><div style='width: 100%; background-color:lightgreen'>Section 1</div><br><div style='page -break-after:always; '></div><div style='width: 100%; background-color:lightgreen'>Section 2</div></body></html>";

            // execute the Node.js component
            result = await nodeServices.InvokeAsync<JSONResponse>("pdf.js", rawdata, options);

            return result;
        }
        public void LoadStatic()
        {
            string path = _env.WebRootPath.ToString() + "\\data\\static.json";
            string staticValues = string.Join("", System.IO.File.ReadAllLines(path));
            HttpContext.Session.Set("Static", Encoding.ASCII.GetBytes(staticValues));
        }
    }
}
