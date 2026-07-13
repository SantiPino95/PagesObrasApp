
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


        public async Task<bool> ExisteObraPorCodigoPublicoAsync(string codigo)
        {
            // Ajusta la ruta del string según cómo manejes las URLs en tu HttpClient
            var response = await _obraHttpService.ExisteObraPorCodigoAsync(codigo);

            return response;
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


        public async Task<IActionResult> OnPostCambiarEstadoAsync(int id, string nuevoEstado)
        {
            // 1. Buscamos el objeto completo desde la API para no perder sus datos (Nombre, avance, etc.)
            var obra = await _obraHttpService.ObtenerObraPorIdAsync(id);
            if (obra == null)
            {
                TempData["MensajeError"] = "La obra no existe.";
                return RedirectToPage();
            }

            // 2. Le asignamos el nuevo estado que viene del dropdown
            obra.Estado = nuevoEstado;

            // Si el administrador decide pasarla a Finalizada manualmente desde aquí, forzamos el 100%
            if (nuevoEstado == "Finalizada")
            {
                obra.PorcentajeAvanceActual = 100;
            }

            // 3. Enviamos el ID y la OBRA completa (objeto) a la API
            var guardado = await _obraHttpService.ActualizarObraAsync(id, obra);

            if (!guardado)
            {
                TempData["MensajeError"] = "No se pudo actualizar el estado en el servidor.";
            }

            return RedirectToPage();
        }

    }
}