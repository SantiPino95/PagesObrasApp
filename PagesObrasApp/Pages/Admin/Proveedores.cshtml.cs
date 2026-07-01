using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class ProveedoresModel : PageModel
    {
        public List<ProveedorDto> Proveedores { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet()
        {
            // TODO: GET /api/proveedores (incluye conteo de órdenes y monto total)
            Proveedores = new List<ProveedorDto>
            {
                new() { Id=1, Nombre="Materiales del Norte S.A.", RUT="21-345678-0001", Tel="02 401 2233", Email="ventas@matdelnorte.com.uy",     Ordenes=6, MontoTotal=142000 },
                new() { Id=2, Nombre="Distribuidora Ferpal",      RUT="21-112233-0002", Tel="02 508 4455", Email="ferpal@distribuidora.uy",        Ordenes=4, MontoTotal=98500  },
                new() { Id=3, Nombre="Hierros Uruguay S.A.",      RUT="21-667788-0003", Tel="099 345 678", Email="contacto@hierrosuruguay.com.uy", Ordenes=3, MontoTotal=75200  },
                new() { Id=4, Nombre="Pinturas Rex",              RUT="21-223344-0004", Tel="098 112 233", Email="rex@pinturas.com.uy",            Ordenes=2, MontoTotal=18400  },
                new() { Id=5, Nombre="Alquileres Técnicos SRL",   RUT="21-556677-0005", Tel="02 602 9988", Email="info@alquilertech.uy",           Ordenes=5, MontoTotal=61800  },
            };
        }

        public IActionResult OnPostCrearProveedor(
            string nombre, string rut, string tel, string email)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(rut))
            {
                Mensaje = "Nombre y RUT son obligatorios."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/proveedores
            Mensaje = $"Proveedor \"{nombre}\" creado correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        public IActionResult OnPostEditarProveedor(
            int id, string nombre, string rut, string tel, string email)
        {
            if (id == 0 || string.IsNullOrWhiteSpace(nombre))
            {
                Mensaje = "Datos inválidos."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: PUT /api/proveedores/{id}
            Mensaje = "Proveedor actualizado correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }
    }
}