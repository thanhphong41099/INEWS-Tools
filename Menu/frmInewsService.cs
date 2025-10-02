using News2025.InewsServiceReference;
using News2025.TomcatSystem;
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
    public partial class frmInewsService : Form
    {
        //private News2025.InewsServiceReference.INEWSSystemService client;
        private News2025.TomcatSystem.INEWSSystem client;

        public frmInewsService()
        {
            InitializeComponent();
            //client = new News2025.InewsServiceReference.INEWSSystemService();
            client = new News2025.TomcatSystem.INEWSSystem();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            client = new News2025.TomcatSystem.INEWSSystem();
            client.Url = "http://192.88.8.230:8080/inewswebservice/services/inewssystem";

            try
            {
                var request = new News2025.TomcatSystem.ConnectType
                {
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    Servername = txtServer.Text
                };

                TomcatSystem.ConnectResponseType response = client.Connect(request);

                MessageBox.Show("✅ Connection successful! Extension: " + response.Extension);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Connection failed: " + ex.Message);
            }
        }


        private void frmInewsService_Load(object sender, EventArgs e)
        {
            txtUsername.Text = "root";
            txtPassword.Text = "root";
            txtServer.Text = "inewsServer";
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            //string result = client.Disconnect();
            //MessageBox.Show("✅ Ngắt kết nối thành công! " + result);
        }
    }
}
