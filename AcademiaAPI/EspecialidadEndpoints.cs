using Aplication.Services;
using DTOs;

namespace AcademiaAPI
{
    public static class EspecialidadEndpoints
    {
        public static void MapEspecialidadEndpoints(this WebApplication app)
        {
            var especialidadService = new EspecialidadService();
            app.MapGet("/especialidades", () =>
            {
                var especialidades = especialidadService.GetAll();
                return Results.Ok(especialidades);
            });
            app.MapGet("/especialidades/{id:int}", (int id) =>
            {
                var especialidad = especialidadService.GetById(id);
                return especialidad == null ? Results.NotFound() : Results.Ok(especialidad);
            });
            app.MapPost("/especialidades", (EspecialidadDto especialidadDto) =>
            {
                especialidadService.Add(especialidadDto);
                return Results.Created($"/especialidades/{especialidadDto.Id}", especialidadDto);
            });
            app.MapPut("/especialidades/{id:int}", (int id, EspecialidadDto especialidadDto) =>
            {
                if (id != especialidadDto.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var existingEspecialidad = especialidadService.GetById(id);
                if (existingEspecialidad == null)
                {
                    return Results.NotFound();
                }
                especialidadService.Update(especialidadDto);
                return Results.NoContent();
            });
            app.MapDelete("/especialidades/{id:int}", (int id) =>
            {
                var existingEspecialidad = especialidadService.GetById(id);
                if (existingEspecialidad == null)
                {
                    return Results.NotFound();
                }
                especialidadService.Delete(id);
                return Results.NoContent();
            });
        }

    }
}
