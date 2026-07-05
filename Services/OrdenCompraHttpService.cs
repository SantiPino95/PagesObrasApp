using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IOrdenCompraHttpService
    {
        Task<List<OrdenCompraDTO>?> ObtenerOrdenesAsync();
        Task<bool> CrearOrdenAsync(CrearOrdenCompraDTO dto);
        Task<bool> ConfirmarEntregaAsync(int id);
    }

    public class OrdenCompraHttpService : IOrdenCompraHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/OrdenCompra";

        public OrdenCompraHttpService(IApiService api) => _api = api;

        public Task<List<OrdenCompraDTO>?> ObtenerOrdenesAsync()
            => _api.GetAsync<List<OrdenCompraDTO>>(Endpoint);

        public async Task<bool> CrearOrdenAsync(CrearOrdenCompraDTO dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ConfirmarEntregaAsync(int id)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}/confirmar-entrega", new { });
            return response.IsSuccessStatusCode;
        }
    }
}