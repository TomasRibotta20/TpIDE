// Proyecto: WindowsForm
// Archivo: EditarMateriaForm.cs

using API.Clients;
using DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel; // Para soporte del diseñador
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForm
{
    // Asegúrate de que hereda de Form
    public partial class EditarMateriaForm : Form
    {
        // Campos privados para almacenar la materia y los clientes API
        private MateriaDto _materia;
        private readonly MateriaApiClient _materiaApiClient;
        private readonly PlanApiClient _planApiClient;
        private List<PlanDto> _planes = new List<PlanDto>(); // Inicializar para evitar advertencias

        // Propiedad para saber si estamos editando una materia existente o creando una nueva
        private bool IsEditing => _materia != null && _materia.Id != 0;

        // Constructor: Recibe la materia a editar (null si es nueva) y los clientes API
        public EditarMateriaForm(MateriaDto? materia = null, PlanApiClient? planApiClient = null, MateriaApiClient? materiaApiClient = null)
        {
            InitializeComponent(); // Carga los controles diseñados visualmente

            // Si no se proporciona una materia, crea una nueva instancia vacía
            _materia = materia ?? new MateriaDto();

            // Usa los clientes API proporcionados o crea nuevas instancias si son null
            _planApiClient = planApiClient ?? new PlanApiClient();
            _materiaApiClient = materiaApiClient ?? new MateriaApiClient();

            // Suscribir los eventos principales del formulario y botones
            this.Load += EditarMateriaForm_Load; // Evento que se dispara al cargar el form
            btnAceptar.Click += BtnAceptar_Click; // Evento del botón Aceptar/Guardar
            btnCancelar.Click += BtnCancelar_Click; // Evento del botón Cancelar

            // Configurar el título de la ventana según si es edición o alta
            Text = IsEditing ? $"Editar Materia (ID: {_materia.Id})" : "Nueva Materia";

            // Configurar botones Aceptar/Cancelar por defecto
            this.AcceptButton = btnAceptar;
            this.CancelButton = btnCancelar;
        }

        // --- Manejadores de Eventos ---

        // Método que se ejecuta cuando el formulario se carga por primera vez
        private async void EditarMateriaForm_Load(object? sender, EventArgs e) // Firma corregida para nulabilidad
        {
            await LoadPlanesAsync(); // Carga la lista de Planes en el ComboBox
            BindMateriaData(); // Rellena los campos del formulario con los datos de la materia
        }

        // Método que se ejecuta al hacer clic en el botón Aceptar/Guardar
        private async void BtnAceptar_Click(object? sender, EventArgs e) // Firma corregida
        {
            if (!ValidateForm()) // Primero, valida que los datos ingresados sean correctos
            {
                return; // Si la validación falla, no continúa
            }

            // Mapear los datos de los controles del formulario al objeto _materia (DTO)
            _materia.Descripcion = txtDescripcion.Text.Trim(); // Quitar espacios al inicio/final
            // Usar TryParse para convertir texto a número de forma segura
            int.TryParse(txtHorasSemanales.Text, out int horasSem);
            int.TryParse(txtHorasTotales.Text, out int horasTot);
            _materia.HorasSemanales = horasSem;
            _materia.HorasTotales = horasTot;
            // Obtener el ID del Plan seleccionado en el ComboBox (manejar posible null)
            _materia.IdPlan = (int?)cmbPlan.SelectedValue ?? 0;

            try
            {
                Cursor.Current = Cursors.WaitCursor; // Mostrar cursor de espera durante la operación API

                // Llamar al método API correspondiente (Update si edita, Create si es nuevo)
                if (IsEditing)
                {
                    await _materiaApiClient.UpdateAsync(_materia);
                    MessageBox.Show("Materia actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var createdMateria = await _materiaApiClient.CreateAsync(_materia);
                    MessageBox.Show($"Materia '{createdMateria.Descripcion}' creada con ID: {createdMateria.Id}.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _materia = createdMateria; // Actualizar el objeto local con el ID asignado por la API
                }
                Cursor.Current = Cursors.Default; // Restaurar cursor normal

                DialogResult = DialogResult.OK; // Establecer resultado para indicar éxito al formulario que lo abrió
                this.Close(); // Cerrar este formulario
            }
            catch (UnauthorizedAccessException) // Capturar error específico de sesión expirada
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Su sesión ha expirado. Por favor, inicie sesión nuevamente.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Aquí podrías cerrar y forzar ir al Login si fuera necesario
                DialogResult = DialogResult.Abort; // Indicar un cierre anormal
                this.Close();
            }
            catch (ArgumentException ex) // Capturar errores de validación del backend (400 Bad Request)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Error de validación: {ex.Message}", "Datos Inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // No cerrar, permitir al usuario corregir
            }
            catch (KeyNotFoundException ex) // Capturar errores de "No encontrado" del backend (404 Not Found)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Error: {ex.Message}", "No Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Podrías cerrar si el elemento ya no existe
            }
            catch (Exception ex) // Capturar cualquier otro error inesperado
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Ocurrió un error inesperado al guardar la materia: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // No cerrar, investigar el error
            }
        }

        // Método que se ejecuta al hacer clic en el botón Cancelar
        private void BtnCancelar_Click(object? sender, EventArgs e) // Firma corregida
        {
            DialogResult = DialogResult.Cancel; // Establecer resultado para indicar cancelación
            this.Close(); // Cerrar el formulario
        }

        // --- Métodos Auxiliares ---

        // Carga la lista de Planes desde la API y configura el ComboBox
        private async Task LoadPlanesAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                // Llama al cliente API para obtener los planes, maneja posible respuesta null
                _planes = (await _planApiClient.GetAllAsync())?.OrderBy(p => p.Descripcion).ToList() ?? new List<PlanDto>();

                // Limpiar el ComboBox antes de llenarlo
                cmbPlan.DataSource = null;

                if (_planes.Count > 0)
                {
                    // Asignar la lista de planes como origen de datos
                    cmbPlan.DataSource = _planes;
                    // Configurar qué propiedad mostrar al usuario
                    cmbPlan.DisplayMember = "Descripcion";
                    // Configurar qué propiedad usar como valor interno (el ID)
                    cmbPlan.ValueMember = "Id";
                    cmbPlan.Enabled = true; // Habilitar el ComboBox
                    btnAceptar.Enabled = true; // Habilitar el botón Guardar
                }
                else
                {
                    // Si no hay planes, mostrar advertencia y deshabilitar controles
                    MessageBox.Show("No hay Planes de Estudio disponibles. No se puede crear/editar Materias.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbPlan.DataSource = null;
                    cmbPlan.Enabled = false;
                    btnAceptar.Enabled = false;
                }
            }
            catch (UnauthorizedAccessException)
            { // Manejo de sesión expirada
                MessageBox.Show("Su sesión ha expirado. Por favor, inicie sesión nuevamente.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Abort; // Indicar cierre anormal
                this.Close();
            }
            catch (Exception ex) // Manejo de otros errores al cargar planes
            {
                MessageBox.Show($"Error crítico al cargar planes: {ex.Message}. El formulario se cerrará.", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Abort;
                this.Close(); // Cerrar si la carga falla
            }
            finally
            {
                Cursor.Current = Cursors.Default; // Asegurarse de restaurar el cursor
            }
        }

        // Rellena los controles del formulario con los datos de la materia actual (_materia)
        private void BindMateriaData()
        {
            txtDescripcion.Text = _materia.Descripcion;
            // Mostrar horas solo si son mayores a 0, sino dejar vacío
            txtHorasSemanales.Text = _materia.HorasSemanales > 0 ? _materia.HorasSemanales.ToString() : "";
            txtHorasTotales.Text = _materia.HorasTotales > 0 ? _materia.HorasTotales.ToString() : "";

            // Seleccionar el Plan correcto en el ComboBox si estamos editando
            if (IsEditing && _materia.IdPlan > 0 && _planes.Any(p => p.Id == _materia.IdPlan))
            {
                // Intentar seleccionar por valor (ID del plan)
                try
                {
                    // Buscar el índice del plan a seleccionar
                    int indexToSelect = -1;
                    for (int i = 0; i < _planes.Count; i++)
                    {
                        if (_planes[i].Id == _materia.IdPlan)
                        {
                            indexToSelect = i;
                            break;
                        }
                    }
                    if (indexToSelect != -1)
                    {
                        cmbPlan.SelectedIndex = indexToSelect; // Seleccionar por índice
                    }
                    else if (_planes.Count > 0) // Si no se encontró, seleccionar el primero
                    {
                        cmbPlan.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbPlan.SelectedIndex = -1; // Sin selección si no hay planes
                    }
                }
                catch (Exception ex)
                {
                    // Si falla la selección, registrar y seleccionar el primero como fallback
                    Console.WriteLine($"Advertencia: No se pudo seleccionar el Plan ID {_materia.IdPlan}. {ex.Message}");
                    if (_planes.Count > 0) cmbPlan.SelectedIndex = 0; else cmbPlan.SelectedIndex = -1;
                }
            }
            else if (_planes.Count > 0) // Si es una materia nueva y hay planes disponibles
            {
                cmbPlan.SelectedIndex = 0; // Seleccionar el primer plan por defecto
            }
            else
            { // Si no hay planes disponibles
                cmbPlan.SelectedIndex = -1; // Dejar sin selección
            }
        }

        // Valida los datos ingresados en el formulario antes de guardar
        private bool ValidateForm()
        {
            // Validar Descripción no vacía
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("La descripción de la materia es obligatoria.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescripcion.Focus(); // Poner el foco en el campo con error
                return false;
            }

            // Validar Horas Semanales (número entero positivo)
            if (!int.TryParse(txtHorasSemanales.Text, out int horasSem) || horasSem <= 0)
            {
                MessageBox.Show("Las Horas Semanales deben ser un número entero positivo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHorasSemanales.SelectAll(); // Seleccionar el texto para fácil corrección
                txtHorasSemanales.Focus();
                return false;
            }

            // Validar Horas Totales (número entero positivo)
            if (!int.TryParse(txtHorasTotales.Text, out int horasTot) || horasTot <= 0)
            {
                MessageBox.Show("Las Horas Totales deben ser un número entero positivo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHorasTotales.SelectAll();
                txtHorasTotales.Focus();
                return false;
            }

            // Validar que Horas Totales >= Horas Semanales (Lógica básica)
            if (horasTot < horasSem)
            {
                MessageBox.Show("Las Horas Totales no pueden ser menores que las Horas Semanales.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHorasTotales.SelectAll();
                txtHorasTotales.Focus();
                return false;
            }

            // Validar que se haya seleccionado un Plan válido en el ComboBox
            if (cmbPlan.SelectedValue == null || !(cmbPlan.SelectedValue is int) || (int)cmbPlan.SelectedValue <= 0)
            {
                MessageBox.Show("Debe seleccionar un Plan de Estudios válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPlan.Focus();
                return false;
            }

            return true; // Si todas las validaciones pasan, retorna true
        }

        // --- Código del Diseñador (InitializeComponent) ---
        // Declaraciones de controles con null-forgiving (!) para asegurar al compilador que serán inicializados
        private System.ComponentModel.IContainer components = null!;
        private TableLayoutPanel tableLayoutPanelMain = null!;
        private Label lblDescripcion = null!;
        private TextBox txtDescripcion = null!;
        private Label lblHorasSem = null!;
        private TextBox txtHorasSemanales = null!;
        private Label lblHorasTot = null!;
        private TextBox txtHorasTotales = null!;
        private Label lblPlan = null!;
        private ComboBox cmbPlan = null!;
        private FlowLayoutPanel panelBotones = null!;
        private Button btnAceptar = null!;
        private Button btnCancelar = null!;

        // Método Dispose generado por el diseñador
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblHorasSem = new System.Windows.Forms.Label();
            this.txtHorasSemanales = new System.Windows.Forms.TextBox();
            this.lblHorasTot = new System.Windows.Forms.Label();
            this.txtHorasTotales = new System.Windows.Forms.TextBox();
            this.lblPlan = new System.Windows.Forms.Label();
            this.cmbPlan = new System.Windows.Forms.ComboBox();
            this.panelBotones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain.SuspendLayout();
            this.panelBotones.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayoutPanelMain
            //
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F)); // Fixed label width
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F)); // Textbox takes remaining space
            this.tableLayoutPanelMain.Controls.Add(this.lblDescripcion, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.txtDescripcion, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.lblHorasSem, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.txtHorasSemanales, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.lblHorasTot, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.txtHorasTotales, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.lblPlan, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.cmbPlan, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.panelBotones, 0, 4);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanelMain.RowCount = 5;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F)); // Button panel takes remaining space
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(432, 253);
            this.tableLayoutPanelMain.TabIndex = 0;
            //
            // lblDescripcion
            //
            this.lblDescripcion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(13, 17);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(87, 20);
            this.lblDescripcion.TabIndex = 0;
            this.lblDescripcion.Text = "Descripción:";
            //
            // txtDescripcion
            //
            this.txtDescripcion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescripcion.Location = new System.Drawing.Point(133, 14); // Position based on fixed label width
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(286, 27); // Adjusted size
            this.txtDescripcion.TabIndex = 1;
            //
            // lblHorasSem
            //
            this.lblHorasSem.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblHorasSem.AutoSize = true;
            this.lblHorasSem.Location = new System.Drawing.Point(13, 52);
            this.lblHorasSem.Name = "lblHorasSem";
            this.lblHorasSem.Size = new System.Drawing.Size(108, 20);
            this.lblHorasSem.TabIndex = 2;
            this.lblHorasSem.Text = "Hs. Semanales:";
            //
            // txtHorasSemanales
            //
            this.txtHorasSemanales.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHorasSemanales.Location = new System.Drawing.Point(133, 49);
            this.txtHorasSemanales.Name = "txtHorasSemanales";
            this.txtHorasSemanales.Size = new System.Drawing.Size(286, 27);
            this.txtHorasSemanales.TabIndex = 3;
            //
            // lblHorasTot
            //
            this.lblHorasTot.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblHorasTot.AutoSize = true;
            this.lblHorasTot.Location = new System.Drawing.Point(13, 87);
            this.lblHorasTot.Name = "lblHorasTot";
            this.lblHorasTot.Size = new System.Drawing.Size(91, 20);
            this.lblHorasTot.TabIndex = 4;
            this.lblHorasTot.Text = "Hs. Totales:";
            //
            // txtHorasTotales
            //
            this.txtHorasTotales.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHorasTotales.Location = new System.Drawing.Point(133, 84);
            this.txtHorasTotales.Name = "txtHorasTotales";
            this.txtHorasTotales.Size = new System.Drawing.Size(286, 27);
            this.txtHorasTotales.TabIndex = 5;
            //
            // lblPlan
            //
            this.lblPlan.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPlan.AutoSize = true;
            this.lblPlan.Location = new System.Drawing.Point(13, 122);
            this.lblPlan.Name = "lblPlan";
            this.lblPlan.Size = new System.Drawing.Size(39, 20);
            this.lblPlan.TabIndex = 6;
            this.lblPlan.Text = "Plan:";
            //
            // cmbPlan
            //
            this.cmbPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlan.FormattingEnabled = true;
            this.cmbPlan.Location = new System.Drawing.Point(133, 119);
            this.cmbPlan.Name = "cmbPlan";
            this.cmbPlan.Size = new System.Drawing.Size(286, 28);
            this.cmbPlan.TabIndex = 7;
            //
            // panelBotones
            //
            this.tableLayoutPanelMain.SetColumnSpan(this.panelBotones, 2);
            this.panelBotones.Controls.Add(this.btnCancelar); // Order for RightToLeft
            this.panelBotones.Controls.Add(this.btnAceptar); // Order for RightToLeft
            this.panelBotones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBotones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelBotones.Location = new System.Drawing.Point(13, 153);
            this.panelBotones.Name = "panelBotones";
            this.panelBotones.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelBotones.Size = new System.Drawing.Size(406, 87);
            this.panelBotones.TabIndex = 8;
            //
            // btnAceptar
            //
            this.btnAceptar.Location = new System.Drawing.Point(303, 13);
            this.btnAceptar.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3); // Right margin
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(100, 30);
            this.btnAceptar.TabIndex = 0; // First button on the right
            this.btnAceptar.Text = "&Aceptar"; // Shortcut key
            this.btnAceptar.UseVisualStyleBackColor = true;
            //
            // btnCancelar
            //
            this.btnCancelar.Location = new System.Drawing.Point(197, 13);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 30);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "&Cancelar"; // Shortcut key
            this.btnCancelar.UseVisualStyleBackColor = true;
            //
            // EditarMateriaForm
            //
            this.AcceptButton = this.btnAceptar; // Enter key triggers Accept
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar; // Escape key triggers Cancel
            this.ClientSize = new System.Drawing.Size(432, 253);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.MaximizeBox = false; // Disable maximize
            this.MinimizeBox = false; // Disable minimize
            this.MinimumSize = new System.Drawing.Size(450, 300); // Set minimum size
            this.Name = "EditarMateriaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent; // Center relative to the form that opened it
            this.Text = "Editar Materia"; // Default text, updated in constructor
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.panelBotones.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}
