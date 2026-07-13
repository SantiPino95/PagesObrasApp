namespace PagesObrasApp.Models.DTOs
{
    public class RegistroHoraDto
    {
        public int IdRegistro { get; set; }
        public int IdEmpleadoObra { get; set; }
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = string.Empty;
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

    public class ActualizarRegistroHoraDto
    {
        public decimal HorasComunes { get; set; }
        public decimal HorasExtras { get; set; }
        public string? ObservacionesEmpleado { get; set; }
    }
}