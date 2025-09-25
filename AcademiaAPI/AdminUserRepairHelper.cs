using Microsoft.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

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
                
                // Generar nuevos valores para el usuario admin
                string username = "admin";
                string password = "admin123";
                string salt = GenerateSalt();
                string passwordHash = HashPassword(password, salt);
                
                Console.WriteLine($"Generated new credentials for admin user:");
                Console.WriteLine($"Username: {username}");
                Console.WriteLine($"Password: {password}");
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
                        SET PasswordHash = @PasswordHash, Salt = @Salt 
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
                    SELECT PasswordHash, Salt FROM Usuarios WHERE UsuarioNombre = 'admin'";
                
                using var validateCommand = new SqlCommand(validateQuery, connection);
                using var reader = await validateCommand.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    string storedHash = reader["PasswordHash"].ToString();
                    string storedSalt = reader["Salt"].ToString();
                    
                    // Validar que la contraseña admin123 funcione con el hash y salt almacenados
                    string testHash = HashPassword("admin123", storedSalt);
                    bool isValid = testHash == storedHash;
                    
                    Console.WriteLine($"Validation test: {(isValid ? "PASSED" : "FAILED")}");
                    Console.WriteLine($"Stored Hash: {storedHash}");
                    Console.WriteLine($"Computed Hash: {testHash}");
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