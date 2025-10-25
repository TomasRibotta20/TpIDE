using API.Clients;
using DTOs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormMaterias : Form
    {
        private readonly MateriaApiClient _apiClient;
        private readonly Form _menuPrincipal;
        private BindingList<MateriaDto> _materias = new BindingList<MateriaDto>();

        public FormMaterias(Form menuPrincipal = null)
        {
            InitializeComponent();
            _menuPrincipal = menuPrincipal;

            try
            {
                Debug.WriteLine("Inicializando cliente de API de materias");
                _apiClient = new MateriaApiClient();

                // Configurar DataGridView
                dataGridViewMaterias.DataSource = _materias;
                ConfigurarDataGridView();

                // Asignar eventos a botones
                btnNueva.Click += (s, e) => CrearNuevaMateria();
                btnEditar.Click += (s, e) => EditarMateriaSeleccionada(dataGridViewMaterias);
                btnEliminar.Click += (s, e) => EliminarMateriaSeleccionada(dataGridViewMaterias);
                btnVolver.Click += (s, e) => VolverAlMenu();

                // Suscribir al evento Load
                this.Load += FormMaterias_Load;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar: {ex.Message}",
                    "Error de inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            // Ocultar columnas que no queremos mostrar
            dataGridViewMaterias.AutoGenerateColumns = true;
            
            // Configurar después de cargar los datos
            this.Load += (s, e) =>
            {
                if (dataGridViewMaterias.Columns["IdPlan"] != null)
                    dataGridViewMaterias.Columns["IdPlan"].Visible = false;
            };
        }

        private async void FormMaterias_Load(object sender, EventArgs e)
        {
            await LoadMateriasAsync();
        }

        private async Task LoadMateriasAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var materias = await _apiClient.GetAllAsync();

                _materias.Clear();
                if (materias != null)
                {
                    foreach (var materia in materias)
                    {
                        _materias.Add(materia);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar materias: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async void CrearNuevaMateria()
        {
            var formNuevaMateria = new EditarMateriaForm();
            formNuevaMateria.ShowDialog();

            if (formNuevaMateria.Guardado && formNuevaMateria.MateriaEditada != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.CreateAsync(formNuevaMateria.MateriaEditada);
                    await LoadMateriasAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar materia: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private async void EditarMateriaSeleccionada(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una materia para editar",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var materiaSeleccionada = (MateriaDto)dataGridView.SelectedRows[0].DataBoundItem;
            if (materiaSeleccionada == null)
            {
                MessageBox.Show("No se pudo obtener la materia seleccionada",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var formEditarMateria = new EditarMateriaForm(materiaSeleccionada);
            formEditarMateria.ShowDialog();

            if (formEditarMateria.Guardado && formEditarMateria.MateriaEditada != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.UpdateAsync(formEditarMateria.MateriaEditada);
                    await LoadMateriasAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar materia: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private async void EliminarMateriaSeleccionada(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una materia para eliminar",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var materiaSeleccionada = (MateriaDto)dataGridView.SelectedRows[0].DataBoundItem;
            if (materiaSeleccionada == null)
            {
                MessageBox.Show("No se pudo obtener la materia seleccionada",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var resultado = MessageBox.Show(
                $"¿Está seguro que desea eliminar la materia '{materiaSeleccionada.Descripcion}'?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.DeleteAsync(materiaSeleccionada.Id);
                    await LoadMateriasAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar materia: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void VolverAlMenu()
        {
            if (_menuPrincipal != null)
            {
                _menuPrincipal.Show();
                this.Close();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (_menuPrincipal != null && !_menuPrincipal.Visible)
            {
                _menuPrincipal.Show();
            }
        }
    }
}
