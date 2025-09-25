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

        protected readonly JsonSerializerOptions _jsonOptions;
        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync("usuarios");

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
            HttpResponseMessage response = await client.GetAsync($"usuarios/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UsuarioDto>(_jsonOptions);
            }

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener usuario con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<UsuarioDto> CreateAsync(UsuarioDto usuario)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PostAsJsonAsync("usuarios", usuario);

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
            HttpResponseMessage response = await client.PutAsJsonAsync($"usuarios/{usuario.Id}", usuario);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar usuario con Id {usuario.Id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.DeleteAsync($"usuarios/{id}");

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar usuario con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }
    }
}
