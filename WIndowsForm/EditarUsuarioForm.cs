using DTOs;
using API.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class EditarUsuarioForm : Form
    {
        private readonly UsuarioDto _usuario;
        private readonly bool _esNuevo;
        private readonly UsuarioApiClient _usuarioApiClient;
        private readonly PersonaApiClient _personaApiClient;

        public UsuarioDto UsuarioEditado { get; private set; }
        public bool Guardado { get; private set; }

        // Controles adicionales para persona
        private ComboBox cmbPersona;
        private Label lblPersona;
        private CheckBox chkEsAdministrador;
        private Label lblTipoDetectado;

        public EditarUsuarioForm(UsuarioDto usuario = null)
        {
            InitializeComponent();

            _usuarioApiClient = new UsuarioApiClient();
            _personaApiClient = new PersonaApiClient();
            _usuario = usuario ?? new UsuarioDto { Habilitado = true };
            _esNuevo = usuario == null;

            // Crear controles adicionales
            CrearControlesAdicionales();

            // Configuración específica que depende de si es nuevo o edición
            ConfigurarFormulario();
            
            // Cargar datos de manera asíncrona
            this.Load += async (s, e) => await CargarDatosAsync();

            // Asignar eventos
            btnGuardar.Click += async (s, e) => await BtnGuardar_ClickAsync(s, e);
            btnCancelar.Click += (s, e) => this.Close();
        }

        private void CrearControlesAdicionales()
        {
            // Crear CheckBox para indicar si es Administrador
            chkEsAdministrador = new CheckBox
            {
                Text = "Es Administrador (sin persona asociada)",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Checked = false
            };
            chkEsAdministrador.CheckedChanged += ChkEsAdministrador_CheckedChanged;

            // Crear ComboBox para Persona
            lblPersona = new Label
            {
                Text = "Persona Asociada:",
                AutoSize = true,
                Anchor = AnchorStyles.Right
            };

            cmbPersona = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Width = 200
            };
            cmbPersona.SelectedIndexChanged += CmbPersona_SelectedIndexChanged;

            // Label para mostrar el tipo detectado automáticamente
            lblTipoDetectado = new Label
            {
                Text = "",
                AutoSize = true,
                ForeColor = System.Drawing.Color.Blue,
                Font = new System.Drawing.Font(this.Font, System.Drawing.FontStyle.Bold),
                Anchor = AnchorStyles.Left
            };

            // Agregar controles al TableLayoutPanel (después de Email, antes de Habilitado)
            // Asumiendo que Email está en la fila 5
            int insertRow = 6;
            
            // Insertar fila para CheckBox Administrador
            tableLayoutPanel1.RowCount++;
            tableLayoutPanel1.RowStyles.Insert(insertRow, new RowStyle(SizeType.AutoSize));
            
            // Mover controles existentes hacia abajo
            for (int i = tableLayoutPanel1.RowCount - 1; i > insertRow; i--)
            {
                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    if (tableLayoutPanel1.GetRow(control) == i - 1)
                    {
                        tableLayoutPanel1.SetRow(control, i);
                    }
                }
            }

            tableLayoutPanel1.Controls.Add(chkEsAdministrador, 0, insertRow);
            tableLayoutPanel1.SetColumnSpan(chkEsAdministrador, 2);

            // Insertar fila para Persona
            insertRow++;
            tableLayoutPanel1.RowCount++;
            tableLayoutPanel1.RowStyles.Insert(insertRow, new RowStyle(SizeType.AutoSize));
            
            // Mover controles existentes hacia abajo
            for (int i = tableLayoutPanel1.RowCount - 1; i > insertRow; i--)
            {
                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    if (tableLayoutPanel1.GetRow(control) == i - 1)
                    {
                        tableLayoutPanel1.SetRow(control, i);
                    }
                }
            }

            tableLayoutPanel1.Controls.Add(lblPersona, 0, insertRow);
            tableLayoutPanel1.Controls.Add(cmbPersona, 1, insertRow);

            // Insertar fila para tipo detectado
            insertRow++;
            tableLayoutPanel1.RowCount++;
            tableLayoutPanel1.RowStyles.Insert(insertRow, new RowStyle(SizeType.AutoSize));
            
            // Mover controles existentes hacia abajo
            for (int i = tableLayoutPanel1.RowCount - 1; i > insertRow; i--)
            {
                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    if (tableLayoutPanel1.GetRow(control) == i - 1)
                    {
                        tableLayoutPanel1.SetRow(control, i);
                    }
                }
            }

            tableLayoutPanel1.Controls.Add(lblTipoDetectado, 0, insertRow);
            tableLayoutPanel1.SetColumnSpan(lblTipoDetectado, 2);
        }

        private void ConfigurarFormulario()
        {
            this.Text = _esNuevo ? "Nuevo Usuario" : "Editar Usuario";

            // El campo ID solo se muestra en modo edición
            if (_esNuevo)
            {
                // Ocultar la fila del ID
                tableLayoutPanel1.RowStyles[0].Height = 0;
                lblId.Visible = false;
                txtId.Visible = false;
            }

            // Establecer AcceptButton y CancelButton
            this.AcceptButton = btnGuardar;
            this.CancelButton = btnCancelar;
        }

        private void ChkEsAdministrador_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEsAdministrador.Checked)
            {
                // Es administrador: deshabilitar selección de persona
                cmbPersona.Enabled = false;
                cmbPersona.SelectedIndex = -1;
                lblPersona.Text = "Persona Asociada:";
                lblTipoDetectado.Text = "Tipo: ADMINISTRADOR (acceso total al sistema)";
            }
            else
            {
                // No es administrador: habilitar selección de persona
                cmbPersona.Enabled = true;
                lblPersona.Text = "Persona Asociada*:";
                if (cmbPersona.SelectedIndex == -1)
                {
                    lblTipoDetectado.Text = "";
                }
            }
        }

        private void CmbPersona_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPersona.SelectedValue != null && cmbPersona.SelectedValue is PersonaDto persona)
            {
                // Detectar automáticamente el tipo según la persona seleccionada
                if (persona.TipoPersona == TipoPersonaDto.Profesor)
                {
                    lblTipoDetectado.Text = "Tipo: PROFESOR (puede gestionar cursos e inscripciones)";
                }
                else if (persona.TipoPersona == TipoPersonaDto.Alumno)
                {
                    lblTipoDetectado.Text = "Tipo: ALUMNO (puede inscribirse a cursos)";
                }
            }
            else
            {
                if (!chkEsAdministrador.Checked)
                {
                    lblTipoDetectado.Text = "";
                }
            }
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Cargar todas las personas (profesores y alumnos)
                var todasLasPersonas = await _personaApiClient.GetAllAsync();
                
                cmbPersona.DataSource = null;
                cmbPersona.DisplayMember = "Display";
                cmbPersona.ValueMember = "Persona";
                cmbPersona.DataSource = todasLasPersonas
                    .Where(p => p.TipoPersona == TipoPersonaDto.Profesor || p.TipoPersona == TipoPersonaDto.Alumno)
                    .Select(p => new
                    {
                        Persona = p,
                        Display = $"{p.Apellido}, {p.Nombre} (Leg: {p.Legajo}) - {(p.TipoPersona == TipoPersonaDto.Profesor ? "PROFESOR" : "ALUMNO")}"
                    })
                    .ToList();

                cmbPersona.SelectedIndex = -1;

                // Cargar datos del usuario
                if (!_esNuevo)
                {
                    txtId.Text = _usuario.Id.ToString();
                }

                txtNombre.Text = _usuario.Nombre;
                txtApellido.Text = _usuario.Apellido;
                txtUsuario.Text = _usuario.UsuarioNombre;
                txtContrasenia.Text = ""; // No mostrar contraseña existente
                txtEmail.Text = _usuario.Email;
                chkHabilitado.Checked = _usuario.Habilitado;

                // Determinar tipo de usuario según PersonaId
                if (_usuario.PersonaId == null)
                {
                    // Es administrador
                    chkEsAdministrador.Checked = true;
                }
                else if (_usuario.persona != null)
                {
                    // Tiene persona asociada
                    chkEsAdministrador.Checked = false;
                    
                    // Buscar y seleccionar la persona en el combo
                    var itemSeleccionado = ((List<dynamic>)cmbPersona.DataSource)
                        .FirstOrDefault(item => ((PersonaDto)item.Persona).Id == _usuario.PersonaId.Value);
                    
                    if (itemSeleccionado != null)
                    {
                        cmbPersona.SelectedItem = itemSeleccionado;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async Task BtnGuardar_ClickAsync(object sender, EventArgs e)
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Todos los campos marcados con * son obligatorios",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar contraseña solo para usuarios nuevos
            if (_esNuevo && string.IsNullOrWhiteSpace(txtContrasenia.Text))
            {
                MessageBox.Show("La contraseña es obligatoria para usuarios nuevos",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar persona si no es administrador
            if (!chkEsAdministrador.Checked && cmbPersona.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar una persona o marcar 'Es Administrador'",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener PersonaId
            int? personaId = null;
            if (!chkEsAdministrador.Checked && cmbPersona.SelectedValue is PersonaDto persona)
            {
                personaId = persona.Id;
            }

            // Guardar datos en el usuario
            UsuarioEditado = new UsuarioDto
            {
                Id = _esNuevo ? 0 : _usuario.Id,
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                UsuarioNombre = txtUsuario.Text,
                Contrasenia = string.IsNullOrWhiteSpace(txtContrasenia.Text) ? null : txtContrasenia.Text,
                Email = txtEmail.Text,
                Habilitado = chkHabilitado.Checked,
                PersonaId = personaId
            };

            Guardado = true;
            this.Close();
        }
    }
}
