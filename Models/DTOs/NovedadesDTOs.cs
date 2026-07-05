namespace PagesObrasApp.Models.DTOs
{
    public class NovedadListadoDto
    {
        public int IdNovedad { get; set; }
        public int IdEmpleadoObra { get; set; }
        public string? NombreEmpleado { get; set; }
        public int IdObra { get; set; }
        public string? NombreObra { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoNovedad { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string? EstadoRevision { get; set; }
    }

    public class CrearNovedadDto
    {
        public int IdEmpleadoObra { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoNovedad { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string? EstadoRevision { get; set; } = "Pendiente";
    }
}