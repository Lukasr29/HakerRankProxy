using HakerRankProxy.App.Models;
using HakerRankProxy.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HakerRankProxy.App.Background
{
    internal class HackerRankBackgroundUpdater : BackgroundService
    {
        private EndpointsConfiguration EndpointsConfiguration { get; }
        private IServiceProvider ServiceProvider { get; }
        private ILogger<HackerRankBackgroundUpdater> Logger { get; }

        public HackerRankBackgroundUpdater(IOptions<EndpointsConfiguration> endpointOptions, IServiceProvider serviceProvider, ILogger<HackerRankBackgroundUpdater> logger)
        {
            EndpointsConfiguration = endpointOptions?.Value ?? throw new ArgumentException("Could not map endpoints configuration", nameof(endpointOptions));
            ServiceProvider = serviceProvider ?? throw new ArgumentException(nameof(serviceProvider));
            Logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = ServiceProvider.CreateScope();
                    var hackerRankUpdater = scope.ServiceProvider.GetRequiredService<IHackerRankDataUpdater>();
                    await hackerRankUpdater.Update(stoppingToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error during data update");
                }

                await Task.Delay(EndpointsConfiguration.UpdateInterval, stoppingToken);
            }
        }
    }
}
