using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class SeguimientoObraModel : PageModel
    {
        public List<ObraDto> Obras { get; set; } = new();
        public List<SeguimientoDto> Seguimientos { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet()
        {
            // TODO: GET /api/obras?conSeguimiento=true
            Obras = new List<ObraDto>
            {
                new() { Id=1, Codigo="OB-2026-014", Nombre="Edificio Las Acacias",     Estado="En Progreso", Cliente="Inmobiliaria Sur S.A.", FechaInicio=new DateTime(2026,2,10)  },
                new() { Id=2, Codigo="OB-2026-002", Nombre="Torre Mirador",            Estado="En Progreso", Cliente="Inmobiliaria Sur S.A.", FechaInicio=new DateTime(2026,5,2)   },
                new() { Id=3, Codigo="OB-2026-011", Nombre="Galpón Industrial Ruta 5", Estado="En Progreso", Cliente="Logística del Este",    FechaInicio=new DateTime(2026,4,22)  },
                new() { Id=4, Codigo="OB-2026-019", Nombre="Casa Familia Pérez",       Estado="Planificada", Cliente="Marcos Pérez",          FechaInicio=new DateTime(2026,7,1)   },
                new() { Id=5, Codigo="OB-2025-087", Nombre="Reforma Local Comercial",  Estado="Pausada",     Cliente="Comercial Andina",      FechaInicio=new DateTime(2026,1,15)  },
            };

            // TODO: GET /api/seguimiento
            Seguimientos = new List<SeguimientoDto>
            {
                new() { Id=1,  IdObra=1, Fecha=new DateTime(2026,2,12),  Pct=5,  Desc="Limpieza de terreno e inicio de excavaciones." },
                new() { Id=2,  IdObra=1, Fecha=new DateTime(2026,3,20),  Pct=22, Desc="Losa de fundación hormigonada y curada." },
                new() { Id=3,  IdObra=1, Fecha=new DateTime(2026,4,15),  Pct=38, Desc="Columnas y vigas planta baja finalizadas." },
                new() { Id=4,  IdObra=1, Fecha=new DateTime(2026,5,10),  Pct=50, Desc="Mampostería primer piso al 70%." },
                new() { Id=5,  IdObra=1, Fecha=new DateTime(2026,6,20),  Pct=62, Desc="Revoques exteriores planta baja terminados." },
                new() { Id=6,  IdObra=2, Fecha=new DateTime(2026,5,5),   Pct=5,  Desc="Inicio oficial. Cerco perimetral instalado." },
                new() { Id=7,  IdObra=2, Fecha=new DateTime(2026,6,20),  Pct=21, Desc="Fundaciones completas. Inicio de estructura." },
                new() { Id=8,  IdObra=3, Fecha=new DateTime(2026,4,25),  Pct=10, Desc="Perfiles estructurales en obra." },
                new() { Id=9,  IdObra=3, Fecha=new DateTime(2026,6,18),  Pct=38, Desc="Cubierta al 40%. Portones instalados." },
                new() { Id=10, IdObra=4, Fecha=new DateTime(2026,6,18),  Pct=7,  Desc="Excavaciones para vigas de fundación al 50%." },
                new() { Id=11, IdObra=5, Fecha=new DateTime(2026,3,25),  Pct=45, Desc="Obra pausada por falta de materiales." },
            };
        }

        public IActionResult OnPostRegistrarAvance(
            int idObra, DateTime fecha, int porcentaje,
            string descripcion, string? imgUrl)
        {
            if (idObra == 0 || string.IsNullOrWhiteSpace(descripcion))
            {
                Mensaje = "Seleccioná obra y escribí la descripción."; MensajeTipo = "error";
                return RedirectToPage();
            }
            if (porcentaje < 0 || porcentaje > 100)
            {
                Mensaje = "El porcentaje debe estar entre 0 y 100."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/seguimiento
            // Body: { idObra, fecha, descripcionAvance, porcentajeAvance, imgProgreso }
            Mensaje = $"Avance del {porcentaje}% registrado correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }
    }
}