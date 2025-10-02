namespace News2025
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectCGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewslineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DailyBizToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CultureSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnKetNoiVoi = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnXoaHet = new System.Windows.Forms.Button();
            this.tbnXoaManHinh = new System.Windows.Forms.Button();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelMainContent = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNewsline = new System.Windows.Forms.Button();
            this.btnDailyBiz = new System.Windows.Forms.Button();
            this.btnCultureScene = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.panelMainContent.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.connectCGToolStripMenuItem,
            this.layoutToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1384, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.closeProjectToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Enabled = false;
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Enabled = false;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // closeProjectToolStripMenuItem
            // 
            this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            this.closeProjectToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.closeProjectToolStripMenuItem.Text = "Close All Projects";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // connectCGToolStripMenuItem
            // 
            this.connectCGToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.connectCGToolStripMenuItem.Name = "connectCGToolStripMenuItem";
            this.connectCGToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.connectCGToolStripMenuItem.Text = "Connect CG";
            this.connectCGToolStripMenuItem.Click += new System.EventHandler(this.connectCGToolStripMenuItem_Click);
            // 
            // layoutToolStripMenuItem
            // 
            this.layoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewslineToolStripMenuItem,
            this.DailyBizToolStripMenuItem,
            this.CultureSceneToolStripMenuItem});
            this.layoutToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.layoutToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.layoutToolStripMenuItem.Name = "layoutToolStripMenuItem";
            this.layoutToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.layoutToolStripMenuItem.Text = "Layout";
            this.layoutToolStripMenuItem.Click += new System.EventHandler(this.layoutToolStripMenuItem_Click);
            // 
            // NewslineToolStripMenuItem
            // 
            this.NewslineToolStripMenuItem.Enabled = false;
            this.NewslineToolStripMenuItem.Name = "NewslineToolStripMenuItem";
            this.NewslineToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.NewslineToolStripMenuItem.Text = "NEWSLINE";
            this.NewslineToolStripMenuItem.Click += new System.EventHandler(this.NewslineToolStripMenuItem_Click);
            // 
            // DailyBizToolStripMenuItem
            // 
            this.DailyBizToolStripMenuItem.Enabled = false;
            this.DailyBizToolStripMenuItem.Name = "DailyBizToolStripMenuItem";
            this.DailyBizToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.DailyBizToolStripMenuItem.Text = "DAILY BIZ";
            this.DailyBizToolStripMenuItem.Click += new System.EventHandler(this.DailyBizToolStripMenuItem_Click);
            // 
            // CultureSceneToolStripMenuItem
            // 
            this.CultureSceneToolStripMenuItem.Enabled = false;
            this.CultureSceneToolStripMenuItem.Name = "CultureSceneToolStripMenuItem";
            this.CultureSceneToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.CultureSceneToolStripMenuItem.Text = "CULTURE SCENE";
            this.CultureSceneToolStripMenuItem.Click += new System.EventHandler(this.CultureSceneToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Enabled = false;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.optionsToolStripMenuItem.Text = "Option";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // btnKetNoiVoi
            // 
            this.btnKetNoiVoi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnKetNoiVoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnKetNoiVoi.Location = new System.Drawing.Point(313, 1881);
            this.btnKetNoiVoi.Margin = new System.Windows.Forms.Padding(2);
            this.btnKetNoiVoi.Name = "btnKetNoiVoi";
            this.btnKetNoiVoi.Size = new System.Drawing.Size(100, 25);
            this.btnKetNoiVoi.TabIndex = 16;
            this.btnKetNoiVoi.Text = "Kết nối với HDVG";
            this.btnKetNoiVoi.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnReset.Location = new System.Drawing.Point(429, 1881);
            this.btnReset.Margin = new System.Windows.Forms.Padding(2);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 25);
            this.btnReset.TabIndex = 17;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // btnXoaHet
            // 
            this.btnXoaHet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXoaHet.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnXoaHet.Location = new System.Drawing.Point(1264, 1881);
            this.btnXoaHet.Margin = new System.Windows.Forms.Padding(2);
            this.btnXoaHet.Name = "btnXoaHet";
            this.btnXoaHet.Size = new System.Drawing.Size(100, 25);
            this.btnXoaHet.TabIndex = 23;
            this.btnXoaHet.Text = "Xóa đồ họa";
            this.btnXoaHet.UseVisualStyleBackColor = true;
            // 
            // tbnXoaManHinh
            // 
            this.tbnXoaManHinh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbnXoaManHinh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tbnXoaManHinh.Location = new System.Drawing.Point(1147, 1881);
            this.tbnXoaManHinh.Margin = new System.Windows.Forms.Padding(2);
            this.tbnXoaManHinh.Name = "tbnXoaManHinh";
            this.tbnXoaManHinh.Size = new System.Drawing.Size(100, 25);
            this.tbnXoaManHinh.TabIndex = 22;
            this.tbnXoaManHinh.Text = "Xóa màn hình";
            this.tbnXoaManHinh.UseVisualStyleBackColor = true;
            // 
            // statusStripMain
            // 
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStripMain.Location = new System.Drawing.Point(0, 939);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(1384, 22);
            this.statusStripMain.TabIndex = 25;
            this.statusStripMain.Text = "statusStripMain";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel1.Text = "Status";
            // 
            // panelMainContent
            // 
            this.panelMainContent.BackColor = System.Drawing.Color.Transparent;
            this.panelMainContent.Controls.Add(this.flowLayoutPanel1);
            this.panelMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainContent.Location = new System.Drawing.Point(0, 24);
            this.panelMainContent.Name = "panelMainContent";
            this.panelMainContent.Size = new System.Drawing.Size(1384, 915);
            this.panelMainContent.TabIndex = 38;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnNewsline);
            this.flowLayoutPanel1.Controls.Add(this.btnDailyBiz);
            this.flowLayoutPanel1.Controls.Add(this.btnCultureScene);
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(240, 66);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(897, 165);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnNewsline
            // 
            this.btnNewsline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(8)))), ((int)(((byte)(18)))));
            this.btnNewsline.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNewsline.Font = new System.Drawing.Font("SVN-Avenir Next", 30F, System.Drawing.FontStyle.Bold);
            this.btnNewsline.ForeColor = System.Drawing.Color.White;
            this.btnNewsline.Location = new System.Drawing.Point(20, 20);
            this.btnNewsline.Margin = new System.Windows.Forms.Padding(20);
            this.btnNewsline.Name = "btnNewsline";
            this.btnNewsline.Size = new System.Drawing.Size(258, 123);
            this.btnNewsline.TabIndex = 0;
            this.btnNewsline.Text = "NEWSLINE";
            this.btnNewsline.UseVisualStyleBackColor = false;
            this.btnNewsline.Click += new System.EventHandler(this.btnNewsline_Click);
            // 
            // btnDailyBiz
            // 
            this.btnDailyBiz.BackgroundImage = global::News2025.Properties.Resources.daily_biz_logo2;
            this.btnDailyBiz.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDailyBiz.Font = new System.Drawing.Font("SVN-Avenir Next", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDailyBiz.Location = new System.Drawing.Point(318, 20);
            this.btnDailyBiz.Margin = new System.Windows.Forms.Padding(20);
            this.btnDailyBiz.Name = "btnDailyBiz";
            this.btnDailyBiz.Size = new System.Drawing.Size(258, 123);
            this.btnDailyBiz.TabIndex = 1;
            this.btnDailyBiz.UseVisualStyleBackColor = true;
            this.btnDailyBiz.Click += new System.EventHandler(this.btnDailyBiz_Click);
            // 
            // btnCultureScene
            // 
            this.btnCultureScene.BackgroundImage = global::News2025.Properties.Resources.Logo_Culture_Scene;
            this.btnCultureScene.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCultureScene.Font = new System.Drawing.Font("SVN-Avenir Next", 20.25F, System.Drawing.FontStyle.Bold);
            this.btnCultureScene.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCultureScene.Location = new System.Drawing.Point(616, 20);
            this.btnCultureScene.Margin = new System.Windows.Forms.Padding(20);
            this.btnCultureScene.Name = "btnCultureScene";
            this.btnCultureScene.Size = new System.Drawing.Size(258, 123);
            this.btnCultureScene.TabIndex = 2;
            this.btnCultureScene.UseVisualStyleBackColor = true;
            this.btnCultureScene.Click += new System.EventHandler(this.btnCultureScene_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1384, 961);
            this.Controls.Add(this.panelMainContent);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.btnXoaHet);
            this.Controls.Add(this.tbnXoaManHinh);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnKetNoiVoi);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CG VIETNAM TODAY";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.panelMainContent.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button btnKetNoiVoi;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnXoaHet;
        private System.Windows.Forms.Button tbnXoaManHinh;
        private System.Windows.Forms.ToolStripMenuItem connectCGToolStripMenuItem;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripMenuItem NewslineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DailyBizToolStripMenuItem;
        public System.Windows.Forms.Panel panelMainContent;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnNewsline;
        private System.Windows.Forms.Button btnDailyBiz;
        private System.Windows.Forms.Button btnCultureScene;
        private System.Windows.Forms.ToolStripMenuItem CultureSceneToolStripMenuItem;
    }
}

