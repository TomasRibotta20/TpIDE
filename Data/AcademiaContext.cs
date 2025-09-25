using Microsoft.EntityFrameworkCore;
using Domain.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Data
{
    internal class AcademiaContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                optionsBuilder.UseSqlServer(
                    "Server=localhost,1433;Database=Universidad;User Id=sa;Password=TuContraseñaFuerte123;TrustServerCertificate=True"
                );

                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            }
        }

        internal AcademiaContext()
        {
            try 
            {
                Console.WriteLine("Initializing database...");
                
                // Check if database exists and create it if not
                bool databaseCreated = this.Database.EnsureCreated();
                if (databaseCreated)
                {
                    Console.WriteLine("Database created successfully.");
                }
                else
                {
                    Console.WriteLine("Database already exists.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Especialidad>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);

                // Datos iniciales para especialidades
                entity.HasData(
                    new { Id = 1, Descripcion = "Artes" },
                    new { Id = 2, Descripcion = "Humanidades" },
                    new { Id = 3, Descripcion = "Tecnico Electronico" }
                );
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(u => u.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(u => u.Apellido)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(u => u.UsuarioNombre)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(u => u.Habilitado)
                    .IsRequired();
                
                // Restricciones únicas
                entity.HasIndex(e => e.UsuarioNombre)
                    .IsUnique();
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                // No seeding automático de usuario admin - se manejará con DatabaseSetupHelper
            });
        }
    }
}
