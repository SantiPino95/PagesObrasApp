using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;
using PagesObrasApp.Services;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class ClientesModel : PageModel
    {
        private readonly IClienteHttpService _clienteHttpService;

        public ClientesModel(IClienteHttpService clienteHttpService)
        {
            _clienteHttpService = clienteHttpService;
        }

        public List<ClienteListadoDto> Clientes { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task OnGetAsync()
        {
            Clientes = await _clienteHttpService.ObtenerClientesAsync() ?? new();
        }

        public async Task<IActionResult> OnPostCrearClienteAsync(
            string nombre, string tel, string email, string direccion)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(direccion) || string.IsNullOrWhiteSpace(email))
            {
                Mensaje = "Nombre, dirección y email son obligatorios.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearClienteDto
            {
                Nombre = nombre,
                Telefono = tel,
                Email = email,
                Direccion = direccion
            };

            var creado = await _clienteHttpService.CrearClienteAsync(dto);
            Mensaje = creado ? $"Cliente \"{nombre}\" creado correctamente." : "No se pudo crear el cliente.";
            MensajeTipo = creado ? "ok" : "error";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditarClienteAsync(
            int id, string nombre, string tel, string email, string direccion)
        {
            if (id == 0 || string.IsNullOrWhiteSpace(nombre))
            {
                Mensaje = "Datos inválidos.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            var dto = new CrearClienteDto
            {
                Nombre = nombre,
                Telefono = tel,
                Email = email,
                Direccion = direccion
            };

            var actualizado = await _clienteHttpService.ActualizarClienteAsync(id, dto);
            Mensaje = actualizado ? "Cliente actualizado correctamente." : "No se pudo actualizar el cliente.";
            MensajeTipo = actualizado ? "ok" : "error";
            return RedirectToPage();
        }
    }
}
