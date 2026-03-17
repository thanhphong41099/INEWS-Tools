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
using System.Diagnostics;
using TTDH;

namespace API_iNews
{
    public partial class FormInews : Form
    {
        private readonly IINewsService _service;
        private readonly Services.StoryExportService _exportService;
        private string _selectedQueue;
        private readonly string ROOT_QUEUE ;
        private ServerAPI _server;
        private Process _mockProcess;

        public FormInews()
        {
            InitializeComponent();
            _service = new INewsService();
            _exportService = new Services.StoryExportService();
            // Get from config or fallback to default
            ROOT_QUEUE = ConfigurationManager.AppSettings["QueuesRoot"] ?? "VO_BAN_TIN";

            // Wire up events
            this.FormClosing += FormInews_FormClosing;
        }

        private async void FormInews_Load(object sender, EventArgs e)
        {
            // Auto connect on load
            await ConnectToServer();
            
            // Auto start TCP Server on load
            btnStartServer_Click(null, null);
        }

        private void FormInews_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Tắt Mock process TRƯỚC HẾT để giải phóng các network handle (như port 3000) mà process con vô tình thừa kế
                if (_mockProcess != null && !_mockProcess.HasExited)
                {
                    _mockProcess.Kill();
                    _mockProcess.Dispose();
                    _mockProcess = null;
                }

                _service?.Disconnect();
                _server?.Stop();
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
                    
                    // Stop Local TCP Server
                    try { _server?.Stop(); _server = null; } catch { }

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

        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            await SelectQueue(e.Node);
        }

        private async Task SelectQueue(TreeNode node)
        {
            if (node != null)
            {
                // Assuming the Tag contains the full queue path needed for getting stories
                _selectedQueue = node.Tag as string;
                // If tag is null (fallback), use Text, but typically Tag is safer
                if (string.IsNullOrEmpty(_selectedQueue)) _selectedQueue = node.Text;

                lbStatus.Text = $"Đã chọn: {_selectedQueue}";

                // Load Data to Grid
                await LoadStoriesToGrid();
            }
        }

        private async Task LoadStoriesToGrid()
        {
            if (string.IsNullOrEmpty(_selectedQueue)) return;
            // Ensure connected
            if (!_service.IsConnected) return;

            try
            {
                lbStatus.Text = $"Đang tải dữ liệu từ {_selectedQueue}...";
                
                // 1. Get raw stories
                var rawStories = await _service.GetRawStoriesAsync(_selectedQueue);

                if (rawStories != null)
                {
                    // 2. Create DataTable using config Config fields
                    string fieldsConfig = ConfigurationManager.AppSettings["Fields"];
                    DataTable dt = await Task.Run(() => _exportService.CreateStoryTable(rawStories, fieldsConfig));

                    // 3. Bind to Grid
                    dataGridView1.DataSource = dt;
                    lbStatus.Text = $"Đã tải {dt.Rows.Count} tin.";
                }
            }
            catch (Exception ex)
            {
                lbStatus.Text = $"Lỗi tải dữ liệu: {ex.Message}";
            }
        }

        private async void btnVideoID_Click(object sender, EventArgs e)
        {
            await ExtractVideoIds(true);
        }

        private async Task ExtractVideoIds(bool showErrorPopup = true)
        {
            if (!_service.IsConnected)
            {
                lbStatus.Text = "Vui lòng kết nối trước!";
                return;
            }

            if (string.IsNullOrEmpty(_selectedQueue))
            {
                lbStatus.Text = "Vui lòng chọn một Queue!";
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
                    return;
                }

                // 2. Extract to DataTable
                string fieldsConfig = ConfigurationManager.AppSettings["Fields"];
                DataTable dt = await Task.Run(() => _exportService.CreateStoryTable(rawStories, fieldsConfig));

                // 3. Save to Text File
                string exportFolder = GetExportPath();
                if (!System.IO.Directory.Exists(exportFolder))
                {
                    System.IO.Directory.CreateDirectory(exportFolder);
                }

                // Fixed filename as requested
                string fileName = "videoID_list.xml";
                string fullPath = System.IO.Path.Combine(exportFolder, fileName);

                _exportService.SaveStoriesToXml(dt, fullPath);

                lbStatus.Text = $"Đã xuất {dt.Rows.Count} dòng ra file: {fileName}";

                // Show in lbStatus
                lbStatus.Text = $"Đã xuất thành công {dt.Rows.Count} dòng dữ liệu.\n\nĐường dẫn:\n{fullPath}";
            }
            catch (Exception ex)
            {
                lbStatus.Text = $"Lỗi: {ex.Message}";
                if (showErrorPopup)
                {
                    MessageBox.Show(lbStatus.Text, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                btnVideoID.Enabled = true;
            }
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
             // Toggle Local TCP Server (Port 3000) for External Scripts
            try 
            {
                if (_server == null)
                {
                    string serverIP = ConfigurationManager.AppSettings["ServerIP"];
                    if (string.IsNullOrEmpty(serverIP)) serverIP = "127.0.0.1"; // Default safety

                    _server = new ServerAPI(serverIP);
                    // Safe invoking to avoid cross-thread exceptions
                    _server.Recieve += (msg) => { 
                        if (!this.IsDisposed && this.InvokeRequired)
                        {
                            this.BeginInvoke(new Action(async () => {
                                lbStatus.Text = "TCP: " + msg;
                                if (msg.StartsWith("TRIGGER_EXTRACT|") || msg.StartsWith("TRIGGER_EXTRACT#"))
                                {
                                    string[] parts = msg.Split(new char[] { '|', '#' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (parts.Length == 2)
                                    {
                                        _selectedQueue = parts[1];
                                        await ExtractVideoIds(false);
                                    }
                                }
                            })); 
                        }
                    };
                    _server.Start();
                    lbStatus.Text = $"TCP Server đã khởi động tại Port 3000.\nIP: {serverIP}";
                    btnStartServer.Text = "Stop Server";
                    btnStartServer.Enabled = true;
                }
                else
                {
                    _server.Stop();
                    _server = null;
                    
                    // Kill Mock Process if it's running
                    if (_mockProcess != null && !_mockProcess.HasExited)
                    {
                        _mockProcess.Kill();
                        _mockProcess.Dispose();
                        _mockProcess = null;
                        btnMockData.Text = "Mock Data";
                    }

                    lbStatus.Text = "TCP Server (và Mock Server) đã được dừng.";
                    btnStartServer.Text = "Start Server";
                    btnStartServer.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi động/dừng ServerAPI: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMockData_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mockProcess == null || _mockProcess.HasExited)
                {
                    // Lên lịch Start Process Python
                    string scriptPath = "D:\\PhungPhongDEV\\INEWS-Tools\\mock_inews_server.py";
                    
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "python";
                    startInfo.Arguments = $"\"{scriptPath}\"";
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = true; // Chạy ngầm

                    _mockProcess = Process.Start(startInfo);
                    
                    ((Button)sender).Text = "Stop Mock";
                    lbStatus.Text = "Mock Server (Python) đang chạy ngầm ở Port 8080.";
                }
                else
                {
                    // Tắt Process
                    if (!_mockProcess.HasExited)
                    {
                        _mockProcess.Kill();
                    }
                    _mockProcess.Dispose();
                    _mockProcess = null;
                    
                    ((Button)sender).Text = "Mock Data";
                    lbStatus.Text = "Mock Server (Python) đã tắt.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gọi Mock Server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
