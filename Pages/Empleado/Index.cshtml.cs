using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

// 👈 Asegurate que el namespace sea este
namespace PagesObrasApp.Pages.Empleado
{
    [Authorize(Policy = "Personal")]
    public class IndexModel : PageModel
    {
        private readonly IApiService _api;
        private readonly IEmpleadoHttpService _empleadoHttpService;
        private readonly IObraHttpService _obraHttpService;
        private readonly INovedadHttpService _novedadHttpService;

        public IndexModel(
            IApiService api,
            IEmpleadoHttpService empleadoHttpService,
            IObraHttpService obraHttpService,
            INovedadHttpService novedadHttpService)
        {
            _api = api;
            _empleadoHttpService = empleadoHttpService;
            _obraHttpService = obraHttpService;
            _novedadHttpService = novedadHttpService;
        }

        // ── Propiedades ──
        public int IdEmpleado { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public List<EmpleadoObraDTOs> Asignaciones { get; set; } = new();
        public List<NovedadListadoDto> NovedadesRecientes { get; set; } = new();
        public decimal HorasHoy { get; set; }
        public decimal HorasSemana { get; set; }
        public int NovedadesPendientes { get; set; }
        public bool YaRegistroHoy { get; set; }

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
            NombreCompleto = User.Identity?.Name ?? "Empleado";

            // Obtener asignaciones
            var todasAsignaciones = await _empleadoHttpService.ObtenerAsignacionesAsync() ?? new();
            Asignaciones = todasAsignaciones.Where(a => a.IdEmpleado == IdEmpleado).ToList();

            // TODO: Conectar con API real cuando exista
            // Por ahora datos mock
            HorasHoy = 8;
            HorasSemana = 40;
            YaRegistroHoy = true;
            NovedadesPendientes = 0;

            return Page();
        }
    }
}