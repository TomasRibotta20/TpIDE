using Aplication.Services;
using DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection; // Necesario para GetRequiredService
using System; // Necesario para Exception
using System.Collections.Generic; // Para KeyNotFoundException

namespace AcademiaAPI
{
    public static class MateriaEndpoints
    {
        public static void MapMateriaEndpoints(this WebApplication app)
        {
            // --- Helper para obtener el servicio manualmente ---
            // Este método obtiene las dependencias (Repositorio, PlanService) del contenedor de DI
            // y crea una instancia del MateriaService para cada solicitud.
            MateriaService GetService(HttpContext httpContext)
            {
                // Usar RequestServices es más seguro que CreateScope en Minimal APIs
                var repository = httpContext.RequestServices.GetRequiredService<Data.MateriaRepository>();
                // Asegúrate de que PlanService esté registrado en Program.cs
                var planService = httpContext.RequestServices.GetRequiredService<PlanService>();
                return new MateriaService(repository, planService);
            }
            // ---------------------------------------------------------------------

            // Agrupar endpoints bajo /materias y añadir tag "Materias" para Swagger
            var group = app.MapGroup("/materias").WithTags("AcademiaAPI");

            // GET /materias - Obtener todas las materias
            group.MapGet("/", (HttpContext context) =>
            {
                try
                {
                    var service = GetService(context);
                    var materias = service.GetAll();
                    return Results.Ok(materias);
                }
                catch (NotImplementedException) // Si el repositorio ADO.NET aún no está implementado
                {
                    return Results.Problem(detail: "Funcionalidad GetAll aún no implementada en el repositorio ADO.NET.", statusCode: 501); // 501 Not Implemented
                }
                catch (Exception ex)
                {
                    // Loggear el error real aquí si es posible (usar ILogger sería mejor)
                    Console.WriteLine($"ERROR en GET /materias: {ex.Message} \n {ex.StackTrace}");
                    return Results.Problem(detail: "Ocurrió un error interno al obtener las materias.", statusCode: 500);
                }
            });

            // GET /materias/{id} - Obtener una materia por ID
            group.MapGet("/{id:int}", (int id, HttpContext context) =>
            {
                try
                {
                    var service = GetService(context);
                    var materia = service.GetById(id);
                    // Si GetById devuelve null, es un 404 Not Found
                    return materia == null ? Results.NotFound($"Materia con ID {id} no encontrada.") : Results.Ok(materia);
                }
                catch (NotImplementedException)
                {
                    return Results.Problem(detail: "Funcionalidad GetById aún no implementada en el repositorio ADO.NET.", statusCode: 501);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR en GET /materias/{id}: {ex.Message} \n {ex.StackTrace}");
                    return Results.Problem(detail: "Ocurrió un error interno al obtener la materia.", statusCode: 500);
                }
            });

            // POST /materias - Crear una nueva materia
            group.MapPost("/", (MateriaDto materiaDto, HttpContext context) =>
            {
                try
                {
                    var service = GetService(context);
                    service.Add(materiaDto);
                    // Devolver 201 Created con la ubicación y el objeto creado
                    // Nota: Si Add no devuelve el ID, el DTO de respuesta no tendrá el ID asignado por la BD.
                    // Para obtener el ID real, el método Add del repositorio debería devolverlo.
                    return Results.Created($"/materias/{materiaDto.Id}", materiaDto); // Usamos el ID del DTO (puede ser 0)
                }
                catch (ArgumentException ex) // Error de validación (ej: Plan no existe, datos inválidos en entidad)
                {
                    return Results.BadRequest(new { message = ex.Message }); // 400 Bad Request con mensaje
                }
                catch (NotImplementedException)
                {
                    return Results.Problem(detail: "Funcionalidad Add aún no implementada en el repositorio ADO.NET.", statusCode: 501);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR en POST /materias: {ex.Message} \n {ex.StackTrace}");
                    return Results.Problem(detail: "Ocurrió un error interno al crear la materia.", statusCode: 500);
                }
            });

            // PUT /materias/{id} - Actualizar una materia existente
            group.MapPut("/{id:int}", (int id, MateriaDto materiaDto, HttpContext context) =>
            {
                // Validar que el ID en la ruta coincida con el ID en el cuerpo
                if (id != materiaDto.Id)
                {
                    return Results.BadRequest(new { message = "El ID en la URL no coincide con el ID del cuerpo de la solicitud." });
                }

                try
                {
                    var service = GetService(context);
                    service.Update(materiaDto);
                    return Results.NoContent(); // 204 No Content es la respuesta estándar para un PUT exitoso
                }
                catch (KeyNotFoundException ex) // Materia o Plan no encontrado por el Service
                {
                    return Results.NotFound(new { message = ex.Message }); // 404 Not Found con mensaje
                }
                catch (ArgumentException ex) // Error de validación
                {
                    return Results.BadRequest(new { message = ex.Message }); // 400 Bad Request con mensaje
                }
                catch (NotImplementedException)
                {
                    return Results.Problem(detail: "Funcionalidad Update aún no implementada en el repositorio ADO.NET.", statusCode: 501);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR en PUT /materias/{id}: {ex.Message} \n {ex.StackTrace}");
                    return Results.Problem(detail: "Ocurrió un error interno al actualizar la materia.", statusCode: 500);
                }
            });

            // DELETE /materias/{id} - Eliminar una materia
            group.MapDelete("/{id:int}", (int id, HttpContext context) =>
            {
                try
                {
                    var service = GetService(context);
                    service.Delete(id);
                    return Results.NoContent(); // 204 No Content es la respuesta estándar para un DELETE exitoso
                }
                catch (KeyNotFoundException ex) // Materia no encontrada
                {
                    return Results.NotFound(new { message = ex.Message }); // 404 Not Found con mensaje
                }
                catch (NotImplementedException)
                {
                    return Results.Problem(detail: "Funcionalidad Delete aún no implementada en el repositorio ADO.NET.", statusCode: 501);
                }
                catch (Exception ex)
                {
                    // Podría haber errores si hay dependencias (ej: Cursos asociados a la Materia)
                    Console.WriteLine($"ERROR en DELETE /materias/{id}: {ex.Message} \n {ex.StackTrace}");
                    // Devolver un 409 Conflict si hay dependencias sería más específico, pero requiere lógica adicional en el service/repo
                    return Results.Problem(detail: "Ocurrió un error interno al eliminar la materia. Verifique si tiene dependencias.", statusCode: 500);
                }
            });
        }
    }
}
