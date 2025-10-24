using Aplication.Services;
using DTOs;
using AcademiaAPI;
using Data;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Starting API server...");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(o => { });

// Add CORS for all origins (for development)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

// Asegurar que la base de datos esté actualizada (automático)
Console.WriteLine("Ensuring database is up to date...");
MigrationHelper.EnsureDatabaseUpdated();

// Reparar específicamente el usuario admin para asegurar que funcione el login
Console.WriteLine("Ensuring admin user exists...");
await AdminUserRepairHelper.RepairAdminUserAsync();
await DatabaseSetupHelper.EnsureAdminUserExistsAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpLogging();
    
    // Add test endpoint for debugging
    app.MapGet("/test/login", async () =>
    {
        await LoginTestHelper.TestLoginAsync(app.Configuration);
        return Results.Ok("Check console output for test results");
    })
    .WithName("TestLogin")
    .WithTags("Testing")
    .WithOpenApi();

    // Add a simple health check endpoint
    app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
        .WithName("HealthCheck")
        .WithTags("Health")
        .WithOpenApi();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

// Map endpoints
app.MapAuthEndpoints();
app.MapEspecialidadEndpoints();
app.MapUsuariosEndpoints();
app.MapPlanEndpoints();
app.MapComisionesEndpoints();
app.MapPersonasEndpoints();

app.Run();
