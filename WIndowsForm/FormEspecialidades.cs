using API.Clients;
using DTOs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormEspecialidades : Form
    {
        private readonly EspecialidadApiClient _apiClient;
        private readonly Form _menuPrincipal;
        private BindingList<EspecialidadDto> _especialidades = new BindingList<EspecialidadDto>();

        public FormEspecialidades(Form menuPrincipal = null)
        {
            InitializeComponent();
            _menuPrincipal = menuPrincipal;

            try
            {
                string apiUrl = "https://localhost:7229";
                Debug.WriteLine($"Conectando a API en: {apiUrl}");
                _apiClient = new EspecialidadApiClient(apiUrl);

                // Configurar DataGridView
                dataGridViewEspecialidades.DataSource = _especialidades;

                // Asignar eventos a botones
                btnNueva.Click += (s, e) => CrearNuevaEspecialidad();
                btnEditar.Click += (s, e) => EditarEspecialidadSeleccionada(dataGridViewEspecialidades);
                btnEliminar.Click += (s, e) => EliminarEspecialidadSeleccionada(dataGridViewEspecialidades);
                btnVolver.Click += (s, e) => VolverAlMenu();

                // Suscribir al evento Load
                this.Load += FormEspecialidades_Load;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar: {ex.Message}",
                    "Error de inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void FormEspecialidades_Load(object sender, EventArgs e)
        {
            await LoadEspecialidadesAsync();
        }

        private async Task LoadEspecialidadesAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var especialidades = await _apiClient.GetAllAsync();

                _especialidades.Clear();
                if (especialidades != null)
                {
                    foreach (var especialidad in especialidades)
                    {
                        _especialidades.Add(especialidad);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar especialidades: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async void CrearNuevaEspecialidad()
        {
            var formNuevaEspecialidad = new EditarEspecialidadForm();
            formNuevaEspecialidad.ShowDialog();

            if (formNuevaEspecialidad.Guardado && formNuevaEspecialidad.EspecialidadEditada != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.CreateAsync(formNuevaEspecialidad.EspecialidadEditada);
                    await LoadEspecialidadesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar especialidad: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private async void EditarEspecialidadSeleccionada(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una especialidad para editar",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var especialidadSeleccionada = (EspecialidadDto)dataGridView.SelectedRows[0].DataBoundItem;
            if (especialidadSeleccionada == null)
            {
                MessageBox.Show("No se pudo obtener la especialidad seleccionada",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var formEditarEspecialidad = new EditarEspecialidadForm(especialidadSeleccionada);
            formEditarEspecialidad.ShowDialog();

            if (formEditarEspecialidad.Guardado && formEditarEspecialidad.EspecialidadEditada != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.UpdateAsync(formEditarEspecialidad.EspecialidadEditada);
                    await LoadEspecialidadesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar especialidad: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private async void EliminarEspecialidadSeleccionada(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una especialidad para eliminar",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var especialidadSeleccionada = (EspecialidadDto)dataGridView.SelectedRows[0].DataBoundItem;
            if (especialidadSeleccionada == null)
            {
                MessageBox.Show("No se pudo obtener la especialidad seleccionada",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var resultado = MessageBox.Show(
                $"¿Está seguro que desea eliminar la especialidad '{especialidadSeleccionada.Descripcion}'?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.DeleteAsync(especialidadSeleccionada.Id);
                    await LoadEspecialidadesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar especialidad: {ex.Message}",
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