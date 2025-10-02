using API_iNews;
using News2025.Logic;
using News2025.Menu;
using News2025.Models;
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
using System.Configuration;
using System.Collections.Specialized;
using System.Reflection.Emit;
using KAsyncEngineLib;
using System.Threading;

namespace News2025
{
    public partial class CBSControl1 : UserControl
    {
        private ComboBoxDataLoader _comboBoxLoader;

        private readonly IAccessService _accessService;
        private readonly IKarismaCG3Model _karismaCG3;
        private readonly IStatusReporter _statusReporter;
        private readonly VariableState state = new VariableState();

        private frmTroiTinTuc _frmTroiTinTuc;
        private frmTroiNgang _frmTroiNgang;

        private string Line1;
        private string Line2;
        private string fileLogo;
        private string filePopup;
        private bool isPlayLT = false;
        private bool useTriggerLT = true;

        NameValueCollection CBSConfig;
        string workingFolder, fileRuttit, fileDiadanh, fileUTC, filePhude;
        string logoFolder, popupFolder;


        public CBSControl1(IKarismaCG3Model karismaCG3, IStatusReporter statusReporter)
        {
            InitializeComponent();
            _karismaCG3 = karismaCG3;
            _statusReporter = statusReporter;

            CBSConfig = ConfigService.GetConfigSection("CBSSettings");
            workingFolder = CBSConfig["WorkingFolder"];
            string databasePath = Path.Combine(workingFolder, CBSConfig["Access"]);

            _accessService = new AccessService(databasePath);
            _comboBoxLoader = new ComboBoxDataLoader(_accessService);
        }

        private void CBSControl1_Load(object sender, EventArgs e)
        {
            logoFolder = Path.Combine(workingFolder, CBSConfig["LogoFolder"]);
            LoadFilesToComboBox(logoFolder, cbbLogo, state.LogoMap);
            popupFolder = Path.Combine(workingFolder, CBSConfig["PopupFolder"]);
            LoadFilesToComboBox(popupFolder, cbbPopup, state.PopupMap);

            QueryGeneral();
            _comboBoxLoader.LoadPhongVanBang(cbbPVB, state.PhongVanBangMap);
            _comboBoxLoader.LoadPhongVan3Nguoi(cbbPV3, state.PhongVan3NguoiMap);
            _statusReporter.UpdateStatusLabel("Dữ liệu đã tải xong.");

        }

        private void LoadFilesToComboBox(string folderPath, ComboBox comboBox, Dictionary<string, string> map)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    MessageBox.Show($"Không tìm thấy thư mục: {folderPath}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var allFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly).ToList();
                comboBox.Items.Clear();
                map.Clear();
                foreach (var filePath in allFiles)
                {
                    string fileName = Path.GetFileName(filePath);
                    map[fileName] = filePath;
                    comboBox.Items.Add(fileName);
                }
            }
            catch (Exception ex)
            {
                _statusReporter.UpdateStatusLabel($"Error loading files: {ex.Message}");
            }
        }


        #region Functions similar to frmMain

        private void QueryGeneral() //Query Workingfolder và Layers
        {
            try
            {
                if (CBSConfig == null)
                {
                    _statusReporter.UpdateStatusLabel("Không tìm thấy cấu hình CBSSettings trong file config.cfg!");
                    return;
                }

                workingFolder = state.WorkingFolder = CBSConfig["WorkingFolder"];

                foreach (var layerName in state.LayerNames)
                {
                    int? layerValue = _accessService.QueryIntColumnValue("General", "Name", "Layer", layerName);
                    state.layers[layerName] = layerValue ?? 0;
                }
            }
            catch (Exception ex)
            {
                _statusReporter.UpdateStatusLabel($"Error querying data: {ex.Message}");
            }
        }

