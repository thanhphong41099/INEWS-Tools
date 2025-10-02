namespace News2025.Menu
{
    partial class frmTroiTinTuc
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTroiTinTuc));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.txtTroiTinTuc = new System.Windows.Forms.TextBox();
            this.btnThoat = new System.Windows.Forms.Button();
            this.btnGhiLai = new System.Windows.Forms.Button();
            this.btnXoaTroiTin = new System.Windows.Forms.Button();
            this.btnCapNhatTinTuc = new System.Windows.Forms.Button();
            this.btnTroiTin = new System.Windows.Forms.Button();
            this.btnChonFile = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 939);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(484, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // txtTroiTinTuc
            // 
            this.txtTroiTinTuc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTroiTinTuc.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtTroiTinTuc.Location = new System.Drawing.Point(29, 94);
            this.txtTroiTinTuc.Margin = new System.Windows.Forms.Padding(4);
            this.txtTroiTinTuc.Multiline = true;
            this.txtTroiTinTuc.Name = "txtTroiTinTuc";
            this.txtTroiTinTuc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTroiTinTuc.Size = new System.Drawing.Size(255, 797);
            this.txtTroiTinTuc.TabIndex = 28;
            // 
            // btnThoat
            // 
            this.btnThoat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThoat.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnThoat.Location = new System.Drawing.Point(303, 378);
            this.btnThoat.Margin = new System.Windows.Forms.Padding(2);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(153, 34);
            this.btnThoat.TabIndex = 27;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // btnGhiLai
            // 
            this.btnGhiLai.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGhiLai.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnGhiLai.Location = new System.Drawing.Point(303, 340);
            this.btnGhiLai.Margin = new System.Windows.Forms.Padding(2);
            this.btnGhiLai.Name = "btnGhiLai";
            this.btnGhiLai.Size = new System.Drawing.Size(153, 34);
            this.btnGhiLai.TabIndex = 26;
            this.btnGhiLai.Text = "Ghi lại";
            this.btnGhiLai.UseVisualStyleBackColor = true;
            this.btnGhiLai.Click += new System.EventHandler(this.btnGhiLai_Click);
            // 
            // btnXoaTroiTin
            // 
            this.btnXoaTroiTin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXoaTroiTin.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnXoaTroiTin.Location = new System.Drawing.Point(303, 250);
            this.btnXoaTroiTin.Margin = new System.Windows.Forms.Padding(2);
            this.btnXoaTroiTin.Name = "btnXoaTroiTin";
            this.btnXoaTroiTin.Size = new System.Drawing.Size(153, 34);
            this.btnXoaTroiTin.TabIndex = 25;
            this.btnXoaTroiTin.Text = "Xóa trôi tin";
            this.btnXoaTroiTin.UseVisualStyleBackColor = true;
            this.btnXoaTroiTin.Click += new System.EventHandler(this.btnXoaTroiTin_Click);
            // 
            // btnCapNhatTinTuc
            // 
            this.btnCapNhatTinTuc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCapNhatTinTuc.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnCapNhatTinTuc.Location = new System.Drawing.Point(303, 211);
            this.btnCapNhatTinTuc.Margin = new System.Windows.Forms.Padding(2);
            this.btnCapNhatTinTuc.Name = "btnCapNhatTinTuc";
            this.btnCapNhatTinTuc.Size = new System.Drawing.Size(153, 34);
            this.btnCapNhatTinTuc.TabIndex = 24;
            this.btnCapNhatTinTuc.Text = "Cập nhật tin tức";
            this.btnCapNhatTinTuc.UseVisualStyleBackColor = true;
            this.btnCapNhatTinTuc.Click += new System.EventHandler(this.btnCapNhatTinTuc_Click);
            // 
            // btnTroiTin
            // 
            this.btnTroiTin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTroiTin.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnTroiTin.Location = new System.Drawing.Point(303, 172);
            this.btnTroiTin.Margin = new System.Windows.Forms.Padding(2);
            this.btnTroiTin.Name = "btnTroiTin";
            this.btnTroiTin.Size = new System.Drawing.Size(153, 34);
            this.btnTroiTin.TabIndex = 23;
            this.btnTroiTin.Text = "Trôi tin";
            this.btnTroiTin.UseVisualStyleBackColor = true;
            this.btnTroiTin.Click += new System.EventHandler(this.btnTroiTin_Click);
            // 
            // btnChonFile
            // 
            this.btnChonFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChonFile.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnChonFile.Location = new System.Drawing.Point(303, 91);
            this.btnChonFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnChonFile.Name = "btnChonFile";
            this.btnChonFile.Size = new System.Drawing.Size(153, 34);
            this.btnChonFile.TabIndex = 22;
            this.btnChonFile.Text = "Chọn file";
            this.btnChonFile.UseVisualStyleBackColor = true;
            this.btnChonFile.Click += new System.EventHandler(this.btnChonFile_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(29, 42);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(427, 22);
            this.textBox1.TabIndex = 29;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // frmTroiTinTuc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 961);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtTroiTinTuc);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnGhiLai);
            this.Controls.Add(this.btnXoaTroiTin);
            this.Controls.Add(this.btnCapNhatTinTuc);
            this.Controls.Add(this.btnTroiTin);
            this.Controls.Add(this.btnChonFile);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmTroiTinTuc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Trôi tin tức";
            this.Load += new System.EventHandler(this.frmTroiTinTuc_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox txtTroiTinTuc;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Button btnGhiLai;
        private System.Windows.Forms.Button btnXoaTroiTin;
        private System.Windows.Forms.Button btnCapNhatTinTuc;
        private System.Windows.Forms.Button btnTroiTin;
        private System.Windows.Forms.Button btnChonFile;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer timer;
    }
}