using Domain.Model;
using Microsoft.Data.SqlClient; // Asegúrate de tener este paquete referenciado
using System;
using System.Collections.Generic;
using System.Linq; // Necesario para GetOrdinal

namespace Data
{
    // NO hereda de nada especial, es una clase concreta que maneja ADO.NET
    public class MateriaRepository // Si usan interfaces, sería: : IMateriaRepository
    {
        private readonly string _connectionString; // Almacena la cadena de conexión

        // Constructor: Recibe la cadena de conexión (inyectada desde Program.cs)
        public MateriaRepository(string connectionString)
        {
            // Validación para asegurar que la cadena de conexión no sea nula o vacía
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "La cadena de conexión no puede ser nula o vacía.");
            }
            _connectionString = connectionString;
        }

        // --- Helper Privado para Mapear Datos del Lector SQL a la Entidad Materia ---
        private Materia MapFromReader(SqlDataReader reader)
        {
            // Crea y retorna un objeto Materia a partir de los datos de una fila leída
            return new Materia(
                // Obtener valores por nombre de columna de forma segura
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                descripcion: reader.GetString(reader.GetOrdinal("Descripcion")),
                horasSemanales: reader.GetInt32(reader.GetOrdinal("HorasSemanales")),
                horasTotales: reader.GetInt32(reader.GetOrdinal("HorasTotales")),
                idPlan: reader.GetInt32(reader.GetOrdinal("IdPlan"))
            );
            // Nota: El constructor de Materia ya incluye las validaciones de dominio
        }

        // --- Métodos Públicos para Operaciones CRUD ---

        // Obtener TODAS las materias
        public IEnumerable<Materia> GetAll()
        {
            var materias = new List<Materia>(); // Lista para almacenar los resultados
            // Consulta SQL para seleccionar todas las materias, ordenadas por descripción
            const string sql = "SELECT Id, Descripcion, HorasSemanales, HorasTotales, IdPlan FROM Materias ORDER BY Descripcion";

            // Usar 'using' asegura que la conexión y el comando se cierren correctamente
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open(); // Abrir la conexión a la BD
                using (var reader = command.ExecuteReader()) // Ejecutar la consulta y obtener un lector
                {
                    // Leer cada fila devuelta por la consulta
                    while (reader.Read())
                    {
                        // Mapear la fila actual a un objeto Materia y añadirlo a la lista
                        materias.Add(MapFromReader(reader));
                    }
                }
            }
            return materias; // Devolver la lista de materias
        }

        // Obtener UNA materia por su ID
        public Materia? GetById(int id) // Devuelve Materia? (anulable) si no se encuentra
        {
            // Consulta SQL para seleccionar una materia específica por ID
            const string sql = "SELECT Id, Descripcion, HorasSemanales, HorasTotales, IdPlan FROM Materias WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                // Añadir parámetro @Id para prevenir inyección SQL
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    // Si se encontró una fila, mapearla y devolverla
                    if (reader.Read())
                    {
                        return MapFromReader(reader);
                    }
                    // Si no se encontró ninguna fila, devolver null
                    return null;
                }
            }
        }

        // Obtener UNA materia por su ID (versión asíncrona)
        public async System.Threading.Tasks.Task<Materia?> GetByIdAsync(int id)
        {
            // Consulta SQL para seleccionar una materia específica por ID
            const string sql = "SELECT Id, Descripcion, HorasSemanales, HorasTotales, IdPlan FROM Materias WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                // Añadir parámetro @Id para prevenir inyección SQL
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    // Si se encontró una fila, mapearla y devolverla
                    if (await reader.ReadAsync())
                    {
                        return MapFromReader(reader);
                    }
                    // Si no se encontró ninguna fila, devolver null
                    return null;
                }
            }
        }

        // Agregar una NUEVA materia
        public void Add(Materia materia)
        {
            // Consulta SQL INSERT. No incluimos el ID porque es autoincremental en la BD.
            // Opcional: Usar 'OUTPUT INSERTED.Id' si necesitas recuperar el ID generado.
            const string sql = @"INSERT INTO Materias (Descripcion, HorasSemanales, HorasTotales, IdPlan)
                                 VALUES (@Descripcion, @HorasSemanales, @HorasTotales, @IdPlan)";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                // Añadir parámetros con los valores de la materia
                command.Parameters.AddWithValue("@Descripcion", materia.Descripcion);
                command.Parameters.AddWithValue("@HorasSemanales", materia.HorasSemanales);
                command.Parameters.AddWithValue("@HorasTotales", materia.HorasTotales);
                command.Parameters.AddWithValue("@IdPlan", materia.IdPlan);

                connection.Open();
                command.ExecuteNonQuery(); // Ejecuta el comando INSERT (no devuelve filas)
            }
        }

        // Actualizar una materia EXISTENTE
        public void Update(Materia materia)
        {
            // Consulta SQL UPDATE para modificar una materia por su ID
            const string sql = @"UPDATE Materias SET
                                     Descripcion = @Descripcion,
                                     HorasSemanales = @HorasSemanales,
                                     HorasTotales = @HorasTotales,
                                     IdPlan = @IdPlan
                                 WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                // Añadir parámetros, incluyendo el ID para la cláusula WHERE
                command.Parameters.AddWithValue("@Descripcion", materia.Descripcion);
                command.Parameters.AddWithValue("@HorasSemanales", materia.HorasSemanales);
                command.Parameters.AddWithValue("@HorasTotales", materia.HorasTotales);
                command.Parameters.AddWithValue("@IdPlan", materia.IdPlan);
                command.Parameters.AddWithValue("@Id", materia.Id); // ID de la materia a actualizar

                connection.Open();
                command.ExecuteNonQuery(); // Ejecuta el comando UPDATE
            }
        }

        // Eliminar una materia por su ID
        public void Delete(int id)
        {
            // Consulta SQL DELETE para eliminar una materia por su ID
            const string sql = "DELETE FROM Materias WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                // Añadir el parámetro ID para la cláusula WHERE
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery(); // Ejecuta el comando DELETE
            }
        }
    }
}