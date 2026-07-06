


namespace PagesObrasApp.Models.DTOs
{
        // --- ESTRUCTURA DE CADA RENGLÓN DEL PRESUPUESTO ---
        public class DetallePresupuestoDto
        {
            public string Descripcion { get; set; } = null!;
            public decimal Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
        }

        // --- DTO PARA CREAR UN PRESUPUESTO COMPLETO ---
        public class CrearPresupuestoDto
        {
            public int IdObra { get; set; }
            public string EstadoPresupuesto { get; set; } = "Pendiente"; // Por defecto arranca en Pendiente

            // El cliente manda la lista de renglones adentro del mismo JSON
            public List<DetallePresupuestoDto> Lineas { get; set; } = new List<DetallePresupuestoDto>();
        }

        // --- DTO PARA MOSTRAR EN EL LISTADO ---
        public class PresupuestoListadoDto
        {
            public int IdPresupuesto { get; set; }
            public int IdObra { get; set; }
            public string NombreObra { get; set; } = null!;
            public DateTime FechaEmision { get; set; }
            public decimal MontoTotal { get; set; }
            public string? EstadoPresupuesto { get; set; }
            public int CantidadItems { get; set; } // Opcional: para ver cuántos renglones tiene
        }
    }

