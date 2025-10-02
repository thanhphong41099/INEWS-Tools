namespace API_iNews
{
    partial class API
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(API));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnExportAllRawContent = new System.Windows.Forms.Button();
            this.btnExportContentRaw = new System.Windows.Forms.Button();
            this.btnGetStory = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnThoat = new System.Windows.Forms.Button();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDiadanh = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTroiCuoi = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTroiTin = new System.Windows.Forms.TextBox();
            this.txtRuttitCG = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btnXuatTroiTin = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnExportAllRawContent);
            this.splitContainer1.Panel1.Controls.Add(this.btnExportContentRaw);
            this.splitContainer1.Panel1.Controls.Add(this.btnGetStory);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1516, 783);
            this.splitContainer1.SplitterDistance = 290;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnExportAllRawContent
            // 
            this.btnExportAllRawContent.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.btnExportAllRawContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportAllRawContent.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnExportAllRawContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnExportAllRawContent.Location = new System.Drawing.Point(12, 635);
            this.btnExportAllRawContent.Margin = new System.Windows.Forms.Padding(2);
            this.btnExportAllRawContent.Name = "btnExportAllRawContent";
            this.btnExportAllRawContent.Size = new System.Drawing.Size(255, 35);
            this.btnExportAllRawContent.TabIndex = 33;
            this.btnExportAllRawContent.Text = "Xuất toàn bộ nội dung";
            this.btnExportAllRawContent.UseVisualStyleBackColor = true;
            this.btnExportAllRawContent.Click += new System.EventHandler(this.btnExportAllRawContent_Click);
            // 
            // btnExportContentRaw
            // 
            this.btnExportContentRaw.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.btnExportContentRaw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportContentRaw.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnExportContentRaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnExportContentRaw.Location = new System.Drawing.Point(12, 596);
            this.btnExportContentRaw.Margin = new System.Windows.Forms.Padding(2);
            this.btnExportContentRaw.Name = "btnExportContentRaw";
            this.btnExportContentRaw.Size = new System.Drawing.Size(255, 35);
            this.btnExportContentRaw.TabIndex = 32;
            this.btnExportContentRaw.Text = "Xuất nội dung gốc";
            this.btnExportContentRaw.UseVisualStyleBackColor = true;
            this.btnExportContentRaw.Click += new System.EventHandler(this.btnExportContentRaw_Click);
            // 
            // btnGetStory
            // 
            this.btnGetStory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetStory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnGetStory.Location = new System.Drawing.Point(14, 723);
            this.btnGetStory.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetStory.Name = "btnGetStory";
            this.btnGetStory.Size = new System.Drawing.Size(255, 35);
            this.btnGetStory.TabIndex = 31;
            this.btnGetStory.Text = "Reconnect Apache";
            this.btnGetStory.UseVisualStyleBackColor = true;
            this.btnGetStory.Click += new System.EventHandler(this.btnGetStory_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label2.Location = new System.Drawing.Point(13, 568);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 18);
            this.label2.TabIndex = 30;
            this.label2.Text = "Label trạng thái";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label3.Location = new System.Drawing.Point(13, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 18);
            this.label3.TabIndex = 10;
            this.label3.Text = "Cây ruttit";
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.treeView1.Location = new System.Drawing.Point(0, 46);
            this.treeView1.Margin = new System.Windows.Forms.Padding(2);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(284, 512);
            this.treeView1.TabIndex = 9;
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnThoat);
            this.panel1.Controls.Add(this.txtContent);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtDiadanh);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtTroiCuoi);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtTroiTin);
            this.panel1.Controls.Add(this.txtRuttitCG);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.btnXuatTroiTin);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1219, 779);
            this.panel1.TabIndex = 0;
            // 
            // btnThoat
            // 
            this.btnThoat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnThoat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnThoat.Location = new System.Drawing.Point(748, 725);
            this.btnThoat.Margin = new System.Windows.Forms.Padding(2);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(89, 30);
            this.btnThoat.TabIndex = 31;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // txtContent
            // 
            this.txtContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtContent.Location = new System.Drawing.Point(4, 568);
            this.txtContent.Margin = new System.Windows.Forms.Padding(4);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.Size = new System.Drawing.Size(829, 151);
            this.txtContent.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label1.Location = new System.Drawing.Point(844, 571);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 18);
            this.label1.TabIndex = 29;
            this.label1.Text = "Địa danh";
            // 
            // txtDiadanh
            // 
            this.txtDiadanh.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDiadanh.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtDiadanh.Location = new System.Drawing.Point(842, 596);
            this.txtDiadanh.Margin = new System.Windows.Forms.Padding(4);
            this.txtDiadanh.Multiline = true;
            this.txtDiadanh.Name = "txtDiadanh";
            this.txtDiadanh.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDiadanh.Size = new System.Drawing.Size(348, 123);
            this.txtDiadanh.TabIndex = 28;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label7.Location = new System.Drawing.Point(844, 449);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(227, 18);
            this.label7.TabIndex = 25;
            this.label7.Text = "Xem trước dữ liệu trôi cuối bản tin";
            // 
            // txtTroiCuoi
            // 
            this.txtTroiCuoi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTroiCuoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtTroiCuoi.Location = new System.Drawing.Point(842, 474);
            this.txtTroiCuoi.Margin = new System.Windows.Forms.Padding(4);
            this.txtTroiCuoi.Multiline = true;
            this.txtTroiCuoi.Name = "txtTroiCuoi";
            this.txtTroiCuoi.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTroiCuoi.Size = new System.Drawing.Size(348, 84);
            this.txtTroiCuoi.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label6.Location = new System.Drawing.Point(840, 321);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(244, 18);
            this.label6.TabIndex = 23;
            this.label6.Text = "Xem trước dữ liệu trôi ngang - tin tức";
            // 
            // txtTroiTin
            // 
            this.txtTroiTin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTroiTin.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtTroiTin.Location = new System.Drawing.Point(843, 343);
            this.txtTroiTin.Margin = new System.Windows.Forms.Padding(4);
            this.txtTroiTin.Multiline = true;
            this.txtTroiTin.Name = "txtTroiTin";
            this.txtTroiTin.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTroiTin.Size = new System.Drawing.Size(348, 98);
            this.txtTroiTin.TabIndex = 22;
            // 
            // txtRuttitCG
            // 
            this.txtRuttitCG.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRuttitCG.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtRuttitCG.Location = new System.Drawing.Point(843, 46);
            this.txtRuttitCG.Margin = new System.Windows.Forms.Padding(4);
            this.txtRuttitCG.Multiline = true;
            this.txtRuttitCG.Name = "txtRuttitCG";
            this.txtRuttitCG.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRuttitCG.Size = new System.Drawing.Size(348, 267);
            this.txtRuttitCG.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label5.Location = new System.Drawing.Point(847, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(155, 18);
            this.label5.TabIndex = 12;
            this.label5.Text = "Xem trước dữ liệu ruttit";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label4.Location = new System.Drawing.Point(20, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 18);
            this.label4.TabIndex = 11;
            this.label4.Text = "Story iNews";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button2.Location = new System.Drawing.Point(211, 725);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(174, 30);
            this.button2.TabIndex = 5;
            this.button2.Text = "Xuất tất cả ra file";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Export3TXTFiles_Click);
            // 
            // btnXuatTroiTin
            // 
            this.btnXuatTroiTin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnXuatTroiTin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnXuatTroiTin.Location = new System.Drawing.Point(23, 725);
            this.btnXuatTroiTin.Margin = new System.Windows.Forms.Padding(2);
            this.btnXuatTroiTin.Name = "btnXuatTroiTin";
            this.btnXuatTroiTin.Size = new System.Drawing.Size(174, 30);
            this.btnXuatTroiTin.TabIndex = 5;
            this.btnXuatTroiTin.Text = "Xuất trôi tin - trôi cuối";
            this.btnXuatTroiTin.UseVisualStyleBackColor = true;
            this.btnXuatTroiTin.Click += new System.EventHandler(this.btnXuatTroiTin_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 757);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1219, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(2, 46);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(831, 512);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // API
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1516, 783);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "API";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "API INEWS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.API_FormClosing);
            this.Load += new System.EventHandler(this.API_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnXuatTroiTin;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTroiTin;
        private System.Windows.Forms.TextBox txtRuttitCG;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTroiCuoi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDiadanh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Button btnGetStory;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Button btnExportContentRaw;
        private System.Windows.Forms.Button btnExportAllRawContent;
    }
}