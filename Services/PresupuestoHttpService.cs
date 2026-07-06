using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IPresupuestoHttpService
    {
        Task<List<PresupuestoListadoDto>?> ObtenerPresupuestosAsync();
        Task<bool> CrearPresupuestoAsync(CrearPresupuestoDto dto);
        Task<bool> CambiarEstadoAsync(int id, string nuevoEstado);
    }

    public class PresupuestoHttpService : IPresupuestoHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Presupuestos";

        public PresupuestoHttpService(IApiService api) => _api = api;

        public Task<List<PresupuestoListadoDto>?> ObtenerPresupuestosAsync()
            => _api.GetAsync<List<PresupuestoListadoDto>>(Endpoint);

        public async Task<bool> CrearPresupuestoAsync(CrearPresupuestoDto dto)
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