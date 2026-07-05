using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IPagoProveedorHttpService
    {
        Task<List<PagoProveedorListadoDto>?> ObtenerPagosAsync();
        Task<List<PagoProveedorListadoDto>?> ObtenerPagosPorProveedorAsync(int idProveedor);
        Task<bool> CrearPagoAsync(CrearPagoProveedorDto dto);
        Task<bool> EliminarPagoAsync(int id);
    }

    public class PagoProveedorHttpService : IPagoProveedorHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/PagoProveedor";

        public PagoProveedorHttpService(IApiService api) => _api = api;

        public Task<List<PagoProveedorListadoDto>?> ObtenerPagosAsync()
            => _api.GetAsync<List<PagoProveedorListadoDto>>(Endpoint);

        public Task<List<PagoProveedorListadoDto>?> ObtenerPagosPorProveedorAsync(int idProveedor)
            => _api.GetAsync<List<PagoProveedorListadoDto>>($"{Endpoint}/proveedor/{idProveedor}");

        public async Task<bool> CrearPagoAsync(CrearPagoProveedorDto dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarPagoAsync(int id)
        {
            var response = await _api.DeleteAsync($"{Endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}