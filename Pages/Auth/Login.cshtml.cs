using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Services;
using System.Net;
using System.Security.Claims;

namespace PagesObrasApp.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAuthHttpService _authHttpService;

        public LoginModel(IAuthHttpService authHttpService)
        {
            _authHttpService = authHttpService;
        }

        public string? ErrorMessage { get; set; }
        public string EmailIngresado { get; set; } = string.Empty;

        [TempData] public string? RegistroExitoso { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            EmailIngresado = email?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Completá email y contraseña.";
                return Page();
            }

            var (ok, usuario, mensajeError, statusCode) = await _authHttpService.LoginAsync(email.Trim(), password);

            if (!ok)
            {
                ErrorMessage = statusCode switch
                {
                    (int)HttpStatusCode.Unauthorized => "Email o contraseña incorrectos.",
                    (int)HttpStatusCode.Forbidden => mensajeError ?? "No tenés permiso para acceder.",
                    _ => "Error al iniciar sesión. Intentá de nuevo."
                };
                return Page();
            }

            if (usuario == null)
            {
                ErrorMessage = "Error inesperado. Intentá de nuevo.";
                return Page();
            }

            // ── Construir claims y firmar cookie propia de Razor Pages ──
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name,           usuario.NombreCompleto),
                new Claim(ClaimTypes.Email,          usuario.Email),
                new Claim(ClaimTypes.Role,           usuario.Rol),
            };

            if (usuario.IdEmpleado.HasValue)
                claims.Add(new Claim("IdEmpleado", usuario.IdEmpleado.Value.ToString()));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8) }
            );

            return usuario.Rol switch
            {
                "Administrador" => RedirectToPage("/Admin/Index"),
                "Supervisor" => RedirectToPage("/Empleado/Index"),
                "Empleado" => RedirectToPage("/Empleado/Index"),
                _ => RedirectToPage("/Index"),
            };
        }
    }
}