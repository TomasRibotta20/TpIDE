using Application.Services;
using DTOs;

namespace AcademiaAPI
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var authService = new AuthService(app.Configuration);

            app.MapPost("/auth/login", async (LoginRequest request) =>
            {
                try
                {
                    app.Logger.LogInformation($"Login attempt for username: {request.Username}");
                    
                    var response = await authService.LoginAsync(request);
                    if (response != null)
                    {
                        app.Logger.LogInformation($"Login successful for user: {request.Username}");
                        return Results.Ok(response);
                    }
                    else
                    {
                        app.Logger.LogWarning($"Login failed for user: {request.Username} - Invalid credentials");
                        
                        // Checking if user exists but password is wrong
                        var usuarioRepo = new Data.UsuarioRepository();
                        var user = usuarioRepo.GetByUsername(request.Username);
                        if (user != null)
                        {
                            app.Logger.LogWarning($"User {request.Username} exists but password validation failed");
                            
                            // For debugging purposes, log password hash details
                            bool passwordCheck = user.ValidatePassword(request.Password);
                            app.Logger.LogWarning($"Password validation result: {passwordCheck}");
                        }
                        else
                        {
                            app.Logger.LogWarning($"User {request.Username} does not exist in database");
                        }
                        
                        return Results.Unauthorized();
                    }
                }
                catch (Exception ex)
                {
                    app.Logger.LogError(ex, $"Error during login for user: {request.Username}");
                    return Results.BadRequest($"Error en el login: {ex.Message}");
                }
            })
            .WithName("Login")
            .WithTags("Authentication")
            .WithOpenApi();

            app.MapPost("/auth/validate", (ValidateTokenRequest request) =>
            {
                try
                {
                    var isValid = authService.ValidateToken(request.Token);
                    return isValid ? Results.Ok(new { Valid = true }) : Results.Unauthorized();
                }
                catch (Exception ex)
                {
                    app.Logger.LogError(ex, "Error validating token");
                    return Results.BadRequest($"Error validating token: {ex.Message}");
                }
            })
            .WithName("ValidateToken")
            .WithTags("Authentication")
            .WithOpenApi();
            
            // Add debug endpoint to check if admin user exists and is configured correctly
            if (app.Environment.IsDevelopment())
            {
                app.MapGet("/auth/check-admin", () => 
                {
                    try 
                    {
                        var usuarioRepo = new Data.UsuarioRepository();
                        var adminUser = usuarioRepo.GetByUsername("admin");
                        
                        if (adminUser == null)
                        {
                            return Results.NotFound("Admin user not found in database");
                        }
                        
                        bool canValidate = adminUser.ValidatePassword("admin123");
                        
                        return Results.Ok(new 
                        {
                            exists = true,
                            username = adminUser.UsuarioNombre,
                            email = adminUser.Email,
                            canValidateWithDefaultPassword = canValidate,
                            passwordHashLength = adminUser.PasswordHash?.Length ?? 0,
                            saltLength = adminUser.Salt?.Length ?? 0
                        });
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem($"Error checking admin user: {ex.Message}");
                    }
                })
                .WithName("CheckAdminUser")
                .WithTags("Debugging")
                .WithOpenApi();
            }
        }
    }
}