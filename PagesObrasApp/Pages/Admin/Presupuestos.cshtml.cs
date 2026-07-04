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
        private readonly IPresupuestoHttpService _presupuestoHttpService;
        private readonly IObraHttpService _obraHttpService;

        public PresupuestosModel(
            IPresupuestoHttpService presupuestoHttpService,
            IObraHttpService obraHttpService)
        {
            _presupuestoHttpService = presupuestoHttpService;
            _obraHttpService = obraHttpService;
        }

        public List<PresupuestoListadoDto> Presupuestos { get; set; } = new();
        public List<ObraListadoDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Presupuestos = await _presupuestoHttpService.ObtenerPresupuestosAsync() ?? new();
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
        }

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

            var cambiado = await _presupuestoHttpService.CambiarEstadoAsync(id, dto);
            Mensaje = cambiado
                ? $"Presupuesto PRES-{id:D4} {nuevoEstado.ToLower()} correctamente."
                : "No se pudo cambiar el estado del presupuesto.";
            MensajeTipo = cambiado ? (accion == "Aprobar" ? "ok" : "error") : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCrearPresupuestoAsync(
            int idObra,
            List<string> descripciones,
            List<decimal> cantidades,
            List<decimal> preciosUnitarios)
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

            var creado = await _presupuestoHttpService.CrearPresupuestoAsync(dto);
            Mensaje = creado ? "Presupuesto creado correctamente." : "No se pudo crear el presupuesto.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}