using System.ComponentModel.DataAnnotations;

namespace PagesObrasApp.Models.DTOs
{
    public class ProveedorListadoDto
    {
        public int IdProveedor { get; set; }
        public string Nombre { get; set; } = null!;
        public string Rut { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }

    public class CrearProveedorDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El RUT es obligatorio")]
        public string Rut { get; set; } = null!; // Podés meter validación de 12 dígitos en el DTO más adelante
        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }
}
