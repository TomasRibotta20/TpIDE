using DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace API.Clients
{
    public class DocenteCursoApiClient : BaseApiClient
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<IEnumerable<DocenteCursoDto>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync("docentes-cursos");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<DocenteCursoDto>>(_jsonOptions)
                       ?? Enumerable.Empty<DocenteCursoDto>();
            }
            
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener asignaciones. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<DocenteCursoDto?> GetByIdAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync($"docentes-cursos/{id}");
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<DocenteCursoDto>(_jsonOptions);
            }
            
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener asignación con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<IEnumerable<DocenteCursoDto>> GetByCursoIdAsync(int cursoId)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync($"docentes-cursos/curso/{cursoId}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<DocenteCursoDto>>(_jsonOptions)
                       ?? Enumerable.Empty<DocenteCursoDto>();
            }
            
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener docentes del curso {cursoId}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<IEnumerable<DocenteCursoDto>> GetByDocenteIdAsync(int docenteId)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync($"docentes-cursos/docente/{docenteId}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<DocenteCursoDto>>(_jsonOptions)
                       ?? Enumerable.Empty<DocenteCursoDto>();
            }
            
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener cursos del docente {docenteId}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<DocenteCursoDto> CreateAsync(DocenteCursoCreateDto createDto)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.PostAsJsonAsync("docentes-cursos", createDto, _jsonOptions);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al asignar docente. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            
            return await response.Content.ReadFromJsonAsync<DocenteCursoDto>(_jsonOptions)
                   ?? throw new InvalidOperationException("No se pudo crear la asignación");
        }

        public async Task<DocenteCursoDto> UpdateAsync(int id, DocenteCursoCreateDto updateDto)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.PutAsJsonAsync($"docentes-cursos/{id}", updateDto, _jsonOptions);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar asignación con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            
            return await response.Content.ReadFromJsonAsync<DocenteCursoDto>(_jsonOptions)
                   ?? throw new InvalidOperationException("No se pudo actualizar la asignación");
        }

        public async Task DeleteAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.DeleteAsync($"docentes-cursos/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar asignación con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }
    }
}
