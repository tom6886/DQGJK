using DQGJK.Models;
using DQGJK.Web.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace DQGJK.Web.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Guser user = HttpContext.Session.Get<Guser>("SESSION-ACCOUNT-KEY");

            if (user == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    filterContext.Result = new StatusCodeResult(499);
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" }));
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}