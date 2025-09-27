using Microsoft.EntityFrameworkCore;
using Domain.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class AcademiaContext : DbContext
    {
        private static string DefaultConnectionString = "Server=localhost,1433;Database=Universidad;User Id=sa;Password=TuContraseñaFuerte123;TrustServerCertificate=True";
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Plan> Planes { get; set; }
        public DbSet<Comision> Comisiones { get; set; }

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
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
                optionsBuilder.EnableSensitiveDataLogging(); // This helps with debugging
            }
        }

        public AcademiaContext()
        {
            try 
            {
                Console.WriteLine("Initializing database...");
                
                // Force database creation if it doesn't exist
                bool databaseCreated = this.Database.EnsureCreated();
                
                if (databaseCreated)
                {
                    Console.WriteLine("Database created successfully.");
                    
                    // If we just created the database, let's verify the Comisiones table exists
                    try
                    {
                        var count = Comisiones.Count();
                        Console.WriteLine($"Comisiones table verified. Current count: {count}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error verifying Comisiones table: {ex.Message}");
                        
                        // Try to manually create the Comisiones table
                        try
                        {
                            Database.ExecuteSqlRaw(@"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Comisiones')
                            BEGIN
                                CREATE TABLE [dbo].[Comisiones](
                                    [IdComision] [int] IDENTITY(1,1) NOT NULL,
                                    [DescComision] [nvarchar](50) NOT NULL,
                                    [AnioEspecialidad] [int] NOT NULL,
                                    [IdPlan] [int] NOT NULL,
                                    CONSTRAINT [PK_Comisiones] PRIMARY KEY CLUSTERED ([IdComision] ASC),
                                    CONSTRAINT [FK_Comisiones_Planes] FOREIGN KEY([IdPlan]) REFERENCES [dbo].[Planes] ([Id])
                                );
                            END");
                            Console.WriteLine("Manually created Comisiones table");
                        }
                        catch (Exception innerEx)
                        {
                            Console.WriteLine($"Failed to manually create Comisiones table: {innerEx.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Database already exists, schema checked.");
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
            });

            modelBuilder.Entity<Plan>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(p => p.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(p => p.EspecialidadId)
                    .IsRequired();
                entity.HasOne<Especialidad>()
                    .WithMany()
                    .HasForeignKey(p => p.EspecialidadId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Comision>(entity =>
            {
                // Explicit table name
                entity.ToTable("Comisiones", schema: "dbo");
                
                // Primary key
                entity.HasKey(c => c.IdComision)
                    .HasName("PK_Comisiones");
                
                // Auto-increment primary key
                entity.Property(c => c.IdComision)
                    .HasColumnName("IdComision")
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                
                // Other properties
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
                
                // Relationship
                entity.HasOne<Plan>()
                    .WithMany()
                    .HasForeignKey(c => c.IdPlan)
                    .HasConstraintName("FK_Comisiones_Planes")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
