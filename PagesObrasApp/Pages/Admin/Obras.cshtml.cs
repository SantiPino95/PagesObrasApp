using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class ObrasModel : PageModel
    {
        public List<ObraDto> Obras { get; set; } = new();
        public List<ClienteDto> Clientes { get; set; } = new();

        // Mensaje de éxito/error para feedback en UI
        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; } // "ok" | "error"

        public void OnGet()
        {
            // TODO: GET /api/obras
            Obras = new List<ObraDto>
            {
                new() { Id=1, Codigo="OB-2026-014", Nombre="Edificio Las Acacias",     IdCliente=1, Cliente="Inmobiliaria Sur S.A.", Estado="En Progreso", Avance=62, Direccion="Av. Italia 1234", FechaInicio=new DateTime(2026,2,10),  FechaFinPrevista=new DateTime(2026,11,30) },
                new() { Id=2, Codigo="OB-2026-019", Nombre="Casa Familia Pérez",       IdCliente=2, Cliente="Marcos Pérez",          Estado="Planificada", Avance=0,  Direccion="Artigas 456",      FechaInicio=new DateTime(2026,7,1),   FechaFinPrevista=new DateTime(2027,2,1)  },
                new() { Id=3, Codigo="OB-2026-011", Nombre="Galpón Industrial Ruta 5", IdCliente=3, Cliente="Logística del Este",    Estado="En Progreso", Avance=38, Direccion="Ruta 8 km 12",     FechaInicio=new DateTime(2026,4,22),  FechaFinPrevista=new DateTime(2026,12,31) },
                new() { Id=4, Codigo="OB-2025-087", Nombre="Reforma Local Comercial",  IdCliente=4, Cliente="Comercial Andina",      Estado="Pausada",     Avance=45, Direccion="Bulevar Artigas",   FechaInicio=new DateTime(2026,1,15),  FechaFinPrevista=new DateTime(2026,9,30) },
                new() { Id=5, Codigo="OB-2026-002", Nombre="Torre Mirador",            IdCliente=1, Cliente="Inmobiliaria Sur S.A.", Estado="En Progreso", Avance=21, Direccion="Av. Rivera 500",    FechaInicio=new DateTime(2026,5,2),   FechaFinPrevista=new DateTime(2027,6,30) },
            };

            // TODO: GET /api/clientes (para el select del modal)
            CargarClientes();
        }

        public IActionResult OnPostCrearObra(
            string nombreObra, int idCliente, string estado,
            string direccion, DateTime fechaInicio, DateTime? fechaFinPrevista)
        {
            if (string.IsNullOrWhiteSpace(nombreObra) || idCliente == 0)
            {
                Mensaje = "Completá nombre y cliente.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            // TODO: POST /api/obras
            // Body: { nombreObra, idCliente, estado, direccion, fechaInicio, fechaFinPrevista }
            // La API genera automáticamente el Codigo_Publico

            Mensaje = $"Obra \"{nombreObra}\" creada correctamente.";
            MensajeTipo = "ok";
            return RedirectToPage();
        }

        private void CargarClientes()
        {
            // TODO: GET /api/clientes
            Clientes = new List<ClienteDto>
            {
                new() { Id=1, Nombre="Inmobiliaria Sur S.A." },
                new() { Id=2, Nombre="Marcos Pérez"          },
                new() { Id=3, Nombre="Logística del Este"    },
                new() { Id=4, Nombre="Comercial Andina"      },
                new() { Id=5, Nombre="Lucía Fernández"       },
            };
        }
    }
}