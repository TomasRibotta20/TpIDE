using Microsoft.EntityFrameworkCore;
using Domain.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Logging;
using System;

namespace Data
{
    public class AcademiaContext : DbContext
    {
        private static string DefaultConnectionString = "Server=localhost,1433;Database=Universidad;User Id=sa;Password=TuContraseñaFuerte123;TrustServerCertificate=True";

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Plan> Planes { get; set; }
        public DbSet<Comision> Comisiones { get; set; }
        public DbSet<Persona> Personas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = DefaultConnectionString;

                try
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .Build();

                    connectionString = configuration.GetConnectionString("DefaultConnection") ?? DefaultConnectionString;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading configuration: {ex.Message}. Using default connection string.");
                }

                optionsBuilder.UseSqlServer(connectionString);
                
                // Solo habilitar logging detallado en desarrollo
                #if DEBUG
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
                optionsBuilder.EnableSensitiveDataLogging();
                #endif
            }
        }

        public AcademiaContext()
        {
            // Constructor vacío - la creación/migración de la BD se maneja desde MigrationHelper
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Especialidad
            modelBuilder.Entity<Especialidad>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(50);

                // Datos iniciales
                entity.HasData(
                    new { Id = 1, Descripcion = "Artes" },
                    new { Id = 2, Descripcion = "Humanidades" },
                    new { Id = 3, Descripcion = "Tecnico Electronico" }
                );
            });

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(u => u.UsuarioNombre).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Salt).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Habilitado).IsRequired();

                // Índices únicos
                entity.HasIndex(e => e.UsuarioNombre).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuración de Plan
            modelBuilder.Entity<Plan>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.Descripcion).IsRequired().HasMaxLength(50);
                entity.Property(p => p.EspecialidadId).IsRequired();
                
                entity.HasOne<Especialidad>()
                    .WithMany()
                    .HasForeignKey(p => p.EspecialidadId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Comisión
            modelBuilder.Entity<Comision>(entity =>
            {
                entity.ToTable("Comisiones", schema: "dbo");
                entity.HasKey(c => c.IdComision).HasName("PK_Comisiones");
                
                entity.Property(c => c.IdComision)
                    .HasColumnName("IdComision")
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(c => c.DescComision)
                    .HasColumnName("DescComision")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(c => c.AnioEspecialidad)
                    .HasColumnName("AnioEspecialidad")
                    .IsRequired();

                entity.Property(c => c.IdPlan)
                    .HasColumnName("IdPlan")
                    .IsRequired();

                entity.HasOne<Plan>()
                    .WithMany()
                    .HasForeignKey(c => c.IdPlan)
                    .HasConstraintName("FK_Comisiones_Planes")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Persona (CORREGIDA PARA COINCIDIR CON BD EXISTENTE)
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.ToTable("personas");
                entity.HasKey(p => p.Id);
                
                entity.Property(p => p.Id)
                    .HasColumnName("id_persona")
                    .ValueGeneratedOnAdd();

                entity.Property(p => p.Nombre)
                    .HasColumnName("nombre")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(p => p.Apellido)
                    .HasColumnName("apellido")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(p => p.Direccion)
                    .HasColumnName("direccion")
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(p => p.Email)
                    .HasColumnName("email")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(p => p.Telefono)
                    .HasColumnName("telefono")
                    .IsRequired(false)
                    .HasMaxLength(20);

                entity.Property(p => p.FechaNacimiento)
                    .HasColumnName("fecha_nac")
                    .IsRequired();

                entity.Property(p => p.Legajo)
                    .HasColumnName("legajo")
                    .IsRequired();

                entity.Property(p => p.TipoPersona)
                    .HasColumnName("tipo_persona")
                    .IsRequired()
                    .HasConversion<string>();

                entity.Property(p => p.IdPlan)
                    .HasColumnName("id_plan");

                entity.HasOne<Plan>()
                    .WithMany()
                    .HasForeignKey(p => p.IdPlan)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}