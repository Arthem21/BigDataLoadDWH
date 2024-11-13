
using System.ComponentModel.DataAnnotations.Schema;


namespace LoadDHWNortwind.Data.Entidades.DWHNorthwind
{
    [Table ("DimCargadores")]
    public class DimCargador
    {
        public int DimCargadoresKey { get; set; }
        public int CargadorID { get; set; }
        public string Cargador { get; set; }
    }

}
