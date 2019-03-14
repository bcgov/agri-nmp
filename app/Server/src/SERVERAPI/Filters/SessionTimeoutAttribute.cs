using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace SERVERAPI.Filters
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string x = context.HttpContext.Session.GetString("active");
            if (x == null)
            {
                context.Result = new RedirectToActionResult("SessionExpired", "Error", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
