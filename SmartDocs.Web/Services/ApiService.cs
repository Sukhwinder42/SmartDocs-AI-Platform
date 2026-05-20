using Newtonsoft.Json;
using System.Text;

namespace SmartDocs.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            //_httpClient.BaseAddress =
            //    new Uri("https://localhost:7031/");
            _httpClient.BaseAddress =
    new Uri("https://smartdocs-ai-platform.onrender.com/");
        }


        public async Task<T> PostAsync<T>(
    string endpoint,
    object data,
    string token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }

            var json =
                JsonConvert.SerializeObject(data);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _httpClient.PostAsync(
                    endpoint,
                    content);

            var result =
                await response.Content.ReadAsStringAsync();

            // Handle API errors
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }

            // If T is string, return raw result
            //if (typeof(T) == typeof(string))
            //{
            //    return (T)(object)result;
            //}
            if (typeof(T) == typeof(string))
            {
                return (T)(object)result.Replace("\0", "");
            }

            // Otherwise deserialize JSON
            return JsonConvert.DeserializeObject<T>(result);
        }


        public async Task<T> GetAsync<T>(
            string endpoint,
            string token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers
                    .AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }

            var response =
                await _httpClient.GetAsync(endpoint);

            //var result =
            //    await response.Content
            //        .ReadAsStringAsync();

            //return JsonConvert
            //    .DeserializeObject<T>(result);

            var result =
    await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }

            // DEBUG
            Console.WriteLine(result);

            if (string.IsNullOrWhiteSpace(result))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(result);
        }


    }
}
