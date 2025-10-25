using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Data
{
    public class ComisionRepository
    {
        private const string TABLE_NAME = "Comisiones";
        
        private AcademiaContext CreateContext()
        {
            return new AcademiaContext();
        }

        private async Task EnsureComisionesTableExistsAsync()
        {
            using var context = CreateContext();
            
            try
            {
                try
                {
                    var count = await context.Comisiones.CountAsync();
                    Console.WriteLine($"Comisiones table exists, count: {count}");
                    return;
                }
                catch
                {
                    Console.WriteLine("Comisiones table might not exist, will try to create it");
                }

                var connection = context.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = @"
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
                    PRINT 'Comisiones table created';
                END
                ELSE
                BEGIN
                    PRINT 'Comisiones table already exists';
                END";

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring Comisiones table exists: {ex.Message}");
                throw new Exception($"Error creating Comisiones table: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Domain.Model.Comision>> GetAllAsync()
        {
            try 
            {
                await EnsureComisionesTableExistsAsync();
                
                using var context = CreateContext();
                await context.Database.EnsureCreatedAsync();
                
                return await context.Comisiones.OrderBy(c => c.DescComision).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Comisiones: {ex.Message}");
                throw new Exception($"Error retrieving Comisiones: {ex.Message}", ex);
            }
        }

        public async Task<Domain.Model.Comision?> GetByIdAsync(int id)
        {
            try
            {
                await EnsureComisionesTableExistsAsync();
                
                using var context = CreateContext();
                return await context.Comisiones.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Comision with ID {id}: {ex.Message}");
                throw new Exception($"Error retrieving Comision with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task AddAsync(Domain.Model.Comision comision)
        {
            try
            {
                await EnsureComisionesTableExistsAsync();
                
                using var context = CreateContext();
                context.Comisiones.Add(comision);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Comision: {ex.Message}");
                throw new Exception($"Error adding Comision: {ex.Message}", ex);
            }
        }

        public async Task UpdateAsync(Domain.Model.Comision comision)
        {
            try
            {
                await EnsureComisionesTableExistsAsync();
                
                using var context = CreateContext();
                context.Comisiones.Update(comision);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Comision with ID {comision.IdComision}: {ex.Message}");
                throw new Exception($"Error updating Comision with ID {comision.IdComision}: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await EnsureComisionesTableExistsAsync();
                
                using var context = CreateContext();
                var comision = await context.Comisiones.FindAsync(id);
                if (comision != null)
                {
                    context.Comisiones.Remove(comision);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Comision with ID {id}: {ex.Message}");
                throw new Exception($"Error deleting Comision with ID {id}: {ex.Message}", ex);
            }
        }
    }
}
