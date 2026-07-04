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
        private readonly IProveedorHttpService _proveedorHttpService;

        public ProveedoresModel(IProveedorHttpService proveedorHttpService)
        {
            _proveedorHttpService = proveedorHttpService;
        }

        public List<ProveedorListadoDto> Proveedores { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Proveedores = await _proveedorHttpService.ObtenerProveedoresAsync() ?? new();
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

            var dto = new CrearProveedorDto
            {
                Nombre = nombre,
                Rut = rut,
                Telefono = tel,
                Email = email
            };

            var creado = await _proveedorHttpService.CrearProveedorAsync(dto);
            Mensaje = creado ? $"Proveedor \"{nombre}\" creado correctamente." : "No se pudo crear el proveedor.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}