using API.Clients;
using DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class EditarProfesorForm : Form
    {
        private readonly PersonaDto _profesor;
        private readonly bool _esNuevo;
        private readonly PlanApiClient _planApiClient;

        public PersonaDto ProfesorEditado { get; private set; }
        public bool Guardado { get; private set; }

        public EditarProfesorForm(PersonaDto profesor = null)
        {
            InitializeComponent();
            
            try
            {
                _planApiClient = new PlanApiClient();
                _profesor = profesor ?? new PersonaDto 
                { 
                    FechaNacimiento = DateTime.Today,
                    Nombre = string.Empty,
                    Apellido = string.Empty,
                    Email = string.Empty,
                    Direccion = string.Empty,
                    Telefono = string.Empty,
                    TipoPersona = TipoPersonaDto.Profesor
                };
                _esNuevo = profesor == null;

                // Configuración específica que depende de si es nuevo o edición
                ConfigurarFormulario();
                CargarDatos();

                // Asignar eventos
                this.Load += EditarProfesorForm_Load;
                btnGuardar.Click += BtnGuardar_Click;
                btnCancelar.Click += BtnCancelar_Click;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void EditarProfesorForm_Load(object sender, EventArgs e)
        {
            await CargarPlanes();
        }

        private async Task CargarPlanes()
        {
            try
            {
                var planes = await _planApiClient.GetAllAsync();
                if (planes != null)
                {
                    cmbPlan.DataSource = planes.ToList();
                    cmbPlan.DisplayMember = "Descripcion";
                    cmbPlan.ValueMember = "Id";

                    if (!_esNuevo && _profesor.IdPlan.HasValue)
                    {
                        cmbPlan.SelectedValue = _profesor.IdPlan.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los planes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            try
            {
                this.Text = _esNuevo ? "Nuevo Profesor" : "Editar Profesor";

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
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar formulario: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatos()
        {
            try
            {
                if (!_esNuevo)
                {
                    txtId.Text = _profesor.Id.ToString();
                }
                
                txtNombre.Text = _profesor.Nombre ?? string.Empty;
                txtApellido.Text = _profesor.Apellido ?? string.Empty;
                txtDireccion.Text = _profesor.Direccion ?? string.Empty;
                txtEmail.Text = _profesor.Email ?? string.Empty;
                txtTelefono.Text = _profesor.Telefono ?? string.Empty;
                dtpFechaNacimiento.Value = _profesor.FechaNacimiento;
                txtLegajo.Text = _profesor.Legajo.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
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

                // Validar legajo es número válido
                if (!int.TryParse(txtLegajo.Text, out int legajo) || legajo <= 0)
                {
                    MessageBox.Show("El Legajo debe ser un número válido mayor a 0.", 
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Guardar datos en el profesor
                ProfesorEditado = new PersonaDto
                {
                    Id = _esNuevo ? 0 : _profesor.Id,
                    Nombre = txtNombre.Text?.Trim() ?? string.Empty,
                    Apellido = txtApellido.Text?.Trim() ?? string.Empty,
                    Direccion = txtDireccion.Text?.Trim(),
                    Email = txtEmail.Text?.Trim() ?? string.Empty,
                    Telefono = txtTelefono.Text?.Trim(),
                    FechaNacimiento = dtpFechaNacimiento.Value,
                    Legajo = legajo,
                    IdPlan = cmbPlan.SelectedValue as int?,
                    TipoPersona = TipoPersonaDto.Profesor // Importante: Se asigna el tipo correcto
                };

                Guardado = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar profesor: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}