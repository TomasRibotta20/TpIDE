using API.Clients;
using DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormGestionarDocentesCurso : Form
    {
        private readonly DocenteCursoApiClient _docenteCursoApiClient;
        private readonly CursoApiClient _cursoApiClient;
        private readonly PersonaApiClient _personaApiClient;
        private BindingList<DocenteCursoDto> _asignaciones = new BindingList<DocenteCursoDto>();
        private List<CursoDto> _cursos = new List<CursoDto>();
        private List<PersonaDto> _profesores = new List<PersonaDto>();

        public FormGestionarDocentesCurso()
        {
            InitializeComponent();
            _docenteCursoApiClient = new DocenteCursoApiClient();
            _cursoApiClient = new CursoApiClient();
            _personaApiClient = new PersonaApiClient();

            this.Load += FormGestionarDocentesCurso_Load;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Configuración del Form primero
            this.Text = "Gestión de Docentes por Curso";
            this.Size = new System.Drawing.Size(1100, 650);
            this.MinimumSize = new System.Drawing.Size(900, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.BackColor = FormStyles.Colors.Background;

            // Crear Header Panel
            var headerPanel = FormStyles.CreateHeaderPanel(
                "Gestión de Docentes por Curso",
                "Asigne profesores a cursos con diferentes cargos");
            headerPanel.Dock = DockStyle.Top;
            this.Controls.Add(headerPanel);

            // Crear Panel de Filtros
            var filtrosPanel = new Panel
            {
                BackColor = FormStyles.Colors.CardBackground,
                Dock = DockStyle.Top,
                Height = 70,
                Padding = new Padding(20, 15, 20, 15)
            };

            var lblFiltroCurso = new Label
            {
                Text = "Filtrar por Curso:",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(120, 25),
                Font = FormStyles.Fonts.Normal,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };

            var cmbFiltroCurso = new ComboBox
            {
                Name = "cmbFiltroCurso",
                Location = new System.Drawing.Point(145, 18),
                Size = new System.Drawing.Size(450, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = FormStyles.Fonts.Normal
            };
            FormStyles.StyleComboBox(cmbFiltroCurso);
            cmbFiltroCurso.SelectedIndexChanged += CmbFiltroCurso_SelectedIndexChanged;

            var btnMostrarTodos = FormStyles.CreateSecondaryButton("Mostrar Todos", null);
            btnMostrarTodos.Location = new System.Drawing.Point(610, 15);
            btnMostrarTodos.Size = new System.Drawing.Size(140, 35);
            btnMostrarTodos.Click += async (s, e) => await CargarAsignacionesAsync();

            filtrosPanel.Controls.AddRange(new Control[] { lblFiltroCurso, cmbFiltroCurso, btnMostrarTodos });
            this.Controls.Add(filtrosPanel);

            // Crear Panel contenedor para el DataGridView
            var gridPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 10, 20, 10),
                BackColor = FormStyles.Colors.Background
            };

            // Crear DataGridView
            var dataGridViewAsignaciones = new DataGridView
            {
                Name = "dataGridViewAsignaciones",
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false
            };

            ConfigurarDataGridView(dataGridViewAsignaciones);
            FormStyles.StyleDataGridView(dataGridViewAsignaciones);
            
            gridPanel.Controls.Add(dataGridViewAsignaciones);
            this.Controls.Add(gridPanel);

            // Crear Panel de Botones
            var buttonPanel = new Panel
            {
                BackColor = FormStyles.Colors.Background,
                Dock = DockStyle.Bottom,
                Height = 80,
                Padding = new Padding(20, 20, 20, 20)
            };

            var btnNuevo = FormStyles.CreateSuccessButton("Nueva Asignación", null);
            btnNuevo.Location = new System.Drawing.Point(20, 15);
            btnNuevo.Size = new System.Drawing.Size(160, 40);
            btnNuevo.Click += async (s, e) => await CrearNuevaAsignacion();

            var btnEditar = FormStyles.CreatePrimaryButton("Editar", null);
            btnEditar.Location = new System.Drawing.Point(190, 15);
            btnEditar.Size = new System.Drawing.Size(130, 40);
            btnEditar.Click += async (s, e) => await EditarAsignacionSeleccionada(dataGridViewAsignaciones);

            var btnEliminar = FormStyles.CreateDangerButton("Eliminar", null);
            btnEliminar.Location = new System.Drawing.Point(330, 15);
            btnEliminar.Size = new System.Drawing.Size(130, 40);
            btnEliminar.Click += async (s, e) => await EliminarAsignacionSeleccionada(dataGridViewAsignaciones);

            var btnVolver = FormStyles.CreateSecondaryButton("Volver", null);
            btnVolver.Location = new System.Drawing.Point(920, 15);
            btnVolver.Size = new System.Drawing.Size(130, 40);
            btnVolver.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnVolver.Click += (s, e) => this.Close();

            buttonPanel.Controls.AddRange(new Control[] { btnNuevo, btnEditar, btnEliminar, btnVolver });
            this.Controls.Add(buttonPanel);

            // Establecer orden de controles (importante para Dock)
            this.Controls.SetChildIndex(buttonPanel, 0);
            this.Controls.SetChildIndex(gridPanel, 1);
            this.Controls.SetChildIndex(filtrosPanel, 2);
            this.Controls.SetChildIndex(headerPanel, 3);

            this.ResumeLayout(false);
        }

        private void ConfigurarDataGridView(DataGridView dgv)
        {
            dgv.Columns.Clear();

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdDictado",
                HeaderText = "ID",
                Width = 60,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreCompleto",
                HeaderText = "Docente",
                Width = 250
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CargoDescripcion",
                HeaderText = "Cargo",
                Width = 150
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DescripcionCurso",
                HeaderText = "Curso - Comisión (Año)",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 300
            });

            dgv.DataSource = _asignaciones;
        }

        private async void FormGestionarDocentesCurso_Load(object sender, EventArgs e)
        {
            await CargarDatosInicialesAsync();
        }

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Cargar cursos
                var cursosEnumerable = await _cursoApiClient.GetAllAsync();
                _cursos = cursosEnumerable.ToList();

                var cmbFiltroCurso = this.Controls.Find("cmbFiltroCurso", true).FirstOrDefault() as ComboBox;
                if (cmbFiltroCurso != null)
                {
                    cmbFiltroCurso.DataSource = null;
                    cmbFiltroCurso.Items.Clear();
                    cmbFiltroCurso.Items.Add(new { Display = "-- Todos los cursos --", Value = 0 });
                    
                    foreach (var curso in _cursos.OrderBy(c => c.NombreMateria))
                    {
                        var descripcion = $"{curso.NombreMateria} - {curso.DescComision} ({curso.AnioCalendario})";
                        cmbFiltroCurso.Items.Add(new { Display = descripcion, Value = curso.IdCurso });
                    }
                    
                    cmbFiltroCurso.DisplayMember = "Display";
                    cmbFiltroCurso.ValueMember = "Value";
                    cmbFiltroCurso.SelectedIndex = 0;
                }

                // Cargar profesores
                var personasEnumerable = await _personaApiClient.GetAllAsync();
                _profesores = personasEnumerable
                    .Where(p => p.TipoPersona == TipoPersonaDto.Profesor)
                    .ToList();

                // Cargar asignaciones
                await CargarAsignacionesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async Task CargarAsignacionesAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var asignaciones = await _docenteCursoApiClient.GetAllAsync();

                _asignaciones.Clear();
                foreach (var asignacion in asignaciones)
                {
                    _asignaciones.Add(asignacion);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar asignaciones: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async void CmbFiltroCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = sender as ComboBox;
            if (combo == null || combo.SelectedItem == null) return;

            try
            {
                dynamic selectedItem = combo.SelectedItem;
                int cursoId = selectedItem.Value;

                if (cursoId == 0)
                {
                    await CargarAsignacionesAsync();
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    var asignaciones = await _docenteCursoApiClient.GetByCursoIdAsync(cursoId);

                    _asignaciones.Clear();
                    foreach (var asignacion in asignaciones)
                    {
                        _asignaciones.Add(asignacion);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async Task CrearNuevaAsignacion()
        {
            var formEditar = new FormEditarDocenteCurso(_cursos, _profesores);
            var resultado = formEditar.ShowDialog();

            if (resultado == DialogResult.OK && formEditar.AsignacionCreada != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _docenteCursoApiClient.CreateAsync(formEditar.AsignacionCreada);
                    await CargarAsignacionesAsync();
                    MessageBox.Show("Asignación creada exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al crear asignación: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private async Task EditarAsignacionSeleccionada(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una asignación para editar.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var asignacionSeleccionada = (DocenteCursoDto)dgv.SelectedRows[0].DataBoundItem;
            var formEditar = new FormEditarDocenteCurso(_cursos, _profesores, asignacionSeleccionada);
            var resultado = formEditar.ShowDialog();

            if (resultado == DialogResult.OK && formEditar.AsignacionCreada != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _docenteCursoApiClient.UpdateAsync(
                        asignacionSeleccionada.IdDictado,
                        formEditar.AsignacionCreada);
                    await CargarAsignacionesAsync();
                    MessageBox.Show("Asignación actualizada exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar asignación: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private async Task EliminarAsignacionSeleccionada(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una asignación para eliminar.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var asignacionSeleccionada = (DocenteCursoDto)dgv.SelectedRows[0].DataBoundItem;

            var confirmResult = MessageBox.Show(
                $"¿Está seguro de que desea eliminar la asignación de {asignacionSeleccionada.NombreCompleto} " +
                $"como {asignacionSeleccionada.CargoDescripcion} en {asignacionSeleccionada.DescripcionCurso}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _docenteCursoApiClient.DeleteAsync(asignacionSeleccionada.IdDictado);
                    await CargarAsignacionesAsync();
                    MessageBox.Show("Asignación eliminada exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar asignación: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }
    }
}
