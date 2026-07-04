using PagesObrasApp.Models;

namespace PagesObrasApp.Services
{
    public interface IClienteHttpService
    {
        Task<List<ClienteListadoDto>?> ObtenerClientesAsync();
        Task<ClienteListadoDto?> ObtenerClientePorIdAsync(int id);
        Task<bool> CrearClienteAsync(CrearClienteDto dto);
        Task<bool> ActualizarClienteAsync(int id, CrearClienteDto dto);
        Task<bool> EliminarClienteAsync(int id);
    }

    // "HttpService" para dejar claro que esto solo llama a la API por HTTP,
    // no tiene lógica de negocio ni acceso a la base de datos (eso vive en el repo de WebApi)
    public class ClienteHttpService : IClienteHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Cliente"; // Coincide con [Route("api/[controller]")] del ClienteController

        public ClienteHttpService(IApiService api)
        {
            _api = api;
        }

        public Task<List<ClienteListadoDto>?> ObtenerClientesAsync()
            => _api.GetAsync<List<ClienteListadoDto>>(Endpoint);

        public Task<ClienteListadoDto?> ObtenerClientePorIdAsync(int id)
            => _api.GetAsync<ClienteListadoDto>($"{Endpoint}/{id}");

        public async Task<bool> CrearClienteAsync(CrearClienteDto dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarClienteAsync(int id, CrearClienteDto dto)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarClienteAsync(int id)
        {
            var response = await _api.DeleteAsync($"{Endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
