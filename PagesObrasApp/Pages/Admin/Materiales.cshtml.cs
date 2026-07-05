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
        private readonly IApiService _api;
        private readonly IObraHttpService _obraHttpService;

        public MaterialesModel(IApiService api, IObraHttpService obraHttpService)
        {
            _api = api;
            _obraHttpService = obraHttpService;
        }

        public List<MaterialListadoDto> Materiales { get; set; } = new();
        public List<ObraListadoDto> Obras { get; set; } = new();

        public bool EntradaStockDisponible => false; // ver "Falta en la API"

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Materiales = await _api.GetAsync<List<MaterialListadoDto>>("api/Materiales") ?? new();
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

            var response = await _api.PostAsync("api/Materiales", dto);
            Mensaje = response.IsSuccessStatusCode ? $"Material \"{nombre}\" creado correctamente." : "No se pudo crear el material.";
            MensajeTipo = response.IsSuccessStatusCode ? "ok" : "error";
            return RedirectToPage();
        }

        // Endpoint real — resta stock (consumo en obra), no entrada
        public async Task<IActionResult> OnPostConsumirMaterialAsync(
            int idMaterial, int idObra, decimal cantidad)
        {
            if (idMaterial == 0 || idObra == 0 || cantidad <= 0)
            {
                Mensaje = "Seleccioná material, obra y una cantidad válida.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var response = await _api.PostAsync(
                $"api/Materiales/consumir?idMaterial={idMaterial}&idObra={idObra}&cantidad={cantidad}",
                new { });

            Mensaje = response.IsSuccessStatusCode ? "Consumo registrado y stock actualizado." : "No se pudo registrar el consumo (verificá stock disponible).";
            MensajeTipo = response.IsSuccessStatusCode ? "ok" : "error";
            return RedirectToPage();
        }
    }
}