using API.Clients;
using DTOs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormAlumnos : Form
    {
        private readonly PersonaApiClient _apiClient;
        private readonly Form _menuPrincipal;
        private BindingList<PersonaDto> _alumnos = new BindingList<PersonaDto>();

        public FormAlumnos(Form menuPrincipal = null)
        {
            InitializeComponent();
            _menuPrincipal = menuPrincipal;

            try
            {
                string apiUrl = "https://localhost:7229";
                Debug.WriteLine($"Conectando a API en: {apiUrl}");
                _apiClient = new PersonaApiClient();

                ConfigurarDataGridView();

                // Asignar eventos exactamente como los otros formularios
                this.Load += FormAlumnos_Load;
                btnNuevo.Click += (s, e) => CrearNuevoAlumno();
                btnEditar.Click += (s, e) => EditarAlumnoSeleccionado(dataGridViewAlumnos);
                btnEliminar.Click += (s, e) => EliminarAlumnoSeleccionado(dataGridViewAlumnos);
                btnVolver.Click += (s, e) => VolverAlMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar: {ex.Message}",
                    "Error de inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dataGridViewAlumnos.AutoGenerateColumns = false;
            dataGridViewAlumnos.Columns.Clear();

            dataGridViewAlumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50
            });
            dataGridViewAlumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Legajo",
                HeaderText = "Legajo",
                Width = 80
            });
            dataGridViewAlumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dataGridViewAlumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Apellido",
                HeaderText = "Apellido",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dataGridViewAlumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                HeaderText = "Email",
                Width = 200
            });

            dataGridViewAlumnos.DataSource = _alumnos;
        }

        private async void FormAlumnos_Load(object sender, EventArgs e)
        {
            await LoadAlumnosAsync();
        }

        private async Task LoadAlumnosAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var alumnos = await _apiClient.GetAllAlumnosAsync();

                _alumnos.Clear();
                if (alumnos != null)
                {
                    foreach (var alumno in alumnos)
                    {
                        _alumnos.Add(alumno);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alumnos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async void CrearNuevoAlumno()
        {
            var formNuevoAlumno = new EditarAlumnoForm();
            formNuevoAlumno.ShowDialog();

            if (formNuevoAlumno.Guardado && formNuevoAlumno.AlumnoEditado != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.CreateAsync(formNuevoAlumno.AlumnoEditado);
                    await LoadAlumnosAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar alumno: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private async void EditarAlumnoSeleccionado(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un alumno para editar",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var alumnoSeleccionado = (PersonaDto)dataGridView.SelectedRows[0].DataBoundItem;
            if (alumnoSeleccionado == null)
            {
                MessageBox.Show("No se pudo obtener el alumno seleccionado",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var formEditarAlumno = new EditarAlumnoForm(alumnoSeleccionado);
            formEditarAlumno.ShowDialog();

            if (formEditarAlumno.Guardado && formEditarAlumno.AlumnoEditado != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.UpdateAsync(alumnoSeleccionado.Id, formEditarAlumno.AlumnoEditado);
                    await LoadAlumnosAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar alumno: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private async void EliminarAlumnoSeleccionado(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un alumno para eliminar",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var alumnoSeleccionado = (PersonaDto)dataGridView.SelectedRows[0].DataBoundItem;
            if (alumnoSeleccionado == null)
            {
                MessageBox.Show("No se pudo obtener el alumno seleccionado",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var resultado = MessageBox.Show(
                $"¿Está seguro que desea eliminar al alumno {alumnoSeleccionado.Nombre} {alumnoSeleccionado.Apellido}?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.DeleteAsync(alumnoSeleccionado.Id);
                    await LoadAlumnosAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar alumno: {ex.Message}",
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