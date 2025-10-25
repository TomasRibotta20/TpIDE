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
            tableLayoutPanelMain = new TableLayoutPanel();
            lblDescripcion = new Label();
            txtDescripcion = new TextBox();
            lblHorasSem = new Label();
            txtHorasSemanales = new TextBox();
            lblHorasTot = new Label();
            txtHorasTotales = new TextBox();
            lblPlan = new Label();
            cmbPlan = new ComboBox();
            panelBotones = new FlowLayoutPanel();
            btnCancelar = new Button();
            btnAceptar = new Button();
            tableLayoutPanelMain.SuspendLayout();
            panelBotones.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 2;
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105F));
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Controls.Add(lblDescripcion, 0, 0);
            tableLayoutPanelMain.Controls.Add(txtDescripcion, 1, 0);
            tableLayoutPanelMain.Controls.Add(lblHorasSem, 0, 1);
            tableLayoutPanelMain.Controls.Add(txtHorasSemanales, 1, 1);
            tableLayoutPanelMain.Controls.Add(lblHorasTot, 0, 2);
            tableLayoutPanelMain.Controls.Add(txtHorasTotales, 1, 2);
            tableLayoutPanelMain.Controls.Add(lblPlan, 0, 3);
            tableLayoutPanelMain.Controls.Add(cmbPlan, 1, 3);
            tableLayoutPanelMain.Controls.Add(panelBotones, 0, 4);
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelMain.Location = new Point(0, 0);
            tableLayoutPanelMain.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.Padding = new Padding(9, 8, 9, 8);
            tableLayoutPanelMain.RowCount = 5;
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Size = new Size(380, 196);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Anchor = AnchorStyles.Left;
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(12, 13);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(72, 15);
            lblDescripcion.TabIndex = 0;
            lblDescripcion.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            txtDescripcion.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtDescripcion.Location = new Point(117, 10);
            txtDescripcion.Margin = new Padding(3, 2, 3, 2);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(251, 23);
            txtDescripcion.TabIndex = 1;
            // 
            // lblHorasSem
            // 
            lblHorasSem.Anchor = AnchorStyles.Left;
            lblHorasSem.AutoSize = true;
            lblHorasSem.Location = new Point(12, 39);
            lblHorasSem.Name = "lblHorasSem";
            lblHorasSem.Size = new Size(86, 15);
            lblHorasSem.TabIndex = 2;
            lblHorasSem.Text = "Hs. Semanales:";
            // 
            // txtHorasSemanales
            // 
            txtHorasSemanales.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtHorasSemanales.Location = new Point(117, 36);
            txtHorasSemanales.Margin = new Padding(3, 2, 3, 2);
            txtHorasSemanales.Name = "txtHorasSemanales";
            txtHorasSemanales.Size = new Size(251, 23);
            txtHorasSemanales.TabIndex = 3;
            // 
            // lblHorasTot
            // 
            lblHorasTot.Anchor = AnchorStyles.Left;
            lblHorasTot.AutoSize = true;
            lblHorasTot.Location = new Point(12, 65);
            lblHorasTot.Name = "lblHorasTot";
            lblHorasTot.Size = new Size(66, 15);
            lblHorasTot.TabIndex = 4;
            lblHorasTot.Text = "Hs. Totales:";
            // 
            // txtHorasTotales
            // 
            txtHorasTotales.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtHorasTotales.Location = new Point(117, 62);
            txtHorasTotales.Margin = new Padding(3, 2, 3, 2);
            txtHorasTotales.Name = "txtHorasTotales";
            txtHorasTotales.Size = new Size(251, 23);
            txtHorasTotales.TabIndex = 5;
            // 
            // lblPlan
            // 
            lblPlan.Anchor = AnchorStyles.Left;
            lblPlan.AutoSize = true;
            lblPlan.Location = new Point(12, 91);
            lblPlan.Name = "lblPlan";
            lblPlan.Size = new Size(33, 15);
            lblPlan.TabIndex = 6;
            lblPlan.Text = "Plan:";
            // 
            // cmbPlan
            // 
            cmbPlan.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbPlan.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlan.FormattingEnabled = true;
            cmbPlan.Location = new Point(117, 88);
            cmbPlan.Margin = new Padding(3, 2, 3, 2);
            cmbPlan.Name = "cmbPlan";
            cmbPlan.Size = new Size(251, 23);
            cmbPlan.TabIndex = 7;
            // 
            // panelBotones
            // 
            tableLayoutPanelMain.SetColumnSpan(panelBotones, 2);
            panelBotones.Controls.Add(btnCancelar);
            panelBotones.Controls.Add(btnAceptar);
            panelBotones.Dock = DockStyle.Fill;
            panelBotones.FlowDirection = FlowDirection.RightToLeft;
            panelBotones.Location = new Point(12, 114);
            panelBotones.Margin = new Padding(3, 2, 3, 2);
            panelBotones.Name = "panelBotones";
            panelBotones.Padding = new Padding(0, 8, 0, 0);
            panelBotones.Size = new Size(356, 72);
            panelBotones.TabIndex = 8;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(265, 10);
            btnCancelar.Margin = new Padding(3, 2, 3, 2);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(88, 22);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "&Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(165, 10);
            btnAceptar.Margin = new Padding(3, 2, 9, 2);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(88, 22);
            btnAceptar.TabIndex = 0;
            btnAceptar.Text = "&Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            // 
            // EditarMateriaForm
            // 
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(380, 196);
            Controls.Add(tableLayoutPanelMain);
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(396, 235);
            Name = "EditarMateriaForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Editar Materia";
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            panelBotones.ResumeLayout(false);
            ResumeLayout(false);

        }
        #endregion
    }
}
