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
        private System.Windows.Forms.Panel panelBotones;
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
            panelBotones = new Panel();
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
            tableLayoutPanel1.Padding = new Padding(20);
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.Size = new Size(434, 230);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lblId
            // 
            lblId.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblId.AutoSize = true;
            lblId.Location = new Point(23, 32);
            lblId.Name = "lblId";
            lblId.Size = new Size(112, 15);
            lblId.TabIndex = 0;
            lblId.Text = "ID:";
            lblId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtId
            // 
            txtId.Dock = DockStyle.Fill;
            txtId.Location = new Point(141, 23);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(270, 23);
            txtId.TabIndex = 1;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(23, 72);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(112, 15);
            lblDescripcion.TabIndex = 2;
            lblDescripcion.Text = "Descripción*:";
            lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDescripcion
            // 
            txtDescripcion.Dock = DockStyle.Fill;
            txtDescripcion.Location = new Point(141, 63);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(270, 23);
            txtDescripcion.TabIndex = 3;
            // 
            // lblEspecialidad
            // 
            lblEspecialidad.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblEspecialidad.AutoSize = true;
            lblEspecialidad.Location = new Point(23, 112);
            lblEspecialidad.Name = "lblEspecialidad";
            lblEspecialidad.Size = new Size(112, 15);
            lblEspecialidad.TabIndex = 4;
            lblEspecialidad.Text = "Especialidad*:";
            lblEspecialidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboEspecialidades
            // 
            comboEspecialidades.Dock = DockStyle.Fill;
            comboEspecialidades.FormattingEnabled = true;
            comboEspecialidades.Location = new Point(141, 103);
            comboEspecialidades.Name = "comboEspecialidades";
            comboEspecialidades.Size = new Size(270, 23);
            comboEspecialidades.TabIndex = 5;
            // 
            // panelBotones
            // 
            tableLayoutPanel1.SetColumnSpan(panelBotones, 2);
            panelBotones.Controls.Add(btnCancelar);
            panelBotones.Controls.Add(btnGuardar);
            panelBotones.Dock = DockStyle.Fill;
            panelBotones.Location = new Point(23, 143);
            panelBotones.Name = "panelBotones";
            panelBotones.Size = new Size(388, 64);
            panelBotones.TabIndex = 6;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(160, 5);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 30);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(50, 5);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(100, 30);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            // 
            // EditarPlanForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(434, 230);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
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