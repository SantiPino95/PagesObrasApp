using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Cliente
{
    public class SeguimientoModel : PageModel
    {
        private readonly IObraHttpService _obraHttpService;
        private readonly ISeguimientoHttpService _seguimientoHttpService;
        private readonly IPresupuestoHttpService _presupuestoHttpService;

        public SeguimientoModel(
            IObraHttpService obraHttpService,
            ISeguimientoHttpService seguimientoHttpService,
            IPresupuestoHttpService presupuestoHttpService)
        {
            _obraHttpService = obraHttpService;
            _seguimientoHttpService = seguimientoHttpService;
            _presupuestoHttpService = presupuestoHttpService;
        }

        // ── Propiedades que lee la vista ──────────────────────────────────
        public string Codigo { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }

        public ObraAdminListadoDto Obra { get; set; } = null!;
        public List<SeguimientoListadoDto> Seguimientos { get; set; } = new();
        public List<PresupuestoListadoDto> PresupuestosAprobados { get; set; } = new();

        // ── GET ───────────────────────────────────────────────────────────

        public async Task<IActionResult> OnGetAsync(string? codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return RedirectToPage("/Index");
            }

            var codigoNorm = codigo.Trim().ToUpper();

            // No hay endpoint de búsqueda pública por código — se trae el listado
            // completo y se filtra acá por CodigoFormateado.
            var obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
            var obraEncontrada = obras.FirstOrDefault(o =>
                string.Equals(o.CodigoFormateado?.Trim(), codigoNorm, StringComparison.OrdinalIgnoreCase));

            if (obraEncontrada == null)
            {
                return RedirectToPage("/Index", new { errorCodigo = codigoNorm });
            }

            Codigo = codigoNorm;
            Obra = obraEncontrada;

            Seguimientos = await _seguimientoHttpService.ObtenerPorObraAsync(Obra.IdObra) ?? new();
            Seguimientos = Seguimientos.OrderByDescending(s => s.Fecha).ToList();

            var presupuestos = await _presupuestoHttpService.ObtenerPresupuestosAsync() ?? new();
            PresupuestosAprobados = presupuestos
                .Where(p => p.IdObra == Obra.IdObra && p.EstadoPresupuesto == "Aprobado")
                .OrderByDescending(p => p.FechaEmision)
                .ToList();

            return Page();
        }
    }
}