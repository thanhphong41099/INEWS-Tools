using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TTDH;

namespace API_iNews
{
    public partial class API : Form
    {
        private ServerAPI server;
        private string Content = string.Empty;
        private readonly NameValueCollection appSettings;
        string workingFolder = string.Empty;
        public string selectedName = string.Empty;
        string fileExport = "D:\\StoryInews_exported.txt";
        DataTable tbl;

        public API()
        {
            InitializeComponent();

            appSettings = ConfigurationManager.AppSettings;

            workingFolder = GetAppSetting("WorkingFolder");
        }

        private string GetAppSetting(string key)
        {
            return appSettings[key] ?? string.Empty;
        }

        string QUEUEROOT = "VTV4.04_VO_BAN_TIN";
        string queueChild = "";
        iNewsData iData = null;
        WebServiceSettings settings = new WebServiceSettings();
        private void button1_Click(object sender, EventArgs e)
        {
            List<string> queues = iData.ChangedQueues();
            Display(queues);
        }

        private void API_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dừng server khi đóng form
            server?.Stop();
            iData?.DisconnectAsync();
        }
        private void Display(List<string> queues)
        {
            if (queues != null)
            {
                string str = "";
                //label1.Text = "";
                foreach (string s in queues)
                {
                    str += s + "\n";
                }
                //label1.Text = str;
            }
        }
        private async void API_Load(object sender, EventArgs e)
        {
            await ConnectServerToLoadDataAsync();
            lbTime.Text = "00:00:00";
        }

        private void Server_Recieve(string msg)
        {
            toolStripStatusLabel1.Text = msg;
        }

        void iData_SentError(string msg)
        {
            toolStripStatusLabel1.Text = msg;
        }
        public async Task LoadTreeStoriesAsync(TreeView tree, string nodeNames)
        {
            await Task.Run(() =>
            {
                string[] parentNode = Utility.Split(nodeNames, ";");

                foreach (string rootName in parentNode)
                {
                    if (string.IsNullOrEmpty(rootName))
                        continue;
                    
                    TreeNode root = new TreeNode();
                    root.Text = rootName;
                    List<string> queues = iData.GetFolderChildren(root.Text);
                    
                    foreach (string s in queues)
                    {
                        TreeNode childNode = new TreeNode();
                        childNode.Text = s;
                        childNode.Tag = root.Text + "." + s;

                        // Thêm cấp con để expand. Vd: BAN_TIN_12H00,...
                        childNode.Nodes.Add("Loading...");

                        root.Nodes.Add(childNode);
                    }
                    
                    // Cập nhật UI trên UI thread
                    this.BeginInvoke(new Action(() =>
                    {
                        root.Expand();
                        tree.Nodes.Add(root);
                    }));
                }
            });
        }

        private async void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //TreeNode node = e.Node;
            //if (node != null && node.Nodes.Count == 1 && node.Nodes[0].Text == "Loading...")
            //{
            //    // Hiển thị loading state
            //    toolStripStatusLabel1.Text = "Đang tải dữ liệu...";
                
            //    node.Nodes.Clear();
                
            //    // Chạy việc lấy dữ liệu trên background thread
            //    List<string> queues = await Task.Run(() => iData.GetFolderChildren(node.Tag.ToString()));
                
            //    // Cập nhật UI trên UI thread
            //    foreach (string child in queues)
            //    {
            //        TreeNode newNode = new TreeNode(child)
            //        {
            //            Tag = node.Tag + "." + child
            //        };

