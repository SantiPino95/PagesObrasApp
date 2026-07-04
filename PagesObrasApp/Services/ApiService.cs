using System.Net.Http.Json;

namespace PagesObrasApp.Services
{
    // Envoltorio genérico para no repetir el manejo de HttpClient en cada servicio
    public interface IApiService
    {
        Task<T?> GetAsync<T>(string endpoint);
        Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data);
        Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data);
        Task<HttpResponseMessage> DeleteAsync(string endpoint);
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            // "API" es el nombre que ya registraste en Program.cs con AddHttpClient("API", ...)
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
            => await _httpClient.PostAsJsonAsync(endpoint, data);

        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
            => await _httpClient.PutAsJsonAsync(endpoint, data);

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
            => await _httpClient.DeleteAsync(endpoint);
    }
}
