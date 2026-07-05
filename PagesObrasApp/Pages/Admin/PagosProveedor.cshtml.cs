using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class PagosProveedorModel : PageModel
    {
        private readonly IApiService _api;

        public PagosProveedorModel(IApiService api)
        {
            _api = api;
        }

        public List<ProveedorListadoDto> Proveedores { get; set; } = new();
        public bool ModuloDisponible => false; // no existe controller de Pagos todavía

        public async Task OnGetAsync()
        {
            Proveedores = await _api.GetAsync<List<ProveedorListadoDto>>("api/Proveedores") ?? new();
        }
    }
}