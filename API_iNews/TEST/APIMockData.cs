using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace API_iNews
{
    public partial class API
    {
        private const string MockRootNodeName = "VO_BAN_TIN";
        private static readonly IReadOnlyList<string> MockBanTinNames = new List<string>
        {
            "BAN_TIN_09H00",
            "BAN_TIN_12H00",
            "BAN_TIN_19H00",
            "BAN_TIN_23H00"
        };

        public void LoadMockDataForTreeView()
        {
            isMockMode = true;

            treeView1.BeginUpdate();
            try
            {
                treeView1.Nodes.Clear();
                treeView1.CheckBoxes = true;

                var rootNode = new TreeNode(MockRootNodeName)
                {
                    Tag = MockRootNodeName
                };

                foreach (string banTinName in MockBanTinNames)
                {
                    var childNode = new TreeNode(banTinName)
                    {
                        Tag = $"{MockRootNodeName}.{banTinName}"
                    };

                    rootNode.Nodes.Add(childNode);
                }

                treeView1.Nodes.Add(rootNode);
                rootNode.Expand();
            }
            finally
            {
                treeView1.EndUpdate();
            }

            tbl = null;
            Content = string.Empty;
            dataGridView1.DataSource = null;

            toolStripStatusLabel1.Text = "Đang sử dụng mock data cho TreeView.";
            toolStripStatusLabel2.Text = MockRootNodeName;
            label2.Text = MockRootNodeName;
        }

        public DataTable CreateMockDataTable(string banTinName)
        {
            string shortName = string.IsNullOrWhiteSpace(banTinName)
                ? MockRootNodeName
                : banTinName.Split('.').Last();

            var table = new DataTable();
            table.Columns.Add("page-number", typeof(string));
            table.Columns.Add("Content", typeof(string));

            var rows = new List<(string Page, string Content)>
            {
                ("01", BuildContentBlock("HÀ NỘI", "Chủ tịch nước tiếp đoàn đại biểu", $"Bản tin {shortName}: điểm nhấn đầu tiên trong ngày.")),
                ("02", BuildContentBlock("TP HỒ CHÍ MINH", "Thủ tướng làm việc với các bộ ngành.", "Đây là nội dung tin tức mẫu số 2.")),
                ("03", BuildContentBlock("ĐÀ NẴNG", "Khai mạc hội nghị phát triển kinh tế biển.", $"Phóng sự đặc biệt của {shortName}.")),
                ("04", BuildContentBlock("CẦN THƠ", "Các địa phương đẩy mạnh chuyển đổi số.", "Tin ngắn tổng hợp cuối bản tin."))
            };

            foreach (var row in rows)
            {
                DataRow dataRow = table.NewRow();
                dataRow["page-number"] = row.Page;
                dataRow["Content"] = row.Content;
                table.Rows.Add(dataRow);
            }

            return table;
        }

        private static string BuildContentBlock(string diaDanh, string cgContent, string description)
        {
            string[] lines =
            {
                "##DD:",
                diaDanh,
                "##CG:",
                cgContent,
                "##",
                string.Empty,
                description
            };

            return string.Join(Environment.NewLine, lines);
        }

        private void btnLoadMockData_Click(object sender, EventArgs e)
        {
            LoadMockDataForTreeView();
        }
    }
}
