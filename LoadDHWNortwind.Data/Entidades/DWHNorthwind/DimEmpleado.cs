
using System.ComponentModel.DataAnnotations.Schema;


namespace LoadDHWNortwind.Data.Entidades.DWHNorthwind
{
    [Table("DimEmpleados")]
    public class DimEmpleado
    {
        public int DimEmpleadoKey { get; set; }
        public int EmpleadoID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Titulo { get; set; }
    }

}
