using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiWebApi.DTOs;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;
using System.Net;

namespace PagesObrasApp.Pages.Auth
{
    public class RegistroModel : PageModel
    {
        private readonly IAuthHttpService _authHttpService;

        // AJUSTAR: id del rol por defecto para auto-registro en tu tabla Roles.
        // El admin lo reasigna después vía AprobarUsuarioDto, así que este valor
        // es solo un placeholder mientras el usuario queda en Estado = "Pendiente".
        private const int IdRolPorDefecto = 3;

        public RegistroModel(IAuthHttpService authHttpService)
        {
            _authHttpService = authHttpService;
        }

        public string? ErrorMessage { get; set; }
        public string EmailIngresado { get; set; } = string.Empty;

        [TempData]
        public string? RegistroExitoso { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(
            string email, string password, string confirmPassword
            )
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

            //if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) ||
            //    string.IsNullOrWhiteSpace(cedula) || string.IsNullOrWhiteSpace(categoria))
            //{
            //    ErrorMessage = "El Email y contraseña son obligatorios.";
            //    return Page();
            //}

            var dto = new RegisterDto
            {
                Email = email.Trim(),
                Password = password,
               
            };

            var (ok, mensajeError, statusCode) = await _authHttpService.RegistrarAsync(dto);

            if (!ok)
            {
                ErrorMessage = statusCode == (int)HttpStatusCode.Conflict
                    ? "Ya existe una cuenta con ese email."
                    : (mensajeError ?? "Error al crear la cuenta. Intentá de nuevo.");
                return Page();
            }

            RegistroExitoso = "Cuenta creada correctamente. Esperá la aprobación del administrador para poder ingresar.";
            return RedirectToPage("/Auth/Login");
        }
    }
}