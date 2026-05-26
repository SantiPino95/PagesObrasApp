using System;
using System.Collections.Generic;

namespace PagesObrasApp.Dominio;

public partial class EmpleadoObra
{
    public int IdEmpleadoObra { get; set; }

    public int IdEmpleado { get; set; }

    public int IdObra { get; set; }

    public DateOnly FechaAsignacion { get; set; }

    public string RolEnObra { get; set; } = null!;

    public decimal ValorHoraAsignado { get; set; }

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual Obra IdObraNavigation { get; set; } = null!;

    public virtual ICollection<NovedadesObra> NovedadesObras { get; set; } = new List<NovedadesObra>();

    public virtual ICollection<RegistroHora> RegistroHoras { get; set; } = new List<RegistroHora>();
}
