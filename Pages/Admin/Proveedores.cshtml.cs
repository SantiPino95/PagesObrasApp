using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
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
            string nombre, string rut, string telefono, string? email)
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
                Telefono = telefono,
                Email = email
            };

            var creado = await _proveedorHttpService.CrearProveedorAsync(dto);
            if (!creado)
            {
                Mensaje = "Ya existe un proveedor con ese RUT.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            Mensaje = $"Proveedor \"{nombre}\" creado correctamente.";
            MensajeTipo = "ok";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditarProveedorAsync(
            int id, string nombre, string rut, string telefono, string? email)
        {
            if (id == 0 || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(rut))
            {
                Mensaje = "Datos inválidos.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearProveedorDto
            {
                Nombre = nombre,
                Rut = rut,
                Telefono = telefono,
                Email = email
            };

            var actualizado = await _proveedorHttpService.ActualizarProveedorAsync(id, dto);
            Mensaje = actualizado ? "Proveedor actualizado correctamente." : "No se pudo actualizar el proveedor (verificá que el RUT no esté duplicado).";
            MensajeTipo = actualizado ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarProveedorAsync(int id)
        {
            if (id == 0)
            {
                Mensaje = "Proveedor no encontrado.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var eliminado = await _proveedorHttpService.EliminarProveedorAsync(id);
            Mensaje = eliminado ? "Proveedor eliminado correctamente." : "No se pudo eliminar el proveedor.";
            MensajeTipo = eliminado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}