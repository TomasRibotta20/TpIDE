using System;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
            ConfigureForm();
        }
        // Add the missing event handler method to resolve CS0103
        private void MenuPrincipal_Load(object sender, EventArgs e)
        {
            // Add any initialization code here if needed
        }
        private void ConfigureForm()
        {
            this.Text = "Sistema de Gestión";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 450); // Tamaño aumentado

            // Panel contenedor
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Título
            Label titleLabel = new Label
            {
                Text = "Sistema de Gestión",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 60
            };

            // Botones para los diferentes CRUDs
            Button btnUsuarios = new Button
            {
                Text = "Gestión de Usuarios",
                Size = new Size(200, 50),
                Location = new Point(150, 100),
                Font = new Font("Arial", 12)
            };

            Button btnEspecialidades = new Button
            {
                Text = "Gestión de Especialidades",
                Size = new Size(200, 50),
                Location = new Point(150, 170),
                Font = new Font("Arial", 12)
            };

            Button btnOtroCrud = new Button
            {
                Text = "Otro CRUD (No implementado)",
                Size = new Size(200, 50),
                Location = new Point(150, 240),
                Font = new Font("Arial", 12),
                Enabled = false
            };

            // Etiqueta para próximos CRUDs
            Label lblProximamente = new Label
            {
                Text = "Más CRUDs próximamente...",
                Font = new Font("Arial", 10, FontStyle.Italic),
                Location = new Point(150, 310),
                Size = new Size(200, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnUsuarios.Click += (s, e) => {
                var formUsuarios = new Form1(this);
                formUsuarios.Show();
                this.Hide();
            };

            btnEspecialidades.Click += (s, e) => {
                var formEspecialidades = new FormEspecialidades(this);
                formEspecialidades.Show();
                this.Hide();
            };

            // Agregar controles
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(btnUsuarios);
            mainPanel.Controls.Add(btnEspecialidades);
            mainPanel.Controls.Add(btnOtroCrud);
            mainPanel.Controls.Add(lblProximamente);
            this.Controls.Add(mainPanel);
        }
    }
}