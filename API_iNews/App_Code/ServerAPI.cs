using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TTDH
{
    public class ServerAPI
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        iNewsData iData = null;
        private string ErrorMessage = "";
        public delegate void RecieveEvent(string msg);
        public event RecieveEvent Recieve;
        WebServiceSettings settings = new WebServiceSettings(); 
        public ServerAPI(string ip)
        {
          IPAddress localAddr = IPAddress.Parse("127.0.0.1");
          if(!string.IsNullOrEmpty(ip))
              localAddr = IPAddress.Parse(ip);
          this.tcpListener = new TcpListener(localAddr, 3000);
          settings.ServerBackup = System.Configuration.ConfigurationManager.AppSettings["iNewsServerBackup"];
          settings.ServerName = System.Configuration.ConfigurationManager.AppSettings["iNewsServer"];
          settings.UserName = System.Configuration.ConfigurationManager.AppSettings["iNewsUser"];
          settings.Password = System.Configuration.ConfigurationManager.AppSettings["iNewsPass"];
          iData = new iNewsData(settings);
          iData.SentError += new iNewsData.SendError(iData_SentError);        
        }
        public void Start()
        {
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Name = "Server GetData iNews";
            this.listenThread.IsBackground = true;
            listenThread.IsBackground = true;
            this.listenThread.Start();
        }
        public void Stop()
        {
            try
            {
                iData.DisconnectAsync();
                if (tcpListener != null)
                {
                    tcpListener.Stop();                   
                }
                listenThread.Join();
                listenThread = null;                
            }
            catch
            {
                
            }

        }
        private DataTable TestData()
        {
            string folderName = @"D:\CGExp\21";
            string mapping = System.Configuration.ConfigurationManager.AppSettings["Fields"];
            ProcessingXMl2Class process = new ProcessingXMl2Class();
            process.FieldMapping = mapping;            
            if (System.IO.Directory.Exists(folderName))
            {
                DataTable tbl = process.GetDataRows(folderName);
                return tbl;
            }
            return null;
        }
        private void ListenForClients()
        {
            try
            {
                this.tcpListener.Start();
                String cmd = null;                
                while (true)
                {
                    TcpClient client = this.tcpListener.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    byte[] bytes = new byte[1024];                    
                    // Read the data sent by the client. 
                    stream.Read(bytes, 0, bytes.Length);                                     
                    // Translate data bytes to a ASCII string.
                    cmd = System.Text.Encoding.UTF8.GetString(bytes).Replace("\0", string.Empty);                    
                    if (!string.IsNullOrEmpty(cmd))
                    {
                        string data = "";
                        ErrorMessage = "";
                        switch (cmd)
                        {
                            case "RESTART_SV":
                                data = RestartService();
                                break;
                            case "GET_TREE":
                                data = SerializeArrayToString(GetTree());
                                break;
                            case "GET_SCROLL_TEXT":
                                data = GetScrollText();
                                break;
                            case "TESTDATA":
                                DataTable tbl2 = TestData();
                                if (tbl2 != null)
                                    data = SerializeTableToString(tbl2);
                                break;
                            default:
                                //DataTable tbl = TestData();
                                DataTable tbl = GetStories(cmd);
                                if (tbl != null)
                                    data = SerializeTableToString(tbl);
                                break;
                        }
                        if (ErrorMessage != "")
                            data = ErrorMessage;
                        if (!string.IsNullOrEmpty(data))
                        {
                            Byte[] dataSend = System.Text.Encoding.UTF8.GetBytes(data);
                            // Get a client stream for reading and writing.                             
                            client.ReceiveBufferSize = data.Length;
                            stream = client.GetStream();
                            // Send the message to the connected TcpServer. 
                            stream.Write(dataSend, 0, dataSend.Length);
                           
                        }                       
                        cmd = null;
                    }
                    stream.Close();
                    client.Close();
                }
            }
            catch
            {

            }
            finally
            {                                 
                tcpListener.Stop();
            }
        }

        private string RestartService()
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "restart.bat";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
            proc.Start();
            proc.WaitForExit();
            string exitCode = proc.ExitCode.ToString();
            proc.Close();
            return exitCode;
        }

        private string GetScrollText()
        {
            return string.Empty;
        }

        private DataTable GetStories(string cmd)
        {
            if (string.IsNullOrEmpty(cmd))
                return null;
            string[] abc = cmd.Split(new string[]{"|","#"},StringSplitOptions.RemoveEmptyEntries);
            if (abc.Length != 2)
                return null;
            try
            {
                //string QUEUEROOT = System.Configuration.ConfigurationManager.AppSettings["QueuesRoot"];
                //QUEUEROOT = QUEUEROOT + "." + abc[1];
                string queueName = abc[1];
                List<string> queues = iData.GetStoriesBoard(queueName);
                string mapping = System.Configuration.ConfigurationManager.AppSettings["Fields"];
                ProcessingXMl2Class process = new ProcessingXMl2Class();
                process.FieldMapping = mapping;
                DataTable tbl = process.GetDataRows(queues);
                return tbl;
            }
            catch
            {
                return null;
            }
        }
       
        private List<string> GetTree()
        {
            string QUEUEROOT = System.Configuration.ConfigurationManager.AppSettings["QueuesRoot"];
            string[] parentNote = Utility.Split(QUEUEROOT, ";");
            List<string> queues=new List<string>();
            
            foreach (string rootName in parentNote)
            {
                queues.Add("{"+rootName+":");
                List<string> subQuery = iData.GetFolderChildren(rootName);
                foreach (string s in subQuery)
                {
                    queues.Add(s);
                }
                queues.Add("}");
            }
             
            return queues;
        }
        void iData_SentError(string msg)
        {
            ErrorMessage = "ERROR:"+msg;                     
        }
        //{VO_BAN_TIN:|BAN_TIN_12H|BAN_TIN_14H|}
        //{VO_BAN_TIN:|BAN_TIN_12H|BAN_TIN_14H|}{CHUYEN_MUC:|NGUOI_LAO_DONG|BAN_TIN_14H|}
        private string SerializeArrayToString(List<string> lst)
        {
            
            if (lst == null || lst.Count == 0)
                return "";
            else
            {
                string str = "";
                foreach (string s in lst)
                    str += s + "|";
                return str;

            }
        }
        private string SerializeTableToString( DataTable table)
        {
            if (table == null)
            {
                //return null;
                table = new DataTable();
            }
            //else
            {
                using (var sw = new StringWriter())
                using (var tw = new XmlTextWriter(sw))
                {
                    // Must set name for serialization to succeed.
                    table.TableName = @"Stories";
                    tw.Formatting = Formatting.Indented;
                    tw.WriteStartDocument();
                    tw.WriteStartElement(@"data");
                    ((IXmlSerializable)table).WriteXml(tw);
                    tw.WriteEndElement();
                    tw.WriteEndDocument();                  
                    tw.Flush();
                    tw.Close();
                    sw.Flush();
                    //sw.Close();
                    return sw.ToString();
                }
            }
        }
    }
}
