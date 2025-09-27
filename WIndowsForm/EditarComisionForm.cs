using System;
using System.Collections.Generic;
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
            
            // Configurar el control numérico para el año de especialidad
            numAnioEspecialidad.Minimum = 1;
            numAnioEspecialidad.Maximum = 10;
            numAnioEspecialidad.Value = 1;
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

        #region Windows Form Designer generated code

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblIdComision = new System.Windows.Forms.Label();
            this.txtIdComision = new System.Windows.Forms.TextBox();
            this.lblDescComision = new System.Windows.Forms.Label();
            this.txtDescComision = new System.Windows.Forms.TextBox();
            this.lblAnioEspecialidad = new System.Windows.Forms.Label();
            this.numAnioEspecialidad = new System.Windows.Forms.NumericUpDown();
            this.lblPlan = new System.Windows.Forms.Label();
            this.comboPlanes = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAnioEspecialidad)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.lblIdComision, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtIdComision, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblDescComision, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtDescComision, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblAnioEspecialidad, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.numAnioEspecialidad, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblPlan, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboPlanes, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(364, 140);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblIdComision
            // 
            this.lblIdComision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIdComision.Location = new System.Drawing.Point(3, 0);
            this.lblIdComision.Name = "lblIdComision";
            this.lblIdComision.Size = new System.Drawing.Size(103, 35);
            this.lblIdComision.TabIndex = 0;
            this.lblIdComision.Text = "ID:";
            this.lblIdComision.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtIdComision
            // 
            this.txtIdComision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIdComision.Location = new System.Drawing.Point(112, 3);
            this.txtIdComision.Name = "txtIdComision";
            this.txtIdComision.ReadOnly = true;
            this.txtIdComision.Size = new System.Drawing.Size(249, 23);
            this.txtIdComision.TabIndex = 1;
            // 
            // lblDescComision
            // 
            this.lblDescComision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescComision.Location = new System.Drawing.Point(3, 35);
            this.lblDescComision.Name = "lblDescComision";
            this.lblDescComision.Size = new System.Drawing.Size(103, 35);
            this.lblDescComision.TabIndex = 2;
            this.lblDescComision.Text = "Descripción:";
            this.lblDescComision.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDescComision
            // 
            this.txtDescComision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescComision.Location = new System.Drawing.Point(112, 38);
            this.txtDescComision.Name = "txtDescComision";
            this.txtDescComision.Size = new System.Drawing.Size(249, 23);
            this.txtDescComision.TabIndex = 3;
            // 
            // lblAnioEspecialidad
            // 
            this.lblAnioEspecialidad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAnioEspecialidad.Location = new System.Drawing.Point(3, 70);
            this.lblAnioEspecialidad.Name = "lblAnioEspecialidad";
            this.lblAnioEspecialidad.Size = new System.Drawing.Size(103, 35);
            this.lblAnioEspecialidad.TabIndex = 4;
            this.lblAnioEspecialidad.Text = "Año Especialidad:";
            this.lblAnioEspecialidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numAnioEspecialidad
            // 
            this.numAnioEspecialidad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numAnioEspecialidad.Location = new System.Drawing.Point(112, 73);
            this.numAnioEspecialidad.Name = "numAnioEspecialidad";
            this.numAnioEspecialidad.Size = new System.Drawing.Size(249, 23);
            this.numAnioEspecialidad.TabIndex = 5;
            // 
            // lblPlan
            // 
            this.lblPlan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPlan.Location = new System.Drawing.Point(3, 105);
            this.lblPlan.Name = "lblPlan";
            this.lblPlan.Size = new System.Drawing.Size(103, 35);
            this.lblPlan.TabIndex = 6;
            this.lblPlan.Text = "Plan:";
            this.lblPlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboPlanes
            // 
            this.comboPlanes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboPlanes.FormattingEnabled = true;
            this.comboPlanes.Location = new System.Drawing.Point(112, 108);
            this.comboPlanes.Name = "comboPlanes";
            this.comboPlanes.Size = new System.Drawing.Size(249, 23);
            this.comboPlanes.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnCancelar);
            this.flowLayoutPanel1.Controls.Add(this.btnGuardar);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 207);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(364, 33);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(205, 3);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 27);
            this.btnGuardar.TabIndex = 0;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(286, 3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 27);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // EditarComisionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 250);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditarComisionForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Comisión";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAnioEspecialidad)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblIdComision;
        private System.Windows.Forms.TextBox txtIdComision;
        private System.Windows.Forms.Label lblDescComision;
        private System.Windows.Forms.TextBox txtDescComision;
        private System.Windows.Forms.Label lblAnioEspecialidad;
        private System.Windows.Forms.NumericUpDown numAnioEspecialidad;
        private System.Windows.Forms.Label lblPlan;
        private System.Windows.Forms.ComboBox comboPlanes;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnGuardar;

        #endregion
    }
}