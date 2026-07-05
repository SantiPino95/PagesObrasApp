using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IHerramientaHttpService
    {
        Task<List<HerramientaListadoDto>?> ObtenerHerramientasAsync();
        Task<HerramientaListadoDto?> ObtenerHerramientaPorIdAsync(int id);
        Task<bool> CrearHerramientaAsync(CrearHerramientaDto dto);
        Task<bool> ActualizarHerramientaAsync(int id, CrearHerramientaDto dto);
        Task<bool> EliminarHerramientaAsync(int id);
        Task<bool> AsignarSalidaAsync(int idHerramienta, int idObra);
        Task<bool> RegistrarDevolucionAsync(int idHerramienta, int idObra);
    }

    public class HerramientaHttpService : IHerramientaHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Herramienta";

        public HerramientaHttpService(IApiService api)
        {
            _api = api;
        }

        public Task<List<HerramientaListadoDto>?> ObtenerHerramientasAsync()
            => _api.GetAsync<List<HerramientaListadoDto>>(Endpoint);

        public Task<HerramientaListadoDto?> ObtenerHerramientaPorIdAsync(int id)
            => _api.GetAsync<HerramientaListadoDto>($"{Endpoint}/{id}");

        public async Task<bool> CrearHerramientaAsync(CrearHerramientaDto dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarHerramientaAsync(int id, CrearHerramientaDto dto)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarHerramientaAsync(int id)
        {
            var response = await _api.DeleteAsync($"{Endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AsignarSalidaAsync(int idHerramienta, int idObra)
        {
            // ✅ El endpoint correcto según el Controller
            var response = await _api.PostAsync($"{Endpoint}/asignar-salida?idHerramienta={idHerramienta}&idObra={idObra}", new { });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RegistrarDevolucionAsync(int idHerramienta, int idObra)
        {
            // ✅ El endpoint correcto según el Controller
            var response = await _api.PostAsync($"{Endpoint}/registrar-devolucion?idHerramienta={idHerramienta}&idObra={idObra}", new { });
            return response.IsSuccessStatusCode;
        }
    }
}