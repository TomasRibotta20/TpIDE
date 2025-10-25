using System;
using System.Drawing;
using System.Windows.Forms;
using API.Clients;
using DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace WIndowsForm
{
    public partial class FormReporteCursos : Form
    {
        private readonly InscripcionApiClient _inscripcionApiClient;
        private readonly CursoApiClient _cursoApiClient;
        private CursoDto? _cursoSeleccionado;
        private List<AlumnoCursoDto> _inscripcionesCursoSeleccionado = new List<AlumnoCursoDto>();
        
        public FormReporteCursos()
        {
            InitializeComponent();
            _inscripcionApiClient = new InscripcionApiClient();
            _cursoApiClient = new CursoApiClient();
            
            this.Load += FormReporteCursos_Load;
        }

        private async void FormReporteCursos_Load(object sender, EventArgs e)
        {
            await CargarReporteAsync();
        }

        private async Task CargarReporteAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                // Obtener cursos
                var cursos = await _cursoApiClient.GetAllAsync();
                
                // Cargar cursos en el grid
                var cursosList = new List<object>();
                foreach (var curso in cursos)
                {
                    var disponible = curso.Cupo - (curso.InscriptosActuales ?? 0);
                    var porcentajeOcupacion = curso.Cupo > 0 ? ((curso.InscriptosActuales ?? 0) * 100.0 / curso.Cupo) : 0;
                    
                    cursosList.Add(new
                    {
                        IdCurso = curso.IdCurso,
                        Materia = curso.NombreMateria ?? "Sin Materia",
                        Comision = curso.DescComision ?? "Sin Comisión",
                        Año = curso.AnioCalendario,
                        Cupo = curso.Cupo,
                        Inscriptos = curso.InscriptosActuales ?? 0,
                        Disponible = disponible,
                        PorcentajeOcupacion = $"{porcentajeOcupacion:F1}%",
                        Estado = disponible <= 0 ? "Completo" : disponible <= 5 ? "Casi Lleno" : "Disponible"
                    });
                }
                
                dataGridViewCursos.DataSource = cursosList;
                
                // Configurar evento de selección de curso
                dataGridViewCursos.SelectionChanged += DataGridViewCursos_SelectionChanged;
                
                // Configurar colores de las filas según disponibilidad
                ConfigurarColoresGrid();
                
                // Seleccionar el primer curso por defecto
                if (cursos.Any())
                {
                    _cursoSeleccionado = cursos.First();
                    await ActualizarInformacionCursoSeleccionado();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar reporte: {ex.Message}");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async void DataGridViewCursos_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCursos.SelectedRows.Count > 0)
            {
                var row = dataGridViewCursos.SelectedRows[0];
                var idCurso = (int)row.Cells["IdCurso"].Value;
                
                try
                {
                    var cursos = await _cursoApiClient.GetAllAsync();
                    _cursoSeleccionado = cursos.FirstOrDefault(c => c.IdCurso == idCurso);
                    
                    if (_cursoSeleccionado != null)
                    {
                        await ActualizarInformacionCursoSeleccionado();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar datos del curso: {ex.Message}");
                }
            }
        }

        private async Task ActualizarInformacionCursoSeleccionado()
        {
            if (_cursoSeleccionado == null) return;

            try
            {
                // Obtener inscripciones del curso seleccionado
                _inscripcionesCursoSeleccionado = (await _inscripcionApiClient.GetInscripcionesByCursoAsync(_cursoSeleccionado.IdCurso)).ToList();
                
                // Actualizar estadísticas del curso
                var totalInscriptos = _inscripcionesCursoSeleccionado.Count;
                var disponibles = _cursoSeleccionado.Cupo - totalInscriptos;
                var promocionales = _inscripcionesCursoSeleccionado.Count(i => i.Condicion == CondicionAlumnoDto.Promocional);
                var regulares = _inscripcionesCursoSeleccionado.Count(i => i.Condicion == CondicionAlumnoDto.Regular);
                var libres = _inscripcionesCursoSeleccionado.Count(i => i.Condicion == CondicionAlumnoDto.Libre);

                // Actualizar labels con información del curso seleccionado
                lblTotalInscripciones.Text = $"Inscriptos en este curso: {totalInscriptos}";
                lblPromocionales.Text = $"Promocionales: {promocionales}";
                lblRegulares.Text = $"Regulares: {regulares}";
                lblLibres.Text = $"Libres: {libres}";
                
                // Actualizar título con información del curso
                lblTitulo.Text = $"REPORTE DEL CURSO: {_cursoSeleccionado.NombreMateria ?? "Sin Materia"} - {_cursoSeleccionado.DescComision ?? "Sin Comisión"}";
                
                // Crear label para cupos disponibles si no existe
                if (this.Controls.Find("lblCuposDisponibles", true).Length == 0)
                {
                    var lblCuposDisponibles = new Label
                    {
                        Name = "lblCuposDisponibles",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        ForeColor = disponibles > 0 ? Color.Green : Color.Red,
                        Location = new Point(800, 50),
                        Size = new Size(200, 25),
                        Text = $"Cupos disponibles: {disponibles}"
                    };
                    panelEstadisticas.Controls.Add(lblCuposDisponibles);
                }
                else
                {
                    var lblCuposDisponibles = this.Controls.Find("lblCuposDisponibles", true)[0] as Label;
                    if (lblCuposDisponibles != null)
                    {
                        lblCuposDisponibles.Text = $"Cupos disponibles: {disponibles}";
                        lblCuposDisponibles.ForeColor = disponibles > 0 ? Color.Green : Color.Red;
                    }
                }
                
                // Redibujar gráficos
                DibujarGraficoCondicionesPorCurso();
                DibujarGraficoOcupacionCurso(_cursoSeleccionado);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar información del curso: {ex.Message}");
            }
        }

        private void ConfigurarColoresGrid()
        {
            dataGridViewCursos.CellFormatting += (sender, e) => {
                if (e.ColumnIndex == dataGridViewCursos.Columns["Estado"].Index && e.Value != null)
                {
                    var estado = e.Value.ToString();
                    switch (estado)
                    {
                        case "Completo":
                            e.CellStyle.BackColor = Color.LightCoral;
                            e.CellStyle.ForeColor = Color.DarkRed;
                            break;
                        case "Casi Lleno":
                            e.CellStyle.BackColor = Color.LightYellow;
                            e.CellStyle.ForeColor = Color.DarkOrange;
                            break;
                        case "Disponible":
                            e.CellStyle.BackColor = Color.LightGreen;
                            e.CellStyle.ForeColor = Color.DarkGreen;
                            break;
                    }
                }
            };
        }

        private void DibujarGraficoCondicionesPorCurso()
        {
            if (_inscripcionesCursoSeleccionado == null || !_inscripcionesCursoSeleccionado.Any())
            {
                using (var g = panelGraficoCondiciones.CreateGraphics())
                {
                    g.Clear(Color.White);
                    g.DrawString("No hay inscriptos en este curso", new Font("Segoe UI", 12), Brushes.Gray, 
                        panelGraficoCondiciones.Width / 2 - 100, panelGraficoCondiciones.Height / 2);
                }
                return;
            }

            var promocionales = _inscripcionesCursoSeleccionado.Count(i => i.Condicion == CondicionAlumnoDto.Promocional);
            var regulares = _inscripcionesCursoSeleccionado.Count(i => i.Condicion == CondicionAlumnoDto.Regular);
            var libres = _inscripcionesCursoSeleccionado.Count(i => i.Condicion == CondicionAlumnoDto.Libre);
            var total = promocionales + regulares + libres;

            using (var g = panelGraficoCondiciones.CreateGraphics())
            {
                g.Clear(Color.White);
                
                var rect = panelGraficoCondiciones.ClientRectangle;
                var margen = 20;
                var anchoDisponible = rect.Width - (margen * 2);
                var altoDisponible = rect.Height - (margen * 2);
                
                if (total == 0) return;
                
                // Calcular alturas proporcionales
                var maxAltura = altoDisponible - 40; // Espacio para labels
                var alturaPromocional = promocionales > 0 ? Math.Max(20, (int)(maxAltura * promocionales / (double)total)) : 0;
                var alturaRegular = regulares > 0 ? Math.Max(20, (int)(maxAltura * regulares / (double)total)) : 0;
                var alturaLibre = libres > 0 ? Math.Max(20, (int)(maxAltura * libres / (double)total)) : 0;
                
                var anchoBarra = Math.Min(80, anchoDisponible / 4);
                var spacing = (anchoDisponible - (anchoBarra * 3)) / 4;
                
                // Dibujar barras con colores correctos
                var baseY = rect.Height - margen - 20;
                
                // Promocional = Verde (lo mejor)
                if (promocionales > 0)
                {
                    var x = margen + spacing;
                    var rectPromocional = new Rectangle(x, baseY - alturaPromocional, anchoBarra, alturaPromocional);
                    g.FillRectangle(Brushes.Green, rectPromocional);
                    g.DrawRectangle(Pens.DarkGreen, rectPromocional);
                    g.DrawString($"Promocional\n{promocionales}", new Font("Segoe UI", 8), Brushes.Black, x, baseY + 5);
                }
                
                // Regular = Azul (intermedio)
                if (regulares > 0)
                {
                    var x = margen + spacing * 2 + anchoBarra;
                    var rectRegular = new Rectangle(x, baseY - alturaRegular, anchoBarra, alturaRegular);
                    g.FillRectangle(Brushes.CornflowerBlue, rectRegular);
                    g.DrawRectangle(Pens.DarkBlue, rectRegular);
                    g.DrawString($"Regular\n{regulares}", new Font("Segoe UI", 8), Brushes.Black, x, baseY + 5);
                }
                
                // Libre = Naranja (necesita mejorar)
                if (libres > 0)
                {
                    var x = margen + spacing * 3 + anchoBarra * 2;
                    var rectLibre = new Rectangle(x, baseY - alturaLibre, anchoBarra, alturaLibre);
                    g.FillRectangle(Brushes.Orange, rectLibre);
                    g.DrawRectangle(Pens.DarkOrange, rectLibre);
                    g.DrawString($"Libre\n{libres}", new Font("Segoe UI", 8), Brushes.Black, x, baseY + 5);
                }
                
                // Título del gráfico
                g.DrawString("Distribución de Condiciones en este Curso", new Font("Segoe UI", 10, FontStyle.Bold), Brushes.Black, margen, 5);
            }
        }

        private void DibujarGraficoOcupacionCurso(CursoDto curso)
        {
            using (var g = panelGraficoOcupacion.CreateGraphics())
            {
                g.Clear(Color.White);
                
                var rect = panelGraficoOcupacion.ClientRectangle;
                var margen = 20;
                
                if (curso == null)
                {
                    g.DrawString("Seleccione un curso para ver su ocupación", new Font("Segoe UI", 12), Brushes.Gray, 
                        rect.Width / 2 - 120, rect.Height / 2);
                    return;
                }
                
                var inscriptos = curso.InscriptosActuales ?? 0;
                var cupoTotal = curso.Cupo;
                var disponible = cupoTotal - inscriptos;
                
                if (cupoTotal <= 0)
                {
                    g.DrawString("El curso no tiene cupo definido", new Font("Segoe UI", 12), Brushes.Red, 
                        rect.Width / 2 - 100, rect.Height / 2);
                    return;
                }
                
                // Calcular ángulos para el gráfico de torta
                var anguloInscriptos = 360f * inscriptos / cupoTotal;
                var anguloDisponible = 360f - anguloInscriptos;
                
                // Centro y radio del gráfico
                var centerX = rect.Width / 2;
                var centerY = rect.Height / 2 + 15; // Desplazar para hacer espacio al título
                var radius = Math.Min(centerX, centerY) - margen - 40; // Espacio para leyenda
                
                var rectCircle = new Rectangle(centerX - radius, centerY - radius, radius * 2, radius * 2);
                
                // Dibujar porción de inscriptos (azul)
                if (inscriptos > 0)
                {
                    g.FillPie(Brushes.CornflowerBlue, rectCircle, 0, anguloInscriptos);
                    g.DrawPie(Pens.DarkBlue, rectCircle, 0, anguloInscriptos);
                }
                
                // Dibujar porción disponible (verde claro)
                if (disponible > 0)
                {
                    g.FillPie(Brushes.LightGreen, rectCircle, anguloInscriptos, anguloDisponible);
                    g.DrawPie(Pens.DarkGreen, rectCircle, anguloInscriptos, anguloDisponible);
                }
                
                // Si no hay disponibles, mostrar todo como ocupado
                if (disponible <= 0 && inscriptos > 0)
                {
                    g.FillPie(Brushes.Red, rectCircle, 0, 360f);
                    g.DrawPie(Pens.DarkRed, rectCircle, 0, 360f);
                }
                
                // Título del gráfico
                g.DrawString("Ocupación de Cupos", new Font("Segoe UI", 10, FontStyle.Bold), Brushes.Black, 
                    centerX - 60, 5);
                
                // Información detallada en la parte inferior
                var infoY = centerY + radius + 15;
                var infoFont = new Font("Segoe UI", 9);
                var boldFont = new Font("Segoe UI", 9, FontStyle.Bold);
                
                // Columna izquierda
                g.DrawString($"Inscriptos: {inscriptos}", infoFont, Brushes.DarkBlue, 10, infoY);
                g.DrawString($"Disponible: {disponible}", infoFont, Brushes.DarkGreen, 10, infoY + 18);
                g.DrawString($"Cupo Total: {cupoTotal}", boldFont, Brushes.Black, 10, infoY + 36);
                
                // Columna derecha con porcentajes y estado
                var porcentaje = cupoTotal > 0 ? (inscriptos * 100.0 / cupoTotal) : 0;
                var colorPorcentaje = porcentaje >= 100 ? Brushes.Red : 
                                   porcentaje >= 80 ? Brushes.Orange : 
                                   porcentaje >= 60 ? Brushes.DarkOrange : Brushes.Green;
                
                g.DrawString($"Ocupación: {porcentaje:F1}%", boldFont, colorPorcentaje, rect.Width - 150, infoY);
                
                string estado = disponible <= 0 ? "COMPLETO" : 
                               disponible <= 5 ? "CASI LLENO" : "DISPONIBLE";
                var colorEstado = disponible <= 0 ? Brushes.Red : 
                                 disponible <= 5 ? Brushes.Orange : Brushes.Green;
                
                g.DrawString($"Estado: {estado}", boldFont, colorEstado, rect.Width - 150, infoY + 18);
                
                // Leyenda visual con cuadrados de colores
                var leyendaY = infoY + 40;
                g.FillRectangle(Brushes.CornflowerBlue, 10, leyendaY, 15, 15);
                g.DrawString("Inscriptos", new Font("Segoe UI", 8), Brushes.Black, 30, leyendaY + 2);
                
                g.FillRectangle(Brushes.LightGreen, 10, leyendaY + 20, 15, 15);
                g.DrawString("Disponible", new Font("Segoe UI", 8), Brushes.Black, 30, leyendaY + 22);
                
                if (disponible <= 0)
                {
                    g.FillRectangle(Brushes.Red, 120, leyendaY, 15, 15);
                    g.DrawString("Sin Cupo", new Font("Segoe UI", 8), Brushes.Black, 140, leyendaY + 2);
                }
            }
        }
    }
}