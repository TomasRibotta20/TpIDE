using DTOs;
using System.Text;
using System.Text.Json;

namespace API.Clients
{
    public class InscripcionApiClient : BaseApiClient
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public InscripcionApiClient()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<IEnumerable<AlumnoCursoDto>> GetAllAsync()
        {
            var client = await CreateHttpClientAsync();
            var response = await client.GetAsync("inscripciones");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<AlumnoCursoDto>>(json, _jsonOptions) ?? new List<AlumnoCursoDto>();
        }

        public async Task<AlumnoCursoDto?> GetByIdAsync(int id)
        {
            var client = await CreateHttpClientAsync();
            var response = await client.GetAsync($"inscripciones/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AlumnoCursoDto>(json, _jsonOptions);
        }

        public async Task<AlumnoCursoDto> InscribirAlumnoAsync(int idAlumno, int idCurso, CondicionAlumnoDto condicion = CondicionAlumnoDto.Regular)
        {
            var inscripcionRequest = new
            {
                IdAlumno = idAlumno,
                IdCurso = idCurso,
                Condicion = condicion.ToString()
            };

            var json = JsonSerializer.Serialize(inscripcionRequest, _jsonOptions);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var client = await CreateHttpClientAsync();
            var response = await client.PostAsync("inscripciones", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                
                // Intentar extraer el mensaje de error del contenido
                try
                {
                    if (errorContent.Contains("ya está inscripto"))
                        throw new Exception("ALUMNO_YA_INSCRIPTO");
                        
                    if (errorContent.Contains("No hay cupo disponible"))
                        throw new Exception("CUPO_AGOTADO");
                        
                    if (errorContent.Contains("años anteriores"))
                        throw new Exception("CURSO_AÑO_ANTERIOR");

                    if (errorContent.Contains("no existe"))
                        throw new Exception("CURSO_O_ALUMNO_NO_ENCONTRADO");

                    // Si el mensaje es descriptivo y no contiene detalles técnicos, usarlo directamente
                    if (!errorContent.Contains("Exception") && !errorContent.Contains("System.") && 
                        !errorContent.Contains("Microsoft.") && !errorContent.Contains("Stack"))
                    {
                        throw new Exception(errorContent.Replace("Error al inscribir alumno: ", ""));
                    }

                    // Si llegamos aquí, intentar deserializar el error
                    var errorObj = JsonSerializer.Deserialize<JsonElement>(errorContent);
                    if (errorObj.TryGetProperty("detail", out var detail))
                    {
                        var detailMessage = detail.GetString();
                        if (detailMessage?.Contains("ya está inscripto") == true)
                            throw new Exception("ALUMNO_YA_INSCRIPTO");
                        if (detailMessage?.Contains("No hay cupo") == true)
                            throw new Exception("CUPO_AGOTADO");
                        if (detailMessage?.Contains("años anteriores") == true)
                            throw new Exception("CURSO_AÑO_ANTERIOR");
                        throw new Exception(detailMessage ?? "DATOS_INVALIDOS");
                    }
                }
                catch (JsonException)
                {
                    // Si no podemos deserializar el error pero tenemos contenido, usarlo
                    if (!string.IsNullOrEmpty(errorContent))
                    {
                        throw new Exception(errorContent);
                    }
                }
                
                // Si no pudimos extraer un mensaje específico, usar un mensaje genérico
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        throw new Exception("CURSO_O_ALUMNO_NO_ENCONTRADO");
                    case System.Net.HttpStatusCode.BadRequest:
                        throw new Exception("DATOS_INVALIDOS");
                    case System.Net.HttpStatusCode.InternalServerError:
                        throw new Exception("ERROR_SERVIDOR");
                    default:
                        throw new Exception($"ERROR_{(int)response.StatusCode}");
                }
            }
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AlumnoCursoDto>(responseJson, _jsonOptions) ?? throw new Exception("ERROR_DESERIALIZACION");
        }

