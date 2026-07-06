namespace PagesObrasApp.Models.DTOs
{
    public class UsuarioPendienteDto
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } = null!;
        public string? NombreEmpleado { get; set; }
        public string? ApellidoEmpleado { get; set; }
        public string Estado { get; set; } = null!;
        public string NombreRol { get; set; } = null!;

        public int? IdEmpleado { get; set; }  // ID del empleado vinculado
        public DateTime FechaReg { get; set; } = DateTime.Now;  // Fecha de registro
    }
}