using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class IndexModel : PageModel
    {
        private readonly IObraHttpService _obraHttpService;
        private readonly IEmpleadoHttpService _empleadoHttpService;
        private readonly IHerramientaHttpService _herramientaHttpService;

        public IndexModel(
            IObraHttpService obraHttpService,
            IEmpleadoHttpService empleadoHttpService,
            IHerramientaHttpService herramientaHttpService)
        {
            _obraHttpService = obraHttpService;
            _empleadoHttpService = empleadoHttpService;
            _herramientaHttpService = herramientaHttpService;
        }

        public List<ObraAdminListadoDto> ObrasActivas { get; set; } = new();
        public int ObrasEnProgreso { get; set; }
        public int EmpleadosActivos { get; set; }
        public int HerramientasEnReparacion { get; set; }

        public async Task OnGetAsync()
        {
            // Obtener obras
            var obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
            ObrasActivas = obras.Where(o => o.Estado == "En Progreso" || o.Estado == "Planificada").ToList();
            ObrasEnProgreso = obras.Count(o => o.Estado == "En Progreso");

            // Obtener empleados
            var empleados = await _empleadoHttpService.ObtenerEmpleadosAsync() ?? new();
            EmpleadosActivos = empleados.Count;

            // Obtener herramientas
            var herramientas = await _herramientaHttpService.ObtenerHerramientasAsync() ?? new();
            HerramientasEnReparacion = herramientas.Count(h => h.EstadoDisponibilidad == "En Reparación");
        }
    }
}