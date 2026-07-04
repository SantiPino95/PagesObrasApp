using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class PagoProveedorModel : PageModel
    {
        private readonly IPagoProveedorHttpService _pagoProveedorHttpService;
        private readonly IProveedorHttpService _proveedorHttpService;

        public static readonly string[] Metodos =
            { "Transferencia", "Cheque", "Efectivo", "Débito" };

        public PagoProveedorModel(
            IPagoProveedorHttpService pagoProveedorHttpService,
            IProveedorHttpService proveedorHttpService)
        {
            _pagoProveedorHttpService = pagoProveedorHttpService;
            _proveedorHttpService = proveedorHttpService;
        }

        public List<PagoProveedorListadoDto> Pagos { get; set; } = new();
        public List<ProveedorListadoDto> Proveedores { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Pagos = await _pagoProveedorHttpService.ObtenerPagosAsync() ?? new();
            Proveedores = await _proveedorHttpService.ObtenerProveedoresAsync() ?? new();
        }

        public async Task<IActionResult> OnPostRegistrarPagoAsync(
            int idProveedor, DateTime fechaPago, string metodoPago, decimal monto)
        {
            if (idProveedor == 0 || monto <= 0)
            {
                Mensaje = "Seleccioná proveedor e ingresá un monto válido.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            if (!Metodos.Contains(metodoPago))
            {
                Mensaje = "Método de pago inválido.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearPagoProveedorDto
            {
                IdProveedor = idProveedor,
                FechaPago = fechaPago,
                MetodoPago = metodoPago,
                Monto = monto
            };

            var creado = await _pagoProveedorHttpService.CrearPagoAsync(dto);
            Mensaje = creado ? $"Pago de ${monto:N0} registrado correctamente." : "No se pudo registrar el pago.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}