using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace News2025.Services
{
    public class XmlStoryService
    {
        // Đọc tất cả các file XML trong thư mục được chỉ định
        public List<XDocument> LoadXmlStories(string folderPath)
        {
            var xmlDocs = new List<XDocument>();

            if (!Directory.Exists(folderPath))
                return xmlDocs;

            // Lấy danh sách file và sắp xếp theo tên
            var files = Directory.EnumerateFiles(folderPath, "*.xml")
                                 .OrderBy(file => ExtractNumberFromFileName(Path.GetFileName(file)))
                                 .ToArray();

            Console.WriteLine($"Tìm thấy {files.Length} file XML trong thư mục {folderPath}");

            foreach (var file in files)
            {
                try
                {
                    var doc = XDocument.Load(file);
                    xmlDocs.Add(doc);
                    Console.WriteLine($"Đã đọc file: {file}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi đọc file {file}: {ex.Message}");
                }
            }

            return xmlDocs;
        }
        private int ExtractNumberFromFileName(string fileName)
        {
            var match = System.Text.RegularExpressions.Regex.Match(fileName, @"\d+");
            return match.Success ? int.Parse(match.Value) : int.MaxValue;
        }

        // Trích xuất các dòng CG từ tài liệu XML
        public List<string> ExtractCgLines(XDocument doc)
        {
            var cgLines = new List<string>();

            if (doc == null) return cgLines;

            // Lấy tất cả các phần tử <cc> trong body
            var ccElements = doc.Descendants().Where(e => e.Name.LocalName == "cc").ToList();

            bool isInsideCgBlock = false;

            foreach (var cc in ccElements)
            {
                var value = cc.Value.Trim();

                if (value == "##CG")
                {
                    isInsideCgBlock = true;
                    continue;
                }
                else if (value == "##")
                {
                    isInsideCgBlock = false;
                    continue;
                }

                if (isInsideCgBlock && !string.IsNullOrWhiteSpace(value))
                {
                    cgLines.Add(value);
                }
            }

            return cgLines;
        }
        public List<List<string>> ExtractAllCgBlocks(XDocument doc)
        {
            var allCgBlocks = new List<List<string>>();

            if (doc == null) return allCgBlocks;

            // Lấy tất cả các phần tử <cc> trong body
            var ccElements = doc.Descendants().Where(e => e.Name.LocalName == "cc").ToList();

            List<string> currentCgBlock = null;
            foreach (var cc in ccElements)
            {
                var value = cc.Value.Trim();

                if (value == "##CG" || value == "##CG:")
                {
                    // Bắt đầu một khối CG mới
                    currentCgBlock = new List<string>();
                    allCgBlocks.Add(currentCgBlock);
                }
                else if (value == "##")
                {
                    // Kết thúc khối CG hiện tại
                    currentCgBlock = null;
                }
                else if (currentCgBlock != null && !string.IsNullOrWhiteSpace(value))
                {
                    // Thêm nội dung vào khối CG hiện tại
                    currentCgBlock.Add(value);
                }
            }

            return allCgBlocks;
        }
        public static void HandleLTSelection(
        int rowIndex,
        DataGridView dgvLT,
        DataGridView dgvName,
        TextBox txtLine1,
        TextBox txtLine2,
        TextBox txtLine1Name,
        TextBox txtLine2Name,
        Dictionary<int, List<List<string>>> storyCgData,
        Action<string> reportStatus
    )
        {
            if (rowIndex < 0 || rowIndex >= dgvLT.Rows.Count)
                return;

            var selectedRow = dgvLT.Rows[rowIndex];
            txtLine1.Text = selectedRow.Cells["line1"]?.Value?.ToString() ?? string.Empty;
            txtLine2.Text = selectedRow.Cells["line2"]?.Value?.ToString() ?? string.Empty;

            reportStatus?.Invoke($"Dòng {rowIndex + 1} được chọn.");

            if (storyCgData.TryGetValue(rowIndex, out var cgBlocks))
            {
                dgvName.Rows.Clear();

                foreach (var cgBlock in cgBlocks)
                {
                    string line1 = cgBlock.Count > 0 ? cgBlock[0] : string.Empty;
                    string line2 = cgBlock.Count > 1 ? cgBlock[1] : string.Empty;
                    dgvName.Rows.Add(line1, line2);
                }
            }

            LoadTxtLineForDgvName(dgvName, txtLine1Name, txtLine2Name);
        }

        public static void LoadTxtLineForDgvName(DataGridView dgvName, TextBox txt1, TextBox txt2)
        {
            if (dgvName.CurrentRow != null)
            {
                var selectedRow = dgvName.CurrentRow;
                txt1.Text = selectedRow.Cells["line1name"]?.Value?.ToString() ?? string.Empty;
                txt2.Text = selectedRow.Cells["line2name"]?.Value?.ToString() ?? string.Empty;
            }
            else
            {
                txt1.Text = string.Empty;
                txt2.Text = string.Empty;
            }
        }
        public static void LoadTextBoxFromDgv(DataGridView dgvName, TextBox txt1, TextBox txt2)
        {
            if (dgvName.CurrentRow != null)
            {
                var selectedRow = dgvName.CurrentRow;

                txt1.Text = selectedRow.Cells.Count > 0 && selectedRow.Cells[0].Value != null
                    ? selectedRow.Cells[0].Value.ToString()
                    : string.Empty;

                txt2.Text = selectedRow.Cells.Count > 1 && selectedRow.Cells[1].Value != null
                    ? selectedRow.Cells[1].Value.ToString()
                    : string.Empty;
            }
            else
            {
                txt1.Text = string.Empty;
                txt2.Text = string.Empty;
            }
        }

    }
}
