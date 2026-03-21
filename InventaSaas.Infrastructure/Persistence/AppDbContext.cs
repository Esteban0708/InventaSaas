using InventaSaas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventaSaas.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");

                entity.HasKey(e => e.IdUsuario);

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("id_usuario");

                entity.Property(e => e.Nombres)
                    .HasColumnName("nombres");

                entity.Property(e => e.Apellidos)
                    .HasColumnName("apellidos");

                entity.Property(e => e.Email)
                    .HasColumnName("email");

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono");

                entity.Property(e => e.PasswordHash)
                    .HasColumnName("password_hash");

                entity.Property(e => e.IdRol)
                    .HasColumnName("id_rol");

                entity.Property(e => e.Activo)
                    .HasColumnName("activo");
            });
        }
    }
}