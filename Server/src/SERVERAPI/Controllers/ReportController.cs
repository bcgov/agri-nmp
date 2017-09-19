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
using SERVERAPI.Models.Impl;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.NodeServices;

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
        public string options;
    }

    public class ReportController : Controller
    {
        private IHostingEnvironment _env;
        private UserData _ud;
        private Models.Impl.StaticData _sd;
        private readonly IViewRenderService _viewRenderService;

        public ReportController(IHostingEnvironment env, IViewRenderService viewRenderService, UserData ud, Models.Impl.StaticData sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
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
        public async Task<IActionResult> Print([FromServices] INodeServices nodeServices, bool fields, bool sources, bool application, bool analysis, bool summary, bool sheets )
        {
            string reportHeader = string.Empty;
            string reportFields = string.Empty;
            string reportSources = string.Empty;
            string reportAppl = string.Empty;
            string reportAnalysis = string.Empty;
            string reportSummary = string.Empty;
            string reportSheets = string.Empty;

            FileContentResult result = null;
            //JSONResponse result = null;
            //var pdfHost = Environment.GetEnvironmentVariable("PDF_SERVICE_NAME");

            string pdfHost = "http://localhost:54611";

            string targetUrl = pdfHost + "/api/PDF/BuildPDF";

            PDF_Options options = new PDF_Options();
            options.border = new PDF_Border();
            options.header = new PDF_Header();
            options.footer = new PDF_Footer();

            options.type = "pdf";
            options.quality = "75";
            options.format = "letter";
            options.orientation = "landscape";
            options.border.top = "0in";
            options.border.right = "0in";
            options.border.bottom = "0in";
            options.border.left = "0in";
            options.header.height = "30mm";
            options.header.contents = "<b>Nutrient Management Report</b><br />" +
                                      "Farm Name: " + _ud.FarmDetails().farmName + "<br />" +
                                      "Planning Year: " + _ud.FarmDetails().year;
            options.footer.height = "15mm";
            options.footer.contents = "<span style=\"color: #444;\">Page {{page}}</span>/<span>{{pages}}</span>";

            // call the microservice
            try
            {
                PDFRequest req = new PDFRequest();

                HttpClient client = new HttpClient();

                reportHeader = await RenderHeader();
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
                    reportHeader + 
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
                req.options = JsonConvert.SerializeObject(options);

                FileContentResult res = await BuildPDF(nodeServices, req);

                return res;

                //string payload = JsonConvert.SerializeObject(req);

                //var request = new HttpRequestMessage(HttpMethod.Post, targetUrl);
                //request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

                //request.Headers.Clear();
                //// transfer over the request headers.
                //foreach (var item in Request.Headers)
                //{
                //    string key = item.Key;
                //    string value = item.Value;
                //    request.Headers.Add(key, value);
                //}

                //Task<HttpResponseMessage> responseTask = client.SendAsync(request);
                //responseTask.Wait();

                //HttpResponseMessage response = responseTask.Result;

                //ViewBag.StatusCode = response.StatusCode.ToString();

                //if (response.StatusCode == HttpStatusCode.OK) // success
                //{
                //    var bytetask = response.Content.ReadAsByteArrayAsync();
                //    bytetask.Wait();

                //    result = new FileContentResult(bytetask.Result, "application/pdf");
                //}
                //else
                //{
                //    string errorMsg = "Url: " + targetUrl + "\r\n" +
                //                      "Result: " + response.StatusCode.ToString();
                //    result = new FileContentResult(Encoding.ASCII.GetBytes(errorMsg), "text/plain");
                //}
            }
            catch (Exception e)
            {
                string errorMsg = "Url: " + targetUrl + "\r\n" +
                                  "Result: " + e.Message;
                result = new FileContentResult(Encoding.ASCII.GetBytes(errorMsg), "text/plain");
            }

            return result;
        }
        public async Task<string> RenderHeader()
        {
            ReportViewModel rvm = new ReportViewModel();

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportHeader.cshtml", rvm);

            return result;
        }
        public async Task<string> RenderFields()
        {
            ReportFieldsViewModel rvm = new ReportFieldsViewModel();
            rvm.fields = new List<ReportFieldsField>();

            List<Field> fldList = _ud.GetFields();
            foreach (var f in fldList)
            {
                ReportFieldsField rf = new ReportFieldsField();
                rf.fieldName = f.fieldName;
                rf.fieldComment = f.comment;
                rf.nutrients = new List<ReportFieldNutrient>();
                if (f.nutrients != null)
                {
                    if (f.nutrients.nutrientManures != null)
                    {
                        foreach (var m in f.nutrients.nutrientManures)
                        {
                            Models.StaticData.Manure manure = _sd.GetManure(m.manureId);
                            ReportFieldNutrient rfn = new ReportFieldNutrient();

                            rfn.nutrientName = manure.name;
                            rfn.nutrientAmount = m.rate;
                            rfn.nutrientSeason = _sd.GetApplication(m.applicationId.ToString()).season;
                            rfn.nutrientApplication = _sd.GetApplication(m.applicationId.ToString()).application_method;
                            rfn.nutrientUnit = _sd.GetUnit(m.unitId).name;
                            rf.nutrients.Add(rfn);
                        }
                    }
                }
                if (f.crops != null)
                {
                    foreach (var c in f.crops)
                    {
                        rf.fieldCrops = rf.fieldCrops + (string.IsNullOrEmpty(c.cropOther) ? _sd.GetCrop(Convert.ToInt32(c.cropId)).cropname : c.cropOther) + " ";
                    }
                }
                rvm.fields.Add(rf);
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportFields.cshtml", rvm);

            return result;
        }
        public async Task<string> RenderSources()
        {
            ReportSourcesViewModel rvm = new ReportSourcesViewModel();
            rvm.details = new List<ReportSourcesDetail>();

            List<Field> fldList = _ud.GetFields();
            foreach(var f in fldList)
            {
                if (f.nutrients != null)
                {
                    if (f.nutrients.nutrientManures != null)
                    {
                        foreach (var m in f.nutrients.nutrientManures)
                        {
                            Models.StaticData.Manure manure = _sd.GetManure(m.manureId);
                            ReportSourcesDetail rd = rvm.details.FirstOrDefault(d => d.nutrientName == manure.name);
                            if (rd != null)
                            {
                                rd.nutrientAmount = rd.nutrientAmount + (m.rate * f.area);
                            }
                            else
                            {
                                rd = new ReportSourcesDetail();
                                rd.nutrientName = manure.name;
                                rd.nutrientUnit = _sd.GetUnit(m.unitId).name;
                                int index = rd.nutrientUnit.LastIndexOf("/");
                                if (index > 0)
                                    rd.nutrientUnit = rd.nutrientUnit.Substring(0, index);
                                rd.nutrientAmount = rd.nutrientAmount + (m.rate * f.area);
                                rvm.details.Add(rd);
                            }
                        }
                    }
                }
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportSources.cshtml", rvm);

            return result;
        }
        public async Task<string> RenderAnalysis()
        {
            ReportViewModel rvm = new ReportViewModel();

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportAnalysis.cshtml", rvm);

            return result;
        }
        public async Task<string> RenderApplication()
        {
            ReportApplicationViewModel rvm = new ReportApplicationViewModel();
            rvm.fields = new List<ReportApplicationField>();

            List<Field> fldList = _ud.GetFields();
            foreach(var f in fldList)
            {
                ReportApplicationField rf = new ReportApplicationField();
                rf.fieldName = f.fieldName;
                rf.fieldComment = f.comment;
                rf.nutrients = new List<ReportFieldNutrient>();
                if (f.nutrients != null)
                {
                    if (f.nutrients.nutrientManures != null)
                    {
                        foreach (var m in f.nutrients.nutrientManures)
                        {
                            Models.StaticData.Manure manure = _sd.GetManure(m.manureId);
                            ReportFieldNutrient rfn = new ReportFieldNutrient();

                            rfn.nutrientName = manure.name;
                            rfn.nutrientAmount = m.rate;
                            rfn.nutrientSeason = _sd.GetApplication(m.applicationId.ToString()).season;
                            rfn.nutrientApplication = _sd.GetApplication(m.applicationId.ToString()).application_method;
                            rfn.nutrientUnit = _sd.GetUnit(m.unitId).name;
                            rf.nutrients.Add(rfn);
                        }
                    }
                }
                if(f.crops != null)
                {
                    foreach(var c in f.crops)
                    {
                        rf.fieldCrops = rf.fieldCrops + (string.IsNullOrEmpty(c.cropOther) ? _sd.GetCrop(Convert.ToInt32(c.cropId)).cropname : c.cropOther) + " ";
                    }
                }
                rvm.fields.Add(rf);
            }

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportApplication.cshtml", rvm);

            return result;
        }
        public async Task<string> RenderSummary()
        {
            ReportViewModel rvm = new ReportViewModel();

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportSummary.cshtml", rvm);

            return result;
        }
        public async Task<string> RenderSheets()
        {
            ReportViewModel rvm = new ReportViewModel();

            var result = await _viewRenderService.RenderToStringAsync("~/Views/Report/ReportSheets.cshtml", rvm);

            return result;
        }
        public async Task<FileContentResult> BuildPDF(INodeServices nodeServices, PDFRequest rawdata)
        {
            JObject options = JObject.Parse(rawdata.options);
            JSONResponse result = null;

            // execute the Node.js component to generate a PDF
            result = await nodeServices.InvokeAsync<JSONResponse>("././pdf.js", rawdata.html, options);

            return new FileContentResult(result.data, "application/pdf");
        }
    }
}