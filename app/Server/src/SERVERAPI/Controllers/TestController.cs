/*
 
 *
 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SERVERAPI.Services.Impl;

namespace SERVERAPI.Controllers
{
    /// <summary>
    /// Test Controller
    /// </summary>
    /// <remarks>
    /// Provides examples of how to apply permission checks.
    /// </remarks>
    [Route("api/test")]
    public class TestController : Controller
    {
        private readonly ITestService _service;

        public TestController(ITestService service)
        {
            _service = service;
        }

        /// <summary>
        /// Echoes headers for trouble shooting purposes.
        /// </summary>
        [HttpGet]
        [Route("headers")]
        [Produces("text/html")]
        public virtual IActionResult EchoHeaders()
        {
            return _service.EchoHeaders();
        }
        
    }

    public interface ITestService
    {
        IActionResult EchoHeaders();

     
    }

    /// <summary>
    /// TestService, the service implementation for <see cref="TestController"/>
    /// </summary>
    /// <remarks>
    /// Provides an example of how to split up the controller and service implementation while still being able to apply permissions 
    /// checks to the authenticated user.
    /// </remarks>
    public class TestService : ServiceBase, ITestService
    {
        public TestService(ILogger<TestService> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
        {
            // Just pass things along to the base class.
        }

        /// <summary>
        /// Echoes headers for trouble shooting purposes.
        /// </summary>
        public IActionResult EchoHeaders()
        {
            return Ok(Request.Headers.ToHtml());
        }
        
    }
}
