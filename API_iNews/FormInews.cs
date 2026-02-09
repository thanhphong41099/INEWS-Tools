using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace API_iNews
{
    public partial class FormInews : Form
    {
        private readonly IINewsService _service;
        private string _selectedQueue;
        private readonly string ROOT_QUEUE ;

        public FormInews()
        {
            InitializeComponent();
            _service = new INewsService();
            // Get from config or fallback to default
            ROOT_QUEUE = ConfigurationManager.AppSettings["QueuesRoot"] ?? "VO_BAN_TIN";

            // Wire up events
            this.FormClosing += FormInews_FormClosing;
        }

        private async void FormInews_Load(object sender, EventArgs e)
        {
            // Auto connect on load
            await ConnectToServer();
        }

        private void FormInews_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _service?.Disconnect();
            }
            catch { }
        }

        private async Task LoadTree(string rootQueueName)
        {
            try
            {
                treeView1.Nodes.Clear();
                TreeNode rootNode = new TreeNode(rootQueueName) { Tag = rootQueueName };
                treeView1.Nodes.Add(rootNode);

                var children = await _service.GetQueueChildrenAsync(rootQueueName);
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        // iNews returns names, we assume full path might be needed or just name relative to parent
                        // legacy GetFolderChildren usually returns just names for folders
                        string childValues = child.Trim(); 
                        if(!string.IsNullOrEmpty(childValues))
                        {
                             TreeNode childNode = new TreeNode(childValues);
                             // Start full path construction if needed, for now just simplistic
                             childNode.Tag = $"{rootQueueName}.{childValues}"; 
                             rootNode.Nodes.Add(childNode);
                        }
                    }
                }
                
                rootNode.Expand();
            }
            catch (Exception ex)
            {
                lbStatus.Text = $"Lỗi tải cây thư mục: {ex.Message}";
            }
        }



        private async void btnConnect_Click(object sender, EventArgs e)
        {
            await ConnectToServer();
        }

        private async Task ConnectToServer()
        {
            try
            {
                // Toggle connection logic
                if (_service.IsConnected)
                {
                    _service.Disconnect();
                    btnConnect.Text = "Connect Server";
                    lbStatus.Text = "Đã ngắt kết nối.";
                    treeView1.Nodes.Clear();
                    _selectedQueue = null;
                    return;
                }

                lbStatus.Text = "Đang kết nối...";
                btnConnect.Enabled = false;

                bool connected = await _service.ConnectAsync();

                if (connected)
                {
                    lbStatus.Text = "Kết nối thành công!";
                    btnConnect.Text = "Disconnect";
                    await LoadTree(ROOT_QUEUE);
                }
                else
                {
                    string error = (_service as INewsService)?.LastError ?? "Lỗi không xác định";
                    lbStatus.Text = $"Kết nối thất bại: {error}";
                    MessageBox.Show(lbStatus.Text, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lbStatus.Text = $"Lỗi hệ thống: {ex.Message}";
            }
            finally
            {
                btnConnect.Enabled = true;
            }
        }

        private async void btnExportXML_Click(object sender, EventArgs e)
        {
            await ExportStories();
        }

        private string GetExportPath()
        {
            // Get Base Path from Config
            string folderToSave = ConfigurationManager.AppSettings["FolderToSave"];
            if (string.IsNullOrEmpty(folderToSave))
            {
                folderToSave = @"D:\TEST\XML"; // Default fallback
            }

            // Append selected queue name
            // Sanitize queue name for file system just in case
            string safeQueueName = _selectedQueue;
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                safeQueueName = safeQueueName.Replace(c, '_');
            }

            return System.IO.Path.Combine(folderToSave, safeQueueName);
        }

        private async Task ExportStories()
        {
            if (!_service.IsConnected)
            {
                MessageBox.Show("Vui lòng kết nối trước!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(_selectedQueue))
            {
                MessageBox.Show("Vui lòng chọn một Queue để xuất tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string exportPath = GetExportPath();

                // Create directory if not exists
                if (!System.IO.Directory.Exists(exportPath))
                {
                    System.IO.Directory.CreateDirectory(exportPath);
                }

                lbStatus.Text = $"Đang lấy tin từ {_selectedQueue}...";
                btnExportXML.Enabled = false;

                var rawStories = await _service.GetRawStoriesAsync(_selectedQueue);

                if (rawStories == null || rawStories.Count == 0)
                {
                    lbStatus.Text = "Không tìm thấy tin nào trong queue này.";
                    MessageBox.Show("Queue rỗng hoặc không lấy được tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lbStatus.Text = $"Đang xuất {rawStories.Count} tin...";
                    
                    // Run export on background thread
                    await Task.Run(() => _service.ExportStoriesToXml(rawStories, exportPath));

                    lbStatus.Text = $"Xuất thành công {rawStories.Count} file XML!";
                    MessageBox.Show($"Đã xuất xong {rawStories.Count} tin vào thư mục:\n{exportPath}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                string err = (_service as INewsService)?.LastError ?? ex.Message;
                lbStatus.Text = $"Lỗi xuất XML: {err}";
                MessageBox.Show(lbStatus.Text, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnExportXML.Enabled = true;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectQueue(e.Node);
        }

        private void SelectQueue(TreeNode node)
        {
            if (node != null)
            {
                // Assuming the Tag contains the full queue path needed for getting stories
                _selectedQueue = node.Tag as string;
                // If tag is null (fallback), use Text, but typically Tag is safer
                if (string.IsNullOrEmpty(_selectedQueue)) _selectedQueue = node.Text;

                lbStatus.Text = $"Đã chọn: {_selectedQueue}";
            }
        }

        private async void btnVideoID_Click(object sender, EventArgs e)
        {
            await ExtractVideoIds();
        }

        private async Task ExtractVideoIds()
        {
            if (!_service.IsConnected)
            {
                MessageBox.Show("Vui lòng kết nối trước!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(_selectedQueue))
            {
                MessageBox.Show("Vui lòng chọn một Queue!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                lbStatus.Text = $"Đang tải dữ liệu từ {_selectedQueue}...";
                btnVideoID.Enabled = false;

                // 1. Get raw stories
                var rawStories = await _service.GetRawStoriesAsync(_selectedQueue);

                if (rawStories == null || rawStories.Count == 0)
                {
                    lbStatus.Text = "Không tìm thấy tin nào.";
                    MessageBox.Show("Queue rỗng hoặc không lấy được tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // --- DEBUG: START ---
                // Dump the first story to a text file to check format
                if (rawStories.Count > 0)
                {
                   try {
                        string debugPath = System.IO.Path.Combine(GetExportPath(), "debug_first_story.xml");
                        if (!System.IO.Directory.Exists(GetExportPath())) System.IO.Directory.CreateDirectory(GetExportPath());
                        System.IO.File.WriteAllText(debugPath, rawStories[0]);
                   } catch {}
                }
                // --- DEBUG: END ---

                // 2. Extract to DataTable
                DataTable dt = await Task.Run(() => CreateVideoIdTable(rawStories));

                // 3. Save to Text File
                string exportFolder = GetExportPath();
                if (!System.IO.Directory.Exists(exportFolder))
                {
                    System.IO.Directory.CreateDirectory(exportFolder);
                }

                string fileName = $"VideoIDs_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string fullPath = System.IO.Path.Combine(exportFolder, fileName);

                SaveDataTableToTxt(dt, fullPath);

                lbStatus.Text = $"Đã xuất {dt.Rows.Count} dòng ra file: {fileName}";
                
                // Show in MessageBox
                string msg = $"Đã xuất thành công {dt.Rows.Count} dòng dữ liệu.\n\nĐường dẫn:\n{fullPath}";
                MessageBox.Show(msg, "Xuất Vdieo ID Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lbStatus.Text = $"Lỗi: {ex.Message}";
                MessageBox.Show(lbStatus.Text, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnVideoID.Enabled = true;
            }
        }

        private void SaveDataTableToTxt(DataTable dt, string filePath)
        {
            StringBuilder sb = new StringBuilder();

            // Header
            sb.AppendLine("Page\tTitle\tVideo ID");
            // Removed separator line to ensure clean TSV format

            // Rows
            foreach (DataRow row in dt.Rows)
            {
                string page = SanitizeField(row["page-number"]?.ToString());
                string title = SanitizeField(row["title"]?.ToString());
                string vid = SanitizeField(row["video-id"]?.ToString());
                sb.AppendLine($"{page}\t{title}\t{vid}");
            }

            System.IO.File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private string SanitizeField(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            // Replace tabs and newlines with space to maintain TSV structure
            return input.Replace("\t", " ").Replace("\r", " ").Replace("\n", " ").Trim();
        }

        private DataTable CreateVideoIdTable(List<string> rawXmlList)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("page-number");
            dt.Columns.Add("title");
            dt.Columns.Add("video-id");

            foreach (string xml in rawXmlList)
            {
                try
                {
                    DataRow row = dt.NewRow();
                    row["page-number"] = ExtractField(xml, "page-number");
                    row["title"] = ExtractField(xml, "title");
                    row["video-id"] = ExtractField(xml, "video-id");
                    
                    // Only add if at least one field has data
                    if (!string.IsNullOrEmpty(row["title"].ToString()) || !string.IsNullOrEmpty(row["video-id"].ToString()))
                    {
                        dt.Rows.Add(row);
                    }
                }
                catch { }
            }
            return dt;
        }

        private string ExtractField(string xmlInfo, string fieldName)
        {
            try
            {
                // Improved regex to handle attributes order, quotes, and whitespace
                // Matches: <f id="fieldName">...</f> or <f id='fieldName'>...</f>
                // Case insensitive, singleline mode
                string pattern = $@"<f[^>]*id\s*=\s*[""']{fieldName}[""'][^>]*>(.*?)</f>";
                
                var match = System.Text.RegularExpressions.Regex.Match(xmlInfo, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
                
                if (match.Success)
                {
                    return match.Groups[1].Value.Trim();
                }
                
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
