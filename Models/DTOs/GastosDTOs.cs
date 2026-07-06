namespace PagesObrasApp.Models.DTOs
{
    public class GastoListadoDto
    {
        public int IdGasto { get; set; }
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = null!;
        public string CategoriaGasto { get; set; } = null!;
        public string? NroComprobante { get; set; }
    }

    public class CrearGastoDto
    {
        public int IdObra { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = null!;
        public string CategoriaGasto { get; set; } = null!;
        public string? NroComprobante { get; set; }
    }

    public class ActualizarGastoDto
    {
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = null!;
        public string CategoriaGasto { get; set; } = null!;
        public string? NroComprobante { get; set; }
    }
}