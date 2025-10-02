using System.Collections.Generic;
using System.Windows.Forms;
using News2025.Services;

namespace News2025.Logic
{
    public class ComboBoxDataLoader
    {
        private readonly IAccessService _accessService;

        public ComboBoxDataLoader(IAccessService accessService)
        {
            _accessService = accessService;
        }

        public void LoadDataToComboBox(string tableName, ComboBox comboBox, Dictionary<string, string> map)
        {
            var data = _accessService.GetNameAndDescriptionFromTable(tableName);
            comboBox.Items.Clear();
            map.Clear();

            // Add empty item at the beginning
            comboBox.Items.Add(string.Empty);
            map[string.Empty] = string.Empty;

            foreach (var item in data)
            {
                comboBox.Items.Add(item.Name);
                map[item.Name] = item.Description;
            }

            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = 0;
        }

        public void LoadPhongVanBang(ComboBox cbbPVB, Dictionary<string, (string, string, string)> map)
        {
            var data = _accessService.GetPhongVanBangData();
            cbbPVB.Items.Clear();
            map.Clear();

            foreach (var item in data)
            {
                cbbPVB.Items.Add(item.Name1);
                map[item.Name1] = (item.Description1, item.Name2, item.Description2);
            }

            if (cbbPVB.Items.Count > 0)
                cbbPVB.SelectedIndex = 0;
        }

        public void LoadPhongVanLech(ComboBox cbbPVL, Dictionary<string, (string, string)> map)
        {
            var data = _accessService.GetPhongVanLechData();
            cbbPVL.Items.Clear();
            map.Clear();

            foreach (var item in data)
            {
                cbbPVL.Items.Add(item.Name1);
                map[item.Name1] = (item.Description1, item.Description2);
            }

            if (cbbPVL.Items.Count > 0)
                cbbPVL.SelectedIndex = 0;
        }

        public void LoadPhongVan3Nguoi(ComboBox cbbPV3, Dictionary<string, (string, string, string, string, string)> map)
        {
            var data = _accessService.GetPhongVan3NguoiData();
            cbbPV3.Items.Clear();
            map.Clear();

            foreach (var item in data)
            {
                cbbPV3.Items.Add(item.Name1);
                map[item.Name1] = (item.Description1, item.Name2, item.Description2, item.Name3, item.Description3);
            }

            if (cbbPV3.Items.Count > 0)
                cbbPV3.SelectedIndex = 0;
        }
    }
}
