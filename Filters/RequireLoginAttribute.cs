using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class RequireLoginAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        var path = context.HttpContext.Request.Path;

        if (!(path.StartsWithSegments("/Login") || path.StartsWithSegments("/Account")))
        {
            if (string.IsNullOrEmpty(session.GetString("login")))
            {
                context.Result = new RedirectToActionResult("Form", "Login", null);
            }
        }
        if (path.StartsWithSegments("/User"))
        {
            if (string.IsNullOrEmpty(session.GetString("role")))
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
        if (!string.IsNullOrEmpty(session.GetString("role")) &&
            !path.StartsWithSegments("/User") &&
            !path.StartsWithSegments("/Login/LogOut")){
            context.Result = new RedirectToActionResult("Index", "User", null);
        }
        base.OnActionExecuting(context);
    }
}
