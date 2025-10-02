using News2025.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace News2025.Menu
{
    public partial class frmUTC : Form
    {
        public string fileUTC { get; set; } = "";

        public frmUTC()
        {
            InitializeComponent();
        }

        private void frmUTC_Load(object sender, EventArgs e)
        {
            //var thoiSuConfig = ConfigService.GetConfigSection("ThoiSuSettings");
            //string workingFolder = thoiSuConfig["WorkingFolder"];
            //string fileUTC = Path.Combine(workingFolder, thoiSuConfig["UTC"]);
            LoadFileUTC(fileUTC);
        }
        private void LoadFileUTC(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    string content = System.IO.File.ReadAllText(filePath);
                    textBox1.Text = content;
                }
                else
                {
                    MessageBox.Show("File không tồn tại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc file: " + ex.Message);
            }
        }

        private void btnGhiLai_Click(object sender, EventArgs e)
        {
            // Ghi lại nội dung vào file
            try
            {
                System.IO.File.WriteAllText(fileUTC, textBox1.Text);
                MessageBox.Show("Ghi lại thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi file: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
