namespace API_iNews
{
    partial class FormInews
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInews));
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.topPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnExportXML = new System.Windows.Forms.Button();
            this.grpQueue = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainLayout.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.grpQueue.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.topPanel, 0, 0);
            this.mainLayout.Controls.Add(this.grpQueue, 0, 1);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 2;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(800, 428);
            this.mainLayout.TabIndex = 0;
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.btnConnect);
            this.topPanel.Controls.Add(this.btnExportXML);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topPanel.Location = new System.Drawing.Point(3, 3);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(5);
            this.topPanel.Size = new System.Drawing.Size(794, 44);
            this.topPanel.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(8, 8);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(120, 32);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect Server";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // btnExportXML
            // 
            this.btnExportXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportXML.Location = new System.Drawing.Point(134, 8);
            this.btnExportXML.Name = "btnExportXML";
            this.btnExportXML.Size = new System.Drawing.Size(120, 32);
            this.btnExportXML.TabIndex = 1;
            this.btnExportXML.Text = "Export XML";
            this.btnExportXML.UseVisualStyleBackColor = true;
            // 
            // grpQueue
            // 
            this.grpQueue.Controls.Add(this.treeView1);
            this.grpQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpQueue.Location = new System.Drawing.Point(3, 53);
            this.grpQueue.Name = "grpQueue";
            this.grpQueue.Size = new System.Drawing.Size(794, 372);
            this.grpQueue.TabIndex = 1;
            this.grpQueue.TabStop = false;
            this.grpQueue.Text = "Danh sách Queue";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 18);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(788, 351);
            this.treeView1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbStatus
            // 
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(60, 17);
            this.lbStatus.Text = "Trạng thái";
            // 
            // FormInews
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormInews";
            this.Text = "iNews Reader";
            this.Load += new System.EventHandler(this.FormInews_Load);
            this.mainLayout.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.grpQueue.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.FlowLayoutPanel topPanel;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnExportXML;
        private System.Windows.Forms.GroupBox grpQueue;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel lbStatus;
    }
}