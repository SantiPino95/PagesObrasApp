using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class UsuariosModel : PageModel
    {
        private readonly IUsuarioHttpService _usuarioHttpService;
        private readonly IEmpleadoHttpService _empleadoHttpService;

        public static readonly string[] Roles = { "Administrador", "Capataz", "Empleado" };

        public UsuariosModel(
            IUsuarioHttpService usuarioHttpService,
            IEmpleadoHttpService empleadoHttpService)
        {
            _usuarioHttpService = usuarioHttpService;
            _empleadoHttpService = empleadoHttpService;
        }

        public List<UsuarioListadoDto> Pendientes { get; set; } = new();
        public List<UsuarioListadoDto> Activos { get; set; } = new();
        public List<UsuarioListadoDto> Suspendidos { get; set; } = new();
        public List<EmpleadoListadoDto> EmpleadosSinUsuario { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            var todos = await _usuarioHttpService.ObtenerUsuariosAsync() ?? new();

            Pendientes = todos.Where(u => u.Estado == "Pendiente").OrderByDescending(u => u.FechaReg).ToList();
            Activos = todos.Where(u => u.Estado == "Activo").OrderBy(u => u.Rol).ThenBy(u => u.Email).ToList();
            Suspendidos = todos.Where(u => u.Estado == "Suspendido").ToList();

            // TODO: si el backend expone un endpoint que filtre empleados sin usuario vinculado,
            // reemplazar esto por: await _empleadoHttpService.ObtenerEmpleadosSinUsuarioAsync()
            var empleados = await _empleadoHttpService.ObtenerEmpleadosAsync() ?? new();
            EmpleadosSinUsuario = empleados; // filtrar del lado del backend cuando esté el endpoint
        }

        public async Task<IActionResult> OnPostAprobarUsuarioAsync(int id, string rol, int? idEmpleado)
        {
            if (id == 0 || string.IsNullOrWhiteSpace(rol) || !Roles.Contains(rol))
            {
                Mensaje = "Seleccioná un rol válido antes de aprobar.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new AprobarUsuarioDto { Rol = rol, IdEmpleado = idEmpleado };
            var aprobado = await _usuarioHttpService.AprobarUsuarioAsync(id, dto);
            Mensaje = aprobado ? $"Usuario aprobado con rol {rol}." : "No se pudo aprobar el usuario.";
            MensajeTipo = aprobado ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRechazarUsuarioAsync(int id)
        {
            if (id == 0)
            {
                Mensaje = "Usuario no encontrado.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var rechazado = await _usuarioHttpService.RechazarUsuarioAsync(id);
            Mensaje = rechazado ? "Registro rechazado correctamente." : "No se pudo rechazar el usuario.";
            MensajeTipo = rechazado ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSuspenderUsuarioAsync(int id)
        {
            if (id == 0)
            {
                Mensaje = "Usuario no encontrado.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var suspendido = await _usuarioHttpService.SuspenderUsuarioAsync(id);
            Mensaje = suspendido ? "Usuario suspendido correctamente." : "No se pudo suspender el usuario.";
            MensajeTipo = suspendido ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostReactivarUsuarioAsync(int id)
        {
            if (id == 0)
            {
                Mensaje = "Usuario no encontrado.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var reactivado = await _usuarioHttpService.ReactivarUsuarioAsync(id);
            Mensaje = reactivado ? "Usuario reactivado correctamente." : "No se pudo reactivar el usuario.";
            MensajeTipo = reactivado ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCambiarRolAsync(int id, string nuevoRol)
        {
            if (id == 0 || !Roles.Contains(nuevoRol))
            {
                Mensaje = "Rol inválido.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CambiarRolDto { Rol = nuevoRol };
            var cambiado = await _usuarioHttpService.CambiarRolAsync(id, dto);
            Mensaje = cambiado ? $"Rol actualizado a {nuevoRol}." : "No se pudo cambiar el rol.";
            MensajeTipo = cambiado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}