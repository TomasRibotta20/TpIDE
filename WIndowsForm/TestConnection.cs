using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WIndowsForm
{
    public static class TestConnection
    {
        public static async Task<bool> TestApiConnectionAsync()
        {
            try
            {
                // Leer la URL base desde appsettings.json
                string baseUrl = GetBaseUrlFromConfig();
                
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                
                Console.WriteLine($"Testing API connection to: {baseUrl}");
                
                // Probar endpoint de health o swagger
                var testUrls = new[]
                {
                    $"{baseUrl}swagger/index.html",
                    $"{baseUrl}swagger",
                    baseUrl.TrimEnd('/')
                };
                
                foreach (var url in testUrls)
                {
                    try
                    {
                        Console.WriteLine($"Trying: {url}");
                        var response = await client.GetAsync(url);
                        Console.WriteLine($"Response: {response.StatusCode}");
                        
                        if (response.IsSuccessStatusCode || 
                            response.StatusCode == System.Net.HttpStatusCode.OK ||
                            response.StatusCode == System.Net.HttpStatusCode.NotFound) // Swagger puede devolver 404 si está deshabilitado
                        {
                            Console.WriteLine("API is reachable!");
                            return true;
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Failed for {url}: {ex.Message}");
                        continue;
                    }
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                return false;
            }
        }
        
        private static string GetBaseUrlFromConfig()
        {
            try
            {
                string appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                
                if (File.Exists(appSettingsPath))
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

                    var baseUrl = configuration["ApiSettings:BaseUrl"];
                    if (!string.IsNullOrEmpty(baseUrl))
                    {
                        return baseUrl.EndsWith("/") ? baseUrl : baseUrl + "/";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading appsettings.json: {ex.Message}");
            }
            
            // URL por defecto
            return "http://localhost:5000/";
        }
    }
}
