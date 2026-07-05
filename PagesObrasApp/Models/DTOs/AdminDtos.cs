// Models/ApiDtos.cs
// DTOs del frontend alineados con los DTOs reales del backend (MiWebApi)
// Cada sección corresponde a un archivo DTO del equipo de backend.
// Para entidades sin DTO confirmado (Gastos, Novedades, etc.)
// se usan estructuras razonables hasta que el backend las provea.

namespace PagesObrasApp.Models
{
    // ═══════════════════════════════════════════════════════════════
    // CLIENTES  ←  CLientesDTOs.cs
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/clientes — listado completo con obras.
    /// Mapeado de CLientesListadoDTOs del backend.
    /// </summary>
    public class ClienteListadoDto
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = "";
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }

        // Lista resumida de obras del cliente
        public List<ObraResumenClienteDto> Obras { get; set; } = new();
    }

    /// <summary>
    /// Obra resumida que viene dentro del listado de clientes.
    /// Mapeado de ObraResumenDto del backend.
    /// </summary>
    public class ObraResumenClienteDto
    {
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = "";
    }

    /// <summary>
    /// Body para POST /api/clientes.
    /// Mapeado de CrearClienteDTOs del backend.
    /// </summary>
    public class CrearClienteDto
    {
        public string Nombre { get; set; } = "";   // obligatorio
        public string Direccion { get; set; } = "";   // obligatorio
        public string? Telefono { get; set; }
        public string Email { get; set; } = "";   // obligatorio
    }


    // ═══════════════════════════════════════════════════════════════
    // EMPLEADOS  ←  EmpleadoDTOs.cs
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/empleados — listado para admin.
    /// Mapeado de EmpleadoListadoDTOs del backend.
    /// Ojo: el backend unifica Nombre + Apellido en NombreCompleto.
    /// </summary>
    public class EmpleadoListadoDto
    {
        public int IdEmpleado { get; set; }
        public string NombreCompleto { get; set; } = "";  // "Rodrigo Méndez"
        public string Categoria { get; set; } = "";  // "Oficial", "Ayudante", etc.
        public string? Telefono { get; set; }
    }

    /// <summary>
    /// Empleado con datos de obra — para AsignacionEmpleados y Empleados por obra.
    /// Mapeado de EmpleadoObraDTOs del backend.
    /// </summary>
    public class EmpleadoObraDto
    {
        public int IdEmpleado { get; set; }
        public string NombreCompleto { get; set; } = "";
        public string Categoria { get; set; } = "";
        public string? Telefono { get; set; }
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = "";
    }

    /// <summary>
    /// Body para POST /api/empleados.
    /// Mapeado de CrearEmpleadoDTOs del backend.
    /// Ojo: el backend espera Nombre y Apellido por separado.
    /// </summary>
    public class CrearEmpleadoDto
    {
        public string Nombre { get; set; } = "";   // obligatorio
        public string Apellido { get; set; } = "";   // obligatorio
        public string Categoria { get; set; } = "";   // obligatorio
        public string? Telefono { get; set; }
    }
    /// <summary>
    /// Body para POST /api/Empleado/asignar.
    /// Ojo: la ruta NO lleva {id} — el idEmpleado va dentro del body, junto con idObra, rolEnObra y valorHoraAsignado.
    /// No existe endpoint para "quitar" asignación todavía.
    /// </summary>
    public class AsignarEmpleadoDto
    {
        public int IdEmpleado { get; set; }
        public int IdObra { get; set; }
        public string RolEnObra { get; set; } = "";
        public decimal ValorHoraAsignado { get; set; }
    }

    // ═══════════════════════════════════════════════════════════════
    // HERRAMIENTAS  ←  HerramientasDTOs.cs
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/herramientas.
    /// Mapeado de HerramientaListadoDto del backend.
    /// Ojo: usa NombreTipo, CodigoInventario, EstadoDisponibilidad (no Estado).
    /// </summary>
    public class HerramientaListadoDto
    {
        public int IdHerramienta { get; set; }
        public string NombreTipo { get; set; } = "";
        public string CodigoInventario { get; set; } = "";
        public string EstadoDisponibilidad { get; set; } = "En Depósito";
        public string Origen { get; set; } = "";

        // Asignación actual (null si está en depósito)
        public int? IdObraActual { get; set; }
        public string? NombreObraActual { get; set; }
        public DateTime? UltimaFechaSalida { get; set; }
        public DateTime? FechaDevolucion { get; set; }

        // Calculado por el backend — true si salió y no fue devuelta
        public bool EstaAsignadaActualmente { get; set; }
    }

    /// <summary>
    /// Body para POST /api/herramientas.
    /// Mapeado de CrearHerramientaDto del backend.
    /// </summary>
    public class CrearHerramientaDto
    {
        public string NombreTipo { get; set; } = "";
        public string CodigoInventario { get; set; } = "";
        public string EstadoDisponibilidad { get; set; } = "En Depósito";
        public string Origen { get; set; } = "";
        // IdObra no va al crear — la herramienta nace en depósito
    }

    /// <summary>
    /// Body para POST /api/herramientas/{id}/asignar.
    /// No tiene DTO propio en el backend aún — estructura razonable.
    /// </summary>
    public class AsignarHerramientaDto
    {
        public int IdObra { get; set; }
        public DateTime FechaSalida { get; set; }
    }


    // ═══════════════════════════════════════════════════════════════
    // MATERIALES  ←  MaterialesDTOs.cs
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/materiales.
    /// Mapeado de MaterialListadoDto del backend.
    /// Ojo: usa UnidadMedida (no Unidad), CantidadDisponible (no Disponible).
    /// </summary>
    public class MaterialListadoDto
    {
        public int IdMaterial { get; set; }
        public string Nombre { get; set; } = "";
        public string UnidadMedida { get; set; } = "";
        public decimal CantidadDisponible { get; set; }
        public decimal StockMinimo { get; set; }

        // Calculado por el backend
        public bool RequiereReposicion { get; set; }
    }

    /// <summary>
    /// Body para POST /api/materiales.
    /// Mapeado de CrearMaterialDto del backend.
    /// </summary>
    public class CrearMaterialDto
    {
        public string Nombre { get; set; } = "";
        public string UnidadMedida { get; set; } = "";
        public decimal CantidadInicial { get; set; } = 0;
        public decimal StockMinimo { get; set; } = 0;
    }

    /// <summary>
    /// Body para POST /api/stock/entrada.
    /// No tiene DTO propio en el backend aún — estructura razonable.
    /// </summary>
    public class EntradaStockDto
    {
        public int IdMaterial { get; set; }
        public decimal Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public int? IdProveedor { get; set; }
        public string? NroComprobante { get; set; }
    }


    // ═══════════════════════════════════════════════════════════════
    // OBRAS  ←  ObrasDTOs.cs
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/obras — listado para admin.
    /// Mapeado de ObraAdminListadoDto del backend.
    /// Ojo: usa NombreObra, NombreCliente, TotalGastado, PorcentajeAvanceActual.
    /// </summary>
    public class ObraListadoDto
    {
        public int IdObra { get; set; }
        public string CodigoFormateado { get; set; } = "";   // "OB-2026-014"
        public string NombreObra { get; set; } = "";
        public string Direccion { get; set; } = "";
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinPrevista { get; set; }
        public string? Estado { get; set; }
        public decimal TotalGastado { get; set; }
        public int PorcentajeAvanceActual { get; set; }

        // Solo los campos de cliente que necesita el listado
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = "";
    }

    /// <summary>
    /// Body para POST /api/obras.
    /// Mapeado de CrearObraDto del backend.
    /// </summary>
    public class CrearObraDto
    {
        public int IdCliente { get; set; }
        public string NombreObra { get; set; } = "";
        public string Direccion { get; set; } = "";
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinPrevista { get; set; }
        // Estado no va al crear — la API lo inicializa en "Planificada"
    }

    /// <summary>
    /// Detalle completo de una obra — para página de detalle.
    /// Mapeado de ObraDetalleDto del backend.
    /// Reutiliza EmpleadoListadoDto para los empleados asignados.
    /// </summary>
    public class ObraDetalleDto
    {
        public int IdObra { get; set; }
        public string CodigoFormateado { get; set; } = "";
        public string NombreObra { get; set; } = "";
        public string Direccion { get; set; } = "";
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinPrevista { get; set; }
        public string? Estado { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = "";

        // Empleados asignados — reutiliza el mismo DTO de listado
        public List<EmpleadoListadoDto> EmpleadosAsignados { get; set; } = new();
    }


    // ═══════════════════════════════════════════════════════════════
    // PROVEEDORES  ←  ProveedorDTOs.cs
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/proveedores.
    /// Mapeado de ProveedorListadoDto del backend.
    /// Ojo: usa Rut en minúscula (no RUT).
    /// </summary>
    public class ProveedorListadoDto
    {
        public int IdProveedor { get; set; }
        public string Nombre { get; set; } = "";
        public string Rut { get; set; } = "";   // minúscula — así viene del backend
        public string? Telefono { get; set; }
        public string? Email { get; set; }

        // Estos no están en el DTO del backend aún — se agregan cuando
        // el backend provea el endpoint enriquecido con conteo de órdenes
        public int TotalOrdenes { get; set; }
        public decimal TotalCompras { get; set; }
    }

    /// <summary>
    /// Body para POST /api/proveedores.
    /// Mapeado de CrearProveedorDto del backend.
    /// </summary>
    public class CrearProveedorDto
    {
        public string Nombre { get; set; } = "";   // obligatorio
        public string Rut { get; set; } = "";   // obligatorio, minúscula
        public string? Telefono { get; set; }          // obligatorio según el backend
        public string? Email { get; set; }
    }


    // ═══════════════════════════════════════════════════════════════
    // PRESUPUESTOS  ←  PresupuestoDTOs.cs
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/presupuestos — listado para admin.
    /// Mapeado de PresupuestoListadoDto del backend.
    /// Ojo: usa EstadoPresupuesto (no Estado), CantidadItems.
    /// </summary>
    public class PresupuestoListadoDto
    {
        public int IdPresupuesto { get; set; }
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = "";
        public DateTime FechaEmision { get; set; }
        public decimal MontoTotal { get; set; }
        public string? EstadoPresupuesto { get; set; }   // "Pendiente" | "Aprobado" | "Rechazado"
        public int CantidadItems { get; set; }
    }

    /// <summary>
    /// Línea de detalle de un presupuesto.
    /// Mapeado de DetallePresupuestoDto del backend.
    /// Ojo: usa Descripcion (no Desc), PrecioUnitario (no PU).
    /// El Subtotal no viene del backend — lo calcula el frontend:
    /// Subtotal = Cantidad × PrecioUnitario
    /// </summary>
    public class DetallePresupuestoDto
    {
        public string Descripcion { get; set; } = "";
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        // Calculado en el frontend antes de enviar — no viene del backend
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }

    /// <summary>
    /// Body para POST /api/presupuestos.
    /// Mapeado de CrearPresupuestoDto del backend.
    /// Ojo: las líneas van en la propiedad "Lineas" (no "Detalles").
    /// </summary>
    public class CrearPresupuestoDto
    {
        public int IdObra { get; set; }
        public string EstadoPresupuesto { get; set; } = "Pendiente";
        public List<DetallePresupuestoDto> Lineas { get; set; } = new();
    }

    /// <summary>
    /// Body para PATCH /api/presupuestos/{id}/estado.
    /// No tiene DTO propio en el backend aún.
    /// </summary>
    public class CambiarEstadoPresupuestoDto
    {
        public string Estado { get; set; } = "";   // "Aprobado" | "Rechazado"
    }


    // ═══════════════════════════════════════════════════════════════
    // GASTOS  ←  DTO pendiente del backend
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/gastos.
    /// Estructura razonable — confirmar con el backend cuando lo provean.
    /// </summary>
    public class GastoListadoDto
    {
        public int IdGasto { get; set; }
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = "";
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = "";
        public string CategoriaGasto { get; set; } = "";
        public string? NroComprobante { get; set; }
    }

    /// <summary>
    /// Body para POST /api/gastos.
    /// </summary>
    public class CrearGastoDto
    {
        public int IdObra { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = "";
        public string CategoriaGasto { get; set; } = "";
        public string? NroComprobante { get; set; }
    }


    // ═══════════════════════════════════════════════════════════════
    // NOVEDADES  ←  DTO pendiente del backend
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/novedades.
    /// Estructura razonable — confirmar con el backend cuando lo provean.
    /// </summary>
    public class NovedadListadoDto
    {
        public int IdNovedad { get; set; }
        public int IdEmpleadoObra { get; set; }
        public string NombreEmpleado { get; set; } = "";
        public string NombreObra { get; set; } = "";
        public DateTime Fecha { get; set; }
        public string TipoNovedad { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public string EstadoRevision { get; set; } = "Pendiente";
    }

    /// <summary>
    /// Body para PATCH /api/novedades/{id}/revisar.
    /// </summary>
    public class MarcarRevisadaDto
    {
        public string EstadoRevision { get; set; } = "Revisado";
    }


    // ═══════════════════════════════════════════════════════════════
    // SEGUIMIENTO  ←  DTO pendiente del backend
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/seguimiento.
    /// Estructura razonable — confirmar con el backend cuando lo provean.
    /// </summary>
    public class SeguimientoListadoDto
    {
        public int IdSeguimiento { get; set; }
        public int IdObra { get; set; }
        public DateTime Fecha { get; set; }
        public string DescripcionAvance { get; set; } = "";
        public int PorcentajeAvance { get; set; }
        public string? ImgProgreso { get; set; }
    }

    /// <summary>
    /// Body para POST /api/seguimiento.
    /// </summary>
    public class CrearSeguimientoDto
    {
        public int IdObra { get; set; }
        public DateTime Fecha { get; set; }
        public string DescripcionAvance { get; set; } = "";
        public int PorcentajeAvance { get; set; }
        public string? ImgProgreso { get; set; }
    }


    // ═══════════════════════════════════════════════════════════════
    // ÓRDENES DE COMPRA  ←  DTO pendiente del backend
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/ordenes-compra.
    /// Estructura razonable — confirmar con el backend cuando lo provean.
    /// </summary>
    public class OrdenCompraListadoDto
    {
        public int IdOrden { get; set; }
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; } = "";
        public DateTime FechaPedido { get; set; }
        public decimal MontoTotal { get; set; }
        public string EstadoEntrega { get; set; } = "";
        public List<DetalleOrdenDto> Detalles { get; set; } = new();
    }

    public class DetalleOrdenDto
    {
        public int IdMaterial { get; set; }
        public string NombreMaterial { get; set; } = "";
        public string UnidadMedida { get; set; } = "";
        public decimal CantidadPedida { get; set; }
        public decimal PrecioUnitarioCompra { get; set; }
        public decimal StockActual { get; set; }
        public decimal Subtotal => CantidadPedida * PrecioUnitarioCompra;
    }

    public class CrearOrdenCompraDto
    {
        public int IdProveedor { get; set; }
        public DateTime FechaPedido { get; set; }
        public List<DetalleCrearOrdenDto> Detalles { get; set; } = new();
    }

    public class DetalleCrearOrdenDto
    {
        public int IdMaterial { get; set; }
        public decimal CantidadPedida { get; set; }
        public decimal PrecioUnitarioCompra { get; set; }
    }


    // ═══════════════════════════════════════════════════════════════
    // PAGOS A PROVEEDOR  ←  DTO pendiente del backend
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/pagos-proveedor.
    /// </summary>
    public class PagoProveedorListadoDto
    {
        public int IdPagoProveedor { get; set; }
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; } = "";
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = "";
    }

    /// <summary>
    /// Body para POST /api/pagos-proveedor.
    /// </summary>
    public class CrearPagoProveedorDto
    {
        public int IdProveedor { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = "";
    }


    // ═══════════════════════════════════════════════════════════════
    // USUARIOS  ←  DTO pendiente del backend
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Lo que devuelve GET /api/usuarios.
    /// </summary>
    public class UsuarioListadoDto
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } = "";
        public string? Rol { get; set; }          // null si está Pendiente
        public string Estado { get; set; } = "";    // "Pendiente" | "Activo" | "Suspendido"
        public DateTime FechaReg { get; set; }

        // Empleado vinculado (null si es admin puro o aún no se vinculó)
        public int? IdEmpleado { get; set; }
        public string? NombreEmpleado { get; set; }
        public string? CedulaEmpleado { get; set; }
    }

    /// <summary>
    /// Body para PATCH /api/usuarios/{id}/aprobar.
    /// </summary>
    public class AprobarUsuarioDto
    {
        public string Rol { get; set; } = "";
        public int? IdEmpleado { get; set; }
    }

    /// <summary>
    /// Body para PATCH /api/usuarios/{id}/rol.
    /// </summary>
    public class CambiarRolDto
    {
        public string Rol { get; set; } = "";
    }


    // ═══════════════════════════════════════════════════════════════
    // AUTH  ←  DTO pendiente del backend
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Body para POST /api/Auth/login.
    /// Ojo: el backend espera "password", no "contrasena".
    /// </summary>
    public class LoginDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    /// <summary>
    /// Lo que devuelve POST /api/Auth/login si las credenciales son correctas.
    /// TODO: confirmar contra el Swagger real de tu compañero si el login
    /// devuelve IdEmpleado (necesario para la claim "IdEmpleado" en Login.cshtml.cs).
    /// Si no lo devuelve, hay que sacar esa claim del login.
    /// </summary>
    public class LoginResponseDto
    {
        public string Token { get; set; } = "";
        public int IdUsuario { get; set; }
        public string Email { get; set; } = "";
        public string NombreCompleto { get; set; } = "";
        public string Rol { get; set; } = "";
        public DateTime Expira { get; set; }
        public int? IdEmpleado { get; set; }
    }

    /// <summary>
    /// Body para POST /api/Auth/registro.
    /// Ojo: el backend crea Usuario + Empleado juntos, por eso pide todos estos campos.
    /// "IdRol" es obligatorio aunque el registro público quede en Estado="Pendiente";
    /// el admin lo reasigna después vía AprobarUsuarioDto.
    /// </summary>
    public class RegistroDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Cedula { get; set; } = "";
        public string? Telefono { get; set; }
        public decimal ValorHora { get; set; } = 0;
        public string Categoria { get; set; } = "";
        public int IdRol { get; set; }
    }
}