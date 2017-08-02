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

namespace SERVERAPI.Controllers
{
    public class JSONResponse
    {
        public string type;
        public byte[] data;
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
            ViewBag.Title = "NMP";
            FarmData userData = new FarmData();
            userData.Years = new List<YearData>();
            YearData year = new YearData();
            year.Fields = new List<Field>();
            Field fld1 = new Field();
            Field fld2 = new Field();
            year.Fields.Add(fld1);
            year.Fields.Add(fld2);
            userData.Years.Add(year);
            userData.farmName = "ElDorado";
            var json = JsonConvert.SerializeObject(userData);

            HttpContext.Session.SetObjectAsJson("FarmData", userData);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Report([FromServices] INodeServices nodeServices)
        {
            JSONResponse result = null;

            var options = new { format = "letter", orientation = "landscape" };

            var opts = new
            {
                orientation = "landscape",

            };

            string rawdata = "<!DOCTYPE html><html><head><meta charset='utf-8' /><title></title></head><body><div style='width: 100%; background-color:lightgreen'>Section 1</div><br><div style='page -break-after:always; '></div><div style='width: 100%; background-color:lightgreen'>Section 2</div></body></html>";

            // execute the Node.js component
            result = await nodeServices.InvokeAsync<JSONResponse>("./PDF.js", rawdata, options);

            return new FileContentResult(result.data, "application/pdf");
        }
        [HttpGet]
        public IActionResult Farm()
        {
            var userData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            FarmViewModel fvm = new FarmViewModel();

            fvm.farmName = userData.farmName;
            if(userData.soilTests != null)
            {
                fvm.soilTests = userData.soilTests.Value;
            }
            if (userData.manure != null)
            {
                fvm.manure = userData.manure.Value;
            }


            return View(fvm);
        }
        [HttpPost]
        public IActionResult Farm(FarmViewModel fvm)
        {
            var userData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            userData.farmName = fvm.farmName;
            userData.soilTests = (fvm.soilTests == null) ? null : fvm.soilTests;
            userData.manure = (fvm.manure == null) ? null : fvm.manure;

            HttpContext.Session.SetObjectAsJson("FarmData", userData);

            return View(fvm);
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
