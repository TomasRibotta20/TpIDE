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
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<ModulosUsuarios> ModulosUsuarios { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Plan> Planes { get; set; }
        public DbSet<Comision> Comisiones { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<AlumnoCurso> AlumnoCursos { get; set; }

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
                entity.ToTable("Usuarios");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(u => u.UsuarioNombre).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Salt).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Habilitado).IsRequired().HasDefaultValue(true);

                // Relación bidireccional con Persona (opcional)
                entity.HasOne(u => u.Persona)           // Un usuario tiene una persona
                      .WithMany(p => p.Usuarios)         // Una persona tiene muchos usuarios
                      .HasForeignKey(u => u.PersonaId)   // FK es PersonaId
                      .OnDelete(DeleteBehavior.SetNull)  // Si se elimina persona, PersonaId = null
                      .IsRequired(false);                // Relación opcional

                // Índices únicos
                entity.HasIndex(u => u.UsuarioNombre).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
            });

            // Configuración de Plan
            modelBuilder.Entity<Plan>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.Descripcion).IsRequired().HasMaxLength(50);
                entity.Property(p => p.EspecialidadId).IsRequired();
            });

            // Configuración de Comisión
            modelBuilder.Entity<Comision>(entity =>
            {
                entity.HasKey(c => c.IdComision);
                entity.Property(c => c.IdComision).ValueGeneratedOnAdd();
                entity.Property(c => c.DescComision).IsRequired().HasMaxLength(50);
                entity.Property(c => c.AnioEspecialidad).IsRequired();
                entity.Property(c => c.IdPlan).IsRequired();
            });

            // Configuración de Persona
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Email).IsRequired().HasMaxLength(100);
                entity.Property(p => p.TipoPersona).HasConversion<string>();
                entity.HasIndex(p => p.Email).IsUnique();
            });

            // Configuración de Curso
            modelBuilder.Entity<Curso>(entity =>
            {
                entity.HasKey(c => c.IdCurso);
                entity.Property(c => c.IdCurso).ValueGeneratedOnAdd();
                entity.Property(c => c.IdMateria).IsRequired(false); // Nullable temporalmente
                entity.Property(c => c.IdComision).IsRequired();
                entity.Property(c => c.AnioCalendario).IsRequired();
                entity.Property(c => c.Cupo).IsRequired();
            });

            // Configuración de AlumnoCurso
            modelBuilder.Entity<AlumnoCurso>(entity =>
            {
                entity.HasKey(ac => ac.IdInscripcion);
                entity.Property(ac => ac.IdInscripcion).ValueGeneratedOnAdd();
                entity.Property(ac => ac.IdAlumno).IsRequired();
                entity.Property(ac => ac.IdCurso).IsRequired();
                entity.Property(ac => ac.Condicion).HasConversion<string>();
                entity.Property(ac => ac.Nota).IsRequired(false);
                
                // Índice único para evitar inscripciones duplicadas
                entity.HasIndex(ac => new { ac.IdAlumno, ac.IdCurso }).IsUnique();
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.ToTable("Modulos");
                entity.HasKey(m => m.Id_Modulo);
                entity.Property(m => m.Id_Modulo).ValueGeneratedOnAdd();
                entity.Property(m => m.Desc_Modulo).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Ejecuta).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<ModulosUsuarios>(entity =>
            {
                entity.ToTable("ModulosUsuarios");
                entity.HasKey(mu => mu.Id_ModuloUsuario);
                entity.Property(mu => mu.Id_ModuloUsuario).ValueGeneratedOnAdd();
                entity.Property(mu => mu.UsuarioId).IsRequired();
                entity.Property(mu => mu.ModuloId).IsRequired();
                entity.Property(mu => mu.alta).IsRequired().HasDefaultValue(false);
                entity.Property(mu => mu.baja).IsRequired().HasDefaultValue(false);
                entity.Property(mu => mu.modificacion).IsRequired().HasDefaultValue(false);
                entity.Property(mu => mu.consulta).IsRequired().HasDefaultValue(false);

                // Relación con Usuario (uno-a-muchos)
                entity.HasOne(mu => mu.Usuario)
                    .WithMany(u => u.ModulosUsuarios)
                    .HasForeignKey(mu => mu.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con Modulo (uno-a-muchos)
                entity.HasOne(mu => mu.Modulo)
                    .WithMany(m => m.ModulosUsuarios)
                    .HasForeignKey(mu => mu.ModuloId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Índice único para evitar duplicados Usuario-Modulo
                entity.HasIndex(mu => new { mu.UsuarioId, mu.ModuloId }).IsUnique();
            });
        }
    }
}