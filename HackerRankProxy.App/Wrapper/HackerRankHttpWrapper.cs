using HackerRankProxy.App.Clients;
using HackerRankProxy.App.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace HackerRankProxy.App.Wrapper
{
    internal interface IHackerRankHttpWrapper
    {
        Task<int[]> GetBestStories(CancellationToken cancellationToken);
        Task<Story?> GetStoryDetails(int storyId, CancellationToken cancellationToken);
        Task<bool> HasPendingBestStoriesUpdates(CancellationToken cancellationToken);
    }

    internal class HackerRankHttpWrapper : IHackerRankHttpWrapper
    {
        private const string CacheKey = $"{nameof(HackerRankHttpClient)}.{nameof(HasPendingBestStoriesUpdates)}";

        private HackerRankHttpClient HttpClient { get; }
        private IDistributedCache Cache { get; }

        public HackerRankHttpWrapper(HackerRankHttpClient httpClient, IDistributedCache cache)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public Task<int[]> GetBestStories(CancellationToken cancellationToken) => HttpClient.GetBestStories(cancellationToken);

        public Task<Story?> GetStoryDetails(int storyId, CancellationToken cancellationToken) => HttpClient.GetStoryDetails(storyId, cancellationToken);

        public async Task<bool> HasPendingBestStoriesUpdates(CancellationToken cancellationToken)
        {
            var rawResponse = await HttpClient.GetBestStoriesRaw(cancellationToken);
            var cachedResponse = await Cache.GetStringAsync(CacheKey, cancellationToken);

            await Cache.SetStringAsync(CacheKey, rawResponse, cancellationToken);

            return !string.Equals(rawResponse, cachedResponse, StringComparison.OrdinalIgnoreCase);
        }
    }
}
