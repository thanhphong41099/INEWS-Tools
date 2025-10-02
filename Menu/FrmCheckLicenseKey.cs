using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace News2025.Menu
{
    public partial class frmCheckLicenseKey : Form
    {
        public frmCheckLicenseKey()
        {
            InitializeComponent();
            txtProductID.Text = LicenseKeyHandler.onGetProductId();
        }

        private void checkKey_Click(object sender, EventArgs e)
        {
            string licenseKey = txtLicenseKey.Text;

            if (string.IsNullOrWhiteSpace(licenseKey))
            {
                MessageBox.Show("Không thể xử lý với key rỗng. Xin hãy nhập key!");
            }

            if (LicenseKeyHandler.onCheckLicenseKeyIsValid(licenseKey, false))
            {
                LicenseKeyHandler.writeLicenseLocalFile(licenseKey);
                string expirationDate = LicenseKeyHandler.onGetValueOfLicenseByKey(licenseKey, "expirationDate");
                DateTimeOffset dateOfExpired = LicenseKeyHandler.onGetExpirationDate(expirationDate);
                MessageBox.Show("Kiểm tra key bản quyền thành công! Hạn sử dụng: " + dateOfExpired.Date.ToString("dd/MM/yyyy"));
                this.Close();
            }
        }
    }
}
