using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TTDH; // Dependency on App_Code classes

namespace API_iNews
{
    public interface IINewsService
    {
        /// <summary>
        /// Connects to the iNews server using settings from ConfigurationManager.
        /// </summary>
        Task<bool> ConnectAsync();

        /// <summary>
        /// Disconnects from the iNews server.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Checks if the client is currently connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Gets the last error message from the iNews service.
        /// </summary>
        string LastError { get; }

        /// <summary>
        /// Gets the list of child queues for a given folder/queue.
        /// </summary>
        Task<List<string>> GetQueueChildrenAsync(string parentQueueName);

        /// <summary>
        /// Converts raw story strings into a DataTable using the configured field mapping.
        /// </summary>
        DataTable ProcessStories(List<string> rawStories);

        /// <summary>
        /// Retrieves stories for a queue and returns a DataTable suitable for UI binding (matches InewsForm.dataGridView1).
        /// </summary>
        Task<DataTable> GetStoriesForUiAsync(string queueName);

        /// <summary>
        /// Retrieves the list of raw story strings (XML/NSML).
        /// </summary>
        Task<List<string>> GetRawStoriesAsync(string queueName);

        /// <summary>
        /// Exports raw stories to XML files in the specified destination folder.
        /// </summary>
        void ExportStoriesToXml(List<string> rawStories, string destinationPath);
    }

    public class INewsService : IINewsService
    {
        private iNewsData _iNewsData;
        private WebServiceSettings _settings;
        private bool _isConnected;
        private string _lastError;

        public bool IsConnected => _isConnected; // Simple tracking, real check is internal to iNewsData/iNews
        public string LastError => _lastError;

        public INewsService()
        {
            // Initialize settings from ConfigurationManager
            _settings = new WebServiceSettings
            {
                ServerName = ConfigurationManager.AppSettings["iNewsServer"],
                ServerBackup = ConfigurationManager.AppSettings["iNewsServerBackup"],
                UserName = ConfigurationManager.AppSettings["iNewsUser"],
                Password = ConfigurationManager.AppSettings["iNewsPass"]
            };

            // iNewsData acts as a facade in the existing code, we wrap it here.
            _iNewsData = new iNewsData(_settings);
            _iNewsData.SentError += OnINewsError;
        }

        private void OnINewsError(string msg)
        {
            _lastError = msg;
            // Optionally could log here or fire an event if INewsService had one
        }

        public async Task<bool> ConnectAsync()
        {
            // Reset error state
            _lastError = null;

            return await Task.Run(() =>
            {
                // In the legacy code, iNewsData creates a new connection per call using 'using (iNews inews = ...)'
                // except for the ServerAPI checks.
                // However, there is an `IsConnect()` method in iNewsData that checks connection.
                // We will verify connectivity by trying a lightweight operation or using IsConnect.
                try
                {
                    bool result = _iNewsData.IsConnect();
                    _isConnected = result;
                    return result;
                }
                catch (Exception ex)
                {
                    _lastError = ex.Message;
                    _isConnected = false;
                    return false;
                }
            });
        }

        public void Disconnect()
        {
            try
            {
                // Legacy iNewsData handles disconnect via DisconnectAsync which runs Task.Run internally
                _iNewsData.DisconnectAsync();
            }
            catch { }
            finally
            {
                _isConnected = false;
            }
        }

        public async Task<List<string>> GetQueueChildrenAsync(string parentQueueName)
        {
            return await Task.Run(() =>
            {
                return _iNewsData.GetFolderChildren(parentQueueName);
            });
        }

        public DataTable ProcessStories(List<string> rawStories)
        {
            string mapping = ConfigurationManager.AppSettings["Fields"];
            ProcessingXMl2Class processor = new ProcessingXMl2Class
            {
                FieldMapping = mapping
            };
            return processor.GetDataRows(rawStories);
        }

        public async Task<DataTable> GetStoriesForUiAsync(string queueName)
        {
            return await Task.Run(async () =>
            {
                List<string> rawStories = await GetRawStoriesAsync(queueName);
                return ProcessStories(rawStories);
            });
        }

        public async Task<List<string>> GetRawStoriesAsync(string queueName)
        {
            return await Task.Run(() =>
            {
                return _iNewsData.GetStoriesBoard(queueName);
            });
        }

        public void ExportStoriesToXml(List<string> rawStories, string destinationPath)
        {
            if (rawStories == null || rawStories.Count == 0) return;

            // Ensure directory exists
            if (!Directory.Exists(destinationPath))
            {
                try { Directory.CreateDirectory(destinationPath); } catch { }
            }

            int index = 1;
            foreach (string story in rawStories)
            {
                if (!string.IsNullOrEmpty(story))
                {
                    string fileName = $"story_{index}.xml";
                    string fullPath = Path.Combine(destinationPath, fileName);
                    File.WriteAllText(fullPath, story, Encoding.Unicode);
                }
                index++;
            }
        }
    }
}
