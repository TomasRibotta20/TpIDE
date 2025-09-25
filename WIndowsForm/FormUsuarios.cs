using API.Clients;
using DTOs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormUsuarios : Form
    {
        private readonly UsuarioApiClient _apiClient;
        private readonly Form _menuPrincipal;
        private BindingList<UsuarioDto> _usuarios = new BindingList<UsuarioDto>();

        public FormUsuarios(Form menuPrincipal = null)
        {
            InitializeComponent();
            _menuPrincipal = menuPrincipal;

            try
            {
                string apiUrl = "https://localhost:7229";
                Debug.WriteLine($"Conectando a API en: {apiUrl}");
                _apiClient = new UsuarioApiClient();

                // Configurar DataGridView
                dataGridViewUsuarios.DataSource = _usuarios;

                // Asignar eventos
                this.Load += FormUsuarios_Load;
                btnNuevo.Click += (s, e) => CrearNuevoUsuario();
                btnEditar.Click += (s, e) => EditarUsuarioSeleccionado(dataGridViewUsuarios);
                btnEliminar.Click += (s, e) => EliminarUsuarioSeleccionado(dataGridViewUsuarios);
                btnVolver.Click += (s, e) => VolverAlMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar: {ex.Message}",
                    "Error de inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void FormUsuarios_Load(object sender, EventArgs e)
        {
            await LoadUsuariosAsync();
        }

        private async Task LoadUsuariosAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var usuarios = await _apiClient.GetAllAsync();

                _usuarios.Clear();
                if (usuarios != null)
                {
                    foreach (var usuario in usuarios)
                    {
                        _usuarios.Add(usuario);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async void CrearNuevoUsuario()
        {
            var formNuevoUsuario = new EditarUsuarioForm();
            formNuevoUsuario.ShowDialog();

            if (formNuevoUsuario.Guardado && formNuevoUsuario.UsuarioEditado != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.CreateAsync(formNuevoUsuario.UsuarioEditado);
                    await LoadUsuariosAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar usuario: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private async void EditarUsuarioSeleccionado(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para editar",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var usuarioSeleccionado = (UsuarioDto)dataGridView.SelectedRows[0].DataBoundItem;
            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("No se pudo obtener el usuario seleccionado",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var formEditarUsuario = new EditarUsuarioForm(usuarioSeleccionado);
            formEditarUsuario.ShowDialog();

            if (formEditarUsuario.Guardado && formEditarUsuario.UsuarioEditado != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.UpdateAsync(formEditarUsuario.UsuarioEditado);
                    await LoadUsuariosAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar usuario: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private async void EliminarUsuarioSeleccionado(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para eliminar",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var usuarioSeleccionado = (UsuarioDto)dataGridView.SelectedRows[0].DataBoundItem;
            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("No se pudo obtener el usuario seleccionado",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var resultado = MessageBox.Show(
                $"¿Está seguro que desea eliminar al usuario {usuarioSeleccionado.Nombre} {usuarioSeleccionado.Apellido}?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    await _apiClient.DeleteAsync(usuarioSeleccionado.Id);
                    await LoadUsuariosAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar usuario: {ex.Message}",
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
    }
}
