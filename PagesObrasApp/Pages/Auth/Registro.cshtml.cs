using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PagesObrasApp.Pages.Auth
{
    public class RegistroModel : PageModel
    {
        public string? ErrorMessage { get; set; }
        public string EmailIngresado { get; set; } = string.Empty;

        [TempData]
        public string? RegistroExitoso { get; set; }

        public void OnGet() { }

        public IActionResult OnPost(string email, string password, string confirmPassword)
        {
            EmailIngresado = email?.Trim() ?? string.Empty;

            // ── Validaciones ──────────────────────────────────────────
            if (string.IsNullOrWhiteSpace(email))
            {
                ErrorMessage = "El email es obligatorio.";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                ErrorMessage = "La contraseña debe tener al menos 8 caracteres.";
                return Page();
            }

            if (password != confirmPassword)
            {
                ErrorMessage = "Las contraseñas no coinciden.";
                return Page();
            }

            // ── TODO: HttpClient → POST /api/auth/registro ────────────
            // Body: { email, contrasena }
            // Respuestas posibles:
            //   201 → usuario creado con Estado = "Pendiente"
            //   409 → ya existe una cuenta con ese email
            //
            // Ejemplo de manejo:
            // var response = await _httpClient.PostAsJsonAsync("api/auth/registro", new { email, contrasena = password });
            // if (response.StatusCode == HttpStatusCode.Conflict)
            // {
            //     ErrorMessage = "Ya existe una cuenta con ese email.";
            //     return Page();
            // }
            // if (!response.IsSuccessStatusCode)
            // {
            //     ErrorMessage = "Error al crear la cuenta. Intentá de nuevo.";
            //     return Page();
            // }

            // Registro exitoso → ir al login con mensaje
            RegistroExitoso = "Cuenta creada correctamente. Esperá la aprobación del administrador para poder ingresar.";
            return RedirectToPage("/Auth/Login");
        }
    }
}