using System;

namespace PagesObrasApp.Models.DTOs
{
    // --- DTO PARA MOSTRAR EN EL LISTADO DEL ADMIN ---
    public class HerramientaListadoDto
    {
        public int IdHerramienta { get; set; }
        public string NombreTipo { get; set; } = null!;
        public string CodigoInventario { get; set; } = null!;
        public string EstadoDisponibilidad { get; set; }  = "Disponible"; 
        public string Origen { get; set; } = null!;

        // --- DATOS DE LA TABLA INTERMEDIA (Asignación Actual / Última) ---
        public int? IdObraActual { get; set; }
        public string? NombreObraActual { get; set; } // Opcional, por si querés mostrar en qué obra está trabajando
        public DateTime? UltimaFechaSalida { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public bool EstaAsignadaActualmente => UltimaFechaSalida.HasValue && !FechaDevolucion.HasValue;
    }

    // --- DTO PARA CREAR UNA NUEVA HERRAMIENTA ---
    public class CrearHerramientaDto
    {
        // No va IdHerramienta porque es AUTO_INCREMENT en MySQL
        public string NombreTipo { get; set; } = null!;
        public string CodigoInventario { get; set; } = null!;
        public string EstadoDisponibilidad { get; set; } = default!; // Valor por defecto lógico
        public string Origen { get; set; } = null!;

        // Al crear la herramienta de cero en el inventario, no se manda IdObra 
        // porque primero nace "Disponible" en el taller, luego se asignará.
    }
}