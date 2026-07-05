using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiWebApi.DTOs;
using PagesObrasApp.Models;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;
using System.Net;
using System.Security.Claims;

namespace PagesObrasApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAuthHttpService _authHttpService;

        public IndexModel(IAuthHttpService authHttpService)
        {
            _authHttpService = authHttpService;
        }

        // AJUSTAR: id del rol por defecto para auto-registro en tu tabla Roles.
        // El admin lo reasigna después vía AprobarUsuarioDto.
        private const int IdRolPorDefecto = 5;

        public string? ErrorMessage { get; set; }
        public string ActiveTab { get; set; } = "cliente";

        [TempData]
        public string? RegistroExitoso { get; set; }

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(RegistroExitoso))
            {
                ActiveTab = "empleado";
                ErrorMessage = RegistroExitoso;
            }
        }

        public IActionResult OnPostBuscarObra(string codigoObra)
        {
            ActiveTab = "cliente";

            if (string.IsNullOrWhiteSpace(codigoObra))
            {
                ErrorMessage = "Ingresá el código de tu obra.";
                return Page();
            }

            var codigo = codigoObra.Trim().ToUpper();
            var codigosValidos = new[]
            {
                "OB-2026-014", "OB-2026-002", "OB-2026-011",
                "OB-2026-019", "OB-2025-087",
            };

            if (!codigosValidos.Contains(codigo))
            {
                ErrorMessage = $"No encontramos ninguna obra con el código \"{codigoObra.ToUpper()}\".";
                return Page();
            }

            return RedirectToPage("/Cliente/Seguimiento", new { codigo });
        }

        // ── POST: Login — vía AuthHttpService ─────────────────────────────
        public async Task<IActionResult> OnPostLoginAsync(string email, string password)
        {
            ActiveTab = "empleado";

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

        public async Task<IActionResult> OnPostRegistroAsync(
    string email,
    string password,
    string confirmPassword)
        {
            // 🔍 LOG PARA VERIFICAR QUE LLEGA
            Console.WriteLine($"📝 OnPostRegistroAsync llamado con: Email={email}, Password={password}, Confirm={confirmPassword}");

            ActiveTab = "registro";

            if (string.IsNullOrWhiteSpace(email))
            {
                ErrorMessage = "El email es obligatorio.";
                Console.WriteLine("❌ Error: Email vacío");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                ErrorMessage = "La contraseña debe tener al menos 8 caracteres.";
                Console.WriteLine($"❌ Error: Password inválida (length={password?.Length ?? 0})");
                return Page();
            }

            if (password != confirmPassword)
            {
                ErrorMessage = "Las contraseñas no coinciden.";
                Console.WriteLine("❌ Error: Las contraseñas no coinciden");
                return Page();
            }

            Console.WriteLine("✅ Validaciones pasadas, llamando a la API...");

            var dto = new RegisterDto
            {
                Email = email.Trim(),
                Password = password
            };

            try
            {
                var (ok, mensajeError, statusCode) = await _authHttpService.RegistrarAsync(dto);
                Console.WriteLine($"📥 Respuesta API: ok={ok}, statusCode={statusCode}, error={mensajeError}");

                if (!ok)
                {
                    ErrorMessage = statusCode == (int)HttpStatusCode.Conflict
                        ? "Ya existe una cuenta con ese email."
                        : (mensajeError ?? "No se pudo crear la cuenta. Intentá de nuevo.");
                    return Page();
                }

                RegistroExitoso = "✓ Cuenta creada. Esperá la aprobación del administrador para poder ingresar.";
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ EXCEPCIÓN: {ex.Message}");
                Console.WriteLine($"Stack: {ex.StackTrace}");
                ErrorMessage = "Error inesperado. Intentá de nuevo.";
                return Page();
            }
        }
    }
}