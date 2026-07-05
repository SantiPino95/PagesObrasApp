using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IEmpleadoHttpService
    {
        Task<List<EmpleadoListadoDTOs>?> ObtenerEmpleadosAsync();
        Task<List<EmpleadoObraDTOs>?> ObtenerAsignacionesAsync();
        Task<bool> CrearEmpleadoAsync(CrearEmpleadoDTOs dto);
        Task<bool> ActualizarEmpleadoAsync(int id, CrearEmpleadoDTOs dto);
        Task<bool> EliminarEmpleadoAsync(int id);
        Task<bool> AsignarAObraAsync(AsignarEmpleadoObraDto dto);
    }

    public class EmpleadoHttpService : IEmpleadoHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Empleado";

        public EmpleadoHttpService(IApiService api) => _api = api;

        public Task<List<EmpleadoListadoDTOs>?> ObtenerEmpleadosAsync()
            => _api.GetAsync<List<EmpleadoListadoDTOs>>(Endpoint);

        public Task<List<EmpleadoObraDTOs>?> ObtenerAsignacionesAsync()
            => _api.GetAsync<List<EmpleadoObraDTOs>>($"{Endpoint}/asignados");

        public async Task<bool> CrearEmpleadoAsync(CrearEmpleadoDTOs dto)
        {
            var response = await _api.PostAsync(Endpoint, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarEmpleadoAsync(int id, CrearEmpleadoDTOs dto)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarEmpleadoAsync(int id)
        {
            var response = await _api.DeleteAsync($"{Endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AsignarAObraAsync(AsignarEmpleadoObraDto dto)
        {
            var response = await _api.PostAsync($"{Endpoint}/asignar", dto);
            return response.IsSuccessStatusCode;
        }
    }
}