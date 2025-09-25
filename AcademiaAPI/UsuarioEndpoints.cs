namespace AcademiaAPI
{
    public static class UsuarioEndpoints
    {
        public static void MapUsuariosEndpoints(this WebApplication app) 
        { 
        
        var usuariosService = new Aplication.Services.UsuarioService();
            app.MapGet("/usuarios", () =>
            {
                var usuarios = usuariosService.GetAll();
                return Results.Ok(usuarios);
            });
            app.MapGet("/usuarios/{id:int}", (int id) =>
            {
                var usuario = usuariosService.GetById(id);
                return usuario == null ? Results.NotFound() : Results.Ok(usuario);
            });
            app.MapPost("/usuarios", (DTOs.UsuarioDto usuarioDto) =>
            {
                usuariosService.Add(usuarioDto);
                return Results.Created($"/usuarios/{usuarioDto.Id}", usuarioDto);
            });
            app.MapPut("/usuarios/{id:int}", (int id, DTOs.UsuarioDto usuarioDto) =>
            {
                if (id != usuarioDto.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var existingUsuario = usuariosService.GetById(id);
                if (existingUsuario == null)
                {
                    return Results.NotFound();
                }
                usuariosService.Update(usuarioDto);
                return Results.NoContent();
            });
            app.MapDelete("/usuarios/{id:int}", (int id) =>
            {
                var existingUsuario = usuariosService.GetById(id);
                if (existingUsuario == null)
                {
                    return Results.NotFound();
                }
                usuariosService.Delete(id);
                return Results.NoContent();
            });

        }
    }
}
