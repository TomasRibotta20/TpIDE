using DTOs;
using Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaAPI
{
    public static class CursosEndpoints
    {
        public static void MapCursosEndpoints(this WebApplication app)
        {
            var cursosGroup = app.MapGroup("/cursos")
                .WithTags("Cursos")
                .WithOpenApi();

            // GET /cursos - Obtener todos los cursos
            cursosGroup.MapGet("/", async () =>
            {
                try
                {
                    var service = new CursoService();
                    var cursos = await service.GetAllAsync();
                    return Results.Ok(cursos);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener cursos: {ex.Message}");
                }
            })
            .WithName("GetAllCursos")
            .WithSummary("Obtiene todos los cursos")
            .Produces<IEnumerable<CursoDto>>(200)
            .Produces(500);

            // GET /cursos/{id} - Obtener curso por ID
            cursosGroup.MapGet("/{id:int}", async (int id) =>
            {
                try
                {
                    var service = new CursoService();
                    var curso = await service.GetByIdAsync(id);
                    return curso != null ? Results.Ok(curso) : Results.NotFound($"Curso con ID {id} no encontrado");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener curso: {ex.Message}");
                }
            })
            .WithName("GetCursoById")
            .WithSummary("Obtiene un curso por su ID")
            .Produces<CursoDto>(200)
            .Produces(404)
            .Produces(500);

            // POST /cursos - Crear nuevo curso
            cursosGroup.MapPost("/", async ([FromBody] CursoDto cursoDto) =>
            {
                try
                {
                    var service = new CursoService();
                    var nuevoCurso = await service.CreateAsync(cursoDto);
                    return Results.Created($"/cursos/{nuevoCurso.IdCurso}", nuevoCurso);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error al crear curso: {ex.Message}");
                }
            })
            .WithName("CreateCurso")
            .WithSummary("Crea un nuevo curso")
            .Accepts<CursoDto>("application/json")
            .Produces<CursoDto>(201)
            .Produces(400)
            .Produces(500);

            // PUT /cursos/{id} - Actualizar curso
            cursosGroup.MapPut("/{id:int}", async (int id, [FromBody] CursoDto cursoDto) =>
            {
                try
                {
                    if (id != cursoDto.IdCurso)
                        return Results.BadRequest("El ID del curso no coincide");

                    var service = new CursoService();
                    await service.UpdateAsync(cursoDto);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error al actualizar curso: {ex.Message}");
                }
            })
            .WithName("UpdateCurso")
            .WithSummary("Actualiza un curso existente")
            .Accepts<CursoDto>("application/json")
            .Produces(204)
            .Produces(400)
            .Produces(500);

            // DELETE /cursos/{id} - Eliminar curso
            cursosGroup.MapDelete("/{id:int}", async (int id) =>
            {
                try
                {
                    var service = new CursoService();
                    await service.DeleteAsync(id);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error al eliminar curso: {ex.Message}");
                }
            })
            .WithName("DeleteCurso")
            .WithSummary("Elimina un curso")
            .Produces(204)
            .Produces(400)
            .Produces(500);

            // GET /cursos/comision/{idComision} - Obtener cursos por comisión
            cursosGroup.MapGet("/comision/{idComision:int}", async (int idComision) =>
            {
                try
                {
                    var service = new CursoService();
                    var cursos = await service.GetByComisionAsync(idComision);
                    return Results.Ok(cursos);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener cursos por comisión: {ex.Message}");
                }
            })
            .WithName("GetCursosByComision")
            .WithSummary("Obtiene todos los cursos de una comisión")
            .Produces<IEnumerable<CursoDto>>(200)
            .Produces(500);

            // GET /cursos/anio/{anioCalendario} - Obtener cursos por año calendario
            cursosGroup.MapGet("/anio/{anioCalendario:int}", async (int anioCalendario) =>
            {
                try
                {
                    var service = new CursoService();
                    var cursos = await service.GetByAnioCalendarioAsync(anioCalendario);
                    return Results.Ok(cursos);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener cursos por año: {ex.Message}");
                }
            })
            .WithName("GetCursosByAnio")
            .WithSummary("Obtiene todos los cursos de un año calendario")
            .Produces<IEnumerable<CursoDto>>(200)
            .Produces(500);
        }
    }
}