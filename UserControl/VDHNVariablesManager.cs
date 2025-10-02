using System.Collections.Generic;

namespace News2025
{
    public class VDHNVariablesManager
    {
        public Dictionary<string, int> Layers { get; private set; } = new Dictionary<string, int>();

        public Dictionary<string, string> LocationMap { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> PopupMap { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> LogoMap { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> UtcMap { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, (string Description1, string Name2, string Description2)> PhongVanBangMap { get; private set; } = new Dictionary<string, (string, string, string)>();
        public Dictionary<string, (string Description1, string Description2)> PhongVanLechMap { get; private set; } = new Dictionary<string, (string, string)>();
        public Dictionary<string, (string Description1, string Name2, string Description2, string Name3, string Description3)> PhongVan3NguoiMap { get; private set; } = new Dictionary<string, (string, string, string, string, string)>();

        public Dictionary<int, List<List<string>>> StoryCgData { get; private set; } = new Dictionary<int, List<List<string>>>();
        public string WorkingFolder { get; set; }
        public string XmlPath { get; set; }

        public readonly string[] LayerNames = new[]
        {
            "Location", "Popup", "Logo", "TroiNgang", "TroiTinTuc",
            "ChuKet", "LowerThird", "ThucHien", "BinhLuan", "PhongVanBang", "PhongVanLech", "PhongVan3",
            "Breaking", "Live", "Transition", "Hotline", "TimeUTC", "Clock"
        };

        public void InitializeLayers(Dictionary<string, int> layers)
        {
            Layers = layers;
        }
    }
}
