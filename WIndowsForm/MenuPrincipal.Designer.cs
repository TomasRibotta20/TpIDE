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
    private Panel mainPanel;
    private Button btnUsuarios;
    private Label titleLabel;
    private Label lblProximamente;
    private Button btnOtroCrud;
    private Button btnEspecialidades;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem usuarioToolStripMenuItem;
    private ToolStripMenuItem nuevoUsuarioToolStripMenuItem;
    private ToolStripMenuItem listarUsuariosToolStripMenuItem;
    private ToolStripMenuItem especialidadToolStripMenuItem;
    private ToolStripMenuItem nuevaEspecialidadToolStripMenuItem;
    private ToolStripMenuItem listarEspecialidadesToolStripMenuItem;
}