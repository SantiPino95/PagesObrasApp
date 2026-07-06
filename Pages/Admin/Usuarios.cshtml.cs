using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class UsuariosModel : PageModel
    {
        private readonly IUsuarioHttpService _usuarioHttpService;
        private readonly IEmpleadoHttpService _empleadoHttpService;

        public UsuariosModel(IUsuarioHttpService usuarioHttpService, IEmpleadoHttpService empleadoHttpService)
        {
            _usuarioHttpService = usuarioHttpService;
            _empleadoHttpService = empleadoHttpService;
        }

        public List<UsuarioPendienteDto> Usuarios { get; set; } = new();  // ✅ TODOS los usuarios
        public List<EmpleadoListadoDTOs> Empleados { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            // ✅ Obtener TODOS los usuarios
            Usuarios = await _usuarioHttpService.ObtenerTodosAsync() ?? new();
            Empleados = await _empleadoHttpService.ObtenerEmpleadosAsync() ?? new();
        }

        public async Task<IActionResult> OnPostAprobarUsuarioAsync(int id, string rol, int? idEmpleado)
        {
            if (id == 0 || string.IsNullOrWhiteSpace(rol))
            {
                Mensaje = "Usuario y rol son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var aprobado = await _usuarioHttpService.AprobarUsuarioAsync(id, rol, idEmpleado);
            Mensaje = aprobado ? "Usuario aprobado correctamente." : "No se pudo aprobar el usuario.";
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
            Mensaje = rechazado ? "Usuario rechazado correctamente." : "No se pudo rechazar el usuario.";
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

        public async Task<IActionResult> OnPostCambiarRolAsync(int id, string rol)
        {
            if (id == 0 || string.IsNullOrWhiteSpace(rol))
            {
                Mensaje = "Usuario y rol son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var cambiado = await _usuarioHttpService.CambiarRolAsync(id, rol);
            Mensaje = cambiado ? "Rol actualizado correctamente." : "No se pudo actualizar el rol.";
            MensajeTipo = cambiado ? "ok" : "error";
            return RedirectToPage();
        }

        // ✅ NUEVO: Editar usuario
        public async Task<IActionResult> OnPostEditarUsuarioAsync(int id, string email, string rol, int? idEmpleado)
        {
            if (id == 0 || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(rol))
            {
                Mensaje = "Todos los campos son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var rolCambiado = await _usuarioHttpService.CambiarRolAsync(id, rol);
            if (!rolCambiado)
            {
                Mensaje = "No se pudo actualizar el rol.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var usuario = Usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario != null && usuario.Estado != "Pendiente")
            {
                var aprobado = await _usuarioHttpService.AprobarUsuarioAsync(id, rol, idEmpleado);
                if (!aprobado)
                {
                    Mensaje = "No se pudo actualizar el empleado vinculado.";
                    MensajeTipo = "error";
                    return RedirectToPage();
                }
            }

            Mensaje = "Usuario actualizado correctamente.";
            MensajeTipo = "ok";
            return RedirectToPage();
        }

        // ✅ NUEVO: Eliminar usuario
        public async Task<IActionResult> OnPostEliminarUsuarioAsync(int id)
        {
            if (id == 0)
            {
                Mensaje = "Usuario no encontrado.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var eliminado = await _usuarioHttpService.EliminarUsuarioAsync(id);
            Mensaje = eliminado ? "Usuario eliminado correctamente." : "No se pudo eliminar el usuario.";
            MensajeTipo = eliminado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}