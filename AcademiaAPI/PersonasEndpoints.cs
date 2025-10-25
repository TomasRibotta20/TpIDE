using Aplication.Services;
using DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace AcademiaAPI
{
    public static class PersonasEndpoints
    {
        public static void MapPersonasEndpoints(this WebApplication app)
        {
            // Endpoint para obtener TODAS las personas (alumnos y profesores)
            app.MapGet("/personas", async () =>
            {
                try
                {
                    var personaService = new PersonaService();
                    var personas = await personaService.GetAllAsync();
                    return Results.Ok(personas);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ocurrió un error al obtener las personas: {ex.Message}");
                }
            });

            // Endpoint para obtener todos los alumnos
            app.MapGet("/personas/alumnos", async () =>
            {
                try
                {
                    var personaService = new PersonaService();
                    var alumnos = await personaService.GetAllAlumnosAsync();
                    return Results.Ok(alumnos);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ocurrió un error al obtener los alumnos: {ex.Message}");
                }
            });

            // Endpoint para obtener todos los profesores
            app.MapGet("/personas/profesores", async () =>
            {
                try
                {
                    var personaService = new PersonaService();
                    var profesores = await personaService.GetAllProfesoresAsync();
                    return Results.Ok(profesores);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ocurrió un error al obtener los profesores: {ex.Message}");
                }
            });

            // Endpoint para obtener una persona por ID
            app.MapGet("/personas/{id:int}", async (int id) =>
            {
                var personaService = new PersonaService();
                var persona = await personaService.GetByIdAsync(id);
                return persona == null ? Results.NotFound() : Results.Ok(persona);
            });

            // Endpoint para crear una nueva persona
            app.MapPost("/personas", async (PersonaDto personaDto) =>
            {
                try
                {
                    var personaService = new PersonaService();
                    await personaService.AddAsync(personaDto);
                    return Results.Created($"/personas/{personaDto.Id}", personaDto);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al crear la persona: {ex.Message}");
                }
            });

            // Endpoint para actualizar una persona
            app.MapPut("/personas/{id:int}", async (int id, PersonaDto personaDto) =>
            {
                if (id != personaDto.Id)
                {
                    return Results.BadRequest("El ID de la ruta no coincide con el ID de la persona.");
                }

                try
                {
                    var personaService = new PersonaService();
                    await personaService.UpdateAsync(personaDto);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al actualizar la persona: {ex.Message}");
                }
            });

            // Endpoint para eliminar una persona
            app.MapDelete("/personas/{id:int}", async (int id) =>
            {
                try
                {
                    var personaService = new PersonaService();
                    await personaService.DeleteAsync(id);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al eliminar la persona: {ex.Message}");
                }
            });
        }
    }
}