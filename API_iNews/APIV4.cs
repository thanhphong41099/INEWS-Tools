using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TTDH;

namespace API_iNews
{
    public partial class APIV4 : Form
    {
        public APIV4()
        {
            InitializeComponent();
        }
        string QUEUEROOT = "CHO_DUYET_PHONG";
        string queueChild = "";
        iNewsData iData = null;
        WebServiceSettings settings = new WebServiceSettings();
        BarType CurrentBarType;
        BarTypeCollection barCollection = new BarTypeCollection();
        string currentBarName = "Bar1";
        private void button1_Click(object sender, EventArgs e)
        {
            List<string> queues = iData.ChangedQueues();
            Display(queues);
        }

        private void API_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(iData!=null && iData.IsConnect())
                iData.Disconnect();
        }
        private void Display(List<string> queues)
        {
            if (queues != null)
            {
                string str = "";
                label1.Text = "";
                foreach (string s in queues)
                {
                    str += s + "\n";
                }
                label1.Text = str;
            }
        }
        private void API_Load(object sender, EventArgs e)
        {
            QUEUEROOT = System.Configuration.ConfigurationManager.AppSettings["QueuesRoot"];
            queueChild = System.Configuration.ConfigurationManager.AppSettings["QueuesChild"];
            settings.ServerName = System.Configuration.ConfigurationManager.AppSettings["iNewsServer"];
            settings.ServerBackup = System.Configuration.ConfigurationManager.AppSettings["iNewsServerBackup"];
            settings.UserName = System.Configuration.ConfigurationManager.AppSettings["iNewsUser"];
            settings.Password = System.Configuration.ConfigurationManager.AppSettings["iNewsPass"];
            iData = new iNewsData(settings);
            iData.SentError += new iNewsData.SendError(iData_SentError);
            //comboBox1.SelectedIndex = 0;
            currentBarName = "Bar1";                        
            CurrentBarType = barCollection.GetBar(currentBarName);
            comboBox2.Items.Clear();          
            comboBox2.DataSource = barCollection.BarTypes;
            comboBox2.ValueMember = "Name";
            comboBox2.DisplayMember = "Display";

        }

        void iData_SentError(string msg)
        {
            toolStripStatusLabel1.Text = msg;
        }
        public void LoadTreeStories(TreeView tree, string nodeNames)
        {
            string[] parentNote = Utility.Split(nodeNames, ";");

            foreach (string rootName in parentNote)
            {
                if (string.IsNullOrEmpty(rootName))
                    continue;
                TreeNode root = new TreeNode();               
                root.Text = rootName;
                List<string> queues = iData.GetFolderChildren(root.Text);
                if (queues.Count == 0)
                    toolStripStatusLabel1.Text = "Cannot load the tree of " + rootName;
                foreach (string s in queues)
                {
                    TreeNode note = new TreeNode();
                    note.Text = s;
                    note.Tag = root.Text + "." + s;
                    root.Nodes.Add(note);
                }
                tree.Nodes.Add(root);
            }
            tree.ExpandAll();
        }

        private void LoadTreeQueues()
        {
            LoadTreeStories(treeView1, QUEUEROOT);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if (node != null && node.Tag != null)
            {
                //string name = QUEUEROOT + "." + node.Tag;

                string name = node.Tag.ToString().Replace(txtiNewsTreeRoot.Text + ".", "");
                if (!string.IsNullOrEmpty(queueChild))
                    name = name + "." + queueChild;
                toolStripStatusLabel2.Text = "Load story of " + name;
                List<string> queues = iData.GetStoriesBoard(name);
                string mapping = System.Configuration.ConfigurationManager.AppSettings["Fields"];
                ProcessingXMl2Class process = new ProcessingXMl2Class();
                process.FieldMapping = mapping;
                bool isExpXML = System.Configuration.ConfigurationManager.AppSettings["ExportStory2XML"] == "1";
                if (isExpXML)
                {
                    ExportXML(queues);
                }
                DataTable tbl = process.GetDataRows(queues);
                dataGridView1.DataSource = tbl;
            }
        }

