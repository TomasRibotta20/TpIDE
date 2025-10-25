using API.Clients;
using DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormMisCursosAlumno : Form
    {
        private readonly int _personaId;
        private readonly InscripcionApiClient _inscripcionApiClient;
        private DataGridView dgvMisCursos;
        private Label lblTitulo;
        private Button btnVolver;
        private Button btnActualizar;

        public FormMisCursosAlumno(int personaId)
        {
            _personaId = personaId;
            _inscripcionApiClient = new InscripcionApiClient();
            InitializeComponent();
            this.Load += FormMisCursosAlumno_Load;
        }

        private async void FormMisCursosAlumno_Load(object? sender, EventArgs e)
        {
            await CargarMisCursosAsync();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Configuracion del formulario
            this.Size = new Size(1000, 600);
            this.Text = "Mis Cursos e Inscripciones";
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel principal
            var panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 244, 248),
                Padding = new Padding(20)
            };

            // Titulo
            lblTitulo = new Label
            {
                Text = "Mis Cursos e Inscripciones",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            // DataGridView para mostrar los cursos
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

            // Boton Actualizar
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

            // Boton Volver
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
                
                // Obtener inscripciones del alumno
                var inscripciones = await _inscripcionApiClient.GetByAlumnoIdAsync(_personaId);
                
                if (inscripciones == null || !inscripciones.Any())
                {
                    MessageBox.Show("No tienes inscripciones registradas.", 
                        "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvMisCursos.DataSource = null;
                    return;
                }

                // Preparar datos para el grid
                var cursosData = inscripciones.Select(i => new
                {
                    IdInscripcion = i.IdInscripcion,
                    Curso = i.DescripcionCurso ?? "N/A",
                    Condicion = ObtenerTextoCondicion(i.Condicion),
                    Nota = i.Nota.HasValue ? i.Nota.Value.ToString() : "Sin nota",
                    Estado = DeterminarEstado(i.Condicion, i.Nota)
                }).ToList();

                dgvMisCursos.DataSource = cursosData;
                
                // Configurar columnas
                dgvMisCursos.Columns["IdInscripcion"].Visible = false;
                dgvMisCursos.Columns["Curso"].HeaderText = "Curso";
                dgvMisCursos.Columns["Condicion"].HeaderText = "Condicion";
                dgvMisCursos.Columns["Nota"].HeaderText = "Nota";
                dgvMisCursos.Columns["Estado"].HeaderText = "Estado";

                // Colorear filas segun condicion
                foreach (DataGridViewRow row in dgvMisCursos.Rows)
                {
                    var condicion = row.Cells["Condicion"].Value?.ToString();
                    switch (condicion)
                    {
                        case "Promocional":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(200, 247, 197); // Verde claro
                            break;
                        case "Regular":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 205); // Amarillo claro
                            break;
                        case "Libre":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224); // Rojo claro
                            break;
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

        private string ObtenerTextoCondicion(CondicionAlumnoDto condicion)
        {
            return condicion switch
            {
                CondicionAlumnoDto.Promocional => "Promocional",
                CondicionAlumnoDto.Regular => "Regular",
                CondicionAlumnoDto.Libre => "Libre",
                _ => "N/A"
            };
        }

        private string DeterminarEstado(CondicionAlumnoDto condicion, int? nota)
        {
            if (condicion == CondicionAlumnoDto.Promocional)
                return "Aprobado";
            else if (condicion == CondicionAlumnoDto.Regular && nota.HasValue && nota >= 6)
                return "Aprobado";
            else if (condicion == CondicionAlumnoDto.Regular)
                return "Pendiente de examen";
            else if (condicion == CondicionAlumnoDto.Libre)
                return "Debe recursar";
            
            return "En curso";
        }

        private void BtnVolver_Click(object? sender, EventArgs e)
        {
            var menuAlumno = new MenuAlumno();
            menuAlumno.Show();
            this.Close();
        }
    }
}
