using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Services;

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
        public string ActiveTab { get; set; } = "empleado";

        [TempData] public string? RegistroExitoso { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostLoginAsync(string email, string password)
        {
            ActiveTab = "empleado";
            Console.WriteLine($"🔐 Login: Email={email}");

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Completá email y contraseña.";
                return Page();
            }

            var (ok, usuario, mensajeError, statusCode) = await _authHttpService.LoginAsync(email.Trim(), password);

            Console.WriteLine($"📥 Login: ok={ok}, statusCode={statusCode}, usuario={(usuario != null ? "OK" : "NULL")}");

            if (!ok)
            {
                ErrorMessage = statusCode switch
                {
                    401 => "Email o contraseña incorrectos.",
                    403 => mensajeError ?? "No tenés permiso para acceder.",
                    _ => "Error al iniciar sesión. Intentá de nuevo."
                };
                return Page();
            }

            if (usuario == null)
            {
                ErrorMessage = "Error inesperado. Intentá de nuevo.";
                return Page();
            }

            Console.WriteLine($"✅ Usuario logueado: {usuario.Email}, Rol: {usuario.Rol}");

            // ✅ CREAR LAS CLAIMS
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
        new Claim(ClaimTypes.Name, usuario.NombreCompleto ?? usuario.Email), // ✅ Si no tiene nombre, usar email
        new Claim(ClaimTypes.Email, usuario.Email),
        new Claim(ClaimTypes.Role, usuario.Rol ?? "Empleado"),
        new Claim("Token", usuario.Token) // ✅ Guardar el token para usarlo en las llamadas a la API
    };

            if (usuario.IdEmpleado.HasValue)
                claims.Add(new Claim("IdEmpleado", usuario.IdEmpleado.Value.ToString()));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // ✅ CREAR LA COOKIE
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                }
            );

            Console.WriteLine($"✅ Cookie creada. Redirigiendo a: {usuario.Rol}");

            // ✅ REDIRIGIR SEGÚN ROL
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