        private void ExportXML(List<string> queues)
        {
            if (queues == null || queues.Count <= 0)
                return;
            string folder = System.Configuration.ConfigurationManager.AppSettings["FolderToSave"];
            int a = 1;
            foreach (string s in queues)
            {
                if (!string.IsNullOrEmpty(s))
                {

                    System.IO.File.WriteAllText(folder + "\\story_" + a.ToString() + ".xml", s, Encoding.Unicode);
                    //
                }
                a++;               
            }
            toolStripStatusLabel1.Text = "Đã xuất XML story thành công.";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            List<string> strChange = iData.ChangedQueues();
            Display(strChange);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable tbl = (DataTable)dataGridView1.DataSource;

            if (tbl != null && tbl.Rows.Count > 0)
            {
                string strEx = "";
                string folder = System.Configuration.ConfigurationManager.AppSettings["FolderToSave"];
                foreach (DataRow r in tbl.Rows)
                {
                    strEx += "[]";
                    strEx += Environment.NewLine;
                    strEx += r["title"].ToString() + Environment.NewLine;
                    strEx += "#";
                    strEx += Environment.NewLine;
                    strEx += r["vtv-nthien"].ToString() + Environment.NewLine;
                    strEx += Environment.NewLine;
                }
                if (!string.IsNullOrEmpty(strEx))
                {
                    System.IO.File.WriteAllText(folder + "\\ruttit.txt", strEx, Encoding.Unicode);
                    MessageBox.Show("Da xuat file thanh cong.");
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            bool isConnect = iData.IsConnect();
            if (isConnect)
            {
                btnConnect.BackColor = Color.Green;
                toolStripStatusLabel3.Text = "Conneted to server " + settings.ServerName;
                btnLoadTree.Enabled = true;
            }
            else
            {
                btnConnect.BackColor = Color.Red;
                toolStripStatusLabel3.Text = "Cannot conneted to server " + settings.ServerName;
                btnLoadTree.Enabled = false;
            }
        }

        private void btnLoadTree_Click(object sender, EventArgs e)
        {
            LoadTreeQueues();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            iData.Disconnect();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if (node != null && node.Tag != null)
            {
                string name = node.Tag.ToString().Replace(txtiNewsTreeRoot.Text + ".", "");
                if (!string.IsNullOrEmpty(queueChild))
                    name = name + "." + queueChild;
                toolStripStatusLabel2.Text = "Load story of " + name;
                List<string> queues = iData.GetStoriesBoard(name);
                string mapping = System.Configuration.ConfigurationManager.AppSettings["Fields"];
                ProcessingXMl2Class process = new ProcessingXMl2Class();
                process.FieldMapping = mapping;
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.ShowNewFolderButton = true;
                dlg.ShowDialog();
                if (System.IO.Directory.Exists(dlg.SelectedPath))
                    ExportXML(queues, dlg.SelectedPath);
            }
        }

        private void ExportXML(List<string> queues, string selectedPath)
        {
            if (queues == null || queues.Count <= 0)
                return;
            int a = 1;
            foreach (string s in queues)
            {
                if (!string.IsNullOrEmpty(s))
                {

                    System.IO.File.WriteAllText(selectedPath + "\\story_" + a.ToString() + ".xml", s, Encoding.Unicode);
                }
                a++;

            }
            toolStripStatusLabel1.Text = "Đã xuất XML story thành công.";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string mapping = System.Configuration.ConfigurationManager.AppSettings["Fields"];
            ProcessingXMl2Class process = new ProcessingXMl2Class();
            process.FieldMapping = mapping;
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            dlg.ShowDialog();
            if (System.IO.Directory.Exists(dlg.SelectedPath))
            {
                DataTable tbl = process.GetDataRows(dlg.SelectedPath);
                dataGridView1.DataSource = tbl;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount == 0)
                return;
            DataGridViewRow r = dataGridView1.Rows[e.RowIndex];
            if (r != null)
            {
                int rowIdx = e.RowIndex;
                if (r.Cells["OradPlugins"].Value != null)
                    if (checkBox1.Checked)
                        label4.Text = GetCGText(r.Cells["OradPlugins"].Value.ToString().Replace("\r\n", Environment.NewLine));
                    else
                        label4.Text = r.Cells["OradPlugins"].Value.ToString().Replace("\n", Environment.NewLine);

            }
        }

        private string GetCGText(string xmlString)
        {
            string pattern = "<mosAbstract>(.*?)</mosAbstract>";
            MatchCollection lst = Regex.Matches(xmlString, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            string str = "";
            if (lst.Count == 0)
                return str;
            foreach (Match s in lst)
            {
                string result = s.ToString();
                //B1. Ta lay duoc dinh dang (Ten plugin cua orad) Gia tri text
                result = Regex.Replace(result, pattern, "$1", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                //B2. Ta chia 1 dong/2 dong dua vao <GIA TRI TEXT> co dau ( ;
                //phan biet 1/2 dong bang dau /
                result = GetTextinLines(result);
                //B3: Lay duoc CG text
                str += result + Environment.NewLine;
            }
            return str;
        }

        //Chua phan biet dau la PHU DE, TITLE, DIA DANH nhe
        private string GetTextinLines(string strText)
        {
            string str = strText.Trim();
            int idx = str.IndexOf("(");
            //Neu ko co dau ( tuc la hieu ung OUT
            if (idx < 0)
                return "";
            else //Nguoc lai
            {
                //Cat tien to <TEN_THANH_BAR> dang (xxx)
                int idx2 = str.IndexOf(")");
                string bar_name = str.Substring(idx, idx2 - idx + 1);
                str = str.Substring(idx2 + 1);
                string result = "";
                string[] arrStr = Utility.Split(str, "/");
                for (int i = 0; i < arrStr.Length; i++)
                {
                    result += arrStr[i];
                    if (i < arrStr.Length - 1)
                        result += Environment.NewLine;
                }
                return result;
                //return bar_name+"|"+result;
            }
        }




        private List<string> GetCGText2(string xmlString)
        {
            string pattern = "<mosAbstract>(.*?)</mosAbstract>";
            MatchCollection lst = Regex.Matches(xmlString, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            List<string> str = new List<string>();
            if (lst.Count == 0)
                return str;
            foreach (Match s in lst)
            {
                string result = s.ToString();
                //B1. Ta lay duoc dinh dang (Ten plugin cua orad) Gia tri text
                result = Regex.Replace(result, pattern, "$1", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                //B2. Ta chia 1 dong/2 dong dua vao <GIA TRI TEXT> co dau ( ;
                //phan biet 1/2 dong bang dau /
                result = GetTextinLines2(result);
                //B3: Lay duoc CG text
                str.Add(result);
            }
            return str;
        }

        List<string> barNames = new List<string>();
        private string GetTextinLines2(string strText)
        {
            string str = strText.Trim();
            int idx = str.IndexOf("(");
            //Neu ko co dau ( tuc la hieu ung OUT
            if (idx < 0)
                return "";
            else //Nguoc lai
            {
                //Cat tien to <TEN_THANH_BAR> dang (xxx)
                int idx2 = str.IndexOf(")");

                string bar_name = str.Substring(idx+1, idx2 - idx-1);
                if (!barNames.Contains(bar_name))
                    barNames.Add(bar_name);
                str = str.Substring(idx2 + 1);
                string result = "";
                string[] arrStr = Utility.Split(str, "/");
                for (int i = 0; i < arrStr.Length; i++)
                {
                    result += arrStr[i];
                    if (i < arrStr.Length - 1)
                        result += Environment.NewLine;
                }
                //return result;
                return bar_name+"|"+result;
            }
        }
       
        private string GetCategoryCG(string prex, string txtText)
        {
            return "";
        }
        public void Get3Element(DataTable tbl, ref string ruttit, ref string news_content, ref string doc_content, ref string new_title, ref string strSub, ref string strLocal, ref string strLogo)
        {
            if (tbl == null || tbl.Rows.Count == 0)
                return;
            string titleGET_NEWS = "THANH_BAR";
            string strEx = "";
            int idx = 0;
            int pd = 0;
            foreach (DataRow r in tbl.Rows)
            {
                try
                {
                    if (r["title"] == null || string.IsNullOrEmpty(r["title"].ToString()))
                        continue;
                    string title = r["title"].ToString().Trim();
                    string ctx = r["OradPlugins"].ToString().Trim();
                    string pageNumber = r["page-number"].ToString().Trim();
                    //Loc ra duoc cac sau co dang TEN_THANH_BAR|Gia tri text ...
                    List<string> textCG = GetCGText2(ctx);
                    if (textCG.Count == 0)
                        continue;
                    //Code moi 08072017

                    string cg = GetCGOfStory(textCG);
                    //add pageNumber
                    if(cg.IndexOf("[]")>=0)
                    {
                        cg = cg.Replace("[]", "[" + pageNumber+"]");
                    }
                    strEx += cg;
                    strEx += Environment.NewLine;
                    string dd = GetLocaltionOfStory(textCG, idx);
                    strLocal += dd;
                    strSub += GetPDOfStory(textCG, pd).Trim();
                    if (cg.Length > 15)
                        pd++;
                    strSub += Environment.NewLine;
                    news_content += GetNewsContentOfStory(textCG);
                    news_content += Environment.NewLine;
                    //logo path
                    strLogo+= GetLogoOfStory(textCG);
                    //doc_content += GetLogoOfStory(textCG);
                    //Code cu
                    //if (string.IsNullOrEmpty(new_title) && !string.IsNullOrEmpty(title))
                    //{
                    //    //if (new_title != title && title.IndexOf(titleGET_NEWS) == -1 && title.IndexOf(titleGET_DOC) == -1)
                    //    if (new_title != title)
                    //    {
                    //        string cg = GetCGOfStory(textCG);
                    //        strEx += cg;
                    //        strEx += Environment.NewLine;
                    //        string dd = GetLocaltionOfStory(textCG, idx);
                    //        strLocal += dd;
                    //        strSub += GetPDOfStory(textCG, pd).Trim();
                    //        if (cg.Length > 15)
                    //            pd++;
                    //        strSub += Environment.NewLine;
                    //    }
                    //    else
                    //    {
                    //        if (title.IndexOf(titleGET_NEWS) >= 0 || new_title == title)
                    //        {
                    //            news_content = ctx.Replace("\n", Environment.NewLine);
                    //            new_title = title;
                    //        }
                    //        else
                    //            doc_content = ctx.Replace("\n", Environment.NewLine);
                    //    }
                    //}
                    //else
                    //{
                    //    if (new_title == title)
                    //    {
                    //        news_content = ctx.Replace("\n", Environment.NewLine);
                    //        new_title = title;
                    //        break;
                    //    }
                    //}

                    string[] nDD = strLocal.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    string tem = "";
                    for (int i = 0; i < nDD.Length; i++)
                    {
                        if (nDD[i].Length > 3)
                            tem += nDD[i] + Environment.NewLine;
                    }
                    strLocal = tem;
                }
                catch
                {

                }
                idx++;
            }
            ruttit = strEx;
        }

        private string GetLogoOfStory(List<string> textCG)
        {
            string pattern = "LOGO_IN";
            foreach (string txt in textCG)
            {
                if (txt.Length < 1)
                    continue;
                string[] temp = Utility.Split(txt, "|");
                if (temp.Length < 1)
                    continue;
                if (temp[0].IndexOf(pattern) >= 0)
                    return temp[1].Replace(Environment.NewLine, "\\");                
            }
            return "";
            
        }

        private string GetNewsContentOfStory(List<string> textCG)
        {
            string pattern =CurrentBarType.LogoEXP;// "Ending_Tittle_IN_NEWS;Ending_Tittle_VALUE_Thieng_ANH";
            string[] lstPatern = Utility.Split(pattern, ";");
            if (lstPatern.Length < 1)
                return "";
            string str = "";
            foreach (string txt in textCG)
            {
                if (txt.Length < 1)
                    continue;
                string[] temp = Utility.Split(txt, "|");
                if (temp.Length < 1)
                    continue;
                bool isExitBar = false;
                for (int i = 0; i < lstPatern.Length; i++)
                {
                    string barname = lstPatern[i].Trim();
                    if (barname == temp[0])
                    {
                        isExitBar = true;
                        break;
                    }
                }
                if (isExitBar == false)
                    return "";
                str += temp[1];
            }
            return str;
         }

        private string GetPDOfStory(List<string> txt, int idx)
        {
            string pattern =  CurrentBarType.LogoEXP; ;// System.Configuration.ConfigurationManager.AppSettings["BarLocaltion"];
            return GetDataOfStory(txt, pattern);
        }

       
        private string GetCGOfStory(List<string> txt)
        {
            //Mac dinh 2 thanh bar dau tien phai la thanh bar chu IN HOA
            //Va la thanh bar chinh dong vai tro la TITLE
            string pattern = CurrentBarType.BarNameEXP;// System.Configuration.ConfigurationManager.AppSettings["BarScene"];// "IN_BAR_VALUE1;IN_BAR_VALUE2;IN_BAR_Value_1;IN_BAR_Value_2;Bar_IN_VALUE_1_LINE_REV;Bar_IN_VALUE_2_LINE_REV";
            return GetDataOfStory(txt, pattern);
        }

        public string GetLocaltionOfStory(List<string> textCG, int idx)
        {
            string pattern = CurrentBarType.LocaltionEXP;// System.Configuration.ConfigurationManager.AppSettings["BarScene"];// "IN_BAR_VALUE1;IN_BAR_VALUE2;IN_BAR_Value_1;IN_BAR_Value_2;Bar_IN_VALUE_1_LINE_REV;Bar_IN_VALUE_2_LINE_REV";
            foreach (string txt in textCG)
            {
                if (txt.Length < 1)
                    continue;
                string[] temp = Utility.Split(txt, "|");
                if (temp.Length < 1)
                    continue;
                if (temp[0].IndexOf(pattern) >= 0)
                    return temp[1].Replace(Environment.NewLine, "\\");
            }
            return "";

        }
        public string GetDataOfStory(List<string> txtCG, string pattern)
        {
            string[] lstPatern = Utility.Split(pattern, ";");
            if (lstPatern.Length <1)
                return "";
            
                
            List<string> header = new List<string>();
            List<string> footer = new List<string>();
            foreach (string txt in txtCG)
            {
                if (txt.Length < 1)
                    continue;
                string[] temp = Utility.Split(txt, "|");
                if (temp.Length < 1)
                    continue;
                bool isExitBar = false;
                for (int i = 0; i < lstPatern.Length; i++)
                {
                    string barname = lstPatern[i].Trim();
                    if (barname == temp[0])
                    {
                        isExitBar = true;
                        break;
                    }
                }
                if (isExitBar == false)
                    return "";
                //C2 Cu thang nao toan bo la chu hoa thi la header
                bool isUpper = true;
                if (temp[1].Length > 5)
                {
                    foreach (char c in temp[1])
                        if (Char.IsLower(c) && Char.IsLetter(c))
                        {
                            isUpper = false;
                            break;
                        }
                }
                if (isUpper)
                    header.Add(temp[1]);
                else
                    footer.Add(temp[1]);
                //------------------------------------------------


                //C1 Doan nay dua vao thanh bar de biet dau la header, dau la footer
                //for (int i = 0; i < lstPatern.Length; i++)
                //{
                //    string barname = lstPatern[i].Trim();
                //    if (barname == temp[0])//neu co thanh bar
                //    {
                //        if (i <= 1)
                //            header.Add(temp[1]);
                //        else
                //            footer.Add(temp[1]);
                //        break;
                //    }
                //    else
                //        continue;

                //}       

            }


            //xu ly header neu co chua co footer
            string str = "";
            if (header.Count ==0 && footer.Count == 0)
                return "";
            str = "[]"+ Environment.NewLine;
            foreach (string s in footer)
            {
                str += s;
                str += Environment.NewLine + Environment.NewLine;
            }
            //txt to           
            str += "#" + Environment.NewLine;
            //Truong hop header co footer
            foreach (string s in header)
            {
                str += s;
                str += Environment.NewLine + Environment.NewLine;
            }            
            return str;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            DataTable tbl = (DataTable)dataGridView1.DataSource;
            string news_content = "";
            string doc_content = "";
            string ruttit = "";
            string new_title = "";
            string diadanh = "";
            string phude = "";
            string logo = "";
            Get3Element(tbl, ref ruttit, ref news_content, ref doc_content, ref new_title, ref phude, ref diadanh, ref logo);
            textBox2.Text = logo;
            textBox1.Text = ruttit;
            comboBox1.Items.Clear();
            foreach (var s in barNames)
                comboBox1.Items.Add(s);
            
            //
        }

        private void SaveBarType(TreeNode selectedNode, string logo, string text)
        {
            BarTypeCollection col = new BarTypeCollection();
            col.AddBarType(selectedNode.Text, logo, text);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentBarType = barCollection.GetBar(comboBox2.SelectedValue.ToString());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length>0)
            {
                BarTypeCollection barTypes = new BarTypeCollection();
                foreach (BarType t in barTypes.BarTypes)
                    if (t.On.IndexOf(comboBox2.SelectedValue.ToString()) >= 0)
                    {
                        MessageBox.Show("Da ton tai.Ko luu");
                        return;
                    }
                SaveBarType(treeView1.SelectedNode, textBox2.Text, comboBox2.SelectedValue.ToString());
                MessageBox.Show("Da luu dc thanh bar moi");
            }
                
           

        }
    }
}
