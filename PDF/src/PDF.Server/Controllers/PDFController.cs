using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Microsoft.EntityFrameworkCore;
using PDF.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.NodeServices;
using Newtonsoft.Json.Linq;

namespace PDF.Controllers
{
    public class PDFRequest
    {
        public string html { get; set; }
        public string options { get; set; }
    }
    public class JSONResponse
    {
        public string type;
        public byte[] data;
    }
    [Route("api/[controller]")] 
    public class PDFController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly DbAppContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string userId;
        private string guid;
        private string directory;

        protected ILogger _logger;

        public PDFController(IHttpContextAccessor httpContextAccessor, IConfigurationRoot configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;

            _httpContextAccessor = httpContextAccessor;

            _logger = loggerFactory.CreateLogger(typeof(PDFController));
        }

        [HttpPost]
        [Route("BuildPDF")]

        public async Task<IActionResult> BuildPDF([FromServices] INodeServices nodeServices, [FromBody]  PDFRequest rawdata )
        {
            //JObject options = JObject.Parse(rawdata.options);

            // execute the Node.js component to generate a PDF
            JSONResponse result = await nodeServices.InvokeAsync<JSONResponse>("./pdf", rawdata.html, JObject.Parse(rawdata.options));
            //options = null;

            return new FileContentResult(result.data, "application/pdf");
        }

        protected HttpRequest Request
        {
            get { return _httpContextAccessor.HttpContext.Request; }
        }
    }
}
