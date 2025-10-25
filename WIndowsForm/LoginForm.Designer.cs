namespace WindowsForms
{
    partial class LoginForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            usernameLabel = new Label();
            usernameTextBox = new TextBox();
            passwordLabel = new Label();
            passwordTextBox = new TextBox();
            loginButton = new Button();
            cancelButton = new Button();
            titleLabel = new Label();
            errorProvider = new ErrorProvider(components);
            registerLinkLabel = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            
            // usernameLabel
            usernameLabel.AutoSize = true;
            usernameLabel.Location = new Point(27, 42);
            usernameLabel.Margin = new Padding(2, 0, 2, 0);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new Size(50, 15);
            usernameLabel.TabIndex = 1;
            usernameLabel.Text = "Usuario:";
            
            // usernameTextBox
            usernameTextBox.Location = new Point(27, 59);
            usernameTextBox.Margin = new Padding(2, 1, 2, 1);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(142, 23);
            usernameTextBox.TabIndex = 2;
            
            // passwordLabel
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(27, 84);
            passwordLabel.Margin = new Padding(2, 0, 2, 0);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(70, 15);
            passwordLabel.TabIndex = 3;
            passwordLabel.Text = "Contraseña:";
            
            // passwordTextBox
            passwordTextBox.Location = new Point(27, 101);
            passwordTextBox.Margin = new Padding(2, 1, 2, 1);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(142, 23);
            passwordTextBox.TabIndex = 4;
            passwordTextBox.KeyPress += passwordTextBox_KeyPress;
            
            // loginButton
            loginButton.Location = new Point(27, 131);
            loginButton.Margin = new Padding(2, 1, 2, 1);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(65, 21);
            loginButton.TabIndex = 5;
            loginButton.Text = "Iniciar Sesión";
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += loginButton_Click;
            
            // cancelButton
            cancelButton.Location = new Point(102, 131);
            cancelButton.Margin = new Padding(2, 1, 2, 1);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(65, 21);
            cancelButton.TabIndex = 6;
            cancelButton.Text = "Cancelar";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            
            // titleLabel
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            titleLabel.Location = new Point(27, 10);
            titleLabel.Margin = new Padding(2, 0, 2, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(130, 25);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Iniciar Sesión";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            
            // registerLinkLabel
            registerLinkLabel = new LinkLabel();
            registerLinkLabel.AutoSize = true;
            registerLinkLabel.Location = new Point(40, 160);
            registerLinkLabel.Name = "registerLinkLabel";
            registerLinkLabel.Size = new Size(113, 15);
            registerLinkLabel.TabIndex = 7;
            registerLinkLabel.TabStop = true;
            registerLinkLabel.Text = "Crear nueva cuenta";
            registerLinkLabel.LinkClicked += registerLinkLabel_LinkClicked;
            
            // errorProvider
            errorProvider.ContainerControl = this;
            
            // LoginForm
            AcceptButton = loginButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new Size(194, 190);
            Controls.Add(registerLinkLabel);
            Controls.Add(cancelButton);
            Controls.Add(loginButton);
            Controls.Add(passwordTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(usernameTextBox);
            Controls.Add(usernameLabel);
            Controls.Add(titleLabel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(2, 1, 2, 1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "TPI - Login";
            Load += LoginForm_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private Label usernameLabel;
        private TextBox usernameTextBox;
        private Label passwordLabel;
        private TextBox passwordTextBox;
        private Button loginButton;
        private Button cancelButton;
        private ErrorProvider errorProvider;
        private LinkLabel registerLinkLabel;
    }
}
