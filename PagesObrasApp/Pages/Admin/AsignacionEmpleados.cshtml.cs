using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class AsignacionEmpleadosModel : PageModel
    {
        private readonly IAsignacionHttpService _asignacionHttpService;
        private readonly IEmpleadoHttpService _empleadoHttpService;
        private readonly IObraHttpService _obraHttpService;

        public AsignacionEmpleadosModel(
            IAsignacionHttpService asignacionHttpService,
            IEmpleadoHttpService empleadoHttpService,
            IObraHttpService obraHttpService)
        {
            _asignacionHttpService = asignacionHttpService;
            _empleadoHttpService = empleadoHttpService;
            _obraHttpService = obraHttpService;
        }

        public List<EmpleadoObraDto> Asignaciones { get; set; } = new();
        public List<EmpleadoListadoDto> Empleados { get; set; } = new();
        public List<ObraListadoDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Asignaciones = await _asignacionHttpService.ObtenerAsignacionesAsync() ?? new();
            Empleados = await _empleadoHttpService.ObtenerEmpleadosAsync() ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
        }

        public async Task<IActionResult> OnPostCrearAsignacionAsync(int idEmpleado, int idObra)
        {
            if (idEmpleado == 0 || idObra == 0)
            {
                Mensaje = "Seleccioná empleado y obra.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var creada = await _asignacionHttpService.CrearAsignacionAsync(idEmpleado, idObra);
            Mensaje = creada ? "Asignación creada correctamente." : "No se pudo crear la asignación.";
            MensajeTipo = creada ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostQuitarAsignacionAsync(int idEmpleado, int idObra)
        {
            if (idEmpleado == 0 || idObra == 0)
            {
                Mensaje = "Asignación no encontrada.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var eliminada = await _asignacionHttpService.EliminarAsignacionAsync(idEmpleado, idObra);
            Mensaje = eliminada ? "Asignación eliminada correctamente." : "No se pudo eliminar la asignación.";
            MensajeTipo = eliminada ? "ok" : "error";
            return RedirectToPage();
        }
    }
}