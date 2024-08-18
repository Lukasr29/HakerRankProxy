using HakerRankProxy.App.Models;

namespace HakerRankProxy.App.Storage
{
    public interface IStorageContainer
    {
        void UpdateBestStories(List<Story> stories);
        IEnumerable<Story> GetStories(int nBestStories);
    }

    internal class StorageContainer : IStorageContainer
    {
        private List<Story> Stories { get; set; } = new List<Story>();

        public void UpdateBestStories(List<Story> stories)
        {
            Stories = stories;
        }

        public IEnumerable<Story> GetStories(int nBestStories)
        {
            return Stories
                .OrderByDescending(x => x.Score)
                .Take(nBestStories);
        }
    }
}
