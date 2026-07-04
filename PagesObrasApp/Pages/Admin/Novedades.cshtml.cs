using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class NovedadesModel : PageModel
    {
        private readonly INovedadHttpService _novedadHttpService;

        public NovedadesModel(INovedadHttpService novedadHttpService)
        {
            _novedadHttpService = novedadHttpService;
        }

        public List<NovedadListadoDto> Pendientes { get; set; } = new();
        public List<NovedadListadoDto> Revisadas { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            var todas = await _novedadHttpService.ObtenerNovedadesAsync() ?? new();

            Pendientes = todas.Where(n => n.EstadoRevision == "Pendiente")
                              .OrderByDescending(n => n.Fecha).ToList();
            Revisadas = todas.Where(n => n.EstadoRevision == "Revisado")
                              .OrderByDescending(n => n.Fecha).ToList();
        }

        public async Task<IActionResult> OnPostMarcarRevisadaAsync(int id)
        {
            if (id == 0)
            {
                Mensaje = "Novedad no encontrada.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var marcada = await _novedadHttpService.MarcarRevisadaAsync(id);
            Mensaje = marcada ? "Novedad marcada como revisada." : "No se pudo actualizar la novedad.";
            MensajeTipo = marcada ? "ok" : "error";
            return RedirectToPage();
        }
    }
}