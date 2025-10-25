using System;
using System.Windows.Forms;
using API.Clients;
using DTOs;

namespace WIndowsForm
{
    public partial class RegisterForm : Form
    {
        private readonly AuthApiClient _authApiClient;

        // Datos del Usuario
        private TextBox txtUsuarioNombre;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private TextBox txtEmail;

        // Datos de la Persona
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtDireccion;
        private TextBox txtTelefono;
        private DateTimePicker dtpFechaNacimiento;
        private NumericUpDown numLegajo;
        private ComboBox cmbTipoPersona;
        private ComboBox cmbPlan;

        // Botones
        private Button btnRegistrar;
        private Button btnCancelar;

        // Labels
        private Label lblTitulo;
        private Label lblUsuarioSection;
        private Label lblPersonaSection;

        public RegisterForm()
        {
            _authApiClient = new AuthApiClient();
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Configurar el formulario
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 700);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registro de Usuario";

            // Título
            lblTitulo = new Label
            {
                Text = "REGISTRO DE NUEVO USUARIO",
                Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(560, 35),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            // Sección Usuario
            lblUsuarioSection = new Label
            {
                Text = "DATOS DEL USUARIO",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 70),
                Size = new System.Drawing.Size(560, 25),
                BackColor = System.Drawing.Color.LightGray
            };

            var lblUsuarioNombre = new Label { Text = "Usuario*:", Location = new System.Drawing.Point(20, 105), Size = new System.Drawing.Size(150, 23) };
            txtUsuarioNombre = new TextBox { Location = new System.Drawing.Point(180, 102), Size = new System.Drawing.Size(380, 23) };

            var lblPassword = new Label { Text = "Contraseña*:", Location = new System.Drawing.Point(20, 135), Size = new System.Drawing.Size(150, 23) };
            txtPassword = new TextBox { Location = new System.Drawing.Point(180, 132), Size = new System.Drawing.Size(380, 23), UseSystemPasswordChar = true };

            var lblConfirmPassword = new Label { Text = "Confirmar Contraseña*:", Location = new System.Drawing.Point(20, 165), Size = new System.Drawing.Size(150, 23) };
            txtConfirmPassword = new TextBox { Location = new System.Drawing.Point(180, 162), Size = new System.Drawing.Size(380, 23), UseSystemPasswordChar = true };

            var lblEmail = new Label { Text = "Email*:", Location = new System.Drawing.Point(20, 195), Size = new System.Drawing.Size(150, 23) };
            txtEmail = new TextBox { Location = new System.Drawing.Point(180, 192), Size = new System.Drawing.Size(380, 23) };

            // Sección Persona
            lblPersonaSection = new Label
            {
                Text = "DATOS PERSONALES",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 235),
                Size = new System.Drawing.Size(560, 25),
                BackColor = System.Drawing.Color.LightGray
            };

