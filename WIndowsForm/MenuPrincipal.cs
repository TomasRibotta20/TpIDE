using API.Clients;
using DTOs;
using System.ComponentModel;
using System.Diagnostics;
using WIndowsForm;

public partial class MenuPrincipal : Form
{

    public MenuPrincipal()
    {
        InitializeComponent();
        ConfigureEvents();

        try
        {
            string apiUrl = "https://localhost:7229";

            Debug.WriteLine($"Conectando a API en: {apiUrl}");
            _apiClient = new UsuarioApiClient();
            _especialidadApiClient = new EspecialidadApiClient(); // Inicializa el cliente de especialidades
            _comisionApiClient = new ComisionApiClient(); // Inicializa el cliente de comisiones
            
            // Create and initialize Comisiones UI
            InitializeComisionesUI();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al inicializar: {ex.Message}",
                "Error de inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void InitializeComisionesUI()
    {
        // Create button
        btnComisiones = new Button();
        btnComisiones.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnComisiones.Location = new Point(150, 310);
        btnComisiones.Name = "btnComisiones";
        btnComisiones.Size = new Size(200, 50);
        btnComisiones.TabIndex = 4;
        btnComisiones.Text = "Gestión de Comisiones";
        btnComisiones.UseVisualStyleBackColor = true;
        btnComisiones.Click += BtnComisiones_Click;
        
        // Move proximamente label down
        lblProximamente.Location = new Point(150, 375);
        
        // Add to panel
        mainPanel.Controls.Add(btnComisiones);
        
        // Create menu items
        comisionToolStripMenuItem = new ToolStripMenuItem();
        nuevaComisionToolStripMenuItem = new ToolStripMenuItem();
        listarComisionesToolStripMenuItem = new ToolStripMenuItem();
        
        // Configure menu items
        comisionToolStripMenuItem.Name = "comisionToolStripMenuItem";
        comisionToolStripMenuItem.Size = new Size(73, 20);
        comisionToolStripMenuItem.Text = "Comisión";
        
        nuevaComisionToolStripMenuItem.Name = "nuevaComisionToolStripMenuItem";
        nuevaComisionToolStripMenuItem.Size = new Size(180, 22);
        nuevaComisionToolStripMenuItem.Text = "Nueva Comisión";
        nuevaComisionToolStripMenuItem.Click += nuevaComisionToolStripMenuItem_Click;
        
        listarComisionesToolStripMenuItem.Name = "listarComisionesToolStripMenuItem";
        listarComisionesToolStripMenuItem.Size = new Size(180, 22);
        listarComisionesToolStripMenuItem.Text = "Listar Comisiones";
        listarComisionesToolStripMenuItem.Click += listarComisionesToolStripMenuItem_Click;
        
        // Add sub-items to Comisiones menu
        comisionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 
            nuevaComisionToolStripMenuItem, 
            listarComisionesToolStripMenuItem 
        });
        
        // Add to main menu strip
        menuStrip1.Items.Add(comisionToolStripMenuItem);
    }

    private void BtnComisiones_Click(object sender, EventArgs e)
    {
        var formComisiones = new FormComisiones(this);
        formComisiones.Show();
        this.Hide();
    }

