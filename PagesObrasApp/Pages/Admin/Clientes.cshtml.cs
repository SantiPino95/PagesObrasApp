using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class ClientesModel : PageModel
    {
        public List<ClienteDto> Clientes { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet()
        {
            // TODO: GET /api/clientes (incluye lista de obras por cliente)
            Clientes = new List<ClienteDto>
            {
                new() { Id=1, Nombre="Inmobiliaria Sur S.A.",  Tel="02 900 1122", Email="contacto@inmsur.com.uy",  Direccion="Av. 18 de Julio 1234",
                    Obras = new() {
                        new() { Codigo="OB-2026-014", Nombre="Edificio Las Acacias", Estado="En Progreso" },
                        new() { Codigo="OB-2026-002", Nombre="Torre Mirador",        Estado="En Progreso" },
                    }},
                new() { Id=2, Nombre="Marcos Pérez",           Tel="099 234 567", Email="marcos.perez@gmail.com",  Direccion="Calle Artigas 456, Paysandú",
                    Obras = new() { new() { Codigo="OB-2026-019", Nombre="Casa Familia Pérez", Estado="Planificada" } }},
                new() { Id=3, Nombre="Logística del Este",     Tel="02 508 7788", Email="admin@logeste.uy",         Direccion="Ruta 8 km 12, Melo",
                    Obras = new() { new() { Codigo="OB-2026-011", Nombre="Galpón Industrial Ruta 5", Estado="En Progreso" } }},
                new() { Id=4, Nombre="Comercial Andina",       Tel="098 445 001", Email="andina@comercial.com.uy", Direccion="Bulevar Artigas 890",
                    Obras = new() { new() { Codigo="OB-2025-087", Nombre="Reforma Local Comercial", Estado="Pausada" } }},
                new() { Id=5, Nombre="Lucía Fernández",        Tel="091 667 334", Email="lufer@hotmail.com",        Direccion="Calle Rivera 21, Colonia",
                    Obras = new() { new() { Codigo="OB-2025-072", Nombre="Vivienda Costa Azul", Estado="Finalizada" } }},
            };
        }

        public IActionResult OnPostCrearCliente(
            string nombre, string tel, string email, string direccion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Mensaje = "El nombre es obligatorio."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/clientes
            Mensaje = $"Cliente \"{nombre}\" creado correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        public IActionResult OnPostEditarCliente(
            int id, string nombre, string tel, string email, string direccion)
        {
            if (id == 0 || string.IsNullOrWhiteSpace(nombre))
            {
                Mensaje = "Datos inválidos."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: PUT /api/clientes/{id}
            Mensaje = $"Cliente actualizado correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }
    }
}