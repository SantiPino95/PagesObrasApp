using System;
using System.Collections.Generic;

namespace PagesObrasApp.Models;

public partial class PagoEmpleado
{
    public int IdPagoEmpleado { get; set; }

    public int IdEmpleado { get; set; }

    public DateOnly FechaPago { get; set; }

    public decimal MontoNeto { get; set; }

    public string PeriodoMesAnio { get; set; } = null!;

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;
}
