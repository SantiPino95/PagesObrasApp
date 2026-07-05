using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class NovedadesModel : PageModel
    {
        public bool ModuloDisponible => false; // no existe controller de Novedades todavía

        public void OnGet() { }
    }
}