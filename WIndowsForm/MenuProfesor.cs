using API.Auth.WindowsForms;
using API.Clients;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class MenuProfesor : Form
    {
        private readonly int _personaId;
        private readonly string _usuarioNombre;
        private Panel headerPanel;
        private Panel mainPanel;
        private Label lblTitulo;
        private Label lblBienvenida;

        public MenuProfesor()
        {
            InitializeComponent();
            
            // Obtener datos del usuario logeado
            _personaId = WindowsFormsAuthService.GetCurrentPersonaId() ?? 0;
            _usuarioNombre = WindowsFormsAuthService.GetCurrentUserId().ToString() ?? "Usuario";
            
            this.Text = $"Sistema Académico - Profesor";
            this.WindowState = FormWindowState.Maximized;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Configuración del Form
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 700);
            this.Name = "MenuProfesor";
            this.Text = "Sistema Académico - Portal del Profesor";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += new EventHandler(this.MenuProfesor_Load);
            
            // Panel principal
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(236, 240, 245)
            };

            // Panel de encabezado
            headerPanel = new Panel
            {
                BackColor = Color.FromArgb(155, 89, 182),
                Location = new Point(40, 40),
                Size = new Size(1120, 120),
                Name = "headerPanel"
            };

            // Título principal
            lblTitulo = new Label
            {
                Text = "????? Portal del Profesor",
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(40, 30)
            };

            // Mensaje de bienvenida
            lblBienvenida = new Label
            {
                Text = $"Bienvenido, {_usuarioNombre}",
                Font = new Font("Segoe UI", 14),
                ForeColor = Color.FromArgb(236, 240, 245),
                AutoSize = true,
                Location = new Point(45, 80)
            };

            headerPanel.Controls.Add(lblTitulo);
            headerPanel.Controls.Add(lblBienvenida);

            // Botones principales estilo cards
            var btnMisCursos = CrearBotonCard(
                "?? Mis Cursos",
                "Ver cursos que dicto",
                Color.FromArgb(52, 152, 219),
                new Point(150, 220),
                BtnMisCursos_Click
            );

            var btnCargarNotas = CrearBotonCard(
                "?? Cargar Notas",
                "Cargar notas y condiciones de alumnos",
                Color.FromArgb(46, 204, 113),
                new Point(550, 220),
                BtnCargarNotas_Click
            );

            // Botón de cerrar sesión
            var btnCerrarSesion = new Button
            {
                Text = "?? Cerrar Sesión",
                Size = new Size(220, 55),
                Location = new Point(490, 500),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.Click += BtnCerrarSesion_Click;

            mainPanel.Controls.Add(headerPanel);
            mainPanel.Controls.Add(btnMisCursos);
            mainPanel.Controls.Add(btnCargarNotas);
            mainPanel.Controls.Add(btnCerrarSesion);
            
            this.Controls.Add(mainPanel);
            this.ResumeLayout(false);
        }

        private Panel CrearBotonCard(string titulo, string descripcion, Color colorFondo, Point ubicacion, EventHandler clickHandler)
        {
            var card = new Panel
            {
                Size = new Size(320, 200),
                Location = ubicacion,
                BackColor = colorFondo,
                Cursor = Cursors.Hand
            };

            var lblTituloCard = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 60),
                AutoSize = false,
                Size = new Size(280, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblDescripcion = new Label
            {
                Text = descripcion,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(236, 240, 245),
                Location = new Point(20, 110),
                AutoSize = false,
                Size = new Size(280, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(lblTituloCard);
            card.Controls.Add(lblDescripcion);
            
            // Eventos para hover
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

        private void MenuProfesor_Load(object sender, EventArgs e)
        {
            // Verificar que el profesor tenga PersonaId
            if (_personaId == 0)
            {
                MessageBox.Show("Error: No se pudo obtener la información del profesor.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void BtnMisCursos_Click(object? sender, EventArgs e)
        {
            try
            {
                var formMisCursos = new FormMisCursosProfesor(_personaId);
                formMisCursos.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir Mis Cursos: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCargarNotas_Click(object? sender, EventArgs e)
        {
            try
            {
                var formCargarNotas = new FormCargarNotasProfesor(_personaId, this);
                formCargarNotas.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir carga de notas: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnCerrarSesion_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar sesión?", 
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
