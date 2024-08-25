using HackerRankProxy.App.Storage;
using HackerRankProxy.App.Wrapper;
using Microsoft.Extensions.Logging;

namespace HackerRankProxy.App.Services
{
    internal interface IHackerRankDataUpdater
    {
        Task Update(CancellationToken cancellationToken);
    }

    internal class HackerRankDataUpdater : IHackerRankDataUpdater
    {
        private IHackerRankHttpWrapper Http { get; }

        private ILogger<HackerRankDataUpdater> Logger { get; }

        private IStorageContainer StorageContainer { get; }

        public HackerRankDataUpdater(
            IHackerRankHttpWrapper http,
            ILogger<HackerRankDataUpdater> logger,
            IStorageContainer storageContainer)
        {
            Http = http ?? throw new ArgumentNullException(nameof(http));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            StorageContainer = storageContainer ?? throw new ArgumentNullException(nameof(storageContainer));
        }

        public async Task Update(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Checking for updates in the best stories");
            var hasUpdates = await Http.HasPendingBestStoriesUpdates(cancellationToken);

            if (hasUpdates)
            {
                Logger.LogInformation("Found updates in the best stories");

                Logger.LogInformation("Getting new best stories ids");
                var newTopList = await Http.GetBestStories(cancellationToken);

                var newListTasks = newTopList
                    .AsParallel()
                    .WithDegreeOfParallelism(16)
                    .Select((storyId) => Http.GetStoryDetails(storyId, cancellationToken))
                    .ToList();

                Logger.LogInformation("Getting new best stories content");
                await Task.WhenAll(newListTasks);
                Logger.LogInformation("All new best stories content retrieved");

                var orderedStories = newListTasks
                    .Select(x => x.Result)
                    .ToList();

                Logger.LogInformation("Updating best stories");
                StorageContainer.UpdateBestStories(orderedStories);
                Logger.LogInformation("Best stories updated");
            }
            else
            {
                Logger.LogInformation("No updates found in the best stories");
            }
        }
    }
}