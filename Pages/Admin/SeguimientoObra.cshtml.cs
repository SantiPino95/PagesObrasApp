using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class SeguimientoObraModel : PageModel
    {
        private readonly ISeguimientoHttpService _seguimientoHttpService;
        private readonly IObraHttpService _obraHttpService;

        public SeguimientoObraModel(ISeguimientoHttpService seguimientoHttpService, IObraHttpService obraHttpService)
        {
            _seguimientoHttpService = seguimientoHttpService;
            _obraHttpService = obraHttpService;
        }

        public List<ObraAdminListadoDto> Obras { get; set; } = new();
        public Dictionary<int, List<SeguimientoListadoDto>> SeguimientosPorObra { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();

            // Cargar seguimientos para cada obra
            foreach (var obra in Obras)
            {
                var seguimientos = await _seguimientoHttpService.ObtenerPorObraAsync(obra.IdObra) ?? new();
                SeguimientosPorObra[obra.IdObra] = seguimientos;
            }
        }

        public async Task<IActionResult> OnPostCrearSeguimientoAsync(
            int idObra, DateTime fecha, string descripcionAvance, int porcentajeAvance, string? imgProgreso)
        {
            if (idObra == 0 || string.IsNullOrWhiteSpace(descripcionAvance))
            {
                Mensaje = "Obra y descripción son obligatorios.";
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
                DescripcionAvance = descripcionAvance,
                PorcentajeAvance = porcentajeAvance,
                ImgProgreso = imgProgreso
            };

            var creado = await _seguimientoHttpService.CrearSeguimientoAsync(dto);
            Mensaje = creado ? "Avance registrado correctamente." : "No se pudo registrar el avance.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}