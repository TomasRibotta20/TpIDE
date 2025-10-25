using API.Clients;
using DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace WIndowsForm
{
    public partial class FormReportePlanes : Form
    {
        private readonly PlanApiClient _planApiClient;
        private readonly EspecialidadApiClient _especialidadApiClient;
        private readonly ComisionApiClient _comisionApiClient;
        private readonly CursoApiClient _cursoApiClient;
        
        private DataGridView dgvPlanes;
        private Label lblTitulo;
        private Label lblEstadisticas;
        private Panel panelGrafico;
        private Button btnExportar;
        private Button btnCerrar;
        private Button btnActualizar;
        private ComboBox cboFiltroEspecialidad;
        private Label lblFiltro;
        private List<PlanDto> _todosLosPlanes = new List<PlanDto>();
        private List<EspecialidadDto> _todasLasEspecialidades = new List<EspecialidadDto>();
        private List<ComisionDto> _todasLasComisiones = new List<ComisionDto>();
        private List<CursoDto> _todosLosCursos = new List<CursoDto>();
        private bool _estaCargando = false;

        public FormReportePlanes()
        {
            _planApiClient = new PlanApiClient();
            _especialidadApiClient = new EspecialidadApiClient();
            _comisionApiClient = new ComisionApiClient();
            _cursoApiClient = new CursoApiClient();
            
            InitializeComponent();
            _ = CargarReporteAsync();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Size = new Size(1400, 800);
            this.Text = "Reporte de Planes de Estudio - Sistema Académico";
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 244, 248),
                Padding = new Padding(25)
            };

            // Header con título
            lblTitulo = new Label
            {
                Text = "Reporte Detallado de Planes de Estudio",
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            // Filtro por especialidad
            lblFiltro = new Label
            {
                Text = "Filtrar por Especialidad:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(30, 80),
                AutoSize = true,
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            cboFiltroEspecialidad = new ComboBox
            {
                Location = new Point(220, 77),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboFiltroEspecialidad.SelectedIndexChanged += (s, e) => FiltrarPorEspecialidad();

            // Estadísticas - Movidas más arriba y al lado izquierdo
            lblEstadisticas = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9),
                Location = new Point(30, 120),
                Size = new Size(400, 140),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            // Panel de gráfico - Al lado de las estadísticas
            panelGrafico = new Panel
            {
                Location = new Point(450, 120),
                Size = new Size(900, 220),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panelGrafico.Paint += PanelGrafico_Paint;

            // Panel con scroll horizontal para el DataGridView - Movido más abajo
            var panelContenedorGrid = new Panel
            {
                Location = new Point(30, 360),
                Size = new Size(1320, 300),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };

            // DataGridView de planes con ancho total aumentado para todas las columnas
            dgvPlanes = new DataGridView
            {
                Location = new Point(0, 0),
                Size = new Size(1500, 280),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9),
                RowHeadersVisible = false,
                RowTemplate = { Height = 35 },
                ScrollBars = ScrollBars.Both,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(245, 248, 250)
                }
            };
            dgvPlanes.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Padding = new Padding(5)
            };
            dgvPlanes.ColumnHeadersHeight = 40;
            dgvPlanes.EnableHeadersVisualStyles = false;
            dgvPlanes.SelectionChanged += DgvPlanes_SelectionChanged;

            // Agregar el grid al panel contenedor
            panelContenedorGrid.Controls.Add(dgvPlanes);

            // Botones
            btnActualizar = new Button
            {
                Text = "Actualizar",
                Size = new Size(150, 45),
                Location = new Point(30, 680),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.Click += async (s, e) => await CargarReporteAsync();

            btnExportar = new Button
            {
                Text = "Exportar a PDF (Proximamente)",
                Size = new Size(280, 45),
                Location = new Point(200, 680),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnExportar.FlatAppearance.BorderSize = 0;

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Size = new Size(150, 45),
                Location = new Point(1200, 680),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => this.Close();

            // Agregar controles al panel principal
            panelPrincipal.Controls.Add(lblTitulo);
            panelPrincipal.Controls.Add(lblFiltro);
            panelPrincipal.Controls.Add(cboFiltroEspecialidad);
            panelPrincipal.Controls.Add(lblEstadisticas);
            panelPrincipal.Controls.Add(panelGrafico);
            panelPrincipal.Controls.Add(panelContenedorGrid);
            panelPrincipal.Controls.Add(btnActualizar);
            panelPrincipal.Controls.Add(btnExportar);
            panelPrincipal.Controls.Add(btnCerrar);
            
            this.Controls.Add(panelPrincipal);
            this.ResumeLayout(false);
        }

        private async Task CargarReporteAsync()
        {
            if (_estaCargando) return;
            
            try
            {
                _estaCargando = true;
                Cursor.Current = Cursors.WaitCursor;
                
                // Obtener datos
                var planes = await _planApiClient.GetAllAsync();
                var especialidades = await _especialidadApiClient.GetAllAsync();
                var comisiones = await _comisionApiClient.GetAllAsync();
                var cursos = await _cursoApiClient.GetAllAsync();
                
                _todosLosPlanes = planes.ToList();
                _todasLasEspecialidades = especialidades.ToList();
                _todasLasComisiones = comisiones.ToList();
                _todosLosCursos = cursos.ToList();

                if (!_todosLosPlanes.Any())
                {
                    MessageBox.Show("No hay planes de estudio registrados.", 
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Cargar filtro de especialidades
                CargarFiltroEspecialidades();
                
                // Preparar y mostrar datos
                ActualizarGridYEstadisticas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar reporte: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _estaCargando = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void ActualizarGridYEstadisticas()
        {
            // Preparar datos para el grid
            var planesData = _todosLosPlanes.Select(p => {
                var especialidad = _todasLasEspecialidades?.FirstOrDefault(e => e.Id == p.EspecialidadId);
                var comisionesPlan = _todasLasComisiones?.Where(c => c.IdPlan == p.Id).ToList() ?? new List<ComisionDto>();
                var cursosPlan = new List<CursoDto>();
                
                foreach (var comision in comisionesPlan)
                {
                    var cursosComision = _todosLosCursos?.Where(c => c.IdComision == comision.IdComision) ?? Enumerable.Empty<CursoDto>();
                    cursosPlan.AddRange(cursosComision);
                }
                
                var totalInscriptos = cursosPlan.Sum(c => c.InscriptosActuales ?? 0);
                var totalCupos = cursosPlan.Sum(c => c.Cupo);
                var porcentajeOcupacion = totalCupos > 0 ? (totalInscriptos * 100.0 / totalCupos) : 0;
                
                return new
                {
                    Id = p.Id,
                    Plan = p.Descripcion,
                    Especialidad = especialidad?.Descripcion ?? "Sin Especialidad",
                    EspecialidadId = p.EspecialidadId,
                    Comisiones = comisionesPlan.Count,
                    CursosActivos = cursosPlan.Count,
                    TotalInscriptos = totalInscriptos,
                    CupoTotal = totalCupos,
                    OcupacionPorcentaje = $"{porcentajeOcupacion:F1}%",
                    Estado = cursosPlan.Any() ? "Activo" : "Sin Cursos"
                };
            }).ToList();

            dgvPlanes.DataSource = planesData;
            
            // Configurar columnas
            ConfigurarColumnas();
            
            // Aplicar colores
            AplicarColoresFilas();
            
            // Mostrar estadísticas
            MostrarEstadisticas(planesData);
            
            // Redibujar gráfico
            panelGrafico.Invalidate();
        }

        private void CargarFiltroEspecialidades()
        {
            cboFiltroEspecialidad.SelectedIndexChanged -= (s, e) => FiltrarPorEspecialidad();
            
            cboFiltroEspecialidad.Items.Clear();
            cboFiltroEspecialidad.Items.Add("Todas las Especialidades");
            
            foreach (var especialidad in _todasLasEspecialidades.OrderBy(e => e.Descripcion))
            {
                cboFiltroEspecialidad.Items.Add(especialidad.Descripcion);
            }
            
            cboFiltroEspecialidad.SelectedIndex = 0;
            
            cboFiltroEspecialidad.SelectedIndexChanged += (s, e) => FiltrarPorEspecialidad();
        }

        private void FiltrarPorEspecialidad()
        {
            if (_estaCargando || cboFiltroEspecialidad.SelectedIndex < 0)
                return;

            try
            {
                if (cboFiltroEspecialidad.SelectedIndex == 0)
                {
                    // Mostrar todos los planes
                    ActualizarGridYEstadisticas();
                    return;
                }

                var especialidadSeleccionada = cboFiltroEspecialidad.SelectedItem.ToString();
                var planesFiltrados = _todosLosPlanes.Where(p => 
                {
                    var especialidad = _todasLasEspecialidades.FirstOrDefault(e => e.Id == p.EspecialidadId);
                    return especialidad?.Descripcion == especialidadSeleccionada;
                }).ToList();

                // Crear datos filtrados con información completa
                var planesData = planesFiltrados.Select(p => {
                    var especialidad = _todasLasEspecialidades?.FirstOrDefault(e => e.Id == p.EspecialidadId);
                    var comisionesPlan = _todasLasComisiones?.Where(c => c.IdPlan == p.Id).ToList() ?? new List<ComisionDto>();
                    var cursosPlan = new List<CursoDto>();
                    
                    foreach (var comision in comisionesPlan)
                    {
                        var cursosComision = _todosLosCursos?.Where(c => c.IdComision == comision.IdComision) ?? Enumerable.Empty<CursoDto>();
                        cursosPlan.AddRange(cursosComision);
                    }
                    
                    var totalInscriptos = cursosPlan.Sum(c => c.InscriptosActuales ?? 0);
                    var totalCupos = cursosPlan.Sum(c => c.Cupo);
                    var porcentajeOcupacion = totalCupos > 0 ? (totalInscriptos * 100.0 / totalCupos) : 0;
                    
                    return new
                    {
                        Id = p.Id,
                        Plan = p.Descripcion,
                        Especialidad = especialidad?.Descripcion ?? "Sin Especialidad",
                        EspecialidadId = p.EspecialidadId,
                        Comisiones = comisionesPlan.Count,
                        CursosActivos = cursosPlan.Count,
                        TotalInscriptos = totalInscriptos,
                        CupoTotal = totalCupos,
                        OcupacionPorcentaje = $"{porcentajeOcupacion:F1}%",
                        Estado = cursosPlan.Any() ? "Activo" : "Sin Cursos"
                    };
                }).ToList();

                dgvPlanes.DataSource = planesData;
                ConfigurarColumnas();
                AplicarColoresFilas();
                MostrarEstadisticas(planesData);
                panelGrafico.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvPlanes.Columns.Count == 0) return;
            
            dgvPlanes.Columns["Id"].HeaderText = "ID";
            dgvPlanes.Columns["Id"].Width = 70;
            dgvPlanes.Columns["Plan"].HeaderText = "Plan de Estudio";
            dgvPlanes.Columns["Plan"].Width = 350;
            dgvPlanes.Columns["Especialidad"].HeaderText = "Especialidad";
            dgvPlanes.Columns["Especialidad"].Width = 250;
            dgvPlanes.Columns["EspecialidadId"].Visible = false;
            dgvPlanes.Columns["Comisiones"].HeaderText = "Comisiones";
            dgvPlanes.Columns["Comisiones"].Width = 120;
            dgvPlanes.Columns["CursosActivos"].HeaderText = "Cursos";
            dgvPlanes.Columns["CursosActivos"].Width = 100;
            dgvPlanes.Columns["TotalInscriptos"].HeaderText = "Inscriptos";
            dgvPlanes.Columns["TotalInscriptos"].Width = 120;
            dgvPlanes.Columns["CupoTotal"].HeaderText = "Cupo Total";
            dgvPlanes.Columns["CupoTotal"].Width = 120;
            dgvPlanes.Columns["OcupacionPorcentaje"].HeaderText = "Ocupacion";
            dgvPlanes.Columns["OcupacionPorcentaje"].Width = 120;
            dgvPlanes.Columns["Estado"].HeaderText = "Estado";
            dgvPlanes.Columns["Estado"].Width = 130;
        }

        private void AplicarColoresFilas()
        {
            foreach (DataGridViewRow row in dgvPlanes.Rows)
            {
                var estado = row.Cells["Estado"].Value?.ToString();
                var cursosActivos = Convert.ToInt32(row.Cells["CursosActivos"].Value);
                
                if (estado == "Sin Cursos" || cursosActivos == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 224);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(230, 126, 34);
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(232, 248, 245);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(22, 160, 133);
                }
            }
        }

        private void MostrarEstadisticas(dynamic planesData)
        {
            int totalPlanes = _todosLosPlanes.Count;
            int totalEspecialidades = _todasLasEspecialidades.Count;
            var planesActivos = ((IEnumerable<dynamic>)planesData).Count(p => p.CursosActivos > 0);
            var planesSinCursos = ((IEnumerable<dynamic>)planesData).Count(p => p.CursosActivos == 0);
            var totalInscriptos = ((IEnumerable<dynamic>)planesData).Sum(p => (int)p.TotalInscriptos);
            var totalCupos = ((IEnumerable<dynamic>)planesData).Sum(p => (int)p.CupoTotal);
            var promedioPlanesPorEsp = totalEspecialidades > 0 ? (totalPlanes / (double)totalEspecialidades) : 0;
            
            lblEstadisticas.Text = 
                $"Total de Planes: {totalPlanes}\n" +
                $"Total de Especialidades: {totalEspecialidades}\n" +
                $"Planes Activos (con cursos): {planesActivos}\n" +
                $"Planes Sin Cursos: {planesSinCursos}\n" +
                $"Total de Inscriptos: {totalInscriptos}\n" +
                $"Capacidad Total: {totalCupos}\n" +
                $"Promedio planes por especialidad: {promedioPlanesPorEsp:F2}";
        }

        private void DgvPlanes_SelectionChanged(object sender, EventArgs e)
        {
            panelGrafico.Invalidate();
        }

        private void PanelGrafico_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            // Título del gráfico
            var titleFont = new Font("Segoe UI", 14, FontStyle.Bold);
            g.DrawString("Distribucion de Planes por Especialidad", titleFont, 
                Brushes.Black, new PointF(10, 10));

            if (!_todosLosPlanes.Any() || !_todasLasEspecialidades.Any())
                return;

            // Agrupar planes por especialidad
            var planesPorEspecialidad = _todasLasEspecialidades
                .Select(esp => new
                {
                    Especialidad = esp.Descripcion,
                    Cantidad = _todosLosPlanes.Count(p => p.EspecialidadId == esp.Id)
                })
                .Where(x => x.Cantidad > 0)
                .OrderByDescending(x => x.Cantidad)
                .Take(8)
                .ToList();

            if (!planesPorEspecialidad.Any())
                return;

            // Dibujar gráfico de barras - Ajustado para el nuevo tamaño del panel
            int barWidth = 90;
            int barSpacing = 20;
            int startX = 40;
            int startY = 170;
            int maxBarHeight = 110;
            int maxCantidad = planesPorEspecialidad.Max(x => x.Cantidad);
            
            var colores = new[]
            {
                Color.FromArgb(52, 152, 219),
                Color.FromArgb(46, 204, 113),
                Color.FromArgb(155, 89, 182),
                Color.FromArgb(230, 126, 34),
                Color.FromArgb(231, 76, 60),
                Color.FromArgb(26, 188, 156),
                Color.FromArgb(241, 196, 15),
                Color.FromArgb(149, 165, 166)
            };

            for (int i = 0; i < planesPorEspecialidad.Count; i++)
            {
                var item = planesPorEspecialidad[i];
                int barHeight = maxCantidad > 0 ? (int)((item.Cantidad / (double)maxCantidad) * maxBarHeight) : 0;
                int x = startX + (i * (barWidth + barSpacing));
                int y = startY - barHeight;

                // Dibujar barra
                var brush = new SolidBrush(colores[i % colores.Length]);
                g.FillRectangle(brush, x, y, barWidth, barHeight);
                g.DrawRectangle(Pens.Black, x, y, barWidth, barHeight);

                // Dibujar valor encima de la barra
                var valueFont = new Font("Segoe UI", 11, FontStyle.Bold);
                var valueText = item.Cantidad.ToString();
                var valueSize = g.MeasureString(valueText, valueFont);
                g.DrawString(valueText, valueFont, Brushes.Black, 
                    x + (barWidth - valueSize.Width) / 2, y - 25);

                // Dibujar etiqueta DEBAJO de la barra
                var labelFont = new Font("Segoe UI", 8);
                var label = item.Especialidad.Length > 12 ? 
                    item.Especialidad.Substring(0, 12) + "..." : item.Especialidad;
                var labelSize = g.MeasureString(label, labelFont);
                
                // Centrar la etiqueta debajo de la barra
                float labelX = x + (barWidth - labelSize.Width) / 2;
                float labelY = startY + 5;
                
                g.DrawString(label, labelFont, Brushes.Black, labelX, labelY);
            }

            // Leyenda
            var legendFont = new Font("Segoe UI", 9);
            g.DrawString("* Se muestran las 8 especialidades con mas planes", 
                legendFont, Brushes.Gray, new PointF(10, 200));
        }
    }
}
