using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Clients
{
    public class ComisionApiClient : BaseApiClient
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<IEnumerable<DTOs.ComisionDto>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync("comisiones");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<DTOs.ComisionDto>>(_jsonOptions)
                       ?? Enumerable.Empty<DTOs.ComisionDto>();
            }
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de comisiones. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<DTOs.ComisionDto?> GetByIdAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync($"comisiones/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<DTOs.ComisionDto>(_jsonOptions);
            }
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener comision con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<DTOs.ComisionDto> CreateAsync(DTOs.ComisionDto comision)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PostAsJsonAsync("comisiones", comision);
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear comision. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
            return await response.Content.ReadFromJsonAsync<DTOs.ComisionDto>(_jsonOptions)
                   ?? throw new InvalidOperationException("No se pudo crear la comision");
        }

        public async Task UpdateAsync(DTOs.ComisionDto comision)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PutAsJsonAsync($"comisiones/{comision.IdComision}", comision);
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar comision con Id {comision.IdComision}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }
        public async Task DeleteAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.DeleteAsync($"comisiones/{id}");
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar comision con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }


    }
}
