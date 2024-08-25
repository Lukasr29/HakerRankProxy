using HackerRankProxy.App.Models;
using HackerRankProxy.App.Services;
using HackerRankProxy.App.Storage;
using HackerRankProxy.App.Wrapper;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace HackerRankProxy.Tests
{

    public class HackerRankDataUpdaterTests
    {
        private HackerRankDataUpdater HackerRankDataUpdater { get; }

        private IHackerRankHttpWrapper Wrapper { get; }
        private ILogger<HackerRankDataUpdater> Logger { get; }
        private IStorageContainer Storage { get; }

        public HackerRankDataUpdaterTests()
        {
            Wrapper = Substitute.For<IHackerRankHttpWrapper>();
            Logger = Substitute.For<ILogger<HackerRankDataUpdater>>();
            Storage = Substitute.For<IStorageContainer>();

            HackerRankDataUpdater = new HackerRankDataUpdater(Wrapper, Logger, Storage);
        }

        [Fact]
        public async Task WhenThereAreNoChangesInApiUpateIsNotCalled()
        {
            var ct = CancellationToken.None;

            Wrapper.HasPendingBestStoriesUpdates(Arg.Any<CancellationToken>()).Returns(false);

            await HackerRankDataUpdater.Update(ct);

            await Wrapper.DidNotReceive().GetStoryDetails(Arg.Any<int>(), Arg.Any<CancellationToken>());
            Storage.DidNotReceive().UpdateBestStories(Arg.Any<List<Story>>());
        }

        [Fact]
        public async Task WhenThereAreChangesInApiUpateIsNotCalled()
        {
            var ct = CancellationToken.None;

            Wrapper.HasPendingBestStoriesUpdates(Arg.Any<CancellationToken>()).Returns(true);
            Wrapper.GetBestStories(Arg.Any<CancellationToken>()).Returns(new int[] { 1, 2, 3 });

            await HackerRankDataUpdater.Update(ct);

            await Wrapper.Received(3).GetStoryDetails(Arg.Any<int>(), Arg.Any<CancellationToken>());
            Storage.Received(1).UpdateBestStories(Arg.Any<List<Story>>());
        }
    }
}
