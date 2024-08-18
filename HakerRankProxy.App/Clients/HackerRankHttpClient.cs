using HakerRankProxy.App.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace HakerRankProxy.App.Clients
{
    internal class HackerRankHttpClient
    {
        private const string CacheKey = nameof(HackerRankHttpClient) + nameof(HasPendingBestStoriesUpdates);

        private HttpClient Http { get; }

        private EndpointsConfiguration EndpointsConfiguration { get; }

        private IDistributedCache Cache { get; }

        public HackerRankHttpClient(HttpClient http, IOptions<EndpointsConfiguration> endpointsOptions, IDistributedCache cache)
        {
            EndpointsConfiguration = endpointsOptions?.Value ?? throw new ArgumentException("Could not map endpoints configuration", nameof(endpointsOptions));
            Http = http ?? throw new ArgumentNullException(nameof(http));
            Cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<int[]> GetBestStories(CancellationToken cancellationToken)
        {
            var rawResponse = await this.GetBestStoriesRaw(cancellationToken);
            var result = JsonSerializer.Deserialize<int[]>(rawResponse);
            return result ?? Array.Empty<int>();
        }

        public async Task<bool> HasPendingBestStoriesUpdates(CancellationToken cancellationToken)
        {
            var rawResponse = await this.GetBestStoriesRaw(cancellationToken);
            var cachedResponse = await Cache.GetStringAsync(CacheKey, cancellationToken);

            await Cache.SetStringAsync(CacheKey, rawResponse, cancellationToken);

            return !string.Equals(rawResponse, cachedResponse, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<Story?> GetStoryDetails(int storyId, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(EndpointsConfiguration.GetStoryDetailsEndpoint(storyId), UriKind.Relative)
            };

            using var response = await Http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize<Story>(content);
        }

        private async Task<string> GetBestStoriesRaw(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(EndpointsConfiguration.BestStories, UriKind.Relative)
            };

            using var response = await Http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}
