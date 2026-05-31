using System;
using System.Collections.Generic;

namespace PagesObrasApp.Models;

public partial class PagoProveedor
{
    public int IdPagoProveedor { get; set; }

    public int IdProveedor { get; set; }

    public DateOnly FechaPago { get; set; }

    public decimal Monto { get; set; }

    public string MetodoPago { get; set; } = null!;

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;
}
