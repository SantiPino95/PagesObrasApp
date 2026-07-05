using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class PresupuestosModel : PageModel
    {
        private readonly IApiService _api;
        private readonly IObraHttpService _obraHttpService;

        public PresupuestosModel(IApiService api, IObraHttpService obraHttpService)
        {
            _api = api;
            _obraHttpService = obraHttpService;
        }

        public List<PresupuestoListadoDto> Presupuestos { get; set; } = new();
        public List<ObraListadoDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Presupuestos = await _api.GetAsync<List<PresupuestoListadoDto>>("api/Presupuestos") ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
        }

        public async Task<IActionResult> OnPostCrearPresupuestoAsync(
            int idObra, List<string> descripciones, List<decimal> cantidades, List<decimal> preciosUnitarios)
        {
            if (idObra == 0 || descripciones == null || descripciones.Count == 0)
            {
                Mensaje = "Seleccioná obra y agregá al menos un ítem.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var lineas = new List<DetallePresupuestoDto>();
            for (int i = 0; i < descripciones.Count; i++)
            {
                lineas.Add(new DetallePresupuestoDto
                {
                    Descripcion = descripciones[i],
                    Cantidad = cantidades[i],
                    PrecioUnitario = preciosUnitarios[i]
                });
            }

            var dto = new CrearPresupuestoDto
            {
                IdObra = idObra,
                EstadoPresupuesto = "Pendiente",
                Lineas = lineas
            };

            var response = await _api.PostAsync("api/Presupuestos", dto);
            Mensaje = response.IsSuccessStatusCode ? "Presupuesto creado correctamente." : "No se pudo crear el presupuesto.";
            MensajeTipo = response.IsSuccessStatusCode ? "ok" : "error";
            return RedirectToPage();
        }

        // ── NUEVO: ya existe PUT api/Presupuestos/{id}/estado ──
        public async Task<IActionResult> OnPostCambiarEstadoAsync(int id, string accion)
        {
            if (id == 0 || (accion != "Aprobar" && accion != "Rechazar"))
            {
                Mensaje = "Acción inválida.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var nuevoEstado = accion == "Aprobar" ? "Aprobado" : "Rechazado";
            var dto = new CambiarEstadoPresupuestoDto { Estado = nuevoEstado };

            var response = await _api.PutAsync($"api/Presupuestos/{id}/estado", dto);
            Mensaje = response.IsSuccessStatusCode
                ? $"Presupuesto PRES-{id:D4} {nuevoEstado.ToLower()} correctamente."
                : "No se pudo cambiar el estado del presupuesto.";
            MensajeTipo = response.IsSuccessStatusCode ? (accion == "Aprobar" ? "ok" : "error") : "error";
            return RedirectToPage();
        }
    }
}