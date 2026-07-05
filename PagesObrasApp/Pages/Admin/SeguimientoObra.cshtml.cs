using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class SeguimientoObraModel : PageModel
    {
        private readonly IObraHttpService _obraHttpService;

        public SeguimientoObraModel(IObraHttpService obraHttpService)
        {
            _obraHttpService = obraHttpService;
        }

        public List<ObraListadoDto> Obras { get; set; } = new();
        public bool ModuloDisponible => false; // no existe controller de Seguimiento todavía

        public async Task OnGetAsync()
        {
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
        }
    }
}