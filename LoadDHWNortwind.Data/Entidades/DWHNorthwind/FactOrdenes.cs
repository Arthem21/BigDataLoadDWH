

namespace LoadDHWNorthwind.Data.Entidades.DWHNorthwind
{
    public class FactOrdenes
    {
        public int FactOrdenesKey { get; set; }

        public int? OrdenID { get; set; }

        public int? ClienteID { get; set; }

        public int? ProductoID { get; set; }

        public int? EmpleadoID { get; set; }

        public int? FechaPedido { get; set; }

        public int? Cargador { get; set; }

        public string Ciudad { get; set; }

        public string Pais { get; set; }

        public string Region { get; set; }

        public decimal? Precio { get; set; }

        public short? Cantidad { get; set; }
    }
}
