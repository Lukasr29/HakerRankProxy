using HackerRankProxy.App.Models;

namespace HackerRankProxy.App.Storage
{
    public interface IStorageContainer
    {
        void UpdateBestStories(List<Story> stories);
        IEnumerable<Story> GetStories(int pageNumber, int pageSize);
        IEnumerable<Story> GetStories(int nBestStories);
    }

    internal class StorageContainer : IStorageContainer
    {
        private List<Story> Stories { get; set; } = new List<Story>();

        public void UpdateBestStories(List<Story> stories)
        {
            Stories = stories;
        }

        public IEnumerable<Story> GetStories(int pageNumber, int pageSize)
        {
            return Stories
                .OrderByDescending(x => x.Score)
                .Skip(pageNumber * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<Story> GetStories(int nBestStories)
        {
            return GetStories(pageNumber: 0, pageSize: nBestStories);
        }
    }
}
