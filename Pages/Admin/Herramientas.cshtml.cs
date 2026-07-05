using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
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
        public List<ObraAdminListadoDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Herramientas = await _herramientaHttpService.ObtenerHerramientasAsync() ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
        }

        // ✅ CREAR HERRAMIENTA
        public async Task<IActionResult> OnPostCrearHerramientaAsync(
            string nombreTipo, string codigoInventario, string origen)
        {
            if (string.IsNullOrWhiteSpace(nombreTipo) || string.IsNullOrWhiteSpace(codigoInventario))
            {
                Mensaje = "Tipo y código son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearHerramientaDto
            {
                NombreTipo = nombreTipo,
                CodigoInventario = codigoInventario,
                Origen = origen,
                EstadoDisponibilidad = "Disponible"
            };

            var creado = await _herramientaHttpService.CrearHerramientaAsync(dto);
            Mensaje = creado ? $"Herramienta \"{codigoInventario}\" dada de alta." : "No se pudo registrar la herramienta.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }

        // ✅ ASIGNAR HERRAMIENTA
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

        // ✅ DEVOLVER HERRAMIENTA
        public async Task<IActionResult> OnPostRegistrarDevolucionAsync(int idHerramienta, int idObra)
        {
            if (idHerramienta == 0 || idObra == 0)
            {
                Mensaje = "Seleccioná herramienta y obra.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var devuelta = await _herramientaHttpService.RegistrarDevolucionAsync(idHerramienta, idObra);
            Mensaje = devuelta ? "Devolución registrada correctamente." : "No se pudo registrar la devolución.";
            MensajeTipo = devuelta ? "ok" : "error";
            return RedirectToPage();
        }

        // ✅ ELIMINAR HERRAMIENTA
        public async Task<IActionResult> OnPostEliminarHerramientaAsync(int id)
        {
            if (id == 0)
            {
                Mensaje = "Herramienta no encontrada.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var eliminado = await _herramientaHttpService.EliminarHerramientaAsync(id);

            if (!eliminado)
            {
                Mensaje = "No se puede eliminar la herramienta porque está asignada a una obra.";
                MensajeTipo = "error";
            }
            else
            {
                Mensaje = "Herramienta eliminada correctamente.";
                MensajeTipo = "ok";
            }

            return RedirectToPage();
        }
    }
}