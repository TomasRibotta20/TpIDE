using DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace API.Clients
{
    public class EspecialidadApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public EspecialidadApiClient(string baseUrl)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(handler);
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<IEnumerable<EspecialidadDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/especialidades");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<EspecialidadDto>>(_jsonOptions)
                ?? Enumerable.Empty<EspecialidadDto>();
        }

        public async Task<EspecialidadDto?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/especialidades/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EspecialidadDto>(_jsonOptions);
        }

        public async Task<EspecialidadDto> CreateAsync(EspecialidadDto especialidad)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/especialidades", especialidad);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EspecialidadDto>(_jsonOptions)
                ?? throw new InvalidOperationException("No se pudo crear la especialidad");
        }

        public async Task UpdateAsync(EspecialidadDto especialidad)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/especialidades/{especialidad.Id}", especialidad);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/especialidades/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}