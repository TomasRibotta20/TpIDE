using DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Data;

namespace AcademiaAPI
{
    public static class DocenteCursoEndpoints
    {
        private static DocenteCursoService CreateService()
        {
            var context = new AcademiaContext();
            var docenteCursoRepo = new DocenteCursoRepository(context);
            var cursoRepo = new CursoRepository();
            var personaRepo = new PersonaRepository();
            var materiaRepo = new MateriaRepository("Server=localhost,1433;Database=Universidad;User Id=sa;Password=TuContrase�aFuerte123;TrustServerCertificate=True");
            var comisionRepo = new ComisionRepository();

            return new DocenteCursoService(docenteCursoRepo, cursoRepo, personaRepo, materiaRepo, comisionRepo);
        }

        public static void MapDocenteCursoEndpoints(this WebApplication app)
        {
            var docenteCursoGroup = app.MapGroup("/docentes-cursos")
                .WithTags("Docentes-Cursos")
                .WithOpenApi();

            // GET /docentes-cursos - Obtener todas las asignaciones
            docenteCursoGroup.MapGet("/", async () =>
            {
                try
                {
                    var service = CreateService();
                    var asignaciones = await service.GetAllAsync();
                    return Results.Ok(asignaciones);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener asignaciones: {ex.Message}");
                }
            })
            .WithName("GetAllDocenteCursos")
            .WithSummary("Obtiene todas las asignaciones de docentes a cursos")
            .Produces<IEnumerable<DocenteCursoDto>>(200)
            .Produces(500);

            // GET /docentes-cursos/{id} - Obtener asignaci�n por ID
            docenteCursoGroup.MapGet("/{id:int}", async (int id) =>
            {
                try
                {
                    var service = CreateService();
                    var asignacion = await service.GetByIdAsync(id);
                    return asignacion != null 
                        ? Results.Ok(asignacion) 
                        : Results.NotFound($"Asignaci�n con ID {id} no encontrada");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener asignaci�n: {ex.Message}");
                }
            })
            .WithName("GetDocenteCursoById")
            .WithSummary("Obtiene una asignaci�n por su ID")
            .Produces<DocenteCursoDto>(200)
            .Produces(404)
            .Produces(500);

            // GET /docentes-cursos/curso/{cursoId} - Obtener docentes de un curso
            docenteCursoGroup.MapGet("/curso/{cursoId:int}", async (int cursoId) =>
            {
                try
                {
                    var service = CreateService();
                    var asignaciones = await service.GetByCursoIdAsync(cursoId);
                    return Results.Ok(asignaciones);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener docentes del curso: {ex.Message}");
                }
            })
            .WithName("GetDocentesByCurso")
            .WithSummary("Obtiene todos los docentes asignados a un curso")
            .Produces<IEnumerable<DocenteCursoDto>>(200)
            .Produces(500);

            // GET /docentes-cursos/docente/{docenteId} - Obtener cursos de un docente
            docenteCursoGroup.MapGet("/docente/{docenteId:int}", async (int docenteId) =>
            {
                try
                {
                    var service = CreateService();
                    var asignaciones = await service.GetByDocenteIdAsync(docenteId);
                    return Results.Ok(asignaciones);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener cursos del docente: {ex.Message}");
                }
            })
            .WithName("GetCursosByDocente")
            .WithSummary("Obtiene todos los cursos asignados a un docente")
            .Produces<IEnumerable<DocenteCursoDto>>(200)
            .Produces(500);

            // POST /docentes-cursos - Crear nueva asignaci�n
            docenteCursoGroup.MapPost("/", async ([FromBody] DocenteCursoCreateDto createDto) =>
            {
                try
                {
                    var service = CreateService();
                    var nuevaAsignacion = await service.CreateAsync(createDto);
                    return Results.Created($"/docentes-cursos/{nuevaAsignacion.IdDictado}", nuevaAsignacion);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al crear asignaci�n: {ex.Message}");
                }
            })
            .WithName("CreateDocenteCurso")
            .WithSummary("Asigna un docente a un curso con un cargo espec�fico")
            .Accepts<DocenteCursoCreateDto>("application/json")
            .Produces<DocenteCursoDto>(201)
            .Produces(400)
            .Produces(500);

            // PUT /docentes-cursos/{id} - Actualizar asignaci�n
            docenteCursoGroup.MapPut("/{id:int}", async (int id, [FromBody] DocenteCursoCreateDto updateDto) =>
            {
                try
                {
                    var service = CreateService();
                    var actualizada = await service.UpdateAsync(id, updateDto);
                    return Results.Ok(actualizada);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al actualizar asignaci�n: {ex.Message}");
                }
            })
            .WithName("UpdateDocenteCurso")
            .WithSummary("Actualiza una asignaci�n existente")
            .Accepts<DocenteCursoCreateDto>("application/json")
            .Produces<DocenteCursoDto>(200)
            .Produces(400)
            .Produces(500);

            // DELETE /docentes-cursos/{id} - Eliminar asignaci�n
            docenteCursoGroup.MapDelete("/{id:int}", async (int id) =>
            {
                try
                {
                    var service = CreateService();
                    var eliminado = await service.DeleteAsync(id);
                    return eliminado 
                        ? Results.NoContent() 
                        : Results.NotFound($"Asignaci�n con ID {id} no encontrada");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al eliminar asignaci�n: {ex.Message}");
                }
            })
            .WithName("DeleteDocenteCurso")
            .WithSummary("Elimina una asignaci�n de docente a curso")
            .Produces(204)
            .Produces(404)
            .Produces(500);
        }
    }
}
