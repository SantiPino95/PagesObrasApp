using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class GastosModel : PageModel
    {
        private readonly IGastoHttpService _gastoHttpService;
        private readonly IObraHttpService _obraHttpService;

        public static readonly string[] Categorias =
        {
            "Materiales", "Mano de Obra", "Herramientas",
            "Transporte", "Alquiler Equipos", "Servicios", "Otro"
        };

        public GastosModel(IGastoHttpService gastoHttpService, IObraHttpService obraHttpService)
        {
            _gastoHttpService = gastoHttpService;
            _obraHttpService = obraHttpService;
        }

        public List<GastoListadoDto> Gastos { get; set; } = new();
        public List<ObraListadoDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Gastos = await _gastoHttpService.ObtenerGastosAsync() ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
        }

        public async Task<IActionResult> OnPostRegistrarGastoAsync(
            int idObra, DateTime fecha, string categoriaGasto,
            string descripcion, decimal monto, string? nroComprobante)
        {
            if (idObra == 0 || monto <= 0 || string.IsNullOrWhiteSpace(descripcion))
            {
                Mensaje = "Completá obra, descripción y monto.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearGastoDto
            {
                IdObra = idObra,
                Fecha = fecha,
                Monto = monto,
                Descripcion = descripcion,
                CategoriaGasto = categoriaGasto,
                NroComprobante = nroComprobante
            };

            var creado = await _gastoHttpService.CrearGastoAsync(dto);
            Mensaje = creado ? "Gasto registrado correctamente." : "No se pudo registrar el gasto.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}