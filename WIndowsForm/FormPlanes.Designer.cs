namespace WIndowsForm
{
    partial class FormPlanes
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            headerPanel = new Panel();
            lblTitle = new Label();
            contentPanel = new Panel();
            dataGridViewPlanes = new DataGridView();
            buttonPanel = new Panel();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            btnVerEspecialidad = new Button();
            btnVolver = new Button();
            headerPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlanes).BeginInit();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            
            // headerPanel
            headerPanel.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Size = new Size(1000, 80);
            
            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Location = new Point(20, 20);
            lblTitle.Text = "Gestion de Planes";
            
            // contentPanel
            contentPanel.BackColor = System.Drawing.Color.White;
            contentPanel.Controls.Add(dataGridViewPlanes);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Padding = new Padding(20);
            
            // dataGridViewPlanes
            dataGridViewPlanes.AllowUserToAddRows = false;
            dataGridViewPlanes.AllowUserToDeleteRows = false;
            dataGridViewPlanes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewPlanes.BackgroundColor = System.Drawing.Color.White;
            dataGridViewPlanes.BorderStyle = BorderStyle.None;
            dataGridViewPlanes.ColumnHeadersHeight = 40;
            dataGridViewPlanes.Dock = DockStyle.Fill;
            dataGridViewPlanes.EnableHeadersVisualStyles = false;
            dataGridViewPlanes.MultiSelect = false;
            dataGridViewPlanes.ReadOnly = true;
            dataGridViewPlanes.RowHeadersVisible = false;
            dataGridViewPlanes.RowTemplate.Height = 35;
            dataGridViewPlanes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPlanes.SelectionChanged += DataGridViewPlanes_SelectionChanged;
            
            // buttonPanel
            buttonPanel.BackColor = System.Drawing.Color.FromArgb(236, 240, 245);
            buttonPanel.Controls.Add(btnVolver);
            buttonPanel.Controls.Add(btnVerEspecialidad);
            buttonPanel.Controls.Add(btnEliminar);
            buttonPanel.Controls.Add(btnEditar);
            buttonPanel.Controls.Add(btnNuevo);
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Size = new Size(1000, 70);
            
            // btnNuevo
            btnNuevo.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnNuevo.ForeColor = System.Drawing.Color.White;
            btnNuevo.Location = new Point(20, 15);
            btnNuevo.Size = new Size(130, 40);
            btnNuevo.Text = "Nuevo Plan";
            btnNuevo.Cursor = Cursors.Hand;
            
            // btnEditar
            btnEditar.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnEditar.ForeColor = System.Drawing.Color.White;
            btnEditar.Location = new Point(170, 15);
            btnEditar.Size = new Size(130, 40);
            btnEditar.Text = "Editar";
            btnEditar.Cursor = Cursors.Hand;
            
            // btnEliminar
            btnEliminar.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnEliminar.ForeColor = System.Drawing.Color.White;
            btnEliminar.Location = new Point(320, 15);
            btnEliminar.Size = new Size(130, 40);
            btnEliminar.Text = "Eliminar";
            btnEliminar.Cursor = Cursors.Hand;
            
            // btnVerEspecialidad
            btnVerEspecialidad.BackColor = System.Drawing.Color.FromArgb(155, 89, 182);
            btnVerEspecialidad.FlatStyle = FlatStyle.Flat;
            btnVerEspecialidad.FlatAppearance.BorderSize = 0;
            btnVerEspecialidad.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnVerEspecialidad.ForeColor = System.Drawing.Color.White;
            btnVerEspecialidad.Location = new Point(470, 15);
            btnVerEspecialidad.Size = new Size(170, 40);
            btnVerEspecialidad.Text = "Ver Especialidad";
            btnVerEspecialidad.Cursor = Cursors.Hand;
            btnVerEspecialidad.Enabled = false;
            
            // btnVolver
            btnVolver.BackColor = System.Drawing.Color.FromArgb(127, 140, 141);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnVolver.ForeColor = System.Drawing.Color.White;
            btnVolver.Location = new Point(840, 15);
            btnVolver.Size = new Size(140, 40);
            btnVolver.Text = "Volver al Menu";
            btnVolver.Cursor = Cursors.Hand;
            
            // FormPlanes
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(236, 240, 245);
            ClientSize = new Size(1000, 670);
            Controls.Add(contentPanel);
            Controls.Add(buttonPanel);
            Controls.Add(headerPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestion de Planes";
            WindowState = FormWindowState.Normal;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            contentPanel.ResumeLayout(false);
            buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlanes).EndInit();
            ResumeLayout(false);
        }

        private Panel headerPanel;
        private Label lblTitle;
        private Panel contentPanel;
        private DataGridView dataGridViewPlanes;
        private Panel buttonPanel;
        private Button btnVolver;
        private Button btnEliminar;
        private Button btnEditar;
        private Button btnNuevo;
        private Button btnVerEspecialidad;
    }
}