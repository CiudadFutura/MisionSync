namespace MisionSync
{
    partial class Frm_main
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_main));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsm_file = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_salir = new System.Windows.Forms.ToolStripMenuItem();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tp_Facturas = new System.Windows.Forms.TabPage();
            this.btn_renameUpload = new System.Windows.Forms.Button();
            this.btnRenombrar = new System.Windows.Forms.Button();
            this.btnSubir = new System.Windows.Forms.Button();
            this.txt_cantFact = new System.Windows.Forms.TextBox();
            this.lbl_CantFact = new System.Windows.Forms.Label();
            this.dtp_ultimaFecha = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.tp_sync = new System.Windows.Forms.TabPage();
            this.btn_Productos = new System.Windows.Forms.Button();
            this.btn_TodoMiniCiclo = new System.Windows.Forms.Button();
            this.btn_TodoCiclo = new System.Windows.Forms.Button();
            this.btn_PedidosMiniCiclo = new System.Windows.Forms.Button();
            this.btn_PedidosCiclo = new System.Windows.Forms.Button();
            this.btn_consumidores = new System.Windows.Forms.Button();
            this.btn_Productores = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txt_log = new System.Windows.Forms.RichTextBox();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tp_Facturas.SuspendLayout();
            this.tp_sync.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 245);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(728, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_file});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(728, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsm_file
            // 
            this.tsm_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_salir});
            this.tsm_file.Name = "tsm_file";
            this.tsm_file.Size = new System.Drawing.Size(60, 20);
            this.tsm_file.Text = "Archivo";
            // 
            // tsmi_salir
            // 
            this.tsmi_salir.Name = "tsmi_salir";
            this.tsmi_salir.Size = new System.Drawing.Size(96, 22);
            this.tsmi_salir.Text = "Salir";
            this.tsmi_salir.Click += new System.EventHandler(this.Tsmi_salir_Click);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tp_Facturas);
            this.tabMain.Controls.Add(this.tp_sync);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(320, 221);
            this.tabMain.TabIndex = 2;
            // 
            // tp_Facturas
            // 
            this.tp_Facturas.Controls.Add(this.btn_renameUpload);
            this.tp_Facturas.Controls.Add(this.btnRenombrar);
            this.tp_Facturas.Controls.Add(this.btnSubir);
            this.tp_Facturas.Controls.Add(this.txt_cantFact);
            this.tp_Facturas.Controls.Add(this.lbl_CantFact);
            this.tp_Facturas.Controls.Add(this.dtp_ultimaFecha);
            this.tp_Facturas.Controls.Add(this.label1);
            this.tp_Facturas.Location = new System.Drawing.Point(4, 22);
            this.tp_Facturas.Name = "tp_Facturas";
            this.tp_Facturas.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Facturas.Size = new System.Drawing.Size(312, 195);
            this.tp_Facturas.TabIndex = 0;
            this.tp_Facturas.Text = "Facturas";
            this.tp_Facturas.UseVisualStyleBackColor = true;
            // 
            // btn_renameUpload
            // 
            this.btn_renameUpload.Location = new System.Drawing.Point(87, 105);
            this.btn_renameUpload.Name = "btn_renameUpload";
            this.btn_renameUpload.Size = new System.Drawing.Size(206, 23);
            this.btn_renameUpload.TabIndex = 7;
            this.btn_renameUpload.Text = "Renombrar y Subir";
            this.btn_renameUpload.UseVisualStyleBackColor = true;
            this.btn_renameUpload.Click += new System.EventHandler(this.Btn_renameUpload_Click);
            // 
            // btnRenombrar
            // 
            this.btnRenombrar.Location = new System.Drawing.Point(87, 76);
            this.btnRenombrar.Name = "btnRenombrar";
            this.btnRenombrar.Size = new System.Drawing.Size(100, 23);
            this.btnRenombrar.TabIndex = 6;
            this.btnRenombrar.Text = "Renombrar";
            this.btnRenombrar.UseVisualStyleBackColor = true;
            this.btnRenombrar.Click += new System.EventHandler(this.BtnRenombrar_Click);
            // 
            // btnSubir
            // 
            this.btnSubir.Location = new System.Drawing.Point(193, 76);
            this.btnSubir.Name = "btnSubir";
            this.btnSubir.Size = new System.Drawing.Size(100, 23);
            this.btnSubir.TabIndex = 5;
            this.btnSubir.Text = "Subir";
            this.btnSubir.UseVisualStyleBackColor = true;
            this.btnSubir.Click += new System.EventHandler(this.BtnSubir_Click);
            // 
            // txt_cantFact
            // 
            this.txt_cantFact.Location = new System.Drawing.Point(193, 50);
            this.txt_cantFact.Name = "txt_cantFact";
            this.txt_cantFact.ReadOnly = true;
            this.txt_cantFact.Size = new System.Drawing.Size(100, 20);
            this.txt_cantFact.TabIndex = 4;
            // 
            // lbl_CantFact
            // 
            this.lbl_CantFact.AutoSize = true;
            this.lbl_CantFact.Location = new System.Drawing.Point(8, 50);
            this.lbl_CantFact.Name = "lbl_CantFact";
            this.lbl_CantFact.Size = new System.Drawing.Size(173, 13);
            this.lbl_CantFact.TabIndex = 3;
            this.lbl_CantFact.Text = "Cantidad de facturas por procesar: ";
            // 
            // dtp_ultimaFecha
            // 
            this.dtp_ultimaFecha.Location = new System.Drawing.Point(80, 16);
            this.dtp_ultimaFecha.Name = "dtp_ultimaFecha";
            this.dtp_ultimaFecha.Size = new System.Drawing.Size(213, 20);
            this.dtp_ultimaFecha.TabIndex = 1;
            this.dtp_ultimaFecha.ValueChanged += new System.EventHandler(this.Dtp_ultimaFecha_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ultima fecha:";
            // 
            // tp_sync
            // 
            this.tp_sync.Controls.Add(this.btn_Productos);
            this.tp_sync.Controls.Add(this.btn_TodoMiniCiclo);
            this.tp_sync.Controls.Add(this.btn_TodoCiclo);
            this.tp_sync.Controls.Add(this.btn_PedidosMiniCiclo);
            this.tp_sync.Controls.Add(this.btn_PedidosCiclo);
            this.tp_sync.Controls.Add(this.btn_consumidores);
            this.tp_sync.Controls.Add(this.btn_Productores);
            this.tp_sync.Location = new System.Drawing.Point(4, 22);
            this.tp_sync.Name = "tp_sync";
            this.tp_sync.Padding = new System.Windows.Forms.Padding(3);
            this.tp_sync.Size = new System.Drawing.Size(312, 195);
            this.tp_sync.TabIndex = 1;
            this.tp_sync.Text = "Sincronizar";
            this.tp_sync.UseVisualStyleBackColor = true;
            // 
            // btn_Productos
            // 
            this.btn_Productos.Location = new System.Drawing.Point(8, 6);
            this.btn_Productos.Name = "btn_Productos";
            this.btn_Productos.Size = new System.Drawing.Size(209, 23);
            this.btn_Productos.TabIndex = 7;
            this.btn_Productos.Text = "Productos";
            this.btn_Productos.UseVisualStyleBackColor = true;
            this.btn_Productos.Click += new System.EventHandler(this.Btn_Productos_Click);
            // 
            // btn_TodoMiniCiclo
            // 
            this.btn_TodoMiniCiclo.Location = new System.Drawing.Point(8, 151);
            this.btn_TodoMiniCiclo.Name = "btn_TodoMiniCiclo";
            this.btn_TodoMiniCiclo.Size = new System.Drawing.Size(209, 23);
            this.btn_TodoMiniCiclo.TabIndex = 6;
            this.btn_TodoMiniCiclo.Text = "Todo para Mini Ciclo";
            this.btn_TodoMiniCiclo.UseVisualStyleBackColor = true;
            this.btn_TodoMiniCiclo.Click += new System.EventHandler(this.Btn_TodoMiniCiclo_Click);
            // 
            // btn_TodoCiclo
            // 
            this.btn_TodoCiclo.Location = new System.Drawing.Point(8, 93);
            this.btn_TodoCiclo.Name = "btn_TodoCiclo";
            this.btn_TodoCiclo.Size = new System.Drawing.Size(209, 23);
            this.btn_TodoCiclo.TabIndex = 5;
            this.btn_TodoCiclo.Text = "Todo para Ciclo Ordinario";
            this.btn_TodoCiclo.UseVisualStyleBackColor = true;
            this.btn_TodoCiclo.Click += new System.EventHandler(this.Btn_TodoCiclo_Click);
            // 
            // btn_PedidosMiniCiclo
            // 
            this.btn_PedidosMiniCiclo.Location = new System.Drawing.Point(8, 122);
            this.btn_PedidosMiniCiclo.Name = "btn_PedidosMiniCiclo";
            this.btn_PedidosMiniCiclo.Size = new System.Drawing.Size(209, 23);
            this.btn_PedidosMiniCiclo.TabIndex = 4;
            this.btn_PedidosMiniCiclo.Text = "Pedidos para Mini Ciclo";
            this.btn_PedidosMiniCiclo.UseVisualStyleBackColor = true;
            this.btn_PedidosMiniCiclo.Click += new System.EventHandler(this.Btn_PedidosMiniCiclo_Click);
            // 
            // btn_PedidosCiclo
            // 
            this.btn_PedidosCiclo.Location = new System.Drawing.Point(8, 64);
            this.btn_PedidosCiclo.Name = "btn_PedidosCiclo";
            this.btn_PedidosCiclo.Size = new System.Drawing.Size(209, 23);
            this.btn_PedidosCiclo.TabIndex = 3;
            this.btn_PedidosCiclo.Text = "Pedidos para Ciclo Ordinario";
            this.btn_PedidosCiclo.UseVisualStyleBackColor = true;
            this.btn_PedidosCiclo.Click += new System.EventHandler(this.Btn_PedidosCiclo_Click);
            // 
            // btn_consumidores
            // 
            this.btn_consumidores.Location = new System.Drawing.Point(8, 35);
            this.btn_consumidores.Name = "btn_consumidores";
            this.btn_consumidores.Size = new System.Drawing.Size(103, 23);
            this.btn_consumidores.TabIndex = 2;
            this.btn_consumidores.Text = "Consumidores";
            this.btn_consumidores.UseVisualStyleBackColor = true;
            this.btn_consumidores.Click += new System.EventHandler(this.Btn_consumidores_Click);
            // 
            // btn_Productores
            // 
            this.btn_Productores.Location = new System.Drawing.Point(114, 35);
            this.btn_Productores.Name = "btn_Productores";
            this.btn_Productores.Size = new System.Drawing.Size(103, 23);
            this.btn_Productores.TabIndex = 1;
            this.btn_Productores.Text = "Productores";
            this.btn_Productores.UseVisualStyleBackColor = true;
            this.btn_Productores.Click += new System.EventHandler(this.Btn_Productores_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabMain);
            this.splitContainer1.Panel1MinSize = 320;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txt_log);
            this.splitContainer1.Panel2.Controls.Add(this.menuStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(728, 221);
            this.splitContainer1.SplitterDistance = 320;
            this.splitContainer1.TabIndex = 3;
            // 
            // txt_log
            // 
            this.txt_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_log.Location = new System.Drawing.Point(0, 24);
            this.txt_log.Name = "txt_log";
            this.txt_log.Size = new System.Drawing.Size(404, 197);
            this.txt_log.TabIndex = 0;
            this.txt_log.Text = "";
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(404, 24);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.guardarToolStripMenuItem});
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.logToolStripMenuItem.Text = "Log";
            // 
            // guardarToolStripMenuItem
            // 
            this.guardarToolStripMenuItem.Name = "guardarToolStripMenuItem";
            this.guardarToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.guardarToolStripMenuItem.Text = "Guardar";
            this.guardarToolStripMenuItem.Click += new System.EventHandler(this.GuardarToolStripMenuItem_Click);
            // 
            // Frm_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 267);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(744, 306);
            this.Name = "Frm_main";
            this.Text = "Mision Sync";
            this.Load += new System.EventHandler(this.Frm_main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tp_Facturas.ResumeLayout(false);
            this.tp_Facturas.PerformLayout();
            this.tp_sync.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsm_file;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tp_Facturas;
        private System.Windows.Forms.TabPage tp_sync;
        private System.Windows.Forms.ToolStripMenuItem tsmi_salir;
        private System.Windows.Forms.DateTimePicker dtp_ultimaFecha;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_cantFact;
        private System.Windows.Forms.Label lbl_CantFact;
        private System.Windows.Forms.Button btnSubir;
        private System.Windows.Forms.Button btn_consumidores;
        private System.Windows.Forms.Button btn_Productores;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox txt_log;
        private System.Windows.Forms.Button btn_Productos;
        private System.Windows.Forms.Button btn_TodoMiniCiclo;
        private System.Windows.Forms.Button btn_TodoCiclo;
        private System.Windows.Forms.Button btn_PedidosMiniCiclo;
        private System.Windows.Forms.Button btn_PedidosCiclo;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarToolStripMenuItem;
        private System.Windows.Forms.Button btn_renameUpload;
        private System.Windows.Forms.Button btnRenombrar;
    }
}

