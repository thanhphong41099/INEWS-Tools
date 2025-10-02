using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace News2025.Services
{
    public class LayoutConfig
    {
        public string Name { get; set; }
        public string BarIn { get; set; }
        public string BarOut { get; set; }
        public string DiaDanh { get; set; }
        public string Troi { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public int? DelayIn { get; set; }
        public int? DelayOut { get; set; }
        public int? Red { get; set; }
        public int? Green { get; set; }
        public int? Blue { get; set; }
    }


    public class LayoutManager
    {
        private readonly FlowLayoutPanel flowPanel;
        private readonly Label labelName;
        private readonly string indexFilePath;

        public List<string> LayoutPaths { get; private set; } = new List<string>();
        public List<LayoutConfig> LayoutConfigs { get; private set; } = new List<LayoutConfig>();
        public LayoutConfig CurrentConfig { get; private set; }

        public LayoutManager(FlowLayoutPanel flowPanel, Label labelName, string indexFilePath)
        {
            this.flowPanel = flowPanel;
            this.labelName = labelName;
            this.indexFilePath = indexFilePath;
        }

        public void AddLayout(LayoutConfig config, string sourcePath, bool select = true)
        {
            // UI
            var panel = new Panel
            {
                Width = flowPanel.Width - 5,
                Height = 30,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(0),
                Margin = new Padding(2)
            };

            var radio = new RadioButton
            {
                Text = config.Name,
                AutoSize = true,
                Tag = config,
                Location = new Point(5, 5)
            };
            radio.CheckedChanged += (s, e) =>
            {
                if (radio.Checked)
                {
                    // Bỏ chọn các radio khác
                    foreach (Control ctrl in flowPanel.Controls)
                    {
                        if (ctrl is Panel p)
                        {
                            var r = p.Controls.OfType<RadioButton>().FirstOrDefault();
                            if (r != null && r != radio)
                                r.Checked = false;
                        }
                    }

                    CurrentConfig = config;
                    labelName.Text = $"CHƯƠNG TRÌNH CG {config.Name}";
                }
            };

            var btnDelete = new Button
            {
                Text = "✖",
                Width = 30,
                Height = 25,
                Tag = panel,
                BackColor = Color.DarkGray,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Location = new Point(panel.Width - btnDelete.Width - 5, 2);
            panel.Resize += (s, e) =>
            {
                btnDelete.Location = new Point(panel.Width - btnDelete.Width - 5, 2);
            };

            btnDelete.Click += (s, e) => RemoveLayout(panel, config, sourcePath);

            panel.Controls.Add(radio);
            panel.Controls.Add(btnDelete);
            flowPanel.Controls.Add(panel);

            LayoutConfigs.Add(config);
            //Chỉ thêm nếu chưa tồn tại
            if (!LayoutPaths.Any(p => string.Equals(p.Trim(), sourcePath.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                LayoutPaths.Add(sourcePath);
                SaveLayoutIndex();
            }


            if (select)
            {
                foreach (Control c in flowPanel.Controls)
                {
                    if (c is Panel p)
                    {
                        var r = p.Controls.OfType<RadioButton>().FirstOrDefault();
                        if (r != null) r.Checked = false;
                    }
                }
                radio.Checked = true;
            }
        }

        public void LoadLayouts()
        {
            if (!File.Exists(indexFilePath)) return;

            try
            {
                string json = File.ReadAllText(indexFilePath);
                var allPaths = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();

                // Xoá trùng đường dẫn
                LayoutPaths = allPaths.Distinct().ToList();

                foreach (var path in LayoutPaths.ToList()) // Copy để tránh lỗi khi Remove
                {
                    try
                    {
                        if (File.Exists(path))
                        {
                            string content = File.ReadAllText(path);
                            var config = JsonConvert.DeserializeObject<LayoutConfig>(content);
                            if (config != null)
                            {
                                AddLayout(config, path, select: false);
                            }
                            else
                            {
                                MessageBox.Show($"File cấu hình không hợp lệ:\n{path}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                LayoutPaths.Remove(path);
                            }
                        }
                        else
                        {
                            // File không tồn tại → bỏ khỏi danh sách
                            LayoutPaths.Remove(path);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi load file: {path}\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LayoutPaths.Remove(path);
                    }
                }

                // Lưu lại danh sách đã dọn sạch
                SaveLayoutIndex();

                // Tự động chọn radio đầu tiên
                var firstRadio = flowPanel.Controls
                    .OfType<Panel>()
                    .Select(p => p.Controls.OfType<RadioButton>().FirstOrDefault())
                    .FirstOrDefault(r => r != null);

                if (firstRadio != null)
                    firstRadio.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load layout: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveLayoutIndex()
        {
            try
            {
                File.WriteAllText(indexFilePath, JsonConvert.SerializeObject(LayoutPaths, Formatting.Indented));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu layouts_index.json: " + ex.Message);
            }
        }

        private void RemoveLayout(Panel panel, LayoutConfig configToRemove, string sourcePath)
        {
            var confirmResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa chương trình \"{configToRemove.Name}\" không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
                return;

            LayoutConfigs.Remove(configToRemove);

            // So sánh chính xác để xóa đúng file trong index
            LayoutPaths.RemoveAll(p => string.Equals(p.Trim(), sourcePath.Trim(), StringComparison.OrdinalIgnoreCase));

            flowPanel.Controls.Remove(panel);
            panel.Dispose();

            SaveLayoutIndex(); // Ghi lại danh sách đã xóa

            // Xử lý UI
            if (CurrentConfig == configToRemove)
            {
                var firstRadio = flowPanel.Controls
                    .OfType<Panel>()
                    .Select(p => p.Controls.OfType<RadioButton>().FirstOrDefault())
                    .FirstOrDefault(r => r != null);

                if (firstRadio != null && firstRadio.Tag is LayoutConfig newConfig)
                {
                    firstRadio.Checked = true;
                    CurrentConfig = newConfig;
                    labelName.Text = $"CHƯƠNG TRÌNH CG {newConfig.Name}";
                }
                else
                {
                    CurrentConfig = null;
                    labelName.Text = "CHƯƠNG TRÌNH CG";
                }
            }
        }

        public void ResetSelection()
        {
            foreach (Control control in flowPanel.Controls)
            {
                if (control is Panel p)
                {
                    var r = p.Controls.OfType<RadioButton>().FirstOrDefault();
                    if (r != null) r.Checked = false;
                }
            }

            CurrentConfig = null;
            labelName.Text = "CHƯƠNG TRÌNH CG";
        }

    }
}
