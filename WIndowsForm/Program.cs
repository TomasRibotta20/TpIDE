using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Auth.WindowsForms;
using API.Clients;
using System.IO;
using WIndowsForm;

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
                        $"No se encontro el archivo de configuracion en:\n{appSettingsPath}\n\nSe creara uno con la configuracion predeterminada.",
                        "Archivo de configuracion faltante",
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
                        "Se ha creado un archivo de configuracion predeterminado.",
                        "Configuracion creada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al iniciar la aplicacion: {ex.Message}",
                    "Error de inicializacion",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            Task.Run(async () => await MainAsync()).GetAwaiter().GetResult();
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
                    // Determinar que menu mostrar segun el tipo de usuario
                    string? tipoUsuario = WindowsFormsAuthService.GetCurrentTipoUsuario();
                    
                    Form menuPrincipal;
                    if (tipoUsuario == "Administrador")
                    {
                        menuPrincipal = new MenuPrincipal();
                    }
                    else if (tipoUsuario == "Profesor")
                    {
                        menuPrincipal = new MenuProfesor();
                    }
                    else if (tipoUsuario == "Alumno")
                    {
                        menuPrincipal = new MenuAlumno();
                    }
                    else
                    {
                        MessageBox.Show($"Tipo de usuario desconocido: {tipoUsuario}", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    Application.Run(menuPrincipal);
                    break;
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(ex.Message, "Sesion Expirada",
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
                MessageBox.Show("Su sesion ha expirado. Debe volver a autenticarse.", "Sesion Expirada",
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
