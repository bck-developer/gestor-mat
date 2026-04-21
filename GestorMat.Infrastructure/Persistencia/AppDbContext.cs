using GestorMat.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GestorMat.Infrastructure.Persistencia
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Material> Materiales { get; set; }
        public DbSet<UnidadMedida> UnidadesMedida { get; set; }
        public DbSet<Deposito> Depositos { get; set; }
        public DbSet<Saldo> Saldos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        
        public DbSet<MovimientoMaterial> MovimientosMaterial { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<UnidadMedida>(entity =>
            {
                entity.ToTable("unidades_medida");
                entity.HasKey(e => e.Id_UnidadMedida);
            });

            modelBuilder.Entity<Deposito>(entity =>
            {
                entity.ToTable("depositos");
                entity.HasKey(e => e.Id_Deposito);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("roles");

                entity.HasKey(e => e.Id_Rol);

                entity.Property(e => e.RolName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250);

                entity.Property(e => e.AccesoTotal)
                    .IsRequired();
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.ToTable("materiales");

                entity.HasKey(e => e.Id_Material);

                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.Precio).HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.UnidadMedida)
                      .WithMany()
                      .HasForeignKey(e => e.Id_UnidadMedida);
            });

            modelBuilder.Entity<Saldo>(entity =>
            {
                entity.ToTable("saldos");

                entity.HasKey(e => e.Id_Saldo);

                entity.Property(e => e.Cantidad)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Material)
                      .WithMany()
                      .HasForeignKey(e => e.Id_Material);

                entity.HasOne(e => e.Deposito)
                      .WithMany()
                      .HasForeignKey(e => e.Id_Deposito);

                entity.HasIndex(e => new { e.Id_Material, e.Id_Deposito })
                      .IsUnique();
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");

                entity.HasKey(e => e.Id_Usuario);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                    .IsRequired();

                entity.Property(e => e.Nombre)
                    .IsRequired();

                entity.Property(e => e.Mail)
                    .IsRequired();

                entity.Property(e => e.FechaAlta)
                    .IsRequired();

                entity.Property(e => e.Activo)
                    .IsRequired();

                entity.HasOne(e => e.Rol)
                    .WithMany(r => r.Usuarios)
                    .HasForeignKey(e => e.IdRol)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
