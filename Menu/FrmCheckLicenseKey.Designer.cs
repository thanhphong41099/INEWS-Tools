namespace News2025.Menu
{
    partial class frmCheckLicenseKey
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
            this.label4 = new System.Windows.Forms.Label();
            this.txtProductID = new System.Windows.Forms.TextBox();
            this.checkKey = new System.Windows.Forms.Button();
            this.txtLicenseKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(567, 285);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 32);
            this.label4.TabIndex = 313;
            this.label4.Text = "License Key";
            // 
            // txtProductID
            // 
            this.txtProductID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtProductID.Location = new System.Drawing.Point(398, 378);
            this.txtProductID.Name = "txtProductID";
            this.txtProductID.ReadOnly = true;
            this.txtProductID.Size = new System.Drawing.Size(484, 23);
            this.txtProductID.TabIndex = 311;
            this.txtProductID.Text = "ProductID";
            // 
            // checkKey
            // 
            this.checkKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(182)))), ((int)(((byte)(213)))));
            this.checkKey.FlatAppearance.BorderSize = 0;
            this.checkKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.checkKey.ForeColor = System.Drawing.Color.White;
            this.checkKey.Location = new System.Drawing.Point(578, 527);
            this.checkKey.Name = "checkKey";
            this.checkKey.Size = new System.Drawing.Size(125, 38);
            this.checkKey.TabIndex = 310;
            this.checkKey.Text = "Check";
            this.checkKey.UseVisualStyleBackColor = false;
            this.checkKey.Click += new System.EventHandler(this.checkKey_Click);
            // 
            // txtLicenseKey
            // 
            this.txtLicenseKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtLicenseKey.Location = new System.Drawing.Point(398, 466);
            this.txtLicenseKey.Name = "txtLicenseKey";
            this.txtLicenseKey.Size = new System.Drawing.Size(484, 23);
            this.txtLicenseKey.TabIndex = 309;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(394, 425);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 16);
            this.label2.TabIndex = 308;
            this.label2.Text = "License Key:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(394, 341);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 16);
            this.label1.TabIndex = 307;
            this.label1.Text = "Product ID:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::News2025.Properties.Resources.VTV_CG_Logo;
            this.pictureBox1.Location = new System.Drawing.Point(410, 53);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(461, 199);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 312;
            this.pictureBox1.TabStop = false;
            // 
            // frmCheckLicenseKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1256, 718);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtProductID);
            this.Controls.Add(this.checkKey);
            this.Controls.Add(this.txtLicenseKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCheckLicenseKey";
            this.Text = "frmCheckLicenseKey";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtProductID;
        private System.Windows.Forms.Button checkKey;
        private System.Windows.Forms.TextBox txtLicenseKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}