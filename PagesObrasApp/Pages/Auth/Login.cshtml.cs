using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace PagesObrasApp.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public string? ErrorMessage { get; set; }
        public string EmailIngresado { get; set; } = string.Empty;

        [TempData]
        public string? RegistroExitoso { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            EmailIngresado = email?.Trim() ?? string.Empty;

            // ── Validaciones básicas ───────────────────────────────────
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Completá email y contraseña.";
                return Page();
            }

            // ── TODO: reemplazar por HttpClient → POST /api/auth/login ─
            // Respuesta esperada de la API:
            // {
            //   "idUsuario": 1,
            //   "nombre": "Rodrigo Méndez",
            //   "email": "rodrigo@...",
            //   "rol": "Administrador",     ← "Administrador" | "Capataz" | "Empleado"
            //   "estado": "Activo",
            //   "idEmpleado": 1             ← null si es solo admin
            // }

            // Credenciales de prueba hardcodeadas mientras no está la API:
            var usuariosMock = new[]
            {
                new { Email="admin@constructora.com",   Password="UTU2026",  Nombre="Administrador", Rol="Administrador", IdUsuario=1, IdEmpleado=(int?)null },
                new { Email="rodrigo.mendez@gmail.com", Password="UTU2026",  Nombre="Rodrigo Méndez",Rol="Capataz",       IdUsuario=2, IdEmpleado=(int?)1    },
                new { Email="sferreira@hotmail.com",    Password="UTU2026",  Nombre="Sebastián F.",  Rol="Empleado",      IdUsuario=3, IdEmpleado=(int?)2    },
            };

            var usuario = usuariosMock.FirstOrDefault(u =>
                u.Email == email.Trim().ToLower() && u.Password == password);

            if (usuario == null)
            {
                ErrorMessage = "Email o contraseña incorrectos.";
                return Page();
            }

            // ── Verificar que el usuario esté Activo ───────────────────
            // TODO: la API ya filtra esto, pero acá podés manejar el caso
            // donde la API devuelva estado = "Pendiente" o "Suspendido"
            // if (usuario.Estado != "Activo")
            // {
            //     ErrorMessage = usuario.Estado == "Pendiente"
            //         ? "Tu cuenta está pendiente de aprobación por el administrador."
            //         : "Tu cuenta fue suspendida. Contactá al administrador.";
            //     return Page();
            // }

            // ── Construir los claims del usuario ───────────────────────
            // Los claims son los datos que viajan dentro de la cookie cifrada
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name,           usuario.Nombre),
                new Claim(ClaimTypes.Email,          usuario.Email),
                new Claim(ClaimTypes.Role,           usuario.Rol),
            };

            // Si tiene empleado vinculado, guardarlo como claim extra
            if (usuario.IdEmpleado.HasValue)
            {
                claims.Add(new Claim("IdEmpleado", usuario.IdEmpleado.Value.ToString()));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // ── Firmar la cookie ───────────────────────────────────────
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,  // la cookie sobrevive al cerrar el browser
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                }
            );

            // ── Redirigir según el rol ─────────────────────────────────
            return usuario.Rol switch
            {
                "Administrador" => RedirectToPage("/Admin/Index"),
                "Capataz" => RedirectToPage("/Empleado/Index"),
                "Empleado" => RedirectToPage("/Empleado/Index"),
                _ => RedirectToPage("/Index"),
            };
        }
    }
}