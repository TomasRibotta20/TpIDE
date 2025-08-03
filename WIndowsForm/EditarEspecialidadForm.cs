using DTOs;
using System;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class EditarEspecialidadForm : Form
    {
        private readonly EspecialidadDto _especialidad;
        private readonly bool _esNueva;

        public EspecialidadDto EspecialidadEditada { get; private set; }
        public bool Guardado { get; private set; }

        public EditarEspecialidadForm(EspecialidadDto especialidad = null)
        {
            InitializeComponent();
            _especialidad = especialidad ?? new EspecialidadDto();
            _esNueva = especialidad == null;

            ConfigurarFormulario();
            CargarDatos();
        }

        private void ConfigurarFormulario()
        {
            // Establecer título según modo
            this.Text = _esNueva ? "Nueva Especialidad" : "Editar Especialidad";

            // Mostrar/ocultar ID según modo
            if (_esNueva)
            {
                lblId.Visible = false;
                txtId.Visible = false;
                // Opcional: ajustar la altura de la fila a 0
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
                txtId.Text = _especialidad.Id.ToString();

            txtDescripcion.Text = _especialidad.Descripcion;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("La descripción de la especialidad es obligatoria",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Guardar datos en la especialidad
            EspecialidadEditada = new EspecialidadDto
            {
                Id = _esNueva ? 0 : _especialidad.Id,
                Descripcion = txtDescripcion.Text
            };

            Guardado = true;
            this.Close();
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void EditarEspecialidadForm_Load(object sender, EventArgs e)
        {

        }
    }
}
