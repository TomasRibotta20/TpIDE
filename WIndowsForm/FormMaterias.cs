// Proyecto: WindowsForm
// Archivo: FormMaterias.cs

using API.Clients;
using DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel; // Necesario para BindingList
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormMaterias : Form
    {
        // Clientes API (hacerlos readonly)
        private readonly MateriaApiClient _materiaApiClient;
        private readonly PlanApiClient _planApiClient;
        private readonly Form? _menuPrincipal; // Referencia al menú para volver (anulable)

        // Lista observable para el DataGridView
        private BindingList<MateriaDto> _materias = new BindingList<MateriaDto>();

        // Constructor
        // Hacer menuPrincipal anulable por si se abre directamente
        public FormMaterias(Form? menuPrincipal = null)
        {
            InitializeComponent();
            _menuPrincipal = menuPrincipal; // Guardar referencia al menú

            // Instanciar clientes API
            _materiaApiClient = new MateriaApiClient();
            _planApiClient = new PlanApiClient(); // Necesario para pasar al formulario de edición

            // Configurar DataGridView y eventos
            ConfigurarDataGridView();
            SuscribirEventos();
        }

        // --- Configuración Inicial ---

        private void ConfigurarDataGridView()
        {
            dataGridViewMaterias.DataSource = _materias; // Enlazar lista
            dataGridViewMaterias.AutoGenerateColumns = false; // Controlar columnas manualmente

            // Limpiar columnas existentes si se reconfigura
            dataGridViewMaterias.Columns.Clear();

            // Agregar columnas manualmente
            dataGridViewMaterias.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id", // Enlazar a la propiedad Id del DTO
                HeaderText = "ID",
                Width = 50,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable // Opcional: Deshabilitar ordenación por ID
            });
            dataGridViewMaterias.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Descripcion",
                DataPropertyName = "Descripcion",
                HeaderText = "Descripción",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, // Ocupar espacio restante
                ReadOnly = true
            });
            dataGridViewMaterias.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HorasSemanales",
                DataPropertyName = "HorasSemanales",
                HeaderText = "Hs. Sem.",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }, // Alinear números
                ReadOnly = true
            });
            dataGridViewMaterias.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HorasTotales",
                DataPropertyName = "HorasTotales",
                HeaderText = "Hs. Tot.",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight },
                ReadOnly = true
            });
            dataGridViewMaterias.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DescripcionPlan", // Usamos el campo enriquecido del DTO
                DataPropertyName = "DescripcionPlan",
                HeaderText = "Plan Estudios",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells, // Ajustar al contenido
                ReadOnly = true
            });

            // Configuraciones adicionales del Grid
            dataGridViewMaterias.AllowUserToAddRows = false;
            dataGridViewMaterias.AllowUserToDeleteRows = false;
            dataGridViewMaterias.MultiSelect = false;
            dataGridViewMaterias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewMaterias.ReadOnly = true; // Todo el grid es de solo lectura
            dataGridViewMaterias.AllowUserToResizeRows = false; // Evitar que el usuario cambie altura de filas
            dataGridViewMaterias.RowHeadersVisible = false; // Ocultar columna vacía de la izquierda
        }

        private void SuscribirEventos()
        {
            // Usar firmas corregidas con object?
            this.Load += FormMaterias_Load; // Cargar datos al iniciar
            btnNuevo.Click += BtnNuevo_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnVolver.Click += BtnVolver_Click;
            // Opcional: Doble clic para editar
            dataGridViewMaterias.DoubleClick += BtnEditar_Click;
        }

        // --- Carga de Datos ---

        private async void FormMaterias_Load(object? sender, EventArgs e) // Firma corregida
        {
            await LoadMateriasAsync();
        }

        private async Task LoadMateriasAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var materias = await _materiaApiClient.GetAllAsync();

                // Limpiar y llenar la BindingList (esto actualiza el Grid automáticamente)
                _materias.Clear();
                if (materias != null) // Verificar si la respuesta no fue null
                {
                    foreach (var materia in materias.OrderBy(m => m.Descripcion)) // Ordenar alfabéticamente
                    {
                        _materias.Add(materia);
                    }
                }

                // Si no hay datos, mostrar un mensaje o estado vacío
                if (_materias.Count == 0)
                {
                    // Podrías mostrar un Panel con un Label indicando "No hay materias registradas."
                }
            }
            catch (UnauthorizedAccessException) // Capturar error de sesión expirada
            {
                MessageBox.Show("Su sesión ha expirado. Por favor, inicie sesión nuevamente.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Cerrar este formulario y posiblemente mostrar el Login
                this.Close();
                // _menuPrincipal?.ShowLogin(); // Si tienes un método para eso
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar materias: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Podrías deshabilitar botones si la carga falla
                btnNuevo.Enabled = false;
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        // --- Lógica de Botones ---

        private async void BtnNuevo_Click(object? sender, EventArgs e) // Firma corregida
        {
            // Crear una nueva materia vacía y pasar los clientes API necesarios
            using (var form = new EditarMateriaForm(null, _planApiClient, _materiaApiClient))
            {
                if (form.ShowDialog(this) == DialogResult.OK) // Mostrar como diálogo modal
                {
                    await LoadMateriasAsync(); // Recargar la lista si se guardó
                }
            }
        }

        private async void BtnEditar_Click(object? sender, EventArgs e) // Firma corregida
        {
            if (dataGridViewMaterias.SelectedRows.Count > 0)
            {
                // Obtener el DTO seleccionado de forma segura
                var selectedMateria = dataGridViewMaterias.SelectedRows[0].DataBoundItem as MateriaDto;

                if (selectedMateria == null)
                {
                    MessageBox.Show("No se pudo obtener la información de la materia seleccionada. Intente de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Pasar el DTO seleccionado y los clientes al formulario de edición
                using (var form = new EditarMateriaForm(selectedMateria, _planApiClient, _materiaApiClient))
                {
                    if (form.ShowDialog(this) == DialogResult.OK) // Mostrar como diálogo modal
                    {
                        await LoadMateriasAsync(); // Recargar la lista si se guardó
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una materia de la lista para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnEliminar_Click(object? sender, EventArgs e) // Firma corregida
        {
            if (dataGridViewMaterias.SelectedRows.Count > 0)
            {
                var selectedMateria = dataGridViewMaterias.SelectedRows[0].DataBoundItem as MateriaDto;

                if (selectedMateria == null)
                {
                    MessageBox.Show("No se pudo obtener la información de la materia seleccionada. Intente de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirmación más explícita
                var confirmResult = MessageBox.Show($"¿Está seguro que desea eliminar la materia '{selectedMateria.Descripcion}' (ID: {selectedMateria.Id})?\nEsta acción no se puede deshacer.",
                                                    "Confirmar Eliminación Permanente",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Warning); // Usar Warning para indicar riesgo

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        await _materiaApiClient.DeleteAsync(selectedMateria.Id);
                        MessageBox.Show("Materia eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Cursor.Current = Cursors.Default;
                        await LoadMateriasAsync(); // Recargar la lista
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Su sesión ha expirado. Por favor, inicie sesión nuevamente.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        Cursor.Current = Cursors.Default;
                        // Mensaje más específico si la API devuelve detalles (ej: error de dependencia)
                        MessageBox.Show($"Error al eliminar materia: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una materia de la lista para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnVolver_Click(object? sender, EventArgs e) // Firma corregida
        {
            this.Close(); // Cierra el formulario actual
                          // El menú principal se mostrará automáticamente si se usó ShowDialog o por el OnFormClosed
        }

        // Sobrescribir OnFormClosed para asegurar que el menú principal se muestre si está oculto y fue pasado
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _menuPrincipal?.Show(); // Muestra el menú al cerrar este formulario
        }


        // --- Código del Diseñador (InitializeComponent) ---
        // Declaraciones con null-forgiving (!)
        private System.ComponentModel.IContainer components = null!;
        private TableLayoutPanel tableLayoutPanelMain = null!;
        private DataGridView dataGridViewMaterias = null!;
        private FlowLayoutPanel panelBotones = null!;
        private Button btnNuevo = null!;
        private Button btnEditar = null!;
        private Button btnEliminar = null!;
        private Button btnVolver = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewMaterias = new System.Windows.Forms.DataGridView();
            this.panelBotones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaterias)).BeginInit();
            this.panelBotones.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayoutPanelMain
            //
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.dataGridViewMaterias, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.panelBotones, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelMain.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(782, 453); // Example size
            this.tableLayoutPanelMain.TabIndex = 0;
            //
            // dataGridViewMaterias
            //
            this.dataGridViewMaterias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMaterias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMaterias.Location = new System.Drawing.Point(13, 13);
            this.dataGridViewMaterias.Name = "dataGridViewMaterias";
            this.dataGridViewMaterias.RowHeadersWidth = 51;
            this.dataGridViewMaterias.RowTemplate.Height = 29;
            this.dataGridViewMaterias.Size = new System.Drawing.Size(756, 367);
            this.dataGridViewMaterias.TabIndex = 0;
            //
            // panelBotones
            //
            this.panelBotones.Controls.Add(this.btnVolver);
            this.panelBotones.Controls.Add(this.btnEliminar);
            this.panelBotones.Controls.Add(this.btnEditar);
            this.panelBotones.Controls.Add(this.btnNuevo);
            this.panelBotones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBotones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelBotones.Location = new System.Drawing.Point(13, 386);
            this.panelBotones.Name = "panelBotones";
            this.panelBotones.Size = new System.Drawing.Size(756, 54);
            this.panelBotones.TabIndex = 1;
            this.panelBotones.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5); // Add vertical padding
            //
            // btnNuevo
            //
            this.btnNuevo.Location = new System.Drawing.Point(653, 8); // Position adjusted for RightToLeft
            this.btnNuevo.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3); // Right margin
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(100, 40);
            this.btnNuevo.TabIndex = 3; // Order changed
            this.btnNuevo.Text = "&Nuevo"; // Add shortcut key
            this.btnNuevo.UseVisualStyleBackColor = true;
            //
            // btnEditar
            //
            this.btnEditar.Location = new System.Drawing.Point(547, 8);
            this.btnEditar.Margin = new System.Windows.Forms.Padding(3);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(100, 40);
            this.btnEditar.TabIndex = 2; // Order changed
            this.btnEditar.Text = "&Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            //
            // btnEliminar
            //
            this.btnEliminar.Location = new System.Drawing.Point(441, 8);
            this.btnEliminar.Margin = new System.Windows.Forms.Padding(3);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(100, 40);
            this.btnEliminar.TabIndex = 1; // Order changed
            this.btnEliminar.Text = "E&liminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            //
            // btnVolver
            //
            this.btnVolver.Location = new System.Drawing.Point(335, 8);
            this.btnVolver.Margin = new System.Windows.Forms.Padding(3);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(100, 40);
            this.btnVolver.TabIndex = 0; // First button on the right
            this.btnVolver.Text = "&Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            //
            // FormMaterias
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 453); // Default size
            this.Controls.Add(this.tableLayoutPanelMain);
            this.MinimumSize = new System.Drawing.Size(600, 400); // Set reasonable minimum size
            this.Name = "FormMaterias";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent; // Center relative to parent
            this.Text = "Gestión de Materias";
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaterias)).EndInit();
            this.panelBotones.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}
