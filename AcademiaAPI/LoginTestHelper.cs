using Application.Services;
using Data;
using DTOs;

namespace AcademiaAPI
{
    public static class LoginTestHelper
    {
        public static async Task TestLoginAsync(IConfiguration configuration)
        {
            try
            {
                Console.WriteLine("=== TESTING DATABASE CONNECTION ===");
                var usuarioRepo = new UsuarioRepository();
                var users = usuarioRepo.GetAll();
                Console.WriteLine($"Found {users.Count()} users in database");
                
                foreach (var user in users)
                {
                    Console.WriteLine($"User: ID={user.Id}, Username='{user.UsuarioNombre}', Email='{user.Email}', Enabled={user.Habilitado}");
                    var hashPreview = user.PasswordHash.Length > 10 ? user.PasswordHash[..10] + "..." : user.PasswordHash;
                    var saltPreview = user.Salt.Length > 10 ? user.Salt[..10] + "..." : user.Salt;
                    Console.WriteLine($"      PasswordHash='{hashPreview}' (length: {user.PasswordHash.Length})");
                    Console.WriteLine($"      Salt='{saltPreview}' (length: {user.Salt.Length})");
                }

                Console.WriteLine("\n=== TESTING USER LOOKUP ===");
                var adminUser = usuarioRepo.GetByUsername("admin");
                if (adminUser != null)
                {
                    Console.WriteLine($"✓ Found admin user: ID={adminUser.Id}, Username='{adminUser.UsuarioNombre}'");
                    
                    Console.WriteLine("\n=== TESTING PASSWORD VALIDATION ===");
                    
                    // Test the correct password
                    bool passwordValid = adminUser.ValidatePassword("admin123");
                    Console.WriteLine($"Password 'admin123' validation: {passwordValid} {(passwordValid ? "✓" : "✗")}");
                    
                    // Test wrong passwords
                    Console.WriteLine($"Password 'admin' validation: {adminUser.ValidatePassword("admin")} ✗");
                    Console.WriteLine($"Password 'Admin123' validation: {adminUser.ValidatePassword("Admin123")} ✗");
                    Console.WriteLine($"Password '' validation: {adminUser.ValidatePassword("")} ✗");
                    
                    if (!passwordValid)
                    {
                        Console.WriteLine("⚠️ WARNING: Admin password validation failed!");
                        Console.WriteLine("This indicates a problem with password hashing or storage.");
                    }
                }
                else
                {
                    Console.WriteLine("✗ Admin user NOT FOUND with username 'admin'");
                }

                Console.WriteLine("\n=== TESTING AUTHENTICATION SERVICE ===");
                var authService = new AuthService(configuration);
                
                var loginRequest = new LoginRequest 
                { 
                    Username = "admin", 
                    Password = "admin123" 
                };
                
                Console.WriteLine($"Attempting login with username: '{loginRequest.Username}' and password: '{loginRequest.Password}'");
                var loginResponse = await authService.LoginAsync(loginRequest);
                
                if (loginResponse != null)
                {
                    Console.WriteLine("✓ LOGIN SUCCESSFUL!");
                    var tokenPreview = loginResponse.Token.Length > 50 ? loginResponse.Token[..50] + "..." : loginResponse.Token;
                    Console.WriteLine($"Token: {tokenPreview}");
                    Console.WriteLine($"Expires: {loginResponse.ExpiresAt}");
                    Console.WriteLine($"Username: {loginResponse.Username}");
                }
                else
                {
                    Console.WriteLine("✗ LOGIN FAILED!");
                    Console.WriteLine("This could be due to:");
                    Console.WriteLine("  - User not found");
                    Console.WriteLine("  - Password validation failure");
                    Console.WriteLine("  - User disabled");
                    Console.WriteLine("  - Authentication service configuration issue");
                }
                
                Console.WriteLine("\n=== TEST COMPLETE ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Test failed with error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}