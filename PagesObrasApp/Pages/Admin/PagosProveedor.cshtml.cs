using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class PagoProveedorModel : PageModel
    {
        public List<PagoProveedorDto> Pagos { get; set; } = new();
        public List<ProveedorDto> Proveedores { get; set; } = new();

        public static readonly string[] Metodos =
            { "Transferencia", "Cheque", "Efectivo", "Débito" };

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet()
        {
            // TODO: GET /api/pagos-proveedor
            Pagos = new List<PagoProveedorDto>
            {
                new() { Id=1,  IdProv=1, Proveedor="Materiales del Norte S.A.", Fecha=new DateTime(2026,4,15), Monto=45000, Metodo="Transferencia" },
                new() { Id=2,  IdProv=3, Proveedor="Hierros Uruguay S.A.",      Fecha=new DateTime(2026,4,28), Monto=98000, Metodo="Transferencia" },
                new() { Id=3,  IdProv=2, Proveedor="Distribuidora Ferpal",      Fecha=new DateTime(2026,5,10), Monto=28500, Metodo="Cheque"        },
                new() { Id=4,  IdProv=5, Proveedor="Alquileres Técnicos SRL",   Fecha=new DateTime(2026,5,20), Monto=18000, Metodo="Transferencia" },
                new() { Id=5,  IdProv=1, Proveedor="Materiales del Norte S.A.", Fecha=new DateTime(2026,5,28), Monto=32000, Metodo="Transferencia" },
                new() { Id=6,  IdProv=3, Proveedor="Hierros Uruguay S.A.",      Fecha=new DateTime(2026,6,5),  Monto=98000, Metodo="Transferencia" },
                new() { Id=7,  IdProv=2, Proveedor="Distribuidora Ferpal",      Fecha=new DateTime(2026,6,10), Monto=24000, Metodo="Débito"        },
                new() { Id=8,  IdProv=4, Proveedor="Pinturas Rex",              Fecha=new DateTime(2026,6,18), Monto=9800,  Metodo="Efectivo"      },
            };

            // TODO: GET /api/proveedores
            Proveedores = new List<ProveedorDto>
            {
                new() { Id=1, Nombre="Materiales del Norte S.A." },
                new() { Id=2, Nombre="Distribuidora Ferpal"      },
                new() { Id=3, Nombre="Hierros Uruguay S.A."      },
                new() { Id=4, Nombre="Pinturas Rex"              },
                new() { Id=5, Nombre="Alquileres Técnicos SRL"   },
            };
        }

        public IActionResult OnPostRegistrarPago(
            int idProveedor, DateTime fechaPago,
            string metodoPago, decimal monto)
        {
            if (idProveedor == 0 || monto <= 0)
            {
                Mensaje = "Seleccioná proveedor e ingresá un monto válido."; MensajeTipo = "error";
                return RedirectToPage();
            }
            if (!Metodos.Contains(metodoPago))
            {
                Mensaje = "Método de pago inválido."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/pagos-proveedor
            Mensaje = $"Pago de ${monto:N0} registrado correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }
    }
}