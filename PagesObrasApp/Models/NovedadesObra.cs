using System;
using System.Collections.Generic;

namespace PagesObrasApp.Models;

public partial class NovedadesObra
{
    public int IdNovedad { get; set; }

    public int IdEmpleadoObra { get; set; }

    public DateOnly Fecha { get; set; }

    public string TipoNovedad { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string? EstadoRevision { get; set; }

    public virtual EmpleadoObra IdEmpleadoObraNavigation { get; set; } = null!;
}
