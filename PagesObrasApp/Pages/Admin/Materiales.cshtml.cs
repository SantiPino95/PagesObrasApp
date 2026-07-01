using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class MaterialesModel : PageModel
    {
        public List<MaterialDto> Materiales { get; set; } = new();
        public List<ConsumoDto> Consumos { get; set; } = new();
        public List<ProveedorDto> Proveedores { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet()
        {
            // TODO: GET /api/materiales (incluye stock)
            Materiales = new List<MaterialDto>
            {
                new() { Id=1, Nombre="Cemento Portland",    Unidad="kg",     Disponible=2400, Minimo=500  },
                new() { Id=2, Nombre="Arena fina",          Unidad="m³",     Disponible=18,   Minimo=10   },
                new() { Id=3, Nombre="Ladrillo común",      Unidad="unidad", Disponible=1200, Minimo=2000 },
                new() { Id=4, Nombre="Hierro Ø10mm",        Unidad="kg",     Disponible=850,  Minimo=300  },
                new() { Id=5, Nombre="Hierro Ø8mm",         Unidad="kg",     Disponible=120,  Minimo=200  },
                new() { Id=6, Nombre="Pintura látex blanca",Unidad="litro",  Disponible=45,   Minimo=20   },
                new() { Id=7, Nombre="Piso cerámico 45×45", Unidad="m²",     Disponible=0,    Minimo=50   },
                new() { Id=8, Nombre="Cal",                 Unidad="kg",     Disponible=600,  Minimo=150  },
            };

            // TODO: GET /api/materiales/consumos
            Consumos = new List<ConsumoDto>
            {
                new() { IdMaterial=1, Obra="Edificio Las Acacias",     Cantidad=1800, Fecha=new DateTime(2026,6,10) },
                new() { IdMaterial=1, Obra="Torre Mirador",            Cantidad=420,  Fecha=new DateTime(2026,6,15) },
                new() { IdMaterial=2, Obra="Edificio Las Acacias",     Cantidad=12,   Fecha=new DateTime(2026,6,8)  },
                new() { IdMaterial=2, Obra="Galpón Industrial Ruta 5", Cantidad=5,    Fecha=new DateTime(2026,6,12) },
                new() { IdMaterial=4, Obra="Torre Mirador",            Cantidad=320,  Fecha=new DateTime(2026,6,14) },
                new() { IdMaterial=5, Obra="Galpón Industrial Ruta 5", Cantidad=280,  Fecha=new DateTime(2026,6,11) },
            };

            // TODO: GET /api/proveedores (para modal entrada stock)
            Proveedores = new List<ProveedorDto>
            {
                new() { Id=1, Nombre="Materiales del Norte S.A." },
                new() { Id=2, Nombre="Distribuidora Ferpal"      },
                new() { Id=3, Nombre="Hierros Uruguay S.A."      },
            };
        }

        public IActionResult OnPostCrearMaterial(
            string nombre, string unidad, decimal stockMinimo, decimal stockInicial)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(unidad))
            {
                Mensaje = "Nombre y unidad son obligatorios."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/materiales → inserta en Material y Stock_Material
            Mensaje = $"Material \"{nombre}\" creado correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        public IActionResult OnPostEntradaStock(
            int idMaterial, decimal cantidad, DateTime fecha,
            int? idProveedor, string? nroComprobante)
        {
            if (idMaterial == 0 || cantidad <= 0)
            {
                Mensaje = "Seleccioná material e ingresá una cantidad válida."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/stock/entrada
            // La API suma cantidad a Stock_Material.Cantidad_Disponible
            Mensaje = "Entrada de stock registrada correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }
    }
}