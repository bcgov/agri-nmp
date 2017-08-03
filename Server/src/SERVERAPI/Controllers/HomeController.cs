using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using SERVERAPI.Models;
using SERVERAPI.ViewModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text;
using System.Net;

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
        public IActionResult Index()
        {
            //ViewBag.Title = "NMP";
            //FarmData userData = new FarmData();
            //userData.Years = new List<YearData>();
            //YearData year = new YearData();
            //year.Fields = new List<Field>();
            //Field fld1 = new Field();
            //Field fld2 = new Field();
            //year.Fields.Add(fld1);
            //year.Fields.Add(fld2);
            //userData.Years.Add(year);
            //userData.farmName = "ElDorado";
            //var json = JsonConvert.SerializeObject(userData);

            //HttpContext.Session.SetObjectAsJson("FarmData", userData);
            return View();
        }
        public IActionResult Launch(string id)
        {
            ViewBag.Title = "NMP";

            LaunchViewModel lvm = new LaunchViewModel();
            lvm.userData = null;

            if (id == "false")
            {
                //userData.Years = new List<YearData>();
                //YearData year = new YearData();
                //year.Fields = new List<Field>();
                //Field fld1 = new Field();
                //Field fld2 = new Field();
                //year.Fields.Add(fld1);
                //year.Fields.Add(fld2);
                //userData.Years.Add(year);
                //userData.farmName = "ElDorado";
                //var json = JsonConvert.SerializeObject(userData);
                //lvm.userData = json;
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

                //HttpContext.Session.SetObjectAsJson("FarmData", lvm.userData);
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

            FarmViewModel fvm = new FarmViewModel();

            fvm.farmName = farmData.farmName;
            if(farmData.soilTests != null)
            {
                fvm.soilTests = farmData.soilTests.Value;
            }
            if (farmData.manure != null)
            {
                fvm.manure = farmData.manure.Value;
            }

            fvm.userData = HttpContext.Session.GetString("FarmData");

            return View(fvm);
        }
        [HttpPost]
        public IActionResult Farm(FarmViewModel fvm)
        {
            string x = HttpContext.Session.GetString("FarmData");
            var farmData = JsonConvert.DeserializeObject<FarmData>(fvm.userData);

            farmData.farmName = fvm.farmName;
            farmData.soilTests = (fvm.soilTests == null) ? null : fvm.soilTests;
            farmData.manure = (fvm.manure == null) ? null : fvm.manure;

            HttpContext.Session.SetObjectAsJson("FarmData", farmData);
            fvm.userData = JsonConvert.SerializeObject(farmData);
            HttpContext.Session.SetObjectAsJson("FarmData", fvm.userData);
            ModelState.Remove("userData");

            return View(fvm);
        }
        [HttpGet]
        public IActionResult FieldDetail()
        {

            return View();
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
    }
}
