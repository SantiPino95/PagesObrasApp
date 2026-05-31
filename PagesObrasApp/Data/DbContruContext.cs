using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PagesObrasApp.Models;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PagesObrasApp.Data;

public partial class DbContruContext : DbContext
{
    public DbContruContext()
    {
    }

    public DbContruContext(DbContextOptions<DbContruContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetalleOrdenCompra> DetalleOrdenCompras { get; set; }

    public virtual DbSet<DetallePresupuesto> DetallePresupuestos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<EmpleadoObra> EmpleadoObras { get; set; }

    public virtual DbSet<GastosObra> GastosObras { get; set; }

    public virtual DbSet<Herramienta> Herramientas { get; set; }

    public virtual DbSet<HerramientasAsignada> HerramientasAsignadas { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialObra> MaterialObras { get; set; }

    public virtual DbSet<NovedadesObra> NovedadesObras { get; set; }

    public virtual DbSet<Obra> Obras { get; set; }

    public virtual DbSet<OrdenCompra> OrdenCompras { get; set; }

    public virtual DbSet<PagoEmpleado> PagoEmpleados { get; set; }

    public virtual DbSet<PagoProveedor> PagoProveedors { get; set; }

    public virtual DbSet<Presupuesto> Presupuestos { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<RegistroHora> RegistroHoras { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<SeguimientoObra> SeguimientoObras { get; set; }

    public virtual DbSet<StockMaterial> StockMaterials { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=dbcontru;uid=root;pwd=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.7.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PRIMARY");

            entity.ToTable("cliente");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");
            entity.Property(e => e.Direccion).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<DetalleOrdenCompra>(entity =>
        {
            entity.HasKey(e => new { e.IdOrden, e.IdMaterial })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("detalle_orden_compra");

            entity.HasIndex(e => e.IdMaterial, "Id_Material");

            entity.Property(e => e.IdOrden).HasColumnName("Id_Orden");
            entity.Property(e => e.IdMaterial).HasColumnName("Id_Material");
            entity.Property(e => e.CantidadPedida)
                .HasPrecision(10, 2)
                .HasColumnName("Cantidad_Pedida");
            entity.Property(e => e.PrecioUnitarioCompra)
                .HasPrecision(10, 2)
                .HasColumnName("Precio_Unitario_Compra");

            entity.HasOne(d => d.IdMaterialNavigation).WithMany(p => p.DetalleOrdenCompras)
                .HasForeignKey(d => d.IdMaterial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("detalle_orden_compra_ibfk_2");

            entity.HasOne(d => d.IdOrdenNavigation).WithMany(p => p.DetalleOrdenCompras)
                .HasForeignKey(d => d.IdOrden)
                .HasConstraintName("detalle_orden_compra_ibfk_1");
        });

        modelBuilder.Entity<DetallePresupuesto>(entity =>
        {
            entity.HasKey(e => e.IdDetallePresupuesto).HasName("PRIMARY");

            entity.ToTable("detalle_presupuesto");

            entity.HasIndex(e => e.IdPresupuesto, "Id_Presupuesto");

            entity.Property(e => e.IdDetallePresupuesto).HasColumnName("Id_Detalle_Presupuesto");
            entity.Property(e => e.Cantidad).HasPrecision(10, 2);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.IdPresupuesto).HasColumnName("Id_Presupuesto");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("Precio_Unitario");
            entity.Property(e => e.Subtotal).HasPrecision(12, 2);

            entity.HasOne(d => d.IdPresupuestoNavigation).WithMany(p => p.DetallePresupuestos)
                .HasForeignKey(d => d.IdPresupuesto)
                .HasConstraintName("detalle_presupuesto_ibfk_1");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PRIMARY");

            entity.ToTable("empleado");

            entity.HasIndex(e => e.Cedula, "Cedula").IsUnique();

            entity.HasIndex(e => e.IdUsuario, "Id_Usuario").IsUnique();

            entity.Property(e => e.IdEmpleado).HasColumnName("Id_Empleado");
            entity.Property(e => e.Cedula).HasMaxLength(20);
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.ValorHora)
                .HasPrecision(10, 2)
                .HasColumnName("Valor_Hora");

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Empleado)
                .HasForeignKey<Empleado>(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("empleado_ibfk_1");
        });

        modelBuilder.Entity<EmpleadoObra>(entity =>
        {
            entity.HasKey(e => e.IdEmpleadoObra).HasName("PRIMARY");

            entity.ToTable("empleado_obra");

            entity.HasIndex(e => e.IdEmpleado, "IDX_EmpleadoObra_Empleado");

            entity.HasIndex(e => e.IdObra, "IDX_EmpleadoObra_Obra");

            entity.HasIndex(e => new { e.IdEmpleado, e.IdObra, e.FechaAsignacion }, "Id_Empleado").IsUnique();

            entity.Property(e => e.IdEmpleadoObra).HasColumnName("Id_Empleado_Obra");
            entity.Property(e => e.FechaAsignacion).HasColumnName("Fecha_Asignacion");
            entity.Property(e => e.IdEmpleado).HasColumnName("Id_Empleado");
            entity.Property(e => e.IdObra).HasColumnName("Id_Obra");
            entity.Property(e => e.RolEnObra)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Oficial'")
                .HasColumnName("Rol_En_Obra");
            entity.Property(e => e.ValorHoraAsignado)
                .HasPrecision(10, 2)
                .HasColumnName("Valor_Hora_Asignado");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.EmpleadoObras)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("empleado_obra_ibfk_1");

            entity.HasOne(d => d.IdObraNavigation).WithMany(p => p.EmpleadoObras)
                .HasForeignKey(d => d.IdObra)
                .HasConstraintName("empleado_obra_ibfk_2");
        });

        modelBuilder.Entity<GastosObra>(entity =>
        {
            entity.HasKey(e => e.IdGasto).HasName("PRIMARY");

            entity.ToTable("gastos_obra");

            entity.HasIndex(e => e.Fecha, "IDX_Gastos_Fecha");

            entity.HasIndex(e => e.IdObra, "IDX_Gastos_Obra");

            entity.Property(e => e.IdGasto).HasColumnName("Id_Gasto");
            entity.Property(e => e.CategoriaGasto)
                .HasMaxLength(50)
                .HasColumnName("Categoria_Gasto");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.IdObra).HasColumnName("Id_Obra");
            entity.Property(e => e.Monto).HasPrecision(10, 2);
            entity.Property(e => e.NroComprobante)
                .HasMaxLength(50)
                .HasColumnName("Nro_Comprobante");

            entity.HasOne(d => d.IdObraNavigation).WithMany(p => p.GastosObras)
                .HasForeignKey(d => d.IdObra)
                .HasConstraintName("gastos_obra_ibfk_1");
        });

        modelBuilder.Entity<Herramienta>(entity =>
        {
            entity.HasKey(e => e.IdHerramienta).HasName("PRIMARY");

            entity.ToTable("herramientas");

            entity.HasIndex(e => e.CodigoInventario, "Codigo_Inventario").IsUnique();

            entity.Property(e => e.IdHerramienta).HasColumnName("Id_Herramienta");
            entity.Property(e => e.CodigoInventario)
                .HasMaxLength(50)
                .HasColumnName("Codigo_Inventario");
            entity.Property(e => e.EstadoDisponibilidad)
                .HasMaxLength(30)
                .HasDefaultValueSql("'En Depósito'")
                .HasColumnName("Estado_Disponibilidad");
            entity.Property(e => e.NombreTipo)
                .HasMaxLength(100)
                .HasColumnName("Nombre_Tipo");
            entity.Property(e => e.Origen)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Propia'");
        });

        modelBuilder.Entity<HerramientasAsignada>(entity =>
        {
            entity.HasKey(e => new { e.IdObra, e.IdHerramienta, e.FechaSalida })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("herramientas_asignadas");

            entity.HasIndex(e => e.IdHerramienta, "IDX_HerramientaAsignada_Herramienta");

            entity.Property(e => e.IdObra).HasColumnName("Id_Obra");
            entity.Property(e => e.IdHerramienta).HasColumnName("Id_Herramienta");
            entity.Property(e => e.FechaSalida).HasColumnName("Fecha_Salida");
            entity.Property(e => e.FechaDevolucion).HasColumnName("Fecha_Devolucion");

            entity.HasOne(d => d.IdHerramientaNavigation).WithMany(p => p.HerramientasAsignada)
                .HasForeignKey(d => d.IdHerramienta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("herramientas_asignadas_ibfk_2");

            entity.HasOne(d => d.IdObraNavigation).WithMany(p => p.HerramientasAsignada)
                .HasForeignKey(d => d.IdObra)
                .HasConstraintName("herramientas_asignadas_ibfk_1");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.IdMaterial).HasName("PRIMARY");

            entity.ToTable("material");

            entity.Property(e => e.IdMaterial).HasColumnName("Id_Material");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.UnidadMedida)
                .HasMaxLength(20)
                .HasColumnName("Unidad_Medida");
        });

        modelBuilder.Entity<MaterialObra>(entity =>
        {
            entity.HasKey(e => new { e.IdObra, e.IdMaterial, e.FechaConsumo })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("material_obra");

            entity.HasIndex(e => e.IdMaterial, "IDX_MaterialObra_Material");

            entity.Property(e => e.IdObra).HasColumnName("Id_Obra");
            entity.Property(e => e.IdMaterial).HasColumnName("Id_Material");
            entity.Property(e => e.FechaConsumo).HasColumnName("Fecha_Consumo");
            entity.Property(e => e.CantidadConsumida)
                .HasPrecision(10, 2)
                .HasColumnName("Cantidad_Consumida");

            entity.HasOne(d => d.IdMaterialNavigation).WithMany(p => p.MaterialObras)
                .HasForeignKey(d => d.IdMaterial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("material_obra_ibfk_2");

            entity.HasOne(d => d.IdObraNavigation).WithMany(p => p.MaterialObras)
                .HasForeignKey(d => d.IdObra)
                .HasConstraintName("material_obra_ibfk_1");
        });

        modelBuilder.Entity<NovedadesObra>(entity =>
        {
            entity.HasKey(e => e.IdNovedad).HasName("PRIMARY");

            entity.ToTable("novedades_obra");

            entity.HasIndex(e => e.IdEmpleadoObra, "IDX_Novedad_EmpleadoObra");

            entity.HasIndex(e => e.EstadoRevision, "IDX_Novedad_Estado");

            entity.Property(e => e.IdNovedad).HasColumnName("Id_Novedad");
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.EstadoRevision)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pendiente'")
                .HasColumnName("Estado_Revision");
            entity.Property(e => e.IdEmpleadoObra).HasColumnName("Id_Empleado_Obra");
            entity.Property(e => e.TipoNovedad)
                .HasMaxLength(50)
                .HasColumnName("Tipo_Novedad");

            entity.HasOne(d => d.IdEmpleadoObraNavigation).WithMany(p => p.NovedadesObras)
                .HasForeignKey(d => d.IdEmpleadoObra)
                .HasConstraintName("novedades_obra_ibfk_1");
        });

        modelBuilder.Entity<Obra>(entity =>
        {
            entity.HasKey(e => e.IdObra).HasName("PRIMARY");

            entity.ToTable("obra");

            entity.HasIndex(e => e.CodigoPublico, "Codigo_Publico").IsUnique();

            entity.HasIndex(e => e.IdCliente, "Id_Cliente");

            entity.Property(e => e.IdObra).HasColumnName("Id_Obra");
            entity.Property(e => e.CodigoPublico)
                .HasMaxLength(50)
                .HasColumnName("Codigo_Publico");
            entity.Property(e => e.Direccion).HasMaxLength(150);
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Planificada'");
            entity.Property(e => e.FechaFinPrevista).HasColumnName("Fecha_Fin_Prevista");
            entity.Property(e => e.FechaInicio).HasColumnName("Fecha_Inicio");
            entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");
            entity.Property(e => e.NombreObra)
                .HasMaxLength(150)
                .HasColumnName("Nombre_Obra");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Obras)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("obra_ibfk_1");
        });

