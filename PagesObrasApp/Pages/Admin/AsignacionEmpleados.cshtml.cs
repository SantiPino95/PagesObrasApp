using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class AsignacionEmpleadosModel : PageModel
    {
        private readonly IApiService _api;
        private readonly IObraHttpService _obraHttpService;

        public AsignacionEmpleadosModel(IApiService api, IObraHttpService obraHttpService)
        {
            _api = api;
            _obraHttpService = obraHttpService;
        }

        public List<EmpleadoObraDto> Asignaciones { get; set; } = new();
        public List<EmpleadoListadoDto> Empleados { get; set; } = new();
        public List<ObraListadoDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Asignaciones = await _api.GetAsync<List<EmpleadoObraDto>>("api/Empleado/asignados") ?? new();
            Empleados = await _api.GetAsync<List<EmpleadoListadoDto>>("api/Empleado") ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
        }

        // ── NUEVO ──
        public async Task<IActionResult> OnPostCrearAsignacionAsync(
            int idEmpleado, int idObra, decimal valorHoraAsignado, string rolEnObra)
        {
            if (idEmpleado == 0 || idObra == 0)
            {
                Mensaje = "Seleccioná empleado y obra.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new { IdObra = idObra, ValorHoraAsignado = valorHoraAsignado, RolEnObra = rolEnObra };
            var response = await _api.PostAsync($"api/Empleado/{idEmpleado}/asignar", dto);

            Mensaje = response.IsSuccessStatusCode ? "Asignación creada correctamente." : "No se pudo crear la asignación.";
            MensajeTipo = response.IsSuccessStatusCode ? "ok" : "error";
            return RedirectToPage();
        }

        // ── NUEVO ──
        public async Task<IActionResult> OnPostQuitarAsignacionAsync(int idEmpleado, int idObra)
        {
            if (idEmpleado == 0 || idObra == 0)
            {
                Mensaje = "Asignación no encontrada.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var response = await _api.DeleteAsync($"api/Empleado/{idEmpleado}/asignar/{idObra}");

            Mensaje = response.IsSuccessStatusCode ? "Asignación eliminada correctamente." : "No se pudo eliminar la asignación.";
            MensajeTipo = response.IsSuccessStatusCode ? "ok" : "error";
            return RedirectToPage();
        }
    }
}