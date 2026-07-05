using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class MaterialesModel : PageModel
    {
        private readonly IMaterialHttpService _materialHttpService;
        private readonly IObraHttpService _obraHttpService;

        public MaterialesModel(IMaterialHttpService materialHttpService, IObraHttpService obraHttpService)
        {
            _materialHttpService = materialHttpService;
            _obraHttpService = obraHttpService;
        }

        public List<MaterialListadoDto> Materiales { get; set; } = new();
        public List<ObraAdminListadoDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Materiales = await _materialHttpService.ObtenerMaterialesAsync() ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
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

        public async Task<IActionResult> OnPostConsumirMaterialAsync(
            int idMaterial, int idObra, decimal cantidad)
        {
            if (idMaterial == 0 || idObra == 0 || cantidad <= 0)
            {
                Mensaje = "Seleccioná material, obra y una cantidad válida.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var consumido = await _materialHttpService.ConsumirMaterialAsync(idMaterial, idObra, cantidad);
            Mensaje = consumido ? "Consumo registrado y stock actualizado." : "No se pudo registrar el consumo (verificá stock disponible).";
            MensajeTipo = consumido ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostReponerStockAsync(int idMaterial, decimal cantidad)
        {
            if (idMaterial == 0 || cantidad <= 0)
            {
                Mensaje = "Seleccioná material y una cantidad válida.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new ReponerStockDto
            {
                IdMaterial = idMaterial,
                Cantidad = cantidad
            };

            var repuesto = await _materialHttpService.ReponerStockAsync(dto);
            Mensaje = repuesto ? $"Stock repuesto con éxito. Se agregaron {cantidad} unidades." : "No se pudo reponer el stock.";
            MensajeTipo = repuesto ? "ok" : "error";
            return RedirectToPage();
        }
    }
}