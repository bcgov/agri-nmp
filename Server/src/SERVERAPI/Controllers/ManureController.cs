using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;


namespace SERVERAPI.Controllers
{
    public class ManureController : BaseController
    {
        private IHostingEnvironment _env;

        public ManureController(IHostingEnvironment env)
        {
            _env = env;
        }
        // GET: /<controller>/
        public IActionResult Manure()
        {
            return View();
        }
    }
}