        modelBuilder.Entity<OrdenCompra>(entity =>
        {
            entity.HasKey(e => e.IdOrden).HasName("PRIMARY");

            entity.ToTable("orden_compra");

            entity.HasIndex(e => e.EstadoEntrega, "IDX_OrdenCompra_Estado");

            entity.HasIndex(e => e.IdProveedor, "IDX_OrdenCompra_Proveedor");

            entity.Property(e => e.IdOrden).HasColumnName("Id_Orden");
            entity.Property(e => e.EstadoEntrega)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Pendiente'")
                .HasColumnName("Estado_Entrega");
            entity.Property(e => e.FechaPedido).HasColumnName("Fecha_Pedido");
            entity.Property(e => e.IdProveedor).HasColumnName("Id_Proveedor");
            entity.Property(e => e.MontoTotal)
                .HasPrecision(12, 2)
                .HasColumnName("Monto_Total");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.OrdenCompras)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orden_compra_ibfk_1");
        });

        modelBuilder.Entity<PagoEmpleado>(entity =>
        {
            entity.HasKey(e => e.IdPagoEmpleado).HasName("PRIMARY");

            entity.ToTable("pago_empleado");

            entity.HasIndex(e => e.PeriodoMesAnio, "IDX_PagoEmpleado_Periodo").IsUnique();

            entity.HasIndex(e => e.IdEmpleado, "Id_Empleado").IsUnique();

            entity.Property(e => e.IdPagoEmpleado).HasColumnName("Id_Pago_Empleado");
            entity.Property(e => e.FechaPago).HasColumnName("Fecha_Pago");
            entity.Property(e => e.IdEmpleado).HasColumnName("Id_Empleado");
            entity.Property(e => e.MontoNeto)
                .HasPrecision(10, 2)
                .HasColumnName("Monto_Neto");
            entity.Property(e => e.PeriodoMesAnio)
                .HasMaxLength(7)
                .HasColumnName("Periodo_Mes_Anio");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithOne(p => p.PagoEmpleado)
                .HasForeignKey<PagoEmpleado>(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pago_empleado_ibfk_1");
        });

        modelBuilder.Entity<PagoProveedor>(entity =>
        {
            entity.HasKey(e => e.IdPagoProveedor).HasName("PRIMARY");

            entity.ToTable("pago_proveedor");

            entity.HasIndex(e => e.IdProveedor, "IDX_PagoProveedor_Proveedor");

            entity.HasIndex(e => e.FechaPago, "IDX_PagoProveedor_fecha");

            entity.Property(e => e.IdPagoProveedor).HasColumnName("Id_Pago_Proveedor");
            entity.Property(e => e.FechaPago).HasColumnName("Fecha_Pago");
            entity.Property(e => e.IdProveedor).HasColumnName("Id_Proveedor");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .HasColumnName("Metodo_Pago");
            entity.Property(e => e.Monto).HasPrecision(11, 2);

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.PagoProveedors)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pago_proveedor_ibfk_1");
        });

        modelBuilder.Entity<Presupuesto>(entity =>
        {
            entity.HasKey(e => e.IdPresupuesto).HasName("PRIMARY");

            entity.ToTable("presupuesto");

            entity.HasIndex(e => e.IdObra, "IDX_Presupuesto_Obra");

            entity.Property(e => e.IdPresupuesto).HasColumnName("Id_Presupuesto");
            entity.Property(e => e.EstadoPresupuesto)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Pendiente'")
                .HasColumnName("Estado_Presupuesto");
            entity.Property(e => e.FechaEmision).HasColumnName("Fecha_Emision");
            entity.Property(e => e.IdObra).HasColumnName("Id_Obra");
            entity.Property(e => e.MontoTotal)
                .HasPrecision(12, 2)
                .HasColumnName("Monto_Total");

            entity.HasOne(d => d.IdObraNavigation).WithMany(p => p.Presupuestos)
                .HasForeignKey(d => d.IdObra)
                .HasConstraintName("presupuesto_ibfk_1");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PRIMARY");

            entity.ToTable("proveedor");

            entity.HasIndex(e => e.Rut, "RUT").IsUnique();

            entity.Property(e => e.IdProveedor).HasColumnName("Id_Proveedor");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Rut)
                .HasMaxLength(20)
                .HasColumnName("RUT");
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<RegistroHora>(entity =>
        {
            entity.HasKey(e => e.IdRegistro).HasName("PRIMARY");

            entity.ToTable("registro_horas");

            entity.HasIndex(e => e.IdEmpleadoObra, "IDX_RegistroHoras_EmpleadoObra");

            entity.HasIndex(e => e.Fecha, "IDX_RegistroHoras_Fecha");

            entity.Property(e => e.IdRegistro).HasColumnName("Id_Registro");
            entity.Property(e => e.HorasComunes)
                .HasPrecision(4, 2)
                .HasColumnName("Horas_Comunes");
            entity.Property(e => e.HorasExtras)
                .HasPrecision(4, 2)
                .HasColumnName("Horas_Extras");
            entity.Property(e => e.IdEmpleadoObra).HasColumnName("Id_Empleado_Obra");
            entity.Property(e => e.ObservacionesEmpleado)
                .HasMaxLength(255)
                .HasColumnName("Observaciones_Empleado");

            entity.HasOne(d => d.IdEmpleadoObraNavigation).WithMany(p => p.RegistroHoras)
                .HasForeignKey(d => d.IdEmpleadoObra)
                .HasConstraintName("registro_horas_ibfk_1");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PRIMARY");

            entity.ToTable("rol");

            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .HasColumnName("Nombre_Rol");
        });

        modelBuilder.Entity<SeguimientoObra>(entity =>
        {
            entity.HasKey(e => e.IdSeguimiento).HasName("PRIMARY");

            entity.ToTable("seguimiento_obra");

            entity.HasIndex(e => e.IdObra, "IDX_Seguimiento_Obra");

            entity.Property(e => e.IdSeguimiento).HasColumnName("Id_Seguimiento");
            entity.Property(e => e.DescripcionAvance)
                .HasColumnType("text")
                .HasColumnName("Descripcion_Avance");
            entity.Property(e => e.IdObra).HasColumnName("Id_Obra");
            entity.Property(e => e.ImgProgreso)
                .HasMaxLength(500)
                .HasColumnName("img_progreso");
            entity.Property(e => e.PorcentajeAvance).HasColumnName("Porcentaje_Avance");

            entity.HasOne(d => d.IdObraNavigation).WithMany(p => p.SeguimientoObras)
                .HasForeignKey(d => d.IdObra)
                .HasConstraintName("seguimiento_obra_ibfk_1");
        });

        modelBuilder.Entity<StockMaterial>(entity =>
        {
            entity.HasKey(e => e.IdMaterial).HasName("PRIMARY");

            entity.ToTable("stock_material");

            entity.Property(e => e.IdMaterial)
                .ValueGeneratedNever()
                .HasColumnName("Id_Material");
            entity.Property(e => e.CantidadDisponible)
                .HasPrecision(10, 2)
                .HasColumnName("Cantidad_Disponible");
            entity.Property(e => e.StockMinimo)
                .HasPrecision(10, 2)
                .HasColumnName("Stock_Minimo");

            entity.HasOne(d => d.IdMaterialNavigation).WithOne(p => p.StockMaterial)
                .HasForeignKey<StockMaterial>(d => d.IdMaterial)
                .HasConstraintName("stock_material_ibfk_1");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.HasIndex(e => e.IdRol, "Id_Rol");

            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.Contrasena).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pendiente'");
            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
