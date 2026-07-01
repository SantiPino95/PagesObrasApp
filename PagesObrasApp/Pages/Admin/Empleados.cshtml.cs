using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class EmpleadosModel : PageModel
    {
        public List<EmpleadoDto> Empleados { get; set; } = new();
        public List<AsignacionDto> Asignaciones { get; set; } = new();
        public List<ObraDto> Obras { get; set; } = new();

        // Período seleccionado para horas: "hoy" | "sem" | "mes"
        public string Periodo { get; set; } = "hoy";

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet(string periodo = "hoy")
        {
            Periodo = periodo;

            // TODO: GET /api/empleados
            Empleados = new List<EmpleadoDto>
            {
                new() { Id=1, Nombre="Rodrigo Méndez",    Cedula="4.521.887-3", Telefono="099 112 233", ValorHora=320 },
                new() { Id=2, Nombre="Sebastián Ferreira", Cedula="3.987.654-1", Telefono="098 445 566", ValorHora=280 },
                new() { Id=3, Nombre="Marcelo Suárez",     Cedula="5.123.456-7", Telefono="091 778 899", ValorHora=240 },
                new() { Id=4, Nombre="Diego Pereira",      Cedula="4.789.012-5", Telefono="097 334 455", ValorHora=280 },
                new() { Id=5, Nombre="Luciana Torres",     Cedula="3.456.789-2", Telefono="092 667 788", ValorHora=320 },
                new() { Id=6, Nombre="Pablo Ríos",         Cedula="5.654.321-9", Telefono="094 223 344", ValorHora=200 },
                new() { Id=7, Nombre="Carlos Espínola",    Cedula="4.111.222-6", Telefono="096 889 900", ValorHora=280 },
            };

            // TODO: GET /api/asignaciones
            Asignaciones = new List<AsignacionDto>
            {
                new() { IdAsig=1, IdEmpleado=1, IdObra=1, NombreObra="Edificio Las Acacias",     CodigoObra="OB-2026-014", Rol="Capataz",  FechaAsig=new DateTime(2026,2,10), ValorHoraAsig=340 },
                new() { IdAsig=2, IdEmpleado=2, IdObra=1, NombreObra="Edificio Las Acacias",     CodigoObra="OB-2026-014", Rol="Oficial",  FechaAsig=new DateTime(2026,2,10), ValorHoraAsig=280 },
                new() { IdAsig=5, IdEmpleado=5, IdObra=2, NombreObra="Torre Mirador",            CodigoObra="OB-2026-002", Rol="Capataz",  FechaAsig=new DateTime(2026,5,2),  ValorHoraAsig=320 },
                new() { IdAsig=7, IdEmpleado=4, IdObra=3, NombreObra="Galpón Industrial Ruta 5", CodigoObra="OB-2026-011", Rol="Oficial",  FechaAsig=new DateTime(2026,4,22), ValorHoraAsig=280 },
                new() { IdAsig=9, IdEmpleado=7, IdObra=3, NombreObra="Galpón Industrial Ruta 5", CodigoObra="OB-2026-011", Rol="Oficial",  FechaAsig=new DateTime(2026,5,10), ValorHoraAsig=280 },
            };

            // TODO: GET /api/obras (para el select del modal asignar)
            Obras = new List<ObraDto>
            {
                new() { Id=1, Codigo="OB-2026-014", Nombre="Edificio Las Acacias"     },
                new() { Id=2, Codigo="OB-2026-002", Nombre="Torre Mirador"            },
                new() { Id=3, Codigo="OB-2026-011", Nombre="Galpón Industrial Ruta 5" },
                new() { Id=4, Codigo="OB-2026-019", Nombre="Casa Familia Pérez"       },
            };
        }

        public IActionResult OnPostCrearEmpleado(
            string nombre, string cedula, string telefono,
            decimal valorHora, int? idObra, string? rolObra)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(cedula) || valorHora <= 0)
            {
                Mensaje = "Nombre, cédula y valor hora son obligatorios."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/empleados → si viene idObra, también POST /api/asignaciones
            Mensaje = $"Empleado \"{nombre}\" dado de alta correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        public IActionResult OnPostAsignarEmpleado(
            int idEmpleado, int idObra, string rol,
            decimal valorHoraAsig, DateTime fechaAsig)
        {
            if (idEmpleado == 0 || idObra == 0)
            {
                Mensaje = "Seleccioná empleado y obra."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/asignaciones
            Mensaje = "Asignación creada correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }
    }
}