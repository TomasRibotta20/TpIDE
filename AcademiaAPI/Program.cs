using Aplication.Services;
using DTOs;
using AcademiaAPI;
using Data;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Starting API server...");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(o => { });

// Register MateriaRepository with hardcoded connection string (same as AcademiaContext)
builder.Services.AddScoped(provider => 
    new MateriaRepository("Server=localhost,1433;Database=Universidad;User Id=sa;Password=TuContraseñaFuerte123;TrustServerCertificate=True"));

// Register PlanService for MateriaService dependency
builder.Services.AddScoped(provider => new PlanService());

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

// Check if force recreate is enabled via environment variable
bool forceRecreate = Environment.GetEnvironmentVariable("FORCE_RECREATE_DB") == "true";
if (forceRecreate)
{
    Console.WriteLine(">>> FORCE_RECREATE_DB is enabled - Database will be dropped and recreated!");
}

MigrationHelper.EnsureDatabaseUpdated(forceRecreate);

// Reparar específicamente el usuario admin para asegurar que funcione el login
Console.WriteLine("Ensuring admin user exists...");
await AdminUserRepairHelper.RepairAdminUserAsync();
await DatabaseSetupHelper.EnsureAdminUserExistsAsync();

// Inicializar módulos del sistema
Console.WriteLine("Initializing system modules...");
var usuarioService = new UsuarioService();
await usuarioService.InicializarModulosAsync();
Console.WriteLine(">>> System modules initialized");

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

// app.UseHttpsRedirection(); // Comentado para desarrollo HTTP

// Use CORS
app.UseCors("AllowAll");

// Map endpoints
app.MapAuthEndpoints();
app.MapEspecialidadEndpoints();
app.MapUsuariosEndpoints();
app.MapPlanEndpoints();
app.MapMateriaEndpoints();
app.MapComisionesEndpoints();
app.MapPersonasEndpoints();
app.MapCursosEndpoints();
app.MapInscripcionesEndpoints();
app.MapDocenteCursoEndpoints();

app.Run();
