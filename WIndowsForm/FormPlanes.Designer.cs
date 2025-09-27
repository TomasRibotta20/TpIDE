namespace WIndowsForm
{
    partial class FormPlanes
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.DataGridView dataGridViewPlanes;
        private System.Windows.Forms.FlowLayoutPanel panelBotones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnVolver;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            dataGridViewPlanes = new System.Windows.Forms.DataGridView();
            panelBotones = new System.Windows.Forms.FlowLayoutPanel();
            btnNuevo = new System.Windows.Forms.Button();
            btnEditar = new System.Windows.Forms.Button();
            btnEliminar = new System.Windows.Forms.Button();
            btnVolver = new System.Windows.Forms.Button();
            tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(dataGridViewPlanes)).BeginInit();
            panelBotones.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 1;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.Controls.Add(dataGridViewPlanes, 0, 0);
            tableLayoutPanelMain.Controls.Add(panelBotones, 0, 1);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelMain.RowCount = 2;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.Size = new System.Drawing.Size(640, 400);
            // 
            // dataGridViewPlanes
            // 
            dataGridViewPlanes.AllowUserToAddRows = false;
            dataGridViewPlanes.AllowUserToDeleteRows = false;
            dataGridViewPlanes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewPlanes.MultiSelect = false;
            dataGridViewPlanes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPlanes.ReadOnly = true;
            dataGridViewPlanes.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridViewPlanes.Name = "dataGridViewPlanes";
            // 
            // panelBotones
            // 
            panelBotones.Controls.Add(btnNuevo);
            panelBotones.Controls.Add(btnEditar);
            panelBotones.Controls.Add(btnEliminar);
            panelBotones.Controls.Add(btnVolver);
            panelBotones.Dock = System.Windows.Forms.DockStyle.Fill;
            panelBotones.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            panelBotones.Name = "panelBotones";
            // 
            // btnNuevo
            // 
            btnNuevo.Text = "Nuevo";
            // 
            // btnEditar
            // 
            btnEditar.Text = "Editar";
            // 
            // btnEliminar
            // 
            btnEliminar.Text = "Eliminar";
            // 
            // btnVolver
            // 
            btnVolver.Text = "Volver";
            // 
            // FormPlanes
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(640, 400);
            Controls.Add(tableLayoutPanelMain);
            Name = "FormPlanes";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Planes";
            tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(dataGridViewPlanes)).EndInit();
            panelBotones.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}