using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Linq;

namespace AcademiaAPI
{
    public static class MigrationHelper
    {
        public static void EnsureDatabaseUpdated()
        {
            try
            {
                using var context = new AcademiaContext();
                
                Console.WriteLine("=== DATABASE MIGRATION CHECK ===");
                
                // Verificar si la base de datos puede conectarse
                bool canConnect = context.Database.CanConnect();
                Console.WriteLine($"Can connect to database: {canConnect}");
                
                if (!canConnect)
                {
                    Console.WriteLine("Database does not exist. Creating database...");
                }
                
                // Obtener migraciones pendientes y aplicadas
                var pendingMigrations = context.Database.GetPendingMigrations().ToList();
                var appliedMigrations = context.Database.GetAppliedMigrations().ToList();
                
                Console.WriteLine($"Applied migrations: {appliedMigrations.Count}");
                if (appliedMigrations.Any())
                {
                    foreach (var migration in appliedMigrations)
                    {
                        Console.WriteLine($"  ? {migration}");
                    }
                }
                
                Console.WriteLine($"Pending migrations: {pendingMigrations.Count}");
                if (pendingMigrations.Any())
                {
                    Console.WriteLine("Applying pending migrations...");
                    foreach (var migration in pendingMigrations)
                    {
                        Console.WriteLine($"  ? {migration}");
                    }
                    
                    // Aplicar migraciones automáticamente
                    // Esto preserva los datos existentes y solo aplica los cambios necesarios
                    context.Database.Migrate();
                    Console.WriteLine("? All migrations applied successfully!");
                    Console.WriteLine("? Database structure is up to date!");
                }
                else
                {
                    Console.WriteLine("? Database is already up to date!");
                }
                
                // Verificar que las tablas clave existan
                VerifyRequiredTables(context);
                
                Console.WriteLine("=== DATABASE READY ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error ensuring database is updated: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Mostrar más detalles si es un error de conexión
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                
                throw; // Re-throw para que el error sea visible en el inicio de la aplicación
            }
        }
        
        private static void VerifyRequiredTables(AcademiaContext context)
        {
            try
            {
                Console.WriteLine("\nVerifying database tables:");
                
                // Verificar tablas principales
                var usuariosCount = context.Usuarios.Count();
                Console.WriteLine($"  ? Usuarios table exists ({usuariosCount} records)");
                
                var especialidadesCount = context.Especialidades.Count();
                Console.WriteLine($"  ? Especialidades table exists ({especialidadesCount} records)");
                
                var planesCount = context.Planes.Count();
                Console.WriteLine($"  ? Planes table exists ({planesCount} records)");
                
                var comisionesCount = context.Comisiones.Count();
                Console.WriteLine($"  ? Comisiones table exists ({comisionesCount} records)");
                
                var personasCount = context.Personas.Count();
                Console.WriteLine($"  ? Personas table exists ({personasCount} records)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ? Warning verifying tables: {ex.Message}");
            }
        }
    }
}
