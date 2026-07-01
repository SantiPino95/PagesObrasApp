using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class HerramientasModel : PageModel
    {
        public List<HerramientaDto> Herramientas { get; set; } = new();
        public List<ObraDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet()
        {
            // TODO: GET /api/herramientas (incluye obra asignada actual)
            Herramientas = new List<HerramientaDto>
            {
                new() { Id=1, Codigo="HER-0091", Tipo="Taladro Percutor",        Estado="Asignada",    Origen="Propia",    Obra="Edificio Las Acacias",     FechaSalida=new DateTime(2026,5,12) },
                new() { Id=2, Codigo="HER-0092", Tipo="Taladro Percutor",        Estado="Asignada",    Origen="Propia",    Obra="Torre Mirador",            FechaSalida=new DateTime(2026,6,1)  },
                new() { Id=3, Codigo="HER-0093", Tipo="Taladro Percutor",        Estado="En Depósito", Origen="Propia",    Obra=null,                       FechaSalida=null },
                new() { Id=4, Codigo="HER-0044", Tipo="Amoladora Angular",       Estado="Asignada",    Origen="Propia",    Obra="Galpón Industrial Ruta 5", FechaSalida=new DateTime(2026,4,25) },
                new() { Id=5, Codigo="HER-0045", Tipo="Amoladora Angular",       Estado="En Reparación",Origen="Propia",   Obra=null,                       FechaSalida=null },
                new() { Id=6, Codigo="HER-0210", Tipo="Andamio Modular (tramo)", Estado="Asignada",    Origen="Alquilada", Obra="Edificio Las Acacias",     FechaSalida=new DateTime(2026,5,20) },
                new() { Id=7, Codigo="HER-0033", Tipo="Hormigonera 150L",        Estado="En Depósito", Origen="Propia",    Obra=null,                       FechaSalida=null },
                new() { Id=8, Codigo="HER-0078", Tipo="Soldadora Inverter",      Estado="Asignada",    Origen="Alquilada", Obra="Galpón Industrial Ruta 5", FechaSalida=new DateTime(2026,6,3)  },
            };

            // TODO: GET /api/obras?estado=activa
            Obras = new List<ObraDto>
            {
                new() { Id=1, Nombre="Edificio Las Acacias",     Codigo="OB-2026-014" },
                new() { Id=2, Nombre="Torre Mirador",            Codigo="OB-2026-002" },
                new() { Id=3, Nombre="Galpón Industrial Ruta 5", Codigo="OB-2026-011" },
                new() { Id=4, Nombre="Casa Familia Pérez",       Codigo="OB-2026-019" },
            };
        }

        public IActionResult OnPostCrearHerramienta(
            string tipo, string codigo, string origen)
        {
            if (string.IsNullOrWhiteSpace(tipo) || string.IsNullOrWhiteSpace(codigo))
            {
                Mensaje = "Tipo y código son obligatorios."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/herramientas
            Mensaje = $"Herramienta \"{codigo}\" dada de alta."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        public IActionResult OnPostAsignarHerramienta(
            int idHerramienta, int idObra, DateTime fechaSalida)
        {
            if (idHerramienta == 0 || idObra == 0)
            {
                Mensaje = "Seleccioná herramienta y obra."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/herramientas/{idHerramienta}/asignar
            // Body: { idObra, fechaSalida }
            // La API cambia Estado a "Asignada" e inserta en Herramientas_asignadas
            Mensaje = "Herramienta asignada correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }
    }
}