using System;
using System.Collections.Generic;

namespace PagesObrasApp.Models;

public partial class DetallePresupuesto
{
    public int IdDetallePresupuesto { get; set; }

    public int IdPresupuesto { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Presupuesto IdPresupuestoNavigation { get; set; } = null!;
}
