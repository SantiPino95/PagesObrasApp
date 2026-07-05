using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class IndexModel : PageModel
    {
        private readonly IObraHttpService _obraHttpService;
        private readonly IHerramientaHttpService _herramientaHttpService;

        public IndexModel(IObraHttpService obraHttpService, IHerramientaHttpService herramientaHttpService)
        {
            _obraHttpService = obraHttpService;
            _herramientaHttpService = herramientaHttpService;
        }

        public List<ObraListadoDto> ObrasActivas { get; set; } = new();
        public int ObrasEnProgreso { get; set; }
        public int HerramientasEnReparacion { get; set; }

        public async Task OnGetAsync()
        {
            var obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
            ObrasActivas = obras.Where(o => o.Estado == "En Progreso").ToList();
            ObrasEnProgreso = ObrasActivas.Count;

            var herramientas = await _herramientaHttpService.ObtenerHerramientasAsync() ?? new();
            HerramientasEnReparacion = herramientas.Count(h => h.EstadoDisponibilidad == "En Reparación");
        }
    }
}