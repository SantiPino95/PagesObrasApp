

using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IClienteHttpService
    {
        Task<List<CLientesListadoDTOs>?> ObtenerClientesAsync();
        Task<CLientesListadoDTOs?> ObtenerClientePorIdAsync(int id);
        Task<bool> CrearClienteAsync(CrearClienteDTOs dto);
        Task<bool> ActualizarClienteAsync(int id, CrearClienteDTOs dto);
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

        public Task<List<CLientesListadoDTOs>?> ObtenerClientesAsync()
            => _api.GetAsync<List<CLientesListadoDTOs>>(Endpoint);

        public Task<CLientesListadoDTOs?> ObtenerClientePorIdAsync(int id)
            => _api.GetAsync<CLientesListadoDTOs>($"{Endpoint}/{id}");

        public async Task<bool> CrearClienteAsync(CrearClienteDTOs dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarClienteAsync(int id, CrearClienteDTOs dto)
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
