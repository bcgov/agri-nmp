using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace SERVERAPI.Controllers
{
    [Authorize]
    [Route("api")]
    public class VersionController : Controller
    {
        // Hack in the git commit id.
        private const string _commitKey = "OPENSHIFT_BUILD_COMMIT";

        private readonly IConfiguration _configuration;

        public VersionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string CommitId
        {
            get
            {
                return _configuration[_commitKey];
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("version")]
        public virtual IActionResult GetServerVersionInfo()
        {
            ProductVersionInfo info = new ProductVersionInfo();
            info.ApplicationVersions.Add(GetApplicationVersionInfo());
            return Ok(info);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("server/version")]
        public virtual IActionResult GetServerVersion()
        {
            return Ok(GetApplicationVersionInfo());
        }

        private ApplicationVersionInfo GetApplicationVersionInfo()
        {
            Assembly assembly = this.GetType().GetTypeInfo().Assembly;
            return assembly.GetApplicationVersionInfo(this.CommitId);
        }
    }
}