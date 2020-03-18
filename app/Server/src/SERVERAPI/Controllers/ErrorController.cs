using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;
using Newtonsoft.Json;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;

namespace SERVERAPI.Controllers
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ErrorHandlingMiddleware");
                //await HandleExceptionAsync(context, ex);
                throw;
            }
        }

        //    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        //    {
        //        //var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        //        var exception2 = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        //        //if (exception is MyNotFoundException) code = HttpStatusCode.NotFound;
        //        //else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
        //        //else if (exception is MyException) code = HttpStatusCode.BadRequest;
        //        context.Response.StatusCode = 500;
        //        context.Response.ContentType = "text/html";

        //        //await context.Response.WriteAsync("<html><body>\r\n");
        //        //await context.Response.WriteAsync(exception.Message + "<br>\r\n");

        //        //await context.Response.WriteAsync("</body></html>\r\n");
        //        //await context.Response.WriteAsync(new string(' ', 512)); // Padding for IE
        //    }
    }

    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            var exception = HttpContext.Features
            .Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

            ViewData["statusCode"] = HttpContext.
                            Response.StatusCode;
            ViewData["message"] = exception.Error.Message;
            ViewData["stackTrace"] = exception.
                           Error.StackTrace;

            return View();
        }

        public IActionResult SessionExpired()
        {
            return View();
        }
    }
}