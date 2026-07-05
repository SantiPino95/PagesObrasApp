using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class GastosModel : PageModel
    {
        private readonly IObraHttpService _obraHttpService;

        public static readonly string[] Categorias =
        {
            "Materiales", "Mano de Obra", "Herramientas",
            "Transporte", "Alquiler Equipos", "Servicios", "Otro"
        };

        public GastosModel(IObraHttpService obraHttpService)
        {
            _obraHttpService = obraHttpService;
        }

        public List<ObraListadoDto> Obras { get; set; } = new();
        public bool ModuloDisponible => false; // no existe controller de Gastos todavía

        public async Task OnGetAsync()
        {
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
        }
    }
}