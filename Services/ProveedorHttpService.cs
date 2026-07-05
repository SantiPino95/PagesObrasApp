using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IProveedorHttpService
    {
        Task<List<ProveedorListadoDto>?> ObtenerProveedoresAsync();
        Task<bool> CrearProveedorAsync(CrearProveedorDto dto);
        Task<bool> ActualizarProveedorAsync(int id, CrearProveedorDto dto);
        Task<bool> EliminarProveedorAsync(int id);
    }

    public class ProveedorHttpService : IProveedorHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Proveedores";

        public ProveedorHttpService(IApiService api) => _api = api;

        public Task<List<ProveedorListadoDto>?> ObtenerProveedoresAsync()
            => _api.GetAsync<List<ProveedorListadoDto>>(Endpoint);

        public async Task<bool> CrearProveedorAsync(CrearProveedorDto dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarProveedorAsync(int id, CrearProveedorDto dto)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarProveedorAsync(int id)
        {
            var response = await _api.DeleteAsync($"{Endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}