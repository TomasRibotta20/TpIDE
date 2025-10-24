using API.Clients;
using DTOs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormProfesores : Form
    {
        private readonly PersonaApiClient _apiClient;
        private readonly Form _menuPrincipal;
        private BindingList<PersonaDto> _profesores = new BindingList<PersonaDto>();

        public FormProfesores(Form menuPrincipal = null)
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
                this.Load += FormProfesores_Load;
                btnNuevo.Click += (s, e) => CrearNuevoProfesor();
                btnEditar.Click += (s, e) => EditarProfesorSeleccionado(dataGridViewProfesores);
                btnEliminar.Click += (s, e) => EliminarProfesorSeleccionado(dataGridViewProfesores);
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
            try
            {
                dataGridViewProfesores.AutoGenerateColumns = false;
                dataGridViewProfesores.Columns.Clear();

                dataGridViewProfesores.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Id",
                    HeaderText = "ID",
                    Width = 50
                });
                dataGridViewProfesores.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Legajo",
                    HeaderText = "Legajo",
                    Width = 80
                });
                dataGridViewProfesores.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Nombre",
                    HeaderText = "Nombre",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridViewProfesores.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Apellido",
                    HeaderText = "Apellido",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridViewProfesores.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Email",
                    HeaderText = "Email",
                    Width = 200
                });

                dataGridViewProfesores.DataSource = _profesores;
                dataGridViewProfesores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewProfesores.MultiSelect = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar DataGridView: {ex.Message}",
                    "Error de configuración", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void FormProfesores_Load(object sender, EventArgs e)
        {
            await LoadProfesoresAsync();
        }

        private async Task LoadProfesoresAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var profesores = await _apiClient.GetAllProfesoresAsync();

                _profesores.Clear();
                if (profesores != null)
                {
                    foreach (var profesor in profesores)
                    {
                        if (profesor != null)
                        {
                            _profesores.Add(profesor);
                        }
                    }
                }

                // Refrescar el DataGridView
                dataGridViewProfesores.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar profesores: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async void CrearNuevoProfesor()
        {
            try
            {
                var formNuevoProfesor = new EditarProfesorForm();
                formNuevoProfesor.ShowDialog();

                if (formNuevoProfesor.Guardado && formNuevoProfesor.ProfesorEditado != null)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        await _apiClient.CreateAsync(formNuevoProfesor.ProfesorEditado);
                        await LoadProfesoresAsync();
                        
                        MessageBox.Show("Profesor creado exitosamente.", 
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar profesor: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear nuevo profesor: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void EditarProfesorSeleccionado(DataGridView dataGridView)
        {
            try
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un profesor para editar",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var profesorSeleccionado = (PersonaDto)dataGridView.SelectedRows[0].DataBoundItem;
                if (profesorSeleccionado == null)
                {
                    MessageBox.Show("No se pudo obtener el profesor seleccionado",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var formEditarProfesor = new EditarProfesorForm(profesorSeleccionado);
                formEditarProfesor.ShowDialog();

                if (formEditarProfesor.Guardado && formEditarProfesor.ProfesorEditado != null)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        await _apiClient.UpdateAsync(profesorSeleccionado.Id, formEditarProfesor.ProfesorEditado);
                        await LoadProfesoresAsync();
                        
                        MessageBox.Show("Profesor actualizado exitosamente.", 
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar profesor: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar profesor: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void EliminarProfesorSeleccionado(DataGridView dataGridView)
        {
            try
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un profesor para eliminar",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var profesorSeleccionado = (PersonaDto)dataGridView.SelectedRows[0].DataBoundItem;
                if (profesorSeleccionado == null)
                {
                    MessageBox.Show("No se pudo obtener el profesor seleccionado",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var resultado = MessageBox.Show(
                    $"¿Está seguro que desea eliminar al profesor {profesorSeleccionado.Nombre} {profesorSeleccionado.Apellido}?",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        await _apiClient.DeleteAsync(profesorSeleccionado.Id);
                        await LoadProfesoresAsync();
                        
                        MessageBox.Show("Profesor eliminado exitosamente.", 
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar profesor: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar profesor: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VolverAlMenu()
        {
            try
            {
                if (_menuPrincipal != null)
                {
                    _menuPrincipal.Show();
                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al volver al menú: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                base.OnFormClosed(e);
                if (_menuPrincipal != null && !_menuPrincipal.Visible)
                {
                    _menuPrincipal.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}