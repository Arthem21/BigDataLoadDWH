
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LoadDHWNortwind.Data.Entidades.DWHNorthwind
{
    public class DimProductos
    {
        public int DimProductosKey { get; set; }
        public int CategoriaID { get; set; }
        public string Categoria { get; set; }
        public int ProductoID { get; set; }
        public string Producto { get; set; }
        public short? Cantidad { get; set; }
        public decimal? Precio { get; set; }
    }

}
