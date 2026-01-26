using System;
using System.Configuration;
using System.Net;

namespace INews.BridgeHost
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("INEWS Bridge Host (NET Framework 4.8)");

            var config = LoadConfig();
            var connection = new API_iNews.INewsConnection(config);
            var provider = new API_iNews.INewsDataProvider(connection);

            string ipSetting = ConfigurationManager.AppSettings["BridgeHostIP"] ?? "127.0.0.1";
            string portSetting = ConfigurationManager.AppSettings["BridgeHostPort"] ?? "3000";

            if (!IPAddress.TryParse(ipSetting, out IPAddress ipAddress))
            {
                Console.WriteLine($"Invalid BridgeHostIP '{ipSetting}'.");
                return 1;
            }

            if (!int.TryParse(portSetting, out int port))
            {
                Console.WriteLine($"Invalid BridgeHostPort '{portSetting}'.");
                return 1;
            }

            var server = new BridgeServer(ipAddress, port, provider);
            server.Start();

            Console.WriteLine($"Listening on {ipAddress}:{port}");
            Console.WriteLine("Commands: GET_TREE | QUEUE|<queue-fullname> | PING");
            Console.WriteLine("Press ENTER to stop.");

            Console.ReadLine();
            server.Stop();
            connection.Dispose();

            return 0;
        }

        private static API_iNews.INewsConfig LoadConfig()
        {
            return new API_iNews.INewsConfig
            {
                Server = ConfigurationManager.AppSettings["iNewsServer"] ?? "192.88.8.21",
                BackupServer = ConfigurationManager.AppSettings["iNewsServerBackup"] ?? "",
                Username = ConfigurationManager.AppSettings["iNewsUser"] ?? "",
                Password = ConfigurationManager.AppSettings["iNewsPass"] ?? "",
                Timeout = ParseIntSetting("iNewsTimeout", 5000),
                FieldMapping = ConfigurationManager.AppSettings["Fields"] ?? "title,page-number"
            };
        }

        private static int ParseIntSetting(string key, int defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            return int.TryParse(value, out int parsed) ? parsed : defaultValue;
        }
    }
}
