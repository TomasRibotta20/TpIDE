using DTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Clients
{
    public class UsuarioApiClient : BaseApiClient
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public UsuarioApiClient()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync("usuarios");  // Cambiado de "api/usuarios" a "usuarios"

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<UsuarioDto>>(_jsonOptions)
                       ?? Enumerable.Empty<UsuarioDto>();
            }

            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de usuarios. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<UsuarioDto?> GetByIdAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync($"usuarios/{id}");  // Cambiado

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UsuarioDto>(_jsonOptions);
            }

            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener usuario con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        /// <summary>
        /// Obtiene todos los módulos disponibles del sistema
        /// </summary>
        public async Task<IEnumerable<ModuloDto>> GetModulosAsync()
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync("usuarios/modulos");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<ModuloDto>>(_jsonOptions)
                       ?? Enumerable.Empty<ModuloDto>();
            }

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener módulos. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        /// <summary>
        /// Obtiene los tipos de usuario predefinidos (Administrador, Profesor, Alumno)
        /// </summary>
        public async Task<IEnumerable<string>> GetTiposUsuarioAsync()
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync("usuarios/tipos");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<string>>(_jsonOptions)
                       ?? Enumerable.Empty<string>();
            }

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener tipos de usuario. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<UsuarioDto> CreateAsync(UsuarioDto usuario)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PostAsJsonAsync("usuarios", usuario);  // Cambiado

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear usuario. Status: {response.StatusCode}, Detalle: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<UsuarioDto>(_jsonOptions)
                   ?? throw new InvalidOperationException("No se pudo crear el usuario");
        }

        public async Task UpdateAsync(UsuarioDto usuario)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PutAsJsonAsync($"usuarios/{usuario.Id}", usuario);  // Cambiado

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar usuario con Id {usuario.Id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.DeleteAsync($"usuarios/{id}");  // Cambiado

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar usuario con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }
    }
}
