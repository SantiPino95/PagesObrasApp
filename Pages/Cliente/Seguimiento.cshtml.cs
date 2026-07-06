using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PagesObrasApp.Pages.Cliente
{
    public class SeguimientoModel : PageModel
    {
        // ── Propiedades que lee la vista ──────────────────────────────────

        // Código de la obra validado (la vista lo puede usar para mostrarlo)
        public string Codigo { get; set; } = string.Empty;

        // Si algo falla se usa en la vista para mostrar el error
        public string? ErrorMessage { get; set; }


        // ── GET ───────────────────────────────────────────────────────────

        public IActionResult OnGet(string? codigo)
        {
            // 1. Si no viene código en la URL → volver al inicio
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return RedirectToPage("/Index");
            }

            var codigoNorm = codigo.Trim().ToUpper();

            // 2. TODO: reemplazar por llamada al HttpClient hacia la API
            //    GET /api/obras/publico/{codigoNorm}
            //    Si la API devuelve 404 → obra no encontrada
            //    Si devuelve 200 → cargar el DTO en propiedades del modelo
            var codigosValidos = new[]
            {
                "OB-2026-014",
                "OB-2026-002",
                "OB-2026-011",
                "OB-2026-019",
                "OB-2025-087",
            };

            if (!codigosValidos.Contains(codigoNorm))
            {
                // Código inválido → volver al index con el error en la URL
                // para que el tab de cliente lo muestre
                return RedirectToPage("/Index", new { errorCodigo = codigoNorm });
            }

            // 3. Código válido → guardar en la propiedad para que la vista lo use
            Codigo = codigoNorm;

            // TODO: acá se cargarán las propiedades de la obra desde el DTO:
            // Obra = await _obraService.GetPorCodigoPublico(codigoNorm);
            // Seguimientos = await _seguimientoService.GetPorObra(Obra.Id);
            // Presupuestos = await _presupuestoService.GetAprobadosPorObra(Obra.Id);

            return Page();
        }
    }
}
