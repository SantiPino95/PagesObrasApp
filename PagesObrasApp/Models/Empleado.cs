using System;
using System.Collections.Generic;

namespace PagesObrasApp.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public int? IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Cedula { get; set; } = null!;

    public string? Telefono { get; set; }

    public decimal ValorHora { get; set; }

    public virtual ICollection<EmpleadoObra> EmpleadoObras { get; set; } = new List<EmpleadoObra>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual PagoEmpleado? PagoEmpleado { get; set; }
}
