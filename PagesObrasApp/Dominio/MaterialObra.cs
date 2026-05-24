using System;
using System.Collections.Generic;

namespace PagesObrasApp.Dominio;

public partial class MaterialObra
{
    public int IdObra { get; set; }

    public int IdMaterial { get; set; }

    public decimal CantidadConsumida { get; set; }

    public DateOnly FechaConsumo { get; set; }

    public virtual Material IdMaterialNavigation { get; set; } = null!;

    public virtual Obra IdObraNavigation { get; set; } = null!;
}
