using API.Clients;
using DTOs;
using System.ComponentModel;
using System.Diagnostics;
using WIndowsForm;

public partial class MenuPrincipal : Form
{
    // Campos de datos y API clients
    private readonly UsuarioApiClient _apiClient;
    private readonly EspecialidadApiClient _especialidadApiClient;
    private readonly ComisionApiClient _comisionApiClient;
    private readonly PersonaApiClient _personaApiClient;
    private BindingList<UsuarioDto> _usuarios = new BindingList<UsuarioDto>();
    private BindingList<EspecialidadDto> _especialidades = new BindingList<EspecialidadDto>();
    private BindingList<ComisionDto> _comisiones = new BindingList<ComisionDto>();

    public MenuPrincipal()
    {
        InitializeComponent();
        ConfigureEvents();

        try
        {
            string apiUrl = "https://localhost:7229";

            Debug.WriteLine($"Conectando a API en: {apiUrl}");
            _apiClient = new UsuarioApiClient();
            _especialidadApiClient = new EspecialidadApiClient();
            _comisionApiClient = new ComisionApiClient();
            _personaApiClient = new PersonaApiClient();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al inicializar: {ex.Message}",
                "Error de inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ConfigureEvents()
    {
        try
        {
            btnUsuarios.Click += (s, e) =>
            {
                try
                {
                    var formUsuarios = new FormUsuarios(this);
                    formUsuarios.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al abrir formulario de usuarios: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnAlumnos.Click += (s, e) =>
            {
                try
                {
                    var formAlumnos = new FormAlumnos(this);
                    formAlumnos.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al abrir formulario de alumnos: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnEspecialidades.Click += (s, e) =>
            {
                try
                {
                    var formEspecialidades = new FormEspecialidades(this);
                    formEspecialidades.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al abrir formulario de especialidades: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnPlanes.Click += (s, e) =>
            {
                try
                {
                    var formPlanes = new FormPlanes(this);
                    formPlanes.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al abrir formulario de planes: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnComisiones.Click += (s, e) =>
            {
                try
                {
                    var formComisiones = new FormComisiones(this);
                    formComisiones.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al abrir formulario de comisiones: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnProfesores.Click += (s, e) =>
            {
                try
                {
                    var formProfesores = new FormProfesores(this);
                    formProfesores.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al abrir formulario de profesores: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al configurar eventos: {ex.Message}", 
                "Error de configuración", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void MenuPrincipal_Load(object sender, EventArgs e)
    {
        // Código de inicialización...
    }

    private void listarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var formAlumnos = new FormAlumnos(this);
            formAlumnos.Show();
            this.Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al abrir listado de alumnos: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void nuevoAlumnoToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var formNuevoAlumno = new EditarAlumnoForm();
            formNuevoAlumno.ShowDialog();

            if (formNuevoAlumno.Guardado && formNuevoAlumno.AlumnoEditado != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _personaApiClient.CreateAsync(formNuevoAlumno.AlumnoEditado);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar alumno: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    var formAlumnos = new FormAlumnos(this);
                    formAlumnos.Show();
                    this.Hide();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al crear nuevo alumno: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void InitializeComponent()
    {
        mainPanel = new Panel();
        lblProximamente = new Label();
        btnAlumnos = new Button();
        btnProfesores = new Button();
        btnComisiones = new Button();
        btnPlanes = new Button();
        btnEspecialidades = new Button();
        btnUsuarios = new Button();
        titleLabel = new Label();
        menuStrip1 = new MenuStrip();
        usuarioToolStripMenuItem = new ToolStripMenuItem();
        nuevoUsuarioToolStripMenuItem = new ToolStripMenuItem();
        listarUsuariosToolStripMenuItem = new ToolStripMenuItem();
        alumnoToolStripMenuItem = new ToolStripMenuItem();
        nuevoAlumnoToolStripMenuItem = new ToolStripMenuItem();
        listarAlumnosToolStripMenuItem = new ToolStripMenuItem();
        profesorToolStripMenuItem = new ToolStripMenuItem();
        nuevoProfesorToolStripMenuItem = new ToolStripMenuItem();
        listarProfesoresToolStripMenuItem = new ToolStripMenuItem();
        especialidadToolStripMenuItem = new ToolStripMenuItem();
        nuevaEspecialidadToolStripMenuItem = new ToolStripMenuItem();
        listarEspecialidadesToolStripMenuItem = new ToolStripMenuItem();
        planToolStripMenuItem = new ToolStripMenuItem();
        nuevoPlanToolStripMenuItem = new ToolStripMenuItem();
        listarPlanesToolStripMenuItem = new ToolStripMenuItem();
        comisionToolStripMenuItem = new ToolStripMenuItem();
        nuevaComisionToolStripMenuItem = new ToolStripMenuItem();
        listarComisionesToolStripMenuItem = new ToolStripMenuItem();
        mainPanel.SuspendLayout();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // mainPanel
        // 
        mainPanel.Controls.Add(lblProximamente);
        mainPanel.Controls.Add(btnProfesores);
        mainPanel.Controls.Add(btnAlumnos);
        mainPanel.Controls.Add(btnComisiones);
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
        lblProximamente.Location = new Point(150, 420);
        lblProximamente.Name = "lblProximamente";
        lblProximamente.Size = new Size(200, 20);
        lblProximamente.TabIndex = 6;
        lblProximamente.Text = "Mas CRUDs Proximamente...";
        lblProximamente.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // btnAlumnos
        // 
        btnAlumnos.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnAlumnos.Location = new Point(150, 170);
        btnAlumnos.Name = "btnAlumnos";
        btnAlumnos.Size = new Size(200, 50);
        btnAlumnos.TabIndex = 5;
        btnAlumnos.Text = "Gestión de Alumnos";
        btnAlumnos.UseVisualStyleBackColor = true;
        // 
        // btnProfesores
        // 
        btnProfesores.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnProfesores.Location = new Point(150, 230);
        btnProfesores.Name = "btnProfesores";
        btnProfesores.Size = new Size(200, 50);
        btnProfesores.TabIndex = 6;
        btnProfesores.Text = "Gestión de Profesores";
        btnProfesores.UseVisualStyleBackColor = true;
        // 
        // btnEspecialidades
        // 
        btnEspecialidades.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnEspecialidades.Location = new Point(150, 290);
        btnEspecialidades.Name = "btnEspecialidades";
        btnEspecialidades.Size = new Size(200, 50);
        btnEspecialidades.TabIndex = 2;
        btnEspecialidades.Text = "Gestion de Especialidades";
        btnEspecialidades.UseVisualStyleBackColor = true;
        // 
        // btnPlanes
        // 
        btnPlanes.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnPlanes.Location = new Point(150, 350);
        btnPlanes.Name = "btnPlanes";
        btnPlanes.Size = new Size(200, 50);
        btnPlanes.TabIndex = 3;
        btnPlanes.Text = "Gestión de Planes";
        btnPlanes.UseVisualStyleBackColor = true;
        // 
        // btnComisiones
        // 
        btnComisiones.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnComisiones.Location = new Point(25, 350);
        btnComisiones.Name = "btnComisiones";
        btnComisiones.Size = new Size(100, 50);
        btnComisiones.TabIndex = 4;
        btnComisiones.Text = "Gestión de Comisiones";
        btnComisiones.UseVisualStyleBackColor = true;
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
        menuStrip1.Items.AddRange(new ToolStripItem[] { 
            usuarioToolStripMenuItem, 
            alumnoToolStripMenuItem,
            profesorToolStripMenuItem,
            especialidadToolStripMenuItem, 
            planToolStripMenuItem,
            comisionToolStripMenuItem 
        });
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
        // alumnoToolStripMenuItem
        // 
        alumnoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevoAlumnoToolStripMenuItem, listarAlumnosToolStripMenuItem });
        alumnoToolStripMenuItem.Name = "alumnoToolStripMenuItem";
        alumnoToolStripMenuItem.Size = new Size(60, 20);
        alumnoToolStripMenuItem.Text = "Alumno";
        // 
        // nuevoAlumnoToolStripMenuItem
        // 
        nuevoAlumnoToolStripMenuItem.Name = "nuevoAlumnoToolStripMenuItem";
        nuevoAlumnoToolStripMenuItem.Size = new Size(152, 22);
        nuevoAlumnoToolStripMenuItem.Text = "Nuevo Alumno";
        nuevoAlumnoToolStripMenuItem.Click += nuevoAlumnoToolStripMenuItem_Click;
        // 
        // listarAlumnosToolStripMenuItem
        // 
        listarAlumnosToolStripMenuItem.Name = "listarAlumnosToolStripMenuItem";
        listarAlumnosToolStripMenuItem.Size = new Size(152, 22);
        listarAlumnosToolStripMenuItem.Text = "Listar Alumnos";
        listarAlumnosToolStripMenuItem.Click += listarAlumnosToolStripMenuItem_Click;
        // 
        // profesorToolStripMenuItem
        // 
        profesorToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevoProfesorToolStripMenuItem, listarProfesoresToolStripMenuItem });
        profesorToolStripMenuItem.Name = "profesorToolStripMenuItem";
        profesorToolStripMenuItem.Size = new Size(64, 20);
        profesorToolStripMenuItem.Text = "Profesor";
        // 
        // nuevoProfesorToolStripMenuItem
        // 
        nuevoProfesorToolStripMenuItem.Name = "nuevoProfesorToolStripMenuItem";
        nuevoProfesorToolStripMenuItem.Size = new Size(156, 22);
        nuevoProfesorToolStripMenuItem.Text = "Nuevo Profesor";
        nuevoProfesorToolStripMenuItem.Click += nuevoProfesorToolStripMenuItem_Click;
        // 
        // listarProfesoresToolStripMenuItem
        // 
        listarProfesoresToolStripMenuItem.Name = "listarProfesoresToolStripMenuItem";
        listarProfesoresToolStripMenuItem.Size = new Size(156, 22);
        listarProfesoresToolStripMenuItem.Text = "Listar Profesores";
        listarProfesoresToolStripMenuItem.Click += listarProfesoresToolStripMenuItem_Click;
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
        // comisionToolStripMenuItem
        // 
        comisionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevaComisionToolStripMenuItem, listarComisionesToolStripMenuItem });
        comisionToolStripMenuItem.Name = "comisionToolStripMenuItem";
        comisionToolStripMenuItem.Size = new Size(73, 20);
        comisionToolStripMenuItem.Text = "Comisión";
        // 
        // nuevaComisionToolStripMenuItem
        // 
        nuevaComisionToolStripMenuItem.Name = "nuevaComisionToolStripMenuItem";
        nuevaComisionToolStripMenuItem.Size = new Size(180, 22);
        nuevaComisionToolStripMenuItem.Text = "Nueva Comisión";
        nuevaComisionToolStripMenuItem.Click += nuevaComisionToolStripMenuItem_Click;
        // 
        // listarComisionesToolStripMenuItem
        // 
        listarComisionesToolStripMenuItem.Name = "listarComisionesToolStripMenuItem";
        listarComisionesToolStripMenuItem.Size = new Size(180, 22);
        listarComisionesToolStripMenuItem.Text = "Listar Comisiones";
        listarComisionesToolStripMenuItem.Click += listarComisionesToolStripMenuItem_Click;
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
        Load += MenuPrincipal_Load;
        mainPanel.ResumeLayout(false);
        mainPanel.PerformLayout();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private async void nuevaEspecialidadToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            MessageBox.Show($"Error al crear nueva especialidad: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void listarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var formUsuarios = new FormUsuarios(this);
            formUsuarios.Show();
            this.Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al listar usuarios: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void listarEspecialidadesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var formEspecialidades = new FormEspecialidades(this);
            formEspecialidades.Show();
            this.Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al listar especialidades: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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

    private async void nuevoUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            MessageBox.Show($"Error al crear nuevo usuario: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void nuevoPlanToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            MessageBox.Show($"Error al crear nuevo plan: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void listarPlanesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var formPlanes = new FormPlanes(this);
            formPlanes.Show();
            this.Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al listar planes: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void nuevaComisionToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            MessageBox.Show($"Error al crear nueva comisión: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void listarComisionesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var formComisiones = new FormComisiones(this);
            formComisiones.Show();
            this.Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al listar comisiones: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void listarProfesoresToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var formProfesores = new FormProfesores(this);
            formProfesores.Show();
            this.Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al listar profesores: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void nuevoProfesorToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var formNuevoProfesor = new EditarProfesorForm();
            formNuevoProfesor.ShowDialog();

            if (formNuevoProfesor.Guardado && formNuevoProfesor.ProfesorEditado != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _personaApiClient.CreateAsync(formNuevoProfesor.ProfesorEditado);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar profesor: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    var formProfesores = new FormProfesores(this);
                    formProfesores.Show();
                    this.Hide();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al crear nuevo profesor: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}