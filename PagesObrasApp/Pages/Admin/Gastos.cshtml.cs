using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class GastosModel : PageModel
    {
        public List<GastoDto> Gastos { get; set; } = new();
        public List<ObraDto> Obras { get; set; } = new();

        public static readonly string[] Categorias =
        {
            "Materiales", "Mano de Obra", "Herramientas",
            "Transporte", "Alquiler Equipos", "Servicios", "Otro"
        };

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet()
        {
            // TODO: GET /api/gastos
            Gastos = new List<GastoDto>
            {
                new() { Id=1,  IdObra=1, Fecha=new DateTime(2026,4,3),  Monto=45000, Desc="Cemento y arena primera entrega",  Cat="Materiales",       Comp="FC-001123" },
                new() { Id=2,  IdObra=1, Fecha=new DateTime(2026,4,10), Monto=18500, Desc="Jornales semana del 7 al 11",      Cat="Mano de Obra",     Comp="RH-0041"   },
                new() { Id=3,  IdObra=1, Fecha=new DateTime(2026,5,5),  Monto=32000, Desc="Hierro Ø10 segunda entrega",       Cat="Materiales",       Comp="FC-001455" },
                new() { Id=4,  IdObra=2, Fecha=new DateTime(2026,5,8),  Monto=22000, Desc="Cemento y cal",                   Cat="Materiales",       Comp="FC-002001" },
                new() { Id=5,  IdObra=3, Fecha=new DateTime(2026,4,22), Monto=55000, Desc="Chapa galvanizada y perfiles",    Cat="Materiales",       Comp="FC-003011" },
                new() { Id=6,  IdObra=3, Fecha=new DateTime(2026,5,10), Monto=18000, Desc="Soldadora y herramientas alquil", Cat="Alquiler Equipos", Comp="ALQ-0031"  },
                new() { Id=7,  IdObra=4, Fecha=new DateTime(2026,6,3),  Monto=14000, Desc="Ladrillos y mortero",             Cat="Materiales",       Comp="FC-004001" },
                new() { Id=8,  IdObra=1, Fecha=new DateTime(2026,6,18), Monto=5600,  Desc="Instalación eléctrica parcial",   Cat="Servicios",        Comp="SRV-0014"  },
            };

            // TODO: GET /api/obras
            Obras = new List<ObraDto>
            {
                new() { Id=1, Codigo="OB-2026-014", Nombre="Edificio Las Acacias"     },
                new() { Id=2, Codigo="OB-2026-002", Nombre="Torre Mirador"            },
                new() { Id=3, Codigo="OB-2026-011", Nombre="Galpón Industrial Ruta 5" },
                new() { Id=4, Codigo="OB-2026-019", Nombre="Casa Familia Pérez"       },
                new() { Id=5, Codigo="OB-2025-087", Nombre="Reforma Local Comercial"  },
            };
        }

        public IActionResult OnPostRegistrarGasto(
            int idObra, DateTime fecha, string categoria,
            string descripcion, decimal monto, string? nroComprobante)
        {
            if (idObra == 0 || monto <= 0 || string.IsNullOrWhiteSpace(descripcion))
            {
                Mensaje = "Completá obra, descripción y monto."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/gastos
            Mensaje = "Gasto registrado correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }
    }
}