using WIndowsForm;

public partial class MenuPrincipal : Form
{
    public MenuPrincipal()
    {
        InitializeComponent();
        ConfigureEvents();
    }

    private void ConfigureEvents()
    {
        btnUsuarios.Click += (s, e) => {
            var formUsuarios = new FormUsuarios(this);
            formUsuarios.Show();
            this.Hide();
        };

        btnEspecialidades.Click += (s, e) => {
            var formEspecialidades = new FormEspecialidades(this);
            formEspecialidades.Show();
            this.Hide();
        };
    }
    private void MenuPrincipal_Load(object sender, EventArgs e)
    {
        // Código de inicialización...
    }

    private void InitializeComponent()
    {
        mainPanel = new Panel();
        lblProximamente = new Label();
        btnOtroCrud = new Button();
        btnEspecialidades = new Button();
        btnUsuarios = new Button();
        titleLabel = new Label();
        mainPanel.SuspendLayout();
        SuspendLayout();
        // 
        // mainPanel
        // 
        mainPanel.Controls.Add(lblProximamente);
        mainPanel.Controls.Add(btnOtroCrud);
        mainPanel.Controls.Add(btnEspecialidades);
        mainPanel.Controls.Add(btnUsuarios);
        mainPanel.Controls.Add(titleLabel);
        mainPanel.Dock = DockStyle.Fill;
        mainPanel.Location = new Point(0, 0);
        mainPanel.Name = "mainPanel";
        mainPanel.Padding = new Padding(20);
        mainPanel.Size = new Size(484, 411);
        mainPanel.TabIndex = 0;
        // 
        // lblProximamente
        // 
        lblProximamente.Font = new Font("Arial", 9.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
        lblProximamente.Location = new Point(150, 310);
        lblProximamente.Name = "lblProximamente";
        lblProximamente.Size = new Size(200, 20);
        lblProximamente.TabIndex = 4;
        lblProximamente.Text = "Mas CRUDs Proximamente...";
        lblProximamente.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // btnOtroCrud
        // 
        btnOtroCrud.Enabled = false;
        btnOtroCrud.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnOtroCrud.Location = new Point(150, 240);
        btnOtroCrud.Name = "btnOtroCrud";
        btnOtroCrud.Size = new Size(200, 50);
        btnOtroCrud.TabIndex = 3;
        btnOtroCrud.Text = "Otro Crud (No implementado)";
        btnOtroCrud.UseVisualStyleBackColor = true;
        // 
        // btnEspecialidades
        // 
        btnEspecialidades.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnEspecialidades.Location = new Point(150, 170);
        btnEspecialidades.Name = "btnEspecialidades";
        btnEspecialidades.Size = new Size(200, 50);
        btnEspecialidades.TabIndex = 2;
        btnEspecialidades.Text = "Gestion de Especialidades";
        btnEspecialidades.UseVisualStyleBackColor = true;
        // 
        // btnUsuarios
        // 
        btnUsuarios.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnUsuarios.Location = new Point(150, 100);
        btnUsuarios.Name = "btnUsuarios";
        btnUsuarios.Size = new Size(200, 50);
        btnUsuarios.TabIndex = 1;
        btnUsuarios.Text = "Gestion de Usuarios";
        btnUsuarios.UseVisualStyleBackColor = true;
        // 
        // titleLabel
        // 
        titleLabel.AutoSize = true;
        titleLabel.Font = new Font("Arial", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        titleLabel.Location = new Point(115, 47);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new Size(270, 32);
        titleLabel.TabIndex = 0;
        titleLabel.Text = "Sistema de Gestion";
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // MenuPrincipal
        // 
        ClientSize = new Size(484, 411);
        Controls.Add(mainPanel);
        Name = "MenuPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SIstema de Gestion";
        Load += MenuPrincipal_Load_1;
        mainPanel.ResumeLayout(false);
        mainPanel.PerformLayout();
        ResumeLayout(false);

    }

    private void MenuPrincipal_Load_1(object sender, EventArgs e)
    {

    }
}