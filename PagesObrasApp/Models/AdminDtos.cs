namespace PagesObrasApp.Models
{
    // ── Obras ──────────────────────────────────────────────────────────
    public class ObraDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Cliente { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Estado { get; set; } = "";
        public int Avance { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinPrevista { get; set; }
    }

    // ── Empleados ──────────────────────────────────────────────────────
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Cedula { get; set; } = "";
        public string Telefono { get; set; } = "";
        public decimal ValorHora { get; set; }
        public int? IdUsuario { get; set; }
    }

    public class AsignacionDto
    {
        public int IdAsig { get; set; }
        public int IdEmpleado { get; set; }
        public int IdObra { get; set; }
        public string NombreEmpleado { get; set; } = "";
        public string CedulaEmpleado { get; set; } = "";
        public string NombreObra { get; set; } = "";
        public string CodigoObra { get; set; } = "";
        public string EstadoObra { get; set; } = "";
        public string Rol { get; set; } = "";
        public decimal ValorHoraAsig { get; set; }
        public DateTime FechaAsig { get; set; }
    }

    public class RegistroHorasDto
    {
        public int IdAsig { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasComunes { get; set; }
        public decimal HorasExtras { get; set; }
    }

    // ── Clientes ───────────────────────────────────────────────────────
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Email { get; set; } = "";
        public string Direccion { get; set; } = "";
        public List<ObraResumenDto> Obras { get; set; } = new();
    }

    public class ObraResumenDto
    {
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Estado { get; set; } = "";
    }

    // ── Materiales ─────────────────────────────────────────────────────
    public class MaterialDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Unidad { get; set; } = "";
        public decimal Disponible { get; set; }
        public decimal Minimo { get; set; }
    }

    public class ConsumoDto
    {
        public int IdMaterial { get; set; }
        public string Obra { get; set; } = "";
        public decimal Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }

    // ── Herramientas ───────────────────────────────────────────────────
    public class HerramientaDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string Estado { get; set; } = "";
        public string Origen { get; set; } = "";
        public string? Obra { get; set; }
        public DateTime? FechaSalida { get; set; }
    }

    // ── Proveedores ────────────────────────────────────────────────────
    public class ProveedorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string RUT { get; set; } = "";
        public string Tel { get; set; } = "";
        public string Email { get; set; } = "";
        public int Ordenes { get; set; }
        public decimal MontoTotal { get; set; }
    }

    // ── Gastos ─────────────────────────────────────────────────────────
    public class GastoDto
    {
        public int Id { get; set; }
        public int IdObra { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Desc { get; set; } = "";
        public string Cat { get; set; } = "";
        public string? Comp { get; set; }
    }

    // ── Novedades ──────────────────────────────────────────────────────
    public class NovedadDto
    {
        public int Id { get; set; }
        public string Empleado { get; set; } = "";
        public string Obra { get; set; } = "";
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = "";
        public string Desc { get; set; } = "";
        public string Estado { get; set; } = "";
    }

    // ── Presupuestos ───────────────────────────────────────────────────
    public class PresupuestoDto
    {
        public int Id { get; set; }
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = "";
        public string CodigoObra { get; set; } = "";
        public string Cliente { get; set; } = "";
        public DateTime FechaEmision { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; } = "";
        public List<DetallePresDto> Detalles { get; set; } = new();
    }

    public class DetallePresDto
    {
        public string Desc { get; set; } = "";
        public decimal Cant { get; set; }
        public decimal PU { get; set; }
        public decimal Sub { get; set; }
    }

    // ── Órdenes de compra ──────────────────────────────────────────────
    public class OrdenCompraDto
    {
        public int Id { get; set; }
        public int IdProv { get; set; }
        public string Proveedor { get; set; } = "";
        public DateTime FechaPedido { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; } = "";
        public List<DetalleOrdenDto> Items { get; set; } = new();
    }

    public class DetalleOrdenDto
    {
        public int IdMat { get; set; }
        public string Material { get; set; } = "";
        public string Unidad { get; set; } = "";
        public decimal Cant { get; set; }
        public decimal PU { get; set; }
        public decimal Sub { get; set; }
        public decimal StockActual { get; set; }
    }

    // ── Pagos proveedor ────────────────────────────────────────────────
    public class PagoProveedorDto
    {
        public int Id { get; set; }
        public int IdProv { get; set; }
        public string Proveedor { get; set; } = "";
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Metodo { get; set; } = "";
    }

    // ── Seguimiento ────────────────────────────────────────────────────
    public class SeguimientoDto
    {
        public int Id { get; set; }
        public int IdObra { get; set; }
        public DateTime Fecha { get; set; }
        public int Pct { get; set; }
        public string Desc { get; set; } = "";
        public string? Img { get; set; }
    }

    // ── Usuarios ───────────────────────────────────────────────────────
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string? Rol { get; set; }
        public string Estado { get; set; } = "";
        public DateTime FechaReg { get; set; }
        public string? Empleado { get; set; }
        public string? Cedula { get; set; }
    }
}