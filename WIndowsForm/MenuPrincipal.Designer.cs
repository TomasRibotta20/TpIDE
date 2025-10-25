partial class MenuPrincipal
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

    // Campos agregados por el diseñador
    private Panel mainPanel;
    private Button btnUsuarios;
    private Button btnAlumnos;
    private Button btnProfesores;
    private Button btnEspecialidades;
    private Button btnPlanes;
    private Button btnComisiones;
    private Button btnCursos;
    private Button btnInscripciones;
    private Label titleLabel;
    private Label lblSubtitulo;
    private Panel headerPanel;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem usuarioToolStripMenuItem;
    private ToolStripMenuItem nuevoUsuarioToolStripMenuItem;
    private ToolStripMenuItem listarUsuariosToolStripMenuItem;
    private ToolStripMenuItem alumnoToolStripMenuItem;
    private ToolStripMenuItem nuevoAlumnoToolStripMenuItem;
    private ToolStripMenuItem listarAlumnosToolStripMenuItem;
    private ToolStripMenuItem profesorToolStripMenuItem;
    private ToolStripMenuItem nuevoProfesorToolStripMenuItem;
    private ToolStripMenuItem listarProfesoresToolStripMenuItem;
    private ToolStripMenuItem especialidadToolStripMenuItem;
    private ToolStripMenuItem nuevaEspecialidadToolStripMenuItem;
    private ToolStripMenuItem listarEspecialidadesToolStripMenuItem;
    private ToolStripMenuItem planToolStripMenuItem;
    private ToolStripMenuItem nuevoPlanToolStripMenuItem;
    private ToolStripMenuItem listarPlanesToolStripMenuItem;
    private ToolStripMenuItem reportePlanesToolStripMenuItem;
    private ToolStripMenuItem comisionToolStripMenuItem;
    private ToolStripMenuItem nuevaComisionToolStripMenuItem;
    private ToolStripMenuItem listarComisionesToolStripMenuItem;
    private ToolStripMenuItem inscripcionToolStripMenuItem;
    private ToolStripMenuItem gestionarInscripcionesToolStripMenuItem;

    private void InitializeComponent()
    {
        mainPanel = new Panel();
        headerPanel = new Panel();
        titleLabel = new Label();
        lblSubtitulo = new Label();
        btnUsuarios = new Button();
        btnAlumnos = new Button();
        btnProfesores = new Button();
        btnEspecialidades = new Button();
        btnPlanes = new Button();
        btnComisiones = new Button();
        btnCursos = new Button();
        btnInscripciones = new Button();
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
        reportePlanesToolStripMenuItem = new ToolStripMenuItem();
        comisionToolStripMenuItem = new ToolStripMenuItem();
        nuevaComisionToolStripMenuItem = new ToolStripMenuItem();
        listarComisionesToolStripMenuItem = new ToolStripMenuItem();
        inscripcionToolStripMenuItem = new ToolStripMenuItem();
        gestionarInscripcionesToolStripMenuItem = new ToolStripMenuItem();
        
        mainPanel.SuspendLayout();
        headerPanel.SuspendLayout();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        
        // 
        // mainPanel
        // 
        mainPanel.BackColor = Color.FromArgb(236, 240, 245);
        mainPanel.Controls.Add(headerPanel);
        mainPanel.Controls.Add(btnUsuarios);
        mainPanel.Controls.Add(btnAlumnos);
        mainPanel.Controls.Add(btnProfesores);
        mainPanel.Controls.Add(btnEspecialidades);
        mainPanel.Controls.Add(btnPlanes);
        mainPanel.Controls.Add(btnComisiones);
        mainPanel.Controls.Add(btnCursos);
        mainPanel.Controls.Add(btnInscripciones);
        mainPanel.Dock = DockStyle.Fill;
        mainPanel.Location = new Point(0, 28);
        mainPanel.Name = "mainPanel";
        mainPanel.Padding = new Padding(30);
        mainPanel.Size = new Size(1200, 700);
        mainPanel.TabIndex = 0;
        
        // 
        // headerPanel
        // 
        headerPanel.BackColor = Color.FromArgb(41, 128, 185);
        headerPanel.Controls.Add(titleLabel);
        headerPanel.Controls.Add(lblSubtitulo);
        headerPanel.Location = new Point(30, 30);
        headerPanel.Name = "headerPanel";
        headerPanel.Size = new Size(1140, 100);
        headerPanel.TabIndex = 0;
        
        // 
        // titleLabel
        // 
        titleLabel.AutoSize = true;
        titleLabel.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
        titleLabel.ForeColor = Color.White;
        titleLabel.Location = new Point(30, 20);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new Size(450, 51);
        titleLabel.TabIndex = 0;
        titleLabel.Text = "Panel de Administración";
        
        // 
        // lblSubtitulo
        // 
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 11F);
        lblSubtitulo.ForeColor = Color.FromArgb(236, 240, 245);
        lblSubtitulo.Location = new Point(33, 68);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(280, 20);
        lblSubtitulo.TabIndex = 1;
        lblSubtitulo.Text = "Sistema Académico - Gestión Completa";
        
        // 
        // btnUsuarios
        // 
        btnUsuarios.BackColor = Color.FromArgb(52, 152, 219);
        btnUsuarios.Cursor = Cursors.Hand;
        btnUsuarios.FlatAppearance.BorderSize = 0;
        btnUsuarios.FlatStyle = FlatStyle.Flat;
        btnUsuarios.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        btnUsuarios.ForeColor = Color.White;
        btnUsuarios.Location = new Point(80, 180);
        btnUsuarios.Name = "btnUsuarios";
        btnUsuarios.Size = new Size(240, 90);
        btnUsuarios.TabIndex = 1;
        btnUsuarios.Text = "👤 Usuarios";
        btnUsuarios.UseVisualStyleBackColor = false;
        
        // 
        // btnAlumnos
        // 
        btnAlumnos.BackColor = Color.FromArgb(46, 204, 113);
        btnAlumnos.Cursor = Cursors.Hand;
        btnAlumnos.FlatAppearance.BorderSize = 0;
        btnAlumnos.FlatStyle = FlatStyle.Flat;
        btnAlumnos.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        btnAlumnos.ForeColor = Color.White;
        btnAlumnos.Location = new Point(350, 180);
        btnAlumnos.Name = "btnAlumnos";
        btnAlumnos.Size = new Size(240, 90);
        btnAlumnos.TabIndex = 2;
        btnAlumnos.Text = "🎓 Alumnos";
        btnAlumnos.UseVisualStyleBackColor = false;
        
        // 
        // btnProfesores
        // 
        btnProfesores.BackColor = Color.FromArgb(155, 89, 182);
        btnProfesores.Cursor = Cursors.Hand;
        btnProfesores.FlatAppearance.BorderSize = 0;
        btnProfesores.FlatStyle = FlatStyle.Flat;
        btnProfesores.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        btnProfesores.ForeColor = Color.White;
        btnProfesores.Location = new Point(620, 180);
        btnProfesores.Name = "btnProfesores";
        btnProfesores.Size = new Size(240, 90);
        btnProfesores.TabIndex = 3;
        btnProfesores.Text = "👨‍🏫 Profesores";
        btnProfesores.UseVisualStyleBackColor = false;
        
        // 
        // btnEspecialidades
        // 
        btnEspecialidades.BackColor = Color.FromArgb(230, 126, 34);
        btnEspecialidades.Cursor = Cursors.Hand;
        btnEspecialidades.FlatAppearance.BorderSize = 0;
        btnEspecialidades.FlatStyle = FlatStyle.Flat;
        btnEspecialidades.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        btnEspecialidades.ForeColor = Color.White;
        btnEspecialidades.Location = new Point(890, 180);
        btnEspecialidades.Name = "btnEspecialidades";
        btnEspecialidades.Size = new Size(240, 90);
        btnEspecialidades.TabIndex = 4;
        btnEspecialidades.Text = "🎯 Especialidades";
        btnEspecialidades.UseVisualStyleBackColor = false;
        
        // 
        // btnPlanes
        // 
        btnPlanes.BackColor = Color.FromArgb(52, 73, 94);
        btnPlanes.Cursor = Cursors.Hand;
        btnPlanes.FlatAppearance.BorderSize = 0;
        btnPlanes.FlatStyle = FlatStyle.Flat;
        btnPlanes.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        btnPlanes.ForeColor = Color.White;
        btnPlanes.Location = new Point(80, 300);
        btnPlanes.Name = "btnPlanes";
        btnPlanes.Size = new Size(240, 90);
        btnPlanes.TabIndex = 5;
        btnPlanes.Text = "📋 Planes";
        btnPlanes.UseVisualStyleBackColor = false;
        
        // 
        // btnComisiones
        // 
        btnComisiones.BackColor = Color.FromArgb(241, 196, 15);
        btnComisiones.Cursor = Cursors.Hand;
        btnComisiones.FlatAppearance.BorderSize = 0;
        btnComisiones.FlatStyle = FlatStyle.Flat;
        btnComisiones.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        btnComisiones.ForeColor = Color.White;
        btnComisiones.Location = new Point(350, 300);
        btnComisiones.Name = "btnComisiones";
        btnComisiones.Size = new Size(240, 90);
        btnComisiones.TabIndex = 6;
        btnComisiones.Text = "📊 Comisiones";
        btnComisiones.UseVisualStyleBackColor = false;
        
        // 
        // btnCursos
        // 
        btnCursos.BackColor = Color.FromArgb(231, 76, 60);
        btnCursos.Cursor = Cursors.Hand;
        btnCursos.FlatAppearance.BorderSize = 0;
        btnCursos.FlatStyle = FlatStyle.Flat;
        btnCursos.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        btnCursos.ForeColor = Color.White;
        btnCursos.Location = new Point(620, 300);
        btnCursos.Name = "btnCursos";
        btnCursos.Size = new Size(240, 90);
        btnCursos.TabIndex = 7;
        btnCursos.Text = "📚 Cursos";
        btnCursos.UseVisualStyleBackColor = false;
        
        // 
        // btnInscripciones
        // 
        btnInscripciones.BackColor = Color.FromArgb(26, 188, 156);
        btnInscripciones.Cursor = Cursors.Hand;
        btnInscripciones.FlatAppearance.BorderSize = 0;
        btnInscripciones.FlatStyle = FlatStyle.Flat;
        btnInscripciones.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        btnInscripciones.ForeColor = Color.White;
        btnInscripciones.Location = new Point(890, 300);
        btnInscripciones.Name = "btnInscripciones";
        btnInscripciones.Size = new Size(240, 90);
        btnInscripciones.TabIndex = 8;
        btnInscripciones.Text = "✍️ Inscripciones";
        btnInscripciones.UseVisualStyleBackColor = false;
        // 
        // menuStrip1
        // 
        menuStrip1.BackColor = Color.FromArgb(44, 62, 80);
        menuStrip1.Font = new Font("Segoe UI", 10F);
        menuStrip1.Items.AddRange(new ToolStripItem[] { 
            usuarioToolStripMenuItem, 
            alumnoToolStripMenuItem,
            profesorToolStripMenuItem,
            especialidadToolStripMenuItem, 
            planToolStripMenuItem,
            comisionToolStripMenuItem,
            inscripcionToolStripMenuItem
        });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Padding = new Padding(10, 4, 0, 4);
        menuStrip1.Size = new Size(1200, 28);
        menuStrip1.TabIndex = 1;
        menuStrip1.Text = "menuStrip1";
        
        // 
        // usuarioToolStripMenuItem
        // 
        usuarioToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevoUsuarioToolStripMenuItem, listarUsuariosToolStripMenuItem });
        usuarioToolStripMenuItem.ForeColor = Color.White;
        usuarioToolStripMenuItem.Name = "usuarioToolStripMenuItem";
        usuarioToolStripMenuItem.Size = new Size(73, 20);
        usuarioToolStripMenuItem.Text = "👤 Usuario";
        
        // 
        // nuevoUsuarioToolStripMenuItem
        // 
        nuevoUsuarioToolStripMenuItem.Name = "nuevoUsuarioToolStripMenuItem";
        nuevoUsuarioToolStripMenuItem.Size = new Size(180, 22);
        nuevoUsuarioToolStripMenuItem.Text = "➕ Nuevo Usuario";
        nuevoUsuarioToolStripMenuItem.Click += nuevoUsuarioToolStripMenuItem_Click;
        
        // 
        // listarUsuariosToolStripMenuItem
        // 
        listarUsuariosToolStripMenuItem.Name = "listarUsuariosToolStripMenuItem";
        listarUsuariosToolStripMenuItem.Size = new Size(180, 22);
        listarUsuariosToolStripMenuItem.Text = "📋 Listar Usuarios";
        listarUsuariosToolStripMenuItem.Click += listarUsuariosToolStripMenuItem_Click;
        
        // 
        // alumnoToolStripMenuItem
        // 
        alumnoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevoAlumnoToolStripMenuItem, listarAlumnosToolStripMenuItem });
        alumnoToolStripMenuItem.ForeColor = Color.White;
        alumnoToolStripMenuItem.Name = "alumnoToolStripMenuItem";
        alumnoToolStripMenuItem.Size = new Size(74, 20);
        alumnoToolStripMenuItem.Text = "🎓 Alumno";
        
        // 
        // nuevoAlumnoToolStripMenuItem
        // 
        nuevoAlumnoToolStripMenuItem.Name = "nuevoAlumnoToolStripMenuItem";
        nuevoAlumnoToolStripMenuItem.Size = new Size(180, 22);
        nuevoAlumnoToolStripMenuItem.Text = "➕ Nuevo Alumno";
        nuevoAlumnoToolStripMenuItem.Click += nuevoAlumnoToolStripMenuItem_Click;
        
        // 
        // listarAlumnosToolStripMenuItem
        // 
        listarAlumnosToolStripMenuItem.Name = "listarAlumnosToolStripMenuItem";
        listarAlumnosToolStripMenuItem.Size = new Size(180, 22);
        listarAlumnosToolStripMenuItem.Text = "📋 Listar Alumnos";
        listarAlumnosToolStripMenuItem.Click += listarAlumnosToolStripMenuItem_Click;
        
        // 
        // profesorToolStripMenuItem
        // 
        profesorToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevoProfesorToolStripMenuItem, listarProfesoresToolStripMenuItem });
        profesorToolStripMenuItem.ForeColor = Color.White;
        profesorToolStripMenuItem.Name = "profesorToolStripMenuItem";
        profesorToolStripMenuItem.Size = new Size(88, 20);
        profesorToolStripMenuItem.Text = "👨‍🏫 Profesor";
        
        // 
        // nuevoProfesorToolStripMenuItem
        // 
        nuevoProfesorToolStripMenuItem.Name = "nuevoProfesorToolStripMenuItem";
        nuevoProfesorToolStripMenuItem.Size = new Size(180, 22);
        nuevoProfesorToolStripMenuItem.Text = "➕ Nuevo Profesor";
        nuevoProfesorToolStripMenuItem.Click += nuevoProfesorToolStripMenuItem_Click;
        
        // 
        // listarProfesoresToolStripMenuItem
        // 
        listarProfesoresToolStripMenuItem.Name = "listarProfesoresToolStripMenuItem";
        listarProfesoresToolStripMenuItem.Size = new Size(180, 22);
        listarProfesoresToolStripMenuItem.Text = "📋 Listar Profesores";
        listarProfesoresToolStripMenuItem.Click += listarProfesoresToolStripMenuItem_Click;
        
        // 
        // especialidadToolStripMenuItem
        // 
        especialidadToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevaEspecialidadToolStripMenuItem, listarEspecialidadesToolStripMenuItem });
        especialidadToolStripMenuItem.ForeColor = Color.White;
        especialidadToolStripMenuItem.Name = "especialidadToolStripMenuItem";
        especialidadToolStripMenuItem.Size = new Size(105, 20);
        especialidadToolStripMenuItem.Text = "🎯 Especialidad";
        
        // 
        // nuevaEspecialidadToolStripMenuItem
        // 
        nuevaEspecialidadToolStripMenuItem.Name = "nuevaEspecialidadToolStripMenuItem";
        nuevaEspecialidadToolStripMenuItem.Size = new Size(200, 22);
        nuevaEspecialidadToolStripMenuItem.Text = "➕ Nueva Especialidad";
        nuevaEspecialidadToolStripMenuItem.Click += nuevaEspecialidadToolStripMenuItem_Click;
        
        // 
        // listarEspecialidadesToolStripMenuItem
        // 
        listarEspecialidadesToolStripMenuItem.Name = "listarEspecialidadesToolStripMenuItem";
        listarEspecialidadesToolStripMenuItem.Size = new Size(200, 22);
        listarEspecialidadesToolStripMenuItem.Text = "📋 Listar Especialidades";
        listarEspecialidadesToolStripMenuItem.Click += listarEspecialidadesToolStripMenuItem_Click;
        // 
        // planToolStripMenuItem
        // 
        planToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevoPlanToolStripMenuItem, listarPlanesToolStripMenuItem, reportePlanesToolStripMenuItem });
        planToolStripMenuItem.ForeColor = Color.White;
        planToolStripMenuItem.Name = "planToolStripMenuItem";
        planToolStripMenuItem.Size = new Size(59, 20);
        planToolStripMenuItem.Text = "📋 Plan";
        
        // 
        // nuevoPlanToolStripMenuItem
        // 
        nuevoPlanToolStripMenuItem.Name = "nuevoPlanToolStripMenuItem";
        nuevoPlanToolStripMenuItem.Size = new Size(180, 22);
        nuevoPlanToolStripMenuItem.Text = "➕ Nuevo Plan";
        nuevoPlanToolStripMenuItem.Click += nuevoPlanToolStripMenuItem_Click;
        
        // 
        // listarPlanesToolStripMenuItem
        // 
        listarPlanesToolStripMenuItem.Name = "listarPlanesToolStripMenuItem";
        listarPlanesToolStripMenuItem.Size = new Size(180, 22);
        listarPlanesToolStripMenuItem.Text = "📋 Listar Planes";
        listarPlanesToolStripMenuItem.Click += listarPlanesToolStripMenuItem_Click;
        
        // 
        // reportePlanesToolStripMenuItem
        // 
        reportePlanesToolStripMenuItem.Name = "reportePlanesToolStripMenuItem";
        reportePlanesToolStripMenuItem.Size = new Size(180, 22);
        reportePlanesToolStripMenuItem.Text = "📊 Reporte de Planes";
        reportePlanesToolStripMenuItem.Click += reportePlanesToolStripMenuItem_Click;
        
        // 
        // comisionToolStripMenuItem
        // 
        comisionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevaComisionToolStripMenuItem, listarComisionesToolStripMenuItem });
        comisionToolStripMenuItem.ForeColor = Color.White;
        comisionToolStripMenuItem.Name = "comisionToolStripMenuItem";
        comisionToolStripMenuItem.Size = new Size(91, 20);
        comisionToolStripMenuItem.Text = "📊 Comisión";
        
        // 
        // nuevaComisionToolStripMenuItem
        // 
        nuevaComisionToolStripMenuItem.Name = "nuevaComisionToolStripMenuItem";
        nuevaComisionToolStripMenuItem.Size = new Size(180, 22);
        nuevaComisionToolStripMenuItem.Text = "➕ Nueva Comisión";
        nuevaComisionToolStripMenuItem.Click += nuevaComisionToolStripMenuItem_Click;
        
        // 
        // listarComisionesToolStripMenuItem
        // 
        listarComisionesToolStripMenuItem.Name = "listarComisionesToolStripMenuItem";
        listarComisionesToolStripMenuItem.Size = new Size(180, 22);
        listarComisionesToolStripMenuItem.Text = "📋 Listar Comisiones";
        listarComisionesToolStripMenuItem.Click += listarComisionesToolStripMenuItem_Click;
        
        // 
        // inscripcionToolStripMenuItem
        // 
        inscripcionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { gestionarInscripcionesToolStripMenuItem });
        inscripcionToolStripMenuItem.ForeColor = Color.White;
        inscripcionToolStripMenuItem.Name = "inscripcionToolStripMenuItem";
        inscripcionToolStripMenuItem.Size = new Size(99, 20);
        inscripcionToolStripMenuItem.Text = "✍️ Inscripción";
        
        // 
        // gestionarInscripcionesToolStripMenuItem
        // 
        gestionarInscripcionesToolStripMenuItem.Name = "gestionarInscripcionesToolStripMenuItem";
        gestionarInscripcionesToolStripMenuItem.Size = new Size(220, 22);
        gestionarInscripcionesToolStripMenuItem.Text = "📝 Gestionar Inscripciones";
        gestionarInscripcionesToolStripMenuItem.Click += gestionarInscripcionesToolStripMenuItem_Click;
        
        // 
        // MenuPrincipal
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 728);
        Controls.Add(mainPanel);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        Name = "MenuPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sistema Académico - Panel de Administración";
        WindowState = FormWindowState.Maximized;
        Load += MenuPrincipal_Load;
        mainPanel.ResumeLayout(false);
        headerPanel.ResumeLayout(false);
        headerPanel.PerformLayout();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}