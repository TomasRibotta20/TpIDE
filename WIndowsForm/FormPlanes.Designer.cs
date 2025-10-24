namespace WIndowsForm
{
    partial class FormPlanes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            gridPanel = new Panel();
            buttonPanel = new Panel();
            btnVerEspecialidad = new Button();
            btnVolver = new Button();
            btnEliminar = new Button();
            btnEditar = new Button();
            btnNuevo = new Button();
            dataGridViewPlanes = new DataGridView();
            gridPanel.SuspendLayout();
            buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlanes).BeginInit();
            SuspendLayout();
            // 
            // gridPanel
            // 
            gridPanel.Controls.Add(buttonPanel);
            gridPanel.Controls.Add(dataGridViewPlanes);
            gridPanel.Dock = DockStyle.Fill;
            gridPanel.Location = new Point(0, 0);
            gridPanel.Name = "gridPanel";
            gridPanel.Padding = new Padding(10);
            gridPanel.Size = new Size(784, 561);
            gridPanel.TabIndex = 0;
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(btnVerEspecialidad);
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
            // btnVerEspecialidad
            // 
            btnVerEspecialidad.Location = new Point(380, 15);
            btnVerEspecialidad.Name = "btnVerEspecialidad";
            btnVerEspecialidad.Size = new Size(120, 30);
            btnVerEspecialidad.TabIndex = 4;
            btnVerEspecialidad.Text = "Ver Especialidad";
            btnVerEspecialidad.UseVisualStyleBackColor = true;
            btnVerEspecialidad.Enabled = false;
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
            // btnEliminar
            // 
            btnEliminar.Location = new Point(270, 15);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(100, 30);
            btnEliminar.TabIndex = 2;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(160, 15);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(100, 30);
            btnEditar.TabIndex = 1;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            // 
            // btnNuevo
            // 
            btnNuevo.Location = new Point(10, 15);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(140, 30);
            btnNuevo.TabIndex = 0;
            btnNuevo.Text = "Nuevo Plan";
            btnNuevo.UseVisualStyleBackColor = true;
            // 
            // dataGridViewPlanes
            // 
            dataGridViewPlanes.AllowUserToAddRows = false;
            dataGridViewPlanes.AllowUserToDeleteRows = false;
            dataGridViewPlanes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewPlanes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPlanes.Dock = DockStyle.Fill;
            dataGridViewPlanes.Location = new Point(10, 10);
            dataGridViewPlanes.MultiSelect = false;
            dataGridViewPlanes.Name = "dataGridViewPlanes";
            dataGridViewPlanes.ReadOnly = true;
            dataGridViewPlanes.RowHeadersVisible = false;
            dataGridViewPlanes.RowTemplate.Height = 25;
            dataGridViewPlanes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPlanes.Size = new Size(764, 481);
            dataGridViewPlanes.TabIndex = 0;
            dataGridViewPlanes.SelectionChanged += DataGridViewPlanes_SelectionChanged;
            // 
            // FormPlanes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(gridPanel);
            Name = "FormPlanes";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestion de Planes";
            gridPanel.ResumeLayout(false);
            buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlanes).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel gridPanel;
        private DataGridView dataGridViewPlanes;
        private Panel buttonPanel;
        private Button btnVolver;
        private Button btnEliminar;
        private Button btnEditar;
        private Button btnNuevo;
        private Button btnVerEspecialidad;
    }
}