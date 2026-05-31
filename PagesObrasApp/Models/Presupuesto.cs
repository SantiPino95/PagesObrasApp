using System;
using System.Collections.Generic;

namespace PagesObrasApp.Models;

public partial class Presupuesto
{
    public int IdPresupuesto { get; set; }

    public int IdObra { get; set; }

    public DateOnly FechaEmision { get; set; }

    public decimal MontoTotal { get; set; }

    public string? EstadoPresupuesto { get; set; }

    public virtual ICollection<DetallePresupuesto> DetallePresupuestos { get; set; } = new List<DetallePresupuesto>();

    public virtual Obra IdObraNavigation { get; set; } = null!;
}
