using System;
using System.Collections.Generic;

namespace PagesObrasApp.Dominio;

public partial class Obra
{
    public int IdObra { get; set; }

    public int IdCliente { get; set; }

    public string NombreObra { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateOnly? FechaFinPrevista { get; set; }

    public string? Estado { get; set; }

    public string CodigoPublico { get; set; } = null!;

    public virtual ICollection<EmpleadoObra> EmpleadoObras { get; set; } = new List<EmpleadoObra>();

    public virtual ICollection<GastosObra> GastosObras { get; set; } = new List<GastosObra>();

    public virtual ICollection<HerramientasAsignada> HerramientasAsignada { get; set; } = new List<HerramientasAsignada>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual ICollection<MaterialObra> MaterialObras { get; set; } = new List<MaterialObra>();

    public virtual ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();

    public virtual ICollection<SeguimientoObra> SeguimientoObras { get; set; } = new List<SeguimientoObra>();
}
