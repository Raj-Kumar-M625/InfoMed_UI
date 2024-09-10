using InfoMed.Services.Interface;
using System.Net.Http.Headers;

namespace InfoMed.Services.Implementation
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string url, HttpContent content = null)
        {
            var request = new HttpRequestMessage(method, url);
            if (content != null)
            {
                request.Content = content;
            }

            var client = _httpClientFactory.CreateClient();

            // Retrieve the JWT token from the session or cookie
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["JWTToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await client.SendAsync(request);
        }
    }
}
