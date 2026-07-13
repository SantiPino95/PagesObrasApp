using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.Ocsp;

namespace PagesObrasApp.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // 1. Borra la cookie de autenticación del proyecto Razor
            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 2. Si guardas el token JWT manualmente en una cookie propia, la borras así:
            if (Request.Cookies["JwtToken"] != null)
            {
                Response.Cookies.Delete("JwtToken");
            }

            // 3. Redirige de forma segura a la página de inicio (Index)
            return RedirectToPage("/Index");
        }
    }
}

