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
        private readonly CursoApiClient _cursoApiClient;
        private DataGridView dgvMisCursos;
        private Label lblTitulo;
        private Button btnVolver;
        private Button btnActualizar;

        public FormMisCursosProfesor(int personaId)
        {
            _personaId = personaId;
            _cursoApiClient = new CursoApiClient();
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
                
                // Obtener cursos del profesor
                // Nota: Asumiendo que existe un metodo en CursoApiClient para obtener cursos por profesor
                // Si no existe, deberas implementarlo en el backend
                var cursos = await _cursoApiClient.GetAllAsync();
                
                if (cursos == null || !cursos.Any())
                {
                    MessageBox.Show("No tienes cursos asignados.", 
                        "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvMisCursos.DataSource = null;
                    return;
                }

                // Filtrar cursos donde el profesor este asignado
                // Por ahora mostramos todos, pero deberias filtrar por IdProfesor
                var cursosData = cursos.Select(c => new
                {
                    IdCurso = c.IdCurso,
                    Curso = c.Nombre,
                    Comision = c.Comision,
                    Anio = c.AnioCalendario,
                    Cupo = c.Cupo,
                    Inscriptos = c.InscriptosCount,
                    Disponibles = c.Cupo - c.InscriptosCount
                }).ToList();

                dgvMisCursos.DataSource = cursosData;
                
                dgvMisCursos.Columns["IdCurso"].Visible = false;
                dgvMisCursos.Columns["Curso"].HeaderText = "Curso";
                dgvMisCursos.Columns["Comision"].HeaderText = "Comision";
                dgvMisCursos.Columns["Anio"].HeaderText = "Año";
                dgvMisCursos.Columns["Cupo"].HeaderText = "Cupo Total";
                dgvMisCursos.Columns["Inscriptos"].HeaderText = "Inscriptos";
                dgvMisCursos.Columns["Disponibles"].HeaderText = "Disponibles";

                // Colorear filas segun disponibilidad
                foreach (DataGridViewRow row in dgvMisCursos.Rows)
                {
                    int disponibles = Convert.ToInt32(row.Cells["Disponibles"].Value);
                    if (disponibles > 5)
                        row.DefaultCellStyle.BackColor = Color.FromArgb(200, 247, 197);
                    else if (disponibles > 0)
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 205);
                    else
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224);
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
