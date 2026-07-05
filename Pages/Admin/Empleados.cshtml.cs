using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class EmpleadosModel : PageModel
    {
        private readonly IEmpleadoHttpService _empleadoHttpService;
        private readonly IObraHttpService _obraHttpService;

        public EmpleadosModel(IEmpleadoHttpService empleadoHttpService, IObraHttpService obraHttpService)
        {
            _empleadoHttpService = empleadoHttpService;
            _obraHttpService = obraHttpService;
        }

        public List<EmpleadoListadoDTOs> Empleados { get; set; } = new();
        public List<ObraAdminListadoDto> Obras { get; set; } = new();
        public List<EmpleadoObraDTOs> Asignados { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Empleados = await _empleadoHttpService.ObtenerEmpleadosAsync() ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
            Asignados = await _empleadoHttpService.ObtenerAsignacionesAsync() ?? new();
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

            var dto = new CrearEmpleadoDTOs
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

        public async Task<IActionResult> OnPostAsignarEmpleadoAsync(
            int idEmpleado, int idObra, decimal valorHoraAsignado, string rolEnObra)
        {
            if (idEmpleado == 0 || idObra == 0)
            {
                Mensaje = "Seleccioná empleado y obra.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new AsignarEmpleadoObraDto
            {
                IdEmpleado = idEmpleado,
                IdObra = idObra,
                RolEnObra = rolEnObra,
                ValorHoraAsignado = valorHoraAsignado
            };

            var asignado = await _empleadoHttpService.AsignarAObraAsync(dto);
            Mensaje = asignado ? "Empleado asignado a la obra correctamente." : "No se pudo asignar (verificá que no esté ya asignado hoy).";
            MensajeTipo = asignado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}