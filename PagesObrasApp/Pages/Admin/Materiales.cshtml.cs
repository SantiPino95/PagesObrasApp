using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class MaterialesModel : PageModel
    {
        private readonly IMaterialHttpService _materialHttpService;
        private readonly IProveedorHttpService _proveedorHttpService;

        public MaterialesModel(
            IMaterialHttpService materialHttpService,
            IProveedorHttpService proveedorHttpService)
        {
            _materialHttpService = materialHttpService;
            _proveedorHttpService = proveedorHttpService;
        }

        public List<MaterialListadoDto> Materiales { get; set; } = new();
        public List<ProveedorListadoDto> Proveedores { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Materiales = await _materialHttpService.ObtenerMaterialesAsync() ?? new();
            Proveedores = await _proveedorHttpService.ObtenerProveedoresAsync() ?? new();
        }

        public async Task<IActionResult> OnPostCrearMaterialAsync(
            string nombre, string unidadMedida, decimal cantidadInicial, decimal stockMinimo)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(unidadMedida))
            {
                Mensaje = "Nombre y unidad de medida son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearMaterialDto
            {
                Nombre = nombre,
                UnidadMedida = unidadMedida,
                CantidadInicial = cantidadInicial,
                StockMinimo = stockMinimo
            };

            var creado = await _materialHttpService.CrearMaterialAsync(dto);
            Mensaje = creado ? $"Material \"{nombre}\" creado correctamente." : "No se pudo crear el material.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEntradaStockAsync(
            int idMaterial, decimal cantidad, DateTime fecha,
            int? idProveedor, string? nroComprobante)
        {
            if (idMaterial == 0 || cantidad <= 0)
            {
                Mensaje = "Seleccioná material e ingresá una cantidad válida.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new EntradaStockDto
            {
                IdMaterial = idMaterial,
                Cantidad = cantidad,
                Fecha = fecha,
                IdProveedor = idProveedor,
                NroComprobante = nroComprobante
            };

            var registrado = await _materialHttpService.RegistrarEntradaStockAsync(dto);
            Mensaje = registrado ? "Entrada de stock registrada correctamente." : "No se pudo registrar la entrada.";
            MensajeTipo = registrado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}