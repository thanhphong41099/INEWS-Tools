using API_iNews;
using News2025.Menu;
using News2025.Models;
using News2025.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace News2025
{
    public partial class NewslineControl1 : UserControl
    {

        private readonly IAccessService _accessService;
        private readonly IKarismaCG3Model _karismaCG3;
        private readonly IStatusReporter _statusReporter;
        private readonly VariableState state = new VariableState();

        private frmTroiTinTuc _frmTroiTinTuc;
        private frmTroiNgang _frmTroiNgang;

        private bool isPlayLT = false;
        private LineTag currentTag = LineTag.None; // tag đang hiển thị (Title = One, hoặc Content = One/Two/Three)

        // Text đang hiển thị trên LT
        private string currentRaw = string.Empty;     // lưu nguyên string hiện có trên LT
        private string currentL1 = string.Empty;      // chỉ meaningful khi currentTag == Two
        private string currentL2 = string.Empty;

        private bool isTitleScenePlaying = false; // TitleScene (TitleLayer) đang phát chưa
        private string currentTitle = string.Empty; // Title "real" đã phát trên TitleLayer

        private NameValueCollection ControlConfig;
        string workingFolder, fileRuttit, fileDiadanh;
        string LTSceneIn, LTSceneOut;


        private VietnamTodaySettings _vtSettings;  //quản lý cấu hình VietnamTodaySettings cho Scene và Layer


        public NewslineControl1(IKarismaCG3Model karismaCG3, IStatusReporter statusReporter)
        {
            InitializeComponent();
            _karismaCG3 = karismaCG3;
            _statusReporter = statusReporter;

            Reload();
            LoadCbbBarOptions();
        }
        public void Reload()
        {
            ControlConfig = ConfigService.GetConfigSection("NewslineSettings");
            workingFolder = ControlConfig["WorkingFolder"];
            _vtSettings = VietnamTodaySettings.Load(ControlConfig);

            fileDiadanh = Path.Combine(workingFolder, ControlConfig["Diadanh"]);
            LoadComboBox(fileDiadanh, cbbLocation);

        }
        private void LoadCbbBarOptions()
        {
            cbbBar.Items.Clear();
            cbbBar.Items.Add("Banner");
            cbbBar.Items.Add("Banner Headline");
            cbbBar.SelectedIndex = 0;
        }

        private void VietnamTodayControl1_Load(object sender, EventArgs e)
        {
            _statusReporter.UpdateStatusLabel("Dữ liệu đã tải xong.");
            dgvContent.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvContent.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _karismaCG3.StopAll();
        }

        #region Functions similar to frmMain

        private void btnShowLocation_Click(object sender, EventArgs e)
        {
            var layer = _vtSettings.RequireLayer("DiaDanhLayer");
            var name = cbbLocation.Text;

            if (btnShowLocation.Text == "Hiện")
            {
                var scene = _vtSettings.RequireScene("DiaDanhScene");

                _karismaCG3.PlaySceneName(scene, layer, name);
                _statusReporter.UpdateStatusLabel($"Đang phát địa danh: {name}");
                btnShowLocation.Text = "Xóa"; // Change button text to "Xóa"
            }
            else if (btnShowLocation.Text == "Xóa")
            {
                var scene = _vtSettings.RequireScene("DiaDanhOutScene");

                _karismaCG3.PlaySceneName(scene, layer, name);
                btnShowLocation.Text = "Hiện"; // Change button text back to "Hiện"
                _statusReporter.UpdateStatusLabel($"Đã xóa địa danh: {name}");
            }
        }

        private void btnGetDataInews_Click(object sender, EventArgs e)
        {
            API api = new API(ControlConfig);

            api.FormClosed += (s, args) =>
            {
                GetDataFromTXT();
            };

            api.ShowDialog();
        }

        private void GetDataFromTXT()
        {
            fileRuttit = Path.Combine(workingFolder, ControlConfig["Ruttit"]);
            fileDiadanh = Path.Combine(workingFolder, ControlConfig["Diadanh"]);

            if (string.IsNullOrEmpty(fileRuttit) || !File.Exists(fileRuttit))
            {
                _statusReporter.UpdateStatusLabel("Không tìm thấy file TXT trong cấu hình hoặc file không tồn tại!");
                return;
            }

            LoadTxtToDgvLT(fileRuttit);
            LoadComboBox(fileDiadanh, cbbLocation);
            _statusReporter.UpdateStatusLabel("Đã tải dữ liệu từ file ruttit, diadanh, UTC.");
        }
        #endregion

        #region Tag & Helpers
        private enum LineTag { None = 0, One = 1, Two = 2, Three = 3 }

        private static int CountNonEmptyLines(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;
            var parts = s.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            return parts.Count(p => !string.IsNullOrWhiteSpace(p));
        }

        private static LineTag GetLineTag(string s)
        {
            int n = CountNonEmptyLines(s);
            if (n <= 0) return LineTag.None;
            if (n == 1) return LineTag.One;
            if (n == 2) return LineTag.Two;
            return LineTag.Three;
        }

        // Tách 2 dòng đầu tiên (dùng cho 2Line)
        private static (string l1, string l2) SplitTwoLines(string s)
        {
            var parts = (s ?? string.Empty)
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToArray();

            string l1 = parts.Length > 0 ? parts[0] : string.Empty;
            string l2 = parts.Length > 1 ? parts[1] : string.Empty;
            return (l1, l2);
        }

        #endregion


        private void btnLive_Click(object sender, EventArgs e)
        {
            var scene = _vtSettings.RequireScene("LiveScene");
            var layer = _vtSettings.RequireLayer("DiaDanhLayer");

            _karismaCG3.PlayScene(scene, layer);
            _statusReporter.UpdateStatusLabel($"Đang phát Live");
        }

        private void bthShowLT_Click(object sender, EventArgs e)
        {
            HideTitleSceneIfPlaying();

            string text = txtTitle.Text?.Trim() ?? string.Empty; // dùng txtTitle làm nguồn text LT
            if (!isPlayLT) LT_ShowFirst(text);
            else
                LT_TransitionTo(text);

            // Đổi màu dòng đang chọn trong dgvLT nếu có
            if (dgvLT?.CurrentRow != null) dgvLT.CurrentRow.DefaultCellStyle.BackColor = Color.Gray;
        }


        #region CRUD call KarismaCG3

        // Phát TitleScene (không phải LT Scene) với tiêu đề hiện tại
        private void Play_TitleScene(string title)
        {
            var TitleLayer = _vtSettings.RequireLayer("TitleLayer");
            var TitleScene = _vtSettings.RequireScene("TitleScene");
            _karismaCG3.PlaySceneName(TitleScene, TitleLayer, title);
        }

        // Ẩn TitleScene
        private void Play_TitleScene_Out()
        {
            var TitleLayer = _vtSettings.RequireLayer("TitleLayer");
            _karismaCG3.PlayOut(TitleLayer);
        }

        //Phát LT Scene (khung LT để chứa Text Title, Content1, Content2, Content)
        private void Play_LT_Scene()
        {
            var LTlayer = _vtSettings.RequireLayer("BannerLayer");
            _karismaCG3.PlayScene(LTSceneIn, LTlayer);
            isPlayLT = true;
        }
        // Ẩn LT Scene
        private void Play_LT_Scene_Out()
        {
            var LTlayer = _vtSettings.RequireLayer("BannerLayer");
            _karismaCG3.PlayScene(LTSceneOut, LTlayer);
            isPlayLT = false;
        }

        // Hàm phát Title In cho LT (phát lần đầu)
        private void Play_Title_In(string text)
        {
            var TextLayer = _vtSettings.RequireLayer("TextLayer");
            var TextScene = _vtSettings.RequireScene("TextInScene");
            _karismaCG3.PlayOneLineIn(TextScene, TextLayer, text);
        }

        // Hàm phát 2 Line In cho LT (phát lần đầu)
        private void Play_2Line_In(string line1, string line2)
        {
            var TextLayer = _vtSettings.RequireLayer("TextLayer");
            var TextScene = _vtSettings.RequireScene("TextInScene");
            _karismaCG3.PlayTwoLineIn(TextScene, TextLayer, line1, line2);
        }

        // Hàm phát 3 Line In cho LT (phát lần đầu)
        private void Play_3Line_In(string text)
        {
            var TextLayer = _vtSettings.RequireLayer("TextLayer");
            var TextScene = _vtSettings.RequireScene("TextInScene");
            _karismaCG3.PlayContentIn(TextScene, TextLayer, text);
        }

        //Hàm chuyển Text từ Title sang Title (khi đang phát)
        private void Play_TitleToTitle(string oldText, string newText)
        {
            var TextLayer = _vtSettings.RequireLayer("TextLayer");
            var TextScene = _vtSettings.RequireScene("TextTransScene");
            _karismaCG3.PlayTextTitleTrans(TextScene, TextLayer, oldText, newText);
        }
        //Hàm chuyển Text từ Title sang 2Line (khi đang phát)
        private void Play_Title_to_2Line(string oldText, string newLine1, string newLine2)
        {
            var TextLayer = _vtSettings.RequireLayer("TextLayer");
            var TextScene = _vtSettings.RequireScene("TextTransScene");
            _karismaCG3.PlayTextTitleTo2Line(TextScene, TextLayer, oldText, newLine1, newLine2);
        }
        //Hàm chuyển Text từ Title sang 3Line (khi đang phát)
        private void Play_Title_to_3Line(string oldText, string newText)
        {
            var TextLayer = _vtSettings.RequireLayer("TextLayer");
            var TextScene = _vtSettings.RequireScene("TextTransScene");
            _karismaCG3.PlayTextTitleTo3Line(TextScene, TextLayer, oldText, newText);
        }
        //Hàm chuyển 2Line sang Title (khi đang phát)
        private void Play_2Line_to_Title(string oldLine1, string oldLine2, string newText)
        {
            var TextLayer = _vtSettings.RequireLayer("TextLayer");
            var TextScene = _vtSettings.RequireScene("TextTransScene");
            _karismaCG3.PlayText2LineToTitle(TextScene, TextLayer, oldLine1, oldLine2, newText);
        }
        //Hàm chuyển 2Line sang 2Line (khi đang phát)
        private void Play_2Line_to_2Line(string oldLine1, string oldLine2, string newLine1, string newLine2)
        {
            var TextLayer = _vtSettings.RequireLayer("TextLayer");
            var TextScene = _vtSettings.RequireScene("TextTransScene");
            _karismaCG3.PlayText2LineTrans(TextScene, TextLayer, oldLine1, oldLine2, newLine1, newLine2);
        }
        //Hàm chuyển 2Line sang 3Line (khi đang phát)
        private void Play_2Line_to_3Line(string oldLine1, string oldLine2, string newText)
        {
            var TextLayer = _vtSettings.RequireLayer("TextLayer");
            var TextScene = _vtSettings.RequireScene("TextTransScene");
            _karismaCG3.PlayText2LineTo3Line(TextScene, TextLayer, oldLine1, oldLine2, newText);
        }
        //Hàm chuyển 3Line sang Title (khi đang phát)
        private void Play_3Line_to_Title(string oldText, string newText)
        {
            var TextLayer = _vtSettings.RequireLayer("TextLayer");
            var TextScene = _vtSettings.RequireScene("TextTransScene");
            _karismaCG3.PlayText3LineToTitle(TextScene, TextLayer, oldText, newText);
        }
        //Hàm chuyển 3Line sang 2Line (khi đang phát)
        private void Play_3Line_to_2Line(string oldText, string newLine1, string newLine2)
        {
            var TextLayer = _vtSettings.RequireLayer("TextLayer");
            var TextScene = _vtSettings.RequireScene("TextTransScene");
            _karismaCG3.PlayText3LineTo2Line(TextScene, TextLayer, oldText, newLine1, newLine2);
        }
        //Hàm chuyển 3Line sang 3Line (khi đang phát)
        private void Play_3Line_to_3Line(string oldText, string newText)
        {
            var TextLayer = _vtSettings.RequireLayer("TextLayer");
            var TextScene = _vtSettings.RequireScene("TextTransScene");
            _karismaCG3.PlayText3LineTrans(TextScene, TextLayer, oldText, newText);
        }


        #endregion

        //Hàm phát LT + Text lần đầu
        private void LT_ShowFirst(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText)) return;

            if (!isPlayLT)
                Play_LT_Scene();  // In khung LT trước, theo rule mới

            var tag = GetLineTag(newText);
            switch (tag)
            {
                case LineTag.One:
                    Play_Title_In(newText);            // 1 dòng → cỡ Title (LT)
                    currentRaw = newText;
                    currentL1 = newText; currentL2 = string.Empty;
                    break;

                case LineTag.Two:
                    var (l1, l2) = SplitTwoLines(newText);
                    Play_2Line_In(l1, l2);             // 2 dòng → Content1/Content2
                    currentRaw = newText;
                    currentL1 = l1; currentL2 = l2;
                    break;

                case LineTag.Three:
                    Play_3Line_In(newText);            // 3+ dòng → Content nhỏ/vừa (đổ nguyên chuỗi)
                    currentRaw = newText;
                    currentL1 = string.Empty; currentL2 = string.Empty;
                    break;

                default:
                    return;
            }

            currentTag = tag;
            // ❗ Không cập nhật currentTitle ở đây
        }

        //Hàm phát chuyển Text to Text
        private void LT_TransitionTo(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
                return;

            if (!isPlayLT)
            {
                //LT_ShowFirst(newText);
                return;
            }

            var newTag = GetLineTag(newText);
            if (newTag == LineTag.None) return;

            switch (currentTag)
            {
                case LineTag.One: // cũ: 1 dòng (LT Title-size)
                    if (newTag == LineTag.One)
                        Play_TitleToTitle(currentRaw, newText);
                    else if (newTag == LineTag.Two)
                    {
                        var (nl1, nl2) = SplitTwoLines(newText);
                        Play_Title_to_2Line(currentRaw, nl1, nl2);
                        currentL1 = nl1; currentL2 = nl2;
                    }
                    else
                        Play_Title_to_3Line(currentRaw, newText);
                    break;

                case LineTag.Two: // cũ: 2 dòng
                    if (newTag == LineTag.One)
                    {
                        Play_2Line_to_Title(currentL1, currentL2, newText);
                        currentL1 = newText; currentL2 = string.Empty;
                    }
                    else if (newTag == LineTag.Two)
                    {
                        var (nl1, nl2) = SplitTwoLines(newText);
                        Play_2Line_to_2Line(currentL1, currentL2, nl1, nl2);
                        currentL1 = nl1; currentL2 = nl2;
                    }
                    else
                    {
                        Play_2Line_to_3Line(currentL1, currentL2, newText);
                        currentL1 = string.Empty; currentL2 = string.Empty;
                    }
                    break;

                case LineTag.Three: // cũ: 3 dòng+
                    if (newTag == LineTag.One)
                    {
                        Play_3Line_to_Title(currentRaw, newText);
                        currentL1 = newText; currentL2 = string.Empty;
                    }
                    else if (newTag == LineTag.Two)
                    {
                        var (nl1, nl2) = SplitTwoLines(newText);
                        Play_3Line_to_2Line(currentRaw, nl1, nl2);
                        currentL1 = nl1; currentL2 = nl2;
                    }
                    else
                        Play_3Line_to_3Line(currentRaw, newText);
                    break;

                default:
                    LT_ShowFirst(newText);
                    return;
            }

            currentRaw = newText;
            currentTag = newTag;
            // ❗ Không cập nhật currentTitle ở đây
        }
        private void ShowTitleScene(string title)
        {
            title = title?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(title)) return;

            Play_TitleScene(title);
            currentTitle = title;
            isTitleScenePlaying = true;
        }

        private void HideTitleSceneIfPlaying()
        {
            if (!isTitleScenePlaying) return;
            Play_TitleScene_Out();
            isTitleScenePlaying = false;
        }

        // Ẩn text LT theo currentTag, KHÔNG đụng khung LT
        private void LT_PlayOutCurrentText()
        {
            if (currentTag == LineTag.None) return;

            var textLayer = _vtSettings.RequireLayer("TextLayer");
            var textOutScene = _vtSettings.RequireScene("TextOutScene");

            switch (currentTag)
            {
                case LineTag.One:   // 1 dòng (title-size)
                    _karismaCG3.PlayOneLineOut(textOutScene, textLayer, currentRaw);
                    break;

                case LineTag.Two:   // 2 dòng
                    _karismaCG3.PlayTwoLineOut(textOutScene, textLayer, currentL1, currentL2);
                    break;

                case LineTag.Three: // 3+ dòng
                    _karismaCG3.PlayContentOut(textOutScene, textLayer, currentRaw);
                    break;
            }

            // Xoá state text, nhưng GIỮ currentTag để bạn biết vừa out xong
            currentRaw = string.Empty;
            currentL1 = string.Empty;
            currentL2 = string.Empty;
            currentTag = LineTag.None;
        }
        // C. Ẩn text LT + khung LT
        private void HideLTAll()
        {
            //Out Text LT
            LT_PlayOutCurrentText();
            //Out LT Scene
            HideLTSceneIfPlaying();
        }

        // Ẩn khung LT (BannerLayer)
        private void HideLTSceneIfPlaying()
        {
            if (!isPlayLT) return;

            Play_LT_Scene_Out();
            isPlayLT = false;
            currentTag = LineTag.None;
            currentRaw = currentL1 = currentL2 = string.Empty;
        }


        private void btnXoaLT_Click(object sender, EventArgs e)
        {
            HideTitleSceneIfPlaying();
            HideLTAll();
        }

        private void beforeLT_Click(object sender, EventArgs e)
        {
            if (dgvLT.CurrentRow != null && dgvLT.CurrentRow.Index > 0)
            {
                int currentIndex = dgvLT.CurrentRow.Index - 1;
                dgvLT.Rows[currentIndex].Selected = true;
                dgvLT.CurrentCell = dgvLT.Rows[currentIndex].Cells[0];
                dgvLT_SelectionChanged(this, new DataGridViewCellEventArgs(0, currentIndex));
            }
        }

        private void nextLT_Click(object sender, EventArgs e)
        {
            if (dgvLT.CurrentRow != null && dgvLT.CurrentRow.Index < dgvLT.Rows.Count - 1)
            {
                int currentIndex = dgvLT.CurrentRow.Index + 1;
                dgvLT.Rows[currentIndex].Selected = true;
                dgvLT.CurrentCell = dgvLT.Rows[currentIndex].Cells[0];
                dgvLT_SelectionChanged(this, new DataGridViewCellEventArgs(0, currentIndex));
            }
        }




        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetDataFromTXT();
        }


        private void btnStopAll_Click(object sender, EventArgs e)
        {
            isPlayLT = isTitleScenePlaying = false;
            currentTitle = string.Empty; // Reset tiêu đề hiện tại
            currentRaw = string.Empty; currentL1 = string.Empty; currentL2 = string.Empty;

            btnXoaTroiTin_Click(null,null);
            _karismaCG3.StopAll();
            _statusReporter.UpdateStatusLabel("Đã dừng tất cả các lớp.");
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

        private List<List<Tuple<string, string, string>>> nameBlocks = new List<List<Tuple<string, string, string>>>();

        private void LoadTxtToDgvLT(string filePath)  // Hàm này sẽ load file ruttit vào dgvLT
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Không tìm thấy file!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var lines = File.ReadAllLines(filePath);
            var blocks = ParseBlocks(lines);

            BindBlocksToGrid(blocks); // cập nhật dgvLT với dữ liệu từ blocks đã chuyển đổi
            SelectFirstRowAndRaise(); // tách chọn dòng đầu + fire selection
        }

        private void SelectFirstRowAndRaise()
        {
            if (dgvLT.Rows.Count == 0) return;

            dgvLT.ClearSelection();
            var row = dgvLT.Rows[0];
            row.Selected = true;
            dgvLT.CurrentCell = row.Cells[0];

            // Gọi logic cập nhật view phụ (giữ nguyên event hiện có)
            dgvLT_SelectionChanged(dgvLT, EventArgs.Empty);
        }

        private void dgvLT_SelectionChanged(object sender, EventArgs e)
        {
            var row = dgvLT.CurrentRow;
            if (row == null) return;

            int index = row.Index;
            if (index < 0 || index >= nameBlocks.Count) return;

            dgvContent.SuspendLayout();
            try
            {
                dgvContent.Rows.Clear(); // dgvContent có 1 cột: ColContent

                var pairs = nameBlocks[index];
                foreach (var pair in pairs) // Tuple<string, string>
                {
                    var line1 = pair.Item1?.Trim();
                    var line2 = pair.Item2?.Trim();
                    var line3 = pair.Item3?.Trim();

                    if (!string.IsNullOrWhiteSpace(line1) || !string.IsNullOrWhiteSpace(line2) || !string.IsNullOrWhiteSpace(line3))
                    {
                        // Ghép line1 và line2 bằng xuống dòng
                        var content = string.Join(Environment.NewLine,
                                                  new[] { line1, line2, line3 }
                                                  .Where(s => !string.IsNullOrWhiteSpace(s)));

                        dgvContent.Rows.Add(content);
                    }
                }


                // Hiển thị giá trị đang chọn từ dgvLT vào txtLine1
                // Ở dgvLT, line1 nằm ở cột đầu tiên (Cells[0])
                var cellValue = row.Cells[0]?.Value?.ToString();
                txtTitle.Text = cellValue ?? string.Empty;

                if (dgvContent.Rows.Count == 0)
                {
                    txtContent.Text = string.Empty;
                    return;
                }
            }
            finally
            {
                dgvContent.ResumeLayout();
            }
        }


        private void btnHienPV_Click(object sender, EventArgs e)
        {
            // 1) Quản lý phát TitleScene
            string newTitle = txtTitle.Text?.Trim() ?? string.Empty;

            if (!isTitleScenePlaying)
            {
                // Chưa phát → phát mới
                ShowTitleScene(newTitle);
            }
            else
            {
                // Nếu title mới khác title cũ thì phát lại TitleScene với text mới
                if (!string.Equals(newTitle, currentTitle, StringComparison.Ordinal))
                {
                    HideTitleSceneIfPlaying();
                    Thread.Sleep(500); // chờ 1 chút cho out xong
                    ShowTitleScene(newTitle);
                }
                // Nếu giống -> không hiện lại (để nguyên trạng thái đã ẩn theo yêu cầu)
            }

            // LT flow với txtContent
            string content = txtContent.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(content)) return;

            if (!isPlayLT)
                LT_ShowFirst(content);
            else
                LT_TransitionTo(content);

            if (dgvContent != null && dgvContent.CurrentRow != null) 
                dgvContent.CurrentRow.DefaultCellStyle.BackColor = Color.Gray;
        }


        private void btnStopLive_Click(object sender, EventArgs e)
        {
            var scene = _vtSettings.RequireScene("LiveOutScene");
            var layer = _vtSettings.RequireLayer("DiaDanhLayer");

            _karismaCG3.PlayScene(scene, layer);
        }

        private void btnKhungBanner_Click(object sender, EventArgs e)
        {
            var scene = _vtSettings.RequireScene("KhungHeadlineScene");
            var layer = _vtSettings.RequireLayer("KhungLayer");

            _karismaCG3.PlayScene(scene, layer);
        }

        private void btnKhungOut_Click(object sender, EventArgs e)
        {
            var layer = _vtSettings.RequireLayer("KhungLayer");

            _karismaCG3.PlayOut(layer);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            frmDiaDanh frmDiaDanh = new frmDiaDanh();
            var fileDD = Path.Combine(workingFolder, ControlConfig["Diadanh"]);
            frmDiaDanh.fileDiaDanh = fileDD;

            frmDiaDanh.FormClosed += (s, args) =>
            {
                // Reload UTC file into cbbUTC after frmUTC is closed
                if (!string.IsNullOrEmpty(fileDD))
                {
                    LoadComboBox(fileDD, cbbLocation);
                }
            };
            frmDiaDanh.ShowDialog();
            frmDiaDanh.BringToFront();
        }

        private void dgvContent_SelectionChanged(object sender, EventArgs e)
        {
            // Nếu không có dòng nào trong dgvContent → clear txtContent
            if (dgvContent.Rows.Count == 0)
            {
                txtContent.Text = string.Empty;
                return;
            }

            var row = dgvContent.CurrentRow;
            if (row == null)
            {
                txtContent.Text = string.Empty;
                return;
            }

            // Lấy giá trị cell
            var cellValue = row.Cells[0]?.Value?.ToString();
            txtContent.Text = string.IsNullOrWhiteSpace(cellValue) ? string.Empty : cellValue;

            // TODO: Đổi màu dòng đã chọn (nếu cần)
        }

        private void beforeContent_Click(object sender, EventArgs e)
        {
            if (dgvContent.CurrentRow == null) return;
            SelectContentRow(dgvContent.CurrentRow.Index - 1);
        }

        private void nextContent_Click(object sender, EventArgs e)
        {
            if (dgvContent.CurrentRow == null) return;
            SelectContentRow(dgvContent.CurrentRow.Index + 1);
        }

        private void SelectContentRow(int index)
        {
            if (index < 0 || index >= dgvContent.Rows.Count) return;

            dgvContent.ClearSelection();
            dgvContent.CurrentCell = dgvContent.Rows[index].Cells[0];
            dgvContent.Rows[index].Selected = true;

            txtContent.Text = dgvContent.Rows[index].Cells[0]?.Value?.ToString() ?? string.Empty;

            dgvContent_SelectionChanged(dgvContent, EventArgs.Empty);
            dgvContent.FirstDisplayedScrollingRowIndex = index;
        }

        private void cbbBar_SelectedIndexChanged(object sender, EventArgs e)
        {
            var BarIn = _vtSettings.RequireScene("BannerScene");
            var BarOut = _vtSettings.RequireScene("BannerOutScene");

            var Bar2In = _vtSettings.RequireScene("BannerHeadlineScene");
            var Bar2Out = _vtSettings.RequireScene("BannerHeadlineOutScene");

            if (cbbBar.SelectedIndex == 0) // Banner
            {
                LTSceneIn = BarIn;
                LTSceneOut = BarOut;
            }
            else if (cbbBar.SelectedIndex == 1) // Banner Headline
            {
                LTSceneIn = Bar2In;
                LTSceneOut = Bar2Out;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            HideTitleSceneIfPlaying();
            // LT flow với txtContent
            string content = txtContent.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(content)) return;

            if (!isPlayLT)
                LT_ShowFirst(content);
            else
                LT_TransitionTo(content);

            if (dgvContent != null && dgvContent.CurrentRow != null)
                dgvContent.CurrentRow.DefaultCellStyle.BackColor = Color.Gray;

        }

        private void btnTroiTinTuc_Click(object sender, EventArgs e)
        {
            string speedStr = ControlConfig["speedTroiTin"];
            int.TryParse(speedStr, out int speed);

            var layer = _vtSettings.RequireLayer("CrawlLayer");
            string scenePath = _vtSettings.RequireScene("CrawlScene");

            _frmTroiTinTuc = new frmTroiTinTuc(_karismaCG3, layer, speed, scenePath, "NewslineSettings");

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

        private void btnTroiCuoi_Click(object sender, EventArgs e)
        {
            string speedStr = ControlConfig["speedTroiCuoi"];
            int.TryParse(speedStr, out int speed);

            var layer = _vtSettings.RequireLayer("CrawlLayer");
            string scenePath = _vtSettings.RequireScene("CrawlScene");

            _frmTroiNgang = new frmTroiNgang(_karismaCG3, layer, speed, scenePath, "NewslineSettings");

            Form mainForm = this.FindForm();
            var mainFormBounds = mainForm.Bounds;

            _frmTroiNgang.StartPosition = FormStartPosition.Manual;
            _frmTroiNgang.Location = new Point(mainFormBounds.Right, mainFormBounds.Top + (mainFormBounds.Height - _frmTroiNgang.Height) / 2);

            _frmTroiNgang.Show();
        }

        private void btnXoaLT2_Click(object sender, EventArgs e)
        {
            HideTitleSceneIfPlaying();
            HideLTAll();
        }

        private void btnXoaPV_Click(object sender, EventArgs e)
        {
            var layer = _vtSettings.RequireLayer("PVLayer");
            _karismaCG3.PlayOut(layer);
        }

        private void btnImportRuttit_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Chọn file ruttit để import";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        fileRuttit = openFileDialog.FileName;
                        LoadTxtToDgvLT(fileRuttit);
                        _statusReporter.UpdateStatusLabel($"Đã import file: {Path.GetFileName(fileRuttit)}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi đọc file: {ex.Message}", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _statusReporter.UpdateStatusLabel("Lỗi khi import file.");
                    }
                }
            }
        }

        private void btnExportRuttit_Click(object sender, EventArgs e)
        {
            if (dgvLT.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Title = "Xuất file ruttit";
                saveFileDialog.FileName = "ruttit_exported.txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportRuttitToFile(saveFileDialog.FileName);
                        _statusReporter.UpdateStatusLabel($"Đã xuất file: {Path.GetFileName(saveFileDialog.FileName)}");
                        MessageBox.Show($"Đã xuất thành công file: {Path.GetFileName(saveFileDialog.FileName)}",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _statusReporter.UpdateStatusLabel("Lỗi khi xuất file.");
                    }
                }
            }
        }

        private void ExportRuttitToFile(string filePath)
        {
            var exportLines = new List<string>();

            for (int i = 0; i < dgvLT.Rows.Count; i++)
            {
                var row = dgvLT.Rows[i];

                // Lấy header từ HeaderCell
                string header = row.HeaderCell.Value?.ToString();
                if (!string.IsNullOrEmpty(header))
                {
                    exportLines.Add($"[{header}]");
                }

                // Lấy dữ liệu từ dgvLT (dòng đầu tiên của block)
                string line1 = row.Cells[0]?.Value?.ToString()?.Trim() ?? string.Empty;
                string line2 = row.Cells[1]?.Value?.ToString()?.Trim() ?? string.Empty;
                string line3 = row.Cells.Count > 2 ? (row.Cells[2]?.Value?.ToString()?.Trim() ?? string.Empty) : string.Empty;

                // Thêm entry đầu tiên nếu có dữ liệu
                if (!string.IsNullOrEmpty(line1))
                {
                    exportLines.Add("#");
                    exportLines.Add(line1);
                    if (!string.IsNullOrEmpty(line2))
                    {
                        exportLines.Add(line2);
                        if (!string.IsNullOrEmpty(line3))
                        {
                            exportLines.Add(line3);
                        }
                    }
                }

                // Lấy dữ liệu từ nameBlocks (các dòng còn lại của block)
                if (i < nameBlocks.Count)
                {
                    var blockData = nameBlocks[i];
                    foreach (var pair in blockData)
                    {
                        string pairLine1 = pair.Item1?.Trim() ?? string.Empty;
                        string pairLine2 = pair.Item2?.Trim() ?? string.Empty;
                        string pairLine3 = pair.Item3?.Trim() ?? string.Empty;

                        // Chỉ thêm entry nếu có ít nhất line1
                        if (!string.IsNullOrEmpty(pairLine1))
                        {
                            exportLines.Add(string.Empty); // Thêm dòng trống trước mỗi entry mới
                            exportLines.Add("#");
                            exportLines.Add(pairLine1);
                            if (!string.IsNullOrEmpty(pairLine2))
                            {
                                exportLines.Add(pairLine2);
                                if (!string.IsNullOrEmpty(pairLine3))
                                {
                                    exportLines.Add(pairLine3);
                                }
                            }
                        }
                    }
                }

                // Thêm 2 dòng trống giữa các blocks (trừ block cuối)
                if (i < dgvLT.Rows.Count - 1)
                {
                    exportLines.Add(string.Empty);
                    exportLines.Add(string.Empty);
                }
            }

            // Ghi file với encoding UTF-8 để hỗ trợ tiếng Việt
            File.WriteAllLines(filePath, exportLines, System.Text.Encoding.UTF8);
        }

        private void btnSave_dgvLT_dgvContent_Click(object sender, EventArgs e)
        {
            // Show confirmation dialog
            var result = MessageBox.Show(
                "Lưu thay đổi Title và nội dung?", 
                "Xác nhận lưu", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            if (dgvLT.CurrentRow != null)
            {
                string titleValue = txtTitle.Text.Trim();
                dgvLT.CurrentRow.Cells[0].Value = titleValue; // Lưu vào cột đầu tiên dgvLT
            }

            if (dgvContent.CurrentRow != null)
            {
                string contentValue = txtContent.Text.Trim();
                dgvContent.CurrentRow.Cells[0].Value = contentValue; // Lưu vào cột duy nhất dgvContent

                // Cập nhật lại nameBlocks để đồng bộ dữ liệu
                UpdateNameBlocks(dgvLT.CurrentRow.Index, dgvContent.CurrentRow.Index, contentValue);
            }
            _statusReporter.UpdateStatusLabel("Đã lưu lại Title và nội dung");
        }

        // Phương thức hỗ trợ để cập nhật nameBlocks khi thay đổi content
        private void UpdateNameBlocks(int ltRowIndex, int contentRowIndex, string newContent)
        {
            if (ltRowIndex < 0 || ltRowIndex >= nameBlocks.Count)
                return;

            if (contentRowIndex < 0 || contentRowIndex >= nameBlocks[ltRowIndex].Count)
                return;

            // Tách nội dung thành các dòng
            var lines = newContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            string line1 = lines.Length > 0 ? lines[0].Trim() : string.Empty;
            string line2 = lines.Length > 1 ? lines[1].Trim() : string.Empty;
            string line3 = lines.Length > 2 ? lines[2].Trim() : string.Empty;

            // Cập nhật tuple trong nameBlocks
            var currentList = nameBlocks[ltRowIndex];
            currentList[contentRowIndex] = Tuple.Create(line1, line2, line3);
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            var mainForm = this.FindForm() as frmMain;
            if (mainForm != null)
            {
                mainForm.panelMainContent.Controls.Clear();
                mainForm.panelMainContent.Controls.Add(mainForm.flowLayoutPanel1);
            }
        }
        private void PlayPV2Common(
            string sceneName,
            TextBox txtName1, TextBox txtDes1,
            TextBox txtName2, TextBox txtDes2)
        {
            var name1 = txtName1.Text.Trim();
            var name2 = txtName2.Text.Trim();

            if (string.IsNullOrEmpty(name1))
            {
                MessageBox.Show("Vui lòng nhập thông tin Name1!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName1.Focus();
                return;
            }

            if (string.IsNullOrEmpty(name2))
            {
                MessageBox.Show("Vui lòng nhập thông tin Name2!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName2.Focus();
                return;
            }

            var des1 = "|  " + txtDes1.Text.Trim();
            var des2 = "|  " + txtDes2.Text.Trim();

            var scene = _vtSettings.RequireScene(sceneName);
            var layer = _vtSettings.RequireLayer("PVLayer");

            _karismaCG3.PlayPV2(scene, layer, name1, des1, name2, des2);
        }
        // dùng chung cho PV3
        private void PlayPV3Common(
            string sceneName,
            TextBox txtName1, TextBox txtDes1,
            TextBox txtName2, TextBox txtDes2,
            TextBox txtName3, TextBox txtDes3)
        {
            var names = new[] { txtName1, txtName2, txtName3 };
            for (int i = 0; i < names.Length; i++)
            {
                if (string.IsNullOrEmpty(names[i].Text.Trim()))
                {
                    MessageBox.Show($"Vui lòng nhập thông tin Name{i + 1}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    names[i].Focus();
                    return;
                }
            }

            var des1 = "|  " + txtDes1.Text.Trim();
            var des2 = "|  " + txtDes2.Text.Trim();
            var des3 = "|  " + txtDes3.Text.Trim();

            var scene = _vtSettings.RequireScene(sceneName);
            var layer = _vtSettings.RequireLayer("PVLayer");

            _karismaCG3.PlayPV3(scene, layer,
                txtName1.Text.Trim(), des1,
                txtName2.Text.Trim(), des2,
                txtName3.Text.Trim(), des3);
        }

        // dùng chung cho PV4
        private void PlayPV4Common(
            string sceneName,
            TextBox txtName1, TextBox txtDes1,
            TextBox txtName2, TextBox txtDes2,
            TextBox txtName3, TextBox txtDes3,
            TextBox txtName4, TextBox txtDes4)
        {
            var names = new[] { txtName1, txtName2, txtName3, txtName4 };
            for (int i = 0; i < names.Length; i++)
            {
                if (string.IsNullOrEmpty(names[i].Text.Trim()))
                {
                    MessageBox.Show($"Vui lòng nhập thông tin Name{i + 1}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    names[i].Focus();
                    return;
                }
            }

            var des1 = "|  " + txtDes1.Text.Trim();
            var des2 = "|  " + txtDes2.Text.Trim();
            var des3 = "|  " + txtDes3.Text.Trim();
            var des4 = "|  " + txtDes4.Text.Trim();

            var scene = _vtSettings.RequireScene(sceneName);
            var layer = _vtSettings.RequireLayer("PVLayer");

            _karismaCG3.PlayPV4(scene, layer,
                txtName1.Text.Trim(), des1,
                txtName2.Text.Trim(), des2,
                txtName3.Text.Trim(), des3,
                txtName4.Text.Trim(), des4);
        }

        private void XoaPV()
        {
            var layer = _vtSettings.RequireLayer("PVLayer");
            _karismaCG3.PlayOut(layer);
        }

        private void btnPV2trai_Click(object sender, EventArgs e)
        {
            PlayPV2Common("PV2TraiScene", txtPV2Name1, txtPV2Des1, txtPV2Name2, txtPV2Des2);
        }

        private void btnPV2giua_Click(object sender, EventArgs e)
        {
            PlayPV2Common("PV2NhoScene", txtPV2Name1, txtPV2Des1, txtPV2Name2, txtPV2Des2);
        }

        private void btnPV2phai_Click(object sender, EventArgs e)
        {
            PlayPV2Common("PV2PhaiScene", txtPV2Name1, txtPV2Des1, txtPV2Name2, txtPV2Des2);
        }


        private void btnXoaPV2_Click(object sender, EventArgs e)
        {
            XoaPV();
        }

        private void btnPV3_Click(object sender, EventArgs e)
        {
            PlayPV3Common("PV3Scene",
                txtPV3Name1, txtPV3Des1,
                txtPV3Name2, txtPV3Des2,
                txtPV3Name3, txtPV3Des3);
        }

        private void btnXoaPV3_Click(object sender, EventArgs e)
        {
            XoaPV();
        }

        private void btnPV4_Click(object sender, EventArgs e)
        {
            PlayPV4Common("PV4Scene",
                txtPV4Name1, txtPV4Des1,
                txtPV4Name2, txtPV4Des2,
                txtPV4Name3, txtPV4Des3,
                txtPV4Name4, txtPV4Des4);
        }

        private void btnXoaPV4_Click(object sender, EventArgs e)
        {
            XoaPV();
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
        private static List<NameBlock> ParseBlocks(string[] lines)
        {
            var result = new List<NameBlock>();
            NameBlock current = null;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = (lines[i] ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line == "#") continue;

                // Header block: [Header]
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    current = new NameBlock(line.Trim('[', ']'));
                    result.Add(current);
                    continue;
                }

                // Nếu chưa có block mà gặp dữ liệu -> tạo block "NoHeader"
                if (current == null)
                {
                    current = new NameBlock(string.Empty);
                    result.Add(current);
                }

                // Ghép cặp 3 dòng liên tiếp (nếu line tiếp theo hợp lệ)
                string line1 = line;
                string line2 = string.Empty;
                string line3 = string.Empty;

                // Kiểm tra dòng thứ 2
                string next = (i + 1 < lines.Length) ? (lines[i + 1] ?? string.Empty).Trim() : string.Empty;
                bool validSecond = !string.IsNullOrWhiteSpace(next) && !next.StartsWith("[") && next != "#";

                if (validSecond)
                {
                    line2 = next;

                    // Kiểm tra dòng thứ 3
                    string nextNext = (i + 2 < lines.Length) ? (lines[i + 2] ?? string.Empty).Trim() : string.Empty;
                    bool validThird = !string.IsNullOrWhiteSpace(nextNext) && !nextNext.StartsWith("[") && nextNext != "#";

                    if (validThird)
                    {
                        line3 = nextNext;
                        i += 2; // bỏ qua 2 dòng đã dùng
                    }
                    else
                    {
                        i++; // bỏ qua 1 dòng đã dùng
                    }
                }

                current.Pairs.Add(new NamePair(line1, line2, line3));
            }

            return result;
        }
        private void BindBlocksToGrid(List<NameBlock> blocks)
        {
            dgvLT.Rows.Clear();
            nameBlocks.Clear();

            foreach (var b in blocks)
            {
                var first = b.Pairs.FirstOrDefault();
                if (first != null)
                {
                    if (string.IsNullOrEmpty(first.Line2)) // Có đúng 1 dòng (line2 rỗng) → logic như cũ
                    {
                        int idx = dgvLT.Rows.Add(first.Line1, first.Line2, first.Line3);
                        dgvLT.Rows[idx].HeaderCell.Value = b.Header ?? string.Empty;

                        // phần còn lại đưa vào nameBlocks
                        var rest = b.Pairs.Skip(1)
                                        .Select(p => Tuple.Create(p.Line1, p.Line2, p.Line3))
                                        .ToList();
                        nameBlocks.Add(rest);
                    }
                    else
                    {
                        // Có 2 dòng trở lên → để trống dgvLT, đưa tất cả vào nameBlocks
                        int idx = dgvLT.Rows.Add(string.Empty, string.Empty, string.Empty);
                        dgvLT.Rows[idx].HeaderCell.Value = b.Header ?? string.Empty;

                        // Đưa tất cả pairs (kể cả first) vào nameBlocks
                        var allPairs = b.Pairs.Select(p => Tuple.Create(p.Line1, p.Line2, p.Line3)).ToList();
                        nameBlocks.Add(allPairs);
                    }
                }
            }

            DisableColumnSorting(dgvLT);
        }

    }
    public sealed class NamePair
    {
        public string Line1 { get; }
        public string Line2 { get; }
        public string Line3 { get; }
        public NamePair(string line1, string line2, string line3) 
        { 
            Line1 = line1; 
            Line2 = line2;
            Line3 = line3;
        }
    }

    public sealed class NameBlock
    {
        public string Header { get; }
        public List<NamePair> Pairs { get; } = new List<NamePair>();
        public NameBlock(string header) { Header = header ?? string.Empty; }
    }


}
