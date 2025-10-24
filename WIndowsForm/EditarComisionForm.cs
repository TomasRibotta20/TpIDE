using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Clients;
using DTOs;

namespace WIndowsForm
{
    public partial class EditarComisionForm : Form
    {
        private readonly ComisionDto _comision;
        private readonly bool _esNuevo;
        private readonly PlanApiClient _planApiClient = new PlanApiClient();

        private TableLayoutPanel tableLayoutPanel1;
        private Label lblIdComision;
        private TextBox txtIdComision;
        private Label lblDescComision;
        private TextBox txtDescComision;
        private Label lblAnioEspecialidad;
        private NumericUpDown numAnioEspecialidad;
        private Label lblPlan;
        private ComboBox comboPlanes;
        private Panel panelBotones;
        private Button btnCancelar;
        private Button btnGuardar;

        public ComisionDto ComisionEditada { get; private set; }
        public bool Guardado { get; private set; }

        public EditarComisionForm(ComisionDto comision = null)
        {
            InitializeComponent();
            _comision = comision ?? new ComisionDto();
            _esNuevo = comision == null;

            ConfigurarFormulario();
            this.Load += async (_, __) => await CargarDatosAsync();
        }

        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            lblIdComision = new Label();
            txtIdComision = new TextBox();
            lblDescComision = new Label();
            txtDescComision = new TextBox();
            lblAnioEspecialidad = new Label();
            numAnioEspecialidad = new NumericUpDown();
            lblPlan = new Label();
            comboPlanes = new ComboBox();
            panelBotones = new Panel();
            btnCancelar = new Button();
            btnGuardar = new Button();
            tableLayoutPanel1.SuspendLayout();
            ((ISupportInitialize)numAnioEspecialidad).BeginInit();
            panelBotones.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.Controls.Add(lblIdComision, 0, 0);
            tableLayoutPanel1.Controls.Add(txtIdComision, 1, 0);
            tableLayoutPanel1.Controls.Add(lblDescComision, 0, 1);
            tableLayoutPanel1.Controls.Add(txtDescComision, 1, 1);
            tableLayoutPanel1.Controls.Add(lblAnioEspecialidad, 0, 2);
            tableLayoutPanel1.Controls.Add(numAnioEspecialidad, 1, 2);
            tableLayoutPanel1.Controls.Add(lblPlan, 0, 3);
            tableLayoutPanel1.Controls.Add(comboPlanes, 1, 3);
            tableLayoutPanel1.Controls.Add(panelBotones, 0, 4);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(20);
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.Size = new Size(434, 261);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lblIdComision
            // 
            lblIdComision.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblIdComision.AutoSize = true;
            lblIdComision.Location = new Point(23, 32);
            lblIdComision.Name = "lblIdComision";
            lblIdComision.Size = new Size(112, 15);
            lblIdComision.TabIndex = 0;
            lblIdComision.Text = "ID:";
            lblIdComision.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtIdComision
            // 
            txtIdComision.Dock = DockStyle.Fill;
            txtIdComision.Location = new Point(141, 23);
            txtIdComision.Name = "txtIdComision";
            txtIdComision.ReadOnly = true;
            txtIdComision.Size = new Size(270, 23);
            txtIdComision.TabIndex = 1;
            // 
            // lblDescComision
            // 
            lblDescComision.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblDescComision.AutoSize = true;
            lblDescComision.Location = new Point(23, 72);
            lblDescComision.Name = "lblDescComision";
            lblDescComision.Size = new Size(112, 15);
            lblDescComision.TabIndex = 2;
            lblDescComision.Text = "Descripción*:";
            lblDescComision.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDescComision
            // 
            txtDescComision.Dock = DockStyle.Fill;
            txtDescComision.Location = new Point(141, 63);
            txtDescComision.Name = "txtDescComision";
            txtDescComision.Size = new Size(270, 23);
            txtDescComision.TabIndex = 3;
            // 
            // lblAnioEspecialidad
            // 
            lblAnioEspecialidad.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblAnioEspecialidad.AutoSize = true;
            lblAnioEspecialidad.Location = new Point(23, 112);
            lblAnioEspecialidad.Name = "lblAnioEspecialidad";
            lblAnioEspecialidad.Size = new Size(112, 15);
            lblAnioEspecialidad.TabIndex = 4;
            lblAnioEspecialidad.Text = "Año Especialidad*:";
            lblAnioEspecialidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numAnioEspecialidad
            // 
            numAnioEspecialidad.Dock = DockStyle.Fill;
            numAnioEspecialidad.Location = new Point(141, 103);
            numAnioEspecialidad.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numAnioEspecialidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numAnioEspecialidad.Name = "numAnioEspecialidad";
            numAnioEspecialidad.Size = new Size(270, 23);
            numAnioEspecialidad.TabIndex = 5;
            numAnioEspecialidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblPlan
            // 
            lblPlan.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblPlan.AutoSize = true;
            lblPlan.Location = new Point(23, 152);
            lblPlan.Name = "lblPlan";
            lblPlan.Size = new Size(112, 15);
            lblPlan.TabIndex = 6;
            lblPlan.Text = "Plan*:";
            lblPlan.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboPlanes
            // 
            comboPlanes.Dock = DockStyle.Fill;
            comboPlanes.FormattingEnabled = true;
            comboPlanes.Location = new Point(141, 143);
            comboPlanes.Name = "comboPlanes";
            comboPlanes.Size = new Size(270, 23);
            comboPlanes.TabIndex = 7;
            // 
            // panelBotones
            // 
            tableLayoutPanel1.SetColumnSpan(panelBotones, 2);
            panelBotones.Controls.Add(btnCancelar);
            panelBotones.Controls.Add(btnGuardar);
            panelBotones.Dock = DockStyle.Fill;
            panelBotones.Location = new Point(23, 183);
            panelBotones.Name = "panelBotones";
            panelBotones.Size = new Size(388, 55);
            panelBotones.TabIndex = 8;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(160, 5);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 30);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(50, 5);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(100, 30);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            // 
            // EditarComisionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(434, 261);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditarComisionForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Comisión";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((ISupportInitialize)numAnioEspecialidad).EndInit();
            panelBotones.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void ConfigurarFormulario()
        {
            Text = _esNuevo ? "Nueva Comisión" : "Editar Comisión";

            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (_, __) => Close();

            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;

            if (_esNuevo)
            {
                lblIdComision.Visible = false;
                txtIdComision.Visible = false;
                tableLayoutPanel1.RowStyles[0].Height = 0;
            }

            comboPlanes.DropDownStyle = ComboBoxStyle.DropDownList;
            
            // Configurar el control numérico para el año de especialidad sin límite superior
            numAnioEspecialidad.Minimum = 1;
            numAnioEspecialidad.Maximum = 9999; // Máximo valor permitido por NumericUpDown
            numAnioEspecialidad.Value = 1;
            
            // Permitir entrada manual de valores (sin restricción)
            numAnioEspecialidad.TextAlign = HorizontalAlignment.Right;
        }

        private async Task CargarDatosAsync()
        {
            await CargarPlanesAsync();

            if (!_esNuevo)
            {
                txtIdComision.Text = _comision.IdComision.ToString();
                txtDescComision.Text = _comision.DescComision;
                numAnioEspecialidad.Value = _comision.AnioEspecialidad;
                comboPlanes.SelectedValue = _comision.IdPlan;
            }
        }

        private async Task CargarPlanesAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var planes = await _planApiClient.GetAllAsync();
                var lista = planes.ToList();
                comboPlanes.DataSource = lista;
                comboPlanes.DisplayMember = "Descripcion";
                comboPlanes.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar planes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { Cursor.Current = Cursors.Default; }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescComision.Text))
            {
                MessageBox.Show("La descripción es obligatoria.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboPlanes.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un plan.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ComisionEditada = new ComisionDto
            {
                IdComision = _esNuevo ? 0 : _comision.IdComision,
                DescComision = txtDescComision.Text.Trim(),
                AnioEspecialidad = (int)numAnioEspecialidad.Value,
                IdPlan = (int)comboPlanes.SelectedValue
            };

            Guardado = true;
            Close();
        }
    }
}