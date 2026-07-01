using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class OrdenesCompraModel : PageModel
    {
        public List<OrdenCompraDto> Ordenes { get; set; } = new();
        public List<ProveedorDto> Proveedores { get; set; } = new();
        public List<MaterialDto> Materiales { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet()
        {
            // TODO: GET /api/ordenes-compra (incluye detalles y stock actual)
            Ordenes = new List<OrdenCompraDto>
            {
                new() { Id=1, IdProv=1, Proveedor="Materiales del Norte S.A.", FechaPedido=new DateTime(2026,6,5),  MontoTotal=78000,  Estado="Pendiente",
                    Items = new() {
                        new() { IdMat=1, Material="Cemento Portland", Unidad="kg", Cant=2000, PU=25,   Sub=50000, StockActual=2400 },
                        new() { IdMat=2, Material="Arena fina",       Unidad="m³", Cant=15,   PU=1200, Sub=18000, StockActual=18   },
                    }},
                new() { Id=2, IdProv=3, Proveedor="Hierros Uruguay S.A.",      FechaPedido=new DateTime(2026,5,20), MontoTotal=196000, Estado="Entregado",
                    Items = new() {
                        new() { IdMat=4, Material="Hierro Ø10mm", Unidad="kg", Cant=500, PU=280, Sub=140000, StockActual=850 },
                        new() { IdMat=5, Material="Hierro Ø8mm",  Unidad="kg", Cant=300, PU=220, Sub=66000,  StockActual=120 },
                    }},
                new() { Id=3, IdProv=2, Proveedor="Distribuidora Ferpal",      FechaPedido=new DateTime(2026,5,28), MontoTotal=52500,  Estado="Parcial",
                    Items = new() {
                        new() { IdMat=3, Material="Ladrillo común", Unidad="unidad", Cant=5000, PU=8,  Sub=40000, StockActual=1200 },
                        new() { IdMat=8, Material="Cal",            Unidad="kg",     Cant=500,  PU=21, Sub=10500, StockActual=600  },
                    }},
                new() { Id=4, IdProv=1, Proveedor="Materiales del Norte S.A.", FechaPedido=new DateTime(2026,6,15), MontoTotal=63600,  Estado="Pendiente",
                    Items = new() {
                        new() { IdMat=7, Material="Piso cerámico 45×45",  Unidad="m²",    Cant=120, PU=480, Sub=57600, StockActual=0   },
                        new() { IdMat=6, Material="Pintura látex blanca",  Unidad="litro", Cant=80,  PU=120, Sub=9600,  StockActual=45  },
                    }},
            };

            CargarCombos();
        }

        public IActionResult OnPostCrearOrden(
            int idProveedor, DateTime fechaPedido,
            int[] idMateriales, decimal[] cantidades, decimal[] precios)
        {
            if (idProveedor == 0 || idMateriales.Length == 0)
            {
                Mensaje = "Seleccioná proveedor y al menos un material."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/ordenes-compra
            // Body: { idProveedor, fechaPedido, detalles: [{ idMaterial, cantidadPedida, precioUnitarioCompra }] }
            // La API calcula automáticamente el Monto_Total
            Mensaje = "Orden de compra creada correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        public IActionResult OnPostConfirmarEntrega(int id)
        {
            if (id == 0)
            {
                Mensaje = "Orden no encontrada."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: PATCH /api/ordenes-compra/{id}/confirmar-entrega
            // La API en un solo endpoint:
            //   1. Cambia Orden_Compra.Estado_Entrega = "Entregado"
            //   2. Para cada detalle: suma Cantidad_Pedida a Stock_Material.Cantidad_Disponible
            Mensaje = $"Entrega confirmada. Stock actualizado."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        private void CargarCombos()
        {
            // TODO: GET /api/proveedores
            Proveedores = new List<ProveedorDto>
            {
                new() { Id=1, Nombre="Materiales del Norte S.A." },
                new() { Id=2, Nombre="Distribuidora Ferpal"      },
                new() { Id=3, Nombre="Hierros Uruguay S.A."      },
                new() { Id=4, Nombre="Pinturas Rex"              },
            };

            // TODO: GET /api/materiales
            Materiales = new List<MaterialDto>
            {
                new() { Id=1, Nombre="Cemento Portland",    Unidad="kg"     },
                new() { Id=2, Nombre="Arena fina",          Unidad="m³"     },
                new() { Id=3, Nombre="Ladrillo común",      Unidad="unidad" },
                new() { Id=4, Nombre="Hierro Ø10mm",        Unidad="kg"     },
                new() { Id=5, Nombre="Hierro Ø8mm",         Unidad="kg"     },
                new() { Id=6, Nombre="Pintura látex blanca",Unidad="litro"  },
                new() { Id=7, Nombre="Piso cerámico 45×45", Unidad="m²"     },
                new() { Id=8, Nombre="Cal",                 Unidad="kg"     },
            };
        }
    }
}