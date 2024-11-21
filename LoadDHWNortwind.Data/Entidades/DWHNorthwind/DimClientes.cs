
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LoadDHWNortwind.Data.Entidades.DWHNorthwind
{
    
    public class DimClientes
    {
        public int DimClienteKey { get; set; }
        public string ClienteID { get; set; }
        public string Compañia { get; set; }
        public string Contacto { get; set; }
    }

}
