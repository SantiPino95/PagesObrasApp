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
        private readonly IObraHttpService _obraHttpService;

        public IndexModel(IAuthHttpService authHttpService, IObraHttpService obraHttpService)
        {
            _authHttpService = authHttpService;
            _obraHttpService = obraHttpService;

        }

       
       

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

        public async Task<IActionResult> OnPostBuscarObra(string codigoObra)
        {
            ActiveTab = "cliente";

            if (string.IsNullOrWhiteSpace(codigoObra))
            {
                ErrorMessage = "Ingresá el código de tu obra.";
                return Page();
            }

            // Limpiamos los espacios y convertimos a mayúsculas
            var codigo = codigoObra.Trim().ToUpper();

            if(codigo.Length != 10 )
            {

                ErrorMessage = "El código de obra debe tener exactamente 10 caracteres.";
                return Page();
            }

            bool obraExiste = await _obraHttpService.ExisteObraPorCodigoAsync(codigo);

            if (!obraExiste)
            {
                ErrorMessage = "No se encontró ninguna obra con ese código.";
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
                new Claim("IdEmpleado",              usuario.IdEmpleado?.ToString() ?? "0")
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

            return usuario.Rol.ToLower().Trim() switch
            {
                "administrador" => RedirectToPage("/Admin/Index"),
                "supervisor" => RedirectToPage("/Empleado/Index_Supervisor"),
                "empleado" => RedirectToPage("/Empleado/Index"),
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