using LoadDHWNortwind.Data.Entidades.DWHNorthwind;
using Microsoft.EntityFrameworkCore;


public partial class DWHNorthwindContexto : DbContext
{
    public DWHNorthwindContexto(DbContextOptions<DWHNorthwindContexto> options)
        : base(options)
    {
    }

    public virtual DbSet<DimCargadores> DimCargadores { get; set; }

    public virtual DbSet<DimClientes> DimClientes { get; set; }

    public virtual DbSet<DimEmpleados> DimEmpleados { get; set; }

    public virtual DbSet<DimFecha> DimFecha { get; set; }

    public virtual DbSet<DimProductos> DimProductos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DimCargadores>(entity =>
        {
            entity.HasKey(e => e.DimCargadoresKey);

            entity.ToTable("DimCargadores", "DHW");

            entity.Property(e => e.Cargador)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<DimClientes>(entity =>
        {
            entity.HasKey(e => e.DimClienteKey);

            entity.ToTable("DimClientes", "DHW");

            entity.HasIndex(e => e.Compañia, "idx_cliente_compañia");

            entity.HasIndex(e => e.Contacto, "idx_cliente_contacto");

            entity.Property(e => e.ClienteID)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Compañia)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Contacto).HasMaxLength(50);
        });

        modelBuilder.Entity<DimEmpleados>(entity =>
        {
            entity.HasKey(e => e.DimEmpleadoKey);

            entity.ToTable("DimEmpleados", "DHW");

            entity.HasIndex(e => e.Apellido, "idx_Empleado_Apellido");

            entity.HasIndex(e => e.Nombre, "idx_Empleado_Nombre");

            entity.Property(e => e.Apellido)
                .IsRequired()
                .HasMaxLength(15);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(25);
            entity.Property(e => e.Titulo).HasMaxLength(30);
        });

        modelBuilder.Entity<DimFecha>(entity =>
        {
            entity.HasKey(e => e.DimFechaKey);

            entity.ToTable("DimFecha", "DHW");

            entity.HasIndex(e => e.DimFechaKey, "idx_fecha");

            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FormatoCompleto)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreDia)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreFecha)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreMes)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NombreTrimestre)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<DimProductos>(entity =>
        {
            entity.HasKey(e => e.DimProductosKey);

            entity.ToTable("DimProductos", "DHW");

            entity.HasIndex(e => e.Categoria, "idx_Categoria");

            entity.HasIndex(e => e.Producto, "idx_Producto");

            entity.Property(e => e.Categoria)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.Precio).HasColumnType("money");
            entity.Property(e => e.Producto)
                .IsRequired()
                .HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}