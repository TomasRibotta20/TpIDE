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

            // Configuración específica que depende de si es nuevo o edición
            ConfigurarFormulario();
            CargarDatos();

            // Asignar eventos
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (s, e) => this.Close();
        }

        private void ConfigurarFormulario()
        {
            this.Text = _esNuevo ? "Nuevo Usuario" : "Editar Usuario";

            // El campo ID solo se muestra en modo edición
            if (_esNuevo)
            {
                // Ocultar la fila del ID - Ajustar según cómo hayas diseñado el TableLayoutPanel
                // Por ejemplo, si ID está en la primera fila:
                tableLayoutPanel1.RowStyles[0].Height = 0;
                lblId.Visible = false;
                txtId.Visible = false;
            }

            // Establecer AcceptButton y CancelButton
            this.AcceptButton = btnGuardar;
            this.CancelButton = btnCancelar;
        }

        private void CargarDatos()
        {
            if (!_esNuevo)
                txtId.Text = _usuario.Id.ToString();

            txtNombre.Text = _usuario.Nombre;
            txtApellido.Text = _usuario.Apellido;
            txtUsuario.Text = _usuario.UsuarioNombre;
            txtContrasenia.Text = _usuario.Contrasenia;
            txtEmail.Text = _usuario.Email;
            chkHabilitado.Checked = _usuario.Habilitado;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
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
        }
    }
}
