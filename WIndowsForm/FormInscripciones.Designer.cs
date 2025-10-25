namespace WIndowsForm
{
    partial class FormInscripciones
    {
        private System.ComponentModel.IContainer components = null;
        private Panel headerPanel;
        private Label lblTitle;
        private Panel panelIzquierdo;
        private Panel panelCentral;
        private Panel panelDerecho;
        private TextBox txtBuscarAlumno;
        private Label lblBuscar;
        private ListBox listBoxAlumnos;
        private Label lblAlumnos;
        private Panel panelCursos;
        private Label lblCursos;
        private Label lblAlumnoSeleccionado;
        private DataGridView dataGridViewInscripciones;
        private Label lblInscripciones;
        private Button btnDesinscribir;
        private Button btnEditarCondicion;
        private Button btnVolver;
        private Button btnReporteCursos;

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
            panelIzquierdo = new Panel();
            panelCentral = new Panel();
            panelDerecho = new Panel();
            txtBuscarAlumno = new TextBox();
            lblBuscar = new Label();
            listBoxAlumnos = new ListBox();
            lblAlumnos = new Label();
            panelCursos = new Panel();
            lblCursos = new Label();
            lblAlumnoSeleccionado = new Label();
            dataGridViewInscripciones = new DataGridView();
            lblInscripciones = new Label();
            btnDesinscribir = new Button();
            btnEditarCondicion = new Button();
            btnVolver = new Button();
            btnReporteCursos = new Button();
            headerPanel.SuspendLayout();
            panelIzquierdo.SuspendLayout();
            panelCentral.SuspendLayout();
            panelDerecho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewInscripciones).BeginInit();
            SuspendLayout();
            
            // headerPanel
            headerPanel.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Size = new Size(1200, 70);
            
            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Location = new Point(20, 15);
            lblTitle.Text = "Gestion de Inscripciones";
            
            // panelIzquierdo
            panelIzquierdo.BackColor = System.Drawing.Color.White;
            panelIzquierdo.Controls.Add(txtBuscarAlumno);
            panelIzquierdo.Controls.Add(lblBuscar);
            panelIzquierdo.Controls.Add(listBoxAlumnos);
            panelIzquierdo.Controls.Add(lblAlumnos);
            panelIzquierdo.Dock = DockStyle.Left;
            panelIzquierdo.Location = new Point(0, 70);
            panelIzquierdo.Padding = new Padding(10);
            panelIzquierdo.Size = new Size(300, 630);
            
            // lblAlumnos
            lblAlumnos.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            lblAlumnos.Dock = DockStyle.Top;
            lblAlumnos.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblAlumnos.ForeColor = System.Drawing.Color.White;
            lblAlumnos.Location = new Point(10, 10);
            lblAlumnos.Size = new Size(280, 35);
            lblAlumnos.Text = "ALUMNOS";
            lblAlumnos.TextAlign = ContentAlignment.MiddleLeft;
            lblAlumnos.Padding = new Padding(10, 0, 0, 0);
            
            // lblBuscar
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblBuscar.Location = new Point(20, 55);
            lblBuscar.Text = "Buscar:";
            
            // txtBuscarAlumno
            txtBuscarAlumno.Font = new Font("Segoe UI", 10F);
            txtBuscarAlumno.Location = new Point(20, 75);
            txtBuscarAlumno.Size = new Size(260, 25);
            txtBuscarAlumno.PlaceholderText = "Nombre o legajo...";
            
            // listBoxAlumnos
            listBoxAlumnos.Font = new Font("Segoe UI", 10F);
            listBoxAlumnos.Location = new Point(20, 110);
            listBoxAlumnos.Size = new Size(260, 500);
            
            // panelCentral
            panelCentral.BackColor = System.Drawing.Color.White;
            panelCentral.Controls.Add(panelCursos);
            panelCentral.Controls.Add(lblCursos);
            panelCentral.Dock = DockStyle.Fill;
            panelCentral.Padding = new Padding(10);
            
            // lblCursos
            lblCursos.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            lblCursos.Dock = DockStyle.Top;
            lblCursos.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblCursos.ForeColor = System.Drawing.Color.White;
            lblCursos.Location = new Point(10, 10);
            lblCursos.Size = new Size(300, 35);
            lblCursos.Text = "CURSOS DISPONIBLES";
            lblCursos.TextAlign = ContentAlignment.MiddleLeft;
            lblCursos.Padding = new Padding(10, 0, 0, 0);
            
            // panelCursos
            panelCursos.AutoScroll = true;
            panelCursos.Dock = DockStyle.Fill;
            panelCursos.Location = new Point(10, 45);
            panelCursos.Padding = new Padding(5);
            
            // panelDerecho
            panelDerecho.BackColor = System.Drawing.Color.White;
            panelDerecho.Controls.Add(btnReporteCursos);
            panelDerecho.Controls.Add(btnVolver);
            panelDerecho.Controls.Add(btnEditarCondicion);
            panelDerecho.Controls.Add(btnDesinscribir);
            panelDerecho.Controls.Add(dataGridViewInscripciones);
            panelDerecho.Controls.Add(lblInscripciones);
            panelDerecho.Controls.Add(lblAlumnoSeleccionado);
            panelDerecho.Dock = DockStyle.Right;
            panelDerecho.Location = new Point(800, 70);
            panelDerecho.Padding = new Padding(10);
            panelDerecho.Size = new Size(400, 630);
            
            // lblAlumnoSeleccionado
            lblAlumnoSeleccionado.BackColor = System.Drawing.Color.FromArgb(241, 196, 15);
            lblAlumnoSeleccionado.Dock = DockStyle.Top;
            lblAlumnoSeleccionado.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblAlumnoSeleccionado.ForeColor = System.Drawing.Color.White;
            lblAlumnoSeleccionado.Location = new Point(10, 10);
            lblAlumnoSeleccionado.Size = new Size(380, 35);
            lblAlumnoSeleccionado.Text = "Seleccione un alumno";
            lblAlumnoSeleccionado.TextAlign = ContentAlignment.MiddleLeft;
            lblAlumnoSeleccionado.Padding = new Padding(10, 0, 0, 0);
            
            // lblInscripciones
            lblInscripciones.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            lblInscripciones.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblInscripciones.ForeColor = System.Drawing.Color.White;
            lblInscripciones.Location = new Point(10, 50);
            lblInscripciones.Size = new Size(380, 30);
            lblInscripciones.Text = "INSCRIPCIONES ACTUALES";
            lblInscripciones.TextAlign = ContentAlignment.MiddleLeft;
            lblInscripciones.Padding = new Padding(10, 0, 0, 0);
            
            // dataGridViewInscripciones
            dataGridViewInscripciones.AllowUserToAddRows = false;
            dataGridViewInscripciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewInscripciones.BackgroundColor = System.Drawing.Color.White;
            dataGridViewInscripciones.BorderStyle = BorderStyle.None;
            dataGridViewInscripciones.ColumnHeadersHeight = 35;
            dataGridViewInscripciones.EnableHeadersVisualStyles = false;
            dataGridViewInscripciones.Location = new Point(10, 85);
            dataGridViewInscripciones.MultiSelect = false;
            dataGridViewInscripciones.ReadOnly = true;
            dataGridViewInscripciones.RowHeadersVisible = false;
            dataGridViewInscripciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewInscripciones.Size = new Size(380, 480);
            
            // btnDesinscribir
            btnDesinscribir.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            btnDesinscribir.FlatStyle = FlatStyle.Flat;
            btnDesinscribir.FlatAppearance.BorderSize = 0;
            btnDesinscribir.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDesinscribir.ForeColor = System.Drawing.Color.White;
            btnDesinscribir.Location = new Point(10, 575);
            btnDesinscribir.Size = new Size(85, 35);
            btnDesinscribir.Text = "Eliminar";
            btnDesinscribir.Cursor = Cursors.Hand;
            
            // btnEditarCondicion
            btnEditarCondicion.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            btnEditarCondicion.FlatStyle = FlatStyle.Flat;
            btnEditarCondicion.FlatAppearance.BorderSize = 0;
            btnEditarCondicion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEditarCondicion.ForeColor = System.Drawing.Color.White;
            btnEditarCondicion.Location = new Point(105, 575);
            btnEditarCondicion.Size = new Size(85, 35);
            btnEditarCondicion.Text = "Editar";
            btnEditarCondicion.Cursor = Cursors.Hand;
            
            // btnReporteCursos
            btnReporteCursos.BackColor = System.Drawing.Color.FromArgb(155, 89, 182);
            btnReporteCursos.FlatStyle = FlatStyle.Flat;
            btnReporteCursos.FlatAppearance.BorderSize = 0;
            btnReporteCursos.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnReporteCursos.ForeColor = System.Drawing.Color.White;
            btnReporteCursos.Location = new Point(200, 575);
            btnReporteCursos.Size = new Size(90, 35);
            btnReporteCursos.Text = "Reportes";
            btnReporteCursos.Cursor = Cursors.Hand;
            
            // btnVolver
            btnVolver.BackColor = System.Drawing.Color.FromArgb(127, 140, 141);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnVolver.ForeColor = System.Drawing.Color.White;
            btnVolver.Location = new Point(300, 575);
            btnVolver.Size = new Size(90, 35);
            btnVolver.Text = "Volver";
            btnVolver.Cursor = Cursors.Hand;
            
            // FormInscripciones
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(236, 240, 245);
            ClientSize = new Size(1200, 700);
            Controls.Add(panelCentral);
            Controls.Add(panelDerecho);
            Controls.Add(panelIzquierdo);
            Controls.Add(headerPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestion de Inscripciones";
            WindowState = FormWindowState.Normal;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            panelIzquierdo.ResumeLayout(false);
            panelIzquierdo.PerformLayout();
            panelCentral.ResumeLayout(false);
            panelDerecho.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewInscripciones).EndInit();
            ResumeLayout(false);
        }
    }
}