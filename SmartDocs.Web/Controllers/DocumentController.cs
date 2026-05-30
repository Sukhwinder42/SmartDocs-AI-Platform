using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartDocs.Web.Services;
using SmartDocs.Web.ViewModels;

namespace SmartDocs.Web.Controllers
{
    public class DocumentController : Controller
    {
        private readonly ApiService _apiService;

        public DocumentController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // DOCUMENT LIST
        public async Task<IActionResult> List()
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            // 🔥 BLOCK UNLOGGED USERS
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var documents =
                await _apiService.GetAsync<
                    List<DocumentViewModel>>(
                        "api/document",
                        token);

            return View(documents);
        }

        // UPLOAD PAGE
        public IActionResult Upload()
        {
            return View();
        }

        // UPLOAD POST
        [HttpPost]
        public async Task<IActionResult> Upload(
            IFormFile file)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            // BLOCK UNLOGGED USERS
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            using var form =
                new MultipartFormDataContent();

            using var stream =
                file.OpenReadStream();

            form.Add(
                new StreamContent(stream),
                "file",
                file.FileName);


            var response = await client.PostAsync(
             "https://smartdocs-ai-platform.onrender.com/api/document/upload",
             form);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Upload failed.";
                return RedirectToAction("Upload");
            }

            // READ RESPONSE
            var json =
                await response.Content.ReadAsStringAsync();

            var uploadedDoc =
                Newtonsoft.Json.JsonConvert.DeserializeObject<DocumentViewModel>(json);

            // CALL SUMMARIZE API
            await client.PostAsync(
                $"https://smartdocs-ai-platform.onrender.com/api/ai/summarize/{uploadedDoc.Id}",
                null);

            return RedirectToAction("List");
        }
    }
}
