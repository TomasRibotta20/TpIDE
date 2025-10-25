using API.Clients;
using DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormInscripcionAlumno : Form
    {
        private readonly int _personaId;
        private readonly Form _menuAnterior;
        private readonly CursoApiClient _cursoApiClient;
        private readonly InscripcionApiClient _inscripcionApiClient;
        private readonly PersonaApiClient _personaApiClient;
        private FlowLayoutPanel flowPanelCursos;
        private TextBox txtBuscar;
        private Label lblTitulo;
        private Button btnVolver;

        public FormInscripcionAlumno(int personaId, Form menuAnterior)
        {
            _personaId = personaId;
            _menuAnterior = menuAnterior;
            _cursoApiClient = new CursoApiClient();
            _inscripcionApiClient = new InscripcionApiClient();
            _personaApiClient = new PersonaApiClient();
            
            InitializeComponent();
            this.Load += FormInscripcionAlumno_Load;
        }

        private async void FormInscripcionAlumno_Load(object? sender, EventArgs e)
        {
            await CargarCursosDisponiblesAsync();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Size = new Size(1200, 700);
            this.Text = "Inscripcion a Cursos";
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
                Text = "Inscribirse a un Curso",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            // Buscador
            var lblBuscar = new Label
            {
                Text = "Buscar curso:",
                Font = new Font("Segoe UI", 11),
                Location = new Point(30, 80),
                AutoSize = true
            };

            txtBuscar = new TextBox
            {
                Location = new Point(150, 77),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 11)
            };
            txtBuscar.TextChanged += (s, e) => FiltrarCursos();

            // FlowLayoutPanel para las cards de cursos
            flowPanelCursos = new FlowLayoutPanel
            {
                Location = new Point(30, 120),
                Size = new Size(1120, 500),
                AutoScroll = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Boton Volver
            btnVolver = new Button
            {
                Text = "Volver",
                Size = new Size(150, 40),
                Location = new Point(1000, 630),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.Click += BtnVolver_Click;

            panelPrincipal.Controls.Add(lblTitulo);
            panelPrincipal.Controls.Add(lblBuscar);
            panelPrincipal.Controls.Add(txtBuscar);
            panelPrincipal.Controls.Add(flowPanelCursos);
            panelPrincipal.Controls.Add(btnVolver);
            
            this.Controls.Add(panelPrincipal);
            this.ResumeLayout(false);
        }

        private List<CursoDto>? _todosCursos;

        private async System.Threading.Tasks.Task CargarCursosDisponiblesAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                // Obtener todos los cursos
                var cursosEnumerable = await _cursoApiClient.GetAllAsync();
                _todosCursos = cursosEnumerable.ToList();
                
                // Obtener inscripciones del alumno
                var misInscripciones = await _inscripcionApiClient.GetByAlumnoIdAsync(_personaId);
                var cursosInscriptos = misInscripciones?.Select(i => i.IdCurso).ToList() ?? new List<int>();

                // Filtrar cursos disponibles
                var cursosDisponibles = _todosCursos?
                    .Where(c => !cursosInscriptos.Contains(c.IdCurso))
                    .ToList() ?? new List<CursoDto>();

                MostrarCursos(cursosDisponibles);
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

        private void MostrarCursos(List<CursoDto> cursos)
        {
            flowPanelCursos.Controls.Clear();

            if (!cursos.Any())
            {
                var lblSinCursos = new Label
                {
                    Text = "No hay cursos disponibles para inscribirse.",
                    Font = new Font("Segoe UI", 14),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(20, 20)
                };
                flowPanelCursos.Controls.Add(lblSinCursos);
                return;
            }

            foreach (var curso in cursos)
            {
                var card = CrearCardCurso(curso);
                flowPanelCursos.Controls.Add(card);
            }
        }

        private Panel CrearCardCurso(CursoDto curso)
        {
            var card = new Panel
            {
                Size = new Size(340, 160),
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            // Determinar color segun cupo
            int inscriptos = curso.InscriptosCount;
            int cupo = curso.Cupo;
            int disponibles = cupo - inscriptos;

            Color colorFondo;
            if (disponibles > 5)
                colorFondo = Color.FromArgb(200, 247, 197); // Verde
            else if (disponibles > 0)
                colorFondo = Color.FromArgb(255, 250, 205); // Amarillo
            else
                colorFondo = Color.FromArgb(255, 224, 224); // Rojo

            card.BackColor = colorFondo;

            // Titulo del curso
            var lblNombre = new Label
            {
                Text = curso.Nombre,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(300, 25),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            // Informacion del curso
            var lblInfo = new Label
            {
                Text = $"Comision: {curso.Comision}\n" +
                       $"Anio: {curso.AnioCalendario}\n" +
                       $"Cupo: {inscriptos}/{cupo} - Disponibles: {disponibles}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(10, 45),
                Size = new Size(300, 70),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            // Boton inscribirse
            var btnInscribir = new Button
            {
                Text = disponibles > 0 ? "Inscribirse" : "Sin cupo",
                Size = new Size(150, 30),
                Location = new Point(10, 120),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = disponibles > 0 ? Color.FromArgb(46, 204, 113) : Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = disponibles > 0
            };
            btnInscribir.FlatAppearance.BorderSize = 0;
            btnInscribir.Click += async (s, e) => await InscribirseACurso(curso);

            card.Controls.Add(lblNombre);
            card.Controls.Add(lblInfo);
            card.Controls.Add(btnInscribir);

            return card;
        }

        private async System.Threading.Tasks.Task InscribirseACurso(CursoDto curso)
        {
            try
            {
                var result = MessageBox.Show(
                    $"Confirma que desea inscribirse al curso:\n\n" +
                    $"Curso: {curso.Nombre}\n" +
                    $"Comision: {curso.Comision}\n" +
                    $"Anio: {curso.AnioCalendario}",
                    "Confirmar Inscripcion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    var inscripcionDto = new AlumnoCursoDto
                    {
                        IdAlumno = _personaId,
                        IdCurso = curso.IdCurso,
                        Condicion = CondicionAlumnoDto.Regular // Por defecto
                    };

                    await _inscripcionApiClient.CreateAsync(inscripcionDto);

                    MessageBox.Show("Inscripcion exitosa!", 
                        "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await CargarCursosDisponiblesAsync();
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                if (mensaje.Contains("already enrolled", StringComparison.OrdinalIgnoreCase))
                    mensaje = "Ya estas inscripto en este curso.";
                else if (mensaje.Contains("no capacity", StringComparison.OrdinalIgnoreCase))
                    mensaje = "No hay cupo disponible en este curso.";

                MessageBox.Show($"Error al inscribirse: {mensaje}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void FiltrarCursos()
        {
            if (_todosCursos == null) return;

            var busqueda = txtBuscar.Text.ToLower();
            var cursosFiltrados = _todosCursos
                .Where(c => c.Nombre.ToLower().Contains(busqueda) ||
                           c.Comision.ToLower().Contains(busqueda))
                .ToList();

            MostrarCursos(cursosFiltrados);
        }

        private void BtnVolver_Click(object? sender, EventArgs e)
        {
            _menuAnterior.Show();
            this.Close();
        }
    }
}
