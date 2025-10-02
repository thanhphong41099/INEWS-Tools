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
    public partial class frmTroiNgang : Form
    {
        private readonly IKarismaCG3Model _karismaCG3;
        string selectedFilePath;
        private readonly int speed;
        private readonly int layer;
        private readonly string scenePath;

        private string section = "";

        private string TachTroiCuoi = "";

        private LayoutManager _layoutManager;
        private string barTroi;
        public frmTroiNgang(IKarismaCG3Model karismaCG3, int layer, int speed, string scenePath, string section, LayoutManager layoutManager = null)
        {
            InitializeComponent();
            _karismaCG3 = karismaCG3;
            this.layer = layer;
            this.speed = speed;
            this.scenePath = scenePath;
            this.section = section;
            _layoutManager = layoutManager;
        }
        private void frmTroiNgang_Load(object sender, EventArgs e)
        {
            var TroiConfig = ConfigService.GetConfigSection(section);
            string workingFolder = TroiConfig["WorkingFolder"];
            TachTroiCuoi = TroiConfig["TachTroiCuoi"];
        }
        private void btnChonFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Select a Text File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = openFileDialog.FileName;
                    //hiển thị toàn bọ nội dung file txt vừa đọc được vào ô textBox txtTroiNgang
                    txtTroiNgang.Text = System.IO.File.ReadAllText(selectedFilePath);

                    textBox1.Text = selectedFilePath;
                }
            }
        }

        private void btnTroiTin_Click(object sender, EventArgs e)
        {
            string text = processedText(txtTroiNgang.Text);
            List<string> lines = new List<string> {text };


            if (!string.IsNullOrEmpty(barTroi))
            {
                _karismaCG3.PlaySceneScrollStartBar(scenePath, layer, speed, lines, barTroi);
            }
            else
            {
                _karismaCG3.PlaySceneScrollStart(scenePath, layer, speed, lines);
            }
            _karismaCG3.timerTick(layer);
        }
        public void XoaTroiTin()
        {
            _karismaCG3.PlayOut(layer);
        }
        private void btnXoaTroiTin_Click(object sender, EventArgs e)
        {
            XoaTroiTin();
        }

        private void btnCapNhatTinTuc_Click(object sender, EventArgs e)
        {
            try
            {
                string text = processedText(txtTroiNgang.Text);
                List<string> lines = new List<string> { text };

                _karismaCG3.updateTxtCrawl(lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đọc file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGhiLai_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedFilePath))
            {
                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();

                    string initialDirectory = System.IO.Path.GetDirectoryName(selectedFilePath);
                    string defaultFileName = System.IO.Path.GetFileName(selectedFilePath);

                    saveFileDialog.InitialDirectory = initialDirectory;
                    saveFileDialog.FileName = defaultFileName;
                    saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    saveFileDialog.Title = "Chọn nơi lưu file";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string newFilePath = saveFileDialog.FileName;

                        // Nếu file đã tồn tại → hỏi xác nhận
                        if (System.IO.File.Exists(newFilePath))
                        {
                            DialogResult overwriteConfirm = MessageBox.Show(
                                $"File \"{System.IO.Path.GetFileName(newFilePath)}\" đã tồn tại.\nBạn có muốn ghi đè không?",
                                "Xác nhận ghi đè",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);

                            if (overwriteConfirm != DialogResult.Yes)
                                return; // Người dùng không muốn ghi đè
                        }

                        System.IO.File.WriteAllText(newFilePath, txtTroiNgang.Text);

                        // Cập nhật lại đường dẫn nếu muốn dùng tiếp
                        selectedFilePath = newFilePath;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Chưa có đường dẫn file ban đầu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private string processedText(string input)
        {
            // Chia nhỏ các dòng, lọc dòng trống, loại bỏ khoảng trắng thừa
            var lines = input
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line));

            // Ghép lại thành một dòng duy nhất với dấu TachTroiTin giữa các đoạn
            //return string.Join(TachTroiTin, lines);
            return string.Join(TachTroiCuoi, lines);
        }
    }
}
