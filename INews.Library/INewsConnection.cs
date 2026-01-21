using System;
using System.Net;
// Đã xóa using API_iNews.INEWSSystem; -> Gây xung đột namespace
// Đã xóa using API_iNews.INEWSQueue;  -> Gây xung đột namespace

namespace API_iNews
{
    public class INewsConnection : IDisposable
    {
        private readonly INewsConfig _config;
        
        // Dùng Fully Qualified Name: Namespace.Class
        private API_iNews.INEWSSystem.INEWSSystem _systemService;
        private API_iNews.INEWSQueue.INEWSQueue _queueService;
        private CookieContainer _cookieContainer;
        private bool _isConnected;

        public string LastError { get; private set; }
        
        // Expose property
        public API_iNews.INEWSQueue.INEWSQueue QueueService => _queueService;
        public API_iNews.INEWSSystem.INEWSSystem SystemService => _systemService;

        public INewsConnection(INewsConfig config)
        {
            _config = config;
            InitializeWebServices();
        }

        private void InitializeWebServices()
        {
            _cookieContainer = new CookieContainer();
            
            // System Service
            _systemService = new API_iNews.INEWSSystem.INEWSSystem();
            _systemService.Url = "http://192.88.8.230:8080/inewswebservice/services/inewssystem";
            _systemService.CookieContainer = _cookieContainer;
            _systemService.Timeout = _config.Timeout;

            // Queue Service
            _queueService = new API_iNews.INEWSQueue.INEWSQueue();
            _queueService.Url = "http://192.88.8.230:8080/inewswebservice/services/inewsqueue";
            _queueService.CookieContainer = _cookieContainer;
            _queueService.Timeout = _config.Timeout;
        }

        public bool Connect()
        {
            if (_isConnected) return true;
            if (TryConnectSingle(_config.Server)) return true;
            if (!string.IsNullOrEmpty(_config.BackupServer)) return TryConnectSingle(_config.BackupServer);
            return false;
        }

        private bool TryConnectSingle(string serverAddress)
        {
            try
            {
                // ConnectType nằm trong namespace API_iNews.INEWSSystem
                var connectReq = new API_iNews.INEWSSystem.ConnectType
                {
                    Servername = serverAddress,
                    Username = _config.Username,
                    Password = _config.Password
                };

                _systemService.Connect(connectReq);

                // IsConnectedType cũng nằm trong namespace đó
                var status = _systemService.IsConnected(new API_iNews.INEWSSystem.IsConnectedType());
                if (status.IsConnected)
                {
                    _isConnected = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                LastError = $"Lỗi kết nối ({serverAddress}): {ex.Message}";
            }
            return false;
        }

        public void Disconnect()
        {
            if (_isConnected)
            {
                try { _systemService.Disconnect(new API_iNews.INEWSSystem.DisconnectType()); } catch { }
                _isConnected = false;
            }
        }

        public void Dispose()
        {
            Disconnect();
            _systemService?.Dispose();
            _queueService?.Dispose();
        }
    }
}