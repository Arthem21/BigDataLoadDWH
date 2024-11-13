using LoadDHWNortwind.Data.Entidades.DWHNorthwind;

using Microsoft.EntityFrameworkCore;


namespace LoadDHWNortwind.Data.Contexto
{
    internal class DWHNorthwindContexto: DbContext
    {
        public DWHNorthwindContexto(DbContextOptions<DWHNorthwindContexto> options) : base(options) { }

        #region"Dhw Sets"

        public DbSet<DimCargador> DimCargadores { get; set; }
        public DbSet<DimCliente> DimClientes { get; set; }
        public DbSet<DimEmpleado> DimEmpleados { get; set; }
        public DbSet<DimFecha> DimFecha { get; set; }
        public DbSet<DimProducto> DimProductos { get; set; }


        #endregion
    }


}
