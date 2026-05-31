using System;
using System.Collections.Generic;

namespace PagesObrasApp.Models;

public partial class RegistroHora
{
    public int IdRegistro { get; set; }

    public int IdEmpleadoObra { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal HorasComunes { get; set; }

    public decimal HorasExtras { get; set; }

    public string? ObservacionesEmpleado { get; set; }

    public virtual EmpleadoObra IdEmpleadoObraNavigation { get; set; } = null!;
}
