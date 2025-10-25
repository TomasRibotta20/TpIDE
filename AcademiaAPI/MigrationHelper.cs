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
        public static void EnsureDatabaseUpdated(bool forceRecreate = false)
        {
            try
            {
                using var context = new AcademiaContext();
                
                Console.WriteLine("=== DATABASE MIGRATION CHECK ===");
                
                // Verificar si la base de datos puede conectarse
                bool canConnect = context.Database.CanConnect();
                Console.WriteLine($"Can connect to database: {canConnect}");
                
                if (forceRecreate && canConnect)
                {
                    Console.WriteLine("FORCE RECREATE enabled - Dropping database...");
                    context.Database.EnsureDeleted();
                    Console.WriteLine("Database deleted successfully.");
                    canConnect = false;
                }
                
                if (!canConnect)
                {
                    Console.WriteLine("Database does not exist. Creating database...");
                    context.Database.Migrate();
                    Console.WriteLine("Database created successfully!");
                    return;
                }
                
                // Verificar si las tablas existen
                bool tablesExist = VerifyTablesExist(context);
                
                if (!tablesExist)
                {
                    Console.WriteLine("Tables do not exist. Applying migrations...");
                    context.Database.Migrate();
                    Console.WriteLine("Migrations applied successfully!");
                    return;
                }
                
                // Si las tablas existen, verificar migraciones
                var pendingMigrations = context.Database.GetPendingMigrations().ToList();
                var appliedMigrations = context.Database.GetAppliedMigrations().ToList();
                
                Console.WriteLine($"Applied migrations: {appliedMigrations.Count}");
                if (appliedMigrations.Any())
                {
                    foreach (var migration in appliedMigrations)
                    {
                        Console.WriteLine($"  - {migration}");
                    }
                }
                
                Console.WriteLine($"Pending migrations: {pendingMigrations.Count}");
                
                if (pendingMigrations.Any())
                {
                    // Si hay migraciones pendientes pero las tablas ya existen,
                    // es probable que la base de datos se creó manualmente
                    if (appliedMigrations.Count == 0 && tablesExist)
                    {
                        Console.WriteLine("WARNING: Database exists but no migrations are recorded.");
                        Console.WriteLine("This typically happens when the database was created manually.");
                        Console.WriteLine("Skipping automatic migration to preserve existing data.");
                        Console.WriteLine("\nTo fix this issue:");
                        Console.WriteLine("  Option 1 (Recommended): Delete the database from SSMS and restart the API");
                        Console.WriteLine("  Option 2: Set FORCE_RECREATE_DB=true in environment variables");
                        Console.WriteLine("  Option 3: Manually register the migrations in __EFMigrationsHistory table");
                    }
                    else
                    {
                        Console.WriteLine("Applying pending migrations...");
                        foreach (var migration in pendingMigrations)
                        {
                            Console.WriteLine($"  - {migration}");
                        }
                        
                        try
                        {
                            context.Database.Migrate();
                            Console.WriteLine("All migrations applied successfully!");
                        }
                        catch (Exception migrationEx)
                        {
                            Console.WriteLine($"ERROR applying migrations: {migrationEx.Message}");
                            
                            if (migrationEx.Message.Contains("IDENTITY"))
                            {
                                Console.WriteLine("\n??  IDENTITY COLUMN ERROR DETECTED ??");
                                Console.WriteLine("This error occurs when trying to change IDENTITY properties on existing columns.");
                                Console.WriteLine("\nQUICK FIX:");
                                Console.WriteLine("1. Open SQL Server Management Studio (SSMS)");
                                Console.WriteLine("2. Delete the 'Universidad' database");
                                Console.WriteLine("3. Restart this API (F5)");
                                Console.WriteLine("\nThe database will be recreated automatically with the correct structure.");
                            }
                            
                            Console.WriteLine("\nContinuing with existing database structure...");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Database is already up to date!");
                }
                
                // Verificar que las tablas clave existan
                VerifyRequiredTables(context);
                
                Console.WriteLine("=== DATABASE READY ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring database is updated: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Mostrar más detalles si es un error de conexión
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                
                throw; // Re-throw para que el error sea visible en el inicio de la aplicación
            }
        }
        
        private static bool VerifyTablesExist(AcademiaContext context)
        {
            try
            {
                // Intentar hacer una query simple a una tabla clave
                context.Usuarios.Any();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        private static void VerifyRequiredTables(AcademiaContext context)
        {
            try
            {
                Console.WriteLine("\nVerifying database tables:");
                
                // Verificar tablas principales
                var usuariosCount = context.Usuarios.Count();
                Console.WriteLine($"  - Usuarios table exists ({usuariosCount} records)");
                
                var especialidadesCount = context.Especialidades.Count();
                Console.WriteLine($"  - Especialidades table exists ({especialidadesCount} records)");
                
                var planesCount = context.Planes.Count();
                Console.WriteLine($"  - Planes table exists ({planesCount} records)");
                
                var comisionesCount = context.Comisiones.Count();
                Console.WriteLine($"  - Comisiones table exists ({comisionesCount} records)");
                
                var personasCount = context.Personas.Count();
                Console.WriteLine($"  - Personas table exists ({personasCount} records)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Warning verifying tables: {ex.Message}");
            }
        }
    }
}
