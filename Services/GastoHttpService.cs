using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IGastoHttpService
    {
        Task<List<GastoListadoDto>?> ObtenerGastosAsync();
        Task<List<GastoListadoDto>?> ObtenerGastosPorObraAsync(int idObra);
        Task<bool> CrearGastoAsync(CrearGastoDto dto);
        Task<bool> ActualizarGastoAsync(int id, ActualizarGastoDto dto);
        Task<bool> EliminarGastoAsync(int id);
    }

    public class GastoHttpService : IGastoHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Gasto";

        public GastoHttpService(IApiService api) => _api = api;

        public async Task<List<GastoListadoDto>?> ObtenerGastosAsync()
        { return await _api.GetAsync<List<GastoListadoDto>>(Endpoint); }

        public async Task<List<GastoListadoDto>?> ObtenerGastosPorObraAsync(int idObra)

        { return await _api.GetAsync<List<GastoListadoDto>>($"{Endpoint}/obra/{idObra}"); }

        public async Task<bool> CrearGastoAsync(CrearGastoDto dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarGastoAsync(int id, ActualizarGastoDto dto)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarGastoAsync(int id)
        {
            var response = await _api.DeleteAsync($"{Endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}