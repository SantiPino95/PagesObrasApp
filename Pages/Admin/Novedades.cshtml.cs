using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class NovedadesModel : PageModel
    {
        private readonly INovedadHttpService _novedadHttpService;
        private readonly IObraHttpService _obraHttpService;

        public NovedadesModel(INovedadHttpService novedadHttpService, IObraHttpService obraHttpService)
        {
            _novedadHttpService = novedadHttpService;
            _obraHttpService = obraHttpService;
        }

        public List<NovedadListadoDto> Novedades { get; set; } = new();
        public List<ObraAdminListadoDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Novedades = await _novedadHttpService.ObtenerNovedadesAsync() ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
        }

        public async Task<IActionResult> OnPostCrearNovedadAsync(
            int idEmpleadoObra, DateTime fecha, string tipoNovedad, string descripcion)
        {
            if (idEmpleadoObra == 0 || string.IsNullOrWhiteSpace(tipoNovedad) || string.IsNullOrWhiteSpace(descripcion))
            {
                Mensaje = "Todos los campos son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearNovedadDto
            {
                IdEmpleadoObra = idEmpleadoObra,
                Fecha = fecha,
                TipoNovedad = tipoNovedad,
                Descripcion = descripcion,
                EstadoRevision = "Pendiente"
            };

            var creado = await _novedadHttpService.CrearNovedadAsync(dto);
            Mensaje = creado ? "Novedad registrada correctamente." : "No se pudo registrar la novedad.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostMarcarRevisadaAsync(int id)
        {
            if (id == 0)
            {
                Mensaje = "Novedad no encontrada.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var marcada = await _novedadHttpService.CambiarEstadoAsync(id, "Revisado");
            Mensaje = marcada ? "Novedad marcada como revisada." : "No se pudo actualizar la novedad.";
            MensajeTipo = marcada ? "ok" : "error";
            return RedirectToPage();
        }
    }
}