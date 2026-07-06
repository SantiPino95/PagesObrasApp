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

        public int TotalObras { get; set; }
        public int ObrasEnProgreso { get; set; }
        public int ObrasFinalizadas { get; set; }
        public int ObrasPlanificadas { get; set; }
        public decimal TotalGastadoGeneral { get; set; }

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Obras = await _obraHttpService.ObtenerObrasAsync() ?? new();
            Clientes = await _clienteHttpService.ObtenerClientesAsync() ?? new();

            await AutoFinalizarObrasCompletadasAsync();
            CalcularResumen();
        }

        // Si alguna obra llegó al 100% pero su estado no es "Finalizada", se actualiza.
        private async Task AutoFinalizarObrasCompletadasAsync()
        {
            var pendientesDeFinalizar = Obras
                .Where(o => o.PorcentajeAvanceActual >= 100 && o.Estado != "Finalizada")
                .ToList();

            foreach (var obra in pendientesDeFinalizar)
            {
                var actualizado = await _obraHttpService.ActualizarEstadoObraAsync(obra.IdObra, "Finalizada");
                if (actualizado)
                {
                    obra.Estado = "Finalizada"; // reflejar el cambio sin recargar todo
                }
            }
        }

        private void CalcularResumen()
        {
            TotalObras = Obras.Count;
            ObrasEnProgreso = Obras.Count(o => o.Estado == "En Progreso");
            ObrasFinalizadas = Obras.Count(o => o.Estado == "Finalizada");
            ObrasPlanificadas = Obras.Count(o => o.Estado == "Planificada");
            TotalGastadoGeneral = Obras.Sum(o => o.TotalGastado);
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

        public async Task<IActionResult> OnPostEliminarObraAsync(int idObra)
        {
            var eliminado = await _obraHttpService.EliminarObraAsync(idObra);
            Mensaje = eliminado ? "Obra eliminada correctamente." : "No se pudo eliminar la obra.";
            MensajeTipo = eliminado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}