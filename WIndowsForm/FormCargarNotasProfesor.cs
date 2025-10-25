using API.Clients;
using DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormCargarNotasProfesor : Form
    {
        private readonly int _personaId;
        private readonly Form _menuAnterior;
        private readonly InscripcionApiClient _inscripcionApiClient;
        private readonly DocenteCursoApiClient _docenteCursoApiClient;
        private ComboBox cmbCurso;
        private DataGridView dgvAlumnos;
        private Label lblTitulo;
        private Button btnVolver;
        private Button btnGuardar;
        private List<DocenteCursoDto>? _cursosAsignados;
        private List<AlumnoCursoDto>? _inscripcionesActuales;

        public FormCargarNotasProfesor(int personaId, Form menuAnterior)
        {
            _personaId = personaId;
            _menuAnterior = menuAnterior;
            _inscripcionApiClient = new InscripcionApiClient();
            _docenteCursoApiClient = new DocenteCursoApiClient();
            
            InitializeComponent();
            this.Load += FormCargarNotasProfesor_Load;
        }

        private async void FormCargarNotasProfesor_Load(object? sender, EventArgs e)
        {
            await CargarCursosAsync();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Size = new Size(1100, 700);
            this.Text = "Cargar Notas y Condiciones";
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 244, 248),
                Padding = new Padding(20)
            };

            lblTitulo = new Label
            {
                Text = "Cargar Notas y Condiciones",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            var lblSeleccionCurso = new Label
            {
                Text = "Seleccionar Curso:",
                Font = new Font("Segoe UI", 12),
                Location = new Point(30, 80),
                AutoSize = true
            };

            cmbCurso = new ComboBox
            {
                Location = new Point(200, 77),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCurso.SelectedIndexChanged += CmbCurso_SelectedIndexChanged;

            dgvAlumnos = new DataGridView
            {
                Location = new Point(30, 130),
                Size = new Size(1020, 450),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                EditMode = DataGridViewEditMode.EditOnEnter
            };
            
            // Agregar manejador para clicks en celdas de condición
            dgvAlumnos.CellClick += DgvAlumnos_CellClick;
            dgvAlumnos.CellEndEdit += DgvAlumnos_CellEndEdit;

            btnGuardar = new Button
            {
                Text = "Guardar Cambios",
                Size = new Size(180, 45),
                Location = new Point(30, 600),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            btnVolver = new Button
            {
                Text = "Volver",
                Size = new Size(150, 45),
                Location = new Point(870, 600),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.Click += BtnVolver_Click;

            panelPrincipal.Controls.Add(lblTitulo);
            panelPrincipal.Controls.Add(lblSeleccionCurso);
            panelPrincipal.Controls.Add(cmbCurso);
            panelPrincipal.Controls.Add(dgvAlumnos);
            panelPrincipal.Controls.Add(btnGuardar);
            panelPrincipal.Controls.Add(btnVolver);
            
            this.Controls.Add(panelPrincipal);
            this.ResumeLayout(false);
        }

        private async System.Threading.Tasks.Task CargarCursosAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                // Obtener solo los cursos asignados al profesor
                var asignacionesEnumerable = await _docenteCursoApiClient.GetByDocenteIdAsync(_personaId);
                _cursosAsignados = asignacionesEnumerable.ToList();
                
                if (_cursosAsignados != null && _cursosAsignados.Any())
                {
                    cmbCurso.DisplayMember = "Display";
                    cmbCurso.ValueMember = "Value";
                    cmbCurso.DataSource = _cursosAsignados.Select(a => new CursoComboItem
                    {
                        Value = a.IdCurso,
                        Display = $"{a.NombreMateria} - {a.DescComision} ({a.AnioCalendario}) - Cargo: {a.CargoDescripcion}"
                    }).ToList();
                }
                else
                {
                    MessageBox.Show(
                        "No tienes cursos asignados.\n\n" +
                        "Un administrador debe asignarte cursos desde el menú:\n" +
                        "Profesor ? Gestionar Docentes por Curso", 
                        "Sin cursos asignados", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cursos: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async void CmbCurso_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbCurso.SelectedValue == null) return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                int cursoId = (int)cmbCurso.SelectedValue;
                _inscripcionesActuales = await _inscripcionApiClient.GetByCursoIdAsync(cursoId);
                
                if (_inscripcionesActuales != null && _inscripcionesActuales.Any())
                {
                    ConfigurarGridAlumnos();
                }
                else
                {
                    dgvAlumnos.DataSource = null;
                    dgvAlumnos.Rows.Clear();
                    dgvAlumnos.Columns.Clear();
                    MessageBox.Show("No hay alumnos inscriptos en este curso.", 
                        "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ConfigurarGridAlumnos()
        {
            dgvAlumnos.Columns.Clear();
            dgvAlumnos.Rows.Clear();
            dgvAlumnos.AutoGenerateColumns = false;

            // Columna Alumno (readonly)
            dgvAlumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Alumno",
                HeaderText = "Alumno",
                ReadOnly = true,
                FillWeight = 40
            });

            // Columna Legajo (readonly)
            dgvAlumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Legajo",
                HeaderText = "Legajo",
                ReadOnly = true,
                FillWeight = 15
            });

            // Columna Condicion - USAR TEXTBOX en lugar de ComboBox
            dgvAlumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Condicion",
                HeaderText = "Condicion (Click para editar)",
                FillWeight = 20,
                ReadOnly = false
            });

            // Columna Nota (editable)
            dgvAlumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Nota",
                HeaderText = "Nota (1-10)",
                FillWeight = 15
            });

            // Columna IdInscripcion (oculta)
            dgvAlumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdInscripcion",
                Visible = false
            });

            // Agregar datos
            if (_inscripcionesActuales != null && _inscripcionesActuales.Any())
            {
                foreach (var inscripcion in _inscripcionesActuales)
                {
                    var index = dgvAlumnos.Rows.Add();
                    var row = dgvAlumnos.Rows[index];
                    
                    row.Cells["Alumno"].Value = $"{inscripcion.ApellidoAlumno}, {inscripcion.NombreAlumno}";
                    row.Cells["Legajo"].Value = inscripcion.LegajoAlumno;
                    row.Cells["Condicion"].Value = ObtenerTextoCondicion(inscripcion.Condicion);
                    row.Cells["Nota"].Value = inscripcion.Nota;
                    row.Cells["IdInscripcion"].Value = inscripcion.IdInscripcion;
                }
            }
        }

        // Manejador para mostrar un menú contextual al hacer click en la celda de Condición
        private void DgvAlumnos_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            
            // Solo para la columna Condicion
            if (dgvAlumnos.Columns[e.ColumnIndex].Name == "Condicion")
            {
                var currentValue = dgvAlumnos.Rows[e.RowIndex].Cells["Condicion"].Value?.ToString() ?? "Regular";
                
                // Mostrar un diálogo simple para seleccionar
                using (var form = new Form())
                {
                    form.Text = "Seleccionar Condición";
                    form.Size = new Size(300, 200);
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.FormBorderStyle = FormBorderStyle.FixedDialog;
                    form.MaximizeBox = false;
                    form.MinimizeBox = false;

                    var listBox = new ListBox
                    {
                        Location = new Point(20, 20),
                        Size = new Size(240, 80),
                        Font = new Font("Segoe UI", 11)
                    };
                    listBox.Items.Add("Regular");
                    listBox.Items.Add("Promocional");
                    listBox.Items.Add("Libre");
                    listBox.SelectedItem = currentValue;

                    var btnOk = new Button
                    {
                        Text = "Aceptar",
                        Location = new Point(100, 110),
                        Size = new Size(80, 30),
                        DialogResult = DialogResult.OK
                    };

                    form.Controls.Add(listBox);
                    form.Controls.Add(btnOk);
                    form.AcceptButton = btnOk;

                    if (form.ShowDialog() == DialogResult.OK && listBox.SelectedItem != null)
                    {
                        dgvAlumnos.Rows[e.RowIndex].Cells["Condicion"].Value = listBox.SelectedItem.ToString();
                    }
                }
            }
        }

        private void DgvAlumnos_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            // Validar que la condición ingresada sea válida
            if (dgvAlumnos.Columns[e.ColumnIndex].Name == "Condicion")
            {
                var value = dgvAlumnos.Rows[e.RowIndex].Cells["Condicion"].Value?.ToString();
                if (value != null && 
                    value != "Regular" && 
                    value != "Promocional" && 
                    value != "Libre")
                {
                    MessageBox.Show("Condición inválida. Use: Regular, Promocional o Libre", 
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvAlumnos.Rows[e.RowIndex].Cells["Condicion"].Value = "Regular";
                }
            }
        }

        private string ObtenerTextoCondicion(CondicionAlumnoDto condicion)
        {
            return condicion switch
            {
                CondicionAlumnoDto.Promocional => "Promocional",
                CondicionAlumnoDto.Regular => "Regular",
                CondicionAlumnoDto.Libre => "Libre",
                _ => "Regular"
            };
        }

        private CondicionAlumnoDto ObtenerCondicionDto(string texto)
        {
            return texto switch
            {
                "Promocional" => CondicionAlumnoDto.Promocional,
                "Regular" => CondicionAlumnoDto.Regular,
                "Libre" => CondicionAlumnoDto.Libre,
                _ => CondicionAlumnoDto.Regular
            };
        }

        private async void BtnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                var cambios = 0;

                foreach (DataGridViewRow row in dgvAlumnos.Rows)
                {
                    if (row.Cells["IdInscripcion"].Value == null) continue;
                    
                    int idInscripcion = Convert.ToInt32(row.Cells["IdInscripcion"].Value);
                    string condicionTexto = row.Cells["Condicion"].Value?.ToString() ?? "Regular";
                    var condicion = ObtenerCondicionDto(condicionTexto);
                    
                    int? nota = null;
                    if (row.Cells["Nota"].Value != null && 
                        int.TryParse(row.Cells["Nota"].Value.ToString(), out int notaInt))
                    {
                        if (notaInt >= 1 && notaInt <= 10)
                            nota = notaInt;
                        else
                        {
                            MessageBox.Show($"La nota debe estar entre 1 y 10 (Fila {row.Index + 1})", 
                                "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }
                    }

                    await _inscripcionApiClient.ActualizarCondicionYNotaAsync(idInscripcion, condicion, nota);
                    cambios++;
                }

                MessageBox.Show($"Se actualizaron {cambios} registros exitosamente.", 
                    "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargar datos
                if (cmbCurso.SelectedValue != null)
                {
                    int cursoId = (int)cmbCurso.SelectedValue;
                    _inscripcionesActuales = await _inscripcionApiClient.GetByCursoIdAsync(cursoId);
                    ConfigurarGridAlumnos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar cambios: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnVolver_Click(object? sender, EventArgs e)
        {
            _menuAnterior.Show();
            this.Close();
        }

        // Clase auxiliar para el ComboBox de cursos
        private class CursoComboItem
        {
            public int Value { get; set; }
            public string Display { get; set; } = string.Empty;
        }
    }
}
