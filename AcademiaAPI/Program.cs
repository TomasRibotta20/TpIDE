// Proyecto: AcademiaAPI
// Archivo: Program.cs (Revisado y Corregido)

using Aplication.Services;
using DTOs;
using AcademiaAPI;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; // Necesario para AddDbContext

Console.WriteLine("Starting API server...");

var builder = WebApplication.CreateBuilder(args);

// --- Configuración de Servicios (Inyección de Dependencias) ---

// 1. Obtener Cadena de Conexión (Asegúrate que esté en appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("AcademiaDBConnection");

// Validar que la cadena de conexión no sea null o vacía
if (string.IsNullOrEmpty(connectionString))
{
    // Lanzar excepción o usar un valor por defecto si es apropiado para desarrollo
    // Es crucial que esta cadena sea correcta.
    throw new InvalidOperationException("La cadena de conexión 'AcademiaDBConnection' no se encontró o está vacía en appsettings.json.");
}

// 2. Registrar DbContext (Entity Framework)
// Usar AddDbContext para que EF Core funcione correctamente con la cadena de conexión
builder.Services.AddDbContext<AcademiaContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Registrar Repositorios y Servicios (Usando Inyección de Dependencias - BUENA PRÁCTICA)
// Repositorios que usan EF Core (se pueden registrar directamente o a través de interfaces)
builder.Services.AddScoped<PlanRepository>(); // Asume que PlanRepository usa AcademiaContext inyectado
builder.Services.AddScoped<ComisionRepository>();
builder.Services.AddScoped<EspecialidadRepository>();
builder.Services.AddScoped<UsuarioRepository>();
// ... Registra otros repositorios de EF Core ...

// Repositorio de Materias (ADO.NET) - Inyectar la cadena de conexión
builder.Services.AddScoped<MateriaRepository>(provider =>
    new MateriaRepository(connectionString)); // Pasa la cadena al constructor

// Servicios (Inyectar sus dependencias de repositorios)
builder.Services.AddScoped<PlanService>(); // Asume que PlanService recibe PlanRepository en el constructor
builder.Services.AddScoped<ComisionService>();
builder.Services.AddScoped<EspecialidadService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<MateriaService>(); // Asume que recibe MateriaRepository y PlanService
// ... Registra otros servicios ...


// Servicios estándar de ASP.NET Core
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(o => { });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// --- Construcción de la Aplicación ---
var app = builder.Build();

Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

// --- Configuración de Base de Datos y Datos Iniciales ---
// Comentado temporalmente para evitar error de creación de tablas existentes
// Console.WriteLine("Ensuring database is up to date...");
// MigrationHelper.EnsureDatabaseUpdated(); // COMENTADO

Console.WriteLine("Ensuring admin user exists...");
await AdminUserRepairHelper.RepairAdminUserAsync();
await DatabaseSetupHelper.EnsureAdminUserExistsAsync(); // Necesario para el login inicial

// Insertar materias de prueba
Console.WriteLine("Ensuring test materias exist...");
await MateriaTestHelper.InsertTestMateriasAsync(connectionString);

// --- Configuración del Pipeline HTTP ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Habilitar la UI de Swagger
    app.UseHttpLogging();

    // Endpoints de prueba y salud (opcional)
    app.MapGet("/test/login", async () => { /* ... */ }).WithTags("Testing");
    app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow })).WithTags("Health");
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Habilitar CORS

// --- Mapeo de Endpoints ---
// Mapear todos los endpoints existentes
app.MapAuthEndpoints();
app.MapEspecialidadEndpoints();
app.MapUsuariosEndpoints();
app.MapPlanEndpoints();
app.MapComisionesEndpoints();
app.MapPersonasEndpoints();
app.MapCursosEndpoints();
app.MapInscripcionesEndpoints();
app.MapMateriaEndpoints(); // Asegúrate de que este mapeo esté presente y correcto

// --- Ejecutar la Aplicación ---
app.Run();