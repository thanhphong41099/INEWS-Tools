using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iNews.LegacyWrapper.SoapClients.INEWSSystem;
using iNews.LegacyWrapper.SoapClients.INEWSQueue;
using iNews.LegacyWrapper.SoapClients.INEWSStory;
using System.Net;
using System.Threading.Tasks;

namespace iNews.LegacyWrapper.Legacy
{   

    public class iNews : IDisposable
    {
        private INEWSSystem _svc;
        private INEWSQueue _qsvc;
        private bool IsConnectedToServer = false;
        private string CurrentServer = "";
        //private INEWSStory _stvc;    
        public INEWSQueue iNewsQueue { get { return _qsvc; } }
        public INEWSSystem iNewsSystem { get { return _svc; } }
       // public INEWSStory iNewsStory { get { return _stvc; } }
        public string ErrorMsg { get; set; }
        /// <summary>
        /// Constructor.  Attempts to connect to iNews server based on the WS settings supplied.
        /// </summary>
        /// <param name="settings"></param>
        public iNews(WebServiceSettings settings)
        {
            //Initialize iNews Web Services
            _svc = new INEWSSystem();
            _qsvc = new INEWSQueue();
            //_stvc = new INEWSStory();
            // Cấu hình timeout
            _svc.Timeout = 5000; // 5 giây
            _qsvc.Timeout = 5000; // 5 giây

            //Instantiate Webservice objects
            ConnectResponseType response = new ConnectResponseType();
            ConnectType connect = new ConnectType();

            CookieContainer cookieJar = new CookieContainer();
            _svc.CookieContainer = cookieJar;
            _qsvc.CookieContainer = cookieJar;
            //_stvc.CookieContainer = cookieJar;
            //Set connection properties for iNews Web Service and connect...
            connect.Servername = settings.ServerName;
            connect.Username = settings.UserName;
            connect.Password = settings.Password;
            try
            {
                CurrentServer = settings.ServerName;
                response = _svc.Connect(connect);
                IsConnectedToServer = true;
                
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;

                if (ex.Message == "Unable to connect to the remote server" && !string.IsNullOrEmpty(settings.ServerBackup))
                {
                    IsConnectedToServer = false;
                    connect.Servername = settings.ServerBackup;
                    try
                    {
                        CurrentServer = settings.ServerBackup;
                        response = _svc.Connect(connect);
                        IsConnectedToServer = true;                        
                    }
                    catch (Exception ex2)
                    {
                        ErrorMsg = "Không kết nối được với server Inews (Unable to connect to the remote server  " + settings.ServerName + " and  " + settings.ServerBackup + ")";
                        //ErrorMsg = "Unable to connect to the remote server " + ex2.Message +": "+ settings.ServerBackup;
                        IsConnectedToServer = false;
                    }
                }
                else
                {
                    ErrorMsg = "Không kết nối được với server Inews (Unable to connect to the remote server  " + settings.ServerName + ")";
                }
                  
            }            
        }

        /// <summary>
        /// Informs caller if we are connected to iNews
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            try
            {
                if (_svc.IsConnected(new IsConnectedType()).IsConnected)
                {
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                //ErrorMsg = ex.Message;
                ErrorMsg = "Không kết nối được với server Inews (Unable to connect to the remote server  " + CurrentServer + ")";
                return false;
            }
        }

        public void Dispose()
        {
            if (IsConnectedToServer) // Chỉ gọi Disconnect nếu đã kết nối thành công
            {
                try
                {
                    if (IsConnected())
                    {
                        _svc.Disconnect(new DisconnectType());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during Disconnect: {ex.Message}");
                }
            }
        }

    }

    public class iNewsData
    {
        public delegate void SendError(string msg);
        public event SendError SentError;
        private WebServiceSettings settings;       
        public iNewsData()
        {
            
        }
        public iNewsData(WebServiceSettings settings)
        {
            this.settings = settings;
        }
        
