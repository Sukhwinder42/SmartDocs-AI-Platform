using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Application.Interfaces
{
    public interface IGeminiService
    {
        Task<string> GenerateSummaryAsync(string text);

        Task<string> AskQuestionAsync(string documentText, string question);

        Task<float[]> GenerateEmbeddingAsync(string text);
    }
}
