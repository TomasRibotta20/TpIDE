using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Auth.WindowsForms;
using API.Clients;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Data;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace WindowsForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            try
            {
                // Check if appsettings.json exists
                string appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (!File.Exists(appSettingsPath))
                {
                    MessageBox.Show(
                        $"No se encontró el archivo de configuración en:\n{appSettingsPath}\n\nSe creará uno con la configuración predeterminada.",
                        "Archivo de configuración faltante",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    // Create default appsettings.json
                    string defaultConfig = @"{
  ""ConnectionStrings"": {
    ""DefaultConnection"": ""Server=localhost,1433;Database=Universidad;User Id=sa;Password=TuContraseñaFuerte123;TrustServerCertificate=True""
  },
  ""Logging"": {
    ""LogLevel"": {
      ""Default"": ""Information"",
      ""Microsoft"": ""Warning"",
      ""Microsoft.Hosting.Lifetime"": ""Information""
    }
  },
  ""AllowedHosts"": ""*"",
  ""ApiSettings"": {
    ""BaseUrl"": ""https://localhost:7229""
  }
}";
                    File.WriteAllText(appSettingsPath, defaultConfig);
                    MessageBox.Show(
                        "Se ha creado un archivo de configuración predeterminado.",
                        "Configuración creada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                // Ensure database is created before starting the application
                EnsureDatabaseCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al iniciar la aplicación: {ex.Message}",
                    "Error de inicialización",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            Task.Run(async () => await MainAsync()).GetAwaiter().GetResult();
        }

        private static void EnsureDatabaseCreated()
        {
            try
            {
                using (var context = new AcademiaContext())
                {
                    MessageBox.Show("Intentando crear la base de datos...", "Inicialización", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Force recreation of the database
                    Console.WriteLine("Ensuring database exists and is up-to-date...");
                    bool created = context.Database.EnsureCreated();
                    
                    MessageBox.Show(created 
                        ? "Base de datos creada correctamente."
                        : "Base de datos existente verificada.", 
                        "Inicialización", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    
                    // Make sure we can actually query the Comisiones table
                    try
                    {
                        var comisiones = context.Comisiones.ToList();
                        Console.WriteLine($"Successfully queried Comisiones table. Found {comisiones.Count} records.");
                        MessageBox.Show($"Tabla Comisiones verificada. Encontrados {comisiones.Count} registros.", 
                            "Verificación", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error querying Comisiones table: {ex.Message}");
                        MessageBox.Show($"Error al consultar la tabla Comisiones: {ex.Message}\n\nSe intentará recrear la tabla.", 
                            "Error", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
                            
                        // Try to recreate just the Comisiones table by migrating
                        try
                        {
                            // First ensure the database exists
                            context.Database.EnsureDeleted();
                            context.Database.EnsureCreated();
                            
                            MessageBox.Show("Se ha recreado la base de datos para resolver el problema.", 
                                "Recuperación", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
                        }
                        catch (Exception innerEx)
                        {
                            MessageBox.Show($"Error al recrear la base de datos: {innerEx.Message}", 
                                "Error crítico", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                MessageBox.Show($"Error al inicializar la base de datos: {ex.Message}", 
                    "Error de inicialización", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        static async Task MainAsync()
        {
            var authService = new WindowsFormsAuthService();
            AuthServiceProvider.Register(authService);

            while (true)
            {
                if (!await authService.IsAuthenticatedAsync())
                {
                    var loginForm = new LoginForm();
                    if (loginForm.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }

                try
                {
                    Application.Run(new MenuPrincipal());
                    break;
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(ex.Message, "Sesión Expirada",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inesperado: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (e.Exception is UnauthorizedAccessException)
            {
                MessageBox.Show("Su sesión ha expirado. Debe volver a autenticarse.", "Sesión Expirada",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Application.Restart();
            }
            else
            {
                MessageBox.Show($"Error inesperado: {e.Exception.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