        public async Task ActualizarCondicionYNotaAsync(int idInscripcion, CondicionAlumnoDto condicion, int? nota = null)
        {
            var updateRequest = new
            {
                Condicion = condicion.ToString(),
                Nota = nota
            };

            var json = JsonSerializer.Serialize(updateRequest, _jsonOptions);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var client = await CreateHttpClientAsync();
            var response = await client.PutAsync($"inscripciones/{idInscripcion}/condicion", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                
                // Casos específicos por código de estado
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        throw new Exception("INSCRIPCION_NO_ENCONTRADA");
                    case System.Net.HttpStatusCode.BadRequest:
                        // Intentar extraer mensaje específico del BadRequest
                        try
                        {
                            var errorObj = JsonSerializer.Deserialize<JsonElement>(errorContent);
                            if (errorObj.TryGetProperty("detail", out var detail))
                            {
                                throw new Exception(detail.GetString() ?? "DATOS_INVALIDOS");
                            }
                            if (errorObj.TryGetProperty("message", out var message))
                            {
                                throw new Exception(message.GetString() ?? "DATOS_INVALIDOS");
                            }
                            if (errorObj.TryGetProperty("error", out var error))
                            {
                                throw new Exception(error.GetString() ?? "DATOS_INVALIDOS");
                            }
                        }
                        catch (JsonException)
                        {
                            // Si no es JSON válido, usar el contenido como está
                            if (!string.IsNullOrEmpty(errorContent))
                            {
                                throw new Exception(errorContent);
                            }
                        }
                        throw new Exception("DATOS_INVALIDOS");
                    case System.Net.HttpStatusCode.InternalServerError:
                        throw new Exception("ERROR_SERVIDOR");
                    default:
                        // Para otros códigos, intentar extraer mensaje
                        if (!string.IsNullOrEmpty(errorContent))
                        {
                            throw new Exception(errorContent);
                        }
                        throw new Exception($"HTTP_ERROR_{(int)response.StatusCode}");
                }
            }
        }

        public async Task DesinscribirAlumnoAsync(int idInscripcion)
        {
            var client = await CreateHttpClientAsync();
            var response = await client.DeleteAsync($"inscripciones/{idInscripcion}");
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        throw new Exception("INSCRIPCION_NO_ENCONTRADA");
                    case System.Net.HttpStatusCode.BadRequest:
                        throw new Exception("NO_SE_PUEDE_DESINSCRIBIR");
                    case System.Net.HttpStatusCode.InternalServerError:
                        throw new Exception("ERROR_SERVIDOR");
                    default:
                        if (!string.IsNullOrEmpty(errorContent))
                        {
                            throw new Exception(errorContent);
                        }
                        throw new Exception($"HTTP_ERROR_{(int)response.StatusCode}");
                }
            }
        }

        public async Task<IEnumerable<AlumnoCursoDto>> GetInscripcionesByAlumnoAsync(int idAlumno)
        {
            var client = await CreateHttpClientAsync();
            var response = await client.GetAsync($"inscripciones/alumno/{idAlumno}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<AlumnoCursoDto>>(json, _jsonOptions) ?? new List<AlumnoCursoDto>();
        }

        public async Task<IEnumerable<AlumnoCursoDto>> GetInscripcionesByCursoAsync(int idCurso)
        {
            var client = await CreateHttpClientAsync();
            var response = await client.GetAsync($"inscripciones/curso/{idCurso}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<AlumnoCursoDto>>(json, _jsonOptions) ?? new List<AlumnoCursoDto>();
        }

        public async Task<Dictionary<string, int>> GetEstadisticasGeneralesAsync()
        {
            var client = await CreateHttpClientAsync();
            var response = await client.GetAsync("inscripciones/estadisticas");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, int>>(json, _jsonOptions) ?? new Dictionary<string, int>();
        }

        // Métodos adicionales para compatibilidad con formularios
        public async Task<List<AlumnoCursoDto>> GetByAlumnoIdAsync(int alumnoId)
        {
            var result = await GetInscripcionesByAlumnoAsync(alumnoId);
            return result.ToList();
        }

        public async Task<List<AlumnoCursoDto>> GetByCursoIdAsync(int cursoId)
        {
            var result = await GetInscripcionesByCursoAsync(cursoId);
            return result.ToList();
        }

        public async Task UpdateCondicionAsync(int idInscripcion, CondicionAlumnoDto condicion, int? nota)
        {
            await ActualizarCondicionYNotaAsync(idInscripcion, condicion, nota);
        }

        public async Task CreateAsync(AlumnoCursoDto inscripcion)
        {
            await InscribirAlumnoAsync(inscripcion.IdAlumno, inscripcion.IdCurso, inscripcion.Condicion);
        }
    }
}