using HackerRankProxy.Api.Controllers;
using HackerRankProxy.App.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace HackerRankProxy.Tests
{
    public class BestStoriesControllerTests
    {
        private BestStoriesController Controller { get; set; }

        private ILogger<BestStoriesController> Logger { get; set; }
        private IStorageContainer StorageContainer { get; set; }

        public BestStoriesControllerTests()
        {
            Logger = Substitute.For<ILogger<BestStoriesController>>();
            StorageContainer = Substitute.For<IStorageContainer>();

            Controller = new BestStoriesController(Logger, StorageContainer);
        }

        [Theory]
        [InlineData(-1, 1, 400)]
        [InlineData(0, 0, 400)]
        [InlineData(0, 201, 400)]
        [InlineData(0, 1000000, 400)]
        [InlineData(-1000, 1, 400)]
        [InlineData(-1000, 1000, 400)]
        [InlineData(1, 1, 200)]
        [InlineData(0, 1, 200)]
        [InlineData(0, 100, 200)]
        [InlineData(0, 101, 400)]
        public void GetPaginatedReturnsResponseForInputParameters(int pageNubmer, int pageSize, int responseCode)
        {
            var result = Controller.GetPaginated(pageNubmer, pageSize);
            if (responseCode == 400)
            {
                Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            }
            else if (responseCode == 200)
            {
                Assert.IsAssignableFrom<OkObjectResult>(result);
            }
            else
            {
                Assert.Fail("Unexpected case");
            }
        }

        [Theory]
        [InlineData(0, 400)]
        [InlineData(1, 200)]
        [InlineData(-1, 400)]
        [InlineData(200, 200)]
        [InlineData(201, 400)]
        public void GetReturnsResponseForInputParameters(int nBestStories, int responseCode)
        {
            var result = Controller.Get(nBestStories);
            if (responseCode == 400)
            {
                Assert.IsAssignableFrom<BadRequestObjectResult>(result);
                StorageContainer.Received(0).GetStories(Arg.Any<int>());
            }
            else if (responseCode == 200)
            {
                Assert.IsAssignableFrom<OkObjectResult>(result);
                StorageContainer.Received(1).GetStories(Arg.Any<int>());
            }
            else
            {
                Assert.Fail("Unexpected case");
            }
        }
    }
}