namespace WIndowsForm
{
    partial class FormInscripciones
    {
        private System.ComponentModel.IContainer components = null;
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
            panelIzquierdo.SuspendLayout();
            panelCentral.SuspendLayout();
            panelDerecho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewInscripciones).BeginInit();
            SuspendLayout();
            // 
            // panelIzquierdo
            // 
            panelIzquierdo.BackColor = Color.FromArgb(240, 248, 255);
            panelIzquierdo.BorderStyle = BorderStyle.FixedSingle;
            panelIzquierdo.Controls.Add(txtBuscarAlumno);
            panelIzquierdo.Controls.Add(lblBuscar);
            panelIzquierdo.Controls.Add(listBoxAlumnos);
            panelIzquierdo.Controls.Add(lblAlumnos);
            panelIzquierdo.Dock = DockStyle.Left;
            panelIzquierdo.Location = new Point(0, 0);
            panelIzquierdo.Name = "panelIzquierdo";
            panelIzquierdo.Size = new Size(280, 700);
            panelIzquierdo.TabIndex = 0;
            // 
            // lblAlumnos
            // 
            lblAlumnos.BackColor = Color.FromArgb(70, 130, 180);
            lblAlumnos.Dock = DockStyle.Top;
            lblAlumnos.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblAlumnos.ForeColor = Color.White;
            lblAlumnos.Location = new Point(0, 0);
            lblAlumnos.Name = "lblAlumnos";
            lblAlumnos.Padding = new Padding(10, 8, 0, 8);
            lblAlumnos.Size = new Size(278, 35);
            lblAlumnos.TabIndex = 0;
            lblAlumnos.Text = "ALUMNOS";
            lblAlumnos.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBuscar.Location = new Point(10, 45);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(52, 15);
            lblBuscar.TabIndex = 1;
            lblBuscar.Text = "Buscar:";
            // 
            // txtBuscarAlumno
            // 
            txtBuscarAlumno.Font = new Font("Segoe UI", 10F);
            txtBuscarAlumno.Location = new Point(10, 65);
            txtBuscarAlumno.Name = "txtBuscarAlumno";
            txtBuscarAlumno.PlaceholderText = "Nombre o legajo...";
            txtBuscarAlumno.Size = new Size(258, 25);
            txtBuscarAlumno.TabIndex = 2;
            // 
            // listBoxAlumnos
            // 
            listBoxAlumnos.Font = new Font("Segoe UI", 9F);
            listBoxAlumnos.ItemHeight = 15;
            listBoxAlumnos.Location = new Point(10, 95);
            listBoxAlumnos.Name = "listBoxAlumnos";
            listBoxAlumnos.Size = new Size(258, 590);
            listBoxAlumnos.TabIndex = 3;
            // 
            // panelCentral
            // 
            panelCentral.BackColor = Color.FromArgb(245, 255, 250);
            panelCentral.BorderStyle = BorderStyle.FixedSingle;
            panelCentral.Controls.Add(panelCursos);
            panelCentral.Controls.Add(lblCursos);
            panelCentral.Dock = DockStyle.Fill;
            panelCentral.Location = new Point(280, 0);
            panelCentral.Name = "panelCentral";
            panelCentral.Size = new Size(620, 700);
            panelCentral.TabIndex = 1;
            // 
            // lblCursos
            // 
            lblCursos.BackColor = Color.FromArgb(34, 139, 34);
            lblCursos.Dock = DockStyle.Top;
            lblCursos.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblCursos.ForeColor = Color.White;
            lblCursos.Location = new Point(0, 0);
            lblCursos.Name = "lblCursos";
            lblCursos.Padding = new Padding(10, 8, 0, 8);
            lblCursos.Size = new Size(618, 35);
            lblCursos.TabIndex = 0;
            lblCursos.Text = "CURSOS DISPONIBLES - Click para inscribir";
            lblCursos.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelCursos
            // 
            panelCursos.AutoScroll = true;
            panelCursos.Dock = DockStyle.Fill;
            panelCursos.Location = new Point(0, 35);
            panelCursos.Name = "panelCursos";
            panelCursos.Padding = new Padding(10);
            panelCursos.Size = new Size(618, 663);
            panelCursos.TabIndex = 1;
            // 
            // panelDerecho
            // 
            panelDerecho.BackColor = Color.FromArgb(255, 248, 220);
            panelDerecho.BorderStyle = BorderStyle.FixedSingle;
            panelDerecho.Controls.Add(btnReporteCursos);
            panelDerecho.Controls.Add(btnVolver);
            panelDerecho.Controls.Add(btnEditarCondicion);
            panelDerecho.Controls.Add(btnDesinscribir);
            panelDerecho.Controls.Add(dataGridViewInscripciones);
            panelDerecho.Controls.Add(lblInscripciones);
            panelDerecho.Controls.Add(lblAlumnoSeleccionado);
            panelDerecho.Dock = DockStyle.Right;
            panelDerecho.Location = new Point(900, 0);
            panelDerecho.Name = "panelDerecho";
            panelDerecho.Size = new Size(400, 700);
            panelDerecho.TabIndex = 2;
            // 
            // lblAlumnoSeleccionado
            // 
            lblAlumnoSeleccionado.BackColor = Color.FromArgb(255, 165, 0);
            lblAlumnoSeleccionado.Dock = DockStyle.Top;
            lblAlumnoSeleccionado.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblAlumnoSeleccionado.ForeColor = Color.White;
            lblAlumnoSeleccionado.Location = new Point(0, 0);
            lblAlumnoSeleccionado.Name = "lblAlumnoSeleccionado";
            lblAlumnoSeleccionado.Padding = new Padding(5, 8, 5, 8);
            lblAlumnoSeleccionado.Size = new Size(398, 35);
            lblAlumnoSeleccionado.TabIndex = 0;
            lblAlumnoSeleccionado.Text = "Seleccione un alumno";
            lblAlumnoSeleccionado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblInscripciones
            // 
            lblInscripciones.BackColor = Color.FromArgb(178, 34, 34);
            lblInscripciones.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblInscripciones.ForeColor = Color.White;
            lblInscripciones.Location = new Point(0, 45);
            lblInscripciones.Name = "lblInscripciones";
            lblInscripciones.Padding = new Padding(5, 5, 5, 5);
            lblInscripciones.Size = new Size(398, 30);
            lblInscripciones.TabIndex = 1;
            lblInscripciones.Text = "INSCRIPCIONES ACTUALES";
            lblInscripciones.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dataGridViewInscripciones
            // 
            dataGridViewInscripciones.AllowUserToAddRows = false;
            dataGridViewInscripciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewInscripciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewInscripciones.Location = new Point(5, 80);
            dataGridViewInscripciones.MultiSelect = false;
            dataGridViewInscripciones.Name = "dataGridViewInscripciones";
            dataGridViewInscripciones.ReadOnly = true;
            dataGridViewInscripciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewInscripciones.Size = new Size(388, 510);
            dataGridViewInscripciones.TabIndex = 2;
            // 
            // btnDesinscribir
            // 
            btnDesinscribir.BackColor = Color.LightCoral;
            btnDesinscribir.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDesinscribir.Location = new Point(10, 600);
            btnDesinscribir.Name = "btnDesinscribir";
            btnDesinscribir.Size = new Size(90, 30);
            btnDesinscribir.TabIndex = 3;
            btnDesinscribir.Text = "Eliminar";
            btnDesinscribir.UseVisualStyleBackColor = false;
            // 
            // btnEditarCondicion
            // 
            btnEditarCondicion.BackColor = Color.LightBlue;
            btnEditarCondicion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEditarCondicion.Location = new Point(105, 600);
            btnEditarCondicion.Name = "btnEditarCondicion";
            btnEditarCondicion.Size = new Size(90, 30);
            btnEditarCondicion.TabIndex = 4;
            btnEditarCondicion.Text = "Editar";
            btnEditarCondicion.UseVisualStyleBackColor = false;
            // 
            // btnReporteCursos
            // 
            btnReporteCursos.BackColor = Color.LightGreen;
            btnReporteCursos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnReporteCursos.Location = new Point(200, 600);
            btnReporteCursos.Name = "btnReporteCursos";
            btnReporteCursos.Size = new Size(90, 30);
            btnReporteCursos.TabIndex = 5;
            btnReporteCursos.Text = "Reportes";
            btnReporteCursos.UseVisualStyleBackColor = false;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.LightGray;
            btnVolver.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnVolver.Location = new Point(295, 600);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(90, 30);
            btnVolver.TabIndex = 6;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = false;
            // 
            // FormInscripciones
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1300, 700);
            Controls.Add(panelCentral);
            Controls.Add(panelDerecho);
            Controls.Add(panelIzquierdo);
            Name = "FormInscripciones";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Centro de Inscripciones - Sistema Académico";
            WindowState = FormWindowState.Maximized;
            panelIzquierdo.ResumeLayout(false);
            panelIzquierdo.PerformLayout();
            panelCentral.ResumeLayout(false);
            panelDerecho.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewInscripciones).EndInit();
            ResumeLayout(false);
        }
    }
}