        private void cbbPV3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName1 = cbbPV3.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedName1) && state.PhongVan3NguoiMap.TryGetValue(selectedName1, out var values))
            {
                txtName13.Text = selectedName1;
                txtDes13.Text = values.Description1;
                txtName23.Text = values.Name2;
                txtDes23.Text = values.Description2;
                txtName33.Text = values.Name3;
                txtDes33.Text = values.Description3;
            }
            else
            {
                txtName13.Clear();
                txtDes13.Clear();
                txtName23.Clear();
                txtDes23.Clear();
                txtName33.Clear();
                txtDes33.Clear();
            }
        }

        private void btnShowLocation_Click(object sender, EventArgs e)
        {

            string scene = _accessService.QueryColumnValue("General", "Name", "Scene", "Location");
            string name = cbbLocation.Text;
            _karismaCG3.PlaySceneName(workingFolder + scene, state.layers["Location"], name);
            _statusReporter.UpdateStatusLabel($"Đang phát: {scene} (Lớp: {state.layers["Location"]}, Tên: {name}");
        }

        private void btnGetDataInews_Click(object sender, EventArgs e)
        {
            API api = new API(CBSConfig);

            api.FormClosed += (s, args) =>
            {
                GetDataFromTXT();
            };

            api.ShowDialog();
        }

        private void GetDataFromTXT()
        {
            fileRuttit = Path.Combine(workingFolder, CBSConfig["Ruttit"]);
            fileDiadanh = Path.Combine(workingFolder, CBSConfig["Diadanh"]);
            fileUTC = Path.Combine(workingFolder, CBSConfig["UTC"]);
            filePhude = Path.Combine(workingFolder, CBSConfig["Phude"]);

            if (string.IsNullOrEmpty(fileRuttit) || !File.Exists(fileRuttit))
            {
                _statusReporter.UpdateStatusLabel("Không tìm thấy file TXT trong cấu hình hoặc file không tồn tại!");
                return;
            }

            LoadTxtToDgvLT(fileRuttit);
            LoadComboBox(fileDiadanh, cbbLocation);
            LoadComboBox(fileUTC, cbbUTC);
            _statusReporter.UpdateStatusLabel("Đã tải dữ liệu từ file ruttit, diadanh, UTC.");
        }


        private void cbbPVB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName1 = cbbPVB.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedName1) && state.PhongVanBangMap.TryGetValue(selectedName1, out var values))
            {
                txtName1.Text = selectedName1;
                txtDescription1.Text = values.Description1;
                txtName2.Text = values.Name2;
                txtDescription2.Text = values.Description2;
            }
            else
            {
                txtName1.Clear();
                txtDescription1.Clear();
                txtName2.Clear();
                txtDescription2.Clear();
            }
        }

        private void dgvLT_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            XmlStoryService.HandleLTSelection(
                e.RowIndex,
                dgvLT,
                dgvName,
                txtLine1,
                txtLine2,
                txtLine1Name,
                txtLine2Name,
                state.StoryCgData,
                _statusReporter.UpdateStatusLabel
            );
        }

        private void dgvName_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            XmlStoryService.LoadTextBoxFromDgv(dgvName, txtLine1Name, txtLine2Name);
        }
        #endregion

        private void btnDeleteLocation_Click(object sender, EventArgs e)
        {
            _karismaCG3.PlayOut(state.layers["Location"]);

            _statusReporter.UpdateStatusLabel($"Đã dừng lớp: {state.layers["Location"]}");
        }

        private void btnTimeUTC_Click(object sender, EventArgs e)
        {
            string time = GetConvertedTimeWithCity();

            if (btnTimeUTC.Text == "Hiện giờ UTC")
            {
                string Scene = _accessService.QueryColumnValue("General", "Name", "Scene", "TimeUTC");

                _karismaCG3.PlaySceneUTC(workingFolder + Scene, state.layers["TimeUTC"], time);
                btnTimeUTC.Text = "Xóa giờ UTC";

                _statusReporter.UpdateStatusLabel($"Đang phát: {Scene} (Thời gian: {time})");
            }
            else if (btnTimeUTC.Text == "Xóa giờ UTC")
            {
                _karismaCG3.PlayOut(state.layers["TimeUTC"]);
                btnTimeUTC.Text = "Hiện giờ UTC";
                _statusReporter.UpdateStatusLabel($"Đã dừng lớp: {state.layers["TimeUTC"]}");

            }
        }
        private string GetConvertedTimeWithCity()
        {
            var localTime = DateTime.Now;

            string cbbText = cbbUTC.SelectedItem?.ToString() ?? cbbUTC.Text;
            if (string.IsNullOrEmpty(cbbText)) return "No time zone";

            int start = cbbText.IndexOf('('), end = cbbText.IndexOf(')');
            if (start == -1 || end == -1) return "Invalid format";
            string offsetStr = cbbText.Substring(start + 1, end - start - 1);
            string city = cbbText.Substring(0, start).Trim();

            offsetStr = offsetStr.Replace("+", "").Replace("-", "");
            if (!offsetStr.Contains(":")) offsetStr += ":00";
            if (!TimeSpan.TryParse(offsetStr, out var offset)) return "Invalid offset";
            if (cbbText.Contains("-")) offset = -offset;

            var convertedTime = localTime + (offset - TimeSpan.FromHours(7));

            return $"{convertedTime:HH:mm}, {city}"; //Format "HH:mm, City"
        }

        private void btnShowPopup_Click(object sender, EventArgs e)
        {
            if (cbbPopup.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một mục trong danh sách Popup!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string Scene = _accessService.QueryColumnValue("General", "Name", "Scene", "Popup");
            if (string.IsNullOrEmpty(filePopup))
            {
                MessageBox.Show("Không tìm thấy file Popup!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _karismaCG3.PlaySceneName(workingFolder + Scene, state.layers["Popup"], filePopup);
            _statusReporter.UpdateStatusLabel($"Đang phát: {Scene} (Lớp: {state.layers["Popup"]}, File: {filePopup})");
        }

        private void btnDeletePopup_Click(object sender, EventArgs e)
        {
            _karismaCG3.PlayOut(state.layers["Popup"]);
            _statusReporter.UpdateStatusLabel($"Đã dừng lớp: {state.layers["Popup"]}");
        }

        private void btnTroiNgang_Click(object sender, EventArgs e)
        {
            CBSConfig = ConfigService.GetConfigSection("CBSSettings");
            string speedStr = CBSConfig["speedTroiCuoi"];
            int.TryParse(speedStr, out int speed);

            if (!state.layers.TryGetValue("TroiTin", out int layer))
            {
                MessageBox.Show("Không tìm thấy layer cho TroiNgang!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string scenePath = workingFolder + _accessService.QueryColumnValue("General", "Name", "Scene", "TroiTin");

            _frmTroiNgang = new frmTroiNgang(_karismaCG3, layer, speed, scenePath, "CBSSettings");

            Form mainForm = this.FindForm();
            var mainFormBounds = mainForm.Bounds;

            _frmTroiNgang.StartPosition = FormStartPosition.Manual;
            _frmTroiNgang.Location = new Point(mainFormBounds.Right, mainFormBounds.Top + (mainFormBounds.Height - _frmTroiNgang.Height) / 2);

            _frmTroiNgang.Show();
        }

        private void btnChuKet_Click(object sender, EventArgs e)
        {
            string Scene = _accessService.QueryColumnValue("General", "Name", "Scene", "ChuKet");
            _karismaCG3.PlayScene(workingFolder + Scene, state.layers["ChuKet"]);
        }

        private void btnChuKetKhac_Click(object sender, EventArgs e)
        {
            string Scene = _accessService.QueryColumnValue("General", "Name", "Scene", "ChuKet");
            _karismaCG3.PlayScene(workingFolder + Scene, state.layers["ChuKet"]);
        }

        private void btnXoaChuKet_Click(object sender, EventArgs e)
        {
            _karismaCG3.Stop(state.layers["ChuKet"]);

        }

        private void btnShowLogo_Click(object sender, EventArgs e)
        {
            if (btnShowLogo.Text == "Hiện Logo")
            {
                ShowLogo();

                btnShowLogo.Text = "Xóa Logo";
                _statusReporter.UpdateStatusLabel($"Đang phát lớp: {state.layers["Logo"]})");
            }
            else if (btnShowLogo.Text == "Xóa Logo")
            {
                _karismaCG3.PlayOut(state.layers["Logo"]);
                btnShowLogo.Text = "Hiện Logo";
                _statusReporter.UpdateStatusLabel($"Đã dừng lớp: {state.layers["Logo"]}");
            }
        }
        private void ShowLogo()
        {
            if (cbbLogo.SelectedItem == null)
            {
                return;
            }

            string Scene = _accessService.QueryColumnValue("General", "Name", "Scene", "Logo");

            _karismaCG3.PlaySceneName(workingFolder + Scene, state.layers["Logo"], fileLogo);
        }

        private void btnLive_Click(object sender, EventArgs e)
        {
            if (btnLive.Text == "Trực tiếp")
            {
                string Scene = _accessService.QueryColumnValue("General", "Name", "Scene", "Live");
                _karismaCG3.PlayScene(workingFolder + Scene, state.layers["Live"]);
                btnLive.Text = "Xóa trực tiếp";
                _statusReporter.UpdateStatusLabel($"Đang phát: {Scene} (Lớp: {state.layers["Live"]})");
            }
            else if (btnLive.Text == "Xóa trực tiếp")
            {
                _karismaCG3.Stop(state.layers["Live"]);
                btnLive.Text = "Trực tiếp";
                _statusReporter.UpdateStatusLabel($"Đã dừng lớp: {state.layers["Live"]}");
            }
        }

        private void btnClock_Click(object sender, EventArgs e)
        {
            if (btnClock.Text == "Đồng hồ")
            {
                string Scene = _accessService.QueryColumnValue("General", "Name", "Scene", "Clock");
                _karismaCG3.PlayScene(workingFolder + Scene, state.layers["Clock"]);
                btnClock.Text = "Xóa đồng hồ";
                _statusReporter.UpdateStatusLabel($"Đang phát: {Scene} (Lớp: {state.layers["Clock"]})");
            }
            else if (btnClock.Text == "Xóa đồng hồ")
            {
                _karismaCG3.Stop(state.layers["Clock"]);
                btnClock.Text = "Đồng hồ";
                _statusReporter.UpdateStatusLabel($"Đã dừng lớp: {state.layers["Clock"]}");
            }
        }

        private void bthShowLT_Click(object sender, EventArgs e)
        {

            PlayOrTriggerLowerThird(txtLine1, txtLine2);

            if (dgvLT != null && dgvLT.CurrentRow != null)
            {
                dgvLT.CurrentRow.DefaultCellStyle.BackColor = Color.Gray;
            }
        }

        private void btnXoaLT_Click(object sender, EventArgs e)
        {
            PlayOutLowerThird();
        }

        private void btnGhiLT_Click(object sender, EventArgs e)
        {
            if (dgvLT.CurrentRow != null)
            {
                dgvLT.CurrentRow.Cells["line1"].Value = txtLine1.Text.Trim();
                dgvLT.CurrentRow.Cells["line2"].Value = txtLine2.Text.Trim();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng trong danh sách!");
            }
        }

        private void beforeLT_Click(object sender, EventArgs e)
        {
            if (dgvLT.CurrentRow != null && dgvLT.CurrentRow.Index > 0)
            {
                int currentIndex = dgvLT.CurrentRow.Index - 1;
                dgvLT.Rows[currentIndex].Selected = true;
                dgvLT.CurrentCell = dgvLT.Rows[currentIndex].Cells[0];
                dgvLT_CellClick(this, new DataGridViewCellEventArgs(0, currentIndex));
                dgvLT_SelectionChanged(this, new DataGridViewCellEventArgs(0, currentIndex));
                XmlStoryService.LoadTxtLineForDgvName(dgvName, txtLine1Name, txtLine2Name);
            }
        }

        private void nextLT_Click(object sender, EventArgs e)
        {
            if (dgvLT.CurrentRow != null && dgvLT.CurrentRow.Index < dgvLT.Rows.Count - 1)
            {
                int currentIndex = dgvLT.CurrentRow.Index + 1;
                dgvLT.Rows[currentIndex].Selected = true;
                dgvLT.CurrentCell = dgvLT.Rows[currentIndex].Cells[0];
                dgvLT_CellClick(this, new DataGridViewCellEventArgs(0, currentIndex));
                dgvLT_SelectionChanged(this, new DataGridViewCellEventArgs(0, currentIndex));
                XmlStoryService.LoadTxtLineForDgvName(dgvName, txtLine1Name, txtLine2Name);
            }
        }

        private void btnTroiTinTuc_Click(object sender, EventArgs e)
        {
            CBSConfig = ConfigService.GetConfigSection("CBSSettings");
            string speedStr = CBSConfig["speedTroiTin"];
            int.TryParse(speedStr, out int speed);

            if (!state.layers.TryGetValue("TroiTin", out int layer))
            {
                MessageBox.Show("Không tìm thấy layer cho TroiTinTuc!");
                return;
            }

            string scenePath = workingFolder + _accessService.QueryColumnValue("General", "Name", "Scene", "TroiTin");

            _frmTroiTinTuc = new frmTroiTinTuc(_karismaCG3, layer, speed, scenePath, "CBSSettings");

            Form mainForm = this.FindForm();
            var mainFormBounds = mainForm.Bounds;

            _frmTroiTinTuc.StartPosition = FormStartPosition.Manual;
            _frmTroiTinTuc.Location = new Point(mainFormBounds.Right, mainFormBounds.Top + (mainFormBounds.Height - _frmTroiTinTuc.Height) / 2);

            _frmTroiTinTuc.Show();
        }

        private void btnXoaTroiTin_Click(object sender, EventArgs e)
        {
            if (_frmTroiTinTuc != null && !_frmTroiTinTuc.IsDisposed)
            {
                _frmTroiTinTuc.XoaTroiTin();
            }
            if (_frmTroiNgang != null && !_frmTroiNgang.IsDisposed)
            {
                _frmTroiNgang.XoaTroiTin();
            }
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            CBSConfig = ConfigService.GetConfigSection("CBSSettings");
            GetDataFromTXT();

        }

        private void btnPhongvanbang_Click(object sender, EventArgs e)
        {
            if (btnPhongvanbang.Text == "Phỏng vấn")
            {
                try
                {
                    TextBox tb1 = new TextBox();
                    tb1.Text = txtName1.Text + txtName2.Text;
                    TextBox tb2 = new TextBox();
                    tb2.Text = txtDescription1.Text + txtDescription2.Text;
                    PlayOrTriggerLowerThird(tb1, tb2);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi phát scene: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                btnPhongvanbang.Text = "Xóa phỏng vấn";
            }
            else if (btnPhongvanbang.Text == "Xóa phỏng vấn")
            {
                PlayOutLowerThird();
                btnPhongvanbang.Text = "Phỏng vấn";
            }
        }

        private void btnPhongvanlech_Click(object sender, EventArgs e)
        {
            if (btnPhongvanlech.Text == "Phỏng vấn")
            {
                try
                {
                    string name1 = txtName1L.Text.Trim();
                    string description1 = txtDes1L.Text.Trim();
                    string description2 = txtDes2L.Text.Trim();

                    if (string.IsNullOrEmpty(name1))
                    {
                        MessageBox.Show("Vui lòng nhập thông tin Name1!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string scenePath = _accessService.QueryColumnValue("General", "Name", "Scene", "PhongVanLech");
                    if (string.IsNullOrEmpty(scenePath))
                    {
                        MessageBox.Show("Không tìm thấy đường dẫn scene cho PhongVanLech!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!state.layers.TryGetValue("PhongVanLech", out int layer))
                    {
                        MessageBox.Show("Không tìm thấy layer cho PhongVanLech!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _karismaCG3.PlayPV2(workingFolder + scenePath, layer, name1, description1, description2, "");
                    _statusReporter.UpdateStatusLabel($"Đang phát scene: {scenePath} Layer: {layer}");
                    btnPhongvanlech.Text = "Xóa phỏng vấn";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi phát scene: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (btnPhongvanlech.Text == "Xóa phỏng vấn")
            {
                string name1 = txtName1L.Text.Trim();
                string description1 = txtDes1L.Text.Trim();
                string description2 = txtDes2L.Text.Trim();

                if (string.IsNullOrEmpty(name1))
                {
                    MessageBox.Show("Vui lòng nhập thông tin Name1!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string scene = _accessService.QueryColumnValue("General", "Name", "SceneOut", "PhongVanLech");
                _karismaCG3.PlayPV2(workingFolder + scene, state.layers["PhongVanLech"], name1, description1, description2, "");

                _statusReporter.UpdateStatusLabel($"Stopped Layer: {state.layers["PhongVanLech"]}");
                btnPhongvanlech.Text = "Phỏng vấn";
            }
        }

        private void btnPhongvan3_Click(object sender, EventArgs e)
        {
            if (btnPhongvan3.Text == "Phỏng vấn")
            {
                try
                {
                    TextBox tb1 = new TextBox();
                    tb1.Text = txtName13.Text + txtName23.Text + txtName33.Text;
                    TextBox tb2 = new TextBox();
                    tb2.Text = txtDes13.Text + txtDes23.Text + txtDes33.Text;
                    PlayOrTriggerLowerThird(tb1, tb2);
                    btnPhongvan3.Text = "Xóa phỏng vấn";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi phát scene: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (btnPhongvan3.Text == "Xóa phỏng vấn")
            {
                PlayOutLowerThird();
                btnPhongvan3.Text = "Phỏng vấn";
            }
        }

        private void btnGhiName_Click(object sender, EventArgs e)
        {
            if (dgvName.CurrentRow != null)
            {
                dgvName.CurrentRow.Cells["line1name"].Value = txtLine1Name.Text.Trim();
                dgvName.CurrentRow.Cells["line2name"].Value = txtLine2Name.Text.Trim();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng trong danh sách!");
            }
        }

        private void btnXoaChu_Click(object sender, EventArgs e)
        {
            txtLine1Name.Clear();
            txtLine2Name.Clear();
        }

        private void beforeName_Click(object sender, EventArgs e)
        {
            if (dgvName.CurrentRow != null && dgvName.CurrentRow.Index > 0)
            {
                int currentIndex = dgvName.CurrentRow.Index;
                dgvName.Rows[currentIndex - 1].Selected = true;
                dgvName.CurrentCell = dgvName.Rows[currentIndex - 1].Cells[0];
                dgvName_CellClick(this, new DataGridViewCellEventArgs(0, currentIndex - 1));
            }
        }

        private void tbnShowName_Click(object sender, EventArgs e)
        {
            PlayOrTriggerLowerThird(txtLine1Name, txtLine2Name);
            if (dgvName != null && dgvName.CurrentRow != null)
            {
                dgvName.CurrentRow.DefaultCellStyle.BackColor = Color.Gray;
            }
        }

        private void nextName_Click(object sender, EventArgs e)
        {
            if (dgvName.CurrentRow != null && dgvName.CurrentRow.Index < dgvName.Rows.Count - 1)
            {
                int currentIndex = dgvName.CurrentRow.Index;
                dgvName.Rows[currentIndex + 1].Selected = true;
                dgvName.CurrentCell = dgvName.Rows[currentIndex + 1].Cells[0];
                dgvName_CellClick(this, new DataGridViewCellEventArgs(0, currentIndex + 1));
            }
        }

        private void btnXoaName_Click(object sender, EventArgs e)
        {
            PlayOutLowerThird();
        }

        private void btnStopAll_Click(object sender, EventArgs e)
        {
            isPlayLT = false;
            useTriggerLT = true;

            _karismaCG3.StopAll();
            _statusReporter.UpdateStatusLabel("Đã dừng tất cả các lớp.");
        }

        private void btnLenTuDong_Click(object sender, EventArgs e)
        {
        }

        private void btnXoaChuBL_Click(object sender, EventArgs e)
        {
            txtLine1BL.Clear();
            txtLine2BL.Clear();
        }

        private void btnHienBL_Click(object sender, EventArgs e)
        {
            PlayOrTriggerLowerThird(txtLine1BL, txtLine2BL);
        }

        private void btnXoaBL_Click(object sender, EventArgs e)
        {
            PlayOutLowerThird();
        }

        private void LoadComboBox(string Path, ComboBox cbb)
        {
            if (!File.Exists(Path)) return;

            cbb.Items.Clear();
            foreach (var line in File.ReadLines(Path))
            {
                var location = line.Trim();
                if (!string.IsNullOrEmpty(location))
                    cbb.Items.Add(location);
            }

            if (cbb.Items.Count > 0)
                cbb.SelectedIndex = 0;
        }

        private List<List<Tuple<string, string>>> nameBlocks = new List<List<Tuple<string, string>>>();


        private void LoadTxtToDgvLT(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Không tìm thấy file!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvLT.Rows.Clear();
            nameBlocks.Clear();

            var lines = File.ReadAllLines(filePath);
            string pendingHeader = null;
            List<Tuple<string, string>> currentNameBlock = null;
            bool isFirstPairInBlock = true;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Nếu là block mới
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    // Nếu có block cũ đang xử lý thì lưu lại nameBlock
                    if (currentNameBlock != null)
                        nameBlocks.Add(currentNameBlock);

                    pendingHeader = line.Trim('[', ']');
                    isFirstPairInBlock = true;
                    currentNameBlock = new List<Tuple<string, string>>();
                    continue;
                }

                if (line == "#")
                    continue;

                // Lấy dòng tiếp theo nếu có
                string nextLine = (i + 1 < lines.Length) ? lines[i + 1].Trim() : "";
                bool hasValidSecondLine = (!string.IsNullOrWhiteSpace(nextLine) && !nextLine.StartsWith("[") && nextLine != "#");

                string line1 = line;
                string line2 = hasValidSecondLine ? nextLine : "";

                if (isFirstPairInBlock)
                {
                    int rowIndex = dgvLT.Rows.Add(line1, line2);
                    dgvLT.Rows[rowIndex].HeaderCell.Value = pendingHeader ?? "";
                    isFirstPairInBlock = false;
                }
                else
                {
                    currentNameBlock.Add(new Tuple<string, string>(line1, line2));
                }

                if (hasValidSecondLine)
                    i++; // bỏ qua dòng đã dùng
            }

            // Thêm block cuối cùng nếu có
            if (currentNameBlock != null)
                nameBlocks.Add(currentNameBlock);

            DisableColumnSorting(dgvLT);

            // --- FIX: Tự động chọn dòng đầu tiên và cập nhật dgvName ---
            if (dgvLT.Rows.Count > 0)
            {
                dgvLT.Rows[0].Selected = true;
                dgvLT.CurrentCell = dgvLT.Rows[0].Cells[0];
                dgvLT_SelectionChanged(dgvLT, EventArgs.Empty); // Cập nhật dgvName
                dgvLT_CellClick(dgvLT, new DataGridViewCellEventArgs(0, 0)); // Cập nhật txtLine1, txtLine2
            }
        }

        private void dgvLT_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLT.CurrentRow == null)
                return;

            int index = dgvLT.CurrentRow.Index;
            if (index < 0 || index >= nameBlocks.Count)
                return;

            dgvName.Rows.Clear(); // Đảm bảo dgvName đã có 2 cột: line1name, line2name

            foreach (var pair in nameBlocks[index])
            {
                dgvName.Rows.Add(pair.Item1, pair.Item2);
            }
        }


        private void dgvLT_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            XmlStoryService.HandleLTSelection(
            e.RowIndex,
            dgvLT,
            dgvName,
            txtLine1,
            txtLine2,
            txtLine1Name,
            txtLine2Name,
            state.StoryCgData,
            _statusReporter.UpdateStatusLabel);
        }

        private void btnFrmUTC_Click(object sender, EventArgs e)
        {
            frmUTC frmUTC = new frmUTC();
            CBSConfig = ConfigService.GetConfigSection("CBSSettings");
            string fileUTC = Path.Combine(workingFolder, CBSConfig["UTC"]);
            frmUTC.fileUTC = fileUTC;

            frmUTC.FormClosed += (s, args) =>
            {
                // Reload UTC file into cbbUTC after frmUTC is closed
                if (!string.IsNullOrEmpty(fileUTC))
                {
                    LoadComboBox(fileUTC, cbbUTC);
                }
            };
            frmUTC.ShowDialog();
            frmUTC.BringToFront();
        }

        private void btnHienPV_Click(object sender, EventArgs e)
        {
            PlayOrTriggerLowerThird(txtLine1Name, txtLine2Name);
            if (dgvName != null && dgvName.CurrentRow != null)
            {
                dgvName.CurrentRow.DefaultCellStyle.BackColor = Color.Gray;
            }
        }
        private void PlayOrTriggerLowerThird(TextBox textBoxLine1, TextBox textBoxLine2)
        {
            string LTScene = _accessService.QueryColumnValue("General", "Name", "Scene", "LowerThird");
            string TextScene = _accessService.QueryColumnValue("General", "Name", "Scene", "Text");
            string Transition = _accessService.QueryColumnValue("General", "Name", "Transition", "Text");
            int LTLayer = state.layers["LowerThird"];
            int TextLayer = state.layers["Text"];

            if (!isPlayLT) // Chưa phát LT, thực hiện phát LT
            {
                Line1 = textBoxLine1.Text.Trim();
                Line2 = textBoxLine2.Text.Trim();
                if (string.IsNullOrEmpty(Line1))
                {
                    MessageBox.Show("Vui lòng nhập nội dung cho Line1!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    //checkPlayNgoisao("Scene");
                    //checkPlayRadioTinChinh("Scene");
                    CheckBarInOut("BarIn", LTScene, LTLayer);
                    //_karismaCG3.PlayScene(workingFolder + LTScene, LTLayer);
                    isPlayLT = _karismaCG3.PlaySceneLT(workingFolder + TextScene, TextLayer, Line1, Line2);
                    ShowLogo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                string Line1new = textBoxLine1.Text.Trim();
                string Line2new = textBoxLine2.Text.Trim();

                try
                {
                    _karismaCG3.PlayTextTransition(workingFolder + Transition, TextLayer, Line1, Line2, Line1new, Line2new);

                    Line1 = Line1new;
                    Line2 = Line2new;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void PlayOutLowerThird()
        {
            string LTScene = _accessService.QueryColumnValue("General", "Name", "SceneOut", "LowerThird");
            string TextScene = _accessService.QueryColumnValue("General", "Name", "SceneOut", "Text");
            int LTLayer = state.layers["LowerThird"];
            int TextLayer = state.layers["Text"];

            if (!isPlayLT) { return; }

            isPlayLT = false;

            //checkPlayNgoisao("SceneOut");
            //checkPlayRadioTinChinh("SceneOut");
            _karismaCG3.PlaySceneLT(workingFolder + TextScene, TextLayer, Line1, Line2);
            //_karismaCG3.PlayScene(workingFolder + LTScene, LTLayer);
            CheckBarInOut("BarOut", LTScene, LTLayer);

            if (!checkGiuLogo.Checked)
            {
                _karismaCG3.PlayOut(state.layers["Logo"]);
            }

            _statusReporter.UpdateStatusLabel($"Stopped Layer: {LTLayer}, {TextLayer}");
        }

        private void checkPlayRadioTinChinh(string SceneName)
        {
            string Scene = _accessService.QueryColumnValue("General", "Name", SceneName, "TinChinh");
            if (!state.layers.TryGetValue("TinChinh", out int layer))
            {
                return;
            }

            if (radioChinh.Checked)
            {
                _karismaCG3.PlaySceneName(workingFolder + Scene, layer, "TIN CHÍNH");
            }
            if (radioBreakNews.Checked)
            {
                _karismaCG3.PlaySceneName(workingFolder + Scene, layer, "TIN MỚI NHẤT");
            }
        }
        //private void checkPlayNgoisao(string sceneName)
        //{
        //    if (!checkNgoisao.Checked)
        //        return;

        //    if (!state.layers.TryGetValue("Ngoisao", out int layer))
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        string scene = _accessService.QueryColumnValue("General", "Name", "Scene", "Ngoisao");
        //        if (string.IsNullOrEmpty(scene))
        //        {
        //            return;
        //        }

        //        if (sceneName == "Scene")
        //        {
        //            _karismaCG3.PlayScene(workingFolder + scene, layer);
        //            _statusReporter?.UpdateStatusLabel($"Playing Ngoisao scene on layer {layer}");
        //        }
        //        else if (sceneName == "SceneOut")
        //        {
        //            _karismaCG3.PlayOut(layer);
        //            _statusReporter?.UpdateStatusLabel($"Stopping Ngoisao on layer {layer}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _statusReporter?.UpdateStatusLabel($"Error in Ngoisao handling: {ex.Message}");
        //    }
        //}

        private void btnFormBL_Click(object sender, EventArgs e)
        {
            frmBinhLuan frmBinhLuan = new frmBinhLuan();
            frmBinhLuan.FormClosed += (s, args) =>
            {
                txtFilePath.Text = frmBinhLuan.filePath;
                string Content = frmBinhLuan.fileContent;
                ParseTextToDgv(Content, dgvBinhLuan);
            };
            frmBinhLuan.ShowDialog();
            frmBinhLuan.BringToFront();
        }
        private void ParseTextToDgv(string fileContent, DataGridView dgv)
        {
            dgv.Rows.Clear();

            string[] lines = fileContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            List<string> current = new List<string>();

            foreach (string raw in lines)
            {
                string line = raw.Trim();

                if (string.IsNullOrWhiteSpace(line))
                {
                    if (current.Count > 0)
                    {
                        string line1 = current.Count > 0 ? current[0] : "";
                        string line2 = current.Count > 1 ? current[1] : "";
                        dgv.Rows.Add(line1, line2);
                        current.Clear();
                    }
                }
                else
                {
                    current.Add(line);
                }
            }

            // Nếu còn dòng cuối cùng chưa xử lý
            if (current.Count > 0)
            {
                string line1 = current.Count > 0 ? current[0] : "";
                string line2 = current.Count > 1 ? current[1] : "";
                dgv.Rows.Add(line1, line2);
            }
        }

        private void dgvBinhLuan_SelectionChanged(object sender, EventArgs e)
        {
            XmlStoryService.LoadTextBoxFromDgv(dgvBinhLuan, txtLine1BL, txtLine2BL);
        }

        private void cbbPopup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbPopup.SelectedItem != null)
            {
                string selectedFileName = cbbPopup.SelectedItem.ToString();
                if (string.IsNullOrEmpty(selectedFileName) || !state.PopupMap.TryGetValue(selectedFileName, out filePopup))
                {
                    filePopup = string.Empty;
                    _statusReporter.UpdateStatusLabel("Popup: (trống)");
                    return;
                }
                _statusReporter.UpdateStatusLabel("Popup: " + filePopup);
            }
        }

        private void btnHienPhuDe_Click(object sender, EventArgs e)
        {
            string Scene = _accessService.QueryColumnValue("General", "Name", "Scene", "PhuDe");
            if (!state.layers.TryGetValue("PhuDe", out int layer))
            {
                return;
            }
            _karismaCG3.PlayScene2(workingFolder + Scene, layer, txtPhuDe1.Text, txtPhuDe2.Text);
        }

        private void btnXoaPhuDe_Click(object sender, EventArgs e)
        {
            if (!state.layers.TryGetValue("PhuDe", out int layer))
            {
                return;
            }
            _karismaCG3.PlayOut(layer);
        }

        private void tabPhuDe_Click(object sender, EventArgs e)
        {
            try
            {
                CBSConfig = ConfigService.GetConfigSection("CBSSettings");
                string filePath = Path.Combine(workingFolder, CBSConfig["Phude"]);
                string content = File.ReadAllText(filePath, Encoding.UTF8);
                ParseTextToDgv(content, dgvPhuDe);
            }
            catch (Exception ex)
            {
                _statusReporter.UpdateStatusLabel($"Lỗi khi tải dữ liệu phụ đề: {ex.Message}");
            }
        }

        private void dgvPhuDe_SelectionChanged(object sender, EventArgs e)
        {
            XmlStoryService.LoadTextBoxFromDgv(dgvPhuDe, txtPhuDe1, txtPhuDe2);

        }

        private void cbbLogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbLogo.SelectedItem != null)
            {
                string selectedFileName = cbbLogo.SelectedItem.ToString();
                if (string.IsNullOrEmpty(selectedFileName) || !state.LogoMap.TryGetValue(selectedFileName, out fileLogo))
                {
                    fileLogo = string.Empty;
                    _statusReporter.UpdateStatusLabel("Logo: (trống)");
                    return;
                }
                if (state.LogoMap.TryGetValue(selectedFileName, out fileLogo))
                {
                    _statusReporter.UpdateStatusLabel("Logo: " + fileLogo);
                }
            }
        }
        private void CheckBarInOut(string key, string scene, int layer)
        {
            CBSConfig = ConfigService.GetConfigSection("CBSSettings");

            string bar = CBSConfig[key];

            if (string.IsNullOrWhiteSpace(bar))
            {
                _karismaCG3.PlayScene(workingFolder + scene, layer);
            }
            else
            {
                _karismaCG3.PlaySceneBar(workingFolder + scene, layer, bar);
            }
        }
        public void DisableColumnSorting(DataGridView dgv)
        {
            if (dgv == null || dgv.Columns.Count == 0)
                return;

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

    }
}
