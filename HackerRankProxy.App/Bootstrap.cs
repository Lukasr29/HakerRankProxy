using HackerRankProxy.App.Clients;
using HackerRankProxy.App.Models;
using HackerRankProxy.App.Services;
using HackerRankProxy.App.Storage;
using HackerRankProxy.App.Wrapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace HackerRankProxy.App
{
    [ExcludeFromCodeCoverage]
    public static class Bootstrap
    {
        public static void BootstrapHackerRankApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EndpointsConfiguration>(configuration.GetSection(nameof(EndpointsConfiguration)));

            services.AddDistributedMemoryCache();
            services.AddLogging();
            services.AddSingleton<IStorageContainer, StorageContainer>();
            services.AddScoped<IHackerRankDataUpdater, HackerRankDataUpdater>();
            services.AddScoped<IHackerRankHttpWrapper, HackerRankHttpWrapper>();

            services.AddHttpClient<HackerRankHttpClient>((sp, c) =>
            {
                var endpoints = sp.GetRequiredService<IOptions<EndpointsConfiguration>>();
                c.BaseAddress = new Uri(endpoints.Value.HackerRank);
            });

            services.AddHostedService<Background.HackerRankBackgroundUpdater>();
        }

    }
}
