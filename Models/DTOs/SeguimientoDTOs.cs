namespace PagesObrasApp.Models.DTOs
{
    public class SeguimientoListadoDto
    {
        public int IdSeguimiento { get; set; }
        public int IdObra { get; set; }
        public string? NombreObra { get; set; }
        public DateTime Fecha { get; set; }
        public string DescripcionAvance { get; set; } = null!;
        public int PorcentajeAvance { get; set; }
        public string? ImgProgreso { get; set; }
    }

    public class CrearSeguimientoDto
    {
        public int IdObra { get; set; }
        public DateTime Fecha { get; set; }
        public string DescripcionAvance { get; set; } = null!;
        public int PorcentajeAvance { get; set; }
        public string? ImgProgreso { get; set; }
    }
}