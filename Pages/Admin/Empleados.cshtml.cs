using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class EmpleadosModel : PageModel
    {
        private readonly IEmpleadoHttpService _empleadoHttpService;
        private readonly IObraHttpService _obraHttpService;

        public EmpleadosModel(IEmpleadoHttpService empleadoHttpService, IObraHttpService obraHttpService)
        {
            _empleadoHttpService = empleadoHttpService;
            _obraHttpService = obraHttpService;
        }
        [BindProperty]
        public CrearEmpleadoDTOs EmpleadoNuevo { get; set; } = new();

        public List<EmpleadoListadoDTOs> Empleados { get; set; } = new();
        public List<ObraAdminListadoDto> Obras { get; set; } = new();
        public List<EmpleadoObraDTOs> Asignados { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Empleados = await _empleadoHttpService.ObtenerEmpleadosAsync() ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
            Asignados = await _empleadoHttpService.ObtenerAsignacionesAsync() ?? new();
        }

        public async Task<IActionResult> OnPostCrearEmpleadoAsync()
        {
            if (!ModelState.IsValid)
            {
              
                // Los muestra directamente en tu cartel rojo
                Mensaje = ("Error en campos -> " );
                MensajeTipo = "error";
                return Page();
            }

            try
            {
                var creado = await _empleadoHttpService.CrearEmpleadoAsync(EmpleadoNuevo);

                if (creado)
                {
                    Mensaje = $"Empleado \"{EmpleadoNuevo.Nombre} {EmpleadoNuevo.Apellido}\" dado de alta.";
                    MensajeTipo = "ok";
                }
                else
                {
                    Mensaje = "Error: La API rechazó el registro. Verifique Cédula o Email.";
                    MensajeTipo = "error";
                }
            }
            catch (Exception ex)
            {
                
                Mensaje = $"Error: {ex.Message}";
                MensajeTipo = "error";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarEmpleadoAsync(int idEmpleado)
        {
            if (idEmpleado <= 0)
            {
                Mensaje = "Error: ID de empleado inválido.";
                MensajeTipo = "error";
                return RedirectToPage();
            }
            try
            {
                var eliminado = await _empleadoHttpService.EliminarEmpleadoAsync(idEmpleado);
                if (eliminado)
                {
                    Mensaje = $"Empleado con ID {idEmpleado} eliminado correctamente.";
                    MensajeTipo = "ok";
                }
                else
                {
                    Mensaje = "Error: No se pudo eliminar el empleado. Puede que esté asignado a una obra.";
                    MensajeTipo = "error";
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error al eliminar empleado: {ex.Message}";
                MensajeTipo = "error";
            }
            return RedirectToPage();
        } 

       


    }
}