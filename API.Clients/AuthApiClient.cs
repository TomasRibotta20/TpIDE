using DTOs;
using System.Text;
using System.Text.Json;

namespace API.Clients
{
    public class AuthApiClient : BaseApiClient
    {
        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            try
            {
                // Don't require auth for login endpoint
                using var httpClient = await CreateHttpClientAsync(requireAuth: false);

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                System.Diagnostics.Debug.WriteLine($"[DEBUG] Calling login endpoint with user: {request.Username}");
                var response = await httpClient.PostAsync("/auth/login", content);

                System.Diagnostics.Debug.WriteLine($"[DEBUG] Login response status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Login successful, response: {responseContent}");

                    return JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Login failed: {response.StatusCode} - {errorContent}");

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("Credenciales incorrectas. Verifique su nombre de usuario y contraseña.");
                    }
                    else
                    {
                        throw new Exception($"Error en el servidor: {response.StatusCode}" +
                            (!string.IsNullOrEmpty(errorContent) ? $" - {errorContent}" : ""));
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Re-throw unauthorized exceptions directly
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Login exception: {ex.Message}");

                // Check if it's a network-related error
                if (ex is System.Net.Http.HttpRequestException)
                {
                    throw new Exception($"Error de conexión: No se pudo conectar al servidor. Verifique que la API esté funcionando.");
                }

                throw;
            }
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                // Don't require auth for register endpoint
                using var httpClient = await CreateHttpClientAsync(requireAuth: false);

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                System.Diagnostics.Debug.WriteLine($"[DEBUG] Calling register endpoint for user: {request.UsuarioNombre}");
                var response = await httpClient.PostAsync("/auth/register", content);

                System.Diagnostics.Debug.WriteLine($"[DEBUG] Register response status: {response.StatusCode}");

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Register successful, response: {responseContent}");

                    return JsonSerializer.Deserialize<RegisterResponseDto>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new RegisterResponseDto { Success = false, Message = "Error al deserializar respuesta" };
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Register failed: {response.StatusCode} - {responseContent}");
                    
                    // Try to deserialize error response
                    try
                    {
                        return JsonSerializer.Deserialize<RegisterResponseDto>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }) ?? new RegisterResponseDto { Success = false, Message = "Error desconocido" };
                    }
                    catch
                    {
                        return new RegisterResponseDto
                        {
                            Success = false,
                            Message = $"Error en el servidor: {response.StatusCode}"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Register exception: {ex.Message}");

                return new RegisterResponseDto
                {
                    Success = false,
                    Message = $"Error de conexión: {ex.Message}"
                };
            }
        }

        public async Task<Dictionary<string, List<string>>> GetModulePermissionsAsync(string token)
        {
            try
            {
                using var httpClient = await CreateHttpClientAsync(requireAuth: true);
                httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync("/auth/permissions");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Dictionary<string, List<string>>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new Dictionary<string, List<string>>();
                }
                else
                {
                    return new Dictionary<string, List<string>>();
                }
            }
            catch (Exception)
            {
                return new Dictionary<string, List<string>>();
            }
        }
    }
}