using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class GastosModel : PageModel
    {
        private readonly IGastoHttpService _gastoHttpService;
        private readonly IObraHttpService _obraHttpService;

        public GastosModel(IGastoHttpService gastoHttpService, IObraHttpService obraHttpService)
        {
            _gastoHttpService = gastoHttpService;
            _obraHttpService = obraHttpService;
        }

        public List<GastoListadoDto> Gastos { get; set; } = new();
        public List<ObraAdminListadoDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Gastos = await _gastoHttpService.ObtenerGastosAsync() ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
        }

        public async Task<IActionResult> OnPostCrearGastoAsync(
            int idObra, DateTime fecha, decimal monto, string descripcion, string categoriaGasto, string? nroComprobante)
        {
            if (idObra == 0 || monto <= 0 || string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(categoriaGasto))
            {
                Mensaje = "Obra, monto, descripción y categoría son obligatorios.";
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

        public async Task<IActionResult> OnPostActualizarGastoAsync(
            int id, int idObra, DateTime fecha, decimal monto, string descripcion, string categoriaGasto, string? nroComprobante)
        {
            if (id == 0 || idObra == 0 || monto <= 0 || string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(categoriaGasto))
            {
                Mensaje = "Todos los campos son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new ActualizarGastoDto
            {
                Fecha = fecha,
                Monto = monto,
                Descripcion = descripcion,
                CategoriaGasto = categoriaGasto,
                NroComprobante = nroComprobante
            };

            var actualizado = await _gastoHttpService.ActualizarGastoAsync(id, dto);
            Mensaje = actualizado ? "Gasto actualizado correctamente." : "No se pudo actualizar el gasto.";
            MensajeTipo = actualizado ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarGastoAsync(int id)
        {
            if (id == 0)
            {
                Mensaje = "Gasto no encontrado.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var eliminado = await _gastoHttpService.EliminarGastoAsync(id);
            Mensaje = eliminado ? "Gasto eliminado correctamente." : "No se pudo eliminar el gasto.";
            MensajeTipo = eliminado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}