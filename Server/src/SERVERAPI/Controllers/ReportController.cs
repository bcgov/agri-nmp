using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.ViewModels;
using SERVERAPI.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using SERVERAPI.Utility;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SERVERAPI.Controllers
{
    public class ReportController : Controller
    {
        private IHostingEnvironment _env;
        private readonly IViewRenderService _viewRenderService;

        public ReportController(IHostingEnvironment env, IViewRenderService viewRenderService)
        {
            _env = env;
            _viewRenderService = viewRenderService;
        }
        [HttpGet]
        public IActionResult Report()
        {
            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            ReportViewModel rvm = new ReportViewModel();

            return View(rvm);
        }
        [HttpPost]
        public IActionResult Report(ReportViewModel rvm)
        {

            return View(rvm);
        }
        public async Task<IActionResult> Print(bool fields, bool sources, bool application, bool analysis, bool summary, bool sheets )
        {
            string reportFields = string.Empty;
            string reportSources = string.Empty;
            string reportAppl = string.Empty;
            string reportAnalysis = string.Empty;
            string reportSummary = string.Empty;
            string reportSheets = string.Empty;

            FileContentResult result = null;
            //JSONResponse result = null; 
            //var pdfHost = Environment.GetEnvironmentVariable("PDF_SERVICE_NAME", EnvironmentVariableTarget.User);

            string pdfHost = "http://localhost:54611";

            string targetUrl = pdfHost + "/api/PDF/BuildPDF";

            // call the microservice
            try
            {
                PDFRequest req = new PDFRequest();

                HttpClient client = new HttpClient();

                if (fields)
                    reportFields = await RenderFields();
                if (sources)
                    reportSources = await RenderSources();
                if (application)
                    reportAppl = await RenderApplication();
                if (analysis)
                    reportAnalysis = await RenderAnalysis();
                if (summary)
                    reportSummary = await RenderSummary();
                if (sheets)
                    reportSheets = await RenderSheets();

                string rawdata = "<!DOCTYPE html>"  +
                    "<html>" + 
                    "<head>" + 
                    "<meta charset='utf-8' /><title></title>" + 
                    "</head>" + 
                    "<body>" +
                    "<div style='display: table; width: 100%'>" +
                    "<div style='display: table-row-group; width: 100%'>" +
                    reportFields +
                    reportSources +
                    reportAppl +
                    reportAnalysis +
                    reportSummary +
                    reportSheets +
                    "</div>" + 
                    "</div>" + 
                    "</body></html>";

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

            return result;
        }
        public async Task<string> RenderFields()
        {
            ReportViewModel rvm = new ReportViewModel();

            var result = await _viewRenderService.RenderToStringAsync("Report/ReportFields", rvm);

            return result;
        }
        public async Task<string> RenderSources()
        {
            ReportViewModel rvm = new ReportViewModel();

            var result = await _viewRenderService.RenderToStringAsync("Report/ReportSources", rvm);

            return result;
        }
        public async Task<string> RenderAnalysis()
        {
            ReportViewModel rvm = new ReportViewModel();

            var result = await _viewRenderService.RenderToStringAsync("Report/ReportAnalysis", rvm);

            return result;
        }
        public async Task<string> RenderApplication()
        {
            ReportViewModel rvm = new ReportViewModel();

            var result = await _viewRenderService.RenderToStringAsync("Report/ReportApplication", rvm);

            return result;
        }
        public async Task<string> RenderSummary()
        {
            ReportViewModel rvm = new ReportViewModel();

            var result = await _viewRenderService.RenderToStringAsync("Report/ReportSummary", rvm);

            return result;
        }
        public async Task<string> RenderSheets()
        {
            ReportViewModel rvm = new ReportViewModel();

            var result = await _viewRenderService.RenderToStringAsync("Report/ReportSheets", rvm);

            return result;
        }
    }
}