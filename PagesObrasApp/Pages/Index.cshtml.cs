using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;

namespace PagesObrasApp.Pages
{
    public class IndexModel : PageModel
    {
        // ── Propiedades que lee la vista ──────────────────────────────────

        // Mensaje de error que aparece en el tab activo
        public string? ErrorMessage { get; set; }

        // Le dice al JS qué tab abrir cuando hay un error tras el POST
        // Valores posibles: "cliente" | "empleado" | "registro"
        public string ActiveTab { get; set; } = "cliente";

        // Mensaje de éxito tras registro (se muestra en tab empleado)
        [TempData]
        public string? RegistroExitoso { get; set; }


        // ── GET ───────────────────────────────────────────────────────────

        public void OnGet()
        {
            // Si viene de un registro exitoso, abrir tab de login con mensaje
            if (!string.IsNullOrEmpty(RegistroExitoso))
            {
                ActiveTab = "empleado";
                ErrorMessage = RegistroExitoso; // reutilizamos el campo para el aviso verde
            }
        }


        // ── POST: Buscar obra (tab Cliente) ───────────────────────────────

        public IActionResult OnPostBuscarObra(string codigoObra)
        {
            ActiveTab = "cliente";

            if (string.IsNullOrWhiteSpace(codigoObra))
            {
                ErrorMessage = "Ingresá el código de tu obra.";
                return Page();
            }

            var codigo = codigoObra.Trim().ToUpper();

            // TODO: reemplazar por llamada al HttpClient hacia la API
            // GET /api/obras/publico/{codigo}
            var codigosValidos = new[]
            {
                "OB-2026-014",
                "OB-2026-002",
                "OB-2026-011",
                "OB-2026-019",
                "OB-2025-087",
            };

            if (!codigosValidos.Contains(codigo))
            {
                ErrorMessage = $"No encontramos ninguna obra con el código \"{codigoObra.ToUpper()}\". "
                             + "Verificá que sea correcto o pedíselo al administrador.";
                return Page();
            }

            // Código válido → redirigir a la página de seguimiento del cliente
            return RedirectToPage("/Cliente/Seguimiento", new { codigo });
        }


        // ── POST: Login (tab Iniciar sesión) ──────────────────────────────

        public IActionResult OnPostLogin(string email, string password)
        {
            ActiveTab = "empleado";

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Completá email y contraseña.";
                return Page();
            }

            // TODO: reemplazar por llamada al HttpClient hacia la API
            // POST /api/auth/login → devuelve { rol, token/cookie }
            // Por ahora, credencial hardcodeada para pruebas
            if (email == "admin@constructora.com" && password == "UTU2026")
            {
                // TODO: setear cookie de autenticación con el rol
                return RedirectToPage("/Admin/Index");
            }

            // Cualquier otra combinación → acceso denegado por ahora
            ErrorMessage = "Credenciales incorrectas. Verificá tu email y contraseña.";
            return Page();
        }


        // ── POST: Registro (tab Registrarse) ──────────────────────────────

        public IActionResult OnPostRegistro(string email, string password, string confirmPassword)
        {
            ActiveTab = "registro";

            // Validaciones básicas del lado cliente (el servidor repite las mismas)
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

            // TODO: reemplazar por llamada al HttpClient hacia la API
            // POST /api/auth/registro → crea Usuario con Estado = "Pendiente"
            // La API devuelve 201 si el email no existe, 409 si ya está registrado

            // Simulación de email ya registrado para pruebas:
            // if (email == "ya@registrado.com") { ErrorMessage = "Ya existe una cuenta con ese email."; return Page(); }

            // Registro exitoso → redirigir al index abriendo tab de login con aviso
            RegistroExitoso = "✓ Cuenta creada. Esperá la aprobación del administrador para poder ingresar.";
            return RedirectToPage("/Index");
        }
    }
}