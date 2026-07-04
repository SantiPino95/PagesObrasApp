using PagesObrasApp.Models;

namespace PagesObrasApp.Services
{
    public interface IObraHttpService
    {
        Task<List<ObraListadoDto>?> ObtenerObrasAsync();
    }

    public class ObraHttpService : IObraHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Obras"; // Coincide con [Route("api/[controller]")] del ObrasController

        public ObraHttpService(IApiService api)
        {
            _api = api;
        }

        public Task<List<ObraListadoDto>?> ObtenerObrasAsync()
            => _api.GetAsync<List<ObraListadoDto>>(Endpoint);
    }
}
