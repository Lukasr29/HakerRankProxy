using HackerRankProxy.App.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace HackerRankProxy.App.Clients
{
    internal class HackerRankHttpClient 
    {
        private HttpClient Http { get; }

        private EndpointsConfiguration EndpointsConfiguration { get; }

        public HackerRankHttpClient(HttpClient http, IOptions<EndpointsConfiguration> endpointsOptions)
        {
            EndpointsConfiguration = endpointsOptions?.Value ?? throw new ArgumentException("Could not map endpoints configuration", nameof(endpointsOptions));
            Http = http ?? throw new ArgumentNullException(nameof(http));
        }

        public async Task<int[]> GetBestStories(CancellationToken cancellationToken)
        {
            var rawResponse = await this.GetBestStoriesRaw(cancellationToken);
            var result = JsonSerializer.Deserialize<int[]>(rawResponse);
            return result ?? Array.Empty<int>();
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

        internal async Task<string> GetBestStoriesRaw(CancellationToken cancellationToken)
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
