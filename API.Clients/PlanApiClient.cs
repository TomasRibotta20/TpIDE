using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Clients
{
    public class PlanApiClient : BaseApiClient
    {
        protected readonly JsonSerializerOptions _jsonOptions;

        public async Task<IEnumerable<PlanDto>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync("planes");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<PlanDto>>(_jsonOptions)
                       ?? Enumerable.Empty<PlanDto>();
            }
            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de planes. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<PlanDto?> GetByIdAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync($"planes/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PlanDto>(_jsonOptions);
            }
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener plan con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<PlanDto> CreateAsync(PlanDto plan)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PostAsJsonAsync("planes", plan);
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear plan. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            return await response.Content.ReadFromJsonAsync<PlanDto>(_jsonOptions)
                   ?? throw new InvalidOperationException("No se pudo crear el plan");
        }

        public async Task UpdateAsync(PlanDto plan)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PutAsJsonAsync($"planes/{plan.Id}", plan);
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar plan con Id {plan.Id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.DeleteAsync($"planes/{id}");
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar plan con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

    }
}
