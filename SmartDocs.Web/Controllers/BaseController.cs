using Microsoft.AspNetCore.Mvc;

namespace SmartDocs.Web.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(
                HttpContext.Session.GetString("JWToken"));
        }

        protected string GetToken()
        {
            return HttpContext.Session.GetString("JWToken");
        }
    }
}
