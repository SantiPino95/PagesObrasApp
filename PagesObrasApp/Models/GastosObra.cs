using System;
using System.Collections.Generic;

namespace PagesObrasApp.Models;

public partial class GastosObra
{
    public int IdGasto { get; set; }

    public int IdObra { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Monto { get; set; }

    public string Descripcion { get; set; } = null!;

    public string CategoriaGasto { get; set; } = null!;

    public string? NroComprobante { get; set; }

    public virtual Obra IdObraNavigation { get; set; } = null!;
}
