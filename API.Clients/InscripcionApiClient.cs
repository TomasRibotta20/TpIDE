using DTOs;
using System.Text.Json;

namespace API.Clients
{
    public class InscripcionApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public InscripcionApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7229/");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<IEnumerable<AlumnoCursoDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("inscripciones");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<AlumnoCursoDto>>(json, _jsonOptions) ?? new List<AlumnoCursoDto>();
        }

        public async Task<AlumnoCursoDto?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"inscripciones/{id}");
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
            
            var response = await _httpClient.PostAsync("inscripciones", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                
                // Intentar extraer el mensaje de error del contenido
                try
                {
                    if (errorContent.Contains("ya est� inscripto"))
                        throw new Exception("ALUMNO_YA_INSCRIPTO");
                        
                    if (errorContent.Contains("No hay cupo disponible"))
                        throw new Exception("CUPO_AGOTADO");
                        
                    if (errorContent.Contains("a�os anteriores"))
                        throw new Exception("CURSO_A�O_ANTERIOR");

                    if (errorContent.Contains("no existe"))
                        throw new Exception("CURSO_O_ALUMNO_NO_ENCONTRADO");

                    // Si el mensaje es descriptivo y no contiene detalles t�cnicos, usarlo directamente
                    if (!errorContent.Contains("Exception") && !errorContent.Contains("System.") && 
                        !errorContent.Contains("Microsoft.") && !errorContent.Contains("Stack"))
                    {
                        throw new Exception(errorContent.Replace("Error al inscribir alumno: ", ""));
                    }

                    // Si llegamos aqu�, intentar deserializar el error
                    var errorObj = JsonSerializer.Deserialize<JsonElement>(errorContent);
                    if (errorObj.TryGetProperty("detail", out var detail))
                    {
                        var detailMessage = detail.GetString();
                        if (detailMessage?.Contains("ya est� inscripto") == true)
                            throw new Exception("ALUMNO_YA_INSCRIPTO");
                        if (detailMessage?.Contains("No hay cupo") == true)
                            throw new Exception("CUPO_AGOTADO");
                        if (detailMessage?.Contains("a�os anteriores") == true)
                            throw new Exception("CURSO_A�O_ANTERIOR");
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
                
                // Si no pudimos extraer un mensaje espec�fico, usar un mensaje gen�rico
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
            
            var response = await _httpClient.PutAsync($"inscripciones/{idInscripcion}/condicion", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                
                // Casos espec�ficos por c�digo de estado
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        throw new Exception("INSCRIPCION_NO_ENCONTRADA");
                    case System.Net.HttpStatusCode.BadRequest:
                        // Intentar extraer mensaje espec�fico del BadRequest
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
                            // Si no es JSON v�lido, usar el contenido como est�
                            if (!string.IsNullOrEmpty(errorContent))
                            {
                                throw new Exception(errorContent);
                            }
                        }
                        throw new Exception("DATOS_INVALIDOS");
                    case System.Net.HttpStatusCode.InternalServerError:
                        throw new Exception("ERROR_SERVIDOR");
                    default:
                        // Para otros c�digos, intentar extraer mensaje
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
            var response = await _httpClient.DeleteAsync($"inscripciones/{idInscripcion}");
            
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
            var response = await _httpClient.GetAsync($"inscripciones/alumno/{idAlumno}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<AlumnoCursoDto>>(json, _jsonOptions) ?? new List<AlumnoCursoDto>();
        }

        public async Task<IEnumerable<AlumnoCursoDto>> GetInscripcionesByCursoAsync(int idCurso)
        {
            var response = await _httpClient.GetAsync($"inscripciones/curso/{idCurso}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<AlumnoCursoDto>>(json, _jsonOptions) ?? new List<AlumnoCursoDto>();
        }

        public async Task<Dictionary<string, int>> GetEstadisticasGeneralesAsync()
        {
            var response = await _httpClient.GetAsync("inscripciones/estadisticas");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, int>>(json, _jsonOptions) ?? new Dictionary<string, int>();
        }
    }
}