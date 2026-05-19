using Microsoft.AspNetCore.Mvc;
using SmartDocs.Web.Services;
using SmartDocs.Web.ViewModels;

namespace SmartDocs.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;

        public AuthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // LOGIN PAGE
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            try
            {
                var response = await _apiService.PostAsync<dynamic>(
                    "api/auth/login",
                    model);

                if (response == null)
                {
                    ViewBag.Error = "Invalid server response";
                    return View(model);
                }

                HttpContext.Session.SetString(
                    "JWToken",
                    (string)response.token);

                return RedirectToAction("Dashboard", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }



        // REGISTER PAGE
        public IActionResult Register()
        {
            return View();
        }

        // REGISTER POST
        [HttpPost]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            try
            {
                var response = await _apiService.PostAsync<string>(
                    "api/auth/register",
                    model);

                TempData["Success"] = response;

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}