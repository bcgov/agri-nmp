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

namespace PDF.Controllers
{
    public class PDFRequest
    {
        public string html { get; set; }
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

            userId = getFromHeaders("SM_UNIVERSALID");
            guid = getFromHeaders("SMGOV_USERGUID");
            directory = getFromHeaders("SM_AUTHDIRNAME");

            _logger = loggerFactory.CreateLogger(typeof(PDFController));
        }

        private string getFromHeaders(string key)
        {
            string result = null;
            if (Request.Headers.ContainsKey(key))
            {
                result = Request.Headers[key];
            }
            return result;
        }

        [HttpPost]
        [Route("BuildPDF")]

        public async Task<IActionResult> BuildPDF([FromServices] INodeServices nodeServices, [FromBody]  PDFRequest rawdata )
        {
            JSONResponse result = null;
            var options = new { format="letter", orientation= "landscape" };

            // execute the Node.js component to generate a PDF
            result = await nodeServices.InvokeAsync<JSONResponse>("./pdf.js", rawdata.html, options);

            return new FileContentResult(result.data, "application/pdf");
        }

        protected HttpRequest Request
        {
            get { return _httpContextAccessor.HttpContext.Request; }
        }
    }
}
