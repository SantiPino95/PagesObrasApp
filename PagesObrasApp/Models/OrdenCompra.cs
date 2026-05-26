using System;
using System.Collections.Generic;

namespace PagesObrasApp.Dominio;

public partial class OrdenCompra
{
    public int IdOrden { get; set; }

    public int IdProveedor { get; set; }

    public DateOnly FechaPedido { get; set; }

    public decimal MontoTotal { get; set; }

    public string? EstadoEntrega { get; set; }

    public virtual ICollection<DetalleOrdenCompra> DetalleOrdenCompras { get; set; } = new List<DetalleOrdenCompra>();

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;
}
