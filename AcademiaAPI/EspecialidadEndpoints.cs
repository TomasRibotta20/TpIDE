using Aplication.Services;
using DTOs;

namespace AcademiaAPI
{
    public static class EspecialidadEndpoints
    {
        public static void MapEspecialidadEndpoints(this WebApplication app)
        {
            app.MapGet("/especialidades", async () =>
            {
                var especialidadService = new EspecialidadService();
                var especialidades = await especialidadService.GetAllAsync();
                return Results.Ok(especialidades);
            });

            app.MapGet("/especialidades/{id:int}", async (int id) =>
            {
                var especialidadService = new EspecialidadService();
                var especialidad = await especialidadService.GetByIdAsync(id);
                return especialidad == null ? Results.NotFound() : Results.Ok(especialidad);
            });

            app.MapPost("/especialidades", async (EspecialidadDto especialidadDto) =>
            {
                var especialidadService = new EspecialidadService();
                await especialidadService.AddAsync(especialidadDto);
                return Results.Created($"/especialidades/{especialidadDto.Id}", especialidadDto);
            });

            app.MapPut("/especialidades/{id:int}", async (int id, EspecialidadDto especialidadDto) =>
            {
                if (id != especialidadDto.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }

                var especialidadService = new EspecialidadService();
                var existingEspecialidad = await especialidadService.GetByIdAsync(id);
                if (existingEspecialidad == null)
                {
                    return Results.NotFound();
                }

                await especialidadService.UpdateAsync(especialidadDto);
                return Results.NoContent();
            });

            app.MapDelete("/especialidades/{id:int}", async (int id) =>
            {
                var especialidadService = new EspecialidadService();
                var existingEspecialidad = await especialidadService.GetByIdAsync(id);
                if (existingEspecialidad == null)
                {
                    return Results.NotFound();
                }

                await especialidadService.DeleteAsync(id);
                return Results.NoContent();
            });
        }
    }
}
