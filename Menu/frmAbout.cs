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
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();

            try
            {
                string licenseKey = LicenseKeyHandler.readLicenseLocalFile();
                LicenseKeyHandler.writeLicenseLocalFile(licenseKey);
                string expirationDate = LicenseKeyHandler.onGetValueOfLicenseByKey(licenseKey, "expirationDate");
                DateTimeOffset dateOfExpired = LicenseKeyHandler.onGetExpirationDate(expirationDate);
                string expirationDate2 = dateOfExpired.Date.ToString("dd/MM/yyyy");


                txtAbout.Text =
                    $"CGNews Version 2.0.0. \n" +
                    $"Expiration: ({expirationDate2})\n" +
                    $"Copyright © VTVBroadcom MS.\n" +
                    $"All rights reserved http://vtvms.vn.\n\n" +
                    $"--------------------------------------------------";
            }
            catch
            {
                txtAbout.Text =
                $"CGNews Version 2.0.0. \n" +
                $"Ex: No License Key. Contact us. \n" +
                $"Copyright © VTVBroadcom MS.\n" +
                $"All rights reserved http://vtvms.vn\n\n." +
                $"--------------------------------------------------";
            }     
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {

        }
    }
}
