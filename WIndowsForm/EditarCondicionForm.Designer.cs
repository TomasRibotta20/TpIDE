namespace WIndowsForm
{
    partial class EditarCondicionForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblInfo;
        private Label lblCondicion;
        private ComboBox cmbCondicion;
        private CheckBox chkTieneNota;
        private NumericUpDown numNota;
        private Button btnGuardar;
        private Button btnCancelar;
        private TableLayoutPanel tableLayoutPanel1;

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
            tableLayoutPanel1 = new TableLayoutPanel();
            lblInfo = new Label();
            lblCondicion = new Label();
            cmbCondicion = new ComboBox();
            chkTieneNota = new CheckBox();
            numNota = new NumericUpDown();
            btnGuardar = new Button();
            btnCancelar = new Button();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numNota).BeginInit();
            SuspendLayout();
            // 
            // lblInfo
            // 
            lblInfo.BackColor = Color.FromArgb(240, 248, 255);
            lblInfo.BorderStyle = BorderStyle.FixedSingle;
            lblInfo.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblInfo.Location = new Point(12, 12);
            lblInfo.Name = "lblInfo";
            lblInfo.Padding = new Padding(10);
            lblInfo.Size = new Size(360, 60);
            lblInfo.TabIndex = 0;
            lblInfo.Text = "Información del alumno y curso";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.Controls.Add(lblCondicion, 0, 0);
            tableLayoutPanel1.Controls.Add(cmbCondicion, 1, 0);
            tableLayoutPanel1.Controls.Add(chkTieneNota, 0, 1);
            tableLayoutPanel1.Controls.Add(numNota, 1, 1);
            tableLayoutPanel1.Location = new Point(12, 85);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.Size = new Size(360, 70);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // lblCondicion
            // 
            lblCondicion.Anchor = AnchorStyles.Left;
            lblCondicion.AutoSize = true;
            lblCondicion.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblCondicion.Location = new Point(3, 9);
            lblCondicion.Name = "lblCondicion";
            lblCondicion.Size = new Size(76, 16);
            lblCondicion.TabIndex = 0;
            lblCondicion.Text = "Condición*:";
            // 
            // cmbCondicion
            // 
            cmbCondicion.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbCondicion.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCondicion.Font = new Font("Arial", 10F);
            cmbCondicion.FormattingEnabled = true;
            cmbCondicion.Location = new Point(147, 6);
            cmbCondicion.Name = "cmbCondicion";
            cmbCondicion.Size = new Size(210, 24);
            cmbCondicion.TabIndex = 1;
            // 
            // chkTieneNota
            // 
            chkTieneNota.Anchor = AnchorStyles.Left;
            chkTieneNota.AutoSize = true;
            chkTieneNota.Font = new Font("Arial", 10F, FontStyle.Bold);
            chkTieneNota.Location = new Point(3, 43);
            chkTieneNota.Name = "chkTieneNota";
            chkTieneNota.Size = new Size(98, 20);
            chkTieneNota.TabIndex = 2;
            chkTieneNota.Text = "Tiene Nota:";
            chkTieneNota.UseVisualStyleBackColor = true;
            // 
            // numNota
            // 
            numNota.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numNota.Font = new Font("Arial", 10F);
            numNota.Location = new Point(147, 41);
            numNota.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numNota.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numNota.Name = "numNota";
            numNota.Size = new Size(210, 23);
            numNota.TabIndex = 3;
            numNota.Value = new decimal(new int[] { 7, 0, 0, 0 });
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.LightGreen;
            btnGuardar.Font = new Font("Arial", 10F, FontStyle.Bold);
            btnGuardar.Location = new Point(216, 170);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(75, 30);
            btnGuardar.TabIndex = 2;
            btnGuardar.Text = "? Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.LightCoral;
            btnCancelar.Font = new Font("Arial", 10F, FontStyle.Bold);
            btnCancelar.Location = new Point(297, 170);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(75, 30);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "? Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // EditarCondicionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(384, 212);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(lblInfo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditarCondicionForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "?? Editar Condición y Nota";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numNota).EndInit();
            ResumeLayout(false);
        }
    }
}