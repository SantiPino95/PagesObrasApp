using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class ProveedoresModel : PageModel
    {
        private readonly IApiService _api;

        public ProveedoresModel(IApiService api)
        {
            _api = api;
        }

        public List<ProveedorListadoDto> Proveedores { get; set; } = new();
        public bool EdicionDisponible => false; // ver "Falta en la API"

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Proveedores = await _api.GetAsync<List<ProveedorListadoDto>>("api/Proveedores") ?? new();
        }

        public async Task<IActionResult> OnPostCrearProveedorAsync(
            string nombre, string rut, string tel, string? email)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(rut))
            {
                Mensaje = "Nombre y RUT son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearProveedorDto { Nombre = nombre, Rut = rut, Telefono = tel, Email = email };
            var response = await _api.PostAsync("api/Proveedores", dto);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Mensaje = "Ya existe un proveedor con ese RUT.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            Mensaje = response.IsSuccessStatusCode ? $"Proveedor \"{nombre}\" creado correctamente." : "No se pudo crear el proveedor.";
            MensajeTipo = response.IsSuccessStatusCode ? "ok" : "error";
            return RedirectToPage();
        }
    }
}