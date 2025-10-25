namespace WIndowsForm
{
    partial class FormReporteCursos
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelEstadisticas;
        private Label lblTitulo;
        private Label lblTotalInscripciones;
        private Label lblPromocionales; // Cambiado el orden
        private Label lblRegulares;
        private Label lblLibres;
        private DataGridView dataGridViewCursos;
        private Button btnCerrar;
        private Button btnExportar;
        private Panel panelGraficos;
        private Panel panelGraficoCondiciones;
        private Panel panelGraficoOcupacion;

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
            panelEstadisticas = new Panel();
            lblTitulo = new Label();
            lblTotalInscripciones = new Label();
            lblPromocionales = new Label();
            lblRegulares = new Label();
            lblLibres = new Label();
            dataGridViewCursos = new DataGridView();
            btnCerrar = new Button();
            btnExportar = new Button();
            panelGraficos = new Panel();
            panelGraficoCondiciones = new Panel();
            panelGraficoOcupacion = new Panel();
            panelEstadisticas.SuspendLayout();
            panelGraficos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCursos).BeginInit();
            SuspendLayout();
            // 
            // panelEstadisticas
            // 
            panelEstadisticas.BackColor = Color.FromArgb(240, 248, 255);
            panelEstadisticas.BorderStyle = BorderStyle.FixedSingle;
            panelEstadisticas.Controls.Add(lblLibres);
            panelEstadisticas.Controls.Add(lblRegulares);
            panelEstadisticas.Controls.Add(lblPromocionales);
            panelEstadisticas.Controls.Add(lblTotalInscripciones);
            panelEstadisticas.Controls.Add(lblTitulo);
            panelEstadisticas.Dock = DockStyle.Top;
            panelEstadisticas.Location = new Point(0, 0);
            panelEstadisticas.Name = "panelEstadisticas";
            panelEstadisticas.Size = new Size(1200, 80);
            panelEstadisticas.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.FromArgb(70, 130, 180);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Padding = new Padding(10);
            lblTitulo.Size = new Size(1198, 40);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "REPORTE COMPLETO DE CURSOS E INSCRIPCIONES";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalInscripciones
            // 
            lblTotalInscripciones.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalInscripciones.Location = new Point(20, 50);
            lblTotalInscripciones.Name = "lblTotalInscripciones";
            lblTotalInscripciones.Size = new Size(200, 25);
            lblTotalInscripciones.TabIndex = 1;
            lblTotalInscripciones.Text = "Total: 0";
            // 
            // lblPromocionales (VERDE - Lo mejor)
            // 
            lblPromocionales.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPromocionales.ForeColor = Color.Green;
            lblPromocionales.Location = new Point(250, 50);
            lblPromocionales.Name = "lblPromocionales";
            lblPromocionales.Size = new Size(180, 25);
            lblPromocionales.TabIndex = 2;
            lblPromocionales.Text = "?? Promocionales: 0";
            // 
            // lblRegulares (AZUL - Intermedio)
            // 
            lblRegulares.Font = new Font("Segoe UI", 11F);
            lblRegulares.ForeColor = Color.Blue;
            lblRegulares.Location = new Point(450, 50);
            lblRegulares.Name = "lblRegulares";
            lblRegulares.Size = new Size(150, 25);
            lblRegulares.TabIndex = 3;
            lblRegulares.Text = "?? Regulares: 0";
            // 
            // lblLibres (NARANJA - Necesita mejorar)
            // 
            lblLibres.Font = new Font("Segoe UI", 11F);
            lblLibres.ForeColor = Color.DarkOrange;
            lblLibres.Location = new Point(620, 50);
            lblLibres.Name = "lblLibres";
            lblLibres.Size = new Size(150, 25);
            lblLibres.TabIndex = 4;
            lblLibres.Text = "?? Libres: 0";
            // 
            // panelGraficos
            // 
            panelGraficos.Controls.Add(panelGraficoOcupacion);
            panelGraficos.Controls.Add(panelGraficoCondiciones);
            panelGraficos.Dock = DockStyle.Top;
            panelGraficos.Location = new Point(0, 80);
            panelGraficos.Name = "panelGraficos";
            panelGraficos.Size = new Size(1200, 200);
            panelGraficos.TabIndex = 1;
            // 
            // panelGraficoCondiciones
            // 
            panelGraficoCondiciones.BackColor = Color.White;
            panelGraficoCondiciones.BorderStyle = BorderStyle.FixedSingle;
            panelGraficoCondiciones.Location = new Point(12, 12);
            panelGraficoCondiciones.Name = "panelGraficoCondiciones";
            panelGraficoCondiciones.Size = new Size(580, 176);
            panelGraficoCondiciones.TabIndex = 0;
            panelGraficoCondiciones.Paint += (s, e) => {
                // El gráfico se dibuja en el método DibujarGraficoCondiciones
            };
            // 
            // panelGraficoOcupacion
            // 
            panelGraficoOcupacion.BackColor = Color.White;
            panelGraficoOcupacion.BorderStyle = BorderStyle.FixedSingle;
            panelGraficoOcupacion.Location = new Point(606, 12);
            panelGraficoOcupacion.Name = "panelGraficoOcupacion";
            panelGraficoOcupacion.Size = new Size(580, 176);
            panelGraficoOcupacion.TabIndex = 1;
            panelGraficoOcupacion.Paint += (s, e) => {
                // El gráfico se dibuja en el método DibujarGraficoOcupacion
            };
            // 
            // dataGridViewCursos
            // 
            dataGridViewCursos.AllowUserToAddRows = false;
            dataGridViewCursos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCursos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCursos.Location = new Point(12, 292);
            dataGridViewCursos.Name = "dataGridViewCursos";
            dataGridViewCursos.ReadOnly = true;
            dataGridViewCursos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCursos.Size = new Size(1176, 300);
            dataGridViewCursos.TabIndex = 2;
            // 
            // btnExportar
            // 
            btnExportar.BackColor = Color.LightGreen;
            btnExportar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExportar.Location = new Point(950, 600);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(120, 35);
            btnExportar.TabIndex = 3;
            btnExportar.Text = "Exportar PDF";
            btnExportar.UseVisualStyleBackColor = false;
            btnExportar.Click += (s, e) => MessageBox.Show("Funcionalidad de exportar PDF disponible en versión futura", "Información");
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.LightGray;
            btnCerrar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCerrar.Location = new Point(1080, 600);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 35);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += (s, e) => this.Close();
            // 
            // FormReporteCursos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 650);
            Controls.Add(btnCerrar);
            Controls.Add(btnExportar);
            Controls.Add(dataGridViewCursos);
            Controls.Add(panelGraficos);
            Controls.Add(panelEstadisticas);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FormReporteCursos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reporte Completo de Cursos - Sistema Académico";
            panelEstadisticas.ResumeLayout(false);
            panelGraficos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewCursos).EndInit();
            ResumeLayout(false);
        }
    }
}