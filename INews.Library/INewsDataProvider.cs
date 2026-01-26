using System;
using System.Collections.Generic;
using System.Data;
// Không using namespace API_iNews.INEWSQueue để tránh conflict

namespace API_iNews
{
    public class INewsDataProvider
    {
        private readonly INewsConnection _connection;
        public string LastError => _connection.LastError;

        public INewsDataProvider(INewsConnection connection)
        {
            _connection = connection;
        }

        public List<string> GetStoriesXml(string queuePath, int limit = 240)
        {
            var result = new List<string>();
            if (!_connection.Connect()) return result;

            try
            {
                // Sử dụng Fully Qualified Name
                var setQueueReq = new API_iNews.INEWSQueue.SetCurrentQueueType { QueueFullName = queuePath };
                _connection.QueueService.SetCurrentQueue(setQueueReq);

                var getStoriesReq = new API_iNews.INEWSQueue.GetStoriesType
                {
                    IsStoryBodyIncluded = true,
                    NumberOfStoriesToGet = limit,
                    Navigation = API_iNews.INEWSQueue.GetStoriesNavigationEnum.SAME
                };

                var response = _connection.QueueService.GetStories(getStoriesReq);
                if (response?.Stories != null)
                {
                    foreach (var story in response.Stories)
                    {
                        if (!string.IsNullOrEmpty(story.StoryAsNSML)) result.Add(story.StoryAsNSML);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetStories Error: {ex.Message}");
            }
            return result;
        }

        public DataTable GetStoriesAsDataTable(string queuePath, string fieldMapping)
        {
            var xmlList = GetStoriesXml(queuePath);
            return StoryXmlParser.ToDataTable(xmlList, fieldMapping);
        }

        public List<string> GetFolderChildren(string folderPath)
        {
            var result = new List<string>();
            if (!_connection.Connect()) return result;

            try
            {
                var request = new API_iNews.INEWSSystem.GetFolderChildrenType
                {
                    FolderFullName = folderPath
                };

                var response = _connection.SystemService.GetFolderChildren(request);
                if (response?.Children != null)
                {
                    foreach (var child in response.Children)
                    {
                        if (!string.IsNullOrEmpty(child?.Name))
                        {
                            result.Add(child.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetFolderChildren Error: {ex.Message}");
            }

            return result;
        }
    }
}
