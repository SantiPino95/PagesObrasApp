using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagesObrasApp.Models.DTOs;
using PagesObrasApp.Services;
using System.Globalization;

namespace PagesObrasApp.Pages.Empleado
{
    [Authorize(Policy = "Personal")]
    public class RegistroHorasModel : PageModel
    {
        private readonly IApiService _api;
        private readonly IEmpleadoHttpService _empleadoHttpService;

        public RegistroHorasModel(IApiService api, IEmpleadoHttpService empleadoHttpService)
        {
            _api = api;
            _empleadoHttpService = empleadoHttpService;
        }

        // ── Datos del empleado logueado ──
        public int IdEmpleado { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;

        // ── Lista de asignaciones activas del empleado ──
        public List<EmpleadoObraDTOs> Asignaciones { get; set; } = new();

        // ── Registros del mes actual ──
        public List<RegistroHoraDto> RegistrosMes { get; set; } = new();

        // ── Resumen del mes ──
        public decimal TotalHorasMes { get; set; }
        public decimal TotalHorasComunes { get; set; }
        public decimal TotalHorasExtras { get; set; }
        public int DiasTrabajados { get; set; }

        // ── Fechas ──
        public DateTime Hoy { get; set; } = DateTime.Today;
        public DateTime InicioMes { get; set; }
        public DateTime FinMes { get; set; }

        // ── Registro de hoy (si existe) ──
        public RegistroHoraDto? RegistroHoy { get; set; }

        // ── Mensajes ──
        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? MensajeTipo { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Obtener ID del empleado desde los claims
            var idEmpleadoClaim = User.FindFirst("IdEmpleado")?.Value;
            if (string.IsNullOrEmpty(idEmpleadoClaim) || !int.TryParse(idEmpleadoClaim, out int idEmpleado))
            {
                // Si no tiene IdEmpleado, es un Administrador o usuario sin empleado vinculado
                // Redirigir al dashboard correspondiente
                if (User.IsInRole("Administrador"))
                    return RedirectToPage("/Admin/Index");
                else
                    return RedirectToPage("/Index");
            }

            IdEmpleado = idEmpleado;
            NombreCompleto = User.Identity?.Name ?? "Empleado";

            // Obtener asignaciones activas del empleado
            var todasAsignaciones = await _empleadoHttpService.ObtenerAsignacionesAsync() ?? new();
            Asignaciones = todasAsignaciones.Where(a => a.IdEmpleado == IdEmpleado).ToList();

            // Obtener registros del mes actual
            InicioMes = new DateTime(Hoy.Year, Hoy.Month, 1);
            FinMes = InicioMes.AddMonths(1).AddDays(-1);

            // TODO: Conectar con el endpoint real de la API cuando exista
            // Por ahora, usamos datos mock para la estructura
            RegistrosMes = await ObtenerRegistrosMockAsync(IdEmpleado);

            // Calcular resumen
            TotalHorasMes = RegistrosMes.Sum(r => r.HorasComunes + r.HorasExtras);
            TotalHorasComunes = RegistrosMes.Sum(r => r.HorasComunes);
            TotalHorasExtras = RegistrosMes.Sum(r => r.HorasExtras);
            DiasTrabajados = RegistrosMes.Select(r => r.Fecha.Date).Distinct().Count();

            // Buscar registro de hoy
            RegistroHoy = RegistrosMes.FirstOrDefault(r => r.Fecha.Date == Hoy.Date);

            return Page();
        }

        // ── MOCK: Registros del mes ──
        private async Task<List<RegistroHoraDto>> ObtenerRegistrosMockAsync(int idEmpleado)
        {
            // Simular algunos registros del mes actual
            var registros = new List<RegistroHoraDto>();
            var random = new Random();

            for (int i = 1; i <= 15; i++)
            {
                var fecha = new DateTime(Hoy.Year, Hoy.Month, i);
                if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                if (fecha > Hoy)
                    break;

                var tieneRegistro = random.Next(0, 100) > 15; // 85% de días trabajados
                if (!tieneRegistro)
                    continue;

                var horasComunes = 8m;
                var horasExtras = random.Next(0, 100) > 70 ? Math.Round(random.Next(1, 4) * 0.5m, 1) : 0;

                registros.Add(new RegistroHoraDto
                {
                    IdRegistro = i,
                    IdEmpleadoObra = idEmpleado,
                    Fecha = fecha,
                    HorasComunes = horasComunes,
                    HorasExtras = horasExtras,
                    ObservacionesEmpleado = horasExtras > 0 ? "Horas extra por necesidad de obra" : null
                });
            }

            return registros;
        }

        // ── POST: Cargar horas ──
        public async Task<IActionResult> OnPostCargarHorasAsync(
            int idEmpleadoObra,
            decimal horasComunes,
            decimal horasExtras,
            string? observaciones)
        {
            if (idEmpleadoObra == 0)
            {
                Mensaje = "Seleccioná una obra válida.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            if (horasComunes <= 0 && horasExtras <= 0)
            {
                Mensaje = "Debés cargar al menos una hora.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            // TODO: Conectar con el endpoint POST /api/RegistroHoras cuando exista
            var dto = new CrearRegistroHoraDto
            {
                IdEmpleadoObra = idEmpleadoObra,
                Fecha = Hoy,
                HorasComunes = horasComunes,
                HorasExtras = horasExtras,
                ObservacionesEmpleado = observaciones
            };

            // Simular éxito
            var success = true;

            if (success)
            {
                Mensaje = "Horas cargadas correctamente.";
                MensajeTipo = "ok";
            }
            else
            {
                Mensaje = "No se pudieron cargar las horas.";
                MensajeTipo = "error";
            }

            return RedirectToPage();
        }

        // ── POST: Editar horas de hoy ──
        public async Task<IActionResult> OnPostEditarHorasAsync(
            int idRegistro,
            int idEmpleadoObra,
            decimal horasComunes,
            decimal horasExtras,
            string? observaciones)
        {
            if (idRegistro == 0 || idEmpleadoObra == 0)
            {
                Mensaje = "Datos inválidos.";
                MensajeTipo = "error";
                return RedirectToPage();
            }

            // TODO: Conectar con el endpoint PUT /api/RegistroHoras/{id} cuando exista
            var success = true;

            if (success)
            {
                Mensaje = "Registro actualizado correctamente.";
                MensajeTipo = "ok";
            }
            else
            {
                Mensaje = "No se pudo actualizar el registro.";
                MensajeTipo = "error";
            }

            return RedirectToPage();
        }
    }

    // ── DTOs para RegistroHoras ──
    public class RegistroHoraDto
    {
        public int IdRegistro { get; set; }
        public int IdEmpleadoObra { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasComunes { get; set; }
        public decimal HorasExtras { get; set; }
        public string? ObservacionesEmpleado { get; set; }
    }

    public class CrearRegistroHoraDto
    {
        public int IdEmpleadoObra { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasComunes { get; set; }
        public decimal HorasExtras { get; set; }
        public string? ObservacionesEmpleado { get; set; }
    }
}