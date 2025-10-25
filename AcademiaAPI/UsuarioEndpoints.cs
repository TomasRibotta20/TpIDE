namespace AcademiaAPI
{
    public static class UsuarioEndpoints
    {
        public static void MapUsuariosEndpoints(this WebApplication app) 
        { 
            app.MapGet("/usuarios", () =>
            {
                var usuariosService = new Aplication.Services.UsuarioService();
                var usuarios = usuariosService.GetAll();
                return Results.Ok(usuarios);
            });
            
            app.MapGet("/usuarios/{id:int}", (int id) =>
            {
                var usuariosService = new Aplication.Services.UsuarioService();
                var usuario = usuariosService.GetById(id);
                return usuario == null ? Results.NotFound() : Results.Ok(usuario);
            });

            // Obtener módulos disponibles
            app.MapGet("/usuarios/modulos", async () =>
            {
                var usuariosService = new Aplication.Services.UsuarioService();
                var modulos = await usuariosService.GetModulosAsync();
                return Results.Ok(modulos);
            });

            // Obtener tipos de usuario predefinidos
            app.MapGet("/usuarios/tipos", () =>
            {
                var usuariosService = new Aplication.Services.UsuarioService();
                var tipos = usuariosService.GetTiposUsuario();
                return Results.Ok(tipos);
            });
            
            app.MapPost("/usuarios", (DTOs.UsuarioDto usuarioDto) =>
            {
                var usuariosService = new Aplication.Services.UsuarioService();
                usuariosService.Add(usuarioDto);
                return Results.Created($"/usuarios/{usuarioDto.Id}", usuarioDto);
            });
            
            app.MapPut("/usuarios/{id:int}", (int id, DTOs.UsuarioDto usuarioDto) =>
            {
                if (id != usuarioDto.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                
                var usuariosService = new Aplication.Services.UsuarioService();
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
                var usuariosService = new Aplication.Services.UsuarioService();
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
