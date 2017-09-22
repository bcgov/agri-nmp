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
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public Models.Impl.StaticData _sd { get; set; }
        public IViewRenderService _viewRenderService { get; set; }
        public AppSettings _settings;

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
            bool portrait = false;

            if(fields)
            {
                portrait = true;
            }

            FileContentResult result = null;
            //JSONResponse result = null;
            var pdfHost = Environment.GetEnvironmentVariable("PDF_SERVICE_NAME");

            //string pdfHost = "http://localhost:54611";

            string targetUrl = pdfHost + "/api/PDF/BuildPDF";

            PDF_Options options = new PDF_Options();
            options.border = new PDF_Border();
            options.header = new PDF_Header();
            options.footer = new PDF_Footer();

            options.type = "pdf";
            options.quality = "75";
            options.format = "letter";
            options.orientation = (portrait)? "portrait" : "landscape";
            options.border.top = ".25in";
            options.border.right = ".25in";
            options.border.bottom = ".25in";
            options.border.left = ".25in";
            options.header.height = "25mm";
            options.header.contents = "Farm Name: " + _ud.FarmDetails().farmName + "<br />" +
                                      "Planning Year: " + _ud.FarmDetails().year;
            options.footer.height = "20mm";
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

                //FileContentResult res = await BuildPDF(nodeServices, req);

                //return res;

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
                else
                {
                    string errorMsg = "Url: " + targetUrl + "\r\n" +
                                      "Result: " + response.ToString();
                    result = new FileContentResult(Encoding.ASCII.GetBytes(errorMsg), "text/plain");
                }
            }
            catch (Exception e)
            {
                string errorMsg = "Exception " + "\r\n" +
                                  "Url: " + targetUrl + "\r\n" +
                                  "Result: " + e.Message + "\r\n" +
                                  "Result: " + e.InnerException.Message;
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
            rvm.year = _ud.FarmDetails().year;

            List<Field> fldList = _ud.GetFields();
            foreach (var f in fldList)
            {
                ReportFieldsField rf = new ReportFieldsField();
                rf.fieldArea = f.area.ToString();
                rf.fieldName = f.fieldName;
                rf.fieldComment = f.comment;
                rf.soiltest = new ReportFieldSoilTest();
                rf.crops = new List<ReportFieldCrop>();
                rf.otherNutrients = new List<ReportFieldOtherNutrient>();

                if(f.soilTest != null)
                {
                    rf.soiltest.methodName = _sd.GetSoilTestMethod(_ud.FarmDetails().testingMethod);
                    rf.soiltest.sampleDate = f.soilTest.sampleDate.ToString("MMM yyyy");
                    rf.soiltest.dispNO3H = f.soilTest.valNO3H.ToString() + " ppm";
                    rf.soiltest.dispP = f.soilTest.ValP.ToString() + " ppm";
                    rf.soiltest.dispK = f.soilTest.valK.ToString() + " ppm";
                    rf.soiltest.dispPH = f.soilTest.valPH.ToString();
                }

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
                            rfn.reqN = m.yrN;
                            rfn.reqP = m.yrP2o5;
                            rfn.reqK = m.yrK2o;
                            rfn.remN = m.ltN;
                            rfn.remP = m.ltP2o5;
                            rfn.remK = m.ltK2o;
                            rf.nutrients.Add(rfn);

                            rf.reqN = rf.reqN + rfn.reqN;
                            rf.reqP = rf.reqP + rfn.reqP;
                            rf.reqK = rf.reqK + rfn.reqK;
                            rf.remN = rf.remN + rfn.remN;
                            rf.remP = rf.remP + rfn.remP;
                            rf.remK = rf.remK + rfn.remK;
                        }
                    }
                    if(f.nutrients.nutrientOthers != null)
                    {
                        foreach(var o in f.nutrients.nutrientOthers)
                        {
                            ReportFieldOtherNutrient fon = new ReportFieldOtherNutrient();
                            fon.otherName = o.description;
                            fon.reqN = o.nitrogen;
                            fon.reqP = o.phospherous;
                            fon.reqK = o.potassium;
                            fon.remN = o.nitrogen;
                            fon.remP = o.phospherous;
                            fon.remK = o.potassium;
                            rf.otherNutrients.Add(fon);

                            rf.reqN = rf.reqN + fon.reqN;
                            rf.reqP = rf.reqP + fon.reqP;
                            rf.reqK = rf.reqK + fon.reqK;
                            rf.remN = rf.remN + fon.remN;
                            rf.remP = rf.remP + fon.remP;
                            rf.remK = rf.remK + fon.remK;
                        }
                    }
                }
                if (f.crops != null)
                {
                    foreach (var c in f.crops)
                    {
                        ReportFieldCrop fc = new ReportFieldCrop();

                        fc.cropname = string.IsNullOrEmpty(c.cropOther) ? _sd.GetCrop(Convert.ToInt32(c.cropId)).cropname : c.cropOther;
                        if (c.coverCropHarvested.HasValue)
                        {
                            fc.cropname = c.coverCropHarvested.Value ? fc.cropname + "(harvested)" : fc.cropname;
                        }

                        fc.yield = c.yield;
                        fc.reqN =  -c.reqN;
                        fc.reqP =  -c.reqP2o5;
                        fc.reqK =  -c.reqK2o;
                        fc.remN =  -c.remN;
                        fc.remP =  -c.remP2o5;
                        fc.remK =  -c.remK2o;

                        rf.reqN = rf.reqN + fc.reqN;
                        rf.reqP = rf.reqP + fc.reqP;
                        rf.reqK = rf.reqK + fc.reqK;
                        rf.remN = rf.remN + fc.remN;
                        rf.remP = rf.remP + fc.remP;
                        rf.remK = rf.remK + fc.remK;

                        rf.fieldCrops = rf.fieldCrops + fc.cropname + " ";

                        rf.crops.Add(fc);
                    }
                }
                ChemicalBalanceMessage cbm = new ChemicalBalanceMessage(_ud, _sd);

                rf.alertMsgs = cbm.DetermineBalanceMessages(f.fieldName);

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
            result = await nodeServices.InvokeAsync<JSONResponse>("./pdf.js", rawdata.html, options);

            return new FileContentResult(result.data, "application/pdf");
        }
    }
}