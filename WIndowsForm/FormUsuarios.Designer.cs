namespace WIndowsForm
{
    partial class FormUsuarios
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
            dataGridViewUsuarios = new DataGridView();
            buttonPanel = new Panel();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            btnVolver = new Button();
            headerPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).BeginInit();
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
            lblTitle.Text = "Gestion de Usuarios";
            
            // contentPanel
            contentPanel.BackColor = System.Drawing.Color.White;
            contentPanel.Controls.Add(dataGridViewUsuarios);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Padding = new Padding(20);
            
            // dataGridViewUsuarios
            dataGridViewUsuarios.AllowUserToAddRows = false;
            dataGridViewUsuarios.AllowUserToDeleteRows = false;
            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsuarios.BackgroundColor = System.Drawing.Color.White;
            dataGridViewUsuarios.BorderStyle = BorderStyle.None;
            dataGridViewUsuarios.ColumnHeadersHeight = 40;
            dataGridViewUsuarios.Dock = DockStyle.Fill;
            dataGridViewUsuarios.EnableHeadersVisualStyles = false;
            dataGridViewUsuarios.MultiSelect = false;
            dataGridViewUsuarios.ReadOnly = true;
            dataGridViewUsuarios.RowHeadersVisible = false;
            dataGridViewUsuarios.RowTemplate.Height = 35;
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // buttonPanel
            buttonPanel.BackColor = System.Drawing.Color.FromArgb(236, 240, 245);
            buttonPanel.Controls.Add(btnVolver);
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
            btnNuevo.Size = new Size(150, 40);
            btnNuevo.Text = "Nuevo Usuario";
            btnNuevo.Cursor = Cursors.Hand;
            
            // btnEditar
            btnEditar.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnEditar.ForeColor = System.Drawing.Color.White;
            btnEditar.Location = new Point(190, 15);
            btnEditar.Size = new Size(130, 40);
            btnEditar.Text = "Editar";
            btnEditar.Cursor = Cursors.Hand;
            
            // btnEliminar
            btnEliminar.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnEliminar.ForeColor = System.Drawing.Color.White;
            btnEliminar.Location = new Point(340, 15);
            btnEliminar.Size = new Size(130, 40);
            btnEliminar.Text = "Eliminar";
            btnEliminar.Cursor = Cursors.Hand;
            
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
            
            // FormUsuarios
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
            Text = "Gestion de Usuarios";
            WindowState = FormWindowState.Normal;
            Load += this.FormUsuarios_Load;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            contentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).EndInit();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel headerPanel;
        private Label lblTitle;
        private Panel contentPanel;
        private Panel buttonPanel;
        private DataGridView dataGridViewUsuarios;
        private Button btnVolver;
        private Button btnEliminar;
        private Button btnEditar;
        private Button btnNuevo;
    }
}