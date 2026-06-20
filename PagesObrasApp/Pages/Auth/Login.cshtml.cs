using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace PagesObrasApp.Pages.Account
{

    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            // AQUÍ HARDCODEAMOS EL ADMIN ÚNICO PROGRAMADO POR TI
            if (Email == "admin@constructora.com" && Password == "UTU2026")
            {
                // ¡Éxito! De momento lo redirigimos directo al Admin sin cookies 
                // (Luego cuando configuremos las Cookies, esto asegurará el rol)
                return RedirectToPage("/Admin/Index");
            }

            // Si se equivoca, mandamos un error a la pantalla
            ModelState.AddModelError(string.Empty, "Credenciales incorrectas.");
            return Page();
        }
    }
}