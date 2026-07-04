using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class OrdenesCompraModel : PageModel
    {
        private readonly IOrdenCompraHttpService _ordenCompraHttpService;
        private readonly IProveedorHttpService _proveedorHttpService;
        private readonly IMaterialHttpService _materialHttpService;

        public OrdenesCompraModel(
            IOrdenCompraHttpService ordenCompraHttpService,
            IProveedorHttpService proveedorHttpService,
            IMaterialHttpService materialHttpService)
        {
            _ordenCompraHttpService = ordenCompraHttpService;
            _proveedorHttpService = proveedorHttpService;
            _materialHttpService = materialHttpService;
        }

        public List<OrdenCompraListadoDto> Ordenes { get; set; } = new();
        public List<ProveedorListadoDto> Proveedores { get; set; } = new();
        public List<MaterialListadoDto> Materiales { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Ordenes = await _ordenCompraHttpService.ObtenerOrdenesAsync() ?? new();
            Proveedores = await _proveedorHttpService.ObtenerProveedoresAsync() ?? new();
            Materiales = await _materialHttpService.ObtenerMaterialesAsync() ?? new();
        }

        public async Task<IActionResult> OnPostCrearOrdenAsync(
            int idProveedor, DateTime fechaPedido,
            List<int> idMateriales, List<decimal> cantidades, List<decimal> precios)
        {
            if (idProveedor == 0 || idMateriales == null || idMateriales.Count == 0)
            {
                Mensaje = "Seleccioná proveedor y al menos un material.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var detalles = new List<DetalleCrearOrdenDto>();
            for (int i = 0; i < idMateriales.Count; i++)
            {
                detalles.Add(new DetalleCrearOrdenDto
                {
                    IdMaterial = idMateriales[i],
                    CantidadPedida = cantidades[i],
                    PrecioUnitarioCompra = precios[i]
                });
            }

            var dto = new CrearOrdenCompraDto
            {
                IdProveedor = idProveedor,
                FechaPedido = fechaPedido,
                Detalles = detalles
            };

            var creada = await _ordenCompraHttpService.CrearOrdenAsync(dto);
            Mensaje = creada ? "Orden de compra creada correctamente." : "No se pudo crear la orden.";
            MensajeTipo = creada ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostConfirmarEntregaAsync(int id)
        {
            if (id == 0)
            {
                Mensaje = "Orden no encontrada.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            // El servicio se encarga de: cambiar Estado_Entrega a "Entregado"
            // y sumar cada Cantidad_Pedida al Stock_Material correspondiente
            var confirmada = await _ordenCompraHttpService.ConfirmarEntregaAsync(id);
            Mensaje = confirmada ? "Entrega confirmada. Stock actualizado." : "No se pudo confirmar la entrega.";
            MensajeTipo = confirmada ? "ok" : "error";
            return RedirectToPage();
        }
    }
}