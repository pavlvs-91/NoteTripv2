using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class RequireLoginAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        var path = context.HttpContext.Request.Path;
        

        var allowedPaths = new[] { "/Login", "/Account", "/api", "/Login/LogOut" };
        if (!allowedPaths.Any(p => path.StartsWithSegments(p)))
        {
            if (string.IsNullOrEmpty(session.GetString("login")))
            {
                context.Result = new RedirectToActionResult("Form", "Login", null);
                return;
            }
        }

        if (path.StartsWithSegments("/User") && string.IsNullOrEmpty(session.GetString("role")))
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
            return;
        }

        if (!string.IsNullOrEmpty(session.GetString("role")) 
            && !path.StartsWithSegments("/User") 
            && !path.StartsWithSegments("/Login/LogOut"))
        {
            context.Result = new RedirectToActionResult("Index", "User", null);
            return;
        }

        base.OnActionExecuting(context);
    }

}
