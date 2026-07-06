using PagesObrasApp.Models;
using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IObraHttpService
    {
        Task<List<ObraAdminListadoDto>?> ObtenerObrasAsync();
        Task<bool> CrearObraAsync(CrearObraDto dto);
        Task<bool> ActualizarEstadoObraAsync(int idObra, string nuevoEstado);
        Task<bool> EliminarObraAsync(int idObra);
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

        public async Task<bool> CrearObraAsync(CrearObraDto dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarEstadoObraAsync(int idObra, string nuevoEstado)
        {
            var response = await _api.PutAsync($"{Endpoint}/{idObra}/estado", new { estado = nuevoEstado });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarObraAsync(int idObra)
        {
            var response = await _api.DeleteAsync($"{Endpoint}/{idObra}");
            return response.IsSuccessStatusCode;
        }
    }
}