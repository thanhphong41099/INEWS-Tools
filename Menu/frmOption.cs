using News2025.Services;
using System;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace News2025.Menu
{
    public partial class frmOption : Form
    {
        //NameValueCollection thoiSuConfig;
        private readonly IKarismaCG3Model _karismaCG3;

        string outputPath = "D:\\KarismaNews\\DungChung_CG3\\SCENES\\Images\\preview.png";

        public frmOption(IKarismaCG3Model karismaCG3)
        {
            InitializeComponent();
            _karismaCG3 = karismaCG3;
        }

        private void frmOption_Load(object sender, EventArgs e)
        {

        }

        private void btnBarIn_Click(object sender, EventArgs e)
        {
            //Open file Browser to choose file for Bar In
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn hình ảnh cho Bar In";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtBarIn.Text = openFileDialog.FileName;
            }
        }

        private void btnBarOut_Click(object sender, EventArgs e)
        {
            //Open file Browser to choose file for Bar Out
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn hình ảnh cho Bar Out";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtBarOut.Text = openFileDialog.FileName;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            var config = new LayoutConfig
            {
                Name = txtName.Text,
                BarIn = txtBarIn.Text,
                BarOut = txtBarOut.Text,
                DiaDanh = txtDiaDanh.Text,
                Troi = txtTroi.Text,
                X = int.TryParse(txtX.Text, out var x) ? x : (int?)null,
                Y = int.TryParse(txtY.Text, out var y) ? y : (int?)null,
                DelayIn = int.TryParse(txtDelayIn.Text, out var dIn) ? dIn : (int?)null,
                DelayOut = int.TryParse(txtDelayOut.Text, out var dOut) ? dOut : (int?)null,
                Red = int.TryParse(txtRed.Text, out var r) ? r : (int?)null,
                Green = int.TryParse(txtGreen.Text, out var g) ? g : (int?)null,
                Blue = int.TryParse(txtBlue.Text, out var b) ? b : (int?)null
            };

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Lưu file cấu hình layout";
                saveDialog.Filter = "JSON files (*.json)|*.json";
                saveDialog.DefaultExt = "json";
                saveDialog.FileName = string.IsNullOrWhiteSpace(config.Name) ? "layout_config" : config.Name;

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(saveDialog.FileName, json);
                    MessageBox.Show($"Đã lưu cấu hình tại:\n{saveDialog.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn hình ảnh cho Bar Địa danh";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtDiaDanh.Text = openFileDialog.FileName;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn hình ảnh cho Bar Trôi";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtTroi.Text = openFileDialog.FileName;
            }
        }

        private void btnOpenConfig_Click(object sender, EventArgs e)
        {
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Title = "Chọn file cấu hình layout";
                openDialog.Filter = "JSON files (*.json)|*.json";
                openDialog.InitialDirectory = Application.StartupPath;

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string json = File.ReadAllText(openDialog.FileName);
                        var config = Newtonsoft.Json.JsonConvert.DeserializeObject<LayoutConfig>(json);

                        // Gán dữ liệu vào các TextBox
                        txtName.Text = config.Name ?? "";
                        txtBarIn.Text = config.BarIn ?? "";
                        txtBarOut.Text = config.BarOut ?? "";
                        txtDiaDanh.Text = config.DiaDanh ?? "";
                        txtTroi.Text = config.Troi ?? "";
                        txtX.Text = config.X?.ToString() ?? "";
                        txtY.Text = config.Y?.ToString() ?? "";
                        txtDelayIn.Text = config.DelayIn?.ToString() ?? "";
                        txtDelayOut.Text = config.DelayOut?.ToString() ?? "";
                        txtRed.Text = config.Red?.ToString() ?? "";
                        txtGreen.Text = config.Green?.ToString() ?? "";
                        txtBlue.Text = config.Blue?.ToString() ?? "";

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi đọc file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string scenePath = "D:\\KarismaNews\\DungChung_CG3\\SCENES\\Lowerthird-in.tscn";
            _karismaCG3.PlaySceneBar(scenePath, 0, txtBarIn.Text);

            string textPath = "D:\\KarismaNews\\DungChung_CG3\\SCENES\\text-in.tscn";
            PlayTextWithPosition(textPath, 1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string textPath = "D:\\KarismaNews\\DungChung_CG3\\SCENES\\text-out.tscn";
            PlayTextWithPosition(textPath, 1);

            string scenePath = "D:\\KarismaNews\\DungChung_CG3\\SCENES\\Lowerthird-out.tscn";
            _karismaCG3.PlaySceneBar(scenePath, 0, txtBarOut.Text);
        }

        private void PlayTextWithPosition(string scenePath, int layer)
        {
            string Line1 = txtDong1.Text;
            string Line2 = txtDong2.Text;

            int X = int.TryParse(txtX.Text, out var x) ? x : 0;
            int Y = int.TryParse(txtY.Text, out var y) ? y : 0;

            _karismaCG3.PlaySceneLTPosition(scenePath, layer, Line1, Line2, X, Y);
        }

        private void txtTroi_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
