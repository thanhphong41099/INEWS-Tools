using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TTDH;

namespace API_iNews
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }
        ServerAPI server = null;

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start Server")
            {
                button1.Text = "Running.....";
                server.Start();
                this.WindowState = FormWindowState.Minimized;
               
            }
            else
            {
                button1.Text = "Start Server";
                server.Stop();
            }
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            string serverIP = System.Configuration.ConfigurationManager.AppSettings["ServerIP"];
            label1.Text = "Server IP: " + serverIP;
            server = new ServerAPI(serverIP);
            server.Recieve += new ServerAPI.RecieveEvent(server_Error);
            button1_Click(null, null);
            button2_Click(null, null);
        }

        void server_Error(string msg)
        {
            label1.Text = msg;
            //MessageBox.Show(msg);
           // notifyIcon1.Text=notifyIcon1.BalloonTipTitle = msg;
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show(
                "Việc tắt chương trình sẽ dừng server API iNews\n Bạn có muốn tắt chương trình này không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                server.Stop(); // Dừng server nếu người dùng chọn Yes
            }
            else
            {
                e.Cancel = true; // Hủy hành động đóng form nếu người dùng chọn No
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            API api = new API();
            //APIV4 api = new APIV4();
            //APIV42017 api = new APIV42017();
            api.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            exitToolStripMenuItem_Click(null, null);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ClientForm clientF = new ClientForm();
            clientF.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClientForm clientF = new ClientForm();
            clientF.ShowDialog();
        }

        private void hiệnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(hiệnToolStripMenuItem.Text=="Hiện")
            {
                this.Show();
                hiệnToolStripMenuItem.Text = "Ẩn";
            }
            else
            {
                this.Hide();
                hiệnToolStripMenuItem.Text = "Hiện";
            }
        }
    }
}
