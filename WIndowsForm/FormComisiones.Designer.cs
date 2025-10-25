namespace WIndowsForm
{
    partial class FormComisiones
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
            dataGridViewComisiones = new DataGridView();
            buttonPanel = new Panel();
            btnNueva = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            btnVerPlan = new Button();
            btnVolver = new Button();
            headerPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewComisiones).BeginInit();
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
            lblTitle.Text = "Gestion de Comisiones";
            
            // contentPanel
            contentPanel.BackColor = System.Drawing.Color.White;
            contentPanel.Controls.Add(dataGridViewComisiones);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Padding = new Padding(20);
            
            // dataGridViewComisiones
            dataGridViewComisiones.AllowUserToAddRows = false;
            dataGridViewComisiones.AllowUserToDeleteRows = false;
            dataGridViewComisiones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewComisiones.BackgroundColor = System.Drawing.Color.White;
            dataGridViewComisiones.BorderStyle = BorderStyle.None;
            dataGridViewComisiones.ColumnHeadersHeight = 40;
            dataGridViewComisiones.Dock = DockStyle.Fill;
            dataGridViewComisiones.EnableHeadersVisualStyles = false;
            dataGridViewComisiones.MultiSelect = false;
            dataGridViewComisiones.ReadOnly = true;
            dataGridViewComisiones.RowHeadersVisible = false;
            dataGridViewComisiones.RowTemplate.Height = 35;
            dataGridViewComisiones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewComisiones.SelectionChanged += DataGridViewComisiones_SelectionChanged;
            
            // buttonPanel
            buttonPanel.BackColor = System.Drawing.Color.FromArgb(236, 240, 245);
            buttonPanel.Controls.Add(btnVolver);
            buttonPanel.Controls.Add(btnVerPlan);
            buttonPanel.Controls.Add(btnEliminar);
            buttonPanel.Controls.Add(btnEditar);
            buttonPanel.Controls.Add(btnNueva);
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Size = new Size(1000, 70);
            
            // btnNueva
            btnNueva.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            btnNueva.FlatStyle = FlatStyle.Flat;
            btnNueva.FlatAppearance.BorderSize = 0;
            btnNueva.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnNueva.ForeColor = System.Drawing.Color.White;
            btnNueva.Location = new Point(20, 15);
            btnNueva.Size = new Size(160, 40);
            btnNueva.Text = "Nueva Comision";
            btnNueva.Cursor = Cursors.Hand;
            
            // btnEditar
            btnEditar.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnEditar.ForeColor = System.Drawing.Color.White;
            btnEditar.Location = new Point(200, 15);
            btnEditar.Size = new Size(130, 40);
            btnEditar.Text = "Editar";
            btnEditar.Cursor = Cursors.Hand;
            
            // btnEliminar
            btnEliminar.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnEliminar.ForeColor = System.Drawing.Color.White;
            btnEliminar.Location = new Point(350, 15);
            btnEliminar.Size = new Size(130, 40);
            btnEliminar.Text = "Eliminar";
            btnEliminar.Cursor = Cursors.Hand;
            
            // btnVerPlan
            btnVerPlan.BackColor = System.Drawing.Color.FromArgb(155, 89, 182);
            btnVerPlan.FlatStyle = FlatStyle.Flat;
            btnVerPlan.FlatAppearance.BorderSize = 0;
            btnVerPlan.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnVerPlan.ForeColor = System.Drawing.Color.White;
            btnVerPlan.Location = new Point(500, 15);
            btnVerPlan.Size = new Size(130, 40);
            btnVerPlan.Text = "Ver Plan";
            btnVerPlan.Cursor = Cursors.Hand;
            btnVerPlan.Enabled = false;
            
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
            
            // FormComisiones
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
            Text = "Gestion de Comisiones";
            WindowState = FormWindowState.Normal;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            contentPanel.ResumeLayout(false);
            buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewComisiones).EndInit();
            ResumeLayout(false);
        }

        private Panel headerPanel;
        private Label lblTitle;
        private Panel contentPanel;
        private DataGridView dataGridViewComisiones;
        private Panel buttonPanel;
        private Button btnVolver;
        private Button btnEliminar;
        private Button btnEditar;
        private Button btnNueva;
        private Button btnVerPlan;
    }
}