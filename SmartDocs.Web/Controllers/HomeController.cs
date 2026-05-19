using Microsoft.AspNetCore.Mvc;
using SmartDocs.Web.Models;
using SmartDocs.Web.Services;
using SmartDocs.Web.ViewModels;
using System.Diagnostics;

namespace SmartDocs.Web.Controllers
{
    public class HomeController : BaseController
    {
      
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }



        public IActionResult Index()
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            return RedirectToAction(
                "Dashboard",
                "Home");
        }

        public async Task<IActionResult> Dashboard()
        {

            var token =
                HttpContext.Session.GetString("JWToken");

            // ?? BLOCK UNLOGGED USERS
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var stats =
                await _apiService.GetAsync<DashboardStatsViewModel>(
                    "api/dashboard/stats",
                    token);

            ViewBag.TotalDocuments = stats.Documents;
            ViewBag.TotalChats = stats.Chats;
            ViewBag.TotalSummaries = stats.Summaries;

            return View();
        }

    }
}
