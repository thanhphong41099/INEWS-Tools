namespace News2025.Menu
{
    partial class frmBinhLuan
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
            this.txtNoiDung = new System.Windows.Forms.TextBox();
            this.btnThoat = new System.Windows.Forms.Button();
            this.btnGhiLai = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtNoiDung
            // 
            this.txtNoiDung.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNoiDung.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtNoiDung.Location = new System.Drawing.Point(14, -1);
            this.txtNoiDung.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtNoiDung.Multiline = true;
            this.txtNoiDung.Name = "txtNoiDung";
            this.txtNoiDung.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNoiDung.Size = new System.Drawing.Size(1087, 462);
            this.txtNoiDung.TabIndex = 27;
            // 
            // btnThoat
            // 
            this.btnThoat.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnThoat.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnThoat.Location = new System.Drawing.Point(454, 468);
            this.btnThoat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(150, 30);
            this.btnThoat.TabIndex = 26;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // btnGhiLai
            // 
            this.btnGhiLai.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnGhiLai.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnGhiLai.Location = new System.Drawing.Point(298, 468);
            this.btnGhiLai.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGhiLai.Name = "btnGhiLai";
            this.btnGhiLai.Size = new System.Drawing.Size(150, 30);
            this.btnGhiLai.TabIndex = 25;
            this.btnGhiLai.Text = "Ghi lại";
            this.btnGhiLai.UseVisualStyleBackColor = true;
            this.btnGhiLai.Click += new System.EventHandler(this.btnGhiLai_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOpenFile.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnOpenFile.Location = new System.Drawing.Point(142, 468);
            this.btnOpenFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(150, 30);
            this.btnOpenFile.TabIndex = 28;
            this.btnOpenFile.Text = "Chọn file";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(631, 475);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 17);
            this.label1.TabIndex = 29;
            // 
            // frmBinhLuan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 515);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.txtNoiDung);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnGhiLai);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmBinhLuan";
            this.Text = "Bình luận";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNoiDung;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Button btnGhiLai;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Label label1;
    }
}