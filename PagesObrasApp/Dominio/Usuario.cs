using System;
using System.Collections.Generic;

namespace PagesObrasApp.Dominio;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public string Email { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string? Estado { get; set; }

    public virtual Empleado? Empleado { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;
}
