using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class FormEditarDocenteCurso : Form
    {
        private List<CursoDto> _cursos;
        private List<PersonaDto> _profesores;
        private DocenteCursoDto? _asignacionExistente;

        // ComboBoxes como campos privados
        private ComboBox cmbCurso;
        private ComboBox cmbProfesor;
        private ComboBox cmbCargo;
        private Panel contentPanel;

        public DocenteCursoCreateDto? AsignacionCreada { get; private set; }

        public FormEditarDocenteCurso(List<CursoDto> cursos, List<PersonaDto> profesores, DocenteCursoDto? asignacionExistente = null)
        {
            _cursos = cursos;
            _profesores = profesores;
            _asignacionExistente = asignacionExistente;

            InitializeComponent();
            CargarDatos();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Configuración del Form
            this.Text = _asignacionExistente == null ? "Nueva Asignación" : "Editar Asignación";
            this.Size = new System.Drawing.Size(700, 450); // Reducido el alto para mejor visualización
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = FormStyles.Colors.Background;

            // Header Panel
            var headerPanel = FormStyles.CreateHeaderPanel(
                _asignacionExistente == null ? "Nueva Asignación" : "Editar Asignación",
                "Complete los datos de la asignación de docente");
            headerPanel.Dock = DockStyle.Top;
            this.Controls.Add(headerPanel);

            // Content Panel
            contentPanel = new Panel
            {
                BackColor = FormStyles.Colors.CardBackground,
                Dock = DockStyle.Fill,
                Padding = new Padding(30, 20, 30, 20)
            };

            int yPos = 20;

            // Curso
            var lblCurso = new Label
            {
                Text = "Curso*:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(100, 25),
                Font = FormStyles.Fonts.Button,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            FormStyles.StyleLabel(lblCurso, true);

            cmbCurso = new ComboBox
            {
                Location = new System.Drawing.Point(130, yPos),
                Size = new System.Drawing.Size(500, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = FormStyles.Fonts.Normal,
                TabIndex = 0
            };
            FormStyles.StyleComboBox(cmbCurso);

            contentPanel.Controls.Add(lblCurso);
            contentPanel.Controls.Add(cmbCurso);
            yPos += 45;

            // Profesor
            var lblProfesor = new Label
            {
                Text = "Profesor*:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(100, 25),
                Font = FormStyles.Fonts.Button,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            FormStyles.StyleLabel(lblProfesor, true);

            cmbProfesor = new ComboBox
            {
                Location = new System.Drawing.Point(130, yPos),
                Size = new System.Drawing.Size(500, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = FormStyles.Fonts.Normal,
                TabIndex = 1
            };
            FormStyles.StyleComboBox(cmbProfesor);

            contentPanel.Controls.Add(lblProfesor);
            contentPanel.Controls.Add(cmbProfesor);
            yPos += 45;

            // Cargo
            var lblCargo = new Label
            {
                Text = "Cargo*:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(100, 25),
                Font = FormStyles.Fonts.Button,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            FormStyles.StyleLabel(lblCargo, true);

            cmbCargo = new ComboBox
            {
                Location = new System.Drawing.Point(130, yPos),
                Size = new System.Drawing.Size(300, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = FormStyles.Fonts.Normal,
                TabIndex = 2
            };
            FormStyles.StyleComboBox(cmbCargo);

            contentPanel.Controls.Add(lblCargo);
            contentPanel.Controls.Add(cmbCargo);
            yPos += 45;

            // Info Panel
            var infoPanel = new Panel
            {
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(610, 120),
                BackColor = FormStyles.Colors.Background,
                Padding = new Padding(15)
            };

            var lblInfo = new Label
            {
                Text = "Los cargos disponibles son:\n\n" +
                       "• Jefe de Cátedra: Responsable principal de la materia\n" +
                       "• Titular: Profesor titular del curso\n" +
                       "• Auxiliar: Profesor auxiliar o ayudante",
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(590, 100),
                Font = FormStyles.Fonts.Normal,
                ForeColor = FormStyles.Colors.TextSecondary
            };

            infoPanel.Controls.Add(lblInfo);
            contentPanel.Controls.Add(infoPanel);

            this.Controls.Add(contentPanel);

            // Button Panel
            var buttonPanel = new Panel
            {
                BackColor = FormStyles.Colors.Background,
                Dock = DockStyle.Bottom,
                Height = 80,
                Padding = new Padding(20, 20, 20, 20)
            };

            var btnGuardar = FormStyles.CreateSuccessButton("Guardar", null);
            btnGuardar.Location = new System.Drawing.Point(20, 15);
            btnGuardar.Size = new System.Drawing.Size(130, 40);
            btnGuardar.Click += BtnGuardar_Click;

            var btnCancelar = FormStyles.CreateSecondaryButton("Cancelar", null);
            btnCancelar.Location = new System.Drawing.Point(160, 15);
            btnCancelar.Size = new System.Drawing.Size(130, 40);
            btnCancelar.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            buttonPanel.Controls.AddRange(new Control[] { btnGuardar, btnCancelar });
            this.Controls.Add(buttonPanel);

            // Establecer orden de controles
            this.Controls.SetChildIndex(buttonPanel, 0);
            this.Controls.SetChildIndex(contentPanel, 1);
            this.Controls.SetChildIndex(headerPanel, 2);

            this.ResumeLayout(false);
        }

        private void CargarDatos()
        {
            // Cargar cursos
            if (cmbCurso != null)
            {
                cmbCurso.Items.Clear();
                foreach (var curso in _cursos.OrderBy(c => c.NombreMateria))
                {
                    var descripcion = $"{curso.NombreMateria} - {curso.DescComision} ({curso.AnioCalendario})";
                    cmbCurso.Items.Add(new { Display = descripcion, Value = curso.IdCurso });
                }
                cmbCurso.DisplayMember = "Display";
                cmbCurso.ValueMember = "Value";
            }

            // Cargar profesores
            if (cmbProfesor != null)
            {
                cmbProfesor.Items.Clear();
                foreach (var profesor in _profesores.OrderBy(p => p.Apellido).ThenBy(p => p.Nombre))
                {
                    var descripcion = $"{profesor.Apellido}, {profesor.Nombre} (Legajo: {profesor.Legajo})";
                    cmbProfesor.Items.Add(new { Display = descripcion, Value = profesor.Id });
                }
                cmbProfesor.DisplayMember = "Display";
                cmbProfesor.ValueMember = "Value";
            }

            // Cargar cargos
            if (cmbCargo != null)
            {
                cmbCargo.Items.Clear();
                cmbCargo.Items.Add(new { Display = "Jefe de Cátedra", Value = TipoCargoDto.JefeDeCatedra });
                cmbCargo.Items.Add(new { Display = "Titular", Value = TipoCargoDto.Titular });
                cmbCargo.Items.Add(new { Display = "Auxiliar", Value = TipoCargoDto.Auxiliar });
                cmbCargo.DisplayMember = "Display";
                cmbCargo.ValueMember = "Value";
            }

            // Si es edición, seleccionar valores existentes
            if (_asignacionExistente != null)
            {
                if (cmbCurso != null)
                {
                    for (int i = 0; i < cmbCurso.Items.Count; i++)
                    {
                        dynamic item = cmbCurso.Items[i];
                        if (item.Value == _asignacionExistente.IdCurso)
                        {
                            cmbCurso.SelectedIndex = i;
                            break;
                        }
                    }
                }

                if (cmbProfesor != null)
                {
                    for (int i = 0; i < cmbProfesor.Items.Count; i++)
                    {
                        dynamic item = cmbProfesor.Items[i];
                        if (item.Value == _asignacionExistente.IdDocente)
                        {
                            cmbProfesor.SelectedIndex = i;
                            break;
                        }
                    }
                }

                if (cmbCargo != null)
                {
                    for (int i = 0; i < cmbCargo.Items.Count; i++)
                    {
                        dynamic item = cmbCargo.Items[i];
                        if (item.Value.Equals(_asignacionExistente.Cargo))
                        {
                            cmbCargo.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (cmbCurso?.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar un curso.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbCurso?.Focus();
                    return;
                }

                if (cmbProfesor?.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar un profesor.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbProfesor?.Focus();
                    return;
                }

                if (cmbCargo?.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar un cargo.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbCargo?.Focus();
                    return;
                }

                // Crear DTO
                dynamic cursoItem = cmbCurso.SelectedItem;
                dynamic profesorItem = cmbProfesor.SelectedItem;
                dynamic cargoItem = cmbCargo.SelectedItem;

                AsignacionCreada = new DocenteCursoCreateDto
                {
                    IdCurso = cursoItem.Value,
                    IdDocente = profesorItem.Value,
                    Cargo = cargoItem.Value
                };

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
