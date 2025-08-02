using Aplication.Services;
using DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<UsuarioService>();
builder.Services.AddSingleton<EspecialidadService>();

// Agregar CORS para permitir solicitudes desde el cliente Windows Forms
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

// Servicios
var usuarioService = app.Services.GetRequiredService<UsuarioService>();
var especialidadService = app.Services.GetRequiredService<EspecialidadService>();

// ENDPOINTS DE USUARIOS
// GET: Obtener todos los usuarios
app.MapGet("/usuarios", () => usuarioService.GetAll())
    .WithName("GetUsuarios")
    .WithOpenApi();

// GET: Obtener usuario por ID
app.MapGet("/usuarios/{id}", (int id) =>
{
    var usuario = usuarioService.GetById(id);
    return usuario == null ? Results.NotFound() : Results.Ok(usuario);
})
.WithName("GetUsuarioById")
.WithOpenApi();

// POST: Crear nuevo usuario
app.MapPost("/usuarios", (UsuarioDto usuario) =>
{
    usuarioService.Add(usuario);
    return Results.Created($"/usuarios/{usuario.Id}", usuario);
})
.WithName("CreateUsuario")
.WithOpenApi();

// PUT: Actualizar usuario
app.MapPut("/usuarios/{id}", (int id, UsuarioDto usuario) =>
{
    if (id != usuario.Id)
        return Results.BadRequest();

    var existingUsuario = usuarioService.GetById(id);
    if (existingUsuario == null)
        return Results.NotFound();

    usuarioService.Update(usuario);
    return Results.NoContent();
})
.WithName("UpdateUsuario")
.WithOpenApi();

// DELETE: Eliminar usuario
app.MapDelete("/usuarios/{id}", (int id) =>
{
    var usuario = usuarioService.GetById(id);
    if (usuario == null)
        return Results.NotFound();

    usuarioService.Delete(id);
    return Results.NoContent();
})
.WithName("DeleteUsuario")
.WithOpenApi();

// ENDPOINTS DE ESPECIALIDADES
// GET: Obtener todas las especialidades
app.MapGet("/especialidades", () => especialidadService.GetAll())
    .WithName("GetEspecialidades")
    .WithOpenApi();

// GET: Obtener especialidad por ID
app.MapGet("/especialidades/{id}", (int id) =>
{
    var especialidad = especialidadService.GetById(id);
    return especialidad == null ? Results.NotFound() : Results.Ok(especialidad);
})
.WithName("GetEspecialidadById")
.WithOpenApi();

// POST: Crear nueva especialidad
app.MapPost("/especialidades", (EspecialidadDto especialidad) =>
{
    especialidadService.Add(especialidad);
    return Results.Created($"/especialidades/{especialidad.Id}", especialidad);
})
.WithName("CreateEspecialidad")
.WithOpenApi();

// PUT: Actualizar especialidad
app.MapPut("/especialidades/{id}", (int id, EspecialidadDto especialidad) =>
{
    if (id != especialidad.Id)
        return Results.BadRequest();

    var existingEspecialidad = especialidadService.GetById(id);
    if (existingEspecialidad == null)
        return Results.NotFound();

    especialidadService.Update(especialidad);
    return Results.NoContent();
})
.WithName("UpdateEspecialidad")
.WithOpenApi();

// DELETE: Eliminar especialidad
app.MapDelete("/especialidades/{id}", (int id) =>
{
    var especialidad = especialidadService.GetById(id);
    if (especialidad == null)
        return Results.NotFound();

    especialidadService.Delete(id);
    return Results.NoContent();
})
.WithName("DeleteEspecialidad")
.WithOpenApi();

app.Run();

