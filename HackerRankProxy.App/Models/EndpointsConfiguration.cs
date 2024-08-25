using System.Diagnostics.CodeAnalysis;

namespace HackerRankProxy.App.Models
{
    [ExcludeFromCodeCoverage]
    internal class EndpointsConfiguration
    {
        public int UpdateInterval { get; set; }
        public string HackerRank { get; set; } = string.Empty;
        public string BestStories { get; set; } = string.Empty;
        public string StoryDetails { get; set; } = string.Empty;

        public string GetStoryDetailsEndpoint(int id) => StoryDetails.Replace("{storyId}", id.ToString());
    }
}
