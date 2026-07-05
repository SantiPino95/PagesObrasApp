using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface INovedadHttpService
    {
        Task<List<NovedadListadoDto>?> ObtenerNovedadesAsync();
        Task<List<NovedadListadoDto>?> ObtenerNovedadesPorObraAsync(int idObra);
        Task<bool> CrearNovedadAsync(CrearNovedadDto dto);
        Task<bool> CambiarEstadoAsync(int id, string nuevoEstado);
    }

    public class NovedadHttpService : INovedadHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Novedades";

        public NovedadHttpService(IApiService api) => _api = api;

        public Task<List<NovedadListadoDto>?> ObtenerNovedadesAsync()
            => _api.GetAsync<List<NovedadListadoDto>>(Endpoint);

        public Task<List<NovedadListadoDto>?> ObtenerNovedadesPorObraAsync(int idObra)
            => _api.GetAsync<List<NovedadListadoDto>>($"{Endpoint}/obra/{idObra}");

        public async Task<bool> CrearNovedadAsync(CrearNovedadDto dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CambiarEstadoAsync(int id, string nuevoEstado)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}/estado", new { Estado = nuevoEstado });
            return response.IsSuccessStatusCode;
        }
    }
}