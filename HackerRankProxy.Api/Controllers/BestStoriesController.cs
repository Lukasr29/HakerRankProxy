using HackerRankProxy.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace HackerRankProxy.Api.Controllers
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

        /// <summary>
        /// Returns best stories from Hacker Rank news
        /// </summary>
        /// <param name="nBestStories">Nubmer of top best stories to return, has to be between 1 and 200. Default: 50</param>
        /// <returns>List of best stories, 200 OK or 400 Bad Request if page number or page size is invalid</returns>
        /// <response code="200">Returns list of best stories</response>
        /// <response code="400">If nBestStories is invalid</response>
        [HttpGet("n-best")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Story>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult Get([FromQuery] int? nBestStories = 50)
        {
            if (!nBestStories.HasValue || nBestStories < 1 || nBestStories > 200)
            {
                return BadRequest($"{nameof(nBestStories)} has to be between 1 and 200");
            }

            Logger.LogInformation("Getting top {BestStories} best stories", nBestStories);

            var stories = StorageContainer.GetStories(nBestStories.Value);

            return Ok(stories);
        }

        /// <summary>
        /// Returns best stories from Hacker Rank news
        /// </summary>
        /// <param name="pageNumber">Page number, has to be greater than or equal to 0. Default: 0 (page indexing starts from 0)</param>
        /// <param name="pageSize">Page size, has to be between 1 and 100. Default: 25</param>
        /// <returns>List of best stories, 200 OK or 400 Bad Request if page number or page size is invalid</returns>
        /// <response code="200">Returns list of best stories</response>
        /// <response code="400">If page number or page size is invalid</response>
        [HttpGet("paginated")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Story>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult GetPaginated([FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 25)
        {
            if (pageNumber < 0)
            {
                return BadRequest($"{nameof(pageNumber)} has to be greater than or equal to 0");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest($"{nameof(pageSize)} has to be between 1 and 100");
            }

            Logger.LogInformation("Getting best stories with page number {PageNumber} and page size {PageSize}", pageNumber, pageSize);

            var stories = StorageContainer.GetStories(pageNumber, pageSize);

            return Ok(stories);
        }
    }
}
