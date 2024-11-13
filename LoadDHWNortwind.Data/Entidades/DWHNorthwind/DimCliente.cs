
using System.ComponentModel.DataAnnotations.Schema;


namespace LoadDHWNortwind.Data.Entidades.DWHNorthwind
{
    [Table("DimClientes")]
    public class DimCliente
    {

        public int DimClienteKey { get; set; }
        public string ClienteID { get; set; }
        public string Compañia { get; set; }
        public string Contacto { get; set; }
    }

}
