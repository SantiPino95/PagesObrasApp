using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class EmpleadosModel : PageModel
    {
        private readonly IApiService _api;
        private readonly IObraHttpService _obraHttpService;

        public EmpleadosModel(IApiService api, IObraHttpService obraHttpService)
        {
            _api = api;
            _obraHttpService = obraHttpService;
        }

        public List<EmpleadoListadoDto> Empleados { get; set; } = new();
        public List<ObraListadoDto> Obras { get; set; } = new();
        public List<EmpleadoObraDto> Asignados { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Empleados = await _api.GetAsync<List<EmpleadoListadoDto>>("api/Empleado") ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
            Asignados = await _api.GetAsync<List<EmpleadoObraDto>>("api/Empleado/asignados") ?? new();
        }

        public async Task<IActionResult> OnPostCrearEmpleadoAsync(
            string nombre, string apellido, string categoria, string? telefono)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) || string.IsNullOrWhiteSpace(categoria))
            {
                Mensaje = "Nombre, apellido y categoría son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearEmpleadoDto
            {
                Nombre = nombre,
                Apellido = apellido,
                Categoria = categoria,
                Telefono = telefono
            };

            var response = await _api.PostAsync("api/Empleado", dto);
            Mensaje = response.IsSuccessStatusCode ? $"Empleado \"{nombre} {apellido}\" dado de alta." : "No se pudo crear el empleado.";
            MensajeTipo = response.IsSuccessStatusCode ? "ok" : "error";
            return RedirectToPage();
        }

        // Ruta real confirmada por Swagger: POST /api/Empleado/asignar (sin {id}, todo va en el body)
        public async Task<IActionResult> OnPostAsignarEmpleadoAsync(
            int idEmpleado, int idObra, decimal valorHoraAsignado, string rolEnObra)
        {
            if (idEmpleado == 0 || idObra == 0)
            {
                Mensaje = "Seleccioná empleado y obra.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new AsignarEmpleadoDto
            {
                IdEmpleado = idEmpleado,
                IdObra = idObra,
                RolEnObra = rolEnObra,
                ValorHoraAsignado = valorHoraAsignado
            };

            var response = await _api.PostAsync("api/Empleado/asignar", dto);

            Mensaje = response.IsSuccessStatusCode ? "Empleado asignado a la obra correctamente." : "No se pudo asignar (verificá que no esté ya asignado hoy).";
            MensajeTipo = response.IsSuccessStatusCode ? "ok" : "error";
            return RedirectToPage();
        }
    }
}