            //        node.Nodes.Add(newNode);
            //    }
            //    node.Expand();
            //}
        }

        private async Task LoadTreeQueuesAsync()
        {
            await LoadTreeStoriesAsync(treeView1, QUEUEROOT);
        }

        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if (node != null && node.Tag != null)
            {
                //if (node.Nodes.Count > 0)
                //{
                //    treeView1_BeforeExpand(sender, new TreeViewCancelEventArgs(node, false, TreeViewAction.ByMouse));
                //}
                //else
                {
                    // Hiển thị trạng thái loading
                    toolStripStatusLabel1.Text = "Đang tải stories...";
                    
                    string name = node.Tag.ToString().Replace("INEWS.", "");
                    if (!string.IsNullOrEmpty(queueChild))
                        name = name + "." + queueChild;
                    toolStripStatusLabel2.Text = name;
                    label2.Text = name + "." + queueChild;
                    selectedName = name;
                    
                    // Chạy việc lấy dữ liệu trên background thread
                    await Task.Run(() =>
                    {
                        List<string> queues = iData.GetStoriesBoard(name);
                        string mapping = System.Configuration.ConfigurationManager.AppSettings["Fields"];
                        ProcessingXMl2Class process = new ProcessingXMl2Class();
                        process.FieldMapping = mapping;
                        bool isExpXML = System.Configuration.ConfigurationManager.AppSettings["ExportStoryToXML"] == "1";
                        if (isExpXML)
                        {
                            ExportXML(queues);
                        }
                        tbl = process.GetDataRows(queues);
                        
                        // Cập nhật UI trên UI thread
                        this.BeginInvoke(new Action(() =>
                        {
                            dataGridView1.DataSource = tbl;
                            ApplyDataGridViewStyle(dataGridView1);
                            GetDataContent();
                            LoadContentTroiTin();
                            LoadContentTroiCuoi();
                        }));
                    });
                }
            }
        }

        private void ExportXML(List<string> queues)
        {
            if (queues == null || queues.Count <= 0)
                return;
            string folder = System.Configuration.ConfigurationManager.AppSettings["FolderToSave"];
            int a = 1;
            foreach (string s in queues)
            {
                if (!string.IsNullOrEmpty(s))
                {

                    System.IO.File.WriteAllText(folder + "\\story_" + a.ToString() + ".xml", s, Encoding.Unicode);
                    //
                }
                a++;
                //string xml = queues[0];
                //if (!string.IsNullOrEmpty(xml))
                //{
                //    string folder = System.Configuration.ConfigurationManager.AppSettings["FolderToSave"];
                //    System.IO.File.WriteAllText(folder + "\\story.xml", xml, Encoding.Unicode);
                //    toolStripStatusLabel1.Text = "Đã xuất XML story thành công.";
                //
            }
            toolStripStatusLabel1.Text = "Đã xuất XML story thành công.";
        }

        private void GetQueueChange()
        {
            List<string> strChange = iData.ChangedQueues();
            Display(strChange);
        }
        private void Export3TXTFiles_Click(object sender, EventArgs e)
        {
            GetDataContent(); // Lấy nội dung Content RAW
            if (string.IsNullOrWhiteSpace(Content))
            {
                //MessageBox.Show("Content rỗng!");
                return;
            }
            ProcessContentWithDD(Content);
            ProcessContentWithCG();

            MessageBox.Show("Đã xuất file ruttit.txt\n" +
                "Đã xuất file diadanh.txt\n", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void GetDataContent() // Xuất Content RAW
        {
            if (tbl != null && tbl.Rows.Count > 0)
            {
                StringBuilder contentBuilder = new StringBuilder();

                foreach (DataRow r in tbl.Rows)
                {
                    contentBuilder.AppendLine("[]");
                    contentBuilder.AppendLine("#");
                    contentBuilder.AppendLine(r["Content"]?.ToString() ?? string.Empty);
                    contentBuilder.AppendLine();
                }

                Content = contentBuilder.ToString();
            }
        }

        private void ProcessContentWithCG()
        {
            if (tbl != null && tbl.Rows.Count > 0)
            {
                StringBuilder contentBuilder = new StringBuilder();

                foreach (DataRow r in tbl.Rows)
                {
                    string rawContent = r["Content"]?.ToString() ?? string.Empty;
                    if (rawContent.Contains("##CG:"))
                    {
                        // Lấy giá trị page-number, nếu không có thì để rỗng
                        string pageNumber = r.Table.Columns.Contains("page-number")
                            ? r["page-number"]?.ToString() ?? ""
                            : "";

                        // Format lại cho giống [01], [02], ...
                        if (int.TryParse(pageNumber, out int pageNumInt))
                            pageNumber = pageNumInt.ToString("D2");

                        contentBuilder.AppendLine($"[{pageNumber}]");
                        contentBuilder.AppendLine("#");
                        var cgBlocks = ExtractCGBlocks(rawContent);

                        foreach (var block in cgBlocks)
                        {
                            foreach (var line in block)
                            {
                                contentBuilder.AppendLine(line);
                            }
                            contentBuilder.AppendLine(); // Thêm 1 dòng trống sau mỗi block
                        }
                    }
                }

                Content = contentBuilder.ToString();
                txtRuttitCG.Text = Content;

                //Xuất nội dung Content ra file ruttit.txt
                string ruttitSetting = GetAppSetting("Ruttit");
                string ruttitPath = Path.Combine(workingFolder, ruttitSetting);
                try
                {
                    File.WriteAllText(ruttitPath, Content, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi ghi file:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Trích xuất các block CG từ nội dung, mỗi block là List<string>
        /// </summary>
        private List<List<string>> ExtractCGBlocks(string content)
        {
            var result = new List<List<string>>();
            if (string.IsNullOrWhiteSpace(content))
                return result;

            var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            List<string> currentBlock = null;
            bool inCG = false;

            foreach (var raw in lines)
            {
                var line = raw.Trim();
                if (!inCG && (line == "##CG:"))
                {
                    inCG = true;
                    currentBlock = new List<string>();
                    continue;
                }
                if (inCG)
                {
                    if (line == "[]")
                    {
                        continue;
                    }
                    if (line == "##")
                    {
                        inCG = false;
                        if (currentBlock != null && currentBlock.Count > 0)
                            result.Add(currentBlock);
                        currentBlock = null;
                    }
                    else
                    {
                        currentBlock.Add(line);
                    }
                }
            }
            // Trường hợp cuối file không có "##"
            if (inCG && currentBlock != null && currentBlock.Count > 0)
                result.Add(currentBlock);

            return result;
        }

        private void ProcessContentWithDD(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                //MessageBox.Show("Content rỗng!");
                return;
            }

            var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var locations = new List<string>();

            for (int i = 0; i < lines.Length - 1; i++)
            {
                if (lines[i].Trim() == "##DD:")
                {
                    var location = lines[i + 1].Trim();
                    if (!string.IsNullOrWhiteSpace(location))
                        locations.Add(location);
                }
            }
            txtDiadanh.Text = string.Join("\r\n", locations);

            // Đọc đường dẫn lưu từ config
            string outPath = Path.Combine(workingFolder, GetAppSetting("Diadanh"));

            try
            {
                File.WriteAllLines(outPath, locations, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi file địa danh:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ProcessContentWithPhude(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            string phudePath = Path.Combine(workingFolder, GetAppSetting("Phude"));
            if (string.IsNullOrWhiteSpace(phudePath))
            {
                MessageBox.Show("Không tìm thấy đường dẫn lưu file phude.txt trong cấu hình!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                               .ToList();

            try
            {
                File.WriteAllLines(phudePath, lines, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi file phude.txt:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ApplyDataGridViewStyle(DataGridView dataGridView)
        {
            // Font chung cho toàn bảng
            Font defaultFont = new Font("Arial", 12, FontStyle.Regular);
            dataGridView.Font = defaultFont;

            // Font cho nội dung
            dataGridView.DefaultCellStyle.Font = defaultFont;
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Font cho tiêu đề cột
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            dataGridView.GridColor = Color.LightGray;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; // Tắt tự động giãn
                column.Width = 180;

            }

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridView1.Rows[e.RowIndex];
                var cellValue = selectedRow.Cells["Content"].Value;
                string content = cellValue != null ? cellValue.ToString() : string.Empty;

                // Nếu muốn loại bỏ các dòng trắng ở đầu/cuối, dùng Trim và Split, sau đó ghép lại
                var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                txtContent.Text = string.Join("\r\n", lines);
            }
        }

        private void btnXuatTroiTin_Click(object sender, EventArgs e)
        {
            try
            {
                string pathTroiTin = Path.Combine(workingFolder, GetAppSetting("TroiTin"));
                string pathTroiCuoi = Path.Combine(workingFolder, GetAppSetting("TroiCuoi"));
                if (string.IsNullOrWhiteSpace(pathTroiTin))
                {
                    MessageBox.Show("Không tìm thấy đường dẫn lưu file TroiTin.txt trong cấu hình!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (string.IsNullOrWhiteSpace(pathTroiCuoi))
                {
                    MessageBox.Show("Không tìm thấy đường dẫn lưu file TroiCuoi.txt trong cấu hình!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string mess = SaveToFile(txtTroiTin.Text, pathTroiTin);
                mess += (mess != "" ? "\n" : "") + SaveToFile(txtTroiCuoi.Text, pathTroiCuoi);
                if (!string.IsNullOrWhiteSpace(mess))
                {
                    MessageBox.Show(mess, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không có nội dung để ghi vào file TroiTin.txt hoặc TroiCuoi.txt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi file TroiTin.txt, TroiCuoi.txt:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string SaveToFile(string content, string filePath)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                // Kiểm tra thư mục lưu có tồn tại không, nếu không thì tạo mới
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    try
                    {
                        Directory.CreateDirectory(directory);
                    }
                    catch
                    {
                        return $"Không thể tạo thư mục lưu file: {directory}";
                    }
                }

                File.WriteAllText(filePath, content.Trim(), Encoding.UTF8);
                return $"Đã xuất file: {filePath}";
            }
            return "";
        }

        public async Task<string> RefreshDataiNewsAsync(string name)
        {
            try
            {
                await ConnectServerToLoadDataAsync(); // Gọi lại hàm Load async để khởi tạo lại kết nối và dữ liệu
                
                // Chạy các thao tác data processing trên background thread
                await Task.Run(() =>
                {
                    List<string> queues = iData.GetStoriesBoard(name);
                    string mapping = System.Configuration.ConfigurationManager.AppSettings["Fields"];
                    ProcessingXMl2Class process = new ProcessingXMl2Class();
                    process.FieldMapping = mapping;
                    bool isExpXML = System.Configuration.ConfigurationManager.AppSettings["ExportStory2XML"] == "1";
                    if (isExpXML)
                    {
                        ExportXML(queues);
                    }
                    DataTable tbl = process.GetDataRows(queues);
                    
                    // Cập nhật UI trên UI thread
                    this.BeginInvoke(new Action(() =>
                    {
                        dataGridView1.DataSource = tbl;
                    }));
                    
                    GetDataContent();
                    ProcessContentWithDD(Content);
                    ProcessContentWithCG();
                    
                    // Clear dataGridView1 trên UI thread
                    this.BeginInvoke(new Action(() =>
                    {
                        dataGridView1.DataSource = null;
                    }));
                });
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
            return string.Empty;
        }

        public string RefreshDataiNews(string name)
        {
            // Keep synchronous version for backward compatibility
            try
            {
                return RefreshDataiNewsAsync(name).Result;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        private async void btnGetStory_Click(object sender, EventArgs e)
        {
            try
            {
                ClearData();
                await ConnectServerToLoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load lại form: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ConnectServerToLoadDataAsync()
        {
            string serverIP = System.Configuration.ConfigurationManager.AppSettings["ServerIP"];

            // Hiển thị thông báo đang kết nối với IP cụ thể
            toolStripStatusLabel1.Text = $"Đang kết nối đến server {serverIP}...";

            // Chạy các thao tác blocking trên background thread
            await Task.Run(() =>
            {
                workingFolder = GetAppSetting("WorkingFolder");

                // Khởi tạo server
                server = new ServerAPI(serverIP);
                server.Recieve += Server_Recieve;
                server.Start();

                // Cấu hình iNews settings
                QUEUEROOT = System.Configuration.ConfigurationManager.AppSettings["QueuesRoot"];
                queueChild = System.Configuration.ConfigurationManager.AppSettings["QueuesChild"];
                settings.ServerName = System.Configuration.ConfigurationManager.AppSettings["iNewsServer"];
                settings.ServerBackup = System.Configuration.ConfigurationManager.AppSettings["iNewsServerBackup"];
                settings.UserName = System.Configuration.ConfigurationManager.AppSettings["iNewsUser"];
                settings.Password = System.Configuration.ConfigurationManager.AppSettings["iNewsPass"];
                fileExport = System.Configuration.ConfigurationManager.AppSettings["FileExport"];
                
                // Khởi tạo iNewsData
                iData = new iNewsData(settings);
                iData.SentError += new iNewsData.SendError(iData_SentError);
            });

            // Cập nhật UI và load tree data - thông báo sẽ được cập nhật qua Server_Recieve event
            await LoadTreeQueuesAsync();
        }

        public void ClearData()
        {
            // Clear DataGridView
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            // Clear TextBoxes
            txtContent.Clear();
            txtDiadanh.Clear();
            txtRuttitCG.Clear();
            txtTroiTin.Clear();
            txtTroiCuoi.Clear();
            // Clear TreeView
            treeView1.Nodes.Clear();
            // Clear Status
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            // Reset các biến nội bộ
            Content = string.Empty;
            selectedName = string.Empty;
            tbl = null;
            this.Refresh();
        }

        public void LoadContentTroiTin()
        {
            try
            {
                var keyTroiTin = System.Configuration.ConfigurationManager.AppSettings["KeyTroiTin"];
                if (!String.IsNullOrEmpty(Content) && Content.Contains(keyTroiTin.Split(new string[] { "..." }, StringSplitOptions.None)[0]))
                {
                    var matches = Regex.Matches(Content, keyTroiTin.Replace("...", @"\s*(.*?)\s*"), RegexOptions.Singleline);
                    StringBuilder sb = new StringBuilder();
                    foreach (Match m in matches)
                    {
                        sb.AppendLine(m.Groups[1].Value.Trim());
                    }
                    string result = (sb + "").Trim();
                    txtTroiTin.Text = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải nội dung trôi tin: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadContentTroiCuoi()
        {
            try
            {
                var keyTroiCuoi = System.Configuration.ConfigurationManager.AppSettings["KeyTroiCuoi"];
                if (!String.IsNullOrEmpty(Content) && Content.Contains(keyTroiCuoi.Split(new string[] { "..." }, StringSplitOptions.None)[0]))
                {
                    var matches = Regex.Matches(Content, keyTroiCuoi.Replace("...", @"\s*(.*?)\s*"), RegexOptions.Singleline);
                    StringBuilder sb = new StringBuilder();
                    foreach (Match m in matches)
                    {
                        sb.AppendLine(m.Groups[1].Value.Trim());
                    }
                    string result = (sb + "").Trim();
                    txtTroiCuoi.Text = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải nội dung trôi cuối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportContentRaw_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem đã có content để xuất chưa
                if (string.IsNullOrEmpty(Content))
                {
                    MessageBox.Show("Không có dữ liệu để xuất. Vui lòng chọn một bản tin trước.",
                                  "Thông báo",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra xem đã chọn bản tin chưa
                if (string.IsNullOrEmpty(selectedName))
                {
                    MessageBox.Show("Vui lòng chọn một bản tin trước khi xuất.",
                                  "Thông báo",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                // Lấy tên file từ selectedName (bỏ phần prefix nếu có)
                string fileName = selectedName.Split('.').Last(); // Lấy phần cuối cùng
                                                                  // Ví dụ: "VTV4.04_VO_BAN_TIN.NEWSLINE.NEWSLINE_9H00" -> "NEWSLINE_9H00"

                // Tạo SaveFileDialog
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    // Cấu hình dialog
                    saveFileDialog.Title = "Lưu file Content RAW";
                    saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.FileName = fileName + ".txt"; // Tên mặc định
                    saveFileDialog.DefaultExt = "txt";
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    // Hiển thị dialog và xử lý kết quả
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Tạo nội dung với tên file ở đầu
                        StringBuilder exportContent = new StringBuilder();
                        exportContent.AppendLine("[" + fileName + "]");
                        exportContent.Append(Content);

                        // Ghi file với UTF-8 encoding (không BOM)
                        System.Text.Encoding utf8WithoutBom = new System.Text.UTF8Encoding(false);
                        File.WriteAllText(saveFileDialog.FileName, exportContent.ToString(), utf8WithoutBom);

                        // Thông báo thành công
                        MessageBox.Show($"Xuất file thành công!\n\nĐường dẫn: {saveFileDialog.FileName}",
                                      "Thành công",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);

                        // Cập nhật status bar
                        toolStripStatusLabel1.Text = "Đã xuất file: " + Path.GetFileName(saveFileDialog.FileName);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Không có quyền ghi vào thư mục đã chọn. Vui lòng chọn thư mục khác.",
                              "Lỗi quyền truy cập",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"Lỗi khi ghi file:\n{ioEx.Message}",
                              "Lỗi I/O",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra:\n{ex.Message}",
                              "Lỗi",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }
        int i = 0;
        private async void btnExportAllRawContent_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra có QUEUEROOT không
                if (string.IsNullOrEmpty(QUEUEROOT))
                {
                    MessageBox.Show("Không có dữ liệu QUEUEROOT để xuất.",
                                  "Thông báo",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }
                // Lấy danh sách folder con cấp 1
                List<string> folders = iData.GetFolderChildren(QUEUEROOT);

                // Nếu không có thư mục con, trả về thông báo hoặc chuỗi rỗng
                if (folders == null || folders.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu nào để xuất.",
                                  "Thông báo",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }


                // Chọn nơi lưu file
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = "Lưu file tổng hợp Content RAW";
                    saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.FileName = QUEUEROOT + "_ALL_CONTENT.txt";
                    saveFileDialog.DefaultExt = "txt";
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                        return;

                    // Vô hiệu hóa nút và hiển thị trạng thái
                    btnExportAllRawContent.Enabled = false;
                    toolStripStatusLabel1.Text = "Đang xuất tất cả content...";
                    Cursor = Cursors.WaitCursor;

                    // Thực hiện xuất dữ liệu
                    string result = await ExportAllContentAsync(QUEUEROOT);

                    // Ghi file với UTF-8 encoding (không BOM)
                    System.Text.Encoding utf8WithoutBom = new System.Text.UTF8Encoding(false);
                    File.WriteAllText(saveFileDialog.FileName, result, utf8WithoutBom);

                    // Thông báo thành công
                    MessageBox.Show($"Xuất file thành công!\n\nĐường dẫn: {saveFileDialog.FileName}",
                                  "Thành công",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);

                    toolStripStatusLabel1.Text = "Hoàn tất xuất file tổng hợp";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra:\n{ex.Message}",
                              "Lỗi",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                toolStripStatusLabel1.Text = "Lỗi khi xuất file";
            }
            finally
            {
                btnExportAllRawContent.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private async Task<string> ExportAllContentAsync(string rootName)
        {
            return await Task.Run(() =>
            {
                StringBuilder allContent = new StringBuilder();

                // Lấy danh sách folder con cấp 1
                List<string> folders = iData.GetFolderChildren(rootName);

                int totalFolders = folders.Count;
                int currentFolder = 0;

                foreach (string folder in folders)
                {
                    currentFolder++;
                    string folderPath = rootName + "." + folder;

                    // Cập nhật status trên UI thread
                    this.BeginInvoke(new Action(() =>
                    {
                        toolStripStatusLabel1.Text = $"Đang xử lý {folder} ({currentFolder}/{totalFolders})...";
                    }));

                    // Lấy danh sách file con cấp 2
                    List<string> files = iData.GetFolderChildren(folderPath);

                    foreach (string file in files)
                    {
                        // BỎ QUA FILE RUNDOWN (case insensitive)
                        if (file.IndexOf("rundown", StringComparison.OrdinalIgnoreCase) >= 0)
                            continue;

                        string filePath = folderPath + "." + file;

                        try
                        {
                            // Lấy stories của file
                            List<string> stories = iData.GetStoriesBoard(filePath);

                            // BỎ QUA NẾU KHÔNG CÓ STORIES
                            if (stories == null || stories.Count == 0)
                                continue;

                            // Parse XML sang DataTable
                            string mapping = System.Configuration.ConfigurationManager.AppSettings["Fields"];
                            ProcessingXMl2Class process = new ProcessingXMl2Class();
                            process.FieldMapping = mapping;
                            DataTable tblTemp = process.GetDataRows(stories);

                            // BỎ QUA NẾU TABLE RỖNG
                            if (tblTemp == null || tblTemp.Rows.Count == 0)
                                continue;

                            // Kiểm tra có content thực sự không (không phải toàn rỗng)
                            bool hasRealContent = false;
                            foreach (DataRow row in tblTemp.Rows)
                            {
                                string content = row["Content"]?.ToString() ?? string.Empty;
                                if (!string.IsNullOrWhiteSpace(content))
                                {
                                    hasRealContent = true;
                                    break;
                                }
                            }

                            // BỎ QUA NẾU KHÔNG CÓ CONTENT THỰC SỰ
                            if (!hasRealContent)
                                continue;

                            // Thêm header cho file
                            allContent.AppendLine("[" + file + "]");

                            // Thêm content từng story
                            foreach (DataRow row in tblTemp.Rows)
                            {
                                string content = row["Content"]?.ToString() ?? string.Empty;

                                // Chỉ thêm những row có content
                                if (!string.IsNullOrWhiteSpace(content))
                                {
                                    allContent.AppendLine("[]");
                                    allContent.AppendLine("#");
                                    allContent.AppendLine(content);
                                    allContent.AppendLine();
                                }
                            }

                            // Dòng phân cách giữa các file
                            allContent.AppendLine("=====================================");
                            allContent.AppendLine();
                        }
                        catch (Exception ex)
                        {
                            // Log lỗi nhưng tiếp tục xử lý file khác
                            // (có thể comment đoạn này nếu muốn hoàn toàn bỏ qua lỗi)
                            allContent.AppendLine($"[LỖI: {file}]");
                            allContent.AppendLine($"Không thể đọc file: {ex.Message}");
                            allContent.AppendLine("=====================================");
                            allContent.AppendLine();
                        }
                    }
                }

                return allContent.ToString();
            });
        }

        int totalSeconds = 0;   // Tổng thời gian gốc nhập vào
        int currentSeconds = 0; // Thời gian còn lại của vòng lặp

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtSetTime.Text.Trim(), out totalSeconds) && totalSeconds > 0)
            {
                //btnExportAllRawContent.PerformClick();
                currentSeconds = totalSeconds;
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                txtSetTime.Enabled = false;
                label2.Text = "Đang chạy tự động...";
                timer1.Interval = 1000; // 1 giây
                timer1.Start();

                ShowTime(); // Hiển thị ngay số giây đầu vào
            }
            else
            {
                MessageBox.Show("Thời gian nhập vào không hợp lệ (phải là số nguyên dương, tính bằng giây).");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            txtSetTime.Enabled = true;
            label2.Text = "Đã dừng!";
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            currentSeconds--;

            if (currentSeconds <= 0)
            {
                btnExportAllRawContent.PerformClick(); // Xuất file khi hết thời gian
                currentSeconds = totalSeconds;         // Reset lại về đúng số giây thiết lập
                ShowTime();                            // Hiển thị lại toàn bộ thời gian
                return;                                // Thoát khỏi hàm, không chạy tiếp bên dưới!
            }

            ShowTime();
        }


        void ShowTime()
        {
            TimeSpan ts = TimeSpan.FromSeconds(currentSeconds);
            lbTime.Text = ts.ToString(@"hh\:mm\:ss");
        }


    }

}