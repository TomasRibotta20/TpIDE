using DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace API.Clients
{
    public class EspecialidadApiClient : BaseApiClient
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<IEnumerable<EspecialidadDto>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync("especialidades");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<EspecialidadDto>>(_jsonOptions)
                       ?? Enumerable.Empty<EspecialidadDto>();
            }
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de especialidades. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<EspecialidadDto?> GetByIdAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync($"especialidades/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<EspecialidadDto>(_jsonOptions);
            }
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener especialidad con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }
        
        public async Task<EspecialidadDto> CreateAsync(EspecialidadDto especialidad)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PostAsJsonAsync("especialidades", especialidad);
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear especialidad. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            
            return await response.Content.ReadFromJsonAsync<EspecialidadDto>(_jsonOptions)
                   ?? throw new InvalidOperationException("No se pudo crear la especialidad");
        }

        public async Task UpdateAsync(EspecialidadDto especialidad)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PutAsJsonAsync($"especialidades/{especialidad.Id}", especialidad);
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar especialidad con Id {especialidad.Id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.DeleteAsync($"especialidades/{id}");
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar especialidad con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }
    }
}