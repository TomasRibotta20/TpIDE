using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace API.Clients
{
    public class PersonaApiClient : BaseApiClient
    {
        // --- Todas las personas ---
        public async Task<IEnumerable<PersonaDto>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync("personas");
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener personas. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            
            return await response.Content.ReadFromJsonAsync<IEnumerable<PersonaDto>>()
                   ?? Enumerable.Empty<PersonaDto>();
        }

        // --- Alumnos ---
        public async Task<IEnumerable<PersonaDto>> GetAllAlumnosAsync()
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync("personas/alumnos");
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener alumnos. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            
            return await response.Content.ReadFromJsonAsync<IEnumerable<PersonaDto>>()
                   ?? Enumerable.Empty<PersonaDto>();
        }

        // --- Profesores ---
        public async Task<IEnumerable<PersonaDto>> GetAllProfesoresAsync()
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync("personas/profesores");
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener profesores. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            
            return await response.Content.ReadFromJsonAsync<IEnumerable<PersonaDto>>()
                   ?? Enumerable.Empty<PersonaDto>();
        }

        // --- Genérico ---
        public async Task<PersonaDto> GetByIdAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync($"personas/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener persona con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            
            return await response.Content.ReadFromJsonAsync<PersonaDto>()
                   ?? throw new Exception("No se pudo deserializar la persona");
        }

        public async Task CreateAsync(PersonaDto persona)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.PostAsJsonAsync("personas", persona);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear persona. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task UpdateAsync(int id, PersonaDto persona)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.PutAsJsonAsync($"personas/{id}", persona);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar persona con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            var response = await client.DeleteAsync($"personas/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar persona con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }
    }
}