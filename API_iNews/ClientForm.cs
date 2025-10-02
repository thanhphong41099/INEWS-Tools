using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TTDH;
using System.IO;

namespace API_iNews
{
    public partial class ClientForm : Form
    {
        
        public ClientForm()
        {
            InitializeComponent();
        }
        string serverIP = "";
        //string QUEUEROOT = "";
        ClientAPI client = null;
        private void button1_Click(object sender, EventArgs e)
        {
            string arr = client.GetDataFromCmd("GET_TREE");
            if (arr.IndexOf("ERROR")==0)
                MessageBox.Show(arr);
            else
            {
                string[] queues = arr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                //TreeNode root = new TreeNode();
                //root.Text = "INEWS." + QUEUEROOT;                
                //foreach (string s in queues)
                //{
                //    TreeNode note = new TreeNode();
                //    note.Text = s;
                //    note.Tag = s;
                //    root.Nodes.Add(note);
                //}
                //treeView1.Nodes.Add(root);
                //treeView1.ExpandAll();

                //{VO_BAN_TIN:|BAN_TIN_12H|BAN_TIN_14H|}
                //{VO_BAN_TIN:|BAN_TIN_12H|BAN_TIN_14H|}{CHUYEN_MUC:|NGUOI_LAO_DONG|BAN_TIN_14H|}   
                treeView1.Nodes.Clear();                        
                TreeNode root = null;                
                string noteRootName = "";
                foreach (string s in queues)
                {
                    if (s.Length > 3)
                    {
                        if (s.IndexOf("{") >= 0)
                        {
                            root = new TreeNode();
                            noteRootName = s.Replace("{", "").Replace(":", "");
                            root.Text = noteRootName;
                            treeView1.Nodes.Add(root);
                        }
                        else
                        {
                            TreeNode note = new TreeNode();
                            note.Text = s;
                            note.Tag = noteRootName + "." + s.Trim()+ ".RUNDOWN";
                            root.Nodes.Add(note);
                        }
                    }
                }
                treeView1.ExpandAll();
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            serverIP = System.Configuration.ConfigurationManager.AppSettings["ServerIP"];
            //QUEUEROOT = System.Configuration.ConfigurationManager.AppSettings["QueuesRoot"];
            client = new ClientAPI(serverIP);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if (node != null && node.Tag != null)
            {
                //string name = "QUEUE|"+QUEUEROOT + "." + node.Tag;
                string name = "QUEUE|"+ node.Tag.ToString();
                label1.Text = name;
                string arr = client.GetDataFromCmd(name);                
                if (arr.IndexOf("ERROR") == 0)
                    MessageBox.Show(arr);
                else
                {
                    StringReader theReader = new StringReader(arr);                   
                    DataTable tbl = new DataTable();
                    tbl.ReadXml(theReader);
                    dataGridView1.DataSource = tbl;
                }
               
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount == 0)
                return;
            DataGridViewRow r = dataGridView1.Rows[e.RowIndex];
            if (r != null)
            {
                int rowIdx = e.RowIndex;
                if (r.Cells["Content"].Value != null)
                    if(checkBox1.Checked)
                        textBox1.Text = r.Cells["Content"].Value.ToString().Replace("\r\n", Environment.NewLine).Replace("\n", Environment.NewLine);
                    else
                        textBox1.Text = r.Cells["Content"].Value.ToString().Replace("\n", Environment.NewLine);

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
