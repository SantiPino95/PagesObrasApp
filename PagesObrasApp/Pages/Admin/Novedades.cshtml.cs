using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class NovedadesModel : PageModel
    {
        public List<NovedadDto> Pendientes { get; set; } = new();
        public List<NovedadDto> Revisadas { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet()
        {
            // TODO: GET /api/novedades
            var todas = new List<NovedadDto>
            {
                new() { Id=1, Empleado="Rodrigo Méndez",   Obra="Edificio Las Acacias",     Fecha=DateTime.Today,              Tipo="Falta de Material",   Desc="Se agotó el cemento Portland. Quedan aprox. 2 bolsas. Necesitamos reposición urgente.",         Estado="Pendiente" },
                new() { Id=2, Empleado="Luciana Torres",   Obra="Torre Mirador",            Fecha=DateTime.Today,              Tipo="Rotura Equipo",       Desc="La amoladora angular HER-0045 dejó de funcionar. Cortocircuito.",                              Estado="Pendiente" },
                new() { Id=3, Empleado="Carlos Espínola",  Obra="Edificio Las Acacias",     Fecha=DateTime.Today.AddDays(-1),  Tipo="Incidencia Personal", Desc="Compañero Ríos sufrió un corte leve al manipular hierro. Primeros auxilios aplicados.",         Estado="Pendiente" },
                new() { Id=4, Empleado="Diego Pereira",    Obra="Galpón Industrial Ruta 5", Fecha=DateTime.Today.AddDays(-1),  Tipo="Clima",               Desc="Fuertes lluvias impidieron trabajar en exterior. 4 horas perdidas.",                            Estado="Pendiente" },
                new() { Id=5, Empleado="Sebastián Ferreira",Obra="Casa Familia Pérez",      Fecha=DateTime.Today.AddDays(-2),  Tipo="Falta de Material",   Desc="No llegó el pedido de ladrillos. El proveedor no respondió.",                                  Estado="Pendiente" },
                new() { Id=6, Empleado="Carlos Espínola",  Obra="Galpón Industrial Ruta 5", Fecha=DateTime.Today.AddDays(-3),  Tipo="Otro",                Desc="Vecinos se quejaron por el ruido de la soldadora después de las 18hs.",                         Estado="Revisado"  },
                new() { Id=7, Empleado="Rodrigo Méndez",   Obra="Edificio Las Acacias",     Fecha=DateTime.Today.AddDays(-4),  Tipo="Clima",               Desc="Tormenta eléctrica obligó a suspender trabajos en altura. 6 horas perdidas.",                   Estado="Revisado"  },
            };

            Pendientes = todas.Where(n => n.Estado == "Pendiente").OrderByDescending(n => n.Fecha).ToList();
            Revisadas = todas.Where(n => n.Estado == "Revisado").OrderByDescending(n => n.Fecha).ToList();
        }

        public IActionResult OnPostMarcarRevisada(int id)
        {
            if (id == 0)
            {
                Mensaje = "Novedad no encontrada."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: PATCH /api/novedades/{id}/revisar
            // La API cambia Estado_Revision = "Revisado"
            Mensaje = "Novedad marcada como revisada."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        public IActionResult OnPostMarcarTodasRevisadas()
        {
            // TODO: PATCH /api/novedades/revisar-todas
            Mensaje = "Todas las novedades marcadas como revisadas."; MensajeTipo = "ok";
            return RedirectToPage();
        }
    }
}