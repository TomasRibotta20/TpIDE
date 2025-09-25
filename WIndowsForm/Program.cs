using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Auth.WindowsForms;
using API.Clients;


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
                    Application.Run(new MenuPrincipal());
                    break;
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(ex.Message, "Sesión Expirada",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
