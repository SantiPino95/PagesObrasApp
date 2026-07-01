using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class PresupuestosModel : PageModel
    {
        public List<PresupuestoDto> Presupuestos { get; set; } = new();
        public List<ObraDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet()
        {
            // TODO: GET /api/presupuestos (incluye detalles)
            Presupuestos = new List<PresupuestoDto>
            {
                new() { Id=1, IdObra=1, NombreObra="Edificio Las Acacias",     CodigoObra="OB-2026-014", Cliente="Inmobiliaria Sur S.A.", FechaEmision=new DateTime(2026,2,5),  MontoTotal=485000, Estado="Aprobado",
                    Detalles = new() {
                        new() { Desc="Estructura de hormigón armado",  Cant=1,   PU=180000, Sub=180000 },
                        new() { Desc="Mampostería y revoques",         Cant=1,   PU=95000,  Sub=95000  },
                        new() { Desc="Instalación eléctrica completa", Cant=1,   PU=75000,  Sub=75000  },
                        new() { Desc="Instalación sanitaria",          Cant=1,   PU=55000,  Sub=55000  },
                        new() { Desc="Carpintería y aberturas",        Cant=1,   PU=48000,  Sub=48000  },
                        new() { Desc="Pintura general",                Cant=850, PU=38,     Sub=32300  },
                    }},
                new() { Id=2, IdObra=1, NombreObra="Edificio Las Acacias",     CodigoObra="OB-2026-014", Cliente="Inmobiliaria Sur S.A.", FechaEmision=new DateTime(2026,5,18), MontoTotal=92000,  Estado="Pendiente",
                    Detalles = new() {
                        new() { Desc="Ampliación planta baja",  Cant=1, PU=62000, Sub=62000 },
                        new() { Desc="Materiales adicionales",  Cant=1, PU=30000, Sub=30000 },
                    }},
                new() { Id=3, IdObra=3, NombreObra="Galpón Industrial Ruta 5", CodigoObra="OB-2026-011", Cliente="Logística del Este",    FechaEmision=new DateTime(2026,4,20), MontoTotal=780000, Estado="Aprobado",
                    Detalles = new() {
                        new() { Desc="Estructura metálica galpón",    Cant=1,   PU=450000, Sub=450000 },
                        new() { Desc="Cubierta de chapa galvanizada", Cant=800, PU=185,    Sub=148000 },
                        new() { Desc="Portón y cerramientos",         Cant=4,   PU=18000,  Sub=72000  },
                        new() { Desc="Instalación eléctrica trifásica",Cant=1,  PU=110000, Sub=110000 },
                    }},
                new() { Id=4, IdObra=4, NombreObra="Casa Familia Pérez",       CodigoObra="OB-2026-019", Cliente="Marcos Pérez",          FechaEmision=new DateTime(2026,5,28), MontoTotal=135000, Estado="Pendiente",
                    Detalles = new() {
                        new() { Desc="Construcción vivienda tipo A", Cant=1,  PU=95000, Sub=95000 },
                        new() { Desc="Cerco perimetral",             Cant=80, PU=350,   Sub=28000 },
                        new() { Desc="Garage techado",               Cant=1,  PU=12000, Sub=12000 },
                    }},
            };

            // TODO: GET /api/obras (para el select del modal)
            Obras = new List<ObraDto>
            {
                new() { Id=1, Codigo="OB-2026-014", Nombre="Edificio Las Acacias"     },
                new() { Id=2, Codigo="OB-2026-002", Nombre="Torre Mirador"            },
                new() { Id=3, Codigo="OB-2026-011", Nombre="Galpón Industrial Ruta 5" },
                new() { Id=4, Codigo="OB-2026-019", Nombre="Casa Familia Pérez"       },
                new() { Id=5, Codigo="OB-2025-087", Nombre="Reforma Local Comercial"  },
            };
        }

        public IActionResult OnPostCambiarEstado(int id, string accion)
        {
            if (id == 0 || (accion != "Aprobar" && accion != "Rechazar"))
            {
                Mensaje = "Acción inválida."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: PATCH /api/presupuestos/{id}/estado
            // Body: { estado: "Aprobado" | "Rechazado" }
            var nuevoEstado = accion == "Aprobar" ? "Aprobado" : "Rechazado";
            Mensaje = $"Presupuesto PRES-{id:D4} {nuevoEstado.ToLower()} correctamente.";
            MensajeTipo = accion == "Aprobar" ? "ok" : "error";
            return RedirectToPage();
        }

        public IActionResult OnPostCrearPresupuesto(
            int idObra, DateTime fechaEmision,
            string[] descripciones, decimal[] cantidades,
            decimal[] preciosUnitarios, decimal[] subtotales)
        {
            if (idObra == 0 || descripciones.Length == 0)
            {
                Mensaje = "Seleccioná obra y agregá al menos un ítem."; MensajeTipo = "error";
                return RedirectToPage();
            }

            var montoTotal = subtotales.Sum();
            if (montoTotal <= 0)
            {
                Mensaje = "El monto total debe ser mayor a cero."; MensajeTipo = "error";
                return RedirectToPage();
            }

            // TODO: POST /api/presupuestos
            // Body: { idObra, fechaEmision, montoTotal, detalles: [{ descripcion, cantidad, precioUnitario, subtotal }] }
            Mensaje = "Presupuesto creado correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }
    }
}