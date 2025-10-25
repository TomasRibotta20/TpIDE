using API.Clients;
using DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class EditarMateriaForm : Form
    {
        private readonly MateriaDto _materia;
        private readonly bool _esNueva;
        private readonly PlanApiClient _planApiClient;

        public MateriaDto MateriaEditada { get; private set; }
        public bool Guardado { get; private set; }

        public EditarMateriaForm(MateriaDto materia = null)
        {
            InitializeComponent();
            _planApiClient = new PlanApiClient();
            _materia = materia ?? new MateriaDto();
            _esNueva = materia == null;

            ConfigurarFormulario();
            this.Load += EditarMateriaForm_Load;
        }

        private async void EditarMateriaForm_Load(object sender, EventArgs e)
        {
            await CargarPlanes();
            CargarDatos();
        }

        private async Task CargarPlanes()
        {
            try
            {
                var planes = await _planApiClient.GetAllAsync();
                cmbPlan.DataSource = planes.ToList();
                cmbPlan.DisplayMember = "Descripcion";
                cmbPlan.ValueMember = "Id";

                if (!_esNueva && _materia.IdPlan > 0)
                {
                    cmbPlan.SelectedValue = _materia.IdPlan;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los planes: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            // Establecer título según modo
            this.Text = _esNueva ? "Nueva Materia" : "Editar Materia";

            // Mostrar/ocultar ID según modo
            if (_esNueva)
            {
                lblId.Visible = false;
                txtId.Visible = false;
                tableLayoutPanel1.RowStyles[0].Height = 0;
            }

            // Asignar eventos a botones
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (s, e) => this.Close();

            // Establecer botones de aceptar/cancelar
            this.AcceptButton = btnGuardar;
            this.CancelButton = btnCancelar;
        }

        private void CargarDatos()
        {
            if (!_esNueva)
            {
                txtId.Text = _materia.Id.ToString();
                txtDescripcion.Text = _materia.Descripcion;
                numHorasSemanales.Value = _materia.HorasSemanales;
                numHorasTotales.Value = _materia.HorasTotales;
            }
            else
            {
                // Valores por defecto para nueva materia
                numHorasSemanales.Value = 4;
                numHorasTotales.Value = 64;
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("La descripción de la materia es obligatoria",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescripcion.Focus();
                return;
            }

            if (cmbPlan.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un plan de estudios",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPlan.Focus();
                return;
            }

            if (numHorasSemanales.Value <= 0)
            {
                MessageBox.Show("Las horas semanales deben ser mayores que cero",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numHorasSemanales.Focus();
                return;
            }

            if (numHorasTotales.Value <= 0)
            {
                MessageBox.Show("Las horas totales deben ser mayores que cero",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numHorasTotales.Focus();
                return;
            }

            // Validación adicional: las horas totales deberían ser mayores o iguales a las semanales
            if (numHorasTotales.Value < numHorasSemanales.Value)
            {
                MessageBox.Show("Las horas totales no pueden ser menores que las horas semanales",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numHorasTotales.Focus();
                return;
            }

            try
            {
                // Guardar datos en la materia
                MateriaEditada = new MateriaDto
                {
                    Id = _esNueva ? 0 : _materia.Id,
                    Descripcion = txtDescripcion.Text.Trim(),
                    HorasSemanales = (int)numHorasSemanales.Value,
                    HorasTotales = (int)numHorasTotales.Value,
                    IdPlan = (int)cmbPlan.SelectedValue
                };

                Guardado = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar los datos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
