using MiWebApi.DTOs;
using PagesObrasApp.Models;
using PagesObrasApp.Models.DTOs;
using System.Net.Http.Json;

namespace PagesObrasApp.Services
{
    public interface IAuthHttpService
    {
        Task<(bool Ok, LoginResponseDto? Usuario, string? MensajeError, int StatusCode)> LoginAsync(string email, string password);
        Task<(bool Ok, string? MensajeError, int StatusCode)> RegistrarAsync(RegisterDto dto);
    }

    public class AuthHttpService : IAuthHttpService
    {
        private readonly IApiService _api;
        private const string Endpoint = "api/Auth";

        public AuthHttpService(IApiService api)
        {
            _api = api;
        }

        public async Task<(bool Ok, LoginResponseDto? Usuario, string? MensajeError, int StatusCode)> LoginAsync(string email, string password)
        {
            var dto = new LoginDto { Email = email, Password = password };
            var response = await _api.PostAsync($"{Endpoint}/login", dto);
            var status = (int)response.StatusCode;

            if (!response.IsSuccessStatusCode)
            {
                string? mensaje = null;
                try
                {
                    var body = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                    body?.TryGetValue("mensaje", out mensaje);
                }
                catch { /* el body puede no ser JSON o no tener esa clave */ }

                return (false, null, mensaje, status);
            }

            var usuario = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            return (true, usuario, null, status);
        }

        public async Task<(bool Ok, string? MensajeError, int StatusCode)> RegistrarAsync(RegisterDto dto)
        {
            try
            {
                Console.WriteLine($"📤 AuthHttpService.RegistrarAsync: Email={dto.Email}, Password={dto.Password}");

                var response = await _api.PostAsync($"{Endpoint}/register", dto);
                var status = (int)response.StatusCode;

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"📥 Respuesta API: Status={status}, Content={content}");

                if (!response.IsSuccessStatusCode)
                {
                    return (false, content, status);
                }

                return (true, null, status);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ EXCEPCIÓN en AuthHttpService: {ex.Message}");
                return (false, ex.Message, 500);
            }
        }
    }
}