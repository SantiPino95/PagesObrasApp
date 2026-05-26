using System;
using System.Collections.Generic;

namespace PagesObrasApp.Dominio;

public partial class Material
{
    public int IdMaterial { get; set; }

    public string Nombre { get; set; } = null!;

    public string UnidadMedida { get; set; } = null!;

    public virtual ICollection<DetalleOrdenCompra> DetalleOrdenCompras { get; set; } = new List<DetalleOrdenCompra>();

    public virtual ICollection<MaterialObra> MaterialObras { get; set; } = new List<MaterialObra>();

    public virtual StockMaterial? StockMaterial { get; set; }
}
