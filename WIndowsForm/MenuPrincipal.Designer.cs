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
    private Button btnMaterias; // Nuevo botón de materias
    private Button btnCursos;
    private Button btnInscripciones;
    private Label titleLabel;
    private Label lblProximamente;
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
    private ToolStripMenuItem comisionToolStripMenuItem;
    private ToolStripMenuItem nuevaComisionToolStripMenuItem;
    private ToolStripMenuItem listarComisionesToolStripMenuItem;
    private ToolStripMenuItem materiaToolStripMenuItem; // Nuevo menú de materias
    private ToolStripMenuItem nuevaMateriaToolStripMenuItem;
    private ToolStripMenuItem listarMateriasToolStripMenuItem;
    private ToolStripMenuItem inscripcionToolStripMenuItem;
    private ToolStripMenuItem gestionarInscripcionesToolStripMenuItem;

    private void InitializeComponent()
    {
        mainPanel = new Panel();
        lblProximamente = new Label();
        btnAlumnos = new Button();
        btnProfesores = new Button();
        btnComisiones = new Button();
        btnMaterias = new Button(); // Nuevo botón
        btnCursos = new Button();
        btnInscripciones = new Button();
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
        materiaToolStripMenuItem = new ToolStripMenuItem(); // Nuevo menú
        nuevaMateriaToolStripMenuItem = new ToolStripMenuItem();
        listarMateriasToolStripMenuItem = new ToolStripMenuItem();
        inscripcionToolStripMenuItem = new ToolStripMenuItem();
        gestionarInscripcionesToolStripMenuItem = new ToolStripMenuItem();
        mainPanel.SuspendLayout();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // mainPanel
        // 
        mainPanel.Controls.Add(lblProximamente);
        mainPanel.Controls.Add(btnProfesores);
        mainPanel.Controls.Add(btnAlumnos);
        mainPanel.Controls.Add(btnMaterias); // Agregar al panel
        mainPanel.Controls.Add(btnCursos);
        mainPanel.Controls.Add(btnInscripciones);
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
        lblProximamente.Location = new Point(150, 450);
        lblProximamente.Name = "lblProximamente";
        lblProximamente.Size = new Size(200, 20);
        lblProximamente.TabIndex = 6;
        lblProximamente.Text = "Sistema Completo de Academia";
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
        btnPlanes.Location = new Point(25, 350);
        btnPlanes.Name = "btnPlanes";
        btnPlanes.Size = new Size(100, 50);
        btnPlanes.TabIndex = 3;
        btnPlanes.Text = "Gestión de Planes";
        btnPlanes.UseVisualStyleBackColor = true;
        // 
        // btnComisiones
        // 
        btnComisiones.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnComisiones.Location = new Point(135, 350);
        btnComisiones.Name = "btnComisiones";
        btnComisiones.Size = new Size(100, 50);
        btnComisiones.TabIndex = 4;
        btnComisiones.Text = "Gestión de Comisiones";
        btnComisiones.UseVisualStyleBackColor = true;
        // 
        // btnMaterias
        // 
        btnMaterias.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnMaterias.Location = new Point(245, 350);
        btnMaterias.Name = "btnMaterias";
        btnMaterias.Size = new Size(100, 50);
        btnMaterias.TabIndex = 8;
        btnMaterias.Text = "Gestión de Materias";
        btnMaterias.UseVisualStyleBackColor = true;
        // 
        // btnCursos
        // 
        btnCursos.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnCursos.Location = new Point(355, 350);
        btnCursos.Name = "btnCursos";
        btnCursos.Size = new Size(100, 50);
        btnCursos.TabIndex = 7;
        btnCursos.Text = "Gestión de Cursos";
        btnCursos.UseVisualStyleBackColor = true;
        // 
        // btnInscripciones
        // 
        btnInscripciones.Font = new Font("Arial Narrow", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnInscripciones.Location = new Point(150, 410);
        btnInscripciones.Name = "btnInscripciones";
        btnInscripciones.Size = new Size(200, 35);
        btnInscripciones.TabIndex = 9;
        btnInscripciones.Text = "📚 Gestión de Inscripciones";
        btnInscripciones.UseVisualStyleBackColor = true;
        btnInscripciones.BackColor = Color.LightGreen;
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
            comisionToolStripMenuItem,
            materiaToolStripMenuItem, // Agregar al menú
            inscripcionToolStripMenuItem
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
        // materiaToolStripMenuItem
        // 
        materiaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevaMateriaToolStripMenuItem, listarMateriasToolStripMenuItem });
        materiaToolStripMenuItem.Name = "materiaToolStripMenuItem";
        materiaToolStripMenuItem.Size = new Size(60, 20);
        materiaToolStripMenuItem.Text = "Materia";
        // 
        // nuevaMateriaToolStripMenuItem
        // 
        nuevaMateriaToolStripMenuItem.Name = "nuevaMateriaToolStripMenuItem";
        nuevaMateriaToolStripMenuItem.Size = new Size(152, 22);
        nuevaMateriaToolStripMenuItem.Text = "Nueva Materia";
        nuevaMateriaToolStripMenuItem.Click += nuevaMateriaToolStripMenuItem_Click;
        // 
        // listarMateriasToolStripMenuItem
        // 
        listarMateriasToolStripMenuItem.Name = "listarMateriasToolStripMenuItem";
        listarMateriasToolStripMenuItem.Size = new Size(152, 22);
        listarMateriasToolStripMenuItem.Text = "Listar Materias";
        listarMateriasToolStripMenuItem.Click += listarMateriasToolStripMenuItem_Click;
        // 
        // inscripcionToolStripMenuItem
        // 
        inscripcionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { gestionarInscripcionesToolStripMenuItem });
        inscripcionToolStripMenuItem.Name = "inscripcionToolStripMenuItem";
        inscripcionToolStripMenuItem.Size = new Size(79, 20);
        inscripcionToolStripMenuItem.Text = "Inscripción";
        // 
        // gestionarInscripcionesToolStripMenuItem
        // 
        gestionarInscripcionesToolStripMenuItem.Name = "gestionarInscripcionesToolStripMenuItem";
        gestionarInscripcionesToolStripMenuItem.Size = new Size(190, 22);
        gestionarInscripcionesToolStripMenuItem.Text = "Gestionar Inscripciones";
        gestionarInscripcionesToolStripMenuItem.Click += gestionarInscripcionesToolStripMenuItem_Click;
        // 
        // MenuPrincipal
        // 
        ClientSize = new Size(484, 497);
        Controls.Add(mainPanel);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        Name = "MenuPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sistema de Gestion";
        Load += MenuPrincipal_Load;
        mainPanel.ResumeLayout(false);
        mainPanel.PerformLayout();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}