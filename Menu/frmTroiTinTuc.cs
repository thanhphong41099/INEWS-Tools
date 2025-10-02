using KAsyncEngineLib;
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
using static System.Collections.Specialized.BitVector32;

namespace News2025.Menu
{
    public partial class frmTroiTinTuc : Form
    {
        private readonly IKarismaCG3Model _karismaCG3;
        private string selectedFilePath;
        private readonly int layer;
        private readonly int speed;
        private readonly string scenePath;

        private string section = "";
        private readonly string barTroi;

        public frmTroiTinTuc(IKarismaCG3Model karisma, int layer, int speed, string scenePath, string Section, string barTroi = null)
        {
            InitializeComponent();
            _karismaCG3 = karisma;
            this.layer = layer;
            this.speed = speed;
            this.scenePath = scenePath;
            this.section = Section;
            this.barTroi = barTroi;
        }
        private void frmTroiTinTuc_Load(object sender, EventArgs e)
        {
            var TroiConfig = ConfigService.GetConfigSection(section);
            string workingFolder = TroiConfig["WorkingFolder"];
            _karismaCG3.seperator = TroiConfig["TachTroiTin"];
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
                    txtTroiTinTuc.Text = File.ReadAllText(selectedFilePath);

                    textBox1.Text = selectedFilePath;
                }
            }
        }

        private void btnTroiTin_Click(object sender, EventArgs e)
        {
            List<string> text = ProcessedLines(txtTroiTinTuc.Text);

            if (!string.IsNullOrEmpty(barTroi))
            {
                _karismaCG3.PlaySceneScrollStartBar(scenePath, layer, speed, text, barTroi);
            }
            else
            {
                _karismaCG3.PlaySceneScrollStart(scenePath, layer, speed, text);
            }
            // To update scroll text, set a timer
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            _karismaCG3.timerTick(layer);
        }

        public void XoaTroiTin()
        {
            timer.Stop();

            _karismaCG3.PlayOut(layer);

            //PlayOut sceneCG
            //_karismaCG3.PlayOut(layerCG);
        }

        private void btnXoaTroiTin_Click(object sender, EventArgs e)
        {
            XoaTroiTin();
        }


        private void btnCapNhatTinTuc_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> text = ProcessedLines(txtTroiTinTuc.Text);
                _karismaCG3.updateTxtCrawl(text);
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

                    string initialDirectory = Path.GetDirectoryName(selectedFilePath);
                    string defaultFileName = Path.GetFileName(selectedFilePath);

                    saveFileDialog.InitialDirectory = initialDirectory;
                    saveFileDialog.FileName = defaultFileName;
                    saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    saveFileDialog.Title = "Chọn nơi lưu file";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string newFilePath = saveFileDialog.FileName;

                        if (File.Exists(newFilePath))
                        {
                            DialogResult overwriteConfirm = MessageBox.Show(
                                $"File \"{Path.GetFileName(newFilePath)}\" đã tồn tại.\nBạn có muốn ghi đè không?",
                                "Xác nhận ghi đè",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);

                            if (overwriteConfirm != DialogResult.Yes)
                                return;
                        }

                        File.WriteAllText(newFilePath, txtTroiTinTuc.Text);

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

        private List<string> ProcessedLines(string input)
        {
            return input
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();
        }

    }
}