            var lblTipoPersona = new Label { Text = "Tipo*:", Location = new System.Drawing.Point(20, 270), Size = new System.Drawing.Size(150, 23) };
            cmbTipoPersona = new ComboBox 
            { 
                Location = new System.Drawing.Point(180, 267), 
                Size = new System.Drawing.Size(380, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            var lblNombre = new Label { Text = "Nombre*:", Location = new System.Drawing.Point(20, 300), Size = new System.Drawing.Size(150, 23) };
            txtNombre = new TextBox { Location = new System.Drawing.Point(180, 297), Size = new System.Drawing.Size(380, 23) };

            var lblApellido = new Label { Text = "Apellido*:", Location = new System.Drawing.Point(20, 330), Size = new System.Drawing.Size(150, 23) };
            txtApellido = new TextBox { Location = new System.Drawing.Point(180, 327), Size = new System.Drawing.Size(380, 23) };

            var lblLegajo = new Label { Text = "Legajo*:", Location = new System.Drawing.Point(20, 360), Size = new System.Drawing.Size(150, 23) };
            numLegajo = new NumericUpDown 
            { 
                Location = new System.Drawing.Point(180, 357), 
                Size = new System.Drawing.Size(380, 23),
                Minimum = 1,
                Maximum = 999999
            };

            var lblFechaNacimiento = new Label { Text = "Fecha Nacimiento*:", Location = new System.Drawing.Point(20, 390), Size = new System.Drawing.Size(150, 23) };
            dtpFechaNacimiento = new DateTimePicker 
            { 
                Location = new System.Drawing.Point(180, 387), 
                Size = new System.Drawing.Size(380, 23),
                Format = DateTimePickerFormat.Short,
                MaxDate = DateTime.Now.AddYears(-15), // Mínimo 15 años
                Value = DateTime.Now.AddYears(-20)
            };

            var lblDireccion = new Label { Text = "Dirección:", Location = new System.Drawing.Point(20, 420), Size = new System.Drawing.Size(150, 23) };
            txtDireccion = new TextBox { Location = new System.Drawing.Point(180, 417), Size = new System.Drawing.Size(380, 23) };

            var lblTelefono = new Label { Text = "Teléfono:", Location = new System.Drawing.Point(20, 450), Size = new System.Drawing.Size(150, 23) };
            txtTelefono = new TextBox { Location = new System.Drawing.Point(180, 447), Size = new System.Drawing.Size(380, 23) };

            var lblPlan = new Label { Text = "Plan de Estudios:", Location = new System.Drawing.Point(20, 480), Size = new System.Drawing.Size(150, 23) };
            cmbPlan = new ComboBox 
            { 
                Location = new System.Drawing.Point(180, 477), 
                Size = new System.Drawing.Size(380, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Separador
            var separator = new Label
            {
                BorderStyle = BorderStyle.Fixed3D,
                Location = new System.Drawing.Point(20, 520),
                Size = new System.Drawing.Size(560, 2)
            };

            // Información
            var lblInfo = new Label
            {
                Text = "* Campos obligatorios",
                Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic),
                Location = new System.Drawing.Point(20, 535),
                Size = new System.Drawing.Size(560, 20),
                ForeColor = System.Drawing.Color.Gray
            };

            // Botones
            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(300, 620),
                Size = new System.Drawing.Size(120, 35),
                DialogResult = DialogResult.Cancel
            };

            btnRegistrar = new Button
            {
                Text = "Registrar",
                Location = new System.Drawing.Point(440, 620),
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.FromArgb(0, 122, 204),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRegistrar.Click += BtnRegistrar_Click;

            // Link para volver al login
            var lnkVolverLogin = new LinkLabel
            {
                Text = "? Volver al Login",
                Location = new System.Drawing.Point(20, 628),
                Size = new System.Drawing.Size(150, 20)
            };
            lnkVolverLogin.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Agregar controles al formulario
            this.Controls.AddRange(new Control[] 
            {
                lblTitulo,
                lblUsuarioSection, lblUsuarioNombre, txtUsuarioNombre,
                lblPassword, txtPassword, lblConfirmPassword, txtConfirmPassword,
                lblEmail, txtEmail,
                lblPersonaSection, lblTipoPersona, cmbTipoPersona,
                lblNombre, txtNombre, lblApellido, txtApellido,
                lblLegajo, numLegajo, lblFechaNacimiento, dtpFechaNacimiento,
                lblDireccion, txtDireccion, lblTelefono, txtTelefono,
                lblPlan, cmbPlan,
                separator, lblInfo,
                lnkVolverLogin, btnCancelar, btnRegistrar
            });

            this.ResumeLayout(false);
        }

        private async void ConfigurarFormulario()
        {
            // Cargar tipos de persona
            cmbTipoPersona.Items.Clear();
            cmbTipoPersona.Items.Add("Profesor");
            cmbTipoPersona.Items.Add("Alumno");
            cmbTipoPersona.SelectedIndex = 0;

            // Cargar planes (temporal - agregar opción "Sin asignar")
            cmbPlan.Items.Clear();
            cmbPlan.Items.Add(new { Text = "Sin asignar", Value = (int?)null });
            
            try
            {
                // Intentar cargar planes reales de la API
                var planApiClient = new PlanApiClient();
                var planes = await planApiClient.GetAllAsync();
                foreach (var plan in planes)
                {
                    cmbPlan.Items.Add(new { Text = plan.Descripcion, Value = (int?)plan.Id });
                }
            }
            catch
            {
                // Si falla, solo tendremos "Sin asignar"
            }
            
            cmbPlan.DisplayMember = "Text";
            cmbPlan.ValueMember = "Value";
            cmbPlan.SelectedIndex = 0;
        }

        private async void BtnRegistrar_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (!ValidarFormulario())
                    return;

                btnRegistrar.Enabled = false;
                btnRegistrar.Text = "Registrando...";
                this.Cursor = Cursors.WaitCursor;

                // Crear request
                var request = new RegisterRequestDto
                {
                    UsuarioNombre = txtUsuarioNombre.Text.Trim(),
                    Password = txtPassword.Text,
                    Email = txtEmail.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Direccion = string.IsNullOrWhiteSpace(txtDireccion.Text) ? null : txtDireccion.Text.Trim(),
                    Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    FechaNacimiento = dtpFechaNacimiento.Value,
                    Legajo = (int)numLegajo.Value,
                    TipoPersona = cmbTipoPersona.SelectedIndex == 0 ? TipoPersonaDto.Profesor : TipoPersonaDto.Alumno,
                    IdPlan = cmbPlan.SelectedIndex == 0 ? null : (int?)((dynamic)cmbPlan.SelectedItem!).Value
                };

                // Llamar al servicio de registro
                var response = await _authApiClient.RegisterAsync(request);

                if (response.Success)
                {
                    MessageBox.Show(
                        $"¡Registro exitoso!\n\n{response.Message}\n\nUsuario: {request.UsuarioNombre}\nTipo: {request.TipoPersona}\n\nYa puede iniciar sesión con sus credenciales.",
                        "Registro Exitoso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        $"Error al registrar:\n\n{response.Message}",
                        "Error de Registro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error inesperado al registrar:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnRegistrar.Enabled = true;
                btnRegistrar.Text = "Registrar";
                this.Cursor = Cursors.Default;
            }
        }

        private bool ValidarFormulario()
        {
            // Usuario
            if (string.IsNullOrWhiteSpace(txtUsuarioNombre.Text))
            {
                MessageBox.Show("Debe ingresar un nombre de usuario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuarioNombre.Focus();
                return false;
            }

            if (txtUsuarioNombre.Text.Length < 4)
            {
                MessageBox.Show("El nombre de usuario debe tener al menos 4 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuarioNombre.Focus();
                return false;
            }

            // Contraseña
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Debe ingresar una contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return false;
            }

            // Email
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Debe ingresar un email.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("El email no tiene un formato válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Persona
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar el nombre.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("Debe ingresar el apellido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            if (numLegajo.Value == 0)
            {
                MessageBox.Show("Debe ingresar un legajo válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numLegajo.Focus();
                return false;
            }

            if (cmbTipoPersona.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un tipo de persona.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipoPersona.Focus();
                return false;
            }

            // Validar edad mínima (15 años)
            var edad = DateTime.Now.Year - dtpFechaNacimiento.Value.Year;
            if (dtpFechaNacimiento.Value > DateTime.Now.AddYears(-edad)) edad--;

            if (edad < 15)
            {
                MessageBox.Show("La persona debe tener al menos 15 años.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFechaNacimiento.Focus();
                return false;
            }

            return true;
        }
    }
}
