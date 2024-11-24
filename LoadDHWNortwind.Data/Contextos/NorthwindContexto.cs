
using LoadDHWNorthwind.Data.Entidades.Northwind;
using LoadDHWNortwind.Data.Entidades.Northwind;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LoadDHWNortwind.Data.Contexto
{
    public partial class NorthwindContexto:DbContext
    {
        public NorthwindContexto (DbContextOptions<NorthwindContexto> options):base(options){ }

        #region DBSets

        public   DbSet<Category> Categories { get; set; }

        public   DbSet<Customers_x_Employees> Customers_x_Employees { get; set; }

        public   DbSet<Customer> Customers { get; set; }

        public   DbSet<Employee> Employees { get; set; }

        public   DbSet<FactOrders> FactOrdenes { get; set; }

        public   DbSet<OrderDetail> Order_Details { get; set; }

        public   DbSet<Order> Orders { get; set; }

        public   DbSet<Product> Products { get; set; }

        public   DbSet<Shipper> Shippers { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryID);

                entity.HasIndex(e => new { e.CategoryID, e.CategoryName }, "CategoryID");

                entity.HasIndex(e => e.CategoryName, "CategoryName");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(15);
                entity.Property(e => e.Description).HasColumnType("ntext");
                entity.Property(e => e.Picture).HasColumnType("image");
            });

            modelBuilder.Entity<Customers_x_Employees>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("ClientesAtendidosPorEmpleado", "DWH");

                entity.Property(e => e.CantidadClientesUnicos).HasMaxLength(4000);
                entity.Property(e => e.Empleado)
                    .IsRequired()
                    .HasMaxLength(31);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerID);

                entity.HasIndex(e => e.City, "City");

                entity.HasIndex(e => e.PostalCode, "PostalCode");

                entity.HasIndex(e => e.Region, "Region");

                entity.HasIndex(e => new { e.CustomerID, e.CompanyName, e.ContactName }, "idx_CompanyName");

                entity.HasIndex(e => new { e.Country, e.City }, "idx_Paises");

                entity.HasIndex(e => new { e.Region, e.Country, e.City }, "idx_Regiones");

                entity.Property(e => e.CustomerID)
                    .HasMaxLength(5)
                    .IsFixedLength();
                entity.Property(e => e.Address).HasMaxLength(60);
                entity.Property(e => e.City).HasMaxLength(15);
                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(40);
                entity.Property(e => e.ContactName).HasMaxLength(30);
                entity.Property(e => e.ContactTitle).HasMaxLength(30);
                entity.Property(e => e.Country).HasMaxLength(15);
                entity.Property(e => e.Fax).HasMaxLength(24);
                entity.Property(e => e.Phone).HasMaxLength(24);
                entity.Property(e => e.PostalCode).HasMaxLength(10);
                entity.Property(e => e.Region).HasMaxLength(15);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeID);

                entity.HasIndex(e => e.PostalCode, "PostalCode");

                entity.HasIndex(e => new { e.EmployeeID, e.FirstName, e.LastName }, "idx_Name");

                entity.Property(e => e.Address).HasMaxLength(60);
                entity.Property(e => e.BirthDate).HasColumnType("datetime");
                entity.Property(e => e.City).HasMaxLength(15);
                entity.Property(e => e.Country).HasMaxLength(15);
                entity.Property(e => e.Extension).HasMaxLength(4);
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(10);
                entity.Property(e => e.HireDate).HasColumnType("datetime");
                entity.Property(e => e.HomePhone).HasMaxLength(24);
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.Property(e => e.Notes).HasColumnType("ntext");
                entity.Property(e => e.Photo).HasColumnType("image");
                entity.Property(e => e.PhotoPath).HasMaxLength(255);
                entity.Property(e => e.PostalCode).HasMaxLength(10);
                entity.Property(e => e.Region).HasMaxLength(15);
                entity.Property(e => e.Title).HasMaxLength(30);
                entity.Property(e => e.TitleOfCourtesy).HasMaxLength(25);

                entity.HasOne(d => d.ReportsToNavigation).WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo)
                    .HasConstraintName("FK_Employees_Employees");
            });

            modelBuilder.Entity<FactOrders>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("FactOrdenes", "DWH");

                entity.Property(e => e.CiudadID).HasMaxLength(15);
                entity.Property(e => e.ClienteID)
                    .HasMaxLength(5)
                    .IsFixedLength();
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.PaisID).HasMaxLength(15);
                entity.Property(e => e.RegionID).HasMaxLength(15);
                entity.Property(e => e.TotalVentas).HasColumnType("money");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderID, e.ProductID }).HasName("PK_Order_Details");

                entity.ToTable("Order Details");

                entity.HasIndex(e => e.OrderID, "OrderID");

                entity.HasIndex(e => e.ProductID, "ProductID");

                entity.HasIndex(e => new { e.ProductID, e.OrderID }, "idx_ProductosID");

                entity.Property(e => e.Quantity).HasDefaultValue((short)1);
                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.Order).WithMany(p => p.Order_Details)
                    .HasForeignKey(d => d.OrderID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Details_Orders");

                entity.HasOne(d => d.Product).WithMany(p => p.Order_Details)
                    .HasForeignKey(d => d.ProductID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Details_Products");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderID);

                entity.HasIndex(e => e.CustomerID, "CustomerID");

                entity.HasIndex(e => e.EmployeeID, "EmployeeID");

                entity.HasIndex(e => e.OrderDate, "OrderDate");

                entity.HasIndex(e => e.ShipPostalCode, "ShipPostalCode");

                entity.HasIndex(e => e.ShippedDate, "ShippedDate");

                entity.HasIndex(e => e.ShipVia, "ShippersOrders");

                entity.HasIndex(e => e.CustomerID, "idx_ClientesID");

                entity.HasIndex(e => e.EmployeeID, "idx_EmpleadosID");

                entity.HasIndex(e => e.OrderDate, "idx_Fechas");

                entity.HasIndex(e => e.ShipVia, "idx_ProveedoresID");

                entity.Property(e => e.CustomerID)
                    .HasMaxLength(5)
                    .IsFixedLength();
                entity.Property(e => e.Freight)
                    .HasDefaultValue(0m)
                    .HasColumnType("money");
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.RequiredDate).HasColumnType("datetime");
                entity.Property(e => e.ShipAddress).HasMaxLength(60);
                entity.Property(e => e.ShipCity).HasMaxLength(15);
                entity.Property(e => e.ShipCountry).HasMaxLength(15);
                entity.Property(e => e.ShipName).HasMaxLength(40);
                entity.Property(e => e.ShipPostalCode).HasMaxLength(10);
                entity.Property(e => e.ShipRegion).HasMaxLength(15);
                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerID)
                    .HasConstraintName("FK_Orders_Customers");

                entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeID)
                    .HasConstraintName("FK_Orders_Employees");

                entity.HasOne(d => d.ShipViaNavigation).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipVia)
                    .HasConstraintName("FK_Orders_Shippers");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductID);

                entity.HasIndex(e => e.CategoryID, "CategoriesProducts");

                entity.HasIndex(e => e.CategoryID, "CategoryID");

                entity.HasIndex(e => e.ProductName, "ProductName");

                entity.HasIndex(e => e.SupplierID, "SupplierID");

                entity.HasIndex(e => e.SupplierID, "SuppliersProducts");

                entity.HasIndex(e => new { e.CategoryID, e.ProductID, e.ProductName }, "idx_Product");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(40);
                entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);
                entity.Property(e => e.ReorderLevel).HasDefaultValue((short)0);
                entity.Property(e => e.UnitPrice)
                    .HasDefaultValue(0m)
                    .HasColumnType("money");
                entity.Property(e => e.UnitsInStock).HasDefaultValue((short)0);
                entity.Property(e => e.UnitsOnOrder).HasDefaultValue((short)0);

                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryID)
                    .HasConstraintName("FK_Products_Categories");
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.HasKey(e => e.ShipperID);

                entity.HasIndex(e => new { e.ShipperID, e.CompanyName }, "idx_shipperID");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(40);
                entity.Property(e => e.Phone).HasMaxLength(24);
            });

            
        }

        
    }

    
}
