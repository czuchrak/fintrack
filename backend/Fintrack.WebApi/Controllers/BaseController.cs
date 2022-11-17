using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fintrack.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : Controller
{
    protected string UserId { get; private set; }

    public override void OnActionExecuting(ActionExecutingContext ctx)
    {
        base.OnActionExecuting(ctx);
        UserId = GetUserId(HttpContext);
    }

    private static string GetUserId(HttpContext context)
    {
        return GetClaimValue(context, "user_id");
    }

    protected static string GetEmail(HttpContext context)
    {
        return GetClaimValue(context, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
    }

    private static string GetClaimValue(HttpContext context, string type)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        return identity.Claims.First(x => x.Type == type).Value;
    }
}