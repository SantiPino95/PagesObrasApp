using PagesObrasApp.Models;
using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IObraHttpService
    {
        Task<List<ObraAdminListadoDto>?> ObtenerObrasAsync();
        Task<ObraAdminListadoDto?> ObtenerObraPorIdAsync(int id);
        Task<bool> CrearObraAsync(CrearObraDto dto);

        Task<bool> ExisteObraPorCodigoAsync(string codigoPublico);

        Task<bool> ActualizarObraAsync(int id, ObraAdminListadoDto dto);
    }

    public class ObraHttpService : IObraHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Obras";

        public ObraHttpService(IApiService api)
        {
            _api = api;
        }

        public Task<List<ObraAdminListadoDto>?> ObtenerObrasAsync()
            => _api.GetAsync<List<ObraAdminListadoDto>?>(Endpoint);

        public Task<ObraAdminListadoDto?> ObtenerObraPorIdAsync(int id)
            => _api.GetAsync<ObraAdminListadoDto?>($"{Endpoint}/{id}");

        public Task<bool> ExisteObraPorCodigoAsync(string codigoPublico)
            => _api.GetAsync<bool>($"{Endpoint}/existe/{codigoPublico}");

        public async Task<bool> CrearObraAsync(CrearObraDto dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }



        public async Task<bool> ActualizarObraAsync(int id, ObraAdminListadoDto dto)
        {
            // Esto hace un PUT a api/Obras/5 enviando el objeto con el nuevo avance y estado
            var response = await _api.PutAsync($"{Endpoint}/{id}", dto);
            return response.IsSuccessStatusCode;
        }
    }
}