    private void ConfigureEvents()
    {
        btnUsuarios.Click += (s, e) =>
        {
            var formUsuarios = new FormUsuarios(this);
            formUsuarios.Show();
            this.Hide();
        };

        btnEspecialidades.Click += (s, e) =>
        {
            var formEspecialidades = new FormEspecialidades(this);
            formEspecialidades.Show();
            this.Hide();
        };

        btnPlanes.Click += (s, e) =>
        {
            var formPlanes = new FormPlanes(this);
            formPlanes.Show();
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
        btnPlanes = new Button();
        btnEspecialidades = new Button();
        btnUsuarios = new Button();
        titleLabel = new Label();
        menuStrip1 = new MenuStrip();
        usuarioToolStripMenuItem = new ToolStripMenuItem();
        nuevoUsuarioToolStripMenuItem = new ToolStripMenuItem();
        listarUsuariosToolStripMenuItem = new ToolStripMenuItem();
        especialidadToolStripMenuItem = new ToolStripMenuItem();
        nuevaEspecialidadToolStripMenuItem = new ToolStripMenuItem();
        listarEspecialidadesToolStripMenuItem = new ToolStripMenuItem();
        planToolStripMenuItem = new ToolStripMenuItem();
        nuevoPlanToolStripMenuItem = new ToolStripMenuItem();
        listarPlanesToolStripMenuItem = new ToolStripMenuItem();
        mainPanel.SuspendLayout();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // mainPanel
        // 
        mainPanel.Controls.Add(lblProximamente);
        mainPanel.Controls.Add(btnPlanes);
        mainPanel.Controls.Add(btnEspecialidades);
        mainPanel.Controls.Add(btnUsuarios);
        mainPanel.Controls.Add(titleLabel);
        mainPanel.Dock = DockStyle.Fill;
        mainPanel.Location = new Point(0, 24);
        mainPanel.Name = "mainPanel";
        mainPanel.Padding = new Padding(20);
        mainPanel.Size = new Size(484, 473);
        mainPanel.TabIndex = 0;
        // 
        // lblProximamente
        // 
        lblProximamente.Font = new Font("Arial", 9.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
        lblProximamente.Location = new Point(150, 375);
        lblProximamente.Name = "lblProximamente";
        lblProximamente.Size = new Size(200, 20);
        lblProximamente.TabIndex = 4;
        lblProximamente.Text = "Mas CRUDs Proximamente...";
        lblProximamente.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // btnPlanes
        // 
        btnPlanes.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnPlanes.Location = new Point(150, 240);
        btnPlanes.Name = "btnPlanes";
        btnPlanes.Size = new Size(200, 50);
        btnPlanes.TabIndex = 3;
        btnPlanes.Text = "Gestión de Planes";
        btnPlanes.UseVisualStyleBackColor = true;
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
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { usuarioToolStripMenuItem, especialidadToolStripMenuItem, planToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(484, 24);
        menuStrip1.TabIndex = 1;
        menuStrip1.Text = "menuStrip1";
        // 
        // usuarioToolStripMenuItem
        // 
        usuarioToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevoUsuarioToolStripMenuItem, listarUsuariosToolStripMenuItem });
        usuarioToolStripMenuItem.Name = "usuarioToolStripMenuItem";
        usuarioToolStripMenuItem.Size = new Size(59, 20);
        usuarioToolStripMenuItem.Text = "Usuario";
        // 
        // nuevoUsuarioToolStripMenuItem
        // 
        nuevoUsuarioToolStripMenuItem.Name = "nuevoUsuarioToolStripMenuItem";
        nuevoUsuarioToolStripMenuItem.Size = new Size(152, 22);
        nuevoUsuarioToolStripMenuItem.Text = "Nuevo Usuario";
        nuevoUsuarioToolStripMenuItem.Click += nuevoUsuarioToolStripMenuItem_Click;
        // 
        // listarUsuariosToolStripMenuItem
        // 
        listarUsuariosToolStripMenuItem.Name = "listarUsuariosToolStripMenuItem";
        listarUsuariosToolStripMenuItem.Size = new Size(152, 22);
        listarUsuariosToolStripMenuItem.Text = "Listar Usuarios";
        listarUsuariosToolStripMenuItem.Click += listarUsuariosToolStripMenuItem_Click;
        // 
        // especialidadToolStripMenuItem
        // 
        especialidadToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevaEspecialidadToolStripMenuItem, listarEspecialidadesToolStripMenuItem });
        especialidadToolStripMenuItem.Name = "especialidadToolStripMenuItem";
        especialidadToolStripMenuItem.Size = new Size(84, 20);
        especialidadToolStripMenuItem.Text = "Especialidad";
        // 
        // nuevaEspecialidadToolStripMenuItem
        // 
        nuevaEspecialidadToolStripMenuItem.Name = "nuevaEspecialidadToolStripMenuItem";
        nuevaEspecialidadToolStripMenuItem.Size = new Size(181, 22);
        nuevaEspecialidadToolStripMenuItem.Text = "Nueva Especialidad";
        nuevaEspecialidadToolStripMenuItem.Click += nuevaEspecialidadToolStripMenuItem_Click;
        // 
        // listarEspecialidadesToolStripMenuItem
        // 
        listarEspecialidadesToolStripMenuItem.Name = "listarEspecialidadesToolStripMenuItem";
        listarEspecialidadesToolStripMenuItem.Size = new Size(181, 22);
        listarEspecialidadesToolStripMenuItem.Text = "Listar Especialidades";
        listarEspecialidadesToolStripMenuItem.Click += listarEspecialidadesToolStripMenuItem_Click;
        // 
        // planToolStripMenuItem
        // 
        planToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevoPlanToolStripMenuItem, listarPlanesToolStripMenuItem });
        planToolStripMenuItem.Name = "planToolStripMenuItem";
        planToolStripMenuItem.Size = new Size(45, 20);
        planToolStripMenuItem.Text = "Plan";
        // 
        // nuevoPlanToolStripMenuItem
        // 
        nuevoPlanToolStripMenuItem.Name = "nuevoPlanToolStripMenuItem";
        nuevoPlanToolStripMenuItem.Size = new Size(180, 22);
        nuevoPlanToolStripMenuItem.Text = "Nuevo Plan";
        nuevoPlanToolStripMenuItem.Click += nuevoPlanToolStripMenuItem_Click;
        // 
        // listarPlanesToolStripMenuItem
        // 
        listarPlanesToolStripMenuItem.Name = "listarPlanesToolStripMenuItem";
        listarPlanesToolStripMenuItem.Size = new Size(180, 22);
        listarPlanesToolStripMenuItem.Text = "Listar Planes";
        listarPlanesToolStripMenuItem.Click += listarPlanesToolStripMenuItem_Click;
        // 
        // MenuPrincipal
        // 
        ClientSize = new Size(484, 497);
        Controls.Add(mainPanel);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        Name = "MenuPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SIstema de Gestion";
        Load += MenuPrincipal_Load_1;
        mainPanel.ResumeLayout(false);
        mainPanel.PerformLayout();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();

    }
    private readonly UsuarioApiClient _apiClient;
    private readonly Form _menuPrincipal;
    private BindingList<UsuarioDto> _usuarios = new BindingList<UsuarioDto>();
    private BindingList<EspecialidadDto> _especialidades = new BindingList<EspecialidadDto>();
    // Agrega este campo para el cliente de especialidades
    private readonly EspecialidadApiClient _especialidadApiClient;
    // Agrega este campo para el cliente de comisiones
    private readonly ComisionApiClient _comisionApiClient;
    private BindingList<ComisionDto> _comisiones = new BindingList<ComisionDto>();
    
    private void MenuPrincipal_Load_1(object sender, EventArgs e)
    {

    }

    // Ejemplo para el método nuevaEspecialidadToolStripMenuItem_Click:
    private async void nuevaEspecialidadToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var formNuevaEspecialidad = new EditarEspecialidadForm();
        formNuevaEspecialidad.ShowDialog();

        if (formNuevaEspecialidad.Guardado && formNuevaEspecialidad.EspecialidadEditada != null)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                await _especialidadApiClient.CreateAsync(formNuevaEspecialidad.EspecialidadEditada);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar especialidad: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                var formEspecialidades = new FormEspecialidades(this);
                formEspecialidades.ShowDialog();
            }
        }
    }

    private void listarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var formUsuarios = new FormUsuarios(this);
        formUsuarios.Show();
        this.Hide();
    }

    private void listarEspecialidadesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var formEspecialidades = new FormEspecialidades(this);
        formEspecialidades.Show();
        this.Hide();
    }

    private async Task LoadUsuariosAsync()
    {
        try
        {
            Cursor.Current = Cursors.WaitCursor;
            var usuarios = await _apiClient.GetAllAsync();

            _usuarios.Clear();
            if (usuarios != null)
            {
                foreach (var usuario in usuarios)
                {
                    _usuarios.Add(usuario);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar usuarios: {ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }


    // Reemplaza la línea con el error en el método nuevoUsuarioToolStripMenuItem_Click
    private async void nuevoUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var formNuevoUsuario = new EditarUsuarioForm();
        formNuevoUsuario.ShowDialog();

        if (formNuevoUsuario.Guardado && formNuevoUsuario.UsuarioEditado != null)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                await _apiClient.CreateAsync(formNuevoUsuario.UsuarioEditado);
                await LoadUsuariosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar usuario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                var formUsuarios = new FormUsuarios(this);
                formUsuarios.ShowDialog();
            }
        }
    }

    private async void nuevoPlanToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var form = new EditarPlanForm();
        form.ShowDialog();
        if (form.Guardado && form.PlanEditado != null)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var client = new PlanApiClient();
                await client.CreateAsync(form.PlanEditado);
                var listado = new FormPlanes(this);
                listado.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally { Cursor.Current = Cursors.Default; }
        }
    }

    private void listarPlanesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var formPlanes = new FormPlanes(this);
        formPlanes.Show();
        this.Hide();
    }

    // Nuevos métodos para la funcionalidad de Comisiones
    private async void nuevaComisionToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var form = new EditarComisionForm();
        form.ShowDialog();
        if (form.Guardado && form.ComisionEditada != null)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                await _comisionApiClient.CreateAsync(form.ComisionEditada);
                var listado = new FormComisiones(this);
                listado.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally { Cursor.Current = Cursors.Default; }
        }
    }

    private void listarComisionesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var formComisiones = new FormComisiones(this);
        formComisiones.Show();
        this.Hide();
    }
}
