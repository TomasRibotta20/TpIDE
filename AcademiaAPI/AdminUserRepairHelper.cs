using Microsoft.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using Domain.Model;

namespace AcademiaAPI
{
    public static class AdminUserRepairHelper
    {
        private const string ConnectionString = "Server=localhost,1433;Database=Universidad;User Id=sa;Password=TuContraseñaFuerte123;TrustServerCertificate=True";
        
        public static async Task RepairAdminUserAsync()
        {
            Console.WriteLine("==== STARTING ADMIN USER REPAIR ====");
            
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                await connection.OpenAsync();
                
                // Usar la misma implementación de hash que la clase Usuario
                // para garantizar consistencia
                var adminUser = new Usuario("Admin", "User", "admin", "admin@tpi.com", "admin123", true);
                string salt = adminUser.Salt;
                string passwordHash = adminUser.PasswordHash;
                
                Console.WriteLine($"Generated new credentials for admin user:");
                Console.WriteLine($"Username: admin");
                Console.WriteLine($"Password: admin123");
                Console.WriteLine($"Salt: {salt}");
                Console.WriteLine($"PasswordHash: {passwordHash}");
                
                // Verificar si el usuario admin existe
                var checkQuery = "SELECT COUNT(*) FROM Usuarios WHERE UsuarioNombre = 'admin'";
                using var checkCommand = new SqlCommand(checkQuery, connection);
                int userCount = (int)await checkCommand.ExecuteScalarAsync();
                
                if (userCount > 0)
                {
                    // Actualizar el usuario existente
                    var updateQuery = @"
                        UPDATE Usuarios 
                        SET PasswordHash = @PasswordHash, 
                            Salt = @Salt,
                            Nombre = 'Admin',
                            Apellido = 'User',
                            Email = 'admin@tpi.com',
                            Habilitado = 1
                        WHERE UsuarioNombre = 'admin'";
                    
                    using var updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    updateCommand.Parameters.AddWithValue("@Salt", salt);
                    
                    int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                    Console.WriteLine($"Updated {rowsAffected} rows - admin user repaired successfully");
                }
                else
                {
                    // Crear el usuario si no existe
                    var insertQuery = @"
                        INSERT INTO Usuarios (Nombre, Apellido, UsuarioNombre, Email, PasswordHash, Salt, Habilitado)
                        VALUES ('Admin', 'User', 'admin', 'admin@tpi.com', @PasswordHash, @Salt, 1)";
                    
                    using var insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    insertCommand.Parameters.AddWithValue("@Salt", salt);
                    
                    int rowsAffected = await insertCommand.ExecuteNonQueryAsync();
                    Console.WriteLine($"Created new admin user ({rowsAffected} rows affected)");
                }
                
                // Validación
                var validateQuery = @"
                    SELECT Id, Nombre, Apellido, UsuarioNombre, Email, PasswordHash, Salt, Habilitado
                    FROM Usuarios WHERE UsuarioNombre = 'admin'";
                
                using var validateCommand = new SqlCommand(validateQuery, connection);
                using var reader = await validateCommand.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    // Obtener datos del usuario
                    var id = (int)reader["Id"];
                    var nombre = (string)reader["Nombre"];
                    var apellido = (string)reader["Apellido"];
                    var usuario = (string)reader["UsuarioNombre"];
                    var email = (string)reader["Email"];
                    var storedHash = (string)reader["PasswordHash"];
                    var storedSalt = (string)reader["Salt"];
                    var habilitado = (bool)reader["Habilitado"];
                    
                    Console.WriteLine($"Validating user from database:");
                    Console.WriteLine($"ID: {id}, Username: {usuario}, Email: {email}, Enabled: {habilitado}");
                    
                    // No podemos crear una instancia de Usuario con hash y salt directamente
                    // Pero podemos verificar si el hash coincide manualmente
                    var testHash = HashPassword("admin123", storedSalt);
                    var passwordValid = testHash == storedHash;
                    
                    Console.WriteLine($"Manual password validation test: {(passwordValid ? "PASSED" : "FAILED")}");
                    Console.WriteLine($"Stored hash: {storedHash}");
                    Console.WriteLine($"Test hash: {testHash}");
                }
                
                Console.WriteLine("==== ADMIN USER REPAIR COMPLETED ====");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR repairing admin user: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
        
        private static string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] saltBytes = new byte[32];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}