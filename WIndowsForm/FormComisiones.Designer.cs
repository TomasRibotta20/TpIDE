namespace WIndowsForm
{
    partial class FormComisiones
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
            btnVerPlan = new Button();
            btnVolver = new Button();
            btnEliminar = new Button();
            btnEditar = new Button();
            btnNueva = new Button();
            dataGridViewComisiones = new DataGridView();
            gridPanel.SuspendLayout();
            buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewComisiones).BeginInit();
            SuspendLayout();
            // 
            // gridPanel
            // 
            gridPanel.Controls.Add(buttonPanel);
            gridPanel.Controls.Add(dataGridViewComisiones);
            gridPanel.Dock = DockStyle.Fill;
            gridPanel.Location = new Point(0, 0);
            gridPanel.Name = "gridPanel";
            gridPanel.Padding = new Padding(10);
            gridPanel.Size = new Size(784, 561);
            gridPanel.TabIndex = 0;
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(btnVerPlan);
            buttonPanel.Controls.Add(btnVolver);
            buttonPanel.Controls.Add(btnEliminar);
            buttonPanel.Controls.Add(btnEditar);
            buttonPanel.Controls.Add(btnNueva);
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Location = new Point(10, 491);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(764, 60);
            buttonPanel.TabIndex = 1;
            // 
            // btnVerPlan
            // 
            btnVerPlan.Location = new Point(380, 15);
            btnVerPlan.Name = "btnVerPlan";
            btnVerPlan.Size = new Size(120, 30);
            btnVerPlan.TabIndex = 4;
            btnVerPlan.Text = "Ver Plan";
            btnVerPlan.UseVisualStyleBackColor = true;
            btnVerPlan.Enabled = false;
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
            // btnNueva
            // 
            btnNueva.Location = new Point(10, 15);
            btnNueva.Name = "btnNueva";
            btnNueva.Size = new Size(140, 30);
            btnNueva.TabIndex = 0;
            btnNueva.Text = "Nueva Comisión";
            btnNueva.UseVisualStyleBackColor = true;
            // 
            // dataGridViewComisiones
            // 
            dataGridViewComisiones.AllowUserToAddRows = false;
            dataGridViewComisiones.AllowUserToDeleteRows = false;
            dataGridViewComisiones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewComisiones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewComisiones.Dock = DockStyle.Fill;
            dataGridViewComisiones.Location = new Point(10, 10);
            dataGridViewComisiones.MultiSelect = false;
            dataGridViewComisiones.Name = "dataGridViewComisiones";
            dataGridViewComisiones.ReadOnly = true;
            dataGridViewComisiones.RowHeadersVisible = false;
            dataGridViewComisiones.RowTemplate.Height = 25;
            dataGridViewComisiones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewComisiones.Size = new Size(764, 481);
            dataGridViewComisiones.TabIndex = 0;
            dataGridViewComisiones.SelectionChanged += DataGridViewComisiones_SelectionChanged;
            // 
            // FormComisiones
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(gridPanel);
            Name = "FormComisiones";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestion de Comisiones";
            gridPanel.ResumeLayout(false);
            buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewComisiones).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel gridPanel;
        private DataGridView dataGridViewComisiones;
        private Panel buttonPanel;
        private Button btnVolver;
        private Button btnEliminar;
        private Button btnEditar;
        private Button btnNueva;
        private Button btnVerPlan;
    }
}