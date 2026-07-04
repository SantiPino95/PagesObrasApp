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
        private readonly IEmpleadoHttpService _empleadoHttpService;
        private readonly IObraHttpService _obraHttpService;
        private readonly IAsignacionHttpService _asignacionHttpService;

        public EmpleadosModel(
            IEmpleadoHttpService empleadoHttpService,
            IObraHttpService obraHttpService,
            IAsignacionHttpService asignacionHttpService)
        {
            _empleadoHttpService = empleadoHttpService;
            _obraHttpService = obraHttpService;
            _asignacionHttpService = asignacionHttpService;
        }

        public List<EmpleadoListadoDto> Empleados { get; set; } = new();
        public List<ObraListadoDto> Obras { get; set; } = new();
        public List<EmpleadoObraDto> Asignaciones { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Empleados = await _empleadoHttpService.ObtenerEmpleadosAsync() ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
            Asignaciones = await _asignacionHttpService.ObtenerAsignacionesAsync() ?? new();
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

            var creado = await _empleadoHttpService.CrearEmpleadoAsync(dto);
            Mensaje = creado ? $"Empleado \"{nombre} {apellido}\" dado de alta." : "No se pudo crear el empleado.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAsignarEmpleadoAsync(int idEmpleado, int idObra)
        {
            if (idEmpleado == 0 || idObra == 0)
            {
                Mensaje = "Seleccioná empleado y obra.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var asignado = await _asignacionHttpService.CrearAsignacionAsync(idEmpleado, idObra);
            Mensaje = asignado ? "Asignación creada correctamente." : "No se pudo crear la asignación.";
            MensajeTipo = asignado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}