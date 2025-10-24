using Aplication.Services;
using DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AcademiaAPI
{
    public static class ComisionesEndpoints
    {
        public static void MapComisionesEndpoints(this WebApplication app) { 
        
            var comisionService = new Aplication.Services.ComisionService();
            

            app.MapGet("/comisiones/test-table", () =>
            {
                try
                {
                    using var context = new Data.AcademiaContext();
                    
           
                    var count = context.Comisiones.Count();
                    
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
                        
                  
                        context.Database.EnsureCreated();
                        
                   
                        context.Database.ExecuteSqlRaw(@"
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
            
            app.MapGet("/comisiones", () =>
            {
                var comisiones = comisionService.GetAll();
                return Results.Ok(comisiones);
            });
            
            app.MapGet("/comisiones/{id:int}", (int id) =>
            {
                var comision = comisionService.GetById(id);
                return comision == null ? Results.NotFound() : Results.Ok(comision);
            });
            
            app.MapPost("/comisiones", (DTOs.ComisionDto comisionDto) =>
            {
                comisionService.Add(comisionDto);
                return Results.Created($"/comisiones/{comisionDto.IdComision}", comisionDto);
            });
            
            app.MapPut("/comisiones/{id:int}", (int id, DTOs.ComisionDto comisionDto) =>
            {
                if (id != comisionDto.IdComision)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var existingComision = comisionService.GetById(id);
                if (existingComision == null)
                {
                    return Results.NotFound();
                }
                comisionService.Update(comisionDto);
                return Results.NoContent();
            });
            
            app.MapDelete("/comisiones/{id:int}", (int id) =>
            {
                var existingComision = comisionService.GetById(id);
                if (existingComision == null)
                {
                    return Results.NotFound();
                }
                
                comisionService.Delete(id);
                return Results.NoContent();
            });
        }
    }
}
