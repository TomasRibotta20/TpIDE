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

            btnCursos.Click += (s, e) =>
            {
                try
                {
                    var formCursos = new FormCursos(this);
                    formCursos.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al abrir formulario de cursos: {ex.Message}", 
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

            // Evento para el botón de Inscripciones
            btnInscripciones.Click += (s, e) =>
            {
                try
                {
                    var formInscripciones = new FormInscripciones(this);
                    formInscripciones.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al abrir formulario de inscripciones: {ex.Message}", 
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

    // Nuevo método para el menú de inscripciones
    private void gestionarInscripcionesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var formInscripciones = new FormInscripciones(this);
            formInscripciones.Show();
            this.Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al abrir gestión de inscripciones: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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