using Microsoft.AspNetCore.Mvc;
using SmartDocs.Web.Services;
using SmartDocs.Web.ViewModels;

namespace SmartDocs.Web.Controllers
{
    public class AiController : Controller
    {
        private readonly ApiService _apiService;

        public AiController(ApiService apiService)
        {
            _apiService = apiService;
        }

      
        public async Task<IActionResult> Chat(Guid id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            var history =
                await _apiService.GetAsync<List<dynamic>>(
                    $"api/ai/history/{id}",
                    token);

            var model = new AskQuestionViewModel
            {
                DocumentId = id,
                ChatHistory = history.Select(x => new ChatMessageVm
                {
                    User = x.user,
                    AI = x.ai
                }).ToList()
            };

            return View(model);
        }


      

        [HttpPost]
        public async Task<IActionResult> Chat(AskQuestionViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");

            var response =
                await _apiService.PostAsync<ChatResponseDto>(
                    "api/ai/ask",
                    new
                    {
                        documentId = model.DocumentId,
                        question = model.Question
                    },
                    token);

            model.Answer = response.Answer;

            // reload history after new question
            var history =
                await _apiService.GetAsync<List<dynamic>>(
                    $"api/ai/history/{model.DocumentId}",
                    token);

            model.ChatHistory = history.Select(x => new ChatMessageVm
            {
                User = x.user,
                AI = x.ai
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AskAjax([FromBody] AskQuestionViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");

            var response =
                await _apiService.PostAsync<ChatResponseDto>(
                    "api/ai/ask",
                    new
                    {
                        documentId = model.DocumentId,
                        question = model.Question
                    },
                    token);

            return Json(new
            {
                answer = response.Answer
            });
        }

    }

}
