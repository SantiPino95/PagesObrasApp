using System.ComponentModel.DataAnnotations;

namespace PagesObrasApp.Models.DTOs
{
    // DTO para listar clientes con sus obras asociadas
    public class CLientesListadoDTOs
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public List<ObraResumenDto> Obras { get; set; } = new List<ObraResumenDto>();
    }

    // DTO para mostrar un resumen de la obra asociada al cliente
    public class ObraResumenDto
    {
        public int IdObra { get; set; }
        public string NombreObra { get; set; } = null!;
    }

    // DTO para crear un nuevo cliente
    public class CrearClienteDTOs
    {
       //

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!; // Obligatorio

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = null!; // Obligatorio

        // Si el teléfono y el email no son obligatorios para registrarse, se usa '?'
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        public string Email { get; set; } = null!;
    }


}
