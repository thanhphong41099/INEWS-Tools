using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace INews.BridgeHost
{
    public class BridgeServer
    {
        private readonly TcpListener _listener;
        private readonly Thread _listenThread;
        private readonly API_iNews.INewsDataProvider _dataProvider;
        private readonly string _fieldMapping;
        private readonly string _queuesRoot;
        private readonly object _syncRoot = new object();
        private bool _isStopping;

        public BridgeServer(IPAddress ipAddress, int port, API_iNews.INewsDataProvider dataProvider)
        {
            _listener = new TcpListener(ipAddress, port);
            _dataProvider = dataProvider;
            _fieldMapping = ConfigurationManager.AppSettings["Fields"] ?? "title,page-number";
            _queuesRoot = ConfigurationManager.AppSettings["QueuesRoot"] ?? string.Empty;
            _listenThread = new Thread(ListenLoop)
            {
                IsBackground = true,
                Name = "INEWS Bridge Host"
            };
        }

        public void Start()
        {
            _listener.Start();
            _listenThread.Start();
        }

        public void Stop()
        {
            lock (_syncRoot)
            {
                if (_isStopping) return;
                _isStopping = true;
            }

            _listener.Stop();
            if (Thread.CurrentThread != _listenThread)
            {
                _listenThread.Join(TimeSpan.FromSeconds(2));
            }
        }

        private void ListenLoop()
        {
            try
            {
                while (true)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    using (client)
                    using (NetworkStream stream = client.GetStream())
                    {
                        string cmd = ReadCommand(stream);
                        if (string.IsNullOrWhiteSpace(cmd)) continue;

                        string response = HandleCommand(cmd.Trim());
                        if (!string.IsNullOrEmpty(response))
                        {
                            byte[] payload = Encoding.UTF8.GetBytes(response);
                            client.ReceiveBufferSize = payload.Length;
                            stream.Write(payload, 0, payload.Length);
                        }
                    }
                }
            }
            catch (SocketException)
            {
                if (!_isStopping)
                {
                    throw;
                }
            }
        }

        private static string ReadCommand(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead <= 0) return string.Empty;
            return Encoding.UTF8.GetString(buffer, 0, bytesRead).Replace("\0", string.Empty);
        }

        private string HandleCommand(string cmd)
        {
            if (string.Equals(cmd, "PING", StringComparison.OrdinalIgnoreCase))
            {
                return "OK";
            }

            if (string.Equals(cmd, "GET_TREE", StringComparison.OrdinalIgnoreCase))
            {
                return SerializeArrayToString(BuildQueueTree());
            }

            if (string.Equals(cmd, "GET_SCROLL_TEXT", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            return SerializeStories(cmd);
        }

        private string SerializeStories(string cmd)
        {
            string[] parts = cmd.Split(new[] { "|", "#" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return "ERROR:INVALID_COMMAND";
            }

            try
            {
                string queueName = parts[1];
                DataTable table = _dataProvider.GetStoriesAsDataTable(queueName, _fieldMapping);
                return SerializeTableToString(table);
            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message;
            }
        }

        private List<string> BuildQueueTree()
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(_queuesRoot)) return result;

            string[] roots = _queuesRoot.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string root in roots)
            {
                string trimmed = root.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                result.Add("{" + trimmed + ":");
                List<string> children = _dataProvider.GetFolderChildren(trimmed);
                foreach (string child in children)
                {
                    result.Add(child);
                }
                result.Add("}");
            }

            return result;
        }

        private static string SerializeArrayToString(List<string> items)
        {
            if (items == null || items.Count == 0)
            {
                return string.Empty;
            }

            return string.Join("|", items) + "|";
        }

        private static string SerializeTableToString(DataTable table)
        {
            if (table == null)
            {
                table = new DataTable();
            }

            using (var sw = new StringWriter())
            using (var xmlWriter = new XmlTextWriter(sw))
            {
                table.TableName = "Stories";
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("data");
                ((System.Xml.Serialization.IXmlSerializable)table).WriteXml(xmlWriter);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
                sw.Flush();
                return sw.ToString();
            }
        }
    }
}
