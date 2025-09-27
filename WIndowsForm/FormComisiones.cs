using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Clients;
using DTOs;

namespace WIndowsForm
{
    public partial class FormComisiones : Form
    {
        private readonly ComisionApiClient _apiClient;
        private readonly Form _menuPrincipal;
        private BindingList<ComisionDto> _comisiones = new BindingList<ComisionDto>();

        public FormComisiones(Form menuPrincipal = null)
        {
            InitializeComponent();
            _menuPrincipal = menuPrincipal;

            try
            {
                Debug.WriteLine("Inicializando cliente de API de comisiones");
                _apiClient = new ComisionApiClient();

                // Configurar DataGridView
                dataGridViewComisiones.DataSource = _comisiones;

                // Asignar eventos a botones
                btnNueva.Click += (s, e) => CrearNuevaComision();
                btnEditar.Click += (s, e) => EditarComisionSeleccionada();
                btnEliminar.Click += (s, e) => EliminarComisionSeleccionada();
                btnVolver.Click += (s, e) => VolverAlMenu();

                // Suscribir al evento Load
                this.Load += FormComisiones_Load;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar: {ex.Message}",
                    "Error de inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void FormComisiones_Load(object sender, EventArgs e)
        {
            await LoadComisionesAsync();
        }

        private async Task LoadComisionesAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var comisiones = await _apiClient.GetAllAsync();

                _comisiones.Clear();
                if (comisiones != null)
                {
                    foreach (var comision in comisiones)
                    {
                        _comisiones.Add(comision);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar comisiones: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async void CrearNuevaComision()
        {
            var formNuevaComision = new EditarComisionForm();
            formNuevaComision.ShowDialog();

            if (formNuevaComision.Guardado && formNuevaComision.ComisionEditada != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.CreateAsync(formNuevaComision.ComisionEditada);
                    await LoadComisionesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar comisión: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private async void EditarComisionSeleccionada()
        {
            if (dataGridViewComisiones.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una comisión para editar",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var comisionSeleccionada = (ComisionDto)dataGridViewComisiones.SelectedRows[0].DataBoundItem;
            if (comisionSeleccionada == null)
            {
                MessageBox.Show("No se pudo obtener la comisión seleccionada",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var formEditarComision = new EditarComisionForm(comisionSeleccionada);
            formEditarComision.ShowDialog();

            if (formEditarComision.Guardado && formEditarComision.ComisionEditada != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.UpdateAsync(formEditarComision.ComisionEditada);
                    await LoadComisionesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar comisión: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private async void EliminarComisionSeleccionada()
        {
            if (dataGridViewComisiones.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una comisión para eliminar",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var comisionSeleccionada = (ComisionDto)dataGridViewComisiones.SelectedRows[0].DataBoundItem;
            if (comisionSeleccionada == null)
            {
                MessageBox.Show("No se pudo obtener la comisión seleccionada",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var resultado = MessageBox.Show(
                $"¿Está seguro que desea eliminar la comisión '{comisionSeleccionada.DescComision}'?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.DeleteAsync(comisionSeleccionada.IdComision);
                    await LoadComisionesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar comisión: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void VolverAlMenu()
        {
            if (_menuPrincipal != null)
            {
                _menuPrincipal.Show();
                this.Close();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (_menuPrincipal != null && !_menuPrincipal.Visible)
            {
                _menuPrincipal.Show();
            }
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridViewComisiones = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnVolver = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnNueva = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComisiones)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTitulo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 60);
            this.panel1.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(784, 60);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Gestión de Comisiones";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridViewComisiones);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 60);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(784, 351);
            this.panel2.TabIndex = 1;
            // 
            // dataGridViewComisiones
            // 
            this.dataGridViewComisiones.AllowUserToAddRows = false;
            this.dataGridViewComisiones.AllowUserToDeleteRows = false;
            this.dataGridViewComisiones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewComisiones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewComisiones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewComisiones.Location = new System.Drawing.Point(10, 10);
            this.dataGridViewComisiones.MultiSelect = false;
            this.dataGridViewComisiones.Name = "dataGridViewComisiones";
            this.dataGridViewComisiones.ReadOnly = true;
            this.dataGridViewComisiones.RowHeadersVisible = false;
            this.dataGridViewComisiones.RowTemplate.Height = 25;
            this.dataGridViewComisiones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewComisiones.Size = new System.Drawing.Size(764, 331);
            this.dataGridViewComisiones.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnVolver);
            this.panel3.Controls.Add(this.btnEliminar);
            this.panel3.Controls.Add(this.btnEditar);
            this.panel3.Controls.Add(this.btnNueva);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 411);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(784, 50);
            this.panel3.TabIndex = 2;
            // 
            // btnVolver
            // 
            this.btnVolver.Location = new System.Drawing.Point(12, 9);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(100, 30);
            this.btnVolver.TabIndex = 3;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminar.Location = new System.Drawing.Point(670, 9);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(100, 30);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            // 
            // btnEditar
            // 
            this.btnEditar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditar.Location = new System.Drawing.Point(562, 9);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(100, 30);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            // 
            // btnNueva
            // 
            this.btnNueva.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNueva.Location = new System.Drawing.Point(454, 9);
            this.btnNueva.Name = "btnNueva";
            this.btnNueva.Size = new System.Drawing.Size(100, 30);
            this.btnNueva.TabIndex = 0;
            this.btnNueva.Text = "Nueva";
            this.btnNueva.UseVisualStyleBackColor = true;
            // 
            // FormComisiones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "FormComisiones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Comisiones";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComisiones)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridViewComisiones;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnNueva;

        #endregion
    }
}