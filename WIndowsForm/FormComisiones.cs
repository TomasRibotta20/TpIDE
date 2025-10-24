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
        private readonly PlanApiClient _planApiClient;
        private readonly EspecialidadApiClient _especialidadApiClient;
        private readonly Form _menuPrincipal;
        private BindingList<ComisionDto> _comisiones = new BindingList<ComisionDto>();

        public FormComisiones(Form menuPrincipal = null)
        {
            InitializeComponent();
            _menuPrincipal = menuPrincipal;

            try
            {
                Debug.WriteLine("Inicializando clientes de API");
                _apiClient = new ComisionApiClient();
                _planApiClient = new PlanApiClient();
                _especialidadApiClient = new EspecialidadApiClient();

                // Configurar DataGridView
                ConfigurarDataGridView();
                dataGridViewComisiones.DataSource = _comisiones;

                // Asignar eventos a botones
                btnNueva.Click += (s, e) => CrearNuevaComision();
                btnEditar.Click += (s, e) => EditarComisionSeleccionada();
                btnEliminar.Click += (s, e) => EliminarComisionSeleccionada();
                btnVolver.Click += (s, e) => VolverAlMenu();
                btnVerPlan.Click += (s, e) => VerDetallesPlan();

                // Suscribir al evento Load
                this.Load += FormComisiones_Load;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar: {ex.Message}",
                    "Error de inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ConfigurarDataGridView()
        {
            dataGridViewComisiones.AutoGenerateColumns = false;
            
            if (dataGridViewComisiones.Columns.Count == 0)
            {
                // Agregar columnas manualmente para controlar su apariencia
                dataGridViewComisiones.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "IdComision",
                    DataPropertyName = "IdComision",
                    HeaderText = "ID",
                    ReadOnly = true,
                    Width = 50
                });

                dataGridViewComisiones.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "DescComision",
                    DataPropertyName = "DescComision",
                    HeaderText = "Descripción",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                
                dataGridViewComisiones.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "AnioEspecialidad",
                    DataPropertyName = "AnioEspecialidad",
                    HeaderText = "Año Especialidad",
                    ReadOnly = true,
                    Width = 110
                });

                dataGridViewComisiones.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "IdPlan",
                    DataPropertyName = "IdPlan",
                    HeaderText = "ID Plan",
                    ReadOnly = true,
                    Width = 80
                });
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
                
                // Habilitar o deshabilitar el botón Ver Plan
                VerificarSeleccion();
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
        
        private async void VerDetallesPlan()
        {
            if (dataGridViewComisiones.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una comisión para ver su plan asociado.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                var comision = (ComisionDto)dataGridViewComisiones.SelectedRows[0].DataBoundItem;
                var planId = comision.IdPlan;
                
                var plan = await _planApiClient.GetByIdAsync(planId);
                
                if (plan != null)
                {
                    // Obtener la especialidad asociada para mostrar más detalles
                    var especialidad = await _especialidadApiClient.GetByIdAsync(plan.EspecialidadId);
                    string especialidadDescripcion = especialidad != null 
                        ? especialidad.Descripcion 
                        : "No disponible";
                    
                    MessageBox.Show(
                        $"Detalles del Plan\n\n" +
                        $"ID: {plan.Id}\n" +
                        $"Descripción: {plan.Descripcion}\n" +
                        $"ID Especialidad: {plan.EspecialidadId}\n" +
                        $"Especialidad: {especialidadDescripcion}",
                        "Información del Plan", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo encontrar el plan asociado.", 
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener detalles del plan: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
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
        
        private void VerificarSeleccion()
        {
            // Habilitar o deshabilitar el botón Ver Plan según la selección
            btnVerPlan.Enabled = dataGridViewComisiones.SelectedRows.Count > 0;
        }
        
        private void DataGridViewComisiones_SelectionChanged(object sender, EventArgs e)
        {
            VerificarSeleccion();
        }
    }
}