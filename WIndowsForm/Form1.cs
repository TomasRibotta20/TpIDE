using API.Clients;
using DTOs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class Form1 : Form
    {
        private readonly UsuarioApiClient _apiClient;
        private readonly Form _menuPrincipal;
        private BindingList<UsuarioDto> _usuarios = new BindingList<UsuarioDto>();

        public Form1(Form menuPrincipal = null)
        {
            InitializeComponent();
            _menuPrincipal = menuPrincipal;

            try
            {
                string apiUrl = "https://localhost:7229";
                Debug.WriteLine($"Conectando a API en: {apiUrl}");
                _apiClient = new UsuarioApiClient(apiUrl);

                ConfigureForm();
                this.Load += Form1_Load;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar: {ex.Message}",
                    "Error de inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureForm()
        {
            this.Text = "Gestión de Usuarios";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel superior para DataGridView
            Panel gridPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            // DataGridView para mostrar usuarios
            DataGridView dataGridViewUsuarios = new DataGridView
            {
                Name = "dataGridViewUsuarios",
                Dock = DockStyle.Fill,
                DataSource = _usuarios,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Panel de botones
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60
            };

            Button btnNuevo = new Button
            {
                Text = "Nuevo Usuario",
                Width = 120,
                Location = new Point(10, 15)
            };

            Button btnEditar = new Button
            {
                Text = "Editar",
                Width = 100,
                Location = new Point(140, 15)
            };

            Button btnEliminar = new Button
            {
                Text = "Eliminar",
                Width = 100,
                Location = new Point(250, 15)
            };

            Button btnVolver = new Button
            {
                Text = "Volver al Menú",
                Width = 120,
                Location = new Point(gridPanel.Width - 140, 15),
                Anchor = AnchorStyles.Right
            };

            // Eventos
            btnNuevo.Click += (s, e) => CrearNuevoUsuario();
            btnEditar.Click += (s, e) => EditarUsuarioSeleccionado(dataGridViewUsuarios);
            btnEliminar.Click += (s, e) => EliminarUsuarioSeleccionado(dataGridViewUsuarios);
            btnVolver.Click += (s, e) => VolverAlMenu();

            // Agregar controles
            buttonPanel.Controls.Add(btnNuevo);
            buttonPanel.Controls.Add(btnEditar);
            buttonPanel.Controls.Add(btnEliminar);
            buttonPanel.Controls.Add(btnVolver);

            gridPanel.Controls.Add(dataGridViewUsuarios);

            this.Controls.Add(gridPanel);
            this.Controls.Add(buttonPanel);
        }

        private async void Form1_Load(object sender, EventArgs e)
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