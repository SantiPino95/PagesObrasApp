using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IUsuarioHttpService
    {
        Task<List<UsuarioPendienteDto>?> ObtenerPendientesAsync();
        Task<List<UsuarioPendienteDto>?> ObtenerTodosAsync();
        Task<bool> AprobarUsuarioAsync(int id, string rol, int? idEmpleado);
        Task<bool> RechazarUsuarioAsync(int id);
        Task<bool> SuspenderUsuarioAsync(int id);
        Task<bool> ReactivarUsuarioAsync(int id);
        Task<bool> CambiarRolAsync(int id, string nuevoRol);
        Task<bool> ActualizarUsuarioAsync(int id, ActualizarUsuarioDto dto);
        Task<bool> EliminarUsuarioAsync(int id);
    }

    public class UsuarioHttpService : IUsuarioHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Usuario";
        
        public UsuarioHttpService(IApiService api) => _api = api;

        public Task<List<UsuarioPendienteDto>?> ObtenerTodosAsync()
           => _api.GetAsync<List<UsuarioPendienteDto>>($"{Endpoint}/todos");

        public async Task<bool> ActualizarUsuarioAsync(int id, ActualizarUsuarioDto dto)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        public Task<List<UsuarioPendienteDto>?> ObtenerPendientesAsync()
            => _api.GetAsync<List<UsuarioPendienteDto>>($"{Endpoint}/pendientes");

        public async Task<bool> AprobarUsuarioAsync(int id, string rol, int? idEmpleado)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}/aprobar", new { Rol = rol, IdEmpleado = idEmpleado });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RechazarUsuarioAsync(int id)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}/rechazar", new { });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SuspenderUsuarioAsync(int id)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}/suspender", new { });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ReactivarUsuarioAsync(int id)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}/reactivar", new { });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CambiarRolAsync(int id, string nuevoRol)
        {
            var response = await _api.PutAsync($"{Endpoint}/{id}/rol", new { Rol = nuevoRol });
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            var response = await _api.DeleteAsync($"{Endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}