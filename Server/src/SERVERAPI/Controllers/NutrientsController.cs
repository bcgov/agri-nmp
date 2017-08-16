using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.ViewModels;
using SERVERAPI.Models;

namespace SERVERAPI.Controllers
{
    public class NutrientsController : Controller
    {
        private IHostingEnvironment _env;

        public NutrientsController(IHostingEnvironment env)
        {
            _env = env;
        }
        // GET: /<controller>/
        public IActionResult Calculate()
        {
            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            Models.Impl.StaticData sd = new Models.Impl.StaticData();

            FarmViewModel fvm = new FarmViewModel();

            return View(fvm);
        }
        [HttpPost]
        public IActionResult Calculate(FarmViewModel fvm)
        {

            return View(fvm);
        }
    }
}