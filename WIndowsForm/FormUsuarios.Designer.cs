namespace WIndowsForm
{
    partial class FormUsuarios
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
            dataGridViewUsuarios = new DataGridView();
            buttonPanel = new Panel();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            btnVolver = new Button();
            gridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).BeginInit();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // gridPanel
            // 
            gridPanel.Controls.Add(buttonPanel);
            gridPanel.Controls.Add(dataGridViewUsuarios);
            gridPanel.Dock = DockStyle.Fill;
            gridPanel.Location = new Point(0, 0);
            gridPanel.Name = "gridPanel";
            gridPanel.Padding = new Padding(10);
            gridPanel.Size = new Size(784, 561);
            gridPanel.TabIndex = 0;
            // 
            // dataGridViewUsuarios
            // 
            dataGridViewUsuarios.AllowUserToAddRows = false;
            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsuarios.Dock = DockStyle.Fill;
            dataGridViewUsuarios.Location = new Point(10, 10);
            dataGridViewUsuarios.MultiSelect = false;
            dataGridViewUsuarios.Name = "dataGridViewUsuarios";
            dataGridViewUsuarios.ReadOnly = true;
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.Size = new Size(764, 541);
            dataGridViewUsuarios.TabIndex = 0;
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
            btnNuevo.Text = "Nuevo Usuario";
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
            // FormUsuarios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(gridPanel);
            Name = "FormUsuarios";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestion de Usuarios";
            Load += this.FormUsuarios_Load;
            gridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).EndInit();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel gridPanel;
        private Panel buttonPanel;
        private DataGridView dataGridViewUsuarios;
        private Button btnVolver;
        private Button btnEliminar;
        private Button btnEditar;
        private Button btnNuevo;
    }
}