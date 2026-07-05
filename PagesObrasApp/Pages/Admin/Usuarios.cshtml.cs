using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class UsuariosModel : PageModel
    {
        public bool ModuloDisponible => false; // no existe controller de Usuarios/Auth todavía

        public void OnGet() { }
    }
}