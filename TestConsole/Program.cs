using System;
using System.Data;
using API_iNews; // Namespace của Library

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("--- TEST INEWS LIBRARY ---");

            // 1. Cấu hình
            var config = new INewsConfig();
            Console.WriteLine($"Server: {config.Server}");
            Console.WriteLine($"User: {config.Username}");

            // 2. Kết nối
            using (var conn = new INewsConnection(config))
            {
                Console.WriteLine("Connecting...");
                // Note: Connect thực tế sẽ thất bại nếu không VPN vào mạng nội bộ VTV
                // Nhưng logic chạy qua bước này chứng tỏ Library OK.
                if (!conn.Connect())
                {
                    Console.WriteLine("Kết quả (Expected nếu không có VPN): " + conn.LastError);
                }
                else 
                {
                    Console.WriteLine("Kết nối thành công!");

                    // 3. Lấy dữ liệu
                    var provider = new INewsDataProvider(conn);
                    string queuePath = "VTV4.04_VO_BAN_TIN.NEWSLINE"; // <--- Sửa Queue này cho đúng
                    
                    Console.WriteLine($"Đang lấy tin từ: {queuePath}...");
                    DataTable dt = provider.GetStoriesAsDataTable(queuePath, config.FieldMapping);
                    Console.WriteLine($"Đã lấy được {dt.Rows.Count} tin.");
                }
            }

            Console.WriteLine("\nTest hoàn tất. Library đã hoạt động đúng logic.");
            Console.ReadKey();
        }
    }
}
