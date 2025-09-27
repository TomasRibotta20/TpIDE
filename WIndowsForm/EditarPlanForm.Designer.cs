namespace WIndowsForm
{
    partial class EditarPlanForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblEspecialidad;
        private System.Windows.Forms.ComboBox comboEspecialidades;
        private System.Windows.Forms.FlowLayoutPanel panelBotones;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            lblId = new Label();
            txtId = new TextBox();
            lblDescripcion = new Label();
            txtDescripcion = new TextBox();
            lblEspecialidad = new Label();
            comboEspecialidades = new ComboBox();
            panelBotones = new FlowLayoutPanel();
            btnCancelar = new Button();
            btnGuardar = new Button();
            tableLayoutPanel1.SuspendLayout();
            panelBotones.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.Controls.Add(lblId, 0, 0);
            tableLayoutPanel1.Controls.Add(txtId, 1, 0);
            tableLayoutPanel1.Controls.Add(lblDescripcion, 0, 1);
            tableLayoutPanel1.Controls.Add(txtDescripcion, 1, 1);
            tableLayoutPanel1.Controls.Add(lblEspecialidad, 0, 2);
            tableLayoutPanel1.Controls.Add(comboEspecialidades, 1, 2);
            tableLayoutPanel1.Controls.Add(panelBotones, 0, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.Size = new Size(460, 190);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lblId
            // 
            lblId.Dock = DockStyle.Fill;
            lblId.Location = new Point(3, 0);
            lblId.Name = "lblId";
            lblId.Size = new Size(132, 35);
            lblId.TabIndex = 0;
            lblId.Text = "Id:";
            lblId.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtId
            // 
            txtId.Dock = DockStyle.Fill;
            txtId.Location = new Point(141, 3);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(316, 23);
            txtId.TabIndex = 1;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.Location = new Point(3, 35);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(132, 45);
            lblDescripcion.TabIndex = 2;
            lblDescripcion.Text = "Descripción:";
            lblDescripcion.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtDescripcion
            // 
            txtDescripcion.Dock = DockStyle.Fill;
            txtDescripcion.Location = new Point(141, 38);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(316, 23);
            txtDescripcion.TabIndex = 3;
            // 
            // lblEspecialidad
            // 
            lblEspecialidad.Dock = DockStyle.Fill;
            lblEspecialidad.Location = new Point(3, 80);
            lblEspecialidad.Name = "lblEspecialidad";
            lblEspecialidad.Size = new Size(132, 45);
            lblEspecialidad.TabIndex = 4;
            lblEspecialidad.Text = "Especialidad:";
            lblEspecialidad.TextAlign = ContentAlignment.MiddleRight;
            // 
            // comboEspecialidades
            // 
            comboEspecialidades.Dock = DockStyle.Fill;
            comboEspecialidades.Location = new Point(141, 83);
            comboEspecialidades.Name = "comboEspecialidades";
            comboEspecialidades.Size = new Size(316, 23);
            comboEspecialidades.TabIndex = 5;
            // 
            // panelBotones
            // 
            tableLayoutPanel1.SetColumnSpan(panelBotones, 2);
            panelBotones.Controls.Add(btnCancelar);
            panelBotones.Controls.Add(btnGuardar);
            panelBotones.Dock = DockStyle.Fill;
            panelBotones.FlowDirection = FlowDirection.RightToLeft;
            panelBotones.Location = new Point(3, 128);
            panelBotones.Name = "panelBotones";
            panelBotones.Size = new Size(454, 59);
            panelBotones.TabIndex = 6;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(376, 3);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(75, 23);
            btnCancelar.TabIndex = 0;
            btnCancelar.Text = "Cancelar";
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(295, 3);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(75, 23);
            btnGuardar.TabIndex = 1;
            btnGuardar.Text = "Guardar";
            // 
            // EditarPlanForm
            // 
            ClientSize = new Size(460, 190);
            Controls.Add(tableLayoutPanel1);
            Name = "EditarPlanForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Plan";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panelBotones.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}