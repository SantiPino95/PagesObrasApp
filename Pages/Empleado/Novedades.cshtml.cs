using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Empleado
{
    [Authorize(Policy = "Personal")]
    public class NovedadesModel : PageModel
    {
        private readonly INovedadHttpService _novedadHttpService;
        private readonly IEmpleadoHttpService _empleadoHttpService;

        public NovedadesModel(INovedadHttpService novedadHttpService, IEmpleadoHttpService empleadoHttpService)
        {
            _novedadHttpService = novedadHttpService;
            _empleadoHttpService = empleadoHttpService;
        }

        public int IdEmpleado { get; set; }
        public List<EmpleadoObraDTOs> Asignaciones { get; set; } = new();
        public List<NovedadListadoDto> Novedades { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var idEmpleadoClaim = User.FindFirst("IdEmpleado")?.Value;
            if (string.IsNullOrEmpty(idEmpleadoClaim) || !int.TryParse(idEmpleadoClaim, out int idEmpleado))
            {
                if (User.IsInRole("Administrador"))
                    return RedirectToPage("/Admin/Index");
                else
                    return RedirectToPage("/Index");
            }

            IdEmpleado = idEmpleado;
            Asignaciones = await _empleadoHttpService.ObtenerAsignacionesAsync() ?? new();
            Novedades = await _novedadHttpService.ObtenerNovedadesAsync() ?? new();

            // Filtrar novedades del empleado
            var idsAsignaciones = Asignaciones.Select(a => a.IdEmpleado).ToList();
            Novedades = Novedades.Where(n => idsAsignaciones.Contains(n.IdEmpleadoObra)).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostCrearNovedadAsync(
            int idEmpleadoObra, DateTime fecha, string tipoNovedad, string descripcion)
        {
            if (idEmpleadoObra == 0 || string.IsNullOrWhiteSpace(tipoNovedad) || string.IsNullOrWhiteSpace(descripcion))
            {
                Mensaje = "Todos los campos son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearNovedadDto
            {
                IdEmpleadoObra = idEmpleadoObra,
                Fecha = fecha,
                TipoNovedad = tipoNovedad,
                Descripcion = descripcion,
                EstadoRevision = "Pendiente"
            };

            var creado = await _novedadHttpService.CrearNovedadAsync(dto);
            Mensaje = creado ? "Novedad reportada correctamente." : "No se pudo reportar la novedad.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}