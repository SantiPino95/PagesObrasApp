namespace PagesObrasApp.Models.DTOs
{
    public class ObraAdminListadoDto
    {
        public string CodigoFormateado { get; set; } = null!;
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinPrevista { get; set; }
        public string? Estado { get; set; }
        public decimal TotalGastado { get; set; }
        public int PorcentajeAvanceActual { get; set; }

        // 1. En vez de traer todo el objeto Cliente, solo traemos lo que la lista necesita
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = null!;

        // 2. Aquí guardaremos el código ya formateado 
    }

    public class CrearObraDto
    {


        public int IdCliente { get; set; }

        public string NombreObra { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFinPrevista { get; set; }


    }

    public class ObraDetalleDto
    {
        public int IdObra { get; set; }
        public string CodigoFormateado { get; set; } = null!;
        public string NombreObra { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinPrevista { get; set; }
        public string? Estado { get; set; }

       

        // Datos del cliente
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = null!;

        // REUTILIZACIÓN: Lista de empleados asignados a esta obra
        public List<EmpleadoListadoDTOs> EmpleadosAsignados { get; set; } = new();
    }


}
