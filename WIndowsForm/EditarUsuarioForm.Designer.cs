namespace WIndowsForm
{
    partial class EditarUsuarioForm
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
            lblNombre = new Label();
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtUsuario = new TextBox();
            txtContrasenia = new TextBox();
            txtEmail = new TextBox();
            lblApellido = new Label();
            lblUsuario = new Label();
            lblContrasenia = new Label();
            lblEmail = new Label();
            chkHabilitado = new CheckBox();
            panel1 = new Panel();
            btnCancelar = new Button();
            btnGuardar = new Button();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.Controls.Add(lblId, 0, 0);
            tableLayoutPanel1.Controls.Add(txtId, 1, 0);
            tableLayoutPanel1.Controls.Add(lblNombre, 0, 1);
            tableLayoutPanel1.Controls.Add(txtNombre, 1, 1);
            tableLayoutPanel1.Controls.Add(txtApellido, 1, 2);
            tableLayoutPanel1.Controls.Add(txtUsuario, 1, 3);
            tableLayoutPanel1.Controls.Add(txtContrasenia, 1, 4);
            tableLayoutPanel1.Controls.Add(txtEmail, 1, 5);
            tableLayoutPanel1.Controls.Add(lblApellido, 0, 2);
            tableLayoutPanel1.Controls.Add(lblUsuario, 0, 3);
            tableLayoutPanel1.Controls.Add(lblContrasenia, 0, 4);
            tableLayoutPanel1.Controls.Add(lblEmail, 0, 5);
            tableLayoutPanel1.Controls.Add(chkHabilitado, 1, 6);
            tableLayoutPanel1.Controls.Add(panel1, 1, 7);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(20);
            tableLayoutPanel1.RowCount = 8;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(434, 411);
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
            // lblNombre
            // 
            lblNombre.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(23, 72);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(112, 15);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre*:";
            // 
            // txtNombre
            // 
            txtNombre.Dock = DockStyle.Fill;
            txtNombre.Location = new Point(141, 63);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(270, 23);
            txtNombre.TabIndex = 3;
            // 
            // txtApellido
            // 
            txtApellido.Dock = DockStyle.Fill;
            txtApellido.Location = new Point(141, 103);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(270, 23);
            txtApellido.TabIndex = 4;
            // 
            // txtUsuario
            // 
            txtUsuario.Dock = DockStyle.Fill;
            txtUsuario.Location = new Point(141, 143);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(270, 23);
            txtUsuario.TabIndex = 5;
            // 
            // txtContrasenia
            // 
            txtContrasenia.Dock = DockStyle.Fill;
            txtContrasenia.Location = new Point(141, 183);
            txtContrasenia.Name = "txtContrasenia";
            txtContrasenia.Size = new Size(270, 23);
            txtContrasenia.TabIndex = 6;
            // 
            // txtEmail
            // 
            txtEmail.Dock = DockStyle.Fill;
            txtEmail.Location = new Point(141, 223);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(270, 23);
            txtEmail.TabIndex = 7;
            // 
            // lblApellido
            // 
            lblApellido.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(23, 112);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(112, 15);
            lblApellido.TabIndex = 8;
            lblApellido.Text = "Apellido*:";
            // 
            // lblUsuario
            // 
            lblUsuario.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(23, 152);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(112, 15);
            lblUsuario.TabIndex = 9;
            lblUsuario.Text = "Usuario*:";
            // 
            // lblContrasenia
            // 
            lblContrasenia.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblContrasenia.AutoSize = true;
            lblContrasenia.Location = new Point(23, 192);
            lblContrasenia.Name = "lblContrasenia";
            lblContrasenia.Size = new Size(112, 15);
            lblContrasenia.TabIndex = 10;
            lblContrasenia.Text = "Contraseña*:";
            // 
            // lblEmail
            // 
            lblEmail.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(23, 232);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(112, 15);
            lblEmail.TabIndex = 11;
            lblEmail.Text = "Email*:";
            // 
            // chkHabilitado
            // 
            chkHabilitado.AutoSize = true;
            chkHabilitado.Dock = DockStyle.Fill;
            chkHabilitado.Location = new Point(141, 263);
            chkHabilitado.Name = "chkHabilitado";
            chkHabilitado.Size = new Size(270, 34);
            chkHabilitado.TabIndex = 12;
            chkHabilitado.Text = "Habilitado";
            chkHabilitado.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnCancelar);
            panel1.Controls.Add(btnGuardar);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(141, 303);
            panel1.Name = "panel1";
            panel1.Size = new Size(270, 85);
            panel1.TabIndex = 13;
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
            // EditarUsuarioForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(434, 411);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditarUsuarioForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Editar Usuario";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label lblId;
        private TextBox txtId;
        private Label lblNombre;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtUsuario;
        private TextBox txtContrasenia;
        private TextBox txtEmail;
        private Label lblApellido;
        private Label lblUsuario;
        private Label lblContrasenia;
        private Label lblEmail;
        private CheckBox chkHabilitado;
        private Panel panel1;
        private Button btnCancelar;
        private Button btnGuardar;
    }
}