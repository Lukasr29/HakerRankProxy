using HakerRankProxy.App.Clients;
using HakerRankProxy.App.Storage;
using Microsoft.Extensions.Caching.Distributed;

namespace HakerRankProxy.App.Services
{
    interface IHackerRankDataUpdater
    {
        Task Update(CancellationToken cancellationToken);
    }

    internal class HackerRankDataUpdater : IHackerRankDataUpdater
    {
        private HackerRankHttpClient Http { get; }

        private IDistributedCache Cache { get; }

        private IStorageContainer StorageContainer { get; }

        public HackerRankDataUpdater(
            HackerRankHttpClient http,
            IDistributedCache cache,
            IStorageContainer storageContainer)
        {
            Http = http ?? throw new ArgumentNullException(nameof(http));
            Cache = cache ?? throw new ArgumentNullException(nameof(cache));
            StorageContainer = storageContainer ?? throw new ArgumentNullException(nameof(storageContainer));
        }

        public async Task Update(CancellationToken cancellationToken)
        {
            var hasUpdates = await Http.HasPendingBestStoriesUpdates(cancellationToken);

            if (hasUpdates)
            {
                var newTopList = await Http.GetBestStories(cancellationToken);

                var newListTasks = newTopList
                    .AsParallel()
                    .WithDegreeOfParallelism(16)
                    .Select((storyId) => Http.GetStoryDetails(storyId, cancellationToken));

                await Task.WhenAll(newListTasks);

                var orderedStories = newListTasks
                    .Select(x => x.Result)
                    .ToList();

                StorageContainer.UpdateBestStories(orderedStories);
            }
        }
    }
}