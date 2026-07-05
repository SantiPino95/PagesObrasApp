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
        private readonly IApiService _api;

        public OrdenesCompraModel(IApiService api)
        {
            _api = api;
        }

        public List<OrdenCompraListadoDto> Ordenes { get; set; } = new();
        public List<ProveedorListadoDto> Proveedores { get; set; } = new();
        public List<MaterialListadoDto> Materiales { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Ordenes = await _api.GetAsync<List<OrdenCompraListadoDto>>("api/OrdenesCompra") ?? new();
            Proveedores = await _api.GetAsync<List<ProveedorListadoDto>>("api/Proveedores") ?? new();
            Materiales = await _api.GetAsync<List<MaterialListadoDto>>("api/Materiales") ?? new();
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

            var response = await _api.PostAsync("api/OrdenesCompra", dto);
            Mensaje = response.IsSuccessStatusCode ? "Orden de compra creada correctamente." : "No se pudo crear la orden.";
            MensajeTipo = response.IsSuccessStatusCode ? "ok" : "error";
            return RedirectToPage();
        }

        // ── El botón que pediste al principio: suma al stock ──
        public async Task<IActionResult> OnPostConfirmarEntregaAsync(int id)
        {
            if (id == 0)
            {
                Mensaje = "Orden no encontrada.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var response = await _api.PutAsync<object?>($"api/OrdenesCompra/{id}/confirmar-entrega", null);
            Mensaje = response.IsSuccessStatusCode ? "Entrega confirmada. Stock actualizado." : "No se pudo confirmar la entrega.";
            MensajeTipo = response.IsSuccessStatusCode ? "ok" : "error";
            return RedirectToPage();
        }
    }
}