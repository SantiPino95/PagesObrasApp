using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class UsuariosModel : PageModel
    {
        public List<UsuarioDto> Pendientes { get; set; } = new();
        public List<UsuarioDto> Activos { get; set; } = new();
        public List<UsuarioDto> Suspendidos { get; set; } = new();
        public List<EmpleadoDto> EmpleadosSinUsuario { get; set; } = new();

        public static readonly string[] Roles = { "Administrador", "Capataz", "Empleado" };

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet()
        {
            // TODO: GET /api/usuarios
            var todos = new List<UsuarioDto>
            {
                new() { Id=1, Email="admin@constructora.com",      Rol="Administrador", Estado="Activo",    FechaReg=new DateTime(2026,1,15), Empleado=null,                Cedula=null          },
                new() { Id=2, Email="rodrigo.mendez@gmail.com",    Rol="Capataz",       Estado="Activo",    FechaReg=new DateTime(2026,2,8),  Empleado="Rodrigo Méndez",    Cedula="4.521.887-3" },
                new() { Id=3, Email="sferreira@hotmail.com",       Rol="Empleado",      Estado="Activo",    FechaReg=new DateTime(2026,2,9),  Empleado="Sebastián Ferreira",Cedula="3.987.654-1" },
                new() { Id=4, Email="diego.pereira@gmail.com",     Rol="Empleado",      Estado="Activo",    FechaReg=new DateTime(2026,4,20), Empleado="Diego Pereira",     Cedula="4.789.012-5" },
                new() { Id=5, Email="luciana.torres@outlook.com",  Rol="Capataz",       Estado="Activo",    FechaReg=new DateTime(2026,5,1),  Empleado="Luciana Torres",    Cedula="3.456.789-2" },
                new() { Id=6, Email="carlos.espinola@gmail.com",   Rol=null,            Estado="Pendiente", FechaReg=new DateTime(2026,6,20), Empleado=null,                Cedula=null          },
                new() { Id=7, Email="pablo.rios@hotmail.com",      Rol=null,            Estado="Pendiente", FechaReg=new DateTime(2026,6,21), Empleado=null,                Cedula=null          },
                new() { Id=8, Email="nuevoempleado@gmail.com",     Rol=null,            Estado="Pendiente", FechaReg=new DateTime(2026,6,22), Empleado=null,                Cedula=null          },
                new() { Id=9, Email="exempleado@gmail.com",        Rol="Empleado",      Estado="Suspendido",FechaReg=new DateTime(2026,3,10), Empleado="Juan Rodríguez",    Cedula="4.999.888-1" },
            };

            Pendientes = todos.Where(u => u.Estado == "Pendiente").OrderByDescending(u => u.FechaReg).ToList();
            Activos = todos.Where(u => u.Estado == "Activo").OrderBy(u => u.Rol).ThenBy(u => u.Email).ToList();
            Suspendidos = todos.Where(u => u.Estado == "Suspendido").ToList();

            // TODO: GET /api/empleados?sinUsuario=true
            EmpleadosSinUsuario = new List<EmpleadoDto>
            {
                new() { Id=3, Nombre="Marcelo Suárez",  Cedula="5.123.456-7" },
                new() { Id=6, Nombre="Pablo Ríos",      Cedula="5.654.321-9" },
                new() { Id=8, Nombre="Fabian Núñez",    Cedula="4.333.111-8" },
            };
        }

        public IActionResult OnPostAprobarUsuario(
            int id, string rol, int? idEmpleado)
        {
            if (id == 0 || string.IsNullOrWhiteSpace(rol))
            {
                Mensaje = "Seleccioná un rol antes de aprobar."; MensajeTipo = "error";
                return RedirectToPage();
            }
            if (!Roles.Contains(rol))
            {
                Mensaje = "Rol inválido."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: PATCH /api/usuarios/{id}/aprobar
            // Body: { rol, idEmpleado? }
            // La API: cambia Estado = "Activo", asigna Rol, vincula idEmpleado si viene
            Mensaje = $"Usuario aprobado con rol {rol}."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        public IActionResult OnPostRechazarUsuario(int id)
        {
            if (id == 0)
            {
                Mensaje = "Usuario no encontrado."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: DELETE /api/usuarios/{id} o PATCH /api/usuarios/{id}/rechazar
            Mensaje = "Registro rechazado y eliminado."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        public IActionResult OnPostSuspenderUsuario(int id)
        {
            if (id == 0)
            {
                Mensaje = "Usuario no encontrado."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: PATCH /api/usuarios/{id}/suspender
            Mensaje = "Usuario suspendido. No podrá acceder al sistema."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        public IActionResult OnPostReactivarUsuario(int id)
        {
            if (id == 0)
            {
                Mensaje = "Usuario no encontrado."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: PATCH /api/usuarios/{id}/reactivar
            Mensaje = "Usuario reactivado correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        public IActionResult OnPostCambiarRol(int id, string nuevoRol)
        {
            if (id == 0 || !Roles.Contains(nuevoRol))
            {
                Mensaje = "Rol inválido."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: PATCH /api/usuarios/{id}/rol
            // Body: { rol: nuevoRol }
            Mensaje = $"Rol actualizado a {nuevoRol}."; MensajeTipo = "ok";
            return RedirectToPage();
        }
    }
}