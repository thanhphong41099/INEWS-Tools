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
            this.btnConnect.Click += BtnConnect_Click;
            this.btnExportXML.Click += BtnExportXML_Click;
            this.treeView1.AfterSelect += TreeView1_AfterSelect;
            this.FormClosing += FormInews_FormClosing;
        }

        private void FormInews_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _service?.Disconnect();
            }
            catch { }
        }

        private async void BtnConnect_Click(object sender, EventArgs e)
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

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                // Assuming the Tag contains the full queue path needed for getting stories
                _selectedQueue = e.Node.Tag as string;
                // If tag is null (fallback), use Text, but typically Tag is safer
                if (string.IsNullOrEmpty(_selectedQueue)) _selectedQueue = e.Node.Text;
                
                lbStatus.Text = $"Đã chọn: {_selectedQueue}";
            }
        }

        private async void BtnExportXML_Click(object sender, EventArgs e)
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

            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
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
                            // Run export on background thread to keep UI responsive
                            await Task.Run(() => _service.ExportStoriesToXml(rawStories, fbd.SelectedPath));
                            
                            lbStatus.Text = $"Xuất thành công {rawStories.Count} file XML!";
                            MessageBox.Show($"Đã xuất xong {rawStories.Count} tin vào thư mục:\n{fbd.SelectedPath}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            }
        }

        private void FormInews_Load(object sender, EventArgs e)
        {

        }
    }
}
