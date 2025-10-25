using API.Clients;
using DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormMisCursosProfesor : Form
    {
        private readonly int _personaId;
        private readonly DocenteCursoApiClient _docenteCursoApiClient;
        private DataGridView dgvMisCursos;
        private Label lblTitulo;
        private Button btnVolver;
        private Button btnActualizar;

        public FormMisCursosProfesor(int personaId)
        {
            _personaId = personaId;
            _docenteCursoApiClient = new DocenteCursoApiClient();
            InitializeComponent();
            this.Load += FormMisCursosProfesor_Load;
        }

        private async void FormMisCursosProfesor_Load(object? sender, EventArgs e)
        {
            await CargarMisCursosAsync();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Size = new Size(1000, 600);
            this.Text = "Mis Cursos - Profesor";
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 244, 248),
                Padding = new Padding(20)
            };

            lblTitulo = new Label
            {
                Text = "Mis Cursos Asignados",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            dgvMisCursos = new DataGridView
            {
                Location = new Point(30, 80),
                Size = new Size(920, 400),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            btnActualizar = new Button
            {
                Text = "Actualizar",
                Size = new Size(150, 40),
                Location = new Point(30, 500),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.Click += async (s, e) => await CargarMisCursosAsync();

            btnVolver = new Button
            {
                Text = "Volver al Menu",
                Size = new Size(150, 40),
                Location = new Point(800, 500),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.Click += BtnVolver_Click;

            panelPrincipal.Controls.Add(lblTitulo);
            panelPrincipal.Controls.Add(dgvMisCursos);
            panelPrincipal.Controls.Add(btnActualizar);
            panelPrincipal.Controls.Add(btnVolver);
            
            this.Controls.Add(panelPrincipal);
            this.ResumeLayout(false);
        }

        private async System.Threading.Tasks.Task CargarMisCursosAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                // Obtener cursos asignados al profesor a través de docentes_cursos
                var asignaciones = await _docenteCursoApiClient.GetByDocenteIdAsync(_personaId);
                
                if (asignaciones == null || !asignaciones.Any())
                {
                    MessageBox.Show("No tienes cursos asignados.\n\nUn administrador debe asignarte cursos desde el menú 'Gestionar Docentes por Curso'.", 
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvMisCursos.DataSource = null;
                    return;
                }

                // Preparar datos para mostrar
                var cursosData = asignaciones.Select(a => new
                {
                    IdDictado = a.IdDictado,
                    IdCurso = a.IdCurso,
                    Materia = a.NombreMateria ?? "N/A",
                    Comision = a.DescComision ?? "N/A",
                    Anio = a.AnioCalendario ?? 0,
                    Cargo = a.CargoDescripcion,
                    DescripcionCurso = a.DescripcionCurso
                }).ToList();

                dgvMisCursos.DataSource = cursosData;
                
                dgvMisCursos.Columns["IdDictado"].Visible = false;
                dgvMisCursos.Columns["IdCurso"].Visible = false;
                dgvMisCursos.Columns["Materia"].HeaderText = "Materia";
                dgvMisCursos.Columns["Comision"].HeaderText = "Comisión";
                dgvMisCursos.Columns["Anio"].HeaderText = "Año";
                dgvMisCursos.Columns["Cargo"].HeaderText = "Cargo";
                dgvMisCursos.Columns["DescripcionCurso"].HeaderText = "Descripción Completa";

                // Colorear filas según cargo
                foreach (DataGridViewRow row in dgvMisCursos.Rows)
                {
                    string cargo = row.Cells["Cargo"].Value?.ToString() ?? "";
                    if (cargo == "Jefe de Cátedra")
                        row.DefaultCellStyle.BackColor = Color.FromArgb(200, 230, 255); // Azul claro
                    else if (cargo == "Titular")
                        row.DefaultCellStyle.BackColor = Color.FromArgb(200, 255, 200); // Verde claro
                    else if (cargo == "Auxiliar")
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 200); // Amarillo claro
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

        private void BtnVolver_Click(object? sender, EventArgs e)
        {
            var menuProfesor = new MenuProfesor();
            menuProfesor.Show();
            this.Close();
        }
    }
}
