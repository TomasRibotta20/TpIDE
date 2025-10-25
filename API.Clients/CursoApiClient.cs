using DTOs;
using System.Text.Json;

namespace API.Clients
{
    public class CursoApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public CursoApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7229/");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<IEnumerable<CursoDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("cursos");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<CursoDto>>(json, _jsonOptions) ?? new List<CursoDto>();
        }

        public async Task<CursoDto?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"cursos/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CursoDto>(json, _jsonOptions);
        }

        public async Task<CursoDto> CreateAsync(CursoDto curso)
        {
            var json = JsonSerializer.Serialize(curso, _jsonOptions);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("cursos", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CursoDto>(responseJson, _jsonOptions) ?? throw new Exception("Error al deserializar la respuesta");
        }

        public async Task UpdateAsync(CursoDto curso)
        {
            var json = JsonSerializer.Serialize(curso, _jsonOptions);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"cursos/{curso.IdCurso}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"cursos/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<CursoDto>> GetByComisionAsync(int idComision)
        {
            var response = await _httpClient.GetAsync($"cursos/comision/{idComision}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<CursoDto>>(json, _jsonOptions) ?? new List<CursoDto>();
        }

        public async Task<IEnumerable<CursoDto>> GetByAnioCalendarioAsync(int anioCalendario)
        {
            var response = await _httpClient.GetAsync($"cursos/anio/{anioCalendario}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<CursoDto>>(json, _jsonOptions) ?? new List<CursoDto>();
        }
    }
}