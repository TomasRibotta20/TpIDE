using Microsoft.Data.SqlClient;
using Domain.Model;
using System.Security.Cryptography;
using System.Text;

namespace AcademiaAPI
{
    public static class DatabaseSetupHelper
    {
        private const string ConnectionString = "Server=localhost,1433;Database=Universidad;User Id=sa;Password=TuContraseñaFuerte123;TrustServerCertificate=True";
        
        public static async Task EnsureAdminUserExistsAsync()
        {
            try
            {
                Console.WriteLine("=== DATABASE SETUP - ENSURING ADMIN USER ===");
                
                using var connection = new SqlConnection(ConnectionString);
                await connection.OpenAsync();
                
                // Check if the Usuarios table exists and has the correct schema
                var checkTableQuery = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Usuarios' 
                    AND COLUMN_NAME IN ('PasswordHash', 'Salt')";
                
                using var checkCommand = new SqlCommand(checkTableQuery, connection);
                var schemaColumnsCount = (int)await checkCommand.ExecuteScalarAsync();
                
                Console.WriteLine($"Schema columns found: {schemaColumnsCount}");
                
                if (schemaColumnsCount < 2)
                {
                    Console.WriteLine("Database schema is missing PasswordHash/Salt columns. Adding them...");
                    
                    try
                    {
                        var addColumnsQuery = @"
                            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Usuarios' AND COLUMN_NAME = 'PasswordHash')
                            BEGIN
                                ALTER TABLE Usuarios ADD PasswordHash NVARCHAR(255) NOT NULL DEFAULT ''
                            END
                            
                            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Usuarios' AND COLUMN_NAME = 'Salt')
                            BEGIN
                                ALTER TABLE Usuarios ADD Salt NVARCHAR(255) NOT NULL DEFAULT ''
                            END";
                        
                        using var alterCommand = new SqlCommand(addColumnsQuery, connection);
                        await alterCommand.ExecuteNonQueryAsync();
                        Console.WriteLine("Added missing columns to Usuarios table.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Could not add columns: {ex.Message}");
                        return;
                    }
                }
                
                // Check if admin user exists
                var checkUserQuery = "SELECT COUNT(*) FROM Usuarios WHERE UsuarioNombre = 'admin'";
                using var userCommand = new SqlCommand(checkUserQuery, connection);
                var userExists = (int)await userCommand.ExecuteScalarAsync() > 0;
                
                Console.WriteLine($"Admin user exists: {userExists}");
                
                if (!userExists)
                {
                    Console.WriteLine("Creating admin user...");
                    
                    // Create admin user using the Entity class to ensure consistency
                    var adminUser = new Usuario("Admin", "User", "admin", "admin@tpi.com", "admin123", true);
                    
                    var insertUserQuery = @"
                        INSERT INTO Usuarios (Nombre, Apellido, UsuarioNombre, Email, PasswordHash, Salt, Habilitado)
                        VALUES (@Nombre, @Apellido, @UsuarioNombre, @Email, @PasswordHash, @Salt, @Habilitado)";
                    
                    using var insertCommand = new SqlCommand(insertUserQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Nombre", adminUser.Nombre);
                    insertCommand.Parameters.AddWithValue("@Apellido", adminUser.Apellido);
                    insertCommand.Parameters.AddWithValue("@UsuarioNombre", adminUser.UsuarioNombre);
                    insertCommand.Parameters.AddWithValue("@Email", adminUser.Email);
                    insertCommand.Parameters.AddWithValue("@PasswordHash", adminUser.PasswordHash);
                    insertCommand.Parameters.AddWithValue("@Salt", adminUser.Salt);
                    insertCommand.Parameters.AddWithValue("@Habilitado", adminUser.Habilitado);
                    
                    await insertCommand.ExecuteNonQueryAsync();
                    Console.WriteLine("Admin user created successfully.");
                    Console.WriteLine($"  Username: {adminUser.UsuarioNombre}");
                    Console.WriteLine($"  Email: {adminUser.Email}");
                    Console.WriteLine($"  Password: admin123");
                    
                    // Test password validation immediately
                    bool testValidation = adminUser.ValidatePassword("admin123");
                    Console.WriteLine($"  Password validation test: {testValidation}");
                }
                else
                {
                    Console.WriteLine("Admin user already exists.");
                    
                    // Let's verify the existing admin user can validate password correctly
                    var getUserQuery = @"
                        SELECT Id, Nombre, Apellido, UsuarioNombre, Email, PasswordHash, Salt, Habilitado 
                        FROM Usuarios WHERE UsuarioNombre = 'admin'";
                    
                    using var getUserCommand = new SqlCommand(getUserQuery, connection);
                    using var reader = await getUserCommand.ExecuteReaderAsync();
                    
                    if (await reader.ReadAsync())
                    {
                        Console.WriteLine("Found existing admin user:");
                        Console.WriteLine($"  ID: {reader["Id"]}");
                        Console.WriteLine($"  Username: {reader["UsuarioNombre"]}");
                        Console.WriteLine($"  Email: {reader["Email"]}");
                        Console.WriteLine($"  Enabled: {reader["Habilitado"]}");
                        
                        // Note: We cannot test password validation here because we need to construct 
                        // the Usuario object properly, which requires the constructor that sets up validation
                    }
                }
                
                Console.WriteLine("=== DATABASE SETUP COMPLETED ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in database setup: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}