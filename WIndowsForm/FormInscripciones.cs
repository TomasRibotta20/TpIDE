using API.Clients;
using DTOs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormInscripciones : Form
    {
        private readonly InscripcionApiClient _inscripcionApiClient;
        private readonly PersonaApiClient _personaApiClient;
        private readonly CursoApiClient _cursoApiClient;
        private readonly Form _menuPrincipal;
        
        private BindingList<PersonaDto> _alumnos = new BindingList<PersonaDto>();
        private BindingList<CursoDto> _cursos = new BindingList<CursoDto>();
        private BindingList<AlumnoCursoDto> _inscripcionesAlumno = new BindingList<AlumnoCursoDto>();
        
        private PersonaDto? _alumnoSeleccionado;

        public FormInscripciones(Form menuPrincipal = null)
        {
            InitializeComponent();
            _menuPrincipal = menuPrincipal;

            try
            {
                _inscripcionApiClient = new InscripcionApiClient();
                _personaApiClient = new PersonaApiClient();
                _cursoApiClient = new CursoApiClient();

                ConfigurarComponentes();
                
                this.Load += FormInscripciones_Load;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el sistema de inscripciones:\n\n{ex.Message}",
                    "Error de Inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarComponentes()
        {
            // Configurar lista de alumnos
            listBoxAlumnos.DataSource = _alumnos;
            listBoxAlumnos.DisplayMember = "NombreCompleto";
            listBoxAlumnos.ValueMember = "Id";
            listBoxAlumnos.SelectedIndexChanged += ListBoxAlumnos_SelectedIndexChanged;

            // Configurar grid de inscripciones del alumno
            dataGridViewInscripciones.AutoGenerateColumns = false;
            dataGridViewInscripciones.DataSource = _inscripcionesAlumno;
            
            dataGridViewInscripciones.Columns.Clear();
            dataGridViewInscripciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DescripcionCurso",
                HeaderText = "Curso",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dataGridViewInscripciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Condicion",
                HeaderText = "Condición",
                Width = 100
            });
            dataGridViewInscripciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nota",
                HeaderText = "Nota",
                Width = 60
            });

            // Configurar búsqueda
            txtBuscarAlumno.TextChanged += TxtBuscarAlumno_TextChanged;

            // Configurar botones
            btnVolver.Click += (s, e) => VolverAlMenu();
            btnDesinscribir.Click += BtnDesinscribir_Click;
            btnEditarCondicion.Click += BtnEditarCondicion_Click;
            btnReporteCursos.Click += BtnReporteCursos_Click;
        }

        private void TxtBuscarAlumno_TextChanged(object sender, EventArgs e)
        {
            var filtro = txtBuscarAlumno.Text.ToLower();
            var alumnosFiltrados = _alumnos.Where(a => 
                a.NombreCompleto.ToLower().Contains(filtro) || 
                a.Legajo.ToString().Contains(filtro)
            ).ToList();

            listBoxAlumnos.DataSource = new BindingList<PersonaDto>(alumnosFiltrados);
        }

        private async void FormInscripciones_Load(object sender, EventArgs e)
        {
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                // Cargar alumnos
                var personas = await _personaApiClient.GetAllAlumnosAsync();
                _alumnos.Clear();
                foreach (var persona in personas)
                {
                    _alumnos.Add(persona);
                }

                // Cargar cursos
                var cursos = await _cursoApiClient.GetAllAsync();
                _cursos.Clear();
                panelCursos.Controls.Clear();
                
                int x = 10, y = 10;
                foreach (var curso in cursos)
                {
                    _cursos.Add(curso);
                    CrearCardCurso(curso, x, y);
                    
                    x += 220; // Ancho del card + margen
                    if (x > panelCursos.Width - 200)
                    {
                        x = 10;
                        y += 140; // Alto del card + margen
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void CrearCardCurso(CursoDto curso, int x, int y)
        {
            var card = new Panel
            {
                Size = new Size(200, 120),
                Location = new Point(x, y),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Tag = curso
            };

            // Color según disponibilidad
            var disponibles = curso.Cupo - (curso.InscriptosActuales ?? 0);
            if (disponibles <= 0)
            {
                card.BackColor = Color.LightCoral; // Sin cupo
            }
            else if (disponibles <= 5)
            {
                card.BackColor = Color.LightYellow; // Poco cupo
            }
            else
            {
                card.BackColor = Color.LightGreen; // Cupo disponible
            }

            // Título del curso
            var lblTitulo = new Label
            {
                Text = $"Curso #{curso.IdCurso}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(5, 5),
                Size = new Size(190, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblTitulo);

            // Información del curso
            var lblInfo = new Label
            {
                Text = $"Materia: {curso.NombreMateria}\nComisión: {curso.DescComision}\nAño: {curso.AnioCalendario}",
                Location = new Point(5, 25),
                Size = new Size(190, 60),
                Font = new Font("Arial", 8)
            };
            card.Controls.Add(lblInfo);

            // Cupo disponible
            var lblCupo = new Label
            {
                Text = $"Cupo: {curso.InscriptosActuales ?? 0}/{curso.Cupo}",
                Location = new Point(5, 85),
                Size = new Size(190, 20),
                Font = new Font("Arial", 9, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblCupo);

            // Evento click para inscribir
            card.Click += async (s, e) => await InscribirAlumnoACurso(curso);
            lblTitulo.Click += async (s, e) => await InscribirAlumnoACurso(curso);
            lblInfo.Click += async (s, e) => await InscribirAlumnoACurso(curso);
            lblCupo.Click += async (s, e) => await InscribirAlumnoACurso(curso);

            // Efecto hover
            card.MouseEnter += (s, e) => {
                card.BackColor = Color.FromArgb(200, card.BackColor.R, card.BackColor.G, card.BackColor.B);
            };
            card.MouseLeave += (s, e) => {
                if (disponibles <= 0)
                    card.BackColor = Color.LightCoral;
                else if (disponibles <= 5)
                    card.BackColor = Color.LightYellow;
                else
                    card.BackColor = Color.LightGreen;
            };

            panelCursos.Controls.Add(card);
        }

        private async void ListBoxAlumnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxAlumnos.SelectedItem is PersonaDto alumno)
            {
                _alumnoSeleccionado = alumno;
                lblAlumnoSeleccionado.Text = $"Alumno: {alumno.NombreCompleto} (Legajo: {alumno.Legajo})";
                
                await CargarInscripcionesAlumno(alumno.Id);
            }
        }

        private async Task CargarInscripcionesAlumno(int idAlumno)
        {
            try
            {
                var inscripciones = await _inscripcionApiClient.GetInscripcionesByAlumnoAsync(idAlumno);
                _inscripcionesAlumno.Clear();
                foreach (var inscripcion in inscripciones)
                {
                    _inscripcionesAlumno.Add(inscripcion);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar inscripciones: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task InscribirAlumnoACurso(CursoDto curso)
        {
            if (_alumnoSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un alumno primero", "Selección Requerida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"¿Confirma la inscripción de {_alumnoSeleccionado.NombreCompleto} al Curso #{curso.IdCurso}?\n\n" +
                $"Materia: {curso.NombreMateria}\n" +
                $"Comisión: {curso.DescComision}\n" +
                $"Año: {curso.AnioCalendario}",
                "Confirmar Inscripción",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _inscripcionApiClient.InscribirAlumnoAsync(_alumnoSeleccionado.Id, curso.IdCurso);
                    
                    MessageBox.Show($"¡Inscripción exitosa!\n\n" +
                                  $"Alumno: {_alumnoSeleccionado.NombreCompleto}\n" +
                                  $"Curso: {curso.NombreMateria}", 
                                  "Inscripción Completada",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Recargar datos
                    await CargarInscripcionesAlumno(_alumnoSeleccionado.Id);
                    await CargarDatosAsync(); // Recargar cards para actualizar cupos
                }
                catch (Exception ex)
                {
                    string mensajeError = InterpretarErrorInscripcion(ex.Message);
                    MessageBox.Show(mensajeError, "Error en la Inscripción",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private string InterpretarErrorInscripcion(string errorOriginal)
        {
            string errorLower = errorOriginal.ToLower();
            
            // Casos específicos con códigos de error
            switch (errorOriginal)
            {
                case "ALUMNO_YA_INSCRIPTO":
                    return "El alumno ya está inscripto en este curso.\n\n" +
                           "No se puede inscribir un alumno dos veces al mismo curso.\n\n" +
                           "Verifique la lista de inscripciones del alumno.";
                case "CUPO_AGOTADO":
                    return "No hay cupo disponible en este curso.\n\n" +
                           "El curso está completo. Por favor:\n" +
                           "- Seleccione otro curso, o\n" +
                           "- Espere que se libere un lugar.";
                case "CURSO_AÑO_ANTERIOR":
                    return "No se puede inscribir a cursos de años anteriores.\n\n" +
                           "Solo es posible inscribirse a cursos del año actual o futuro.";
                case "CURSO_O_ALUMNO_NO_ENCONTRADO":
                    return "El curso o alumno especificado no existe.\n\n" +
                           "Esto puede deberse a que:\n" +
                           "- Los datos fueron eliminados\n" +
                           "- La sesión expiró\n\n" +
                           "Por favor, actualice la página e intente nuevamente.";
                case "DATOS_INVALIDOS":
                    return "Los datos para la inscripción no son válidos.\n\n" +
                           "Por favor verifique:\n" +
                           "- Que el alumno esté seleccionado\n" +
                           "- Que el curso sea válido";
                case "ERROR_SERVIDOR":
                    return "Error interno del servidor.\n\n" +
                           "Por favor:\n" +
                           "1. Intente nuevamente en unos momentos\n" +
                           "2. Si el problema persiste, contacte al administrador";
            }
            
            // Si el mensaje ya viene formateado del servidor, usarlo directamente
            if (!errorOriginal.Contains("Exception") && 
                !errorOriginal.Contains("System.") && 
                !errorOriginal.Contains("Stack") &&
                errorOriginal.Contains("\n"))
            {
                return errorOriginal;
            }
            
            // Detección por contenido del mensaje para casos no manejados
            if (errorLower.Contains("ya está inscripto") || 
                errorLower.Contains("already enrolled") || 
                errorLower.Contains("duplicate"))
            {
                return "El alumno ya está inscripto en este curso.\n\n" +
                       "No se puede inscribir un alumno dos veces al mismo curso.\n\n" +
                       "Verifique la lista de inscripciones del alumno.";
            }
            
            if (errorLower.Contains("cupo") || 
                errorLower.Contains("capacity") || 
                errorLower.Contains("completo") || 
                errorLower.Contains("lleno"))
            {
                return "No hay cupo disponible en este curso.\n\n" +
                       "El curso está completo. Por favor:\n" +
                       "- Seleccione otro curso, o\n" +
                       "- Espere que se libere un lugar.";
            }
            
            if (errorLower.Contains("no existe") || 
                errorLower.Contains("not found") || 
                errorLower.Contains("404"))
            {
                return "El curso o alumno especificado no existe.\n\n" +
                       "Por favor, actualice la página e intente nuevamente.";
            }
            
            if (errorLower.Contains("año anterior") || 
                errorLower.Contains("year") || 
                errorLower.Contains("calendario") || 
                errorLower.Contains("pasado"))
            {
                return "No se puede inscribir a cursos de años anteriores.\n\n" +
                       "Solo es posible inscribirse a cursos del año actual o futuro.";
            }
            
            if (errorLower.Contains("timeout") || errorLower.Contains("connection"))
            {
                return "Error de conexión con el servidor.\n\n" +
                       "Por favor:\n" +
                       "1. Verifique su conexión a internet\n" +
                       "2. Intente nuevamente en unos momentos";
            }
            
            // Error genérico pero amigable
            return "No se pudo completar la inscripción.\n\n" +
                   "Por favor:\n" +
                   "1. Verifique los datos ingresados\n" +
                   "2. Intente nuevamente\n" +
                   "3. Si el problema persiste, contacte al administrador\n\n" +
                   "Detalle técnico: " + (errorOriginal.Length > 100 ? errorOriginal.Substring(0, 100) + "..." : errorOriginal);
        }

        private async void BtnReporteCursos_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear formulario de selección de reporte personalizado
                using (var formSeleccion = new FormSeleccionReporte())
                {
                    var resultado = formSeleccion.ShowDialog();
                    
                    if (resultado == DialogResult.OK)
                    {
                        switch (formSeleccion.TipoReporteSeleccionado)
                        {
                            case TipoReporte.Rapido:
                                await MostrarReporteRapido();
                                break;
                            case TipoReporte.Detallado:
                                var formReporte = new FormReporteCursos();
                                formReporte.ShowDialog();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir reportes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task MostrarReporteRapido()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                // Obtener estadísticas generales
                var estadisticas = await _inscripcionApiClient.GetEstadisticasGeneralesAsync();
                
                var mensaje = "REPORTE RÁPIDO DE INSCRIPCIONES\n\n";
                mensaje += $"Total de inscripciones: {estadisticas.GetValueOrDefault("TotalInscripciones", 0)}\n\n";
                mensaje += "DISTRIBUCIÓN POR CONDICIÓN:\n";
                mensaje += $"Promocionales: {estadisticas.GetValueOrDefault("AlumnosPromocionales", 0)} (Excelente rendimiento)\n";
                mensaje += $"Regulares: {estadisticas.GetValueOrDefault("AlumnosRegulares", 0)} (Buen encaminamiento)\n";
                mensaje += $"Libres: {estadisticas.GetValueOrDefault("AlumnosLibres", 0)} (Necesitan apoyo)\n\n";
                mensaje += "RESUMEN GENERAL:\n";
                mensaje += $"Total de cursos activos: {_cursos.Count}\n";
                mensaje += $"Total de alumnos: {_alumnos.Count}";
                
                MessageBox.Show(mensaje, "Reporte Rápido - Sistema de Inscripciones", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar reporte: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async Task MostrarEstadisticasAvanzadas()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                var estadisticas = await _inscripcionApiClient.GetEstadisticasGeneralesAsync();
                
                // Calcular estadísticas adicionales
                var cursosConCupo = _cursos.Count(c => (c.Cupo - (c.InscriptosActuales ?? 0)) > 0);
                var cursosCompletos = _cursos.Count(c => (c.Cupo - (c.InscriptosActuales ?? 0)) <= 0);
                var promedioInscriptosPorCurso = _cursos.Any() ? _cursos.Average(c => c.InscriptosActuales ?? 0) : 0;
                
                var mensaje = "ESTADÍSTICAS AVANZADAS DEL SISTEMA\n\n";
                mensaje += "INSCRIPCIONES POR CONDICIÓN:\n";
                mensaje += $"   Promocionales: {estadisticas.GetValueOrDefault("AlumnosPromocionales", 0)} (Excelente!)\n";
                mensaje += $"   Regulares: {estadisticas.GetValueOrDefault("AlumnosRegulares", 0)} (Bien encaminados)\n";
                mensaje += $"   Libres: {estadisticas.GetValueOrDefault("AlumnosLibres", 0)} (Necesitan apoyo)\n";
                mensaje += $"   Total: {estadisticas.GetValueOrDefault("TotalInscripciones", 0)}\n\n";
                mensaje += "ESTADO DE LOS CURSOS:\n";
                mensaje += $"   Total de cursos: {_cursos.Count}\n";
                mensaje += $"   Con cupo disponible: {cursosConCupo}\n";
                mensaje += $"   Cursos completos: {cursosCompletos}\n";
                mensaje += $"   Promedio inscriptos por curso: {promedioInscriptosPorCurso:F1}\n\n";
                mensaje += "INFORMACIÓN GENERAL:\n";
                mensaje += $"   Total de alumnos registrados: {_alumnos.Count}\n";
                
                if (_cursos.Any())
                {
                    var cursoMasPopular = _cursos.OrderByDescending(c => c.InscriptosActuales ?? 0).First();
                    mensaje += $"   Curso más popular: {cursoMasPopular.NombreMateria} ({cursoMasPopular.InscriptosActuales ?? 0} inscriptos)\n";
                    
                    var cursoMenosPopular = _cursos.OrderBy(c => c.InscriptosActuales ?? 0).First();
                    mensaje += $"   Curso con menos inscriptos: {cursoMenosPopular.NombreMateria} ({cursoMenosPopular.InscriptosActuales ?? 0} inscriptos)";
                }
                
                MessageBox.Show(mensaje, "Estadísticas Avanzadas - Sistema Académico", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener estadísticas: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async void BtnDesinscribir_Click(object sender, EventArgs e)
        {
            if (dataGridViewInscripciones.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar una inscripción de la lista para poder eliminarla.", 
                    "Selección Requerida",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var inscripcion = (AlumnoCursoDto)dataGridViewInscripciones.SelectedRows[0].DataBoundItem;
            
            var confirmResult = MessageBox.Show(
                $"¿Confirma que desea desinscribir al alumno?\n\n" +
                $"Alumno: {inscripcion.NombreAlumno} {inscripcion.ApellidoAlumno}\n" +
                $"Curso: {inscripcion.DescripcionCurso}\n\n" +
                $"Esta acción no se puede deshacer.",
                "Confirmar Desinscripción",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _inscripcionApiClient.DesinscribirAlumnoAsync(inscripcion.IdInscripcion);
                    
                    MessageBox.Show($"Desinscripción completada exitosamente.\n\n" +
                                  $"El alumno {inscripcion.NombreAlumno} {inscripcion.ApellidoAlumno} " +
                                  $"ha sido desinscripto del curso.", 
                                  "Desinscripción Exitosa",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    await CargarInscripcionesAlumno(_alumnoSeleccionado!.Id);
                    await CargarDatosAsync();
                }
                catch (Exception ex)
                {
                    string mensajeError = InterpretarErrorDesinscripcion(ex.Message);
                    MessageBox.Show(mensajeError, "Error al Desinscribir",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private string InterpretarErrorDesinscripcion(string errorOriginal)
        {
            string errorLower = errorOriginal.ToLower();
            
            // Casos específicos con códigos de error
            switch (errorOriginal)
            {
                case "INSCRIPCION_NO_ENCONTRADA":
                    return "La inscripción ya no existe en el sistema.\n\nEs posible que haya sido eliminada previamente. La lista se actualizará automáticamente.";
                case "NO_SE_PUEDE_DESINSCRIBIR":
                    return "No se puede desinscribir al alumno.\n\nPosibles causas:\n- Ya tiene calificaciones registradas\n- Restricciones del sistema\n\nContacte al administrador.";
                case "ERROR_SERVIDOR":
                    return "Error interno del servidor.\n\nIntente nuevamente en unos momentos. Si el problema persiste, contacte al administrador.";
            }
            
            // Detección por contenido del mensaje
            if (errorLower.Contains("not found") || errorLower.Contains("404") || errorLower.Contains("no existe"))
            {
                return "La inscripción ya no existe en el sistema.\n\nEs posible que haya sido eliminada previamente. La lista se actualizará automáticamente.";
            }
            
            if (errorLower.Contains("calificaciones") || errorLower.Contains("notas") || errorLower.Contains("evaluaciones"))
            {
                return "No se puede desinscribir al alumno porque ya tiene calificaciones registradas.\n\nContacte al administrador para gestionar esta situación.";
            }
            
            if (errorLower.Contains("400") || errorLower.Contains("bad request"))
            {
                return "No se puede desinscribir al alumno en este momento.\n\nVerifique que la inscripción sea válida.";
            }
            
            if (errorLower.Contains("500") || errorLower.Contains("internal server"))
            {
                return "Error interno del servidor.\n\nIntente nuevamente en unos momentos. Si persiste, contacte al administrador.";
            }
            
            return $"Error inesperado al desinscribir al alumno.\n\nIntente nuevamente. Si el problema persiste, contacte al administrador.\n\nDetalle: {(errorOriginal.Length > 100 ? errorOriginal.Substring(0, 100) + "..." : errorOriginal)}";
        }

        private async void BtnEditarCondicion_Click(object sender, EventArgs e)
        {
            if (dataGridViewInscripciones.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar una inscripción de la lista para poder editarla.", 
                    "Selección Requerida",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var inscripcion = (AlumnoCursoDto)dataGridViewInscripciones.SelectedRows[0].DataBoundItem;
            
            try
            {
                var formEditar = new EditarCondicionForm(inscripcion);
                if (formEditar.ShowDialog() == DialogResult.OK)
                {
                    await CargarInscripcionesAlumno(_alumnoSeleccionado!.Id);
                    
                    MessageBox.Show("La condición del alumno ha sido actualizada correctamente.", 
                        "Actualización Exitosa",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el editor de condiciones:\n\n{ex.Message}", 
                    "Error de Sistema",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VolverAlMenu()
        {
            try
            {
                if (_menuPrincipal != null)
                {
                    _menuPrincipal.Show();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al volver al menú: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}