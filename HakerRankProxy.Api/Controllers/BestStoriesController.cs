using HakerRankProxy.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace HakerRankProxy.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BestStoriesController : ControllerBase
    {
        private ILogger<BestStoriesController> Logger { get; }
        private App.Storage.IStorageContainer StorageContainer { get; }

        public BestStoriesController(ILogger<BestStoriesController> logger, App.Storage.IStorageContainer storageContainer)
        {
            Logger = logger;
            StorageContainer = storageContainer;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Story>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult Get([FromQuery] int? nBestStories = null)
        {
            if (nBestStories.HasValue)
            {
                if (nBestStories.Value <= 0 || nBestStories.Value > 200)
                {
                    return BadRequest($"{nameof(nBestStories)} has to be between 1 and 200");
                }
            }
            else
            {
                nBestStories = int.MaxValue;
            }

            var stories = StorageContainer.GetStories(nBestStories.Value);

            return Ok(stories);
        }
    }
}