        public List<string> ChangedQueues()
        {
            List<string> queues = new List<string>();
            try
            {
                using (iNews inews = new iNews(settings))
                {
                    if (inews.IsConnected())
                    {
                        GetChangedQueuesType changedQueues = new GetChangedQueuesType();
                        GetChangedQueuesResponseType changedQueuesResponse = new GetChangedQueuesResponseType();
                        changedQueuesResponse = inews.iNewsSystem.GetChangedQueues(changedQueues);
                        if (changedQueuesResponse != null && changedQueuesResponse.ChangedQueueNames != null)
                        {
                            //If any queue has changed it will be listed in a string array in the changedQueuesResponse
                            foreach (string queue in changedQueuesResponse.ChangedQueueNames)
                            {
                                queues.Add(queue);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(inews.ErrorMsg))
                        GetErrorIniNews(inews.ErrorMsg);
                }
            }
            catch (Exception ex)
            {
                if(ex.InnerException !=null)
                    GetErrorIniNews(ex.Message + "---" + ex.InnerException.Message);
                else
                    GetErrorIniNews(ex.Message);
            }           
            return queues;
        }
        private void GetErrorIniNews(string msg)
        {
            if (SentError != null)
            {
                SentError(msg);
            }
        }      


        public List<string> GetFolderChildren(string folderParent)
        {
            List<string> queues = new List<string>();
            try
            {
                using (iNews inews = new iNews(settings))
                {                    
                    if (inews.IsConnected())
                    {
                        GetFolderChildrenType folderType = new GetFolderChildrenType();
                        folderType.FolderFullName = folderParent;                        
                        GetFolderChildrenResponseType folderTypeResponse = new GetFolderChildrenResponseType();
                        //folderTypeResponse.ParentFolderFullName = folderParent;
                        folderTypeResponse = inews.iNewsSystem.GetFolderChildren(folderType);
                        
                        if (folderTypeResponse != null && folderTypeResponse.Children != null)
                        {
                            //If any queue has changed it will be listed in a string array in the changedQueuesResponse
                            foreach (var f in folderTypeResponse.Children)
                            {
                                queues.Add(f.Name);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(inews.ErrorMsg))
                        GetErrorIniNews(inews.ErrorMsg);

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    GetErrorIniNews(ex.Message + "---" + ex.InnerException.Message);
                else
                    GetErrorIniNews(ex.Message);
            }
            return queues;
        }

        public string  GetQueuesForm(string queueName,bool isQueueForm)
        {
            string queues = "";
            try
            {
                using (iNews inews = new iNews(settings))
                {
                   
                    if (inews.IsConnected())
                    {
                        GetQueuesFormType queueFormType = new GetQueuesFormType();
                        if(isQueueForm)
                            queueFormType.FormType = FormTypeType.Queue;
                        else
                            queueFormType.FormType = FormTypeType.Story;
                        queueFormType.QueueFullName = queueName;
                        GetQueuesFormResponseType queueFormTypeResponse = new GetQueuesFormResponseType();                        
                        queueFormTypeResponse = inews.iNewsSystem.GetQueuesForm(queueFormType);
                        if (queueFormTypeResponse != null && queueFormTypeResponse.Form != null)
                        {
                            return queueFormTypeResponse.Form;
                        }
                    }
                    if (!string.IsNullOrEmpty(inews.ErrorMsg))
                        GetErrorIniNews(inews.ErrorMsg);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    GetErrorIniNews(ex.Message + "---" + ex.InnerException.Message);
                else
                    GetErrorIniNews(ex.Message);

            }
            return queues;
        }

        public List<string> GetStoriesBoard(string queueName)
        {
            List<string> storylist = new List<string>();
            try
            {
                using (iNews inews = new iNews(settings))
                {                        
                    if (inews.IsConnected())
                    {
                        GetStoriesType storyType = new GetStoriesType();
                        storyType.IsStoryBodyIncluded=true;                        
                        storyType.NumberOfStoriesToGet=240;                        
                        storyType.Navigation = GetStoriesNavigationEnum.SAME;
                        GetStoriesResponseType storyTypeResponse = new GetStoriesResponseType();
                        SetCurrentQueueType queueType=new SetCurrentQueueType();
                        queueType.QueueFullName=queueName;                        
                        inews.iNewsQueue.SetCurrentQueue(queueType);
                        storyTypeResponse = inews.iNewsQueue.GetStories(storyType);
                        if (storyTypeResponse != null && storyTypeResponse.Stories != null)
                        {
                            foreach (var f in storyTypeResponse.Stories)
                            {                                
                                string str = f.StoryAsNSML;
                                if(!string.IsNullOrEmpty(str))
                                    storylist.Add(str);
                            }
                        }                        
                    }
                    if (!string.IsNullOrEmpty(inews.ErrorMsg))
                        GetErrorIniNews(inews.ErrorMsg);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    GetErrorIniNews(ex.Message + "---" + ex.InnerException.Message);
                else
                    GetErrorIniNews(ex.Message);
            }
            return storylist;
        }
        public void Disconnect()
        {
            Console.WriteLine("Starting Disconnect...");
            using (iNews inews = new iNews(settings))
            {
                inews.Dispose();
            }
            Console.WriteLine("Disconnect completed.");
        }

        public void DisconnectAsync()
        {
            Task.Run(() =>
            {
                Disconnect();
            });
        }


        public bool IsConnect()
        {
            using (iNews inews = new iNews(settings))
            {
              return  inews.IsConnected();
            }
        }

    }
  
}
