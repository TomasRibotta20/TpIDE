using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WIndowsForm
{
    public static class TestConnection
    {
        public static async Task<bool> TestApiConnectionAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                
                // Intentar con HTTPS primero
                try
                {
                    var response = await client.GetAsync("https://localhost:7229/swagger/index.html");
                    Console.WriteLine($"HTTPS Response: {response.StatusCode}");
                    return response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.OK;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTPS failed: {ex.Message}");
                    
                    // Intentar con HTTP
                    try
                    {
                        var response = await client.GetAsync("http://localhost:5230/swagger/index.html");
                        Console.WriteLine($"HTTP Response: {response.StatusCode}");
                        return response.IsSuccessStatusCode;
                    }
                    catch (HttpRequestException httpEx)
                    {
                        Console.WriteLine($"HTTP también falló: {httpEx.Message}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                return false;
            }
        }
    }
}
