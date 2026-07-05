using System.Text.Json.Serialization;

namespace MiWebApi.DTOs
{
    public class RegisterDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }

    public class LoginDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }

    public class LoginResponseDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = null!;

        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("nombreCompleto")]
        public string NombreCompleto { get; set; } = null!;

        [JsonPropertyName("rol")]
        public string Rol { get; set; } = null!;

        [JsonPropertyName("idUsuario")]
        public int IdUsuario { get; set; }

        [JsonPropertyName("expira")]
        public DateTime Expira { get; set; }

        [JsonPropertyName("idEmpleado")]
        public int? IdEmpleado { get; set; }
    }
}