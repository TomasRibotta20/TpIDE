using API.Clients;
using System.Net.Http;
using WIndowsForm;

namespace WindowsForms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    loginButton.Enabled = false;
                    loginButton.Text = "Iniciando sesión...";
                    Cursor.Current = Cursors.WaitCursor;

                    var authService = AuthServiceProvider.Instance;
                    bool success = await authService.LoginAsync(usernameTextBox.Text, passwordTextBox.Text);

                    if (success)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos.", "Error de autenticación",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        passwordTextBox.Clear();
                        passwordTextBox.Focus();
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show($"Error de autenticación: {ex.Message}", "Error de autenticación",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    passwordTextBox.Clear();
                    passwordTextBox.Focus();
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show(
                        "Error de conexión: No se pudo conectar al servidor.\n\n" +
                        "Verifique que la API esté funcionando:\n" +
                        "1. Abra http://localhost:5000/swagger en el navegador\n" +
                        "2. Si no funciona, inicie el proyecto AcademiaAPI\n" +
                        "3. Verifique la configuración en appsettings.json\n\n" +
                        $"Detalles técnicos: {ex.Message}",
                        "Error de Conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch (TaskCanceledException)
                {
                    MessageBox.Show(
                        "La conexión con el servidor ha tardado demasiado.\n\n" +
                        "Posibles causas:\n" +
                        "• La API no está respondiendo\n" +
                        "• Problemas de red\n" +
                        "• El servidor está ocupado",
                        "Timeout",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error inesperado al iniciar sesión:\n\n{ex.Message}\n\n" +
                        $"Tipo de error: {ex.GetType().Name}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                finally
                {
                    loginButton.Enabled = true;
                    loginButton.Text = "Iniciar Sesión";
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void registerLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var registerForm = new RegisterForm();
                var result = registerForm.ShowDialog();
                
                if (result == DialogResult.OK)
                {
                    // Limpiar los campos después de un registro exitoso
                    usernameTextBox.Clear();
                    passwordTextBox.Clear();
                    usernameTextBox.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al abrir el formulario de registro:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            errorProvider.SetError(usernameTextBox, string.Empty);
            errorProvider.SetError(passwordTextBox, string.Empty);

            bool isValid = true;

            if (string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                errorProvider.SetError(usernameTextBox, "El nombre de usuario es requerido");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                errorProvider.SetError(passwordTextBox, "La contraseña es requerida");
                isValid = false;
            }

            return isValid;
        }

        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                loginButton_Click(sender, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            // Precargar el usuario admin para facilitar las pruebas
            usernameTextBox.Text = "admin";
            passwordTextBox.Select();

            // Verificar conexión con la API
            try
            {
                loginButton.Enabled = false;
                loginButton.Text = "Verificando API...";

                bool isConnected = await TestConnection.TestApiConnectionAsync();

                if (!isConnected)
                {
                    var result = MessageBox.Show(
                        "No se puede conectar con la API.\n\n" +
                        "Asegúrese de que:\n" +
                        "1. El proyecto AcademiaAPI esté ejecutándose\n" +
                        "2. La API esté corriendo en http://localhost:5000\n" +
                        "3. La configuración en appsettings.json sea correcta\n\n" +
                        "¿Desea continuar de todas formas?",
                        "Advertencia de Conexión",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                    {
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al verificar la conexión: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                loginButton.Enabled = true;
                loginButton.Text = "Iniciar Sesión";
            }
        }
    }
}