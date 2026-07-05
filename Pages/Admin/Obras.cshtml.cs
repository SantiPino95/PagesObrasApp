
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class ObrasModel : PageModel
    {
        private readonly IObraHttpService _obraHttpService;
        private readonly IClienteHttpService _clienteHttpService;

        public ObrasModel(IObraHttpService obraHttpService, IClienteHttpService clienteHttpService)
        {
            _obraHttpService = obraHttpService;
            _clienteHttpService = clienteHttpService;
        }

        public List<ObraAdminListadoDto> Obras { get; set; } = new();
        public List<CLientesListadoDTOs> Clientes { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
            Clientes = await _clienteHttpService.ObtenerClientesAsync() ?? new();
        }

        public async Task<IActionResult> OnPostCrearObraAsync(
            string nombreObra, int idCliente, string direccion,
            DateTime fechaInicio, DateTime? fechaFinPrevista)
        {
            if (string.IsNullOrWhiteSpace(nombreObra) || idCliente == 0 || string.IsNullOrWhiteSpace(direccion))
            {
                Mensaje = "Nombre, cliente y dirección son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearObraDto
            {
                NombreObra = nombreObra,
                IdCliente = idCliente,
                Direccion = direccion,
                FechaInicio = fechaInicio,
                FechaFinPrevista = fechaFinPrevista
            };

            var creado = await _obraHttpService.CrearObraAsync(dto);
            Mensaje = creado ? $"Obra \"{nombreObra}\" creada correctamente." : "No se pudo crear la obra.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}