using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class IndexModel : PageModel
    {
        // Stats del hero
        public int ObrasEnProgreso { get; set; }
        public int EmpleadosHoy { get; set; }
        public int HerramientasEnReparacion { get; set; }

        // Tabla de obras activas
        public List<ObraDto> ObrasActivas { get; set; } = new();

        public void OnGet()
        {
            // TODO: reemplazar por HttpClient → GET /api/admin/dashboard
            ObrasEnProgreso = 2;
            EmpleadosHoy = 14;
            HerramientasEnReparacion = 3;

            ObrasActivas = new List<ObraDto>
            {
                new() { Codigo="OB-2026-014", Nombre="Edificio Las Acacias",     Cliente="Inmobiliaria Sur S.A.", Estado="En Progreso", Avance=62, FechaInicio=new DateTime(2026,2,10) },
                new() { Codigo="OB-2026-019", Nombre="Casa Familia Pérez",       Cliente="Marcos Pérez",          Estado="Planificada", Avance=0,  FechaInicio=new DateTime(2026,7,1)  },
                new() { Codigo="OB-2026-011", Nombre="Galpón Industrial Ruta 5", Cliente="Logística del Este",    Estado="En Progreso", Avance=38, FechaInicio=new DateTime(2026,4,22) },
                new() { Codigo="OB-2025-087", Nombre="Reforma Local Comercial",  Cliente="Comercial Andina",      Estado="Pausada",     Avance=45, FechaInicio=new DateTime(2026,1,15) },
            };
        }
    }
}