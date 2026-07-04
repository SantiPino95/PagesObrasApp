using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class SeguimientoObraModel : PageModel
    {
        private readonly ISeguimientoHttpService _seguimientoHttpService;
        private readonly IObraHttpService _obraHttpService;

        public SeguimientoObraModel(
            ISeguimientoHttpService seguimientoHttpService,
            IObraHttpService obraHttpService)
        {
            _seguimientoHttpService = seguimientoHttpService;
            _obraHttpService = obraHttpService;
        }

        public List<ObraListadoDto> Obras { get; set; } = new();
        public List<SeguimientoListadoDto> Seguimientos { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
            Seguimientos = await _seguimientoHttpService.ObtenerSeguimientosAsync() ?? new();
        }

        public async Task<IActionResult> OnPostRegistrarAvanceAsync(
            int idObra, DateTime fecha, int porcentajeAvance,
            string descripcionAvance, string? imgProgreso)
        {
            if (idObra == 0 || string.IsNullOrWhiteSpace(descripcionAvance))
            {
                Mensaje = "Seleccioná obra y escribí la descripción.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            if (porcentajeAvance < 0 || porcentajeAvance > 100)
            {
                Mensaje = "El porcentaje debe estar entre 0 y 100.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearSeguimientoDto
            {
                IdObra = idObra,
                Fecha = fecha,
                PorcentajeAvance = porcentajeAvance,
                DescripcionAvance = descripcionAvance,
                ImgProgreso = imgProgreso
            };

            var creado = await _seguimientoHttpService.CrearSeguimientoAsync(dto);
            Mensaje = creado ? $"Avance del {porcentajeAvance}% registrado correctamente." : "No se pudo registrar el avance.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}