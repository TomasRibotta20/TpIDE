using Data;
using Domain.Model;
using Microsoft.Data.SqlClient;

namespace AcademiaAPI
{
    public static class MateriaTestHelper
    {
        public static async Task InsertTestMateriasAsync(string connectionString)
        {
            try
            {
                Console.WriteLine("Insertando materias de prueba...");

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                // Primero verificar si ya existen materias
                var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Materias", connection);
                var count = (int)await checkCmd.ExecuteScalarAsync();

                if (count > 0)
                {
                    Console.WriteLine($"Ya existen {count} materias en la base de datos.");
                    return;
                }

                // Verificar que existan planes
                var planCmd = new SqlCommand("SELECT COUNT(*) FROM Planes", connection);
                var planCount = (int)await planCmd.ExecuteScalarAsync();

                if (planCount == 0)
                {
                    Console.WriteLine("No hay planes disponibles. Creando plan de prueba...");
                    var insertPlanCmd = new SqlCommand(
                        "INSERT INTO Planes (Descripcion, EspecialidadId) VALUES ('Plan de Prueba', 1)", 
                        connection);
                    await insertPlanCmd.ExecuteNonQueryAsync();
                }

                // Insertar materias de prueba
                var insertMateriasCmd = new SqlCommand(@"
                    INSERT INTO Materias (Descripcion, HorasSemanales, HorasTotales, IdPlan) VALUES 
                    ('Matemática I', 4, 64, 1),
                    ('Física I', 6, 96, 1),
                    ('Programación I', 6, 96, 1),
                    ('Álgebra', 4, 64, 1),
                    ('Química General', 5, 80, 1)", connection);

                int rowsAffected = await insertMateriasCmd.ExecuteNonQueryAsync();
                Console.WriteLine($"Se insertaron {rowsAffected} materias de prueba correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error insertando materias de prueba: {ex.Message}");
            }
        }
    }
}