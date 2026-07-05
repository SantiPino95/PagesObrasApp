using System;

namespace PagesObrasApp.Models.DTOs
{
    // --- DTO PARA MOSTRAR EN EL LISTADO DEL ADMIN ---
    public class MaterialListadoDto
    {
        public int IdMaterial { get; set; }
        public string Nombre { get; set; } = null!;
        public string UnidadMedida { get; set; } = null!; // Ej: "Kg", "Litros", "Bolsas"

        // --- DATOS DE LA RELACIÓN 1 A 1 (Stock) ---
        public decimal CantidadDisponible { get; set; }
        public decimal StockMinimo { get; set; }

        // Propiedad calculada útil para meter alertas visuales rojas en tu Razor Pages
        public bool RequiereReposicion => CantidadDisponible <= StockMinimo;
    }

    // --- DTO PARA CREAR UN NUEVO MATERIAL ---
    public class CrearMaterialDto
    {
        public string Nombre { get; set; } = null!;
        public string UnidadMedida { get; set; } = null!;

        // Cuando das de alta un material, es buena práctica inicializar su stock
        public decimal CantidadInicial { get; set; } = 0;
        public decimal StockMinimo { get; set; } = 0;
    }


    public class ReponerStockDto
    {
        public int IdMaterial { get; set; }
        public decimal Cantidad { get; set; }
        public string? Observaciones { get; set; }
    }
}
