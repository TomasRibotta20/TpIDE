using API.Clients;
using DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormAsignarProfesores : Form
    {
        private readonly Form _menuAnterior;
        private readonly CursoApiClient _cursoApiClient;
        private readonly PersonaApiClient _personaApiClient;
        private ComboBox cmbCurso;
        private ComboBox cmbProfesor;
        private Label lblTitulo;
        private Label lblCursoInfo;
        private Button btnAsignar;
        private Button btnVolver;
        private DataGridView dgvProfesoresAsignados;

        public FormAsignarProfesores(Form menuAnterior)
        {
            _menuAnterior = menuAnterior;
            _cursoApiClient = new CursoApiClient();
            _personaApiClient = new PersonaApiClient();
            
            InitializeComponent();
            CargarDatosAsync();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Size = new Size(1000, 650);
            this.Text = "Asignar Profesores a Cursos";
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 244, 248),
                Padding = new Padding(20)
            };

            lblTitulo = new Label
            {
                Text = "????? Asignar Profesores a Cursos",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            // Selección de Curso
            var lblCurso = new Label
            {
                Text = "Curso:",
                Font = new Font("Segoe UI", 12),
                Location = new Point(30, 90),
                AutoSize = true
            };

            cmbCurso = new ComboBox
            {
                Location = new Point(120, 87),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCurso.SelectedIndexChanged += CmbCurso_SelectedIndexChanged;

            // Información del curso
            lblCursoInfo = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 130),
                Size = new Size(800, 40),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            // Selección de Profesor
            var lblProfesor = new Label
            {
                Text = "Profesor:",
                Font = new Font("Segoe UI", 12),
                Location = new Point(30, 190),
                AutoSize = true
            };

            cmbProfesor = new ComboBox
            {
                Location = new Point(120, 187),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Botón Asignar
            btnAsignar = new Button
            {
                Text = "? Asignar Profesor",
                Size = new Size(180, 45),
                Location = new Point(540, 185),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAsignar.FlatAppearance.BorderSize = 0;
            btnAsignar.Click += BtnAsignar_Click;

            // Grid de profesores ya asignados
            var lblAsignados = new Label
            {
                Text = "Profesores Asignados:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(30, 250),
                AutoSize = true
            };

            dgvProfesoresAsignados = new DataGridView
            {
                Location = new Point(30, 285),
                Size = new Size(920, 250),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Botón Volver
            btnVolver = new Button
            {
                Text = "? Volver",
                Size = new Size(150, 45),
                Location = new Point(800, 555),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.Click += BtnVolver_Click;

            panelPrincipal.Controls.Add(lblTitulo);
            panelPrincipal.Controls.Add(lblCurso);
            panelPrincipal.Controls.Add(cmbCurso);
            panelPrincipal.Controls.Add(lblCursoInfo);
            panelPrincipal.Controls.Add(lblProfesor);
            panelPrincipal.Controls.Add(cmbProfesor);
            panelPrincipal.Controls.Add(btnAsignar);
            panelPrincipal.Controls.Add(lblAsignados);
            panelPrincipal.Controls.Add(dgvProfesoresAsignados);
            panelPrincipal.Controls.Add(btnVolver);
            
            this.Controls.Add(panelPrincipal);
            this.ResumeLayout(false);
        }

        private async System.Threading.Tasks.Task CargarDatosAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                // Cargar cursos
                var cursosEnumerable = await _cursoApiClient.GetAllAsync();
                var cursos = cursosEnumerable.ToList();
                
                if (cursos != null && cursos.Any())
                {
                    cmbCurso.DataSource = cursos.Select(c => new
                    {
                        Value = c.IdCurso,
                        Display = $"{c.Nombre} - {c.Comision} ({c.AnioCalendario})"
                    }).ToList();
                    
                    cmbCurso.DisplayMember = "Display";
                    cmbCurso.ValueMember = "Value";
                }

                // Cargar profesores
                var personas = await _personaApiClient.GetAllAsync();
                var profesores = personas?.Where(p => p.TipoPersona == TipoPersonaDto.Profesor).ToList();
                
                if (profesores != null && profesores.Any())
                {
                    cmbProfesor.DataSource = profesores.Select(p => new
                    {
                        Value = p.Id,
                        Display = $"{p.Apellido}, {p.Nombre} (Leg: {p.Legajo})"
                    }).ToList();
                    
                    cmbProfesor.DisplayMember = "Display";
                    cmbProfesor.ValueMember = "Value";
                }
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

        private void CmbCurso_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbCurso.SelectedValue != null)
            {
                // Aquí mostrarías información del curso seleccionado
                lblCursoInfo.Text = "Nota: La funcionalidad de asignar profesores está en preparación.\n" +
                                   "Requiere extender el modelo Curso para incluir profesores.";
                
                // Por ahora, dejamos el grid vacío
                dgvProfesoresAsignados.DataSource = null;
            }
        }

        private async void BtnAsignar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cmbCurso.SelectedValue == null || cmbProfesor.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un curso y un profesor.", 
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show(
                    "Funcionalidad en desarrollo.\n\n" +
                    "Para implementar completamente esta función, es necesario:\n" +
                    "1. Agregar tabla CursosProfesores en la base de datos\n" +
                    "2. Crear endpoints en la API\n" +
                    "3. Implementar lógica de asignación",
                    "En Desarrollo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVolver_Click(object? sender, EventArgs e)
        {
            _menuAnterior.Show();
            this.Close();
        }
    }
}
