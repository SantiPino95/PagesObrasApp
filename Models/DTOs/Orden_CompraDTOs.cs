namespace PagesObrasApp.Models.DTOs
{
 

        public class OrdenCompraDTO
        {
            public int IdOrden { get; set; }

            public int IdProveedor { get; set; }

            public string? NombreProveedor { get; set; }

            public DateTime FechaPedido { get; set; }

            public decimal MontoTotal { get; set; }

            public string? EstadoEntrega { get; set; }

            public List<DetalleOrdenCompraDTO> Detalles { get; set; } = new();
        }




        public class DetalleOrdenCompraDTO
        {
            public int IdMaterial { get; set; }

            public string? NombreMaterial { get; set; }

            public decimal CantidadPedida { get; set; }

            public decimal PrecioUnitarioCompra { get; set; }

            public decimal Subtotal
            {
                get
                {
                    return CantidadPedida * PrecioUnitarioCompra;
                }
            }

        }





            public class CrearOrdenCompraDTO
        {
            public int IdProveedor { get; set; }

            public DateTime FechaPedido { get; set; }

            public List<DetalleOrdenCompraDTO> Detalles { get; set; } = new();
        }
    }

