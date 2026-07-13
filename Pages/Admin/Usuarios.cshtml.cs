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

        public List<UsuarioPendienteDto> Usuarios { get; set; } = new();
        public List<EmpleadoListadoDTOs> Empleados { get; set; } = new();

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? MensajeTipo { get; set; } // "ok" o "error"

        public async Task OnGetAsync()
        {
            await CargarDatosAsync();
        }

        // ── 1. EDITAR / ACTUALIZAR USUARIO ──
        // (Unificado: llama a tu endpoint de actualización completa vía DTO)
        public async Task<IActionResult> OnPostEditarUsuarioAsync(ActualizarUsuarioDto dto)
        {
            // Validación básica antes de llamar al servicio
            if (dto == null || dto.IdUsuario <= 0 || string.IsNullOrWhiteSpace(dto.Email) || dto.Rol == null)
            {
                Mensaje = "Todos los campos obligatorios deben estar completos.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var exito = await _usuarioHttpService.ActualizarUsuarioAsync(dto.IdUsuario, dto);

            if (exito)
            {
                Mensaje = "Usuario y vinculación actualizados correctamente.";
                MensajeTipo = "ok";
            }
            else
            {
                Mensaje = "Ocurrió un error al intentar actualizar el usuario.";
                MensajeTipo = "error";
            }

            return RedirectToPage();
        }

        // ── 2. APROBAR USUARIO ──
        public async Task<IActionResult> OnPostAprobarUsuarioAsync(int id, string rol, int? idEmpleado)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(rol))
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

        // ── 3. RECHAZAR USUARIO ──
        public async Task<IActionResult> OnPostRechazarUsuarioAsync(int id)
        {
            if (id <= 0)
            {
                Mensaje = "ID de usuario inválido.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var rechazado = await _usuarioHttpService.RechazarUsuarioAsync(id);
            Mensaje = rechazado ? "Usuario rechazado correctamente." : "No se pudo rechazar el usuario.";
            MensajeTipo = rechazado ? "ok" : "error";
            return RedirectToPage();
        }

        // ── 4. CAMBIAR ROL RÁPIDO ──
        public async Task<IActionResult> OnPostCambiarRolAsync(int id, string rol)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(rol))
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

        // ── 5. SUSPENDER USUARIO ──
        public async Task<IActionResult> OnPostSuspenderUsuarioAsync(int id)
        {
            if (id <= 0)
            {
                Mensaje = "ID de usuario inválido.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var suspendido = await _usuarioHttpService.SuspenderUsuarioAsync(id);
            Mensaje = suspendido ? "Usuario suspendido correctamente." : "No se pudo suspender el usuario.";
            MensajeTipo = suspendido ? "ok" : "error";
            return RedirectToPage();
        }

        // ── 6. REACTIVAR USUARIO ──
        public async Task<IActionResult> OnPostReactivarUsuarioAsync(int id)
        {
            if (id <= 0)
            {
                Mensaje = "ID de usuario inválido.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var reactivado = await _usuarioHttpService.ReactivarUsuarioAsync(id);
            Mensaje = reactivado ? "Usuario reactivado correctamente." : "No se pudo reactivar el usuario.";
            MensajeTipo = reactivado ? "ok" : "error";
            return RedirectToPage();
        }

        // ── 7. ELIMINAR USUARIO ──
        public async Task<IActionResult> OnPostEliminarUsuarioAsync(int id)
        {
            if (id <= 0)
            {
                Mensaje = "ID de usuario inválido.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var eliminado = await _usuarioHttpService.EliminarUsuarioAsync(id);
            Mensaje = eliminado ? "Usuario eliminado correctamente." : "No se pudo eliminar el usuario.";
            MensajeTipo = eliminado ? "ok" : "error";
            return RedirectToPage();
        }

        // Método auxiliar reusable para la carga de datos del GET
        private async Task CargarDatosAsync()
        {
            Usuarios = await _usuarioHttpService.ObtenerTodosAsync() ?? new();
            Empleados = await _empleadoHttpService.ObtenerEmpleadosAsync() ?? new();
        }
    }
}