using System;
using System.Collections.Generic;

namespace PagesObrasApp.Dominio;

public partial class Herramienta
{
    public int IdHerramienta { get; set; }

    public string NombreTipo { get; set; } = null!;

    public string CodigoInventario { get; set; } = null!;

    public string EstadoDisponibilidad { get; set; } = null!;

    public string Origen { get; set; } = null!;

    public virtual ICollection<HerramientasAsignada> HerramientasAsignada { get; set; } = new List<HerramientasAsignada>();
}
