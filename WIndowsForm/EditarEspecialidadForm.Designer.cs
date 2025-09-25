namespace WIndowsForm
{
    partial class EditarEspecialidadForm
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
            txtDescripcion = new TextBox();
            lblDescripcion = new Label();
            buttonPanel = new Panel();
            btnCancelar = new Button();
            btnGuardar = new Button();
            tableLayoutPanel1.SuspendLayout();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.Controls.Add(lblId, 0, 0);
            tableLayoutPanel1.Controls.Add(txtId, 1, 0);
            tableLayoutPanel1.Controls.Add(txtDescripcion, 1, 1);
            tableLayoutPanel1.Controls.Add(lblDescripcion, 0, 1);
            tableLayoutPanel1.Controls.Add(buttonPanel, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(434, 161);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lblId
            // 
            lblId.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblId.AutoSize = true;
            lblId.Location = new Point(3, 17);
            lblId.Name = "lblId";
            lblId.Size = new Size(124, 15);
            lblId.TabIndex = 0;
            lblId.Text = "ID:";
            lblId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtId
            // 
            txtId.Dock = DockStyle.Fill;
            txtId.Location = new Point(133, 3);
            txtId.Multiline = true;
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(298, 44);
            txtId.TabIndex = 1;
            txtId.TextChanged += txtId_TextChanged;
            // 
            // txtDescripcion
            // 
            txtDescripcion.Dock = DockStyle.Fill;
            txtDescripcion.Location = new Point(133, 53);
            txtDescripcion.Multiline = true;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(298, 44);
            txtDescripcion.TabIndex = 2;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(3, 67);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(124, 15);
            lblDescripcion.TabIndex = 3;
            lblDescripcion.Text = "Descripcion*:";
            lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // buttonPanel
            // 
            tableLayoutPanel1.SetColumnSpan(buttonPanel, 2);
            buttonPanel.Controls.Add(btnCancelar);
            buttonPanel.Controls.Add(btnGuardar);
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.Location = new Point(3, 103);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(428, 55);
            buttonPanel.TabIndex = 4;
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
            // EditarEspecialidadForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(434, 161);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditarEspecialidadForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nueva Especialidad";
            Load += EditarEspecialidadForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label lblId;
        private TextBox txtId;
        private TextBox txtDescripcion;
        private Label lblDescripcion;
        private Panel buttonPanel;
        private Button btnCancelar;
        private Button btnGuardar;
    }
}