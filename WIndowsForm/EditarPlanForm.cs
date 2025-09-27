using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Clients;
using DTOs;

namespace WIndowsForm
{
    public partial class EditarPlanForm : Form
    {
        private readonly PlanDto _plan;
        private readonly bool _esNuevo;
        private readonly EspecialidadApiClient _especialidadApiClient = new EspecialidadApiClient();

        public PlanDto? PlanEditado { get; private set; }
        public bool Guardado { get; private set; }

        public EditarPlanForm(PlanDto? plan = null)
        {
            InitializeComponent();
            _plan = plan ?? new PlanDto();
            _esNuevo = plan == null;

            ConfigurarFormulario();
            this.Load += async (_, __) => await CargarDatosAsync();
        }

        private void ConfigurarFormulario()
        {
            Text = _esNuevo ? "Nuevo Plan" : "Editar Plan";

            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (_, __) => Close();

            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;

            if (_esNuevo)
            {
                lblId.Visible = false;
                txtId.Visible = false;
                tableLayoutPanel1.RowStyles[0].Height = 0;
            }

            comboEspecialidades.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private async Task CargarDatosAsync()
        {
            await CargarEspecialidadesAsync();

            if (!_esNuevo)
            {
                txtId.Text = _plan.Id.ToString();
                txtDescripcion.Text = _plan.Descripcion;
                comboEspecialidades.SelectedValue = _plan.EspecialidadId;
            }
        }

        private async Task CargarEspecialidadesAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var especialidades = await _especialidadApiClient.GetAllAsync();
                var lista = especialidades.ToList();
                comboEspecialidades.DataSource = lista;
                comboEspecialidades.DisplayMember = "Descripcion";
                comboEspecialidades.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar especialidades: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { Cursor.Current = Cursors.Default; }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("La descripción es obligatoria.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboEspecialidades.SelectedValue == null)
            {
                MessageBox.Show("Seleccione una especialidad.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PlanEditado = new PlanDto
            {
                Id = _esNuevo ? 0 : _plan.Id,
                Descripcion = txtDescripcion.Text.Trim(),
                EspecialidadId = (int)comboEspecialidades.SelectedValue
            };

            Guardado = true;
            Close();
        }
    }
}