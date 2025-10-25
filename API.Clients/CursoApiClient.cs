using DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace API.Clients
{
    public class CursoApiClient : BaseApiClient
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<IEnumerable<CursoDto>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync("cursos");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<CursoDto>>(_jsonOptions)
                       ?? Enumerable.Empty<CursoDto>();
            }
            
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de cursos. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<CursoDto?> GetByIdAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync($"cursos/{id}");
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CursoDto>(_jsonOptions);
            }
            
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener curso con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<CursoDto> CreateAsync(CursoDto curso)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.PostAsJsonAsync("cursos", curso, _jsonOptions);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear curso. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            
            return await response.Content.ReadFromJsonAsync<CursoDto>(_jsonOptions)
                   ?? throw new InvalidOperationException("No se pudo crear el curso");
        }

        public async Task UpdateAsync(CursoDto curso)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.PutAsJsonAsync($"cursos/{curso.IdCurso}", curso, _jsonOptions);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar curso con Id {curso.IdCurso}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.DeleteAsync($"cursos/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar curso con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task<IEnumerable<CursoDto>> GetByComisionAsync(int idComision)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync($"cursos/comision/{idComision}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<CursoDto>>(_jsonOptions)
                       ?? Enumerable.Empty<CursoDto>();
            }
            
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener cursos de la comisión {idComision}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<IEnumerable<CursoDto>> GetByAnioCalendarioAsync(int anioCalendario)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync($"cursos/anio/{anioCalendario}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<CursoDto>>(_jsonOptions)
                       ?? Enumerable.Empty<CursoDto>();
            }
            
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener cursos del año {anioCalendario}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }
    }
}