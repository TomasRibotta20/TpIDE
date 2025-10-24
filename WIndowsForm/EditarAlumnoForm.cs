using API.Clients;
using DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class EditarAlumnoForm : Form
    {
        private readonly PersonaDto _alumno;
        private readonly bool _esNuevo;
        private readonly PlanApiClient _planApiClient;

        public PersonaDto AlumnoEditado { get; private set; }
        public bool Guardado { get; private set; }

        public EditarAlumnoForm(PersonaDto alumno = null)
        {
            InitializeComponent();
            
            _planApiClient = new PlanApiClient();
            _alumno = alumno ?? new PersonaDto { FechaNacimiento = DateTime.Today };
            _esNuevo = alumno == null;

            // Configuración específica que depende de si es nuevo o edición
            ConfigurarFormulario();
            CargarDatos();

            // Asignar eventos
            this.Load += EditarAlumnoForm_Load;
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;
        }

        private async void EditarAlumnoForm_Load(object sender, EventArgs e)
        {
            await CargarPlanes();
        }

        private async Task CargarPlanes()
        {
            try
            {
                var planes = await _planApiClient.GetAllAsync();
                cmbPlan.DataSource = planes.ToList();
                cmbPlan.DisplayMember = "Descripcion";
                cmbPlan.ValueMember = "Id";

                if (!_esNuevo && _alumno.IdPlan.HasValue)
                {
                    cmbPlan.SelectedValue = _alumno.IdPlan.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los planes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            this.Text = _esNuevo ? "Nuevo Alumno" : "Editar Alumno";

            // El campo ID solo se muestra en modo edición
            if (_esNuevo)
            {
                lblId.Visible = false;
                txtId.Visible = false;
                tableLayoutPanel1.RowStyles[0].Height = 0;
            }

            // Establecer AcceptButton y CancelButton
            this.AcceptButton = btnGuardar;
            this.CancelButton = btnCancelar;
        }

        private void CargarDatos()
        {
            if (!_esNuevo)
            {
                txtId.Text = _alumno.Id.ToString();
            }
            txtNombre.Text = _alumno.Nombre;
            txtApellido.Text = _alumno.Apellido;
            txtDireccion.Text = _alumno.Direccion;
            txtEmail.Text = _alumno.Email;
            txtTelefono.Text = _alumno.Telefono;
            dtpFechaNacimiento.Value = _alumno.FechaNacimiento;
            txtLegajo.Text = _alumno.Legajo.ToString();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtLegajo.Text))
            {
                MessageBox.Show("Los campos Nombre, Apellido, Email y Legajo son obligatorios.", 
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Guardar datos en el alumno
            AlumnoEditado = new PersonaDto
            {
                Id = _esNuevo ? 0 : _alumno.Id,
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Direccion = txtDireccion.Text,
                Email = txtEmail.Text,
                Telefono = txtTelefono.Text,
                FechaNacimiento = dtpFechaNacimiento.Value,
                Legajo = int.TryParse(txtLegajo.Text, out int legajo) ? legajo : 0,
                IdPlan = (int?)cmbPlan.SelectedValue,
                TipoPersona = TipoPersonaDto.Alumno // Importante: Se asigna el tipo correcto
            };

            Guardado = true;
            this.Close();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}