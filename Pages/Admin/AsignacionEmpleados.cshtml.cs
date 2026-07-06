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
    }
}