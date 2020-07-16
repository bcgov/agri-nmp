using Agri.Models;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.ViewModels;

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

        [HttpPost]
        public IActionResult CallRefreshOfNextPreviousNavigation(CoreSiteActions currentAction)
        {
            string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            string url = Url.Action("RefreshNextPreviousNavigation", controllerName, new { currentAction = currentAction });
            return Json(new { success = true, url = url, target = "#next-previous-navigation" });
        }

        public IActionResult RefreshNextPreviousNavigation(CoreSiteActions currentAction)
        {
            return ViewComponent("NextPreviousNavigation", new NextPrevNavViewModel { CurrentAction = currentAction });
        }
    }
}