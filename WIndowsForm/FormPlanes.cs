using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Clients;
using DTOs;

namespace WIndowsForm
{
    public partial class FormPlanes : Form
    {
        private readonly PlanApiClient _planApiClient;
        private readonly EspecialidadApiClient _especialidadApiClient;
        private readonly Form _menuPrincipal;
        private readonly BindingList<PlanDto> _planes = new BindingList<PlanDto>();

        public FormPlanes(Form menuPrincipal = null)
        {
            InitializeComponent();
            _menuPrincipal = menuPrincipal;

            _planApiClient = new PlanApiClient();
            _especialidadApiClient = new EspecialidadApiClient();

            dataGridViewPlanes.DataSource = _planes;

            btnNuevo.Click += (s, e) => CrearNuevoPlan();
            btnEditar.Click += (s, e) => EditarPlanSeleccionado();
            btnEliminar.Click += (s, e) => EliminarPlanSeleccionado();
            btnVolver.Click += (s, e) => VolverAlMenu();

            this.Load += async (_, __) => await LoadPlanesAsync();
        }

        private async Task LoadPlanesAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var planes = await _planApiClient.GetAllAsync();
                _planes.Clear();
                foreach (var p in planes)
                    _planes.Add(p);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar planes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void CrearNuevoPlan()
        {
            using var form = new EditarPlanForm();
            form.ShowDialog();
            if (form.Guardado && form.PlanEditado != null)
            {
                _ = GuardarNuevoAsync(form.PlanEditado);
            }
        }

        private async Task GuardarNuevoAsync(PlanDto dto)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                await _planApiClient.CreateAsync(dto);
                await LoadPlanesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar plan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void EditarPlanSeleccionado()
        {
            if (dataGridViewPlanes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un plan.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var plan = (PlanDto)dataGridViewPlanes.SelectedRows[0].DataBoundItem;
            using var form = new EditarPlanForm(plan);
            form.ShowDialog();
            if (form.Guardado && form.PlanEditado != null)
            {
                _ = ActualizarAsync(form.PlanEditado);
            }
        }

        private async Task ActualizarAsync(PlanDto dto)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                await _planApiClient.UpdateAsync(dto);
                await LoadPlanesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar plan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { Cursor.Current = Cursors.Default; }
        }

        private void EliminarPlanSeleccionado()
        {
            if (dataGridViewPlanes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un plan.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var plan = (PlanDto)dataGridViewPlanes.SelectedRows[0].DataBoundItem;
            var res = MessageBox.Show($"¿Eliminar plan '{plan.Descripcion}'?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                _ = EliminarAsync(plan.Id);
            }
        }

        private async Task EliminarAsync(int id)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                await _planApiClient.DeleteAsync(id);
                await LoadPlanesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { Cursor.Current = Cursors.Default; }
        }

        private void VolverAlMenu()
        {
            _menuPrincipal?.Show();
            Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (_menuPrincipal != null && !_menuPrincipal.Visible)
                _menuPrincipal.Show();
        }
    }
}