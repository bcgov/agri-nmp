using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models;
using SERVERAPI.Models.Impl;
using SERVERAPI.Utility;
using SERVERAPI.ViewModels;
using static SERVERAPI.Models.StaticData;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SERVERAPI.Controllers
{
    public class ManureManagementController : Controller
    {
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public Models.Impl.StaticData _sd { get; set; }
        public IViewRenderService _viewRenderService { get; set; }
        public AppSettings _settings;

        public ManureManagementController(IHostingEnvironment env, IViewRenderService viewRenderService, UserData ud,
            Models.Impl.StaticData sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
            _viewRenderService = viewRenderService;
        }

        [HttpGet]
        public IActionResult ManureGeneratedObtained()
        {
            return View();
        }

        public IActionResult ManureGeneratedObtainedDetail(int? id, string target)
        {
            ManureManagementViewModel mm = new ManureManagementViewModel();
            mm.animalType = "";
            mm.subType = "";
            mm.manureType = "";
            return PartialView("ManureGeneratedObtainedDetail", mm);
        }
    }
}
