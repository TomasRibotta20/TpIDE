namespace WIndowsForm
{
    partial class FormProfesores
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                // Liberar el HttpClient del API

            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            gridPanel = new Panel();
            dataGridViewProfesores = new DataGridView();
            buttonPanel = new Panel();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            btnVolver = new Button();
            gridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewProfesores).BeginInit();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // gridPanel
            // 
            gridPanel.Controls.Add(buttonPanel);
            gridPanel.Controls.Add(dataGridViewProfesores);
            gridPanel.Dock = DockStyle.Fill;
            gridPanel.Location = new Point(0, 0);
            gridPanel.Name = "gridPanel";
            gridPanel.Padding = new Padding(10);
            gridPanel.Size = new Size(784, 561);
            gridPanel.TabIndex = 0;
            // 
            // dataGridViewProfesores
            // 
            dataGridViewProfesores.AllowUserToAddRows = false;
            dataGridViewProfesores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProfesores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewProfesores.Dock = DockStyle.Fill;
            dataGridViewProfesores.Location = new Point(10, 10);
            dataGridViewProfesores.MultiSelect = false;
            dataGridViewProfesores.Name = "dataGridViewProfesores";
            dataGridViewProfesores.ReadOnly = true;
            dataGridViewProfesores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProfesores.Size = new Size(764, 541);
            dataGridViewProfesores.TabIndex = 0;
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(btnVolver);
            buttonPanel.Controls.Add(btnEliminar);
            buttonPanel.Controls.Add(btnEditar);
            buttonPanel.Controls.Add(btnNuevo);
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Location = new Point(10, 491);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(764, 60);
            buttonPanel.TabIndex = 1;
            // 
            // btnNuevo
            // 
            btnNuevo.Location = new Point(10, 15);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(120, 30);
            btnNuevo.TabIndex = 0;
            btnNuevo.Text = "Nuevo Profesor";
            btnNuevo.UseVisualStyleBackColor = true;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(140, 15);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(100, 30);
            btnEditar.TabIndex = 1;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(250, 15);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(100, 30);
            btnEliminar.TabIndex = 2;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            // 
            // btnVolver
            // 
            btnVolver.Anchor = AnchorStyles.Right;
            btnVolver.Location = new Point(640, 15);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(120, 30);
            btnVolver.TabIndex = 3;
            btnVolver.Text = "Volver al Menu";
            btnVolver.UseVisualStyleBackColor = true;
            // 
            // FormProfesores
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(gridPanel);
            Name = "FormProfesores";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gesti�n de Profesores";
            Load += FormProfesores_Load;
            gridPanel.ResumeLayout(false);
            buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewProfesores).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel gridPanel;
        private Panel buttonPanel;
        private DataGridView dataGridViewProfesores;
        private Button btnVolver;
        private Button btnEliminar;
        private Button btnEditar;
        private Button btnNuevo;
    }
}
