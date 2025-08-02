using DTOs;
using System;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class EditarUsuarioForm : Form
    {
        private readonly UsuarioDto _usuario;
        private readonly bool _esNuevo;

        public UsuarioDto UsuarioEditado { get; private set; }
        public bool Guardado { get; private set; }

        public EditarUsuarioForm(UsuarioDto usuario = null)
        {
            InitializeComponent();
            _usuario = usuario ?? new UsuarioDto { Habilitado = true };
            _esNuevo = usuario == null;
            ConfigureForm();
        }

        private void EditarUsuarios_Load(object sender, EventArgs e)
        {
            // Este método se puede usar si necesitas lógica adicional al cargar el formulario
        }   
        private void ConfigureForm()
        {
            this.Text = _esNuevo ? "Nuevo Usuario" : "Editar Usuario";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(450, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // TableLayoutPanel para organizar los campos
            TableLayoutPanel tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 2,
                RowCount = 8,
                ColumnStyles = {
                    new ColumnStyle(SizeType.Percent, 30),
                    new ColumnStyle(SizeType.Percent, 70)
                }
            };

            // Agregar filas con alturas específicas
            for (int i = 0; i < 8; i++)
            {
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            }

            int row = 0;

            // Solo mostrar ID si es edición
            if (!_esNuevo)
            {
                Label lblId = new Label { Text = "ID:", Anchor = AnchorStyles.Left | AnchorStyles.Right };
                TextBox txtId = new TextBox
                {
                    Text = _usuario.Id.ToString(),
                    ReadOnly = true,
                    Dock = DockStyle.Fill
                };

                tableLayout.Controls.Add(lblId, 0, row);
                tableLayout.Controls.Add(txtId, 1, row);
                row++;
            }

            // Nombre
            Label lblNombre = new Label { Text = "Nombre*:", Anchor = AnchorStyles.Left | AnchorStyles.Right };
            TextBox txtNombre = new TextBox
            {
                Text = _usuario.Nombre,
                Dock = DockStyle.Fill
            };

            tableLayout.Controls.Add(lblNombre, 0, row);
            tableLayout.Controls.Add(txtNombre, 1, row);
            row++;

            // Apellido
            Label lblApellido = new Label { Text = "Apellido*:", Anchor = AnchorStyles.Left | AnchorStyles.Right };
            TextBox txtApellido = new TextBox
            {
                Text = _usuario.Apellido,
                Dock = DockStyle.Fill
            };

            tableLayout.Controls.Add(lblApellido, 0, row);
            tableLayout.Controls.Add(txtApellido, 1, row);
            row++;

            // Usuario
            Label lblUsuario = new Label { Text = "Usuario*:", Anchor = AnchorStyles.Left | AnchorStyles.Right };
            TextBox txtUsuario = new TextBox
            {
                Text = _usuario.UsuarioNombre,
                Dock = DockStyle.Fill
            };

            tableLayout.Controls.Add(lblUsuario, 0, row);
            tableLayout.Controls.Add(txtUsuario, 1, row);
            row++;

            // Contraseña
            Label lblContrasenia = new Label { Text = "Contraseña*:", Anchor = AnchorStyles.Left | AnchorStyles.Right };
            TextBox txtContrasenia = new TextBox
            {
                Text = _usuario.Contrasenia,
                PasswordChar = '*',
                Dock = DockStyle.Fill
            };

            tableLayout.Controls.Add(lblContrasenia, 0, row);
            tableLayout.Controls.Add(txtContrasenia, 1, row);
            row++;

            // Email
            Label lblEmail = new Label { Text = "Email*:", Anchor = AnchorStyles.Left | AnchorStyles.Right };
            TextBox txtEmail = new TextBox
            {
                Text = _usuario.Email,
                Dock = DockStyle.Fill
            };

            tableLayout.Controls.Add(lblEmail, 0, row);
            tableLayout.Controls.Add(txtEmail, 1, row);
            row++;

            // Habilitado
            CheckBox chkHabilitado = new CheckBox
            {
                Text = "Habilitado",
                Checked = _usuario.Habilitado,
                Dock = DockStyle.Fill
            };

            tableLayout.Controls.Add(new Label(), 0, row); // Celda vacía
            tableLayout.Controls.Add(chkHabilitado, 1, row);
            row++;

            // Panel de botones
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 40
            };

            Button btnGuardar = new Button
            {
                Text = "Guardar",
                Width = 100,
                Location = new Point(50, 5)
            };

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Width = 100,
                Location = new Point(160, 5)
            };

            btnGuardar.Click += (s, e) => {
                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text) ||
                    string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                    string.IsNullOrWhiteSpace(txtContrasenia.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Todos los campos marcados con * son obligatorios",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Guardar datos en el usuario
                UsuarioEditado = new UsuarioDto
                {
                    Id = _esNuevo ? 0 : _usuario.Id,
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    UsuarioNombre = txtUsuario.Text,
                    Contrasenia = txtContrasenia.Text,
                    Email = txtEmail.Text,
                    Habilitado = chkHabilitado.Checked
                };

                Guardado = true;
                this.Close();
            };

            btnCancelar.Click += (s, e) => this.Close();

            buttonPanel.Controls.Add(btnGuardar);
            buttonPanel.Controls.Add(btnCancelar);

            tableLayout.Controls.Add(buttonPanel, 1, row);

            this.Controls.Add(tableLayout);
            this.AcceptButton = btnGuardar;
            this.CancelButton = btnCancelar;
        }
    }
}