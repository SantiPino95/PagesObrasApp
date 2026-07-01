using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models;

namespace PagesObrasApp.Pages.Admin
{
    [Authorize(Policy = "SoloAdmin")]
    public class AsignacionEmpleadosModel : PageModel
    {
        public List<AsignacionDto> Asignaciones { get; set; } = new();
        public List<EmpleadoDto> Empleados { get; set; } = new();
        public List<ObraDto> Obras { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public void OnGet()
        {
            // TODO: GET /api/asignaciones (incluye datos de empleado y obra)
            Asignaciones = new List<AsignacionDto>
            {
                new() { IdAsig=1,  IdEmpleado=1, IdObra=1, NombreEmpleado="Rodrigo Méndez",    CedulaEmpleado="4.521.887-3", NombreObra="Edificio Las Acacias",     CodigoObra="OB-2026-014", EstadoObra="En Progreso", Rol="Capataz",  FechaAsig=new DateTime(2026,2,10), ValorHoraAsig=340 },
                new() { IdAsig=2,  IdEmpleado=2, IdObra=1, NombreEmpleado="Sebastián Ferreira", CedulaEmpleado="3.987.654-1", NombreObra="Edificio Las Acacias",     CodigoObra="OB-2026-014", EstadoObra="En Progreso", Rol="Oficial",  FechaAsig=new DateTime(2026,2,10), ValorHoraAsig=280 },
                new() { IdAsig=3,  IdEmpleado=7, IdObra=1, NombreEmpleado="Carlos Espínola",   CedulaEmpleado="4.111.222-6", NombreObra="Edificio Las Acacias",     CodigoObra="OB-2026-014", EstadoObra="En Progreso", Rol="Oficial",  FechaAsig=new DateTime(2026,3,15), ValorHoraAsig=280 },
                new() { IdAsig=4,  IdEmpleado=6, IdObra=1, NombreEmpleado="Pablo Ríos",        CedulaEmpleado="5.654.321-9", NombreObra="Edificio Las Acacias",     CodigoObra="OB-2026-014", EstadoObra="En Progreso", Rol="Ayudante", FechaAsig=new DateTime(2026,4,1),  ValorHoraAsig=200 },
                new() { IdAsig=5,  IdEmpleado=5, IdObra=2, NombreEmpleado="Luciana Torres",    CedulaEmpleado="3.456.789-2", NombreObra="Torre Mirador",            CodigoObra="OB-2026-002", EstadoObra="En Progreso", Rol="Capataz",  FechaAsig=new DateTime(2026,5,2),  ValorHoraAsig=320 },
                new() { IdAsig=6,  IdEmpleado=3, IdObra=2, NombreEmpleado="Marcelo Suárez",    CedulaEmpleado="5.123.456-7", NombreObra="Torre Mirador",            CodigoObra="OB-2026-002", EstadoObra="En Progreso", Rol="Ayudante", FechaAsig=new DateTime(2026,5,2),  ValorHoraAsig=240 },
                new() { IdAsig=7,  IdEmpleado=4, IdObra=3, NombreEmpleado="Diego Pereira",     CedulaEmpleado="4.789.012-5", NombreObra="Galpón Industrial Ruta 5", CodigoObra="OB-2026-011", EstadoObra="En Progreso", Rol="Oficial",  FechaAsig=new DateTime(2026,4,22), ValorHoraAsig=280 },
                new() { IdAsig=9,  IdEmpleado=7, IdObra=3, NombreEmpleado="Carlos Espínola",   CedulaEmpleado="4.111.222-6", NombreObra="Galpón Industrial Ruta 5", CodigoObra="OB-2026-011", EstadoObra="En Progreso", Rol="Oficial",  FechaAsig=new DateTime(2026,5,10), ValorHoraAsig=280 },
            };

            CargarCombos();
        }

        public IActionResult OnPostCrearAsignacion(
            int idEmpleado, int idObra, string rol,
            decimal valorHoraAsig, DateTime fechaAsig)
        {
            if (idEmpleado == 0 || idObra == 0 || string.IsNullOrWhiteSpace(rol))
            {
                Mensaje = "Completá todos los campos."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: POST /api/asignaciones
            Mensaje = "Asignación creada correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        public IActionResult OnPostQuitarAsignacion(int idAsig)
        {
            if (idAsig == 0)
            {
                Mensaje = "Asignación no encontrada."; MensajeTipo = "error";
                return RedirectToPage();
            }
            // TODO: DELETE /api/asignaciones/{idAsig}
            Mensaje = "Asignación eliminada correctamente."; MensajeTipo = "ok";
            return RedirectToPage();
        }

        private void CargarCombos()
        {
            // TODO: GET /api/empleados
            Empleados = new List<EmpleadoDto>
            {
                new() { Id=1, Nombre="Rodrigo Méndez",    Cedula="4.521.887-3", ValorHora=320 },
                new() { Id=2, Nombre="Sebastián Ferreira", Cedula="3.987.654-1", ValorHora=280 },
                new() { Id=3, Nombre="Marcelo Suárez",     Cedula="5.123.456-7", ValorHora=240 },
                new() { Id=4, Nombre="Diego Pereira",      Cedula="4.789.012-5", ValorHora=280 },
                new() { Id=5, Nombre="Luciana Torres",     Cedula="3.456.789-2", ValorHora=320 },
                new() { Id=6, Nombre="Pablo Ríos",         Cedula="5.654.321-9", ValorHora=200 },
                new() { Id=7, Nombre="Carlos Espínola",    Cedula="4.111.222-6", ValorHora=280 },
            };

            // TODO: GET /api/obras?activa=true
            Obras = new List<ObraDto>
            {
                new() { Id=1, Codigo="OB-2026-014", Nombre="Edificio Las Acacias",     Estado="En Progreso" },
                new() { Id=2, Codigo="OB-2026-002", Nombre="Torre Mirador",            Estado="En Progreso" },
                new() { Id=3, Codigo="OB-2026-011", Nombre="Galpón Industrial Ruta 5", Estado="En Progreso" },
                new() { Id=4, Codigo="OB-2026-019", Nombre="Casa Familia Pérez",       Estado="Planificada" },
                new() { Id=5, Codigo="OB-2025-087", Nombre="Reforma Local Comercial",  Estado="Pausada"     },
            };
        }
    }
}