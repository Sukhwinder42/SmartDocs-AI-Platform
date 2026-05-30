using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmartDocs.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartDocs.Infrastructure.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GenerateSummaryAsync(string text)
        {
            return await CallGeminiAsync(
                $"Summarize this document:\n\n{text}"
            );
        }

        public async Task<string> AskQuestionAsync(string documentText, string question)
        {
            //return await CallGeminiAsync(
            //    $"Document:\n{documentText}\n\nQuestion: {question}"
            //);

            return await CallGeminiAsync(
            $"""
            You are a document assistant.

            Answer ONLY using the provided context.

            If answer is not found,
            say:
            'Information not found in document.'

            Context:
            {documentText}

            Question:
            {question}
            """
            );
        }

        // 🔥 CENTRAL SAFE METHOD
        private async Task<string> CallGeminiAsync(string prompt)
        {
            try
            {
                var apiKey = _configuration["SmartDocsAPI_GEMINI_API_KEY"];

                var endpoint =
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync(endpoint, content);

                var responseString = await response.Content.ReadAsStringAsync();

                // ❗ CHECK HTTP SUCCESS FIRST
                if (!response.IsSuccessStatusCode)
                {
                    if ((int)response.StatusCode == 503)
                        throw new Exception("AI service is currently busy. Please try again later.");

                    throw new Exception("AI service temporarily unavailable.");
                }

                dynamic result = JsonConvert.DeserializeObject(responseString);

                // ❗ SAFE NULL CHECKS
                var text =
                    result?.candidates?[0]?.content?.parts?[0]?.text;

                if (text == null)
                {
                    return "Gemini returned empty response.";
                }

                return text.ToString();
            }
            catch (Exception ex)
            {
                return $"Gemini Service Exception: {ex.Message}";
            }
        }

        //public async Task<float[]> GenerateEmbeddingAsync(string text)
        //{
        //    // TEMP MOCK EMBEDDINGS

        //    Random random = new Random();

        //    return Enumerable.Range(0, 768)
        //        .Select(x => (float)random.NextDouble())
        //        .ToArray();
        //}

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            var apiKey =
                _configuration["SmartDocsAPI_GEMINI_API_KEY"];

            var endpoint =
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-embedding-001:embedContent?key={apiKey}";

            var body = new
            {
                content = new
                {
                    parts = new[]
                    {
                new
                {
                    text = text
                }
            }
                }
            };

            var json =
                JsonConvert.SerializeObject(body);

            var response =
                await _httpClient.PostAsync(
                    endpoint,
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json"));


            //just for checking error details from Gemini
            //response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Gemini Embedding Error ({response.StatusCode}): {responseBody}");
            }






            var result =
                JsonConvert.DeserializeObject<dynamic>(
                    await response.Content.ReadAsStringAsync());

            var values =
                result.embedding.values;

            return ((IEnumerable<dynamic>)values)
                .Select(x => (float)x)
                .ToArray();
        }
    }
}