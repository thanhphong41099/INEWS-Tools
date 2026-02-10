using Microsoft.AspNetCore.Mvc;
using iNews.LegacyWrapper.Legacy;
using System.Configuration;

namespace iNews.Gateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoryController : ControllerBase
{
    private readonly ILogger<StoryController> _logger;

    public StoryController(ILogger<StoryController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{queueName}")]
    public IActionResult GetStories(string queueName)
    {
        try
        {
            // Read settings from App.config (via ConfigurationManager which iNewsData uses internally too?)
            // Actually iNewsData constructor takes a settings object.
            var settings = new WebServiceSettings();
            settings.ServerName = System.Configuration.ConfigurationManager.AppSettings["iNewsServer"];
            settings.ServerBackup = System.Configuration.ConfigurationManager.AppSettings["iNewsServerBackup"];
            settings.UserName = System.Configuration.ConfigurationManager.AppSettings["iNewsUser"];
            settings.Password = System.Configuration.ConfigurationManager.AppSettings["iNewsPass"];

            // Instantiate Legacy Logic
            var dataProvider = new iNewsData(settings);
            
            // Call Legacy Method
            // Note: GetStoriesBoard returns List<string> (NSML)
            var stories = dataProvider.GetStoriesBoard(queueName);

            return Ok(stories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching stories");
            return StatusCode(500, new { Error = ex.Message, Stack = ex.StackTrace });
        }
    }
}
