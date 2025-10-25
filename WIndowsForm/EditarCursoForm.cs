using API.Clients;
using DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class EditarCursoForm : Form
    {
        private readonly CursoDto _curso;
        private readonly bool _esNuevo;
        private readonly ComisionApiClient _comisionApiClient;
        private readonly MateriaApiClient _materiaApiClient; // Nuevo cliente de materias

        public CursoDto CursoEditado { get; private set; }
        public bool Guardado { get; private set; }

        public EditarCursoForm(CursoDto curso = null)
        {
            InitializeComponent();
            
            _comisionApiClient = new ComisionApiClient();
            _materiaApiClient = new MateriaApiClient(); // Inicializar cliente de materias
            _curso = curso ?? new CursoDto { AnioCalendario = DateTime.Now.Year, Cupo = 30 };
            _esNuevo = curso == null;

            ConfigurarFormulario();
            CargarDatos();

            this.Load += EditarCursoForm_Load;
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;
        }

        private async void EditarCursoForm_Load(object sender, EventArgs e)
        {
            await CargarComisiones();
            await CargarMaterias(); // Cargar materias reales
        }

        private async Task CargarComisiones()
        {
            try
            {
                var comisiones = await _comisionApiClient.GetAllAsync();
                cmbComision.DataSource = comisiones.ToList();
                cmbComision.DisplayMember = "DescComision";
                cmbComision.ValueMember = "IdComision";

                if (!_esNuevo && _curso.IdComision > 0)
                {
                    cmbComision.SelectedValue = _curso.IdComision;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las comisiones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarMaterias()
        {
            try
            {
                var materias = await _materiaApiClient.GetAllAsync();
                cmbMateria.DataSource = materias.ToList();
                cmbMateria.DisplayMember = "Descripcion";
                cmbMateria.ValueMember = "Id";

                if (!_esNuevo && _curso.IdMateria > 0)
                {
                    cmbMateria.SelectedValue = _curso.IdMateria;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las materias: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            this.Text = _esNuevo ? "Nuevo Curso" : "Editar Curso";

            // El campo ID solo se muestra en modo edición
            if (_esNuevo)
            {
                lblId.Visible = false;
                txtId.Visible = false;
                tableLayoutPanel1.RowStyles[0].Height = 0;
            }

            // Cargar años (desde 2020 hasta 5 años en el futuro)
            int currentYear = DateTime.Now.Year;
            for (int year = 2020; year <= currentYear + 5; year++)
            {
                cmbAnioCalendario.Items.Add(year);
            }

            this.AcceptButton = btnGuardar;
            this.CancelButton = btnCancelar;
        }

        private void CargarDatos()
        {
            if (!_esNuevo)
            {
                txtId.Text = _curso.IdCurso.ToString();
            }

            // Año calendario
            if (_curso.AnioCalendario > 0)
            {
                cmbAnioCalendario.SelectedItem = _curso.AnioCalendario;
            }
            else
            {
                cmbAnioCalendario.SelectedItem = DateTime.Now.Year;
            }

            // Cupo
            numCupo.Value = _curso.Cupo > 0 ? _curso.Cupo : 30;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Validar campos obligatorios
            if (cmbMateria.SelectedValue == null ||
                cmbComision.SelectedValue == null ||
                cmbAnioCalendario.SelectedItem == null ||
                numCupo.Value <= 0)
            {
                MessageBox.Show("Todos los campos son obligatorios y el cupo debe ser mayor que cero.", 
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validaciones adicionales
            if (numCupo.Value > 100)
            {
                MessageBox.Show("El cupo no puede ser mayor a 100 estudiantes.", 
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Guardar datos en el curso
                CursoEditado = new CursoDto
                {
                    IdCurso = _esNuevo ? 0 : _curso.IdCurso,
                    IdMateria = (int)cmbMateria.SelectedValue, // Ahora usa materia real
                    IdComision = (int)cmbComision.SelectedValue,
                    AnioCalendario = (int)cmbAnioCalendario.SelectedItem,
                    Cupo = (int)numCupo.Value
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

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}