using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IMaterialHttpService
    {
        Task<List<MaterialListadoDto>?> ObtenerMaterialesAsync();
        Task<bool> CrearMaterialAsync(CrearMaterialDto dto);
        Task<bool> ActualizarMaterialAsync(int id, CrearMaterialDto dto);
        Task<bool> EliminarMaterialAsync(int id);
        Task<bool> ConsumirMaterialAsync(int idMaterial, int idObra, decimal cantidad);
        Task<bool> ReponerStockAsync(ReponerStockDto dto);
    }

    public class MaterialHttpService : IMaterialHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Materiales";

        public MaterialHttpService(IApiService api) => _api = api;

        public Task<List<MaterialListadoDto>?> ObtenerMaterialesAsync()
            => _api.GetAsync<List<MaterialListadoDto>>(Endpoint);

        public async Task<bool> CrearMaterialAsync(CrearMaterialDto dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarMaterialAsync(int id, CrearMaterialDto dto)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarMaterialAsync(int id)
        {
            var response = await _api.DeleteAsync($"{Endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ConsumirMaterialAsync(int idMaterial, int idObra, decimal cantidad)
        {
            var response = await _api.PostAsync(
                $"{Endpoint}/consumir?idMaterial={idMaterial}&idObra={idObra}&cantidad={cantidad}",
                new { });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ReponerStockAsync(ReponerStockDto dto)
        {
            var response = await _api.PostAsync($"{Endpoint}/entrada", dto);
            return response.IsSuccessStatusCode;
        }
    }
}