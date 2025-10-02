using News2025.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace News2025
{
    public class News2025Logic
    {
        private readonly IAccessService _accessService;
        private readonly IKarismaCG3Model _karismaCG3;

        public Dictionary<string, int> Layers { get; private set; } = new Dictionary<string, int>();
        public Dictionary<string, string> LocationMap { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> PopupMap { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> LogoMap { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> UtcMap { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, (string Description1, string Name2, string Description2)> PhongVanBangMap { get; private set; } = new Dictionary<string, (string, string, string)>();
        public Dictionary<string, (string Description1, string Description2)> PhongVanLechMap { get; private set; } = new Dictionary<string, (string, string)>();
        public Dictionary<string, (string Description1, string Name2, string Description2, string Name3, string Description3)> PhongVan3NguoiMap { get; private set; } = new Dictionary<string, (string, string, string, string, string)>();

        public string WorkingFolder { get; private set; }
        public string XmlPath { get; private set; }

        private readonly string[] _layerNames = new[]
        {
            "Location", "Popup", "Logo", "TroiNgang", "TroiTinTuc",
            "ChuKet", "LowerThird", "ThucHien", "BinhLuan", "PhongVanBang", "PhongVanLech", "PhongVan3",
            "Breaking", "Live", "Transition", "Hotline", "TimeUTC", "Clock"
        };

        public News2025Logic(IAccessService accessService, IKarismaCG3Model karismaCG3)
        {
            _accessService = accessService;
            _karismaCG3 = karismaCG3;
        }

        public void Initialize()
        {
            QueryGeneral();
        }

        public void QueryGeneral()
        {
            try
            {
                var configService = new ConfigService();
                XmlPath = configService.GetConfigValue("CONNECT", "XML");
                WorkingFolder = configService.GetConfigValue("CONNECT", "WORKINGFOLDER");

                foreach (var layerName in _layerNames)
                {
                    int? layerValue = _accessService.QueryIntColumnValue("General", "Name", "Layer", layerName);
                    Layers[layerName] = layerValue ?? 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error querying data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadDataToComboBox(string tableName, ComboBox comboBox, Dictionary<string, string> map)
        {
            var data = _accessService.GetNameAndDescriptionFromTable(tableName);

            comboBox.Items.Clear();
            map.Clear();

            foreach (var item in data)
            {
                comboBox.Items.Add(item.Name);
                map[item.Name] = item.Description;
            }

            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = 0;
        }

        public void PlayScene(string sceneName, string layerName, string name, string description)
        {
            if (!Layers.TryGetValue(layerName, out int layer))
            {
                MessageBox.Show($"Không tìm thấy layer cho {layerName}!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string scenePath = WorkingFolder + _accessService.QueryColumnValue("General", "Name", "Scene", sceneName);

            if (string.IsNullOrEmpty(scenePath))
            {
                MessageBox.Show($"Không tìm thấy đường dẫn scene cho {sceneName}!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _karismaCG3.PlaySceneNameDes(scenePath, layer, name, description);
        }
    }
}
