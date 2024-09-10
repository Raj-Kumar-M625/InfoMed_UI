namespace InfoMed.Services.Interface
{
    public interface IApiService
    {
        Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string url, HttpContent content = null);
    }
}
