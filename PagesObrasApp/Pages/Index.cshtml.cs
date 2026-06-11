using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;

namespace PagesObrasApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel (IHttpClientFactory httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string CodigoObra { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostBuscarObraAsync(string codigoObra)
        {
            if (string.IsNullOrEmpty(codigoObra))
            {
                ErrorMessage = "Por favor, ingrese un código de obra.";
                return Page();
            }

            // Llamar a la API para verificar si el código existe
            var client = _httpClientFactory.CreateClient("API");

            try
            {
                var response = await client.GetAsync($"/api/obras/por-codigo/{codigoObra}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var obra = JsonSerializer.Deserialize<ObraDto>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (obra != null)
                    {
                        // Guardar el código en sesión o tempdata
                        TempData["CodigoObra"] = codigoObra;
                        TempData["ObraId"] = obra.Id_Obra;

                        // Redirigir a la página de seguimiento del cliente
                        return RedirectToPage("/Cliente/Seguimiento");
                    }
                }

                ErrorMessage = "Código de obra no válido. Verifique e intente nuevamente.";
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error al conectar con el servidor. Intente más tarde.";
                return Page();
            }
        }
    }

    // DTO para recibir datos de la API
    public class ObraDto
    {
        public int Id_Obra { get; set; }
        public string Nombre_Obra { get; set; }
        public string Codigo_Publico { get; set; }
        public string Direccion { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime? Fecha_Fin_Prevista { get; set; }
        public string Estado { get; set; }
    }
}