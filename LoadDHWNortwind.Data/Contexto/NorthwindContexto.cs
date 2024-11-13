
using LoadDHWNortwind.Data.Entidades.Northwind;
using Microsoft.EntityFrameworkCore;

namespace LoadDHWNortwind.Data.Contexto
{
    public partial class NorthwindContexto:DbContext
    {
        public NorthwindContexto (DbContextOptions<NorthwindContexto> options):base(options){ }

        #region"Db Sets"

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Territory> Territories { get; set; }
        public DbSet<Shipper> Shippers { get; set; }

        #endregion
    }
}
