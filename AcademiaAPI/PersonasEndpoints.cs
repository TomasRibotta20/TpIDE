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
            var personaService = new PersonaService();

            // Endpoint para obtener todos los alumnos
            app.MapGet("/personas/alumnos", () =>
            {
                try
                {
                    var alumnos = personaService.GetAllAlumnos();
                    return Results.Ok(alumnos);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ocurrió un error al obtener los alumnos: {ex.Message}");
                }
            });

            // Endpoint para obtener todos los profesores
            app.MapGet("/personas/profesores", () =>
            {
                try
                {
                    var profesores = personaService.GetAllProfesores();
                    return Results.Ok(profesores);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ocurrió un error al obtener los profesores: {ex.Message}");
                }
            });

            // Endpoint para obtener una persona por ID
            app.MapGet("/personas/{id:int}", (int id) =>
            {
                var persona = personaService.GetById(id);
                return persona == null ? Results.NotFound() : Results.Ok(persona);
            });

            // Endpoint para crear una nueva persona
            app.MapPost("/personas", (PersonaDto personaDto) =>
            {
                try
                {
                    personaService.Add(personaDto);
                    return Results.Created($"/personas/{personaDto.Id}", personaDto);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al crear la persona: {ex.Message}");
                }
            });

            // Endpoint para actualizar una persona
            app.MapPut("/personas/{id:int}", (int id, PersonaDto personaDto) =>
            {
                if (id != personaDto.Id)
                {
                    return Results.BadRequest("El ID de la ruta no coincide con el ID de la persona.");
                }

                try
                {
                    personaService.Update(personaDto);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error al actualizar la persona: {ex.Message}");
                }
            });

            // Endpoint para eliminar una persona
            app.MapDelete("/personas/{id:int}", (int id) =>
            {
                try
                {
                    personaService.Delete(id);
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