using System;
using System.Collections.Generic;

namespace PagesObrasApp.Models;

public partial class HerramientasAsignada
{
    public int IdObra { get; set; }

    public int IdHerramienta { get; set; }

    public DateOnly FechaSalida { get; set; }

    public DateOnly? FechaDevolucion { get; set; }

    public virtual Herramienta IdHerramientaNavigation { get; set; } = null!;

    public virtual Obra IdObraNavigation { get; set; } = null!;
}
