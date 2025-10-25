using API.Clients;
using DTOs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormCursos : Form
    {
        private readonly CursoApiClient _apiClient;
        private readonly Form _menuPrincipal;
        private BindingList<CursoDto> _cursos = new BindingList<CursoDto>();

        public FormCursos(Form menuPrincipal = null)
        {
            InitializeComponent();
            _menuPrincipal = menuPrincipal;

            try
            {
                string apiUrl = "https://localhost:7229";
                Debug.WriteLine($"Conectando a API en: {apiUrl}");
                _apiClient = new CursoApiClient();

                ConfigurarDataGridView();

                // Asignar eventos
                this.Load += FormCursos_Load;
                btnNuevo.Click += (s, e) => CrearNuevoCurso();
                btnEditar.Click += (s, e) => EditarCursoSeleccionado(dataGridViewCursos);
                btnEliminar.Click += (s, e) => EliminarCursoSeleccionado(dataGridViewCursos);
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
            dataGridViewCursos.AutoGenerateColumns = false;
            dataGridViewCursos.Columns.Clear();

            dataGridViewCursos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdCurso",
                HeaderText = "ID",
                Width = 50
            });
            dataGridViewCursos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreMateria",
                HeaderText = "Materia",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dataGridViewCursos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DescComision",
                HeaderText = "Comisión",
                Width = 120
            });
            dataGridViewCursos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "AnioCalendario",
                HeaderText = "Año",
                Width = 60
            });
            dataGridViewCursos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cupo",
                HeaderText = "Cupo",
                Width = 60
            });
            dataGridViewCursos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "InscriptosActuales",
                HeaderText = "Inscriptos",
                Width = 80
            });

            dataGridViewCursos.DataSource = _cursos;
        }

        private async void FormCursos_Load(object sender, EventArgs e)
        {
            await LoadCursosAsync();
        }

        private async Task LoadCursosAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var cursos = await _apiClient.GetAllAsync();

                _cursos.Clear();
                if (cursos != null)
                {
                    foreach (var curso in cursos)
                    {
                        _cursos.Add(curso);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cursos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async void CrearNuevoCurso()
        {
            try
            {
                var formNuevoCurso = new EditarCursoForm();
                var resultado = formNuevoCurso.ShowDialog();

                if (resultado == DialogResult.OK && formNuevoCurso.Guardado && formNuevoCurso.CursoEditado != null)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        await _apiClient.CreateAsync(formNuevoCurso.CursoEditado);
                        await LoadCursosAsync();
                        MessageBox.Show("Curso creado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar curso: {ex.Message}",
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
                MessageBox.Show($"Error al crear nuevo curso: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void EditarCursoSeleccionado(DataGridView dataGridView)
        {
            try
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un curso para editar.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var cursoSeleccionado = (CursoDto)dataGridView.SelectedRows[0].DataBoundItem;

                var formEditarCurso = new EditarCursoForm(cursoSeleccionado);
                var resultado = formEditarCurso.ShowDialog();

                if (resultado == DialogResult.OK && formEditarCurso.Guardado && formEditarCurso.CursoEditado != null)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        await _apiClient.UpdateAsync(formEditarCurso.CursoEditado);
                        await LoadCursosAsync();
                        MessageBox.Show("Curso actualizado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar curso: {ex.Message}",
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
                MessageBox.Show($"Error al editar curso: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void EliminarCursoSeleccionado(DataGridView dataGridView)
        {
            try
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un curso para eliminar.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var cursoSeleccionado = (CursoDto)dataGridView.SelectedRows[0].DataBoundItem;

                var confirmResult = MessageBox.Show(
                    $"¿Está seguro de que desea eliminar el curso ID: {cursoSeleccionado.IdCurso}?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        await _apiClient.DeleteAsync(cursoSeleccionado.IdCurso);
                        await LoadCursosAsync();
                        MessageBox.Show("Curso eliminado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar curso: {ex.Message}",
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
                MessageBox.Show($"Error al eliminar curso: {ex.Message}",
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
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al volver al menú: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}