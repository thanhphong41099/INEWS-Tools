using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News2025.Services
{
    // Interface định nghĩa các hàm tương tác với server iNEWS
    public interface IInewsService
    {
        bool Connect(string username, string password, string servername);
        bool Disconnect();
        bool IsConnected();
        // ... có thể mở rộng
    }

    // Lớp triển khai kết nối thực tế tới Web Service
    public class InewsServiceClient : IInewsService
    {
        private News2025.InewsServiceReference.INEWSSystemService _systemClient;

        public InewsServiceClient()
        {
            _systemClient = new News2025.InewsServiceReference.INEWSSystemService();
        }

        public bool Connect(string username, string password, string servername)
        {
            try
            {
                string result = _systemClient.Connect(username, password, servername);
                return !string.IsNullOrEmpty(result);
            }
            catch
            {
                return false;
            }
        }

        public bool Disconnect()
        {
            try
            {
                _systemClient.Disconnect();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsConnected()
        {
            try
            {
                string extension;
                return _systemClient.IsConnected(out extension);
            }
            catch
            {
                return false;
            }
        }
    }

}
