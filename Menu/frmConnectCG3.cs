using News2025.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace News2025.Menu
{
    public partial class frmConnectCG3 : Form
    {

        private readonly IKarismaCG3Model _karismaCG3;
        string ipMain, portMain, ipBackup, portBackup;
        public frmConnectCG3(IKarismaCG3Model karismaCG3)
        {
            InitializeComponent();
            _karismaCG3 = karismaCG3;
            LoadConfigFile();
            ConnectKarismaCG3();

            // Cập nhật log sau khi kết nối
            UpdateLog();
        }
        private void frmConnectCG3_Load(object sender, EventArgs e)
        {
            LoadConfigFile();
            ConnectKarismaCG3();

            // Cập nhật log sau khi kết nối
            UpdateLog();
        }
        private void LoadConfigFile()
        {
            try
            {
                var KarismaConfig = ConfigService.GetConfigSection("KarismaSettings");

                // Đọc các giá trị từ App.config
                ipMain = KarismaConfig["IpMain"];
                portMain = KarismaConfig["PortMain"];
                ipBackup = KarismaConfig["IpBackup"];
                portBackup = KarismaConfig["PortBackup"];

                // Hiển thị các giá trị trong TextBox
                txtIpMain.Text = ipMain;
                txtIpBackup.Text = ipBackup;
                txtPortMain.Text = portMain;
                txtPortBackup.Text = portBackup;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Không tìm thấy file cấu hình. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Không có quyền truy cập file cấu hình. Vui lòng kiểm tra quyền truy cập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định khi tải cấu hình: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnConnectKarisma_Click(object sender, EventArgs e)
        {
            ConnectKarismaCG3();
        }
        private async void ConnectKarismaCG3()
        {
            ipMain = txtIpMain.Text.Trim();
            portMain = txtPortMain.Text.Trim();
            ipBackup = txtIpBackup.Text.Trim();
            portBackup = txtPortBackup.Text.Trim();

            if (_karismaCG3 is DualKarismaCG3Model dual)
            {
                // Kích hoạt server nào cần chạy
                dual.EnableMain(checkMain.Checked);
                dual.EnableBackup(checkBackup.Checked);

                var tasks = new List<Task>();

                if (checkMain.Checked)
                {
                    if (string.IsNullOrEmpty(ipMain) || string.IsNullOrEmpty(portMain))
                    {
                        MessageBox.Show("Vui lòng nhập IP và Port cho Main!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    tasks.Add(Task.Run(() => dual.ConnectMainCG(ipMain, portMain)));
                }

                if (checkBackup.Checked)
                {
                    if (string.IsNullOrEmpty(ipBackup) || string.IsNullOrEmpty(portBackup))
                    {
                        MessageBox.Show("Vui lòng nhập IP và Port cho Backup!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    tasks.Add(Task.Run(() => dual.ConnectBackupCG(ipBackup, portBackup)));
                }

                await Task.WhenAll(tasks);
                UpdateLog();
            }
            else
            {
                // Nếu không phải dual, chỉ chạy 1 server
                if (!checkMain.Checked)
                {
                    MessageBox.Show("Chế độ đơn chỉ hỗ trợ kết nối Main. Hãy tick Main.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(ipMain) || string.IsNullOrEmpty(portMain))
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ IP và Port!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    _karismaCG3?.ConnectCG(ipMain, portMain);
                    await Task.Delay(500);
                    UpdateLog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi kết nối CG: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDisconnectKarisma_Click(object sender, EventArgs e)
        {
            try
            {
                _karismaCG3?.DisconnectCG();

                MessageBox.Show("Đã ngắt kết nối CG Server.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Cập nhật log sau khi ngắt kết nối
                UpdateLog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi ngắt kết nối CG Server: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenConfig_Click(object sender, EventArgs e)
        {
            try
            {
                // Tìm file cấu hình thực tế trong thư mục đầu ra
                string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{AppDomain.CurrentDomain.FriendlyName}.config");

                if (File.Exists(configFilePath))
                {
                    System.Diagnostics.Process.Start("notepad.exe", configFilePath);
                }
                else
                {
                    MessageBox.Show("File cấu hình không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Không có quyền mở file cấu hình. Vui lòng kiểm tra quyền truy cập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở file cấu hình: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadConfig_Click(object sender, EventArgs e)
        {
            LoadConfigFile();
        }

        private void btnWorkingFolderBrowse_Click(object sender, EventArgs e)
        {

        }

        private void btnXoaLog_Click(object sender, EventArgs e)
        {
            logBoxKarisma.Items.Clear();
            _karismaCG3.ClearLog();
        }

        private void btnDataBrowser_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdateLog_Click(object sender, EventArgs e)
        {
            UpdateLog();
        }

        private void UpdateLogBox()
        {
            if (logBoxKarisma.InvokeRequired)
            {
                logBoxKarisma.Invoke(new Action(UpdateLogBox));
                return;
            }

            logBoxKarisma.Items.Clear();
            var logs = _karismaCG3.LogText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            logBoxKarisma.Items.AddRange(logs);
            logBoxKarisma.TopIndex = logBoxKarisma.Items.Count - 1; // Tự động cuộn xuống cuối
        }
        private void UpdateLog()
        {
            try
            {
                if (logBoxKarisma.InvokeRequired)
                {
                    logBoxKarisma.Invoke(new Action(UpdateLog));
                    return;
                }

                logBoxKarisma.Items.Clear();
                var logs = _karismaCG3.LogText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                logBoxKarisma.Items.AddRange(logs);
                logBoxKarisma.TopIndex = logBoxKarisma.Items.Count - 1; // Tự động cuộn xuống cuối
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật log: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
