using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using SERVERAPI.Models;
using Newtonsoft.Json;

namespace SERVERAPI.Controllers
{
    public class JSONResponse
    {
        public string type;
        public byte[] data;
    }
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "NMP";
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
            result = await nodeServices.InvokeAsync<JSONResponse>("pdf.js", rawdata, options);

            return new FileContentResult(result.data, "application/pdf");
        }
        [HttpGet]
        public IActionResult Farm()
        {
            FarmData userData = new FarmData();
            userData.Years = new List<YearData>();
            YearData year = new YearData();
            year.Fields = new List<Field>();
            Field fld1 = new Field();
            Field fld2 = new Field();
            year.Fields.Add(fld1);
            year.Fields.Add(fld2);
            userData.Years.Add(year);
            var json = JsonConvert.SerializeObject(userData);

            return View();
        }
    }
}
