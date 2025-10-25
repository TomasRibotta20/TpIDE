using API.Clients;
using DTOs;
using System;
using System.Windows.Forms;

namespace WIndowsForm
{
    public partial class EditarCondicionForm : Form
    {
        private readonly AlumnoCursoDto _inscripcion;
        private readonly InscripcionApiClient _apiClient;

        public EditarCondicionForm(AlumnoCursoDto inscripcion)
        {
            InitializeComponent();
            _inscripcion = inscripcion;
            _apiClient = new InscripcionApiClient();
            
            ConfigurarFormulario();
            CargarDatos();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Editar Condición y Nota del Alumno";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            
            // Configurar ComboBox de condición
            cmbCondicion.Items.Add(CondicionAlumnoDto.Libre);
            cmbCondicion.Items.Add(CondicionAlumnoDto.Regular);
            cmbCondicion.Items.Add(CondicionAlumnoDto.Promocional);
            cmbCondicion.DropDownStyle = ComboBoxStyle.DropDownList;

            // Configurar NumericUpDown para nota
            numNota.Minimum = 1;
            numNota.Maximum = 10;
            numNota.DecimalPlaces = 0;
            
            // Asignar eventos
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private void CargarDatos()
        {
            lblInfo.Text = $"Alumno: {_inscripcion.NombreAlumno} {_inscripcion.ApellidoAlumno}\n" +
                          $"Curso: {_inscripcion.DescripcionCurso}";
            
            cmbCondicion.SelectedItem = _inscripcion.Condicion;
            
            if (_inscripcion.Nota.HasValue)
            {
                chkTieneNota.Checked = true;
                numNota.Value = _inscripcion.Nota.Value;
                numNota.Enabled = true;
            }
            else
            {
                chkTieneNota.Checked = false;
                numNota.Enabled = false;
            }

            chkTieneNota.CheckedChanged += (s, e) => numNota.Enabled = chkTieneNota.Checked;
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCondicion.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar una condición para el alumno.", "Campo Requerido", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbCondicion.Focus();
                    return;
                }

                if (chkTieneNota.Checked && numNota.Value < 1)
                {
                    MessageBox.Show("La nota debe ser un valor entre 1 y 10.", "Nota Inválida", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numNota.Focus();
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                
                var condicion = (CondicionAlumnoDto)cmbCondicion.SelectedItem;
                int? nota = chkTieneNota.Checked ? (int)numNota.Value : null;

                await _apiClient.ActualizarCondicionYNotaAsync(_inscripcion.IdInscripcion, condicion, nota);
                
                string mensaje = $"Condición actualizada correctamente.\n\n";
                mensaje += $"Alumno: {_inscripcion.NombreAlumno} {_inscripcion.ApellidoAlumno}\n";
                mensaje += $"Nueva condición: {condicion}";
                if (nota.HasValue)
                {
                    mensaje += $"\nNota asignada: {nota}";
                }
                
                MessageBox.Show(mensaje, "Actualización Exitosa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                string mensajeError = InterpretarErrorActualizacion(ex.Message);
                MessageBox.Show(mensajeError, "Error al Actualizar",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private string InterpretarErrorActualizacion(string errorOriginal)
        {
            string errorLower = errorOriginal.ToLower();
            
            // Casos específicos con códigos de error
            switch (errorOriginal)
            {
                case "INSCRIPCION_NO_ENCONTRADA":
                    return "La inscripción ya no existe en el sistema.\n\nEs posible que haya sido eliminada. Cierre esta ventana y actualice la lista.";
                case "DATOS_INVALIDOS":
                    return "Los datos enviados no son válidos.\n\nVerifique la condición y nota seleccionadas.";
                case "ERROR_SERVIDOR":
                    return "Error interno del servidor.\n\nIntente nuevamente en unos momentos. Si el problema persiste, contacte al administrador.";
            }
            
            // Detección por contenido del mensaje
            if (errorLower.Contains("not found") || errorLower.Contains("404"))
            {
                return "La inscripción ya no existe en el sistema.\n\nEs posible que haya sido eliminada. Cierre esta ventana y actualice la lista.";
            }
            
            if (errorLower.Contains("condición inválida") || (errorLower.Contains("condición") && errorLower.Contains("inválida")))
            {
                return "La condición seleccionada no es válida.\n\nDebe seleccionar: Libre, Regular o Promocional.";
            }
            
            if (errorLower.Contains("nota") && errorLower.Contains("inválida"))
            {
                return "La nota ingresada no es válida.\n\nDebe ser un número entre 1 y 10.";
            }
            
            if (errorLower.Contains("object") && errorLower.Contains("string"))
            {
                return "Error en el formato de los datos.\n\nIntente nuevamente. Si el problema persiste, contacte al administrador.";
            }
            
            if (errorLower.Contains("400") || errorLower.Contains("bad request"))
            {
                return "Los datos enviados no son correctos.\n\nVerifique la información ingresada e intente nuevamente.";
            }
            
            if (errorLower.Contains("500") || errorLower.Contains("internal server"))
            {
                return "Error interno del servidor.\n\nIntente nuevamente en unos momentos. Si persiste, contacte al administrador.";
            }
            
            // Si el error es muy técnico, simplificarlo
            if (errorOriginal.Contains("JsonException") || errorOriginal.Contains("HttpRequestException"))
            {
                return "Error de comunicación con el servidor.\n\nVerifique su conexión e intente nuevamente.";
            }
            
            // Si el mensaje del servidor es descriptivo, mostrarlo directamente
            if (!errorOriginal.Contains("Exception") && !errorOriginal.Contains("Stack") && errorOriginal.Length < 200)
            {
                return errorOriginal;
            }
            
            return $"Error inesperado al actualizar la condición.\n\nSi el problema persiste, contacte al administrador.\n\nDetalle: {errorOriginal}";
        }
    }
}