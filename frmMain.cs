using News2025.Services;
using News2025.Menu;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace News2025
{
    public partial class frmMain : Form, IStatusReporter
    {

        private NewslineControl1 NewslineControl;
        private DailyBizControl1 DailyBizControl;
        private CultureSceneControl1 CultureSceneControl;

        private readonly IKarismaCG3Model _karismaCG3;

        private frmConnectCG3 _frmConnectCG3;
        private frmAbout _frmAbout;
        public static frmCheckLicenseKey frmCheckLicenseKey;


        public frmMain(IServiceProvider provider)
        {
            InitializeComponent();
            _karismaCG3 = new DualKarismaCG3Model();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitializefrmConnectCG3();
            string licenseKey = LicenseKeyHandler.readLicenseLocalFile();
            bool isLicenseKeyValid = LicenseKeyHandler.onCheckLicenseKeyIsValid(licenseKey, false);

            if (!isLicenseKeyValid)
            {
                OpenfrmCheckLicenseKey();
            }
            else
            {
                OpenUCWhenLicenseKeyOk();
            }
        }
        //Open form check key
        private void OpenfrmCheckLicenseKey()
        {
            if (frmCheckLicenseKey == null || frmCheckLicenseKey.IsDisposed)
            {
                frmCheckLicenseKey = new frmCheckLicenseKey();
                frmCheckLicenseKey.TopLevel = false;
                frmCheckLicenseKey.Dock = DockStyle.None;

                panelMainContent.Controls.Clear();
                panelMainContent.Controls.Add(frmCheckLicenseKey);

                frmCheckLicenseKey.Location = new Point(
                    (panelMainContent.Width - frmCheckLicenseKey.Width) / 2,
                    (panelMainContent.Height - frmCheckLicenseKey.Height) / 2
                );

                frmCheckLicenseKey.Show();
                frmCheckLicenseKey.BringToFront();
                panelMainContent.Visible = true;

                panelMainContent.Controls.Add(flowLayoutPanel1);

            }
            else
            {
                frmCheckLicenseKey.BringToFront();
            }
        }

        private void OpenUCWhenLicenseKeyOk()
        {
            panelMainContent.Controls.Clear();
            panelMainContent.Controls.Add(flowLayoutPanel1);

            var VNConfig = ConfigService.GetConfigSection("NewslineSettings");
            _karismaCG3.space = Convert.ToInt32(VNConfig["KhoangCachTin"]);
        }

        private void OpenNewslineUserControl()
        {
            // Add NewslineControl to main panel on load
            if (NewslineControl == null || NewslineControl.IsDisposed)
            {
                NewslineControl = new NewslineControl1(_karismaCG3, this); // Truyền các service vào
                NewslineControl.Dock = DockStyle.Fill;
            }

            panelMainContent.Controls.Clear();
            panelMainContent.Controls.Add(NewslineControl);
            panelMainContent.Visible = true;
        }

        private void OpenDailyBizUserControl()
        {
            // Add DailyBizControl to main panel
            if (DailyBizControl == null || DailyBizControl.IsDisposed)
            {
                DailyBizControl = new DailyBizControl1(_karismaCG3, this); // Truyền các service vào
                DailyBizControl.Dock = DockStyle.Fill;
            }

            panelMainContent.Controls.Clear();
            panelMainContent.Controls.Add(DailyBizControl);
            panelMainContent.Visible = true;

            // Load DailyBiz configuration similar to NewslineSettings
            try
            {
                var dailyBizConfig = ConfigService.GetConfigSection("DailyBizSettings");
                if (dailyBizConfig != null && dailyBizConfig["KhoangCachTin"] != null)
                {
                    _karismaCG3.space = Convert.ToInt32(dailyBizConfig["KhoangCachTin"]);
                }
            }
            catch (Exception ex)
            {
                // Fallback to default configuration if DailyBizSettings not found
                var VNConfig = ConfigService.GetConfigSection("NewslineSettings");
                _karismaCG3.space = Convert.ToInt32(VNConfig["KhoangCachTin"]);
            }
        }

        private void OpenCultureSceneUserControl()
        {
            // Add CultureSceneControl to main panel
            if (CultureSceneControl == null || CultureSceneControl.IsDisposed)
            {
                CultureSceneControl = new CultureSceneControl1(_karismaCG3, this); // Truyền các service vào
                CultureSceneControl.Dock = DockStyle.Fill;
            }

            panelMainContent.Controls.Clear();
            panelMainContent.Controls.Add(CultureSceneControl);
            panelMainContent.Visible = true;

            // Load CultureScene configuration similar to DailyBizSettings
            try
            {
                var cultureSceneConfig = ConfigService.GetConfigSection("CultureSceneSettings");
                if (cultureSceneConfig != null && cultureSceneConfig["KhoangCachTin"] != null)
                {
                    _karismaCG3.space = Convert.ToInt32(cultureSceneConfig["KhoangCachTin"]);
                }
            }
            catch (Exception ex)
            {
                // Fallback to default configuration if CultureSceneSettings not found
                var VNConfig = ConfigService.GetConfigSection("NewslineSettings");
                _karismaCG3.space = Convert.ToInt32(VNConfig["KhoangCachTin"]);
            }
        }

        private void connectCGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openfrmConnectCG3();
        }
        private void openfrmConnectCG3()
        {
            if (_frmConnectCG3 == null || _frmConnectCG3.IsDisposed)
            {
                _frmConnectCG3 = new frmConnectCG3(_karismaCG3);

                // Lấy vị trí và kích thước của frmMain
                var mainFormBounds = this.Bounds;

                // Đặt vị trí form ở chính giữa frmMain
                _frmConnectCG3.StartPosition = FormStartPosition.Manual;
                _frmConnectCG3.Location = new Point(
                    mainFormBounds.Left + (mainFormBounds.Width - _frmConnectCG3.Width) / 2,
                    mainFormBounds.Top + (mainFormBounds.Height - _frmConnectCG3.Height) / 2
                );

                _frmConnectCG3.Show();
            }
            else
            {
                _frmConnectCG3.Show();
                _frmConnectCG3.BringToFront(); // Đưa form đã mở lên phía trước
            }
        }

        private void InitializefrmConnectCG3()
        {
            if (_frmConnectCG3 == null || _frmConnectCG3.IsDisposed)
            {
                _frmConnectCG3 = new frmConnectCG3(_karismaCG3);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_frmAbout == null || _frmAbout.IsDisposed)
            {
                _frmAbout = new frmAbout();

                var mainFormBounds = this.Bounds;

                _frmAbout.StartPosition = FormStartPosition.Manual;
                _frmAbout.Location = new Point(
                    mainFormBounds.Left + (mainFormBounds.Width - _frmAbout.Width) / 2,
                    mainFormBounds.Top + (mainFormBounds.Height - _frmAbout.Height) / 2
                );

                _frmAbout.Show();
            }
            else
            {
                _frmAbout.BringToFront(); // Đưa form đã mở lên phía trước
            }
        }

        private void layoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string licenseKey = LicenseKeyHandler.readLicenseLocalFile();
            bool isLicenseKeyValid = LicenseKeyHandler.onCheckLicenseKeyIsValid(licenseKey, false);

            if (isLicenseKeyValid)
            {
                NewslineToolStripMenuItem.Enabled = true;
                DailyBizToolStripMenuItem.Enabled = true;
                CultureSceneToolStripMenuItem.Enabled = true;
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOption frmOption = new frmOption(_karismaCG3);

            frmOption.ShowDialog();
            frmOption.BringToFront();
        }

        #region IStatusReporter
        public void UpdateStatusLabel(string text)
        {
            toolStripStatusLabel1.Text = text;
        }
        #endregion

        private void NewslineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenNewslineUserControl();
        }

        private void btnNewsline_Click(object sender, EventArgs e)
        {
            OpenNewslineUserControl();
        }

        private void btnDailyBiz_Click(object sender, EventArgs e)
        {
            OpenDailyBizUserControl();
        }

        private void btnCultureScene_Click(object sender, EventArgs e)
        {
            OpenCultureSceneUserControl();
        }

        private void DailyBizToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDailyBizUserControl();
        }

        private void CultureSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenCultureSceneUserControl();
        }
    }
    public interface IStatusReporter
    {
        void UpdateStatusLabel(string message);
    }
}
