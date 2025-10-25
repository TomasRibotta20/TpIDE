using System;
using System.Drawing;
using System.Windows.Forms;

namespace WIndowsForm
{
    public enum TipoReporte
    {
        Rapido,
        Detallado
    }

    public partial class FormSeleccionReporte : Form
    {
        public TipoReporte TipoReporteSeleccionado { get; private set; }

        public FormSeleccionReporte()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Seleccionar Tipo de Reporte";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(450, 250);
            this.BackColor = Color.White;
            
            // T�tulo principal
            var lblTitulo = new Label
            {
                Text = "Seleccione el tipo de reporte que desea generar:",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(400, 30),
                ForeColor = Color.DarkBlue,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitulo);

            // Bot�n Reporte R�pido
            var btnRapido = new Button
            {
                Text = "Reporte R�pido\n(Resumen b�sico)",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(50, 70),
                Size = new Size(160, 60),
                BackColor = Color.LightBlue,
                ForeColor = Color.DarkBlue,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnRapido.FlatAppearance.BorderColor = Color.DarkBlue;
            btnRapido.FlatAppearance.BorderSize = 2;
            btnRapido.Click += (s, e) => {
                TipoReporteSeleccionado = TipoReporte.Rapido;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            this.Controls.Add(btnRapido);

            // Bot�n Reporte Detallado
            var btnDetallado = new Button
            {
                Text = "Reporte Detallado\n(Ventana con gr�ficos)",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(240, 70),
                Size = new Size(160, 60),
                BackColor = Color.LightGreen,
                ForeColor = Color.DarkGreen,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnDetallado.FlatAppearance.BorderColor = Color.DarkGreen;
            btnDetallado.FlatAppearance.BorderSize = 2;
            btnDetallado.Click += (s, e) => {
                TipoReporteSeleccionado = TipoReporte.Detallado;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            this.Controls.Add(btnDetallado);

            // Bot�n Cancelar
            var btnCancelar = new Button
            {
                Text = "Cancelar",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(170, 170),
                Size = new Size(80, 30),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.Click += (s, e) => {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };
            this.Controls.Add(btnCancelar);

            // Descripci�n
            var lblDescripcion = new Label
            {
                Text = "� R�pido: Resumen en ventana emergente\n� Detallado: An�lisis completo con gr�ficos por curso",
                Font = new Font("Segoe UI", 8F),
                Location = new Point(30, 140),
                Size = new Size(370, 30),
                ForeColor = Color.Gray
            };
            this.Controls.Add(lblDescripcion);
        }
    }
}