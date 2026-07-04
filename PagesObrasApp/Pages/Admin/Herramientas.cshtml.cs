using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class HerramientasModel : PageModel
    {
        private readonly IHerramientaHttpService _herramientaHttpService;
        private readonly IObraHttpService _obraHttpService;

        public HerramientasModel(IHerramientaHttpService herramientaHttpService, IObraHttpService obraHttpService)
        {
            _herramientaHttpService = herramientaHttpService;
            _obraHttpService = obraHttpService;
        }

        public List<HerramientaListadoDto> Herramientas { get; set; } = new();
        public List<ObraListadoDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Herramientas = await _herramientaHttpService.ObtenerHerramientasAsync() ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
        }

        public async Task<IActionResult> OnPostCrearHerramientaAsync(
            string tipo, string codigo, string origen)
        {
            if (string.IsNullOrWhiteSpace(tipo) || string.IsNullOrWhiteSpace(codigo))
            {
                Mensaje = "Tipo y código son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearHerramientaDto
            {
                NombreTipo = tipo,
                CodigoInventario = codigo,
                Origen = origen,
                EstadoDisponibilidad = "Disponible"
            };

            var creado = await _herramientaHttpService.CrearHerramientaAsync(dto);
            Mensaje = creado ? $"Herramienta \"{codigo}\" dada de alta." : "No se pudo registrar la herramienta.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }

        // La fecha de salida no se envía: el HerramientaController.RegistrarSalida solo recibe idHerramienta e idObra
        public async Task<IActionResult> OnPostAsignarHerramientaAsync(int idHerramienta, int idObra)
        {
            if (idHerramienta == 0 || idObra == 0)
            {
                Mensaje = "Seleccioná herramienta y obra.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var asignada = await _herramientaHttpService.AsignarSalidaAsync(idHerramienta, idObra);
            Mensaje = asignada ? "Herramienta asignada correctamente." : "No se pudo asignar la herramienta.";
            MensajeTipo = asignada ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRegistrarDevolucionAsync(int idHerramienta, int idObra)
        {
            var devuelta = await _herramientaHttpService.RegistrarDevolucionAsync(idHerramienta, idObra);
            Mensaje = devuelta ? "Devolución registrada correctamente." : "No se pudo registrar la devolución.";
            MensajeTipo = devuelta ? "ok" : "error";
            return RedirectToPage();
        }
    }
}
