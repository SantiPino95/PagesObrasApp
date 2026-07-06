namespace PagesObrasApp.Models.DTOs
{
    public class PagoProveedorListadoDto
    {
        public int IdPagoProveedor { get; set; }
        public int IdProveedor { get; set; }
        public string? NombreProveedor { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = null!;
    }

    public class CrearPagoProveedorDto
    {
        public int IdProveedor { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = null!;
    }
}