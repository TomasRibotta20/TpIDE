using Aplication.Services;
using DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AcademiaAPI
{
    public static class ComisionesEndpoints
    {
        public static void MapComisionesEndpoints(this WebApplication app) { 

            app.MapGet("/comisiones/test-table", async () =>
            {
                try
                {
                    using var context = new Data.AcademiaContext();
                    var count = await context.Comisiones.CountAsync();
                    
                    return Results.Ok(new { 
                        message = "La tabla Comisiones existe correctamente.", 
                        recordCount = count 
                    });
                }
                catch (Exception ex)
                {
                    try
                    {
                        using var context = new Data.AcademiaContext();
                        await context.Database.EnsureCreatedAsync();
                        
                        await context.Database.ExecuteSqlRawAsync(@"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Comisiones')
                        BEGIN
                            CREATE TABLE [dbo].[Comisiones](
                                [IdComision] [int] IDENTITY(1,1) NOT NULL,
                                [DescComision] [nvarchar](50) NOT NULL,
                                [AnioEspecialidad] [int] NOT NULL,
                                [IdPlan] [int] NOT NULL,
                                CONSTRAINT [PK_Comisiones] PRIMARY KEY CLUSTERED ([IdComision] ASC),
                                CONSTRAINT [FK_Comisiones_Planes] FOREIGN KEY([IdPlan]) REFERENCES [dbo].[Planes] ([Id])
                            );
                        END");
                        
                        return Results.Ok(new { 
                            message = "La tabla Comisiones ha sido creada manualmente.", 
                            originalError = ex.Message 
                        });
                    }
                    catch (Exception innerEx)
                    {
                        return Results.BadRequest(new { 
                            message = "Error al crear la tabla Comisiones.", 
                            originalError = ex.Message, 
                            creationError = innerEx.Message 
                        });
                    }
                }
            });
            
            app.MapGet("/comisiones", async () =>
            {
                var comisionService = new ComisionService();
                var comisiones = await comisionService.GetAllAsync();
                return Results.Ok(comisiones);
            });
            
            app.MapGet("/comisiones/{id:int}", async (int id) =>
            {
                var comisionService = new ComisionService();
                var comision = await comisionService.GetByIdAsync(id);
                return comision == null ? Results.NotFound() : Results.Ok(comision);
            });
            
            app.MapPost("/comisiones", async (DTOs.ComisionDto comisionDto) =>
            {
                var comisionService = new ComisionService();
                await comisionService.AddAsync(comisionDto);
                return Results.Created($"/comisiones/{comisionDto.IdComision}", comisionDto);
            });
            
            app.MapPut("/comisiones/{id:int}", async (int id, DTOs.ComisionDto comisionDto) =>
            {
                if (id != comisionDto.IdComision)
                {
                    return Results.BadRequest("ID mismatch");
                }

                var comisionService = new ComisionService();
                var existingComision = await comisionService.GetByIdAsync(id);
                if (existingComision == null)
                {
                    return Results.NotFound();
                }

                await comisionService.UpdateAsync(comisionDto);
                return Results.NoContent();
            });
            
            app.MapDelete("/comisiones/{id:int}", async (int id) =>
            {
                var comisionService = new ComisionService();
                var existingComision = await comisionService.GetByIdAsync(id);
                if (existingComision == null)
                {
                    return Results.NotFound();
                }
                
                await comisionService.DeleteAsync(id);
                return Results.NoContent();
            });
        }
    }
}
