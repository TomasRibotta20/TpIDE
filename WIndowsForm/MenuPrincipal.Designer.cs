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
}