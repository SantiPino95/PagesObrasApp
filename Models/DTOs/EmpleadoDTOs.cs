    using System.ComponentModel.DataAnnotations;

    namespace PagesObrasApp.Models.DTOs 
    {
        // 1. DTO PARA REGISTRAR / EDITAR EMPLEADO
        public class CrearEmpleadoDTOs
        {
            [Required(ErrorMessage = "El nombre es obligatorio")]
            public string Nombre { get; set; } = null!;

            [Required(ErrorMessage = "El apellido es obligatorio")]
            public string Apellido { get; set; } = null!;

            [Required(ErrorMessage = "La categoría es obligatoria")]
            public string Categoria { get; set; } = null!;

            public string? Telefono { get; set; }
        }

        // 2. DTO PARA MOSTRAR EN LOS LISTADOS (Para el Administrador)
        public class EmpleadoListadoDTOs
        {
            public int IdEmpleado { get; set; }
            public string NombreCompleto { get; set; } = null!;
            public string Categoria { get; set; } = null!;
            public string? Telefono { get; set; }
        }

    
    public class EmpleadoObraDTOs
    {
        public int IdEmpleado { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public string? Telefono { get; set; }
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = null!;
    }


    // DTO para ASIGNAR empleado a una obra
    public class AsignarEmpleadoObraDto
    {
        public int IdEmpleado { get; set; }
        public int IdObra { get; set; }
        public string RolEnObra { get; set; } = "Oficial";
        public decimal ValorHoraAsignado { get; set; }
    }


    public class RegistroHoraDto
    {
        public int IdRegistro { get; set; }
        public int IdEmpleadoObra { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasComunes { get; set; }
        public decimal HorasExtras { get; set; }
        public string? ObservacionesEmpleado { get; set; }
    }

    public class CrearRegistroHoraDto
    {
        public int IdEmpleadoObra { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasComunes { get; set; }
        public decimal HorasExtras { get; set; }
        public string? ObservacionesEmpleado { get; set; }
    }
}


