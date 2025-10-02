namespace News2025.Menu
{
    partial class frmConnectCG3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnectCG3));
            this.txtWorkingFolder = new System.Windows.Forms.TextBox();
            this.label88 = new System.Windows.Forms.Label();
            this.label87 = new System.Windows.Forms.Label();
            this.label85 = new System.Windows.Forms.Label();
            this.txtIpMain = new System.Windows.Forms.TextBox();
            this.txtPortMain = new System.Windows.Forms.TextBox();
            this.btnConnectKarisma = new System.Windows.Forms.Button();
            this.btnDisconnectKarisma = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnOpenConfig = new System.Windows.Forms.Button();
            this.groupSetting = new System.Windows.Forms.GroupBox();
            this.checkBackup = new System.Windows.Forms.CheckBox();
            this.checkMain = new System.Windows.Forms.CheckBox();
            this.txtPortBackup = new System.Windows.Forms.TextBox();
            this.txtIpBackup = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.logBoxKarisma = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUpdateLog = new System.Windows.Forms.Button();
            this.btnXoaLog = new System.Windows.Forms.Button();
            this.groupSetting.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtWorkingFolder
            // 
            this.txtWorkingFolder.Location = new System.Drawing.Point(126, 119);
            this.txtWorkingFolder.Name = "txtWorkingFolder";
            this.txtWorkingFolder.Size = new System.Drawing.Size(443, 25);
            this.txtWorkingFolder.TabIndex = 7;
            this.txtWorkingFolder.Visible = false;
            // 
            // label88
            // 
            this.label88.AutoSize = true;
            this.label88.Location = new System.Drawing.Point(15, 123);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(105, 19);
            this.label88.TabIndex = 6;
            this.label88.Text = "Working Folder:";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label88.Visible = false;
            // 
            // label87
            // 
            this.label87.AutoSize = true;
            this.label87.Location = new System.Drawing.Point(61, 37);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(59, 19);
            this.label87.TabIndex = 2;
            this.label87.Text = "IP Main:";
            this.label87.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label85
            // 
            this.label85.AutoSize = true;
            this.label85.Location = new System.Drawing.Point(421, 37);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(72, 19);
            this.label85.TabIndex = 3;
            this.label85.Text = "Port Main:";
            this.label85.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtIpMain
            // 
            this.txtIpMain.Location = new System.Drawing.Point(126, 34);
            this.txtIpMain.Name = "txtIpMain";
            this.txtIpMain.Size = new System.Drawing.Size(276, 25);
            this.txtIpMain.TabIndex = 4;
            // 
            // txtPortMain
            // 
            this.txtPortMain.Location = new System.Drawing.Point(499, 34);
            this.txtPortMain.Name = "txtPortMain";
            this.txtPortMain.Size = new System.Drawing.Size(70, 25);
            this.txtPortMain.TabIndex = 4;
            // 
            // btnConnectKarisma
            // 
            this.btnConnectKarisma.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(182)))), ((int)(((byte)(213)))));
            this.btnConnectKarisma.FlatAppearance.BorderSize = 0;
            this.btnConnectKarisma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnectKarisma.ForeColor = System.Drawing.Color.White;
            this.btnConnectKarisma.Location = new System.Drawing.Point(621, 34);
            this.btnConnectKarisma.Name = "btnConnectKarisma";
            this.btnConnectKarisma.Size = new System.Drawing.Size(100, 30);
            this.btnConnectKarisma.TabIndex = 5;
            this.btnConnectKarisma.Text = "Connect";
            this.btnConnectKarisma.UseVisualStyleBackColor = false;
            this.btnConnectKarisma.Click += new System.EventHandler(this.btnConnectKarisma_Click);
            // 
            // btnDisconnectKarisma
            // 
            this.btnDisconnectKarisma.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(182)))), ((int)(((byte)(213)))));
            this.btnDisconnectKarisma.FlatAppearance.BorderSize = 0;
            this.btnDisconnectKarisma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisconnectKarisma.ForeColor = System.Drawing.Color.White;
            this.btnDisconnectKarisma.Location = new System.Drawing.Point(621, 70);
            this.btnDisconnectKarisma.Name = "btnDisconnectKarisma";
            this.btnDisconnectKarisma.Size = new System.Drawing.Size(100, 30);
            this.btnDisconnectKarisma.TabIndex = 5;
            this.btnDisconnectKarisma.Text = "Disconnect";
            this.btnDisconnectKarisma.UseVisualStyleBackColor = false;
            this.btnDisconnectKarisma.Click += new System.EventHandler(this.btnDisconnectKarisma_Click);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(182)))), ((int)(((byte)(213)))));
            this.btnSaveConfig.FlatAppearance.BorderSize = 0;
            this.btnSaveConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveConfig.ForeColor = System.Drawing.Color.White;
            this.btnSaveConfig.Location = new System.Drawing.Point(368, 132);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(100, 30);
            this.btnSaveConfig.TabIndex = 189;
            this.btnSaveConfig.Text = "Load Config";
            this.btnSaveConfig.UseVisualStyleBackColor = false;
            this.btnSaveConfig.Click += new System.EventHandler(this.LoadConfig_Click);
            // 
            // btnOpenConfig
            // 
            this.btnOpenConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(182)))), ((int)(((byte)(213)))));
            this.btnOpenConfig.FlatAppearance.BorderSize = 0;
            this.btnOpenConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenConfig.ForeColor = System.Drawing.Color.White;
            this.btnOpenConfig.Location = new System.Drawing.Point(262, 132);
            this.btnOpenConfig.Name = "btnOpenConfig";
            this.btnOpenConfig.Size = new System.Drawing.Size(100, 30);
            this.btnOpenConfig.TabIndex = 200;
            this.btnOpenConfig.Text = "Open Config File";
            this.btnOpenConfig.UseVisualStyleBackColor = false;
            this.btnOpenConfig.Click += new System.EventHandler(this.btnOpenConfig_Click);
            // 
            // groupSetting
            // 
            this.groupSetting.BackColor = System.Drawing.SystemColors.Control;
            this.groupSetting.Controls.Add(this.checkBackup);
            this.groupSetting.Controls.Add(this.checkMain);
            this.groupSetting.Controls.Add(this.txtPortBackup);
            this.groupSetting.Controls.Add(this.txtIpBackup);
            this.groupSetting.Controls.Add(this.label1);
            this.groupSetting.Controls.Add(this.label2);
            this.groupSetting.Controls.Add(this.btnOpenConfig);
            this.groupSetting.Controls.Add(this.btnSaveConfig);
            this.groupSetting.Controls.Add(this.btnDisconnectKarisma);
            this.groupSetting.Controls.Add(this.btnConnectKarisma);
            this.groupSetting.Controls.Add(this.txtPortMain);
            this.groupSetting.Controls.Add(this.txtIpMain);
            this.groupSetting.Controls.Add(this.label85);
            this.groupSetting.Controls.Add(this.label87);
            this.groupSetting.Controls.Add(this.label88);
            this.groupSetting.Controls.Add(this.txtWorkingFolder);
            this.groupSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupSetting.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.groupSetting.Location = new System.Drawing.Point(28, 21);
            this.groupSetting.Name = "groupSetting";
            this.groupSetting.Size = new System.Drawing.Size(743, 217);
            this.groupSetting.TabIndex = 202;
            this.groupSetting.TabStop = false;
            this.groupSetting.Text = "CONNECTION";
            // 
            // checkBackup
            // 
            this.checkBackup.AutoSize = true;
            this.checkBackup.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.checkBackup.Location = new System.Drawing.Point(585, 80);
            this.checkBackup.Name = "checkBackup";
            this.checkBackup.Size = new System.Drawing.Size(15, 14);
            this.checkBackup.TabIndex = 206;
            this.checkBackup.UseVisualStyleBackColor = true;
            // 
            // checkMain
            // 
            this.checkMain.AutoSize = true;
            this.checkMain.Checked = true;
            this.checkMain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkMain.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.checkMain.Location = new System.Drawing.Point(585, 39);
            this.checkMain.Name = "checkMain";
            this.checkMain.Size = new System.Drawing.Size(15, 14);
            this.checkMain.TabIndex = 205;
            this.checkMain.UseVisualStyleBackColor = true;
            // 
            // txtPortBackup
            // 
            this.txtPortBackup.Location = new System.Drawing.Point(499, 75);
            this.txtPortBackup.Name = "txtPortBackup";
            this.txtPortBackup.Size = new System.Drawing.Size(70, 25);
            this.txtPortBackup.TabIndex = 203;
            // 
            // txtIpBackup
            // 
            this.txtIpBackup.Location = new System.Drawing.Point(126, 75);
            this.txtIpBackup.Name = "txtIpBackup";
            this.txtIpBackup.Size = new System.Drawing.Size(276, 25);
            this.txtIpBackup.TabIndex = 204;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(408, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 19);
            this.label1.TabIndex = 202;
            this.label1.Text = "Port Backup:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 19);
            this.label2.TabIndex = 201;
            this.label2.Text = "IP Backup:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // logBoxKarisma
            // 
            this.logBoxKarisma.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logBoxKarisma.BackColor = System.Drawing.SystemColors.InfoText;
            this.logBoxKarisma.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.logBoxKarisma.ForeColor = System.Drawing.SystemColors.Info;
            this.logBoxKarisma.FormattingEnabled = true;
            this.logBoxKarisma.HorizontalScrollbar = true;
            this.logBoxKarisma.ItemHeight = 17;
            this.logBoxKarisma.Location = new System.Drawing.Point(15, 33);
            this.logBoxKarisma.Name = "logBoxKarisma";
            this.logBoxKarisma.Size = new System.Drawing.Size(706, 242);
            this.logBoxKarisma.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.btnUpdateLog);
            this.groupBox1.Controls.Add(this.btnXoaLog);
            this.groupBox1.Controls.Add(this.logBoxKarisma);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.groupBox1.Location = new System.Drawing.Point(28, 254);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(743, 346);
            this.groupBox1.TabIndex = 203;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "STATUS KARISMA";
            // 
            // btnUpdateLog
            // 
            this.btnUpdateLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(182)))), ((int)(((byte)(213)))));
            this.btnUpdateLog.FlatAppearance.BorderSize = 0;
            this.btnUpdateLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateLog.ForeColor = System.Drawing.Color.White;
            this.btnUpdateLog.Location = new System.Drawing.Point(511, 297);
            this.btnUpdateLog.Name = "btnUpdateLog";
            this.btnUpdateLog.Size = new System.Drawing.Size(100, 30);
            this.btnUpdateLog.TabIndex = 203;
            this.btnUpdateLog.Text = "Update Log";
            this.btnUpdateLog.UseVisualStyleBackColor = false;
            this.btnUpdateLog.Click += new System.EventHandler(this.btnUpdateLog_Click);
            // 
            // btnXoaLog
            // 
            this.btnXoaLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXoaLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(182)))), ((int)(((byte)(213)))));
            this.btnXoaLog.FlatAppearance.BorderSize = 0;
            this.btnXoaLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaLog.ForeColor = System.Drawing.Color.White;
            this.btnXoaLog.Location = new System.Drawing.Point(619, 297);
            this.btnXoaLog.Name = "btnXoaLog";
            this.btnXoaLog.Size = new System.Drawing.Size(100, 30);
            this.btnXoaLog.TabIndex = 202;
            this.btnXoaLog.Text = "Xóa Log";
            this.btnXoaLog.UseVisualStyleBackColor = false;
            this.btnXoaLog.Click += new System.EventHandler(this.btnXoaLog_Click);
            // 
            // frmConnectCG3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 636);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupSetting);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConnectCG3";
            this.Text = "Connect KARISMACG3";
            this.Load += new System.EventHandler(this.frmConnectCG3_Load);
            this.groupSetting.ResumeLayout(false);
            this.groupSetting.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox txtWorkingFolder;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.TextBox txtIpMain;
        private System.Windows.Forms.TextBox txtPortMain;
        private System.Windows.Forms.Button btnConnectKarisma;
        private System.Windows.Forms.Button btnDisconnectKarisma;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Button btnOpenConfig;
        private System.Windows.Forms.GroupBox groupSetting;
        private System.Windows.Forms.ListBox logBoxKarisma;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnXoaLog;
        private System.Windows.Forms.Button btnUpdateLog;
        private System.Windows.Forms.CheckBox checkBackup;
        private System.Windows.Forms.CheckBox checkMain;
        private System.Windows.Forms.TextBox txtPortBackup;
        private System.Windows.Forms.TextBox txtIpBackup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}