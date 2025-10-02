namespace News2025.Menu
{
    partial class frmDiaDanh
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnThoat = new System.Windows.Forms.Button();
            this.btnGhiLai = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.textBox1.Location = new System.Drawing.Point(2, 3);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(796, 272);
            this.textBox1.TabIndex = 27;
            // 
            // btnThoat
            // 
            this.btnThoat.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnThoat.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnThoat.Location = new System.Drawing.Point(399, 281);
            this.btnThoat.Margin = new System.Windows.Forms.Padding(2);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(156, 34);
            this.btnThoat.TabIndex = 26;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // btnGhiLai
            // 
            this.btnGhiLai.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnGhiLai.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnGhiLai.Location = new System.Drawing.Point(227, 281);
            this.btnGhiLai.Margin = new System.Windows.Forms.Padding(2);
            this.btnGhiLai.Name = "btnGhiLai";
            this.btnGhiLai.Size = new System.Drawing.Size(156, 34);
            this.btnGhiLai.TabIndex = 25;
            this.btnGhiLai.Text = "Ghi lại";
            this.btnGhiLai.UseVisualStyleBackColor = true;
            this.btnGhiLai.Click += new System.EventHandler(this.btnGhiLai_Click);
            // 
            // frmDiaDanh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 355);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnGhiLai);
            this.Name = "frmDiaDanh";
            this.Text = "Địa danh";
            this.Load += new System.EventHandler(this.frmDiaDanh_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Button btnGhiLai;
    }
}