using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class PagosProveedorModel : PageModel
    {
        private readonly IPagoProveedorHttpService _pagoHttpService;
        private readonly IProveedorHttpService _proveedorHttpService;

        public PagosProveedorModel(IPagoProveedorHttpService pagoHttpService, IProveedorHttpService proveedorHttpService)
        {
            _pagoHttpService = pagoHttpService;
            _proveedorHttpService = proveedorHttpService;
        }

        public List<PagoProveedorListadoDto> Pagos { get; set; } = new();
        public List<ProveedorListadoDto> Proveedores { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Pagos = await _pagoHttpService.ObtenerPagosAsync() ?? new();
            Proveedores = await _proveedorHttpService.ObtenerProveedoresAsync() ?? new();
        }

        public async Task<IActionResult> OnPostCrearPagoAsync(
            int idProveedor, DateTime fechaPago, decimal monto, string metodoPago)
        {
            if (idProveedor == 0 || monto <= 0 || string.IsNullOrWhiteSpace(metodoPago))
            {
                Mensaje = "Proveedor, monto y método de pago son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearPagoProveedorDto
            {
                IdProveedor = idProveedor,
                FechaPago = fechaPago,
                Monto = monto,
                MetodoPago = metodoPago
            };

            var creado = await _pagoHttpService.CrearPagoAsync(dto);
            Mensaje = creado ? "Pago registrado correctamente." : "No se pudo registrar el pago.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarPagoAsync(int id)
        {
            if (id == 0)
            {
                Mensaje = "Pago no encontrado.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var eliminado = await _pagoHttpService.EliminarPagoAsync(id);
            Mensaje = eliminado ? "Pago eliminado correctamente." : "No se pudo eliminar el pago.";
            MensajeTipo = eliminado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}