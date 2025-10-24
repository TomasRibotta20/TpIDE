using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace API.Clients
{
    public class PersonaApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed = false;

        public PersonaApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7229/") };
        }

        // --- Alumnos ---
        public async Task<IEnumerable<PersonaDto>> GetAllAlumnosAsync()
        {
            var response = await _httpClient.GetAsync("personas/alumnos");
            
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
            var response = await _httpClient.GetAsync("personas/profesores");
            
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
            var response = await _httpClient.GetAsync($"personas/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener persona con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            
            return await response.Content.ReadFromJsonAsync<PersonaDto>();
        }

        public async Task CreateAsync(PersonaDto persona)
        {
            var response = await _httpClient.PostAsJsonAsync("personas", persona);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear persona. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task UpdateAsync(int id, PersonaDto persona)
        {
            var response = await _httpClient.PutAsJsonAsync($"personas/{id}", persona);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar persona con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"personas/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar persona con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}