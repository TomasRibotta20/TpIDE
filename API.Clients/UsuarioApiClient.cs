using DTOs;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace API.Clients
{
    public class UsuarioApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public UsuarioApiClient(string baseUrl)
        {
            // Configurar HttpClient para ignorar certificados inválidos en desarrollo
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(handler);
            _baseUrl = baseUrl.TrimEnd('/');
            Debug.WriteLine($"Cliente API inicializado con URL base: {_baseUrl}");
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            try
            {
                Debug.WriteLine($"Solicitando GET a {_baseUrl}/usuarios");
                var response = await _httpClient.GetAsync($"{_baseUrl}/usuarios");

                Debug.WriteLine($"Respuesta recibida: {(int)response.StatusCode} {response.ReasonPhrase}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Contenido recibido: {content}");

                var result = JsonSerializer.Deserialize<IEnumerable<UsuarioDto>>(content, _jsonOptions);
                return result ?? Enumerable.Empty<UsuarioDto>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en GetAllAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<UsuarioDto?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/usuarios/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UsuarioDto>(_jsonOptions);
        }

        public async Task<UsuarioDto> CreateAsync(UsuarioDto usuario)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/usuarios", usuario);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UsuarioDto>(_jsonOptions)
                ?? throw new InvalidOperationException("No se pudo crear el usuario");
        }

        public async Task UpdateAsync(UsuarioDto usuario)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/usuarios/{usuario.Id}", usuario);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/usuarios/{id}");
            response.EnsureSuccessStatusCode();
        }

        public static implicit operator UsuarioApiClient(EspecialidadApiClient v)
        {
            throw new NotImplementedException();
        }
    }
}