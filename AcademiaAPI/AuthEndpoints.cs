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
                        app.Logger.LogWarning($"Login failed for user: {request.Username}");
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
        }
    }
}