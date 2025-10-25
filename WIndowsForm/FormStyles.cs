using System.Drawing;
using System.Windows.Forms;

namespace WIndowsForm
{
    /// <summary>
    /// Clase estatica con los estilos compartidos por todos los formularios
    /// </summary>
    public static class FormStyles
    {
        // Paleta de colores principal
        public static class Colors
        {
            public static readonly Color Primary = Color.FromArgb(41, 128, 185);      // Azul principal
            public static readonly Color Secondary = Color.FromArgb(52, 152, 219);    // Azul claro
            public static readonly Color Success = Color.FromArgb(46, 204, 113);      // Verde
            public static readonly Color Danger = Color.FromArgb(231, 76, 60);        // Rojo
            public static readonly Color Warning = Color.FromArgb(241, 196, 15);      // Amarillo
            public static readonly Color Info = Color.FromArgb(155, 89, 182);         // Purpura
            public static readonly Color Turquoise = Color.FromArgb(26, 188, 156);    // Turquesa
            
            public static readonly Color Background = Color.FromArgb(236, 240, 245);  // Gris claro
            public static readonly Color CardBackground = Color.White;                 // Blanco
            public static readonly Color TextPrimary = Color.FromArgb(44, 62, 80);    // Gris oscuro
            public static readonly Color TextSecondary = Color.FromArgb(127, 140, 141);// Gris medio
            public static readonly Color Border = Color.FromArgb(189, 195, 199);       // Borde gris
        }

        // Fuentes
        public static class Fonts
        {
            public static readonly Font TitleLarge = new Font("Segoe UI", 32F, FontStyle.Bold);
            public static readonly Font TitleMedium = new Font("Segoe UI", 24F, FontStyle.Bold);
            public static readonly Font TitleSmall = new Font("Segoe UI", 18F, FontStyle.Bold);
            public static readonly Font Subtitle = new Font("Segoe UI", 14F);
            public static readonly Font ButtonLarge = new Font("Segoe UI", 13F, FontStyle.Bold);
            public static readonly Font Button = new Font("Segoe UI", 11F, FontStyle.Bold);
            public static readonly Font Normal = new Font("Segoe UI", 10F);
            public static readonly Font Small = new Font("Segoe UI", 9F);
        }

        // Metodos de aplicacion de estilos
        public static void ApplyFormStyle(Form form, string title = "Sistema Academico")
        {
            form.BackColor = Colors.Background;
            form.Font = Fonts.Normal;
            form.Text = title;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.WindowState = FormWindowState.Normal;
        }

        public static Panel CreateHeaderPanel(string title, string subtitle = "")
        {
            var headerPanel = new Panel
            {
                BackColor = Colors.Primary,
                Dock = DockStyle.Top,
                Height = string.IsNullOrEmpty(subtitle) ? 80 : 100,
                Padding = new Padding(20, 10, 20, 10)
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = Fonts.TitleMedium,
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 15)
            };

            headerPanel.Controls.Add(lblTitle);

            if (!string.IsNullOrEmpty(subtitle))
            {
                var lblSubtitle = new Label
                {
                    Text = subtitle,
                    Font = Fonts.Subtitle,
                    ForeColor = Colors.Background,
                    AutoSize = true,
                    Location = new Point(20, 55)
                };
                headerPanel.Controls.Add(lblSubtitle);
            }

            return headerPanel;
        }

        public static Button CreateButton(string text, Color color, EventHandler clickHandler = null)
        {
            var button = new Button
            {
                Text = text,
                Font = Fonts.Button,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 40),
                Cursor = Cursors.Hand
            };
            
            button.FlatAppearance.BorderSize = 0;

            if (clickHandler != null)
            {
                button.Click += clickHandler;
            }

            // Efectos hover
            button.MouseEnter += (s, e) =>
            {
                button.BackColor = ControlPaint.Light(color, 0.2f);
            };
            button.MouseLeave += (s, e) =>
            {
                button.BackColor = color;
            };

            return button;
        }

        public static Button CreatePrimaryButton(string text, EventHandler clickHandler = null)
        {
            return CreateButton(text, Colors.Primary, clickHandler);
        }

        public static Button CreateSuccessButton(string text, EventHandler clickHandler = null)
        {
            return CreateButton(text, Colors.Success, clickHandler);
        }

        public static Button CreateDangerButton(string text, EventHandler clickHandler = null)
        {
            return CreateButton(text, Colors.Danger, clickHandler);
        }

        public static Button CreateSecondaryButton(string text, EventHandler clickHandler = null)
        {
            return CreateButton(text, Colors.Secondary, clickHandler);
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Colors.CardBackground;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.GridColor = Colors.Border;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.MultiSelect = false;

            // Estilo de las cabeceras
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Colors.Primary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = Fonts.Button;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Colors.Primary;
            dgv.ColumnHeadersHeight = 40;

            // Estilo de las celdas
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Colors.TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = Colors.Secondary;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = Fonts.Normal;
            dgv.RowTemplate.Height = 35;

            // Estilo de celdas alternas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
        }

        public static Panel CreateContentPanel()
        {
            return new Panel
            {
                BackColor = Colors.CardBackground,
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };
        }

        public static Panel CreateButtonPanel()
        {
            return new Panel
            {
                BackColor = Colors.Background,
                Dock = DockStyle.Bottom,
                Height = 70,
                Padding = new Padding(20, 15, 20, 15)
            };
        }

        public static void StyleTextBox(TextBox textBox)
        {
            textBox.Font = Fonts.Normal;
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        public static void StyleComboBox(ComboBox comboBox)
        {
            comboBox.Font = Fonts.Normal;
            comboBox.FlatStyle = FlatStyle.Flat;
        }

        public static void StyleLabel(Label label, bool isBold = false)
        {
            label.Font = isBold ? Fonts.Button : Fonts.Normal;
            label.ForeColor = Colors.TextPrimary;
        }
    }
}
