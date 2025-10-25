using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace API.Clients
{
    public abstract class BaseApiClient
    {
        protected static async Task<HttpClient> CreateHttpClientAsync(bool requireAuth = true)
        {
            var client = new HttpClient();
            await ConfigureHttpClientAsync(client, requireAuth);
            return client;
        }

        protected static async Task ConfigureHttpClientAsync(HttpClient client, bool requireAuth = true)
        {
            
            string baseUrl = GetBaseUrlFromConfig();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            if (requireAuth)
            {
                await AddAuthorizationHeaderAsync(client);
            }
        }

        private static string GetBaseUrlFromConfig()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Intentando leer configuración...");

                
                string? envUrl = Environment.GetEnvironmentVariable("TPI_API_BASE_URL");
                if (!string.IsNullOrEmpty(envUrl))
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] URL desde variable de entorno: {envUrl}");
                    return envUrl;
                }

                // Intentar leer desde appsettings.json
                try
                {
                    string appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Buscando appsettings.json en: {appSettingsPath}");
                    
                    if (File.Exists(appSettingsPath))
                    {
                        var configuration = new ConfigurationBuilder()
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .Build();

                        var baseUrl = configuration["ApiSettings:BaseUrl"];
                        if (!string.IsNullOrEmpty(baseUrl))
                        {
                            System.Diagnostics.Debug.WriteLine($"[DEBUG] URL desde appsettings.json: {baseUrl}");
                            return baseUrl;
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Archivo appsettings.json no encontrado");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Error leyendo appsettings.json: {ex.Message}");
                }

                
                string runtimeInfo = System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier;
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Runtime: {runtimeInfo}");

                if (runtimeInfo.StartsWith("android"))
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Detectado Android - usando IP de emulador");
                    return "http://10.0.2.2:5183/";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Error detectando plataforma: {ex.Message}");
            }

            // URL por defecto para Windows/otras plataformas
            string defaultUrl = "http://localhost:5000/";
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Usando URL por defecto: {defaultUrl}");
            return defaultUrl;
        }

        protected static async Task AddAuthorizationHeaderAsync(HttpClient client)
        {
            try
            {
                var authService = AuthServiceProvider.Instance;

                // Verificar expiración antes de usar el token
                await authService.CheckTokenExpirationAsync();

                var token = await authService.GetTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (InvalidOperationException)
            {
                // AuthService no está registrado, no agregamos header de autorización
                // Esto puede ocurrir durante el login
                System.Diagnostics.Debug.WriteLine("[DEBUG] AuthService no registrado, saltando autorización");
            }
        }

        protected static async Task EnsureAuthenticatedAsync()
        {
            var authService = AuthServiceProvider.Instance;

            // Verificar expiración primero
            await authService.CheckTokenExpirationAsync();

            if (!await authService.IsAuthenticatedAsync())
            {
                throw new UnauthorizedAccessException("Su sesión ha expirado.");
            }
        }

        protected static async Task HandleUnauthorizedResponseAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Limpiar sesión actual
                var authService = AuthServiceProvider.Instance;
                await authService.LogoutAsync();

                // Lanzar excepción con mensaje simple
                throw new UnauthorizedAccessException("Su sesión ha expirado.");
            }
        }
    }
}
