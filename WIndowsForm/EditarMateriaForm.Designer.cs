namespace WIndowsForm
{
    partial class EditarMateriaForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            lblId = new Label();
            txtId = new TextBox();
            lblDescripcion = new Label();
            txtDescripcion = new TextBox();
            lblHorasSemanales = new Label();
            numHorasSemanales = new NumericUpDown();
            lblHorasTotales = new Label();
            numHorasTotales = new NumericUpDown();
            lblPlan = new Label();
            cmbPlan = new ComboBox();
            buttonPanel = new Panel();
            btnCancelar = new Button();
            btnGuardar = new Button();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numHorasSemanales).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numHorasTotales).BeginInit();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tableLayoutPanel1.Controls.Add(lblId, 0, 0);
            tableLayoutPanel1.Controls.Add(txtId, 1, 0);
            tableLayoutPanel1.Controls.Add(lblDescripcion, 0, 1);
            tableLayoutPanel1.Controls.Add(txtDescripcion, 1, 1);
            tableLayoutPanel1.Controls.Add(lblHorasSemanales, 0, 2);
            tableLayoutPanel1.Controls.Add(numHorasSemanales, 1, 2);
            tableLayoutPanel1.Controls.Add(lblHorasTotales, 0, 3);
            tableLayoutPanel1.Controls.Add(numHorasTotales, 1, 3);
            tableLayoutPanel1.Controls.Add(lblPlan, 0, 4);
            tableLayoutPanel1.Controls.Add(cmbPlan, 1, 4);
            tableLayoutPanel1.Controls.Add(buttonPanel, 0, 5);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(10);
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.Size = new Size(534, 331);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lblId
            // 
            lblId.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblId.AutoSize = true;
            lblId.Location = new Point(13, 27);
            lblId.Name = "lblId";
            lblId.Size = new Size(173, 15);
            lblId.TabIndex = 0;
            lblId.Text = "ID:";
            lblId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtId
            // 
            txtId.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtId.Location = new Point(192, 23);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(329, 23);
            txtId.TabIndex = 1;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(13, 77);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(173, 15);
            lblDescripcion.TabIndex = 2;
            lblDescripcion.Text = "Descripción*:";
            lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDescripcion
            // 
            txtDescripcion.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtDescripcion.Location = new Point(192, 73);
            txtDescripcion.MaxLength = 100;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(329, 23);
            txtDescripcion.TabIndex = 3;
            // 
            // lblHorasSemanales
            // 
            lblHorasSemanales.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblHorasSemanales.AutoSize = true;
            lblHorasSemanales.Location = new Point(13, 127);
            lblHorasSemanales.Name = "lblHorasSemanales";
            lblHorasSemanales.Size = new Size(173, 15);
            lblHorasSemanales.TabIndex = 4;
            lblHorasSemanales.Text = "Horas Semanales*:";
            lblHorasSemanales.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numHorasSemanales
            // 
            numHorasSemanales.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numHorasSemanales.Location = new Point(192, 123);
            numHorasSemanales.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numHorasSemanales.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numHorasSemanales.Name = "numHorasSemanales";
            numHorasSemanales.Size = new Size(329, 23);
            numHorasSemanales.TabIndex = 5;
            numHorasSemanales.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // lblHorasTotales
            // 
            lblHorasTotales.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblHorasTotales.AutoSize = true;
            lblHorasTotales.Location = new Point(13, 177);
            lblHorasTotales.Name = "lblHorasTotales";
            lblHorasTotales.Size = new Size(173, 15);
            lblHorasTotales.TabIndex = 6;
            lblHorasTotales.Text = "Horas Totales*:";
            lblHorasTotales.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numHorasTotales
            // 
            numHorasTotales.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numHorasTotales.Location = new Point(192, 173);
            numHorasTotales.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            numHorasTotales.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numHorasTotales.Name = "numHorasTotales";
            numHorasTotales.Size = new Size(329, 23);
            numHorasTotales.TabIndex = 7;
            numHorasTotales.Value = new decimal(new int[] { 64, 0, 0, 0 });
            // 
            // lblPlan
            // 
            lblPlan.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblPlan.AutoSize = true;
            lblPlan.Location = new Point(13, 227);
            lblPlan.Name = "lblPlan";
            lblPlan.Size = new Size(173, 15);
            lblPlan.TabIndex = 8;
            lblPlan.Text = "Plan de Estudios*:";
            lblPlan.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbPlan
            // 
            cmbPlan.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbPlan.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlan.FormattingEnabled = true;
            cmbPlan.Location = new Point(192, 223);
            cmbPlan.Name = "cmbPlan";
            cmbPlan.Size = new Size(329, 23);
            cmbPlan.TabIndex = 9;
            // 
            // buttonPanel
            // 
            tableLayoutPanel1.SetColumnSpan(buttonPanel, 2);
            buttonPanel.Controls.Add(btnCancelar);
            buttonPanel.Controls.Add(btnGuardar);
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.Location = new Point(13, 263);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(508, 55);
            buttonPanel.TabIndex = 10;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(220, 12);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 30);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(110, 12);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(100, 30);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            // 
            // EditarMateriaForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 331);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditarMateriaForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nueva Materia";
            Load += EditarMateriaForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numHorasSemanales).EndInit();
            ((System.ComponentModel.ISupportInitialize)numHorasTotales).EndInit();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label lblId;
        private TextBox txtId;
        private Label lblDescripcion;
        private TextBox txtDescripcion;
        private Label lblHorasSemanales;
        private NumericUpDown numHorasSemanales;
        private Label lblHorasTotales;
        private NumericUpDown numHorasTotales;
        private Label lblPlan;
        private ComboBox cmbPlan;
        private Panel buttonPanel;
        private Button btnCancelar;
        private Button btnGuardar;
    }
}
