using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SERVERAPI.Pages
{
    public class BasePageModel : PageModel
    {
        public string Title { get; set; }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            string x = context.HttpContext.Session.GetString("active");
            if (x == null)
            {
                //context.Result = new RedirectToActionResult("SessionExpired", "Error", null);
                context.Result = RedirectToAction("SessionExpired", "Error", null);
            }
            base.OnPageHandlerExecuting(context);
        }
    }
}