using System;
using System.Collections.Generic;

namespace PagesObrasApp.Dominio;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string Nombre { get; set; } = null!;

    public string Rut { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<OrdenCompra> OrdenCompras { get; set; } = new List<OrdenCompra>();

    public virtual ICollection<PagoProveedor> PagoProveedors { get; set; } = new List<PagoProveedor>();
}
