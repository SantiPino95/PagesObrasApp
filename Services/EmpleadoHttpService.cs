using PagesObrasApp.Models.DTOs;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

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
       Task<bool> EliminarEmpleadoAsignadoAsync (int idObra, int idEmpleado);
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
            try
            {
                // Pasa el objeto DTO directamente 
                var response = await _api.PostAsync(Endpoint, dto);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error API ({response.StatusCode}): {errorContent}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR DETALLADO: {ex.Message}");
                return false;
            }
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

        public async Task<bool> EliminarEmpleadoAsignadoAsync(int idObra, int idEmpleado)
        {
            var response = await _api.DeleteAsync($"{Endpoint}/obra/{idObra}/asignacion/empleado/{idEmpleado}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AsignarAObraAsync(AsignarEmpleadoObraDto dto)
        {
            var response = await _api.PostAsync($"{Endpoint}/asignar", dto);

            // Si el estado es exitoso (201 Created), todo perfecto
            return response.IsSuccessStatusCode;
        }
    }
}
