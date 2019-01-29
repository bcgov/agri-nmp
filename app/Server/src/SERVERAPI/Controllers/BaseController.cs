using Agri.Models;
using Microsoft.AspNetCore.Mvc;

namespace SERVERAPI.Controllers
{
    public class BaseController : Controller
    {
        [HttpPost]
        public IActionResult CallRefreshOfNavigation(CoreSiteActions currentAction)
        {
            string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            string url = Url.Action("RefreshNavigation", controllerName, new { currentAction = currentAction });
            return Json(new { success = true, url = url, target = "#navigation-container" });
        }

        public IActionResult RefreshNavigation(CoreSiteActions currentAction)
        {
            return ViewComponent("Navigation", new { currentAction = currentAction });
        }
    }
}