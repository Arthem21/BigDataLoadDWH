using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDHWNorthwind.Data.Entidades.Northwind
{
    public class FactOrders
    {
        public int OrdenID { get; set; }

        public string? ClienteID { get; set; }

        public int ProductoID { get; set; }

        public int? EmpleadoID { get; set; }

        public DateTime? Fecha { get; set; }

        public int? CargadorID { get; set; }

        public string? CiudadID { get; set; }

        public string? PaisID { get; set; }

        public string? RegionID { get; set; }

        public decimal? TotalVentas { get; set; }

        public int? Cantidad { get; set; }
    }
}
