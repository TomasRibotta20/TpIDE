using API.Auth.WindowsForms;
using API.Clients;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class MenuAlumno : Form
    {
        private readonly int _personaId;
        private readonly string _usuarioNombre;
        private Panel headerPanel;
        private Panel mainPanel;
        private Label lblTitulo;
        private Label lblBienvenida;

        public MenuAlumno()
        {
            InitializeComponent();
            
            _personaId = WindowsFormsAuthService.GetCurrentPersonaId() ?? 0;
            _usuarioNombre = WindowsFormsAuthService.GetCurrentUserId().ToString() ?? "Usuario";
            
            this.Text = "Sistema Academico - Alumno";
            this.WindowState = FormWindowState.Normal;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Configuracion del Form
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 650);
            this.Name = "MenuAlumno";
            this.Text = "Sistema Academico - Portal del Alumno";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.WindowState = FormWindowState.Normal;
            this.BackColor = Color.FromArgb(236, 240, 245);
            this.Load += new EventHandler(this.MenuAlumno_Load);
            
            // Panel principal
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(236, 240, 245)
            };

            // Panel de encabezado
            headerPanel = new Panel
            {
                BackColor = Color.FromArgb(41, 128, 185),
                Dock = DockStyle.Top,
                Height = 100
            };

            // Titulo principal
            lblTitulo = new Label
            {
                Text = "Portal del Alumno",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(30, 20)
            };

            // Mensaje de bienvenida
            lblBienvenida = new Label
            {
                Text = $"Bienvenido, {_usuarioNombre}",
                Font = new Font("Segoe UI", 13),
                ForeColor = Color.FromArgb(236, 240, 245),
                AutoSize = true,
                Location = new Point(35, 65)
            };

            headerPanel.Controls.Add(lblTitulo);
            headerPanel.Controls.Add(lblBienvenida);

            // Botones principales estilo cards
            var btnMisCursos = CrearBotonCard(
                "Mis Cursos",
                "Ver cursos en los que estoy inscripto",
                Color.FromArgb(52, 152, 219),
                new Point(120, 170),
                BtnMisCursos_Click
            );

            var btnInscribirse = CrearBotonCard(
                "Inscribirse",
                "Inscribirse a nuevos cursos",
                Color.FromArgb(46, 204, 113),
                new Point(480, 170),
                BtnInscribirse_Click
            );

            // Boton de cerrar sesion
            var btnCerrarSesion = new Button
            {
                Text = "Cerrar Sesion",
                Size = new Size(200, 45),
                Location = new Point(350, 480),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.Click += BtnCerrarSesion_Click;

            mainPanel.Controls.Add(btnMisCursos);
            mainPanel.Controls.Add(btnInscribirse);
            mainPanel.Controls.Add(btnCerrarSesion);
            
            this.Controls.Add(mainPanel);
            this.Controls.Add(headerPanel);
            this.ResumeLayout(false);
        }

        private Panel CrearBotonCard(string titulo, string descripcion, Color colorFondo, Point ubicacion, EventHandler clickHandler)
        {
            var card = new Panel
            {
                Size = new Size(280, 180),
                Location = ubicacion,
                BackColor = colorFondo,
                Cursor = Cursors.Hand
            };

            var lblTituloCard = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 50),
                AutoSize = false,
                Size = new Size(240, 35),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblDescripcion = new Label
            {
                Text = descripcion,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(236, 240, 245),
                Location = new Point(20, 95),
                AutoSize = false,
                Size = new Size(240, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(lblTituloCard);
            card.Controls.Add(lblDescripcion);
            
            card.MouseEnter += (s, e) =>
            {
                card.BackColor = ControlPaint.Light(colorFondo, 0.2f);
            };
            card.MouseLeave += (s, e) =>
            {
                card.BackColor = colorFondo;
            };
            
            card.Click += clickHandler;
            lblTituloCard.Click += clickHandler;
            lblDescripcion.Click += clickHandler;

            return card;
        }

        private void MenuAlumno_Load(object sender, EventArgs e)
        {
            // Verificar que el alumno tenga PersonaId
            if (_personaId == 0)
            {
                MessageBox.Show("Error: No se pudo obtener la informacion del alumno.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void BtnMisCursos_Click(object? sender, EventArgs e)
        {
            try
            {
                var formMisCursos = new FormMisCursosAlumno(_personaId);
                formMisCursos.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir Mis Cursos: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnInscribirse_Click(object? sender, EventArgs e)
        {
            try
            {
                var formInscribirse = new FormInscripcionAlumno(_personaId, this);
                formInscribirse.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir inscripcion: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnCerrarSesion_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Esta seguro que desea cerrar sesion?", 
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                var authService = AuthServiceProvider.Instance;
                await authService.LogoutAsync();
                Application.Restart();
            }
        }
    }
}
