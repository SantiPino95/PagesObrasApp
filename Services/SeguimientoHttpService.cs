using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface ISeguimientoHttpService
    {
        Task<List<SeguimientoListadoDto>?> ObtenerPorObraAsync(int idObra);
        Task<bool> CrearSeguimientoAsync(CrearSeguimientoDto dto);
    }

    public class SeguimientoHttpService : ISeguimientoHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Seguimiento";

        public SeguimientoHttpService(IApiService api) => _api = api;

        public Task<List<SeguimientoListadoDto>?> ObtenerPorObraAsync(int idObra)
            => _api.GetAsync<List<SeguimientoListadoDto>>($"{Endpoint}/obra/{idObra}");

        public async Task<bool> CrearSeguimientoAsync(CrearSeguimientoDto dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }
    }
}