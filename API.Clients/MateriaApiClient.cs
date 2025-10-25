using DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Text.Json;
using System.Net; 

namespace API.Clients
{
    public class MateriaApiClient : BaseApiClient
    {
        
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

       
        public async Task<IEnumerable<MateriaDto>> GetAllAsync()
        {
           
            using var client = await CreateHttpClientAsync();
           
            HttpResponseMessage response = await client.GetAsync("materias");

           
            if (response.IsSuccessStatusCode)
            {
                
                return await response.Content.ReadFromJsonAsync<IEnumerable<MateriaDto>>(_jsonOptions)
                       ?? Enumerable.Empty<MateriaDto>();
            }

            
            await HandleUnauthorizedResponseAsync(response);

          
            string errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error en GetAllAsync: Status {response.StatusCode}, Content: {errorContent}"); 
                                                                                                               
            try
            {
                var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions);
                if (problem != null && !string.IsNullOrEmpty(problem.Detail))
                {
                    throw new Exception($"Error al obtener lista de materias: {problem.Detail} (Status: {response.StatusCode})");
                }
            }
            catch (JsonException) { /* Ignorar si no es ProblemDetails */ }

            throw new Exception($"Error al obtener lista de materias. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        
        public async Task<MateriaDto?> GetByIdAsync(int id) 
        {
            using var client = await CreateHttpClientAsync();
            
            HttpResponseMessage response = await client.GetAsync($"materias/{id}");

            if (response.IsSuccessStatusCode)
            {
              
                return await response.Content.ReadFromJsonAsync<MateriaDto>(_jsonOptions);
            }

          
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

           
            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error en GetByIdAsync({id}): Status {response.StatusCode}, Content: {errorContent}"); 
            try
            {
                var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions);
                if (problem != null && !string.IsNullOrEmpty(problem.Detail))
                {
                    throw new Exception($"Error al obtener materia con ID {id}: {problem.Detail} (Status: {response.StatusCode})");
                }
            }
            catch (JsonException) { /* Ignorar */ }
            throw new Exception($"Error al obtener materia con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

      
        public async Task<MateriaDto> CreateAsync(MateriaDto materia)
        {
            using var client = await CreateHttpClientAsync();
            
            HttpResponseMessage response = await client.PostAsJsonAsync("materias", materia);

            
            if (!response.IsSuccessStatusCode)
            {
                await HandleUnauthorizedResponseAsync(response);
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error en CreateAsync: Status {response.StatusCode}, Content: {errorContent}"); 
                try
                {
                    
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(_jsonOptions);
                    if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Message))
                    {
                        
                        throw new ArgumentException($"Error al crear materia: {errorResponse.Message}");
                    }
                    var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions);
                    if (problem != null && !string.IsNullOrEmpty(problem.Detail))
                    {
                        throw new Exception($"Error al crear materia: {problem.Detail} (Status: {response.StatusCode})");
                    }
                }
                catch (JsonException) { /* Ignorar si el formato no es el esperado */ }
               
                throw new Exception($"Error al crear materia. Status: {response.StatusCode}, Detalle: {errorContent}");
            }

            
            var createdMateria = await response.Content.ReadFromJsonAsync<MateriaDto>(_jsonOptions);
            if (createdMateria == null)
            {
                Console.WriteLine("Error en CreateAsync: No se pudo deserializar la respuesta JSON.");
                throw new InvalidOperationException("No se pudo deserializar la respuesta JSON al crear la materia.");
            }
            return createdMateria; 
        }

        public async Task UpdateAsync(MateriaDto materia)
        {
            using var client = await CreateHttpClientAsync();
           
            HttpResponseMessage response = await client.PutAsJsonAsync($"materias/{materia.Id}", materia);

            
            if (!response.IsSuccessStatusCode)
            {
                await HandleUnauthorizedResponseAsync(response);
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error en UpdateAsync({materia.Id}): Status {response.StatusCode}, Content: {errorContent}"); 
                try
                {
                    
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(_jsonOptions);
                    if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Message))
                    {
                        
                        if (response.StatusCode == HttpStatusCode.NotFound) throw new KeyNotFoundException($"Error al actualizar materia: {errorResponse.Message}");
                        else throw new ArgumentException($"Error al actualizar materia: {errorResponse.Message}");
                    }
                    var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions);
                    if (problem != null && !string.IsNullOrEmpty(problem.Detail))
                    {
                        throw new Exception($"Error al actualizar materia: {problem.Detail} (Status: {response.StatusCode})");
                    }
                }
                catch (JsonException) { /* Ignorar */ }
                throw new Exception($"Error al actualizar materia con Id {materia.Id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            
        }

    
        public async Task DeleteAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
           
            HttpResponseMessage response = await client.DeleteAsync($"materias/{id}");

           
            if (!response.IsSuccessStatusCode)
            {
                await HandleUnauthorizedResponseAsync(response);
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error en DeleteAsync({id}): Status {response.StatusCode}, Content: {errorContent}"); 
                try
                {
                    
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(_jsonOptions);
                    if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Message))
                    {
                        
                        if (response.StatusCode == HttpStatusCode.NotFound) throw new KeyNotFoundException($"Error al eliminar materia: {errorResponse.Message}");
                        
                        else throw new Exception($"Error al eliminar materia: {errorResponse.Message} (Status: {response.StatusCode})");
                    }
                    var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions);
                    if (problem != null && !string.IsNullOrEmpty(problem.Detail))
                    {
                        throw new Exception($"Error al eliminar materia: {problem.Detail} (Status: {response.StatusCode})");
                    }
                }
                catch (JsonException) { /* Ignorar */ }
                throw new Exception($"Error al eliminar materia con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            
        }

        private class ErrorResponse { public string? Message { get; set; } } 
        private class ProblemDetails { public string? Detail { get; set; } } 
    }
}

