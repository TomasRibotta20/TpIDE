using DTOs;
using Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaAPI
{
    public static class InscripcionesEndpoints
    {
        public static void MapInscripcionesEndpoints(this WebApplication app)
        {
            var inscripcionesGroup = app.MapGroup("/inscripciones")
                .WithTags("Inscripciones")
                .WithOpenApi();

            // GET /inscripciones - Obtener todas las inscripciones
            inscripcionesGroup.MapGet("/", async () =>
            {
                try
                {
                    var service = new InscripcionService();
                    var inscripciones = await service.GetAllAsync();
                    return Results.Ok(inscripciones);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener inscripciones: {ex.Message}");
                }
            })
            .WithName("GetAllInscripciones")
            .WithSummary("Obtiene todas las inscripciones")
            .Produces<IEnumerable<AlumnoCursoDto>>(200)
            .Produces(500);

            // GET /inscripciones/{id} - Obtener inscripción por ID
            inscripcionesGroup.MapGet("/{id:int}", async (int id) =>
            {
                try
                {
                    var service = new InscripcionService();
                    var inscripcion = await service.GetByIdAsync(id);
                    return inscripcion != null ? Results.Ok(inscripcion) : Results.NotFound($"Inscripción con ID {id} no encontrada");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener inscripción: {ex.Message}");
                }
            })
            .WithName("GetInscripcionById")
            .WithSummary("Obtiene una inscripción por su ID")
            .Produces<AlumnoCursoDto>(200)
            .Produces(404)
            .Produces(500);

            // POST /inscripciones - Inscribir alumno a curso
            inscripcionesGroup.MapPost("/", async ([FromBody] InscripcionRequest request) =>
            {
                try
                {
                    var service = new InscripcionService();
                    
                    // Convertir string a enum
                    if (!Enum.TryParse<CondicionAlumnoDto>(request.Condicion, true, out var condicion))
                    {
                        return Results.BadRequest("La condición especificada no es válida. Debe ser: Libre, Regular o Promocional");
                    }
                    
                    try 
                    {
                        var inscripcion = await service.InscribirAlumnoAsync(request.IdAlumno, request.IdCurso, condicion);
                        return Results.Created($"/inscripciones/{inscripcion.IdInscripcion}", inscripcion);
                    }
                    catch (Exception ex) when (ex.Message.Contains("ya está inscripto"))
                    {
                        return Results.BadRequest(ex.Message);
                    }
                    catch (Exception ex) when (ex.Message.Contains("No hay cupo"))
                    {
                        return Results.BadRequest(ex.Message);
                    }
                    catch (Exception ex) when (ex.Message.Contains("años anteriores"))
                    {
                        return Results.BadRequest(ex.Message);
                    }
                    catch (Exception ex) when (ex.Message.Contains("no existe"))
                    {
                        return Results.NotFound(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error al inscribir alumno: {ex.Message}");
                }
            })
            .WithName("InscribirAlumno")
            .WithSummary("Inscribe un alumno a un curso")
            .Accepts<InscripcionRequest>("application/json")
            .Produces<AlumnoCursoDto>(201)
            .Produces(400)
            .Produces(500);

            // PUT /inscripciones/{id}/condicion - Actualizar condición y nota
            inscripcionesGroup.MapPut("/{id:int}/condicion", async (int id, [FromBody] ActualizarCondicionRequest request) =>
            {
                try
                {
                    var service = new InscripcionService();
                    
                    // Convertir string a enum
                    if (!Enum.TryParse<CondicionAlumnoDto>(request.Condicion, true, out var condicion))
                    {
                        return Results.BadRequest("Condición inválida. Debe ser: Libre, Regular o Promocional");
                    }
                    
                    await service.ActualizarCondicionYNotaAsync(id, condicion, request.Nota);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error al actualizar condición: {ex.Message}");
                }
            })
            .WithName("ActualizarCondicion")
            .WithSummary("Actualiza la condición y nota de una inscripción")
            .Accepts<ActualizarCondicionRequest>("application/json")
            .Produces(204)
            .Produces(400)
            .Produces(500);

            // DELETE /inscripciones/{id} - Desinscribir alumno
            inscripcionesGroup.MapDelete("/{id:int}", async (int id) =>
            {
                try
                {
                    var service = new InscripcionService();
                    await service.DesinscribirAlumnoAsync(id);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error al desinscribir alumno: {ex.Message}");
                }
            })
            .WithName("DesinscribirAlumno")
            .WithSummary("Desinscribe un alumno de un curso")
            .Produces(204)
            .Produces(400)
            .Produces(500);

            // GET /inscripciones/alumno/{idAlumno} - Obtener inscripciones de un alumno
            inscripcionesGroup.MapGet("/alumno/{idAlumno:int}", async (int idAlumno) =>
            {
                try
                {
                    var service = new InscripcionService();
                    var inscripciones = await service.GetInscripcionesByAlumnoAsync(idAlumno);
                    return Results.Ok(inscripciones);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener inscripciones del alumno: {ex.Message}");
                }
            })
            .WithName("GetInscripcionesByAlumno")
            .WithSummary("Obtiene todas las inscripciones de un alumno")
            .Produces<IEnumerable<AlumnoCursoDto>>(200)
            .Produces(500);

            // GET /inscripciones/curso/{idCurso} - Obtener inscripciones de un curso
            inscripcionesGroup.MapGet("/curso/{idCurso:int}", async (int idCurso) =>
            {
                try
                {
                    var service = new InscripcionService();
                    var inscripciones = await service.GetInscripcionesByCursoAsync(idCurso);
                    return Results.Ok(inscripciones);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener inscripciones del curso: {ex.Message}");
                }
            })
            .WithName("GetInscripcionesByCurso")
            .WithSummary("Obtiene todas las inscripciones de un curso")
            .Produces<IEnumerable<AlumnoCursoDto>>(200)
            .Produces(500);

            // GET /inscripciones/estadisticas - Obtener estadísticas generales
            inscripcionesGroup.MapGet("/estadisticas", async () =>
            {
                try
                {
                    var service = new InscripcionService();
                    var estadisticas = await service.GetEstadisticasGeneralesAsync();
                    return Results.Ok(estadisticas);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al obtener estadísticas: {ex.Message}");
                }
            })
            .WithName("GetEstadisticasInscripciones")
            .WithSummary("Obtiene estadísticas generales de inscripciones")
            .Produces<Dictionary<string, int>>(200)
            .Produces(500);
        }
    }

    // DTOs para las requests
    public record InscripcionRequest(int IdAlumno, int IdCurso, string Condicion = "Regular");
    public record ActualizarCondicionRequest(string Condicion, int? Nota = null);
}