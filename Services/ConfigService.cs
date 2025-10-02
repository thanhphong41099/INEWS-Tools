using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace News2025.Services
{
    public class ConfigService
    {
        private readonly string _configFilePath;

        public ConfigService()
        {

        }

        public static NameValueCollection GetConfigSection(string sectionName)
        {
            try
            {
                ConfigurationManager.RefreshSection(sectionName);

                return (NameValueCollection)ConfigurationManager.GetSection(sectionName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đọc phần cấu hình '{sectionName}': {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
