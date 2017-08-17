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

        public IActionResult ManureDetails(int? id)
        {
            ManureDetailsViewModel mvm = new ManureDetailsViewModel();

            mvm.act = id == null ? "Add" : "Edit";
            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            Models.Impl.StaticData sd = new Models.Impl.StaticData();


            mvm.manOptions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            mvm.manOptions.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Beef", Value = "1" });
            mvm.manOptions.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Chicken", Value = "2" });
            mvm.manOptions.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Turkey", Value = "3" });

            mvm.applOptions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            mvm.applOptions.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Spring - spray", Value = "1" });
            mvm.applOptions.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Spring - broadcast", Value = "2" });
            mvm.applOptions.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Fall - spray", Value = "3" });
            mvm.applOptions.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Fall - braodcast", Value = "4" });

            mvm.rateOptions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            mvm.rateOptions.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "gal/ac", Value = "1" });
            mvm.rateOptions.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "lbs/ac", Value = "2" });

            return View(mvm);
        }
    }
}