using HackerRankProxy.App.Storage;

namespace HackerRankProxy.Tests
{
    public class StorageContainerTests
    {
        private IStorageContainer Storage { get; set; }

        public StorageContainerTests()
        {
            Storage = new StorageContainer();
        }

        [Fact]
        public void StorageContentIsNotNull()
        {
            Assert.NotNull(Storage.GetStories(10));
        }

        [Fact]
        public void UpdateBestStoriesSetsNewListAndGetReturnsNewValues()
        {
            Storage.UpdateBestStories(new List<App.Models.Story> { new App.Models.Story() });

            var result = Storage.GetStories(10);

            Assert.NotNull(result);
            Assert.Single(result);
        }
    }
}
