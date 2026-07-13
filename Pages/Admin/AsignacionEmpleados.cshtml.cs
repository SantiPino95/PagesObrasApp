using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class AsignacionEmpleadosModel : PageModel
    {
        private readonly IEmpleadoHttpService _empleadoHttpService;
        private readonly IObraHttpService _obraHttpService;

        public AsignacionEmpleadosModel(IEmpleadoHttpService empleadoHttpService, IObraHttpService obraHttpService)
        {
            _empleadoHttpService = empleadoHttpService;
            _obraHttpService = obraHttpService;
        }

        public List<EmpleadoObraDTOs> Asignaciones { get; set; } = new();
        public List<EmpleadoListadoDTOs> Empleados { get; set; } = new();
        public List<ObraAdminListadoDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Asignaciones = await _empleadoHttpService.ObtenerAsignacionesAsync() ?? new();
            Empleados = await _empleadoHttpService.ObtenerEmpleadosAsync() ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
        }

        public async Task<IActionResult> OnPostCrearAsignacionAsync(
     int idEmpleado, int idObra, decimal valorHoraAsignado, string rolEnObra)
        {
            if (idEmpleado == 0 || idObra == 0)
            {
                Mensaje = "Seleccioná empleado y obra.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            //  Traemos las asignaciones de la pantalla
            var asignacionesActuales = await _empleadoHttpService.ObtenerAsignacionesAsync() ?? new();

            // Verificamos si este empleado ya existe en esa obra específica
            bool yaAsignado = asignacionesActuales.Any(a => a.IdEmpleado == idEmpleado && a.IdObra == idObra);

            if (yaAsignado)
            {
                Mensaje = "El empleado ya se encuentra asignado a una obra.";
                MensajeTipo = "error"; // Esto pintará tu alerta en color rojo
                return RedirectToPage();
            }
           

           
            // Si pasa el filtro, procedemos a enviar los datos a la API
            var dto = new AsignarEmpleadoObraDto
            {
                IdEmpleado = idEmpleado,
                IdObra = idObra,
                RolEnObra = rolEnObra,
                ValorHoraAsignado = valorHoraAsignado
            };

            var asignado = await _empleadoHttpService.AsignarAObraAsync(dto);
            Mensaje = asignado ? "Asignación creada correctamente." : "No se pudo crear la asignación.";
            MensajeTipo = asignado ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarAsignacionAsync(int idObra, int idEmpleado)
        {
           
            if (idObra == 0 || idEmpleado == 0)
            {
                Mensaje = $"Error en HTML: Llegó Obra: {idObra} y Empleado: {idEmpleado}.";
                MensajeTipo = "error";
                return RedirectToPage();
            }
            var eliminado = await _empleadoHttpService.EliminarEmpleadoAsignadoAsync(idObra, idEmpleado);
            Mensaje = eliminado
       ? "Asignación eliminada correctamente."
       : $"La API rechazó la baja. Se intentó con Obra ID: {idObra} y Empleado ID: {idEmpleado}.";
            MensajeTipo = eliminado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}