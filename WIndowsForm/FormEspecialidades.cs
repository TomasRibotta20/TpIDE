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

                ConfigureForm();
                this.Load += FormEspecialidades_Load;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar: {ex.Message}",
                    "Error de inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureForm()
        {
            this.Text = "Gestión de Especialidades";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel superior para DataGridView
            Panel gridPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            // DataGridView para mostrar especialidades
            DataGridView dataGridViewEspecialidades = new DataGridView
            {
                Name = "dataGridViewEspecialidades",
                Dock = DockStyle.Fill,
                DataSource = _especialidades,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Panel de botones
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60
            };

            Button btnNueva = new Button
            {
                Text = "Nueva Especialidad",
                Width = 140,
                Location = new Point(10, 15)
            };

            Button btnEditar = new Button
            {
                Text = "Editar",
                Width = 100,
                Location = new Point(160, 15)
            };

            Button btnEliminar = new Button
            {
                Text = "Eliminar",
                Width = 100,
                Location = new Point(270, 15)
            };

            Button btnVolver = new Button
            {
                Text = "Volver al Menú",
                Width = 120,
                Location = new Point(gridPanel.Width - 140, 15),
                Anchor = AnchorStyles.Right
            };

            // Eventos
            btnNueva.Click += (s, e) => CrearNuevaEspecialidad();
            btnEditar.Click += (s, e) => EditarEspecialidadSeleccionada(dataGridViewEspecialidades);
            btnEliminar.Click += (s, e) => EliminarEspecialidadSeleccionada(dataGridViewEspecialidades);
            btnVolver.Click += (s, e) => VolverAlMenu();

            // Agregar controles
            buttonPanel.Controls.Add(btnNueva);
            buttonPanel.Controls.Add(btnEditar);
            buttonPanel.Controls.Add(btnEliminar);
            buttonPanel.Controls.Add(btnVolver);

            gridPanel.Controls.Add(dataGridViewEspecialidades);

            this.Controls.Add(gridPanel);
            this.Controls.Add(buttonPanel);
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