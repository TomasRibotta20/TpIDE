using DTOs;
using System;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class EditarEspecialidadForm : Form
    {
        private readonly EspecialidadDto _especialidad;
        private readonly bool _esNueva;

        public EspecialidadDto EspecialidadEditada { get; private set; }
        public bool Guardado { get; private set; }

        public EditarEspecialidadForm(EspecialidadDto especialidad = null)
        {
            InitializeComponent();
            _especialidad = especialidad ?? new EspecialidadDto();
            _esNueva = especialidad == null;
            ConfigureForm();
        }

        private void ConfigureForm()
        {
            this.Text = _esNueva ? "Nueva Especialidad" : "Editar Especialidad";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(450, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // TableLayoutPanel para organizar los campos
            TableLayoutPanel tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 2,
                RowCount = 3,
                ColumnStyles = {
                    new ColumnStyle(SizeType.Percent, 30),
                    new ColumnStyle(SizeType.Percent, 70)
                }
            };

            // Agregar filas con alturas específicas
            for (int i = 0; i < 3; i++)
            {
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            }

            int row = 0;

            // Solo mostrar ID si es edición
            if (!_esNueva)
            {
                Label lblId = new Label { Text = "ID:", Anchor = AnchorStyles.Left | AnchorStyles.Right };
                TextBox txtId = new TextBox
                {
                    Text = _especialidad.Id.ToString(),
                    ReadOnly = true,
                    Dock = DockStyle.Fill
                };

                tableLayout.Controls.Add(lblId, 0, row);
                tableLayout.Controls.Add(txtId, 1, row);
                row++;
            }

            // Descripción
            Label lblDescripcion = new Label { Text = "Descripción*:", Anchor = AnchorStyles.Left | AnchorStyles.Right };
            TextBox txtDescripcion = new TextBox
            {
                Text = _especialidad.Descripcion,
                Dock = DockStyle.Fill,
                Multiline = true,
                Height = 80
            };

            tableLayout.Controls.Add(lblDescripcion, 0, row);
            tableLayout.Controls.Add(txtDescripcion, 1, row);
            row++;

            // Panel de botones
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 40
            };

            Button btnGuardar = new Button
            {
                Text = "Guardar",
                Width = 100,
                Location = new Point(50, 5)
            };

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Width = 100,
                Location = new Point(160, 5)
            };

            btnGuardar.Click += (s, e) => {
                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    MessageBox.Show("La descripción de la especialidad es obligatoria",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Guardar datos en la especialidad
                EspecialidadEditada = new EspecialidadDto
                {
                    Id = _esNueva ? 0 : _especialidad.Id,
                    Descripcion = txtDescripcion.Text
                };

                Guardado = true;
                this.Close();
            };

            btnCancelar.Click += (s, e) => this.Close();

            buttonPanel.Controls.Add(btnGuardar);
            buttonPanel.Controls.Add(btnCancelar);

            tableLayout.Controls.Add(buttonPanel, 1, row);

            this.Controls.Add(tableLayout);
            this.AcceptButton = btnGuardar;
            this.CancelButton = btnCancelar;
        }
    }
}