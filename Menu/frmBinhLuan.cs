using News2025.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace News2025.Menu
{
    public partial class frmBinhLuan : Form
    {
        public static string filePath = string.Empty;
        public static string fileContent =string.Empty;
        public frmBinhLuan()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Chọn file bình luận";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        filePath = openFileDialog.FileName;
                        fileContent = System.IO.File.ReadAllText(filePath);
                        txtNoiDung.Text = fileContent;
                        label1.Text = filePath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi đọc file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnGhiLai_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
                {
                    fileContent = txtNoiDung.Text;
                }
                else
                {
                    System.IO.File.WriteAllText(filePath, txtNoiDung.Text);
                }
            }
            catch (Exception ex)
            {
                label1.Text = "Lỗi khi ghi file: " + ex.Message;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
