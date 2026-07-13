using PagesObrasApp.Models.DTOs;

namespace PagesObrasApp.Services
{
    public interface IRegistroHoraHttpService
    {
        Task<List<RegistroHoraDto>?> ObtenerPorEmpleadoYMesAsync(int idEmpleado, int anio, int mes);
        Task<bool> CrearAsync(int idEmpleado, CrearRegistroHoraDto dto);
        Task<bool> ActualizarAsync(int idEmpleado, int idRegistro, ActualizarRegistroHoraDto dto);
    }

    public class RegistroHoraHttpService : IRegistroHoraHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/RegistroHora";

        public RegistroHoraHttpService(IApiService api) => _api = api;

        public Task<List<RegistroHoraDto>?> ObtenerPorEmpleadoYMesAsync(int idEmpleado, int anio, int mes)
            => _api.GetAsync<List<RegistroHoraDto>>($"{Endpoint}/empleado/{idEmpleado}?anio={anio}&mes={mes}");

        public async Task<bool> CrearAsync(int idEmpleado, CrearRegistroHoraDto dto)
        {
            var response = await _api.PostAsync($"{Endpoint}/empleado/{idEmpleado}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(int idEmpleado, int idRegistro, ActualizarRegistroHoraDto dto)
        {
            var response = await _api.PutAsync($"{Endpoint}/{idRegistro}/empleado/{idEmpleado}", dto);
            return response.IsSuccessStatusCode;
        }
    }
}