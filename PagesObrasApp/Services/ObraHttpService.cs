using PagesObrasApp.Models;

namespace PagesObrasApp.Services
{
    public interface IObraHttpService
    {
        Task<List<ObraListadoDto>?> ObtenerObrasAsync();
        Task<bool> CrearObraAsync(CrearObraDto dto);
    }

    public class ObraHttpService : IObraHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Obras";

        public ObraHttpService(IApiService api)
        {
            _api = api;
        }

        public Task<List<ObraListadoDto>?> ObtenerObrasAsync()
            => _api.GetAsync<List<ObraListadoDto>>(Endpoint);

        public async Task<bool> CrearObraAsync(CrearObraDto dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }
    